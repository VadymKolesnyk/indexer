using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexing.BL
{
    class FileManager
    {
        private readonly IEnumerable<string> _files;
        private readonly IEnumerator<string> _filesEnumerator;
        private readonly object _locker = new object();
        public FileManager(string root)
        {
            if (string.IsNullOrEmpty(root))
            {
                throw new ArgumentException($"\"{nameof(root)}\" can't be null or empty.");
            }
            _files = GetAllFiles(root);
            _filesEnumerator = _files.GetEnumerator();
        }

        private IEnumerable<string> GetAllFiles(string root)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(root);
            return Directory.EnumerateDirectories(root).Aggregate(files, (files, dir) => files.Concat(GetAllFiles(dir)));
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
