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

        [Test]
        public void Decode_ChrisEvansStreamIsGiven_KeysAndValuesAreAdded()
        {
            // Arrange
            XMLResponseDecoder xMLResponseDecoder = CreateXMLResponseDecoder();
            Stream stream = CreateChrisEvansFileStream();

            Dictionary<string, Dictionary<string, List<string>>> ExpectedResult = new();
            ExpectedResult.Add("wd:Q2964710", new());
            ExpectedResult.Add("wd:Q108329853", new());
            ExpectedResult.Add("wd:Q21538587", new());
            ExpectedResult.Add("wd:Q5392352", new());

            ExpectedResult["wd:Q2964710"].Add("Type", new() { "human" });
            ExpectedResult["wd:Q2964710"].Add("Spouse", new() { "Billie Piper", "Carol McGiffin", "Natasha Shishmanian" });

            ExpectedResult["wd:Q108329853"].Add("Type", new() { "human" });
            ExpectedResult["wd:Q108329853"].Add("date_of_birth", new() { "1961-03-11T00:00:00Z" });

            ExpectedResult["wd:Q21538587"].Add("Type", new() { "human" });
            ExpectedResult["wd:Q2964710"].Add("Occupation", new() { "visual artist" });

            ExpectedResult["wd:Q5392352"].Add("Type", new() { "human" });
            ExpectedResult["wd:Q5392352"].Add("Occupation", new() { "animator" });

            // Act
            Dictionary<string, Dictionary<string, List<string>>> result = xMLResponseDecoder.Decode(stream);

            // Assert
            foreach (var id in ExpectedResult)
            {
                foreach (var kvp in id.Value)
                {
                    Assert.Contains(kvp.Key, result[id.Key].Keys);
                    Assert.Contains(kvp.Value, result[id.Key].Values);
                }
            }
        }
    }
}
