using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Indexing.BL.Tests
{
    public class FileManagerTests : IDisposable
    {
        static int _testsCount = 1;

        private readonly string _directoryPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\TestDirectory_{_testsCount++}";
        private readonly FileManager _fileManager;
        public FileManagerTests()
        {
            Directory.CreateDirectory(_directoryPath);
            File.Create(Path.Combine(_directoryPath, "file1.txt")).Close();
            _fileManager = new FileManager(_directoryPath);
        }
        public void Dispose()
        {
            Directory.Delete(_directoryPath, true);
        }

        [Fact]
        public void FileManager_WhenNullArgument_ThenThrowArgumentExeption()
        {
            Assert.Throws<ArgumentException>(() => new FileManager(null));
        }
        [Fact]
        public void FileManager_WhenEmptyArgument_ThenThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new FileManager(""));
        }
        [Fact]
        public void FileManager_WhenDirectoryNotExists_ThenThrowDirectoryNotFoundException()
        {
            string path = Path.Combine(_directoryPath, "NotExistDirectory");           
            Assert.Throws<DirectoryNotFoundException>(() => new FileManager(path));
        }
    }
}
