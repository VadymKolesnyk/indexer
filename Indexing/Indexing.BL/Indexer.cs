using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Indexing.BL
{
    public class Indexer
    {
        public int NumberOfThreads { get; }
        private readonly FileManager _manager;
        public Indexer(string rootDirectory, int numberOfThreads)
        {
            if (string.IsNullOrEmpty(rootDirectory))
            {
                throw new ArgumentException($"\"{nameof(rootDirectory)}\" can't be null or empty.", nameof(rootDirectory));
            }
            if (numberOfThreads < 1)
            {
                throw new ArgumentException($"\"{nameof(numberOfThreads)}\" can't be less then 1");
            }
            NumberOfThreads = numberOfThreads;
            _manager = new FileManager(rootDirectory);
        }

        public async Task<Index> GetIndexAsync()
        {
            Index index = new();

            var tasks = Enumerable.Repeat(Task.Run(() => ProcessIndex(index)), NumberOfThreads);

            return await Task.WhenAll(tasks).ContinueWith(_ => index);
        }

        private void ProcessIndex(Index index)
        {
            string file;
            while ((file = _manager.GetNextOrNull()) is not null)
            {
                string text = File.ReadAllText(file);
                string[] words = text.Split(' ');
                foreach (string word in words)
                {
                    index.Add(word, file);
                }
            }
        }
    }
}
