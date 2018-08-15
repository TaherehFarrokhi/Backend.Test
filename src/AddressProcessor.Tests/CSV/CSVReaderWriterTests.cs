using System;
using AddressProcessing.CSV;
using Moq;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private Mock<ICsvReaderWriterBuilder> _csvReaderWriterBuilderMock;
        private Mock<ICsvReader> _csvReaderMock;
        private Mock<ICsvWriter> _csvWriterMock;
        private CSVReaderWriter _subject;

        [SetUp]
        public void Setup()
        {
            _csvReaderWriterBuilderMock = new Mock<ICsvReaderWriterBuilder>();

            _csvReaderMock = new Mock<ICsvReader>();
            _csvReaderWriterBuilderMock.Setup(x => x.BuildReader(It.IsAny<string>()))
                .Returns(_csvReaderMock.Object);

            _csvWriterMock = new Mock<ICsvWriter>();
            _csvReaderWriterBuilderMock.Setup(x => x.BuildWriter(It.IsAny<string>()))
                .Returns(_csvWriterMock.Object);


            _subject = new CSVReaderWriter(_csvReaderWriterBuilderMock.Object);
        }

        [Test]
        public void Should_ReadCorrectlyFromCsvReader_WhenOpeningModeIsRead()
        {
            // Arrange

            // Act
            _subject.Open("TestFileName.csv", CSVReaderWriter.Mode.Read);
            _subject.Read(out var column1, out var column2);

            // Assert
            _csvReaderMock.Verify(m => m.Read(), Times.Once);
        }

        [Test]
        public void Should_WriteCorrectlyToCsvWritter_WhenOpeningModeIsWrite()
        {
            // Arrange

            // Act
            _subject.Open("TestFileName.csv", CSVReaderWriter.Mode.Write);
            _subject.Write("Value1");

            // Assert
            _csvWriterMock.Verify(m => m.Write("Value1"), Times.Once);
        }

        [Test]
        public void Should_ReadThrowException_WhenOpeningModeIsWrite()
        {
            // Arrange

            // Act
            _subject.Open("TestFileName.csv", CSVReaderWriter.Mode.Write);
            ;

            // Assert
            var ex = Assert.Throws<ApplicationException>(() => _subject.Read(out var column1, out var column2));
            Assert.That(ex.Message, Is.EqualTo("File is closed or hasn't been opened in read mode."));
        }

        [Test]
        public void Should_WriteThrowException_WhenOpeningModeIsRead()
        {
            // Arrange

            // Act
            _subject.Open("TestFileName.csv", CSVReaderWriter.Mode.Read);
            
            // Assert
            var ex = Assert.Throws<ApplicationException>(() => _subject.Write("Value1"));
            Assert.That(ex.Message, Is.EqualTo("File is closed or hasn't been opened in write mode."));
        }

        [Test]
        public void Should_ReadThrowException_WhenReaderWriterIsClosed()
        {
            // Arrange
            _subject.Open("TestFileName.csv", CSVReaderWriter.Mode.Read);
            _subject.Close();

            // Act

            // Assert
            var ex = Assert.Throws<ApplicationException>(() => _subject.Read(out var column1, out var column2));
            Assert.That(ex.Message, Is.EqualTo("File is closed or hasn't been opened in read mode."));
        }

        [Test]
        public void Should_WriteThrowException_WhenReaderWriterIsClosed()
        {
            // Arrange
            _subject.Open("TestFileName.csv", CSVReaderWriter.Mode.Write);
            _subject.Close();

            // Act

            // Assert
            var ex = Assert.Throws<ApplicationException>(() => _subject.Write("Value1"));
            Assert.That(ex.Message, Is.EqualTo("File is closed or hasn't been opened in write mode."));
        }
    }
}
