using System;
using System.Collections.Generic;
using System.Linq;
using dwx14.eventstore;
using ttt.app.adapters;

namespace ttt.app.domain
{
    public class Spiel
    {
        private readonly EventStore _eventStore;

        public Spiel(EventStore eventStore) { _eventStore = eventStore; }


        public Spielstatusse Spieler_feststellen()
        {
            var e = _eventStore.History.LastOrDefault();
            if (e.Name == Spielevents.EVENT_NEUES_SPIEL)
                return e.Payload.IndexOf("XamZug") >= 0 ? Spielstatusse.XamZug : Spielstatusse.OamZug;

            return e.Payload.IndexOf("XamZug") >= 0 ? Spielstatusse.OamZug : Spielstatusse.XamZug;
        }   


        public void Spielstein_setzen(Spielstatusse spieler, int spielfeldindex)
        {
            _eventStore.Append(Spielevents.EVENT_SPIELSTEIN_GESETZT, string.Format("{0},{1}", spieler, spielfeldindex));
        }


        public void Neues_Spiel_beginnen()
        {
            _eventStore.Append(Spielevents.EVENT_NEUES_SPIEL, Spielstatusse.XamZug.ToString());
        }


        public void Zug_validieren(int spielfeldindex, Action validerZug, Action invaliderZug)
        {
            var spielzüge = Spielzüge_des_aktuellen_Spiels_ermittteln();
            if (spielzüge.Any(e => e.Payload.IndexOf(spielfeldindex.ToString()) >= 0))
                invaliderZug();
            else
                validerZug();
        }


        public void Spielende_schon_erreicht(Action beiSpielGehtWeiter, Action<Spielstatusse> beiSpielende)
        {
            var letzterEvent = _eventStore.History.Last();
            if (letzterEvent.Name == Spielevents.EVENT_SPIEL_UNENTSCHIEDEN)
                beiSpielende(Spielstatusse.Unentschieden);
            else if (letzterEvent.Name == Spielevents.EVENT_SPIEL_GEWONNEN)
                beiSpielende((Spielstatusse) Enum.Parse(typeof (Spielstatusse), letzterEvent.Payload));
            else
                beiSpielGehtWeiter();
        }


        public void Prüfe_auf_Spielende(Action beiSpielGehtWeiter, Action<Spielstatusse> beiSpielende)
        {
            Prüfe_auf_Gewinn(
                () => Prüfe_auf_Unentschieden(
                        beiSpielGehtWeiter,
                        () => beiSpielende(Spielstatusse.Unentschieden)),
                beiSpielende);
        }

        private void Prüfe_auf_Unentschieden(Action beiSpielGehtWeiter, Action beiUnentschieden)
        {
            if (Spielzüge_des_aktuellen_Spiels_ermittteln().Count() >= 9) {
                _eventStore.Append(Spielevents.EVENT_SPIEL_UNENTSCHIEDEN, "");
                beiUnentschieden();
            }
            else
                beiSpielGehtWeiter();
        }

        private void Prüfe_auf_Gewinn(Action beiSpielGehtWeiter, Action<Spielstatusse> beiGwinn)
        {
            var spielzüge = Spielzüge_des_aktuellen_Spiels_ermittteln().ToArray();

            Prüfe_auf_Gewinn_für_Spieler(spielzüge, Spielstatusse.XamZug, Spielstatusse.Xgewonnen, 
                () => Prüfe_auf_Gewinn_für_Spieler(spielzüge, Spielstatusse.OamZug, Spielstatusse.Ogewonnen, 
                        beiSpielGehtWeiter, 
                        beiGwinn), 
                beiGwinn);
        }

        private void Prüfe_auf_Gewinn_für_Spieler(IEnumerable<Event> spielzüge, Spielstatusse spieler, Spielstatusse gewinn,
                                                 Action beiSpielGehtWeiter, Action<Spielstatusse> beiGwinn)
        {
            var spielerzüge = spielzüge.Select(e => {
                                            var parts = e.Payload.Split(',');
                                            return new {
                                                    Spieler = (Spielstatusse) Enum.Parse(typeof (Spielstatusse), parts[0]),
                                                    Spielfeldindex = int.Parse(parts[1])
                                                };
                                        })
                                       .Where(e => e.Spieler == spieler)
                                       .Select(e => e.Spielfeldindex)
                                       .ToList();
            spielerzüge.Sort();

            if (new[] {
                    new[] {0, 1, 2}, new[] {3, 4, 5}, new[] {6, 7, 8},
                    new[] {0, 3, 6}, new[] {1, 4, 7}, new[] {2, 5, 8},
                    new[] {0, 4, 8}, new[] {2, 4, 6}
                }.Any(g => ArrayEqual(g, spielerzüge.ToArray()))) {
                _eventStore.Append(Spielevents.EVENT_SPIEL_GEWONNEN, gewinn.ToString());
                beiGwinn(gewinn);
            }
            else
                beiSpielGehtWeiter();
        }

        bool ArrayEqual(int[] a, int[] b)
        {
            if (a.Length != b.Length) return false;
            return !a.Where((t, i) => t != b[i]).Any();
        }


        public IEnumerable<Event> Spielzüge_des_aktuellen_Spiels_ermittteln()
        {
            var indexAktuellesSpiel = _eventStore.History
                                                 .Select((e, i) => new { Event = e, Index = i })
                                                 .Where(ei => ei.Event.Name == Spielevents.EVENT_NEUES_SPIEL)
                                                 .Select(ei => ei.Index)
                                                 .LastOrDefault();
            var spielevents = _eventStore.History.Where((_, i) => i > indexAktuellesSpiel);
            return spielevents;
        }
    }
}