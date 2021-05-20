using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Indexing
{
    public class Index
    {
        private readonly ConcurrentDictionary<string, IImmutableSet<string>> _dictionary = new();

        public IEnumerable<string> this[params string[] words]
        {
            get
            {
                if (words is null || words.Length == 0)
                {
                    return Enumerable.Empty<string>();
                }
                var listsOfFiles = words
                  .Where(word => word is not null && _dictionary.ContainsKey(word))
                  .Select(word => _dictionary[word]);
                if (!listsOfFiles.Any())
                {
                    return Enumerable.Empty<string>();
                }
                return listsOfFiles
                  .Aggregate<IEnumerable<string>>((filesIntersect, files) => filesIntersect.Intersect(files));
            }
        }
        public void Add(string word, string file)
        {
            _dictionary.AddOrUpdate(
                word,
                word => ImmutableHashSet.Create(file),
                (word, files) => files.Add(file));
        }
    }
}