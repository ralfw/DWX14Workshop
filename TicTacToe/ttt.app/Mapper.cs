using System.Collections.Generic;
using System.Linq;
using dwx14.eventstore;
using ttt.app.adapters;
using ttt.app.domain;

namespace ttt.app
{
    public class Mapper
    {
        private readonly Spielbrett _spielbrett;

        public Mapper(Spielbrett spielbrett) { _spielbrett = spielbrett; }


        public Spielstand Spieldstand_generieren(IEnumerable<Event> spielevents, Spielstatusse aktuellerSpieler)
        {
            return new Spielstand {Spielbrett = _spielbrett.Spielfelder, Spielstatus = aktuellerSpieler};
        }
    }
}