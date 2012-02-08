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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;

namespace JordanRift.Grassroots.Web.Controllers
{
	public class UserProfileController : GrassrootsControllerBase
	{
	    private const string ADMIN_ROLES = "Root,Administrator";
		private readonly IUserProfileRepository userProfileRepository;
	    private readonly IRoleRepository roleRepository;
		private readonly ICauseRepository causeRepository;
		private readonly IUserProfileMailer mailer;

        public UserProfileController(IUserProfileRepository userProfileRepository, IRoleRepository roleRepository, 
            ICauseRepository causeRepository, IUserProfileMailer mailer)
		{
			this.userProfileRepository = userProfileRepository;
            this.roleRepository = roleRepository;
            this.causeRepository = causeRepository;
			this.mailer = mailer;
			Mapper.CreateMap<UserProfile, UserProfileDetailsModel>();
			Mapper.CreateMap<Campaign, CampaignDetailsModel>();
			Mapper.CreateMap<Cause, CauseDetailsModel>();
            Mapper.CreateMap<CampaignDonor, DonationDetailsModel>();
            Mapper.CreateMap<UserProfile, UserProfileAdminModel>();
            Mapper.CreateMap<UserProfileAdminModel, UserProfile>();
		}

        ~UserProfileController()
        {
            userProfileRepository.Dispose();
            causeRepository.Dispose();
        }

		[Authorize]
		public ActionResult Index(int id = -1)
		{
            using (userProfileRepository)
            {
                var userProfile = id != -1
                    ? userProfileRepository.GetUserProfileByID(id)
                    : userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile != null)
                {
                    var viewModel = MapUserProfileDetails(userProfile);
                    return View(viewModel);
                }
            }

		    return HttpNotFound("The User Profile you are looking for could not be found.");
		}

