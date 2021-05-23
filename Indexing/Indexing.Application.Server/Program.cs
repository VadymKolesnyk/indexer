using System;

namespace Indexing.Application.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Enter directory with files (if empty - D:\datasets\aclImdb)");
            var input = Console.ReadLine();
            var path = input == string.Empty ? @"D:\datasets\aclImdb" : input;

        }
    }
}
