using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Indexing.Application.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine($"Argument error : numberOfArguments");
                return;
            }
            var parseInt = int.TryParse(args.First(), out int numberOfThreads);
            string path = args.Last();
            var existsDirectory = Directory.Exists(path);
            if (!parseInt)
            {
                Console.WriteLine($"Argument error: numberOfThreads");
                return;
            }
            if (!existsDirectory)
            {
                Console.WriteLine($"Argument error: directoryPath");
                return;
            }


            var server = new Server();
            await server.SetUpAsync(path, numberOfThreads);
            await server.StartAsync();

        }
    }
}
