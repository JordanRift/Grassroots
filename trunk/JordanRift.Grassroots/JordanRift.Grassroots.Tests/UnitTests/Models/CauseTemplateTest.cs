//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Tests.Fakes;

namespace JordanRift.Grassroots.Tests.UnitTests.Models
{
	[TestFixture]
	public class CauseTemplateTest
	{
		private ICauseTemplateRepository repository;
	    private CauseTemplate causeTemplate;

		[SetUp]
		public void SetUp()
		{
			repository = new FakeCauseTemplateRepository();
            ((FakeCauseTemplateRepository)repository).SetUpRepository();
		    causeTemplate = EntityHelpers.GetValidCauseTemplate();
		}

		[Test]
		public void FindAllCauseTemplates_Should_Return_More_Than_Zero_CauseTemplates()
		{
			var causeTemplates = repository.FindAllCauseTemplates();
			Assert.IsTrue( causeTemplates.Count() > 0 );
		}

        [Test]
        public void CreateCause_Should_Return_Cause()
        {
            var cause = causeTemplate.CreateCause();
            Assert.IsNotNull(cause);
            Assert.IsInstanceOf(typeof(Cause), cause);
        }

        [Test]
        public void CreateCause_Should_Generate_Cause_With_The_Same_Name()
        {
            var cause = causeTemplate.CreateCause();
            Assert.AreEqual(causeTemplate.Name, cause.Name);
        }

        [Test]
        public void CreateCause_Should_Generate_Cause_With_Same_Summary()
        {
            var cause = causeTemplate.CreateCause();
            Assert.AreEqual(causeTemplate.Summary, cause.Summary);
        }

        [Test]
        public void CreateCause_Should_Generate_Cause_With_Same_DescriptionHtml()
        {
            var cause = causeTemplate.CreateCause();
            Assert.AreEqual(causeTemplate.DescriptionHtml, cause.DescriptionHtml);
        }

        [Test]
        public void CreateCause_Should_Generate_Cause_With_Same_ImagePath()
        {
            var cause = causeTemplate.CreateCause();
            Assert.AreEqual(causeTemplate.ImagePath, cause.ImagePath);
        }

        [Test]
        public void CreateCause_Should_Generate_Cause_With_Same_VideoEmbedHtml()
        {
            var cause = causeTemplate.CreateCause();
            Assert.AreEqual(causeTemplate.VideoEmbedHtml, cause.VideoEmbedHtml);
        }
	}
}