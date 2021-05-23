using System.Net.Sockets;
using System.Threading.Tasks;

namespace Indexing.Application.Server
{
    class ClientHandler
    {
        readonly Index _index;
        readonly Logger _logger;
        readonly TcpClient _client;

        public ClientHandler(Index index, Logger logger, TcpClient client)
        {
            _index = index;
            _logger = logger;
            _client = client;
        }

        public void Handle()
        {
            NetworkStream stream = null;
            try
            {
                stream = _client.GetStream();



            }
            finally
            {
                stream?.Close();
                _client?.Close();
            }
        }

        public Task HandleAsync() => Task.Run(Handle);

    }
}
