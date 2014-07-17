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
            if (e == null || e.Name != Spielevents.EVENT_SPIELSTEIN_GESETZT) return Spielstatusse.XamZug;

            return e.Payload.IndexOf("X") >= 0 ? Spielstatusse.OamZug : Spielstatusse.XamZug;
        }   


        public void Spielstein_setzen(Spielstatusse spieler, int spielfeldindex)
        {
            _eventStore.Append(Spielevents.EVENT_SPIELSTEIN_GESETZT, string.Format("{0},{1}", spieler, spielfeldindex));
        }


        public void Neues_Spiel_beginnen()
        {
            _eventStore.Append(Spielevents.EVENT_NEUES_SPIEL, "");
        }


        public void Zug_validieren(int spielfeldindex, Action validerZug, Action invaliderZug)
        {
            var spielzüge = Spielzüge_des_aktuellen_Spiels_ermittteln();
            if (spielzüge.Any(e => e.Payload.IndexOf(spielfeldindex.ToString()) >= 0))
                invaliderZug();
            else
                validerZug();
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