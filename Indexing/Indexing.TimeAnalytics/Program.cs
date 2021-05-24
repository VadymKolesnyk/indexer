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

            var indexerInOneThread = new Indexer(path, 1);
            var clock = Stopwatch.StartNew();
            var task = indexerInOneThread.GetIndexAsync();
            var indexInOneThread = task.Result;
            clock.Stop();
            Console.WriteLine($"Path: {path},\t Number of threads : {1,2},\tTime : {clock.Elapsed}");
            GC.Collect();
            for (int i = 2; i <= 16; i++)
            {
                var indexer = new Indexer(path, i);
                clock = Stopwatch.StartNew();
                task = indexer.GetIndexAsync();
                var index = task.Result;
                clock.Stop();
                var equals = CompareIndexs(indexInOneThread, index);

                Console.WriteLine($"Path: {path},\t Number of threads : {i,2},\tTime : {clock.Elapsed},\tEquals : {equals}");
                GC.Collect();
            }
            Console.ReadLine();

        }


        static bool CompareIndexs(Index index1, Index index2)
        {
            var dict1 = index1.GetDictionaryUnsave();
            var dict2 = index2.GetDictionaryUnsave();

            foreach (var kvp in dict1)
            {
                foreach (var file in kvp.Value)
                {
                    if (dict2.TryGetValue(kvp.Key, out var value))
                    {
                        if (!value.Contains(file))
                        {
                            return false;
                        } 
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            foreach (var kvp in dict2)
            {
                foreach (var file in kvp.Value)
                {
                    if (dict1.TryGetValue(kvp.Key, out var value))
                    {
                        if (!value.Contains(file))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
