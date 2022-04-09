using System;
using System.IO;
using Moq;
using NUnit.Framework;

namespace MediaDownloaderLib.UnitTest
{
    [TestFixture]
    public class DestinationPathBuilderTests
    {
        private Mock<IDirectoryWrapper> _mockDirectoryWrapper = null!;
        private Mock<IFileWrapper> _mockFileWrapper = null!;

        private DestinationPathBuilder _destinationPathBuilder = null!;

        [SetUp]
        public void TestInitialize()
        {
            _mockDirectoryWrapper = new Mock<IDirectoryWrapper>();
            _mockFileWrapper = new Mock<IFileWrapper>();
            _destinationPathBuilder = new DestinationPathBuilder(
                _mockDirectoryWrapper.Object,
                _mockFileWrapper.Object);
        }

        [Test]
        public void ValidatesArgs()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _destinationPathBuilder = new DestinationPathBuilder(null!, _mockFileWrapper.Object));
            Assert.Throws<ArgumentNullException>(() =>
                _destinationPathBuilder = new DestinationPathBuilder(_mockDirectoryWrapper.Object, null!));
            Assert.IsNotNull(_destinationPathBuilder =
                new DestinationPathBuilder(_mockDirectoryWrapper.Object, _mockFileWrapper.Object));
        }

        [Test]
        public void WhenUriNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _destinationPathBuilder.CreateDestinationPath(null!));
        }

        [Test]
        public void ReturnsExpectedDestinationPath()
        {
            // arrange
            const string fileName = "track01.flac";
            const string albumName = "mock-album-name";
            var sourcePath = $"https://www.mock.com/{albumName}/{fileName}";
            var uri = new Uri(sourcePath);
            var destinationPath = Path.Combine("Downloads", albumName, fileName);
            var destinationPathBase = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var fullDestinationPath = Path.Combine(destinationPathBase, destinationPath);

            // act
            var actualDestinationPath = _destinationPathBuilder.CreateDestinationPath(uri);

            // assert
            Assert.AreEqual(fullDestinationPath, actualDestinationPath);
        }

        [Test]
        public void MakesCertainDestinationDirectoryExists()
        {
            // arrange
            const string albumName = "mock-album-name";
            var uri = new Uri($@"https://www.mock.com/{albumName}/track01.flac");
            var destinationPathBase = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var fullDestinationPath =
                Path.Combine(destinationPathBase, $"Downloads{Path.DirectorySeparatorChar}{albumName}");

            // act
            var _ = _destinationPathBuilder.CreateDestinationPath(uri);

            // assert
            _mockDirectoryWrapper.Verify(m => m.CreateDirectory(fullDestinationPath), Times.Once);
        }

        [Test]
        public void WhenFileAlreadyExists_DeletesFile()
        {
            // arrange
            _mockFileWrapper.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            var uri = new Uri(@"https://www.mock.com/mock-album-name/track01.flac");

            // act
            var actualDestinationPath = _destinationPathBuilder.CreateDestinationPath(uri);

            // assert
            _mockFileWrapper.Verify(m => m.Delete(actualDestinationPath), Times.Once);
        }

        [Test]
        public void WhenFileDoesNotAlreadyExist_DoesNotAttemptToDeleteFile()
        {
            // arrange
            _mockFileWrapper.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);
            var uri = new Uri(@"https://www.mock.com/mock-album-name/track01.flac");

            // act
            var actualDestinationPath = _destinationPathBuilder.CreateDestinationPath(uri);

            // assert
            _mockFileWrapper.Verify(m => m.Delete(actualDestinationPath), Times.Never);
        }
    }
}