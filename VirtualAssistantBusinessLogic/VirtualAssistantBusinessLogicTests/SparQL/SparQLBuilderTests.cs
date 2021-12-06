using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using VirtualAssistantBusinessLogic.KnowledgeGraph;
using VirtualAssistantBusinessLogic.SparQL;

namespace VirtualAssistantBusinessLogicTests.SparQL
{
    [TestFixture]
    public class SparQLBuilderTests
    {
        private ISPOEncoder subSPOEncoder;


        [SetUp]
        public void SetUp()
        {

            this.subSPOEncoder = Substitute.For<ISPOEncoder>();

            this.subSPOEncoder.EncodeSubject(Arg.Any<string>())
                .Returns(new EncodedSPO("_s_", "_sn_"));
            this.subSPOEncoder.EncodePredicate(Arg.Any<string>())
               .Returns(new EncodedSPO("_p_", "_pn_"));
        }

        [Test]
        public void Build_PersonSparQLBuilder_HasCorrectParameters()
        {
            PersonSparQLBuilder personSparQLBuilder = new(subSPOEncoder);
            personSparQLBuilder.Query = "Test";
            var resultTemplate = personSparQLBuilder.Build();
            Assert.IsTrue(resultTemplate.Contains("Occupation"));
            Assert.IsTrue(resultTemplate.Contains("birth_name"));
            Assert.IsTrue(resultTemplate.Contains("date_of_birth"));
            Assert.IsTrue(resultTemplate.Contains("Spouse"));
        }

        [Test]
        public void Build_CountrySparQLBuilder_HasCorrectParameters()
        {
            CountrySparQLBuilder countrySparQLBuilder = new(subSPOEncoder);
            countrySparQLBuilder.Query = "Test";
            var resultTemplate = countrySparQLBuilder.Build();
            Assert.IsTrue(resultTemplate.Contains("Continent"));
            Assert.IsTrue(resultTemplate.Contains("Official_language"));
            Assert.IsTrue(resultTemplate.Contains("Capital"));
            Assert.IsTrue(resultTemplate.Contains("Population"));
        }

    }
}
