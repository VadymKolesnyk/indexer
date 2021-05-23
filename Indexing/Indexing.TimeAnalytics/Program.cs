using Indexing.BL;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Indexing.TimeAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Enter directory with files (if empty - D:\datasets\aclImdb)");
            var input = Console.ReadLine();
            var path = input == string.Empty ? @"D:\datasets\aclImdb" : input;

            for (int i = 1; i <= 16; i++)
            {
                var indexer = new Indexer(path, i);
                var clock = Stopwatch.StartNew();
                var task = indexer.GetIndexAsync();
                task.Wait();
                clock.Stop();
                Console.WriteLine($"Path: {path},\t Number of threads : {i,2},\tTime : {clock.Elapsed}");
                GC.Collect();
            }
            Console.ReadLine();

        }
    }
}
