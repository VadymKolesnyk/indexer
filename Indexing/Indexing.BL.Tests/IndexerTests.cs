using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Indexing.BL.Tests
{
    public class IndexerTests : IDisposable
    {
        static int _testsCount = 1;
        private readonly string _directoryPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Indexer_TestDirectory_{_testsCount++}";
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

        [Fact]
        public async Task GetIndexAsync_When1File_ThenAllWordsInFile()
        {
            Indexer indexer = new Indexer(_directoryPath, 8);
            string filePath = Path.Combine(_directoryPath, "file.txt");
            using var file = File.CreateText(filePath);
            file.WriteLine("One two three");
            file.Close();
            var index = await indexer.GetIndexAsync();

            Assert.Equal(filePath, index["one"].First());
            Assert.Equal(filePath, index["two"].First());
            Assert.Equal(filePath, index["three"].First());
            Assert.Equal(filePath, index["three", "one"].First());
            Assert.Equal(filePath, index["three", "one", "two"].First());
        }

        [Fact]
        public async Task GetIndexAsync_WhenFileContainsCpecialSymbols_ThenIgnoreSpecialSymbols()
        {
            Indexer indexer = new Indexer(_directoryPath, 8);
            string filePath = Path.Combine(_directoryPath, "file.txt");
            using var file = File.CreateText(filePath);
            file.WriteLine("One: \"two\" <three> four I'm five; six_seven <br /> ");
            file.Close();
            var index = await indexer.GetIndexAsync();

            Assert.Equal(filePath, index["one"].First());
            Assert.Equal(filePath, index["two"].First());
            Assert.Equal(filePath, index["three"].First());
            Assert.Equal(filePath, index["four"].First());
            Assert.Equal(filePath, index["I'm"].First());
            Assert.Equal(filePath, index["five"].First());
            Assert.Equal(filePath, index["six"].First());
            Assert.Equal(filePath, index["seven"].First());

            Assert.Empty(index["one:"]);
            Assert.Empty(index["\"two\""]);
            Assert.Empty(index["<three>"]);
            Assert.Empty(index["five;"]);
            Assert.Empty(index["six_seven"]);
            Assert.Empty(index["br"]);
            Assert.Empty(index[""]);
        }
        [Fact]
        public async Task GetIndexAsync_WhenWordsIn2Files_Then2Files()
        {
            Indexer indexer = new Indexer(_directoryPath, 8);
            string filePath1 = Path.Combine(_directoryPath, "file1.txt");
            string filePath2 = Path.Combine(_directoryPath, "file2.txt");
            using var file1 = File.CreateText(filePath1);
            file1.WriteLine("One two three");
            file1.Close();
            using var file2 = File.CreateText(filePath2);
            file2.WriteLine("One two three");
            file2.Close();
            var index = await indexer.GetIndexAsync();

            Assert.Equal(2, index["one"].Count());
            Assert.Equal(2, index["two"].Count());
            Assert.Equal(2, index["three"].Count());
            Assert.Equal(2, index["three", "one"].Count());
            Assert.Equal(2, index["three", "one", "two"].Count());

        }


    }
}
