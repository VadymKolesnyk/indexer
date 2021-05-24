using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        public Task<Index> GetIndexAsync()
        {
            Index index = new();

            var tasks = Enumerable.Repeat(0, NumberOfThreads)
                                  .Select(_ => Task.Run(() => ProcessIndex(index)))
                                  .ToArray();

            return Task.WhenAll(tasks).ContinueWith(_ => index);
        }

        private void ProcessIndex(Index index)
        {
            string file = _manager.GetNextOrNull();
            while (file is not null)
            {
                string text = File.ReadAllText(file);
                var words = HandleText(text);
                foreach (string word in words)
                {
                    index.Add(word, file);
                }
                file = _manager.GetNextOrNull();
            }
        }

        private IEnumerable<string> HandleText(string text)
        {
            text = text.Replace("<br />", string.Empty);
            return Regex.Split(text, @"[^(0-9|a-z|A-Z|')]+").Where(w => w != string.Empty).Select(w => w.ToLower());
        }
    }
}
