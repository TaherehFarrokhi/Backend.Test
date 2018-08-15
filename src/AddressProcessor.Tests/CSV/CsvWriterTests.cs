using System;
using System.IO;
using AddressProcessing.CSV;
using Moq;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    public class CsvWriterTests
    {

        [Test]
        public void Should_WriteColums_ToFile()
        {
            //Arrange
            var fileProviderMock = new Mock<IFileProvider>();
            var stringWriter = new StringWriter();
            fileProviderMock.Setup(x => x.GetStreamWriter(It.IsAny<string>())).Returns(stringWriter);

            var subject = new CsvWriter(fileProviderMock.Object, "testFilename.csv");


            //Act
            subject.Write("Value1", "Value2", "Value3");

            //Assert
            var actual = $"Value1\tValue2\tValue3{Environment.NewLine}";
            Assert.AreEqual(actual, stringWriter.ToString());

        }
    }
}