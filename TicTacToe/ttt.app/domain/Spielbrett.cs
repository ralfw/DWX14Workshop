using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dwx14.eventstore;
using ttt.app.adapters;

namespace ttt.app.domain
{
    public class Spielbrett
    {
        private Spielsteine[] _spielfelder;


        public Spielbrett(IEnumerable<Event> events) {
            foreach(var e in events) Update(e);    
        }


        public void Update(Event e)
        {
            switch (e.Name)
            {
                case Spielevents.EVENT_NEUES_SPIEL:
                    _spielfelder = new Spielsteine[9];
                    break;
                case Spielevents.EVENT_SPIELSTEIN_GESETZT:
                    var parts = e.Payload.Split(',');
                    _spielfelder[int.Parse(parts[1])] = parts[0] == Spielstatusse.XamZug.ToString() ? Spielsteine.X : Spielsteine.O;
                    break;
            }
        }


        public Spielsteine[] Spielfelder
        {
            get { return _spielfelder; }
        }
    }
}
