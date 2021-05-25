using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Indexing.BL
{
    public class Index
    {
        private readonly ConcurrentDictionary<string, IImmutableSet<string>> _dictionary = new();

        public ConcurrentDictionary<string, IImmutableSet<string>> GetDictionaryUnsave() => _dictionary;
        public IEnumerable<string> this[params string[] words]
        {
            get
            {
                if (words is null || !words.Any() || words.Any(word => !_dictionary.ContainsKey(word.ToLower())))
                {
                    return Enumerable.Empty<string>();
                }
                return words
                  .Select(word => _dictionary[word.ToLower()])
                  .Aggregate<IEnumerable<string>>((filesIntersect, files) => filesIntersect.Intersect(files));
            }
        }
        public void Add(string word, string file)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentException($"\"{nameof(word)}\" can't be null or empty.", nameof(word));
            }

            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentException($"\"{nameof(file)}\" can't be null or empty.", nameof(file));
            }

            _dictionary.AddOrUpdate(
                word,
                word => ImmutableHashSet.Create(file),
                (word, files) => files.Add(file));
        }
    }
}