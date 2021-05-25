using Indexing.BL;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;

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
            for (int i = 2; i <= 16; i++)
            {
                GC.Collect();
                var indexer = new Indexer(path, i);
                clock = Stopwatch.StartNew();
                task = indexer.GetIndexAsync();
                var index = task.Result;
                clock.Stop();
                var equals = CompareIndexs(indexInOneThread, index);
                Console.WriteLine($"Path: {path},\t Number of threads : {i,2},\tTime : {clock.Elapsed},\tEquals : {equals}");

            }
            Console.ReadLine();

        }


        static bool CompareIndexs(BL.Index index1, BL.Index index2) 
        {
            var dict1 = index1.GetDictionaryUnsave();
            var dict2 = index2.GetDictionaryUnsave();

            return CheckContains(dict1, dict2) && CheckContains(dict2, dict1);
        }

        static bool CheckContains(ConcurrentDictionary<string, IImmutableSet<string>> first, ConcurrentDictionary<string, IImmutableSet<string>> second)
        {
            foreach (var kvp in first)
            {
                foreach (var file in kvp.Value)
                {
                    if (second.TryGetValue(kvp.Key, out var value))
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
