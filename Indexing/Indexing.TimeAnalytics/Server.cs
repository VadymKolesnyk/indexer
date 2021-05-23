using Indexing.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Indexing.TimeAnalytics
{
    class Server
    {
        readonly TcpListener _server;
        readonly IPAddress _ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
        readonly int _port = 5001;
        Index _index;
        public Server()
        {
            _server = new TcpListener(_ip, _port);
        }

        public async Task<Server> SetUpAsync(string directory, int numberOfThreads)
        {
            var indexer = new Indexer(directory, numberOfThreads);
            _index = await indexer.GetIndexAsync();
            return this;
        }

        public async Task<Server> StartAsync()
        {
            _server.Start();
            Log("server started");

            return this;
        }



        private static void Log(string message)
        {
            var lines = message.Split('\n');
            string time = $"[{DateTime.Now.ToShortDateString(),10} {DateTime.Now.ToShortTimeString(),10}] : ";
            string space = new(' ', time.Length);
            Console.Write(time);
            Console.WriteLine(lines.First());
            foreach (var line in lines.Skip(1))
            {
                Console.Write(space);
                Console.WriteLine(line);
            }
        }
    }
}
