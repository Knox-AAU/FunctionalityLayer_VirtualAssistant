using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using VirtualAssistantBusinessLogic.KnowledgeGraph;
using VirtualAssistantBusinessLogic.SparQL;

namespace VirtualAssistantBusinessLogicTests.SparQL
{
    [TestFixture]
    public class SparQLWhereTests
    {
        private SparQLSelect subSparQLSelect;
        private ISPOEncoder subSPOEncoder;

        [SetUp]
        public void SetUp()
        {
            EncodedSPO output = new("?s1 ?p \"SUBJECT\"@en . ", "?s1");
            subSPOEncoder = Substitute.For<ISPOEncoder>();
            subSPOEncoder
                .EncodeSubject(Arg.Any<string>())
                .Returns(output);

            subSparQLSelect = Substitute.For<SparQLSelect>(subSPOEncoder);
        }

        private SparQLWhere CreateSparQLWhere()
        {
            return new SparQLWhere(
                this.subSparQLSelect);
        }

        [Test]
        public void SubjectIs_HasEncodedSPO_SubjectStringCorrect()
        {
            // Arrange
            SparQLWhere sparQLWhere = this.CreateSparQLWhere();
            sparQLWhere.EncodedSPOs.Add("TEST", new EncodedSPO("TRIPLET", "NAME"));
            string subject = "TEST";


            // Act
            _ = sparQLWhere.SubjectIs(
                subject);

            // Assert
            Assert.AreEqual("NAME", sparQLWhere.SubjectString);
        }

        [Test]
        public void PredicateIs_HasEncodedSPO_PredicateStringCorrect()
        {
            // Arrange
            SparQLWhere sparQLWhere = this.CreateSparQLWhere();
            sparQLWhere.EncodedSPOs.Add("PREDICATE", new EncodedSPO("TRIPLET", "NAME"));
            string predicate = "PREDICATE";


            // Act
            _ = sparQLWhere.PredicateIs(
                predicate);

            // Assert
            Assert.AreEqual("NAME", sparQLWhere.PredicateString);
        }

        [Test]
        public void PredicateIs_NoEncodedSPO_PredicateStringCorrect()
        {
            // Arrange
            SparQLWhere sparQLWhere = this.CreateSparQLWhere();
            string predicate = "PREDICATE";


            // Act // Assert
            Assert.Throws<KeyNotFoundException>(
                () => sparQLWhere.PredicateIs(predicate)
                );
        }

        [Test]
        public void ObjectIs_HasEncodedSPO_PredicateStringCorrect()
        {
            // Arrange
            SparQLWhere sparQLWhere = this.CreateSparQLWhere();
            sparQLWhere.EncodedSPOs.Add("TEST", new EncodedSPO("TRIPLET", "NAME"));
            string obj = "OBJ";


            // Act
            _ = sparQLWhere.GetObjectIn(
                obj);

            // Assert
            Assert.AreEqual("?OBJ", sparQLWhere.ObjectString);
        }

    }
}
