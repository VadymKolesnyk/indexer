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

    }
}
