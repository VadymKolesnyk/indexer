using System;
using System.Linq;

namespace Indexing.Application
{
    public class Logger
    {
        readonly Action<string> _log;

        public Logger()
        {
            _log = Console.WriteLine;
        }
        public Logger(Action<string> log)
        {
            _log = log;
        }

        public void Log(string message)
        {
            var lines = message.Split('\n');
            string time = $"[{DateTime.Now.ToShortDateString(),10} {DateTime.Now.ToLongTimeString(),8}] : ";
            string space = new(' ', time.Length);
            _log(time + lines.First());
            foreach (var line in lines.Skip(1))
            {
                _log(space + line);
            }
        }
    }
}
