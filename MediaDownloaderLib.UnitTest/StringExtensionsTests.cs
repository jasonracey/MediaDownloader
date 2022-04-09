using System;
using NUnit.Framework;

namespace MediaDownloaderLib.UnitTest
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void RepeatedlyReplace_NullString_Throws()
        {
            // arrange
            string testString = null!;
        
            // act/assert
            Assert.Throws<ArgumentNullException>(() => testString.RepeatedlyReplace("  ", " "));
        }
    
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("  ", " ")]
        [TestCase("ab", "ab")]
        [TestCase("a b", "a b")]
        [TestCase("a  b", "a b")]
        [TestCase("a   b", "a b")]
        public void RepeatedlyReplace_ReturnsExpectedString(string input, string expectedOutput)
        {
            Assert.AreEqual(expectedOutput, input.RepeatedlyReplace("  ", " "));
        }
    }
}