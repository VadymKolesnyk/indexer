using System;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;

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

                do
                {
                    string[] input = GetWords();
                    Messager.Send(stream, new { words = input });
                    var answer = Messager.Recive(stream);
                    Console.WriteLine(answer);

                } while (WantToContinue());
                Messager.Send(stream, new { stop = true });
            }
            catch (Exception e)
            {
                logger.Log(e.Message);
            }
            finally
            {
                client?.Close();
                stream?.Close();
            }

        }

        static string[] GetWords()
        {
            Console.WriteLine("Please enter the words you want to find");
            return Regex.Split(Console.ReadLine(), @"[^(0-9|a-z|A-Z|')]+").Where(w => w != string.Empty).Select(w => w.ToLower()).ToArray();
        }
        static bool WantToContinue()
        {
            Console.WriteLine("If you want to finish, write to the console \'y\' or \'yes\'. If not press enter");
            var input = Console.ReadLine().Trim().ToLower();
            return input != "y" && input != "yes";
        }
    }
}
