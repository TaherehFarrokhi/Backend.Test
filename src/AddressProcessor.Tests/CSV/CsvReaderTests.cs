using System;
using System.IO;
using AddressProcessing.CSV;
using Moq;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CsvReaderTests
    { 
        private Mock<IFileProvider>  _fileProviderMock;

        [SetUp]
        public void Setup()
        {
            var content = "Value1\tValue2" + Environment.NewLine;
            _fileProviderMock = new Mock<IFileProvider>();
            _fileProviderMock.Setup(x => x.GetStreamReader(It.IsAny<string>())).Returns(new StringReader(content));
        }

        [Test]
        public void Should_ReturnNull_When_ItReachesEndOfFile()
        {
            //Arrange
            var subject = new CsvReader(_fileProviderMock.Object, "TestFileName.cvs");

            // Act
            Columns actual;
            actual = subject.Read();
            actual = subject.Read(); // End of content

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void Should_ReturnOneSetOfColumnsWithCorrectValues_WhenTheFileHasOneRow()
        {
            //Arrange
            var subject = new CsvReader(_fileProviderMock.Object, "TestFileName.cvs");

            // Act
            var actual = subject.Read();

            // Assert
            Assert.NotNull(actual);
            Assert.AreEqual("Value1", actual.Column1);
            Assert.AreEqual("Value2", actual.Column2);
        }
    }
}