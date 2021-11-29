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

        private static XMLResponseDecoder CreateXMLResponseDecoder()
        {
            return new XMLResponseDecoder();
        }

        private static FileStream CreateChrisEvansFileStream()
        {

            return File.OpenRead("../../../TestFiles/ChrisEvans.xml");
        }

        [Test]
        public void Decode_ChrisEvansStreamIsGiven_ResultsArrayIsFilledAndIncludesCertainEvans()
        {
            // Arrange
            XMLResponseDecoder xmlResponseDecoder = CreateXMLResponseDecoder();
            Stream stream = CreateChrisEvansFileStream();
            List<String> ExpectedResult = new()
            {
                "wd:Q2964710",
                "wd:Q108329853",
                "wd:Q21538587",
                "wd:Q5392352"
            };
            // Act
            Dictionary<string, Dictionary<string, List<string>>> result = xmlResponseDecoder.Decode(stream);

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
            XMLResponseDecoder xmlResponseDecoder = CreateXMLResponseDecoder();
            Stream stream = CreateChrisEvansFileStream();

            Dictionary<string, Dictionary<string, List<string>>> ExpectedResult = new();
            string person1 = "wd:Q2964710",
                   person2 = "wd:Q108329853",
                   person3 = "wd:Q21538587",
                   person4 = "wd:Q5392352";

            ExpectedResult.Add(person1, new());
            ExpectedResult.Add(person2, new());
            ExpectedResult.Add(person3, new());
            ExpectedResult.Add(person4, new());

            ExpectedResult[person1].Add("Type", new() { "human" });
            ExpectedResult[person1].Add("Spouse", new() { "Billie Piper", "Carol McGiffin", "Natasha Shishmanian" });

            ExpectedResult[person2].Add("Type", new() { "human" });
            ExpectedResult[person2].Add("date_of_birth", new() { "1961-03-11T00:00:00Z" });

            ExpectedResult[person3].Add("Type", new() { "human" });
            ExpectedResult[person3].Add("Occupation", new() { "painter", "visual artist" });

            ExpectedResult[person4].Add("Type", new() { "human" });
            ExpectedResult[person4].Add("Occupation", new() { "animator" });

            // Act
            Dictionary<string, Dictionary<string, List<string>>> result = xmlResponseDecoder.Decode(stream);

            // Assert
            foreach (var id in ExpectedResult)
            {
                foreach (var kvp in id.Value)
                {
                    Assert.Contains(kvp.Key, result[id.Key].Keys); // All ID's exist in the dictionary
                    Assert.Contains(kvp.Value, result[id.Key].Values); // All values exist in the dictionary
                }
            }
        }
    }
}
