using System.Collections.Generic;
using System.Linq;
using dwx14.eventstore;
using ttt.app.adapters;
using ttt.app.domain;

namespace ttt.app
{
    public class Mapper
    {
        private readonly EventStore _eventStore;

        public Mapper(EventStore eventStore) { _eventStore = eventStore; }


        public Spielstand Spieldstand_generieren(Spielsteine aktuellerSpieler)
        {
            var spielstand = new Spielstand();

            var spielevents = Spielzüge_des_aktuellen_Spiels_ermittteln();

            spielstand.Spielbrett = new Spielsteine[9];
            foreach (var e in spielevents) {
                var parts = e.Payload.Split(',');
                var spielfeldindex = int.Parse(parts[1]);
                spielstand.Spielbrett[spielfeldindex] = parts[0] == "X" ? Spielsteine.X : Spielsteine.O;
            }

            spielstand.Spielstatus = aktuellerSpieler == Spielsteine.X ? Spielstatusse.XamZug : Spielstatusse.OamZug;
            return spielstand;
        }


        private IEnumerable<Event> Spielzüge_des_aktuellen_Spiels_ermittteln()
        {
            var indexAktuellesSpiel = _eventStore.History
                                                 .Select((e, i) => new {Event = e, Index = i})
                                                 .Where(ei => ei.Event.Name == Spielevents.EVENT_NEUES_SPIEL)
                                                 .Select(ei => ei.Index)
                                                 .LastOrDefault();
            var spielevents = _eventStore.History.Where((_, i) => i > indexAktuellesSpiel);
            return spielevents;
        }
    }
}