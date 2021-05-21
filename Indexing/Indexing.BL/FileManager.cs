using System;
using System.Collections.Generic;
using System.IO;

namespace Indexing.BL
{
    public class FileManager
    {
        private readonly IEnumerable<string> _files;
        private readonly IEnumerator<string> _filesEnumerator;
        private readonly object _locker = new();
        public FileManager(string root)
        {
            if (string.IsNullOrEmpty(root))
            {
                throw new ArgumentException($"\"{nameof(root)}\" can't be null or empty.");
            }
            _files = GetAllFiles(root);
            _filesEnumerator = _files.GetEnumerator();
        }

        private static IEnumerable<string> GetAllFiles(string root)
        {
            return Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories);
        }
        public string GetNextOrNull()
        {
            lock (_locker)
            {
                if (_filesEnumerator.MoveNext())
                {
                    return _filesEnumerator.Current;
                }
                return null;
            }
        }
    }
}
