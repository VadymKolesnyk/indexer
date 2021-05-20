using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Indexing
{
    public class Index
    {
        private readonly ConcurrentDictionary<string, IList<string>> _dictionary = new();

        public IEnumerable<string> this[params string[] words]
        {
            get
            {
                if (words is null || words.Length == 0)
                {
                    return Enumerable.Empty<string>();
                }
                return words
                  .Where(word => word is not null && _dictionary.ContainsKey(word))
                  .Select(word => _dictionary[word])
                  .Aggregate(Enumerable.Empty<string>(), (filesIntersect, files) => filesIntersect.Intersect(files));
            }
        }
        public void Add(string word, string file)
        {
            _dictionary.AddOrUpdate(
                word,
                word =>
                {
                    List<string> list = new();
                    list.Add(file);
                    return list;
                },
                (word, files) =>
                {
                    files.Add(file);
                    return files;
                });
        }
    }
}