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


        public Spielstand Spieldstand_generieren(IEnumerable<Event> spielevents, Spielstatusse aktuellerSpieler)
        {
            var spielstand = new Spielstand();

            spielstand.Spielbrett = new Spielsteine[9];
            foreach (var e in spielevents) {
                var parts = e.Payload.Split(',');
                var spielfeldindex = int.Parse(parts[1]);
                spielstand.Spielbrett[spielfeldindex] = parts[0] == "XamZug" ? Spielsteine.X : Spielsteine.O;
            }

            spielstand.Spielstatus = aktuellerSpieler;
            return spielstand;
        }
    }
}