using System;
using System.Net.Sockets;

namespace Indexing.Application.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            Logger logger = new Logger();
            try
            {
                client = new TcpClient("127.0.0.1", 5001);
                stream = client.GetStream();
                string[] input = new string[] { "to", "see", "how", "this", "kind" };
                Messager.Send(stream, new { words = input });
                var answer = Messager.Recive(stream);
                Console.WriteLine(answer);
            }
            catch(SocketException e)
            {
                logger.Log(e.Message);
            }
            finally
            {
                client?.Close();
                stream?.Close();
            }
            
        }
    }
}
