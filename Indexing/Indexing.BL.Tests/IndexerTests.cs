using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Indexing.BL.Tests
{
    public class IndexerTests : IDisposable
    {
        static int _testsCount = 1;
        private readonly string _directoryPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\TestDirectory_{_testsCount++}";
        public IndexerTests()
        {
            Directory.CreateDirectory(_directoryPath);
        }
        public void Dispose()
        {
            Directory.Delete(_directoryPath, true);
        }

        [Fact]
        public void Indexer_WhenNegativeNumberOfTheads_ThenThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Indexer(_directoryPath, -1));
        }


    }
}
