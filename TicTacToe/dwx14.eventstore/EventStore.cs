using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dwx14.eventstore
{
    public class EventStore
    {
        private readonly List<Event> _events = new List<Event>();

        public void Append(string eventName, string payload)
        {
            var e = new Event {Name = eventName, Payload = payload};
            _events.Add(e);
            OnAppended(e);
        }

        public IEnumerable<Event> History { get { return _events; }}


        public event Action<Event> OnAppended;
    }

    public class Event
    {
        public string Name;
        public string Payload;
    }
}
