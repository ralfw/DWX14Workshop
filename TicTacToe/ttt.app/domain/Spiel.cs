﻿using System.Linq;
using dwx14.eventstore;
using ttt.app.adapters;

namespace ttt.app.domain
{
    public class Spiel
    {
        private readonly EventStore _eventStore;

        public Spiel(EventStore eventStore) { _eventStore = eventStore; }


        public Spielsteine Spieler_feststellen()
        {
            var e = _eventStore.History.LastOrDefault();
            if (e == null || e.Name != Spielevents.EVENT_SPIELSTEIN_GESETZT) return Spielsteine.X;

            return e.Payload.IndexOf("X") >= 0 ? Spielsteine.O : Spielsteine.X;
        }   


        public void Spielstein_setzen(Spielsteine spieler, int spielfeldindex)
        {
            _eventStore.Append(Spielevents.EVENT_SPIELSTEIN_GESETZT, string.Format("{0},{1}", spieler, spielfeldindex));
        }


        public void Neues_Spiel_beginnen()
        {
            _eventStore.Append(Spielevents.EVENT_NEUES_SPIEL, "");
        }
    }
}