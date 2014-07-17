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
            _events.Add(new Event{Name=eventName, Payload = payload});
        }

        public IEnumerable<Event> History { get { return _events; }} 
    }

    public class Event
    {
        public string Name;
        public string Payload;
    }
}
