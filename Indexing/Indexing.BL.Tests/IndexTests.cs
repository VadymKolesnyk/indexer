using System;
using System.Linq;
using Xunit;

namespace Indexing.BL.Tests
{
    public class IndexTests
    {
        [Fact]
        public void Add_WhenAdd2Files_Then2Files()
        {
            Index index = new();
            index.Add("word1", "file1");
            index.Add("word1", "file2");
            Assert.Equal(2, index["word1"].Count());
        }
        [Fact]
        public void Add_WhenAddSameCombinations_Then1File()
        {
            Index index = new();
            index.Add("word1", "file1");
            index.Add("word1", "file1");
            Assert.Single(index["word1"]);
        }
        [Fact]
        public void Indexer_WhenNoFilesFor1Word_ThenEmpty()
        {
            Index index = new();
            Assert.Empty(index["word1"]);
        }
        [Fact]
        public void Indexer_WhenNoFilesFor2Words_ThenEmpty()
        {
            Index index = new();
            index.Add("word2", "file1");
            Assert.Empty(index["word1", "word2"]);
        }
        [Fact]
        public void Indexer_When1SameFileFor2Words_ThenSingle()
        {
            Index index = new();
            index.Add("word1", "file1");
            index.Add("word1", "file3");
            index.Add("word2", "file1");
            index.Add("word2", "file2");
            Assert.Single(index["word1", "word2"]);
            Assert.Equal("file1", index["word1", "word2"].First());
        }
        [Fact]
        public void Indexer_WhenEmptyArgument_ThenEmpty()
        {
            Index index = new();
            Assert.Empty(index[Array.Empty<string>()]);
        }

    }
}
