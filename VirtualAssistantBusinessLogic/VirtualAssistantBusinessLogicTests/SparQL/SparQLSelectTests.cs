using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using VirtualAssistantBusinessLogic.KnowledgeGraph;
using VirtualAssistantBusinessLogic.SparQL;

namespace VirtualAssistantBusinessLogicTests.SparQL
{
    [TestFixture]
    public class SparQLSelectTests
    {
        private ISPOEncoder subSPOEncoder;

        [SetUp]
        public void SetUp()
        {
            this.subSPOEncoder = Substitute.For<ISPOEncoder>();
        }

        private SparQLSelect CreateSparQLSelect()
        {
            return new SparQLSelect(
                this.subSPOEncoder);
        }

        [Test]
        public void Select_OneInputValue_SingleValueSavedCorrectly()
        {
            // Arrange
            var sparQLSelect = this.CreateSparQLSelect();
            string[] input = { "test" };


            // Act
            _ = sparQLSelect.Select(input);

            // Assert
            Assert.Contains(input[0], sparQLSelect.Selects);
        }

        [Test]
        public void Select_700InputValue_AllValuesSavedCorrectly()
        {
            // Arrange
            var sparQLSelect = this.CreateSparQLSelect();
            List<string> input = new();

            for (int i = 0; i < 700; i++)
            {
                input.Add(i.ToString());
            }

            // Act
            foreach(string s in input){
                sparQLSelect.Select(s);
            }

            // Assert
            foreach(string s in input)
            {
                Assert.Contains(s, sparQLSelect.Selects);
            }
        }

        [Test]
        public void Where_SelectsAreZero_ThrowsCountZeroException()
        {
            // Arrange
            var sparQLSelect = this.CreateSparQLSelect();

            // Act // Assert
            Assert.Throws<CountZeroException>(() => sparQLSelect.Where());
        }

        [Test]
        public void Where_SelectsHasValues_CorrectObjectReturned()
        {
            // Arrange
            var sparQLSelect = this.CreateSparQLSelect();
            sparQLSelect.Select("Test1", "Test2", "Test3");
            SparQLWhere actual = sparQLSelect.Where();

            // Act // Assert
            Assert.IsTrue(actual.SparQLSelect.Equals(sparQLSelect));
        }

        [Test]
        public void ToString_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sparQLSelect = this.CreateSparQLSelect();
            EncodedSPO output = new ($"?s1 ?p \"SUBJECT\"@en . ", $"?s1");
            sparQLSelect.SPOEncoder
                .EncodeSubject(Arg.Any<string>())
                .Returns(output);
            string expected = "SELECT ?s1 ?VALUELabel ";

            // Act
            sparQLSelect.From("SUBJECT");
            sparQLSelect.Select("VALUE");
            string actual = sparQLSelect.ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
