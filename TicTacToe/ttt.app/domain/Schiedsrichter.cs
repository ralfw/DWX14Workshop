using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dwx14.eventstore;
using ttt.app.adapters;

namespace ttt.app.domain
{
    public class Schiedsrichter
    {
        private readonly EventStore _eventStore;

        public Schiedsrichter(EventStore eventStore) { _eventStore = eventStore; }


        public void Spielende_schon_erreicht(Action beiSpielGehtWeiter, Action<Spielstatusse> beiSpielende)
        {
            var letzterEvent = _eventStore.History.Last();
            if (letzterEvent.Name == Spielevents.EVENT_SPIEL_UNENTSCHIEDEN)
                beiSpielende(Spielstatusse.Unentschieden);
            else if (letzterEvent.Name == Spielevents.EVENT_SPIEL_GEWONNEN)
                beiSpielende((Spielstatusse)Enum.Parse(typeof(Spielstatusse), letzterEvent.Payload));
            else
                beiSpielGehtWeiter();
        }


        public void Prüfe_auf_Spielende(Event[] spielzüge, Action beiSpielGehtWeiter, Action<Spielstatusse> beiSpielende)
        {
            Prüfe_auf_Gewinn(spielzüge,
                () => Prüfe_auf_Unentschieden(spielzüge,
                        beiSpielGehtWeiter,
                        () => beiSpielende(Spielstatusse.Unentschieden)),
                beiSpielende);
        }

        private void Prüfe_auf_Unentschieden(Event[] spielzüge, Action beiSpielGehtWeiter, Action beiUnentschieden)
        {
            if (spielzüge.Length >= 9)
            {
                _eventStore.Append(Spielevents.EVENT_SPIEL_UNENTSCHIEDEN, "");
                beiUnentschieden();
            }
            else
                beiSpielGehtWeiter();
        }

        private void Prüfe_auf_Gewinn(Event[] spielzüge, Action beiSpielGehtWeiter, Action<Spielstatusse> beiGwinn)
        {
            Prüfe_auf_Gewinn_für_Spieler(spielzüge, Spielstatusse.XamZug, Spielstatusse.Xgewonnen,
                () => Prüfe_auf_Gewinn_für_Spieler(spielzüge, Spielstatusse.OamZug, Spielstatusse.Ogewonnen,
                        beiSpielGehtWeiter,
                        beiGwinn),
                beiGwinn);
        }

        private void Prüfe_auf_Gewinn_für_Spieler(IEnumerable<Event> spielzüge, Spielstatusse spieler, Spielstatusse gewinn,
                                                 Action beiSpielGehtWeiter, Action<Spielstatusse> beiGwinn)
        {
            var spielerzüge = spielzüge.Select(e =>
            {
                var parts = e.Payload.Split(',');
                return new
                {
                    Spieler = (Spielstatusse)Enum.Parse(typeof(Spielstatusse), parts[0]),
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
                }.Any(g => ArrayEqual(g, spielerzüge.ToArray())))
            {
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
    }
}
