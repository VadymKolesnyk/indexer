using Indexing.BL;
using Newtonsoft.Json;
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
                while (true)
                {
                    dynamic request = Messager.Recive(stream);
                    _logger.Log($"Was recived mesage : {request}");
                    if (request["stop"] == true)
                    {
                        return;
                    }
                    var words = JsonConvert.DeserializeObject<string[]>(request["words"].ToString());
                    var files = _index[words];
                    var answer = new { files };
                    var jsonAnswer = Messager.Send(stream, answer);
                    _logger.Log($"Was sended answer : {jsonAnswer}");
                }


            }
            catch (JsonException e)
            {
                var answer = new { error = "Invalid body" };
                Messager.Send(stream, answer);
                _logger.Log(e.Message);
            }
            finally
            {
                stream?.Close();
                _client?.Close();
                _logger.Log("Client disconected");
            }
        }

        public Task HandleAsync() => Task.Run(Handle);

    }
}