		[Authorize]
		public ActionResult Edit()
		{
            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile != null)
                {
                    UserProfileDetailsModel viewModel;

                    if (TempData["UserProfileDetailsModel"] != null)
                    {
                        viewModel = TempData["UserProfileDetailsModel"] as UserProfileDetailsModel;
                    }
                    else
                    {
                        viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
                    }

                    return View(viewModel);
                }
            }

		    return HttpNotFound("The User Profile you are looking for could not be found.");
		}

		[HttpPost]
		[Authorize]
        [ValidateAntiForgeryToken(Salt = "UserProfileEdit")]
		public ActionResult Update(UserProfileDetailsModel model)
		{
			using (new UnitOfWorkScope())
			{
				var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

				if (userProfile == null)
				{
					return HttpNotFound("The User Profile you are looking for could not be found.");
				}

				if (ModelState.IsValid)
				{
					Map(userProfile, model);
					userProfileRepository.Save();
					return RedirectToAction("Index");
				}

				TempData["UserProfileDetailsModel"] = model;
				return RedirectToAction("Edit");
			}
		}

        [Authorize]
        public ActionResult DeactivateAccount()
        {
            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile == null)
                {
                    return HttpNotFound("The User Profile you are looking for could not be found.");
                }
            }

            return View();
        }

		[Authorize]
		[HttpPost]
        [ValidateAntiForgeryToken(Salt = "UserProfileDeactivate")]
		public ActionResult Deactivate()
		{
            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile == null)
                {
                    return HttpNotFound("The User Profile you are looking for could not be found.");
                }

                userProfile.Active = false;
                var organization = userProfile.Organization;

                foreach (var user in userProfile.Users)
                {
                    user.IsActive = false;
                }

                userProfileRepository.Save();
                mailer.TaxInfo(new DeactivateModel
                                   {
                                       Email = userProfile.Email,
                                       FirstName = userProfile.FirstName,
                                       FacebookUrl = organization.FacebookPageUrl,
                                       TwitterUrl = string.Format("http://twitter.com/{0}", organization.TwitterName.Replace("@", "")),
                                       BlogUrl = organization.BlogRssUrl
                                   }).SendAsync();
            }

            TempData["UserFeedback"] = "Welcome back! We're glad you're with us again!";
			return RedirectToAction("LogOff", "Account");
		}

        [Authorize]
        public ActionResult ReactivateAccount()
        {
            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile == null)
                {
                    return HttpNotFound("The User Profile you are looking for could not be found.");
                }
            }

            return View();
        }

		[Authorize]
		[HttpPost]
        [ValidateAntiForgeryToken(Salt = "UserProfileReactivate")]
		public ActionResult Reactivate()
		{
            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile == null)
                {
                    return HttpNotFound("The User Profile you are looking for could not be found.");
                }

                userProfile.Active = true;

                foreach (var user in userProfile.Users)
                {
                    user.IsActive = true;
                }

                userProfileRepository.Save();
                var mailerModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
                mailer.WelcomeBack(mailerModel).SendAsync();
            }

		    TempData["UserFeedback"] = "Welcome back! We're glad you're with us again!";
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Action to list the users causes.
		/// </summary>
		/// <param name="id">a user profile ID</param>
		/// <returns></returns>
		public ActionResult Projects( int id = -1 )
		{
            using (userProfileRepository)
            {
                var userProfile = id != -1
                    ? userProfileRepository.GetUserProfileByID(id)
                    : userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile != null)
                {
                    var causes = causeRepository.FindCausesByUserProfileID(userProfile.UserProfileID);
                    var model = new UserProfileProjectsModel
                                    {
                                        UserProfileID = userProfile.UserProfileID,
                                        FirstName = userProfile.FirstName,
                                        Causes = causes.Select(Mapper.Map<Cause, CauseDetailsModel>)
                                    };

                    return View("Projects", model);
                }
            }

		    return HttpNotFound( "The person you are looking for could not be found." );
		}

        public ActionResult Raised(int id = -1)
        {
            using (userProfileRepository)
            {
                var userProfile = id != -1
                    ? userProfileRepository.GetUserProfileByID(id)
                    : userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile != null)
                {
                    var donations = from c in userProfile.Campaigns
                                    from d in c.CampaignDonors
                                    select d;

                    var model = new UserProfileRaisedModel
                                    {
                                        UserProfileID = userProfile.UserProfileID,
                                        FirstName = userProfile.FirstName,
                                        DollarsRaised = userProfile.CalculateTotalDonations(),
                                        Donations = MapDonations(donations)
                                    };

                    return View(model);
                }
            }

            return HttpNotFound("The person you are looking for could not be found.");
        }

        public ActionResult Given(int id = -1)
        {
            using (userProfileRepository)
            {
                var userProfile = id != -1
                    ? userProfileRepository.GetUserProfileByID(id)
                    : userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile != null)
                {
                    var model = new UserProfileRaisedModel
                                    {
                                        UserProfileID = userProfile.UserProfileID,
                                        FirstName = userProfile.FirstName,
                                        DollarsRaised = userProfile.CalculateTotalDonations(),
                                        Donations = MapDonations(userProfile.CampaignDonors)
                                    };

                    return View(model);
                }
            }

            return HttpNotFound("The person you are looking for could not be found.");
        }

		#region Mapping Stuff

		private static void Map(UserProfile userProfile, UserProfileDetailsModel viewModel)
		{
			userProfile.FirstName = viewModel.FirstName;
			userProfile.LastName = viewModel.LastName;
			userProfile.Birthdate = viewModel.Birthdate;
			userProfile.PrimaryPhone = viewModel.PrimaryPhone;
			userProfile.Gender = viewModel.Gender;
			userProfile.AddressLine1 = viewModel.AddressLine1;
			userProfile.AddressLine2 = viewModel.AddressLine2;
			userProfile.City = viewModel.City;
			userProfile.State = viewModel.State;
			userProfile.ZipCode = viewModel.ZipCode;
		}

        private UserProfileDetailsModel MapUserProfileDetails(UserProfile userProfile)
        {
            var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
            var causes = causeRepository.FindCausesByUserProfileID(userProfile.UserProfileID);
            viewModel.Campaigns = userProfile.Campaigns
                     .Select(Mapper.Map<Campaign, CampaignDetailsModel>)
                     .OrderByDescending(c => c.EndDate).ToList();
            viewModel.ActiveCampaignCount = userProfile.Campaigns.Where(c => c.IsActive).Count();
            viewModel.ImagePath = userProfile.GetProfileImagePath(ProfileImageSize.Full);
            viewModel.DollarsRaised = userProfile.CalculateTotalDonations();
            viewModel.DollarsGiven = userProfile.CalculateTotalDonationsGiven();
            viewModel.ProjectsCompleted = causes.Count();
            viewModel.ProjectsCompletedLabel = ModelHelpers.GetCausesLabelText(causes);
            viewModel.LastVisit = userProfile.Users.Any() ? userProfile.Users.First().LastLoggedIn : DateTime.Now;
            viewModel.Role = userProfile.Role != null ? userProfile.Role.Description : "Registered User";
            viewModel.CurrentUserIsOwner = ((User != null) && (userProfile.Email.ToLower() == User.Identity.Name.ToLower()));
            return viewModel;
		}

        private IEnumerable<DonationDetailsModel> MapDonations(IEnumerable<CampaignDonor> donations)
        {
            var models = new List<DonationDetailsModel>();

            foreach (var donation in donations)
            {
                models.Add(new DonationDetailsModel
                               {
                                   Amount = donation.Amount,
                                   Comments = donation.Comments,
                                   DonationDate = donation.DonationDate,
                                   FirstName = donation.FirstName,
                                   LastName = donation.LastName,
                                   Title = donation.Campaign.Title,
                                   UrlSlug = donation.Campaign.UrlSlug,
                                   UserProfileID = donation.UserProfile != null ? donation.UserProfileID : null
                               });
            }

            return models;
        }

		#endregion

