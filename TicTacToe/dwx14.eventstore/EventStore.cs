using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dwx14.eventstore
{
    public class EventStore
    {
        private readonly string _dirPath;

        public EventStore(string dirPath)
        {
            _dirPath = dirPath;
            if (string.IsNullOrEmpty(dirPath)) dirPath = ".";
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        }


        public void Append(string eventName, string payload)
        {
            var filename = Path.Combine(_dirPath, Directory.GetFiles(_dirPath, "*.txt").Length.ToString("000000")) + ".txt";
            using (var sw = new StreamWriter(filename)) {
                sw.WriteLine(eventName);
                sw.WriteLine(payload);
            }
            OnAppended(new Event{Name=eventName, Payload = payload});
        }


        public IEnumerable<Event> History { get
        {
            var events = new List<Event>();
            var filenames = Directory.GetFiles(_dirPath, "*.txt").ToList();
            filenames.Sort();
            foreach (var fn in filenames) {
                using (var sr = new StreamReader(fn)) {
                    var e = new Event();
                    e.Name = sr.ReadLine();
                    e.Payload = sr.ReadToEnd();
                    events.Add(e);
                }                
            }
            return events;
        }}


        public event Action<Event> OnAppended;
    }


    public class Event
    {
        public string Name;
        public string Payload;
    }
}
