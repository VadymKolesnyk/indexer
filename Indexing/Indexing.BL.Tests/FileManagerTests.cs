using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Indexing.BL.Tests
{
    public class FileManagerTests : IDisposable
    {
        static int _testsCount = 1;

        private readonly string _directoryPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\FileManager_TestDirectory_{_testsCount++}";
        private readonly FileManager _fileManager;
        public FileManagerTests()
        {
            Directory.CreateDirectory(_directoryPath);
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
        [Fact]
        public void GetNextOrNull_WhenEmptyDirectory_ThenNull()
        {
            Assert.Null(_fileManager.GetNextOrNull());
        }
        [Fact]
        public void GetNextOrNull_When5Files_ThenGet5Files()
        {
            for (int i = 0; i < 5; i++)
            {
                using var file = File.Create(Path.Combine(_directoryPath, $"file{i}.txt"));
                file.Close();
            }
            for (int i = 0; i < 5; i++)
            {
                Assert.NotNull(_fileManager.GetNextOrNull());
            }
            Assert.Null(_fileManager.GetNextOrNull());
        }
        [Fact]
        public void GetNextOrNull_When5FilesInSubdirecrories_ThenGet5Files()
        {
            for (int i = 0; i < 2; i++)
            {
                using var file = File.Create(Path.Combine(_directoryPath, $"file{i}.txt"));
                file.Close();
            }
            Directory.CreateDirectory(Path.Combine(_directoryPath, "sub1"));
            using var file2 = File.Create(Path.Combine(_directoryPath, $@"sub1\file2.txt"));
            file2.Close();
            Directory.CreateDirectory(Path.Combine(_directoryPath, "sub2"));
            using var file3 = File.Create(Path.Combine(_directoryPath, $@"sub2\file3.txt"));
            file3.Close();
            using var file4 = File.Create(Path.Combine(_directoryPath, $@"sub2\file4.txt"));
            file4.Close();
            for (int i = 0; i < 5; i++)
            {
                Assert.NotNull(_fileManager.GetNextOrNull());
            }
            Assert.Null(_fileManager.GetNextOrNull());
        }
        [Fact]
        public void GetNextOrNull_When1000FilesWithRaceCondition_ThenGet1000Files()
        {
            for (int i = 0; i < 1000; i++)
            {
                File.Create(Path.Combine(_directoryPath, $"file{i}.txt")).Close();
            }
            ConcurrentBag<string> bag = new();
            Parallel.For(0, 1000, i => bag.Add(_fileManager.GetNextOrNull()));
            Assert.Null(_fileManager.GetNextOrNull());
            for (int i = 0; i < 1000; i++)
            {
                Assert.Contains(Path.Combine(_directoryPath, $"file{i}.txt"), bag);
            }
        }
    }
}