#region Admin

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult List()
        {
            // TODO: Add paging support...
            using (userProfileRepository)
            {
                var userProfiles = userProfileRepository.FindAllUserProfiles();
                var models = from userProfile in userProfiles
                             let now = DateTime.Now
                             let campaign = userProfile.Campaigns.FirstOrDefault(c => now > c.StartDate && now < c.EndDate)
                             select new UserProfileAdminModel
                                        {
                                            UserProfileID = userProfile.UserProfileID,
                                            FirstName = userProfile.FirstName,
                                            LastName = userProfile.LastName,
                                            Email = userProfile.Email,
                                            Active = userProfile.Active,
                                            ActiveCampaignID = campaign != null ? campaign.CampaignID : -1,
                                            ActiveCampaignName = campaign != null ? campaign.Title : null
                                        };

                return View(models.ToList());
            }
        }

        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult Admin(int id = -1)
        {
            UserProfileAdminModel model;

            if (TempData["ModelErrors"] != null)
            {
                var errors = TempData["ModelErrors"] as IEnumerable<string> ?? new List<string>();

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }

                model = TempData["UserProfileAdminModel"] as UserProfileAdminModel;
            }
            else
            {
                using (userProfileRepository)
                {
                    var userProfile = userProfileRepository.GetUserProfileByID(id);

                    if (userProfile == null)
                    {
                        return HttpNotFound("The person you are looking for could not be found.");
                    }

                    model = Mapper.Map<UserProfile, UserProfileAdminModel>(userProfile);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "AdminUpdateUserProfile")]
        [Authorize(Roles = ADMIN_ROLES)]
        public ActionResult AdminUpdate(UserProfileAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelErrors"] = FindModelErrors();
                TempData["UserProfileAdminModel"] = model;
                return RedirectToAction("Admin");
            }

            using (new UnitOfWorkScope())
            {
                var userProfile = userProfileRepository.GetUserProfileByID(model.UserProfileID);

                if (userProfile == null)
                {
                    return HttpNotFound("The person you are looking for could not be found.");
                }

                MapUserProfileAdmin(model, userProfile);
                MapRole(model, userProfile);
                userProfileRepository.Save();
                TempData["UserFeedback"] = string.Format("{0}'s profile has been saved.", userProfile.FullName);
            }
            

            return RedirectToAction("List");
        }

        private static void MapUserProfileAdmin(UserProfileAdminModel model, UserProfile userProfile)
        {
            userProfile.FirstName = model.FirstName;
            userProfile.LastName = model.LastName;
            userProfile.AddressLine1 = model.AddressLine1;
            userProfile.AddressLine2 = model.AddressLine2;
            userProfile.City = model.City;
            userProfile.State = model.State;
            userProfile.ZipCode = model.ZipCode;
            userProfile.PrimaryPhone = model.PrimaryPhone;
            userProfile.Birthdate = model.Birthdate;
            userProfile.Gender = model.Gender;
            userProfile.Active = model.Active;
            userProfile.IsActivated = model.IsActivated;

            if (userProfile.Email != model.Email)
            {
                userProfile.Email = model.Email;
                var user = userProfile.Users.FirstOrDefault();
                
                if (user != null)
                {
                    user.Username = model.Email;
                }
            }
        }

        private void MapRole(UserProfileAdminModel model, UserProfile userProfile)
        {
            Role role;

            if (model.RoleID.HasValue)
            {
                role = roleRepository.GetRoleByID(model.RoleID.Value);

                if (role == null)
                {
                    return;
                }

                role.UserProfiles.Add(userProfile);
            }
            else
            {
                if (userProfile.RoleID.HasValue)
                {
                    role = roleRepository.GetRoleByID(userProfile.RoleID.Value);
                    role.UserProfiles.Remove(userProfile);
                }
            }
        }

#endregion
    }
}
