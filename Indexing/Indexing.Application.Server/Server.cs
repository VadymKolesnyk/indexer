using Indexing.BL;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Indexing.Application.Server
{
    public class Server
    {
        readonly TcpListener _server;
        readonly IPAddress _ip = new(new byte[] { 127, 0, 0, 1 });
        readonly int _port = 5001;
        readonly Logger _logger = new();
        Index _index;
        public Server()
        {
            _server = new TcpListener(_ip, _port);
        }

        public async Task<Server> SetUpAsync(string directory, int numberOfThreads)
        {
            var indexer = new Indexer(directory, numberOfThreads);
            _logger.Log("Start indexing");
            _index = await indexer.GetIndexAsync();
            _logger.Log("Finish indexing");
            return this;
        }

        public async Task StartAsync()
        {
            _server.Start();
            _logger.Log($"Server started at port {_port}");

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
