//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Tests.Helpers;
using NUnit.Framework;
using JordanRift.Grassroots.Framework.Services;
using System;

namespace JordanRift.Grassroots.Tests.IntegrationTests.Models
{
    [TestFixture]
    public class CauseRepositoryTests
    {
        private ICauseRepository causeRepository;
        private IOrganizationRepository organizationRepository;

        private Cause cause;
        private CauseTemplate causeTemplate;
        private Organization organization;

		private UserProfile userProfile;
		private User user;

        [SetUp]
        public void SetUp()
        {
            causeRepository = new CauseRepository();
            organizationRepository = new OrganizationRepository();
        }

        [Test]
        public void Add_Should_Add_Cause_To_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTest();
                var id = cause.CauseID;
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void GetCauseByID_Should_Load_Cause_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTest();
                var id = cause.CauseID;
                cause = null;
                cause = causeRepository.GetCauseByID(id);
                Assert.IsNotNull(cause);
                Assert.Greater(id, 0);
            }
        }

        [Test]
        public void GetCauseByID_Should_Return_Null_When_CauseID_Not_Found()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTest();
                var id = cause.CauseID + 1;
                var result = causeRepository.GetCauseByID(id);
                Assert.IsNull(result);
            }
        }

        [Test]
        public void FindAllCauses_Should_Return_List()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTest();
                var results = causeRepository.FindAllCauses();
                Assert.IsNotNull(results);
                Assert.IsNotEmpty(results.ToList());
            }
        }

        [Test]
        public void FindActiveCauses_Should_Return_List()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTest();
                var results = causeRepository.FindActiveCauses();
                Assert.IsNotNull(results);
                Assert.IsNotEmpty(results.ToList());
            }
        }

        [Test]
        public void FindCausesByCauseTemplateID_Should_Return_List()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTest();
                var id = causeTemplate.CauseTemplateID;
                var results = causeRepository.FindCausesByCauseTemplateID(id);
                Assert.IsNotNull(results);
                Assert.IsNotEmpty(results.ToList());
            }
        }

        [Test]
        public void Delete_Should_Delete_Cause_From_Database()
        {
            using (new UnitOfWorkScope())
            using (new TransactionScope())
            {
                ArrangeCauseTest();
                var id = cause.CauseID;
                causeRepository.Delete(cause);
                causeRepository.Save();
                cause = causeRepository.GetCauseByID(id);
                Assert.IsNull(cause);
            }
        }

		//TODO Verify this works
		[Test]
		public void Add_Should_Add_CauseNote_To_Database()
		{
			using ( new UnitOfWorkScope() )
			using ( new TransactionScope() )
			{
				ArrangeCauseTest();
				CauseNote note = cause.CreateNote();

				note.Text = "This is a test note.";
				note.UserProfile = organization.UserProfiles.FirstOrDefault();
				note.EntryDate = DateTime.Now;

				causeRepository.AddNote( note );
				causeRepository.Save();
				Assert.IsNotNull( cause.CauseNotes );
				Assert.Greater( cause.CauseNotes.Count, 0 );
			}
		}

        private void ArrangeCauseTest()
        {
            organization = EntityHelpers.GetValidOrganization();
			organization.UserProfiles = new List<UserProfile>();
            organization.CauseTemplates = new List<CauseTemplate>();
            organization.Causes = new List<Cause>();
            organizationRepository.Add(organization);
            organizationRepository.Save();

			userProfile = EntityHelpers.GetValidUserProfile();
			userProfile.Users = new List<User>();
			userProfile.UserProfileService = new UserProfileService( new UserProfileRepository() );
			organization.UserProfiles.Add( userProfile );
			organizationRepository.Save();

			user = EntityHelpers.GetValidUser();
			userProfile.Users.Add( user );
			organizationRepository.Save();

            causeTemplate = EntityHelpers.GetValidCauseTemplate();
            causeTemplate.Causes = new List<Cause>();
            organization.CauseTemplates.Add(causeTemplate);
            organizationRepository.Save();

            cause = causeTemplate.CreateCause();
            causeTemplate.Causes.Add(cause);
            organization.Causes.Add(cause);
            causeRepository.Save();
        }
    }
}
