//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
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
            //((FakeCauseTemplateRepository)repository).SetUpRepository();
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