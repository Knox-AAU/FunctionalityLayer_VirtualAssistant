using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VirtualAssistantBusinessLogic.SparQL;

namespace VirtualAssistantBusinessLogicTests.SparQL
{
    [TestFixture]
    public class XMLResponseDecoderTests
    {


        [SetUp]
        public void SetUp()
        {

        }

        private XMLResponseDecoder CreateXMLResponseDecoder()
        {
            return new XMLResponseDecoder();
        }

        private FileStream CreateChrisEvansFileStream()
        {

            return File.OpenRead("../../../TestFiles/ChrisEvans.xml");
        }

        [Test]
        public void Decode_ChrisEvansStreamIsGiven_ResultsArrayIsFilledAndIncludesCertainEvans()
        {
            // Arrange
            XMLResponseDecoder xMLResponseDecoder = CreateXMLResponseDecoder();
            Stream stream = CreateChrisEvansFileStream();
            List<String> ExpectedResult = new()
            {
                "wd:Q2964710",
                "wd:Q108329853",
                "wd:Q21538587",
                "wd:Q5392352"
            };
            // Act
            Dictionary<string, Dictionary<string, List<string>>> result = xMLResponseDecoder.Decode(stream);

            // Assert

            Assert.That(result.Count == 19);
            foreach (var expected in ExpectedResult)
            {
                Assert.Contains(expected, result.Keys);
            }
        }
    }
}
