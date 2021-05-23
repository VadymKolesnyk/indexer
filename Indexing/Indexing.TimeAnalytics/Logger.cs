using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexing.TimeAnalytics
{
    class Logger
    {
        Action<string?> _log;

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
            string time = $"[{DateTime.Now.ToShortDateString(),10} {DateTime.Now.ToShortTimeString(),10}] : ";
            string space = new(' ', time.Length);
            _log(time);
            _log(lines.First());
            foreach (var line in lines.Skip(1))
            {
                _log(space);
                _log(line);
            }
        }
    }
}
