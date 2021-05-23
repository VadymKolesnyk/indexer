using Indexing.BL;
using System.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Indexing.TimeAnalytics
{
    class Server
    {
        readonly TcpListener _server;
        readonly IPAddress _ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
        readonly int _port = 5001;
        Logger _logger = new();
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

        public async Task StartAsync()
        {
            _server.Start();
            _logger.Log("server started");

            while (true)
            {
                var client = await _server.AcceptTcpClientAsync();
                _logger.Log("New client was connected");

                var handler = new ClientHandler(_index, _logger, client);
                _ = handler.HandleAsync();
            }

        }

    }
}
