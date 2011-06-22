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
		private readonly IUserProfileRepository userProfileRepository;
		private readonly ICauseRepository causeRepository;
		private readonly IUserProfileMailer mailer;

        public UserProfileController(IUserProfileRepository userProfileRepository, ICauseRepository causeRepository, IUserProfileMailer mailer)
		{
			this.userProfileRepository = userProfileRepository;
            this.causeRepository = causeRepository;
			this.mailer = mailer;
			Mapper.CreateMap<UserProfile, UserProfileDetailsModel>();
			Mapper.CreateMap<Campaign, CampaignDetailsModel>();
			Mapper.CreateMap<Cause, CauseDetailsModel>();
            Mapper.CreateMap<CampaignDonor, DonationDetailsModel>();
		}

		[Authorize]
		public ActionResult Index(int id = -1)
		{
		    var userProfile = id != -1 
                ? userProfileRepository.GetUserProfileByID(id) 
                : userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();
			
			if (userProfile != null)
			{
			    var viewModel = MapUserProfileDetails(userProfile);
				return View(viewModel);
			}

			return HttpNotFound("The User Profile you are looking for could not be found.");
		}

		[Authorize]
		public ActionResult Edit()
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

			return HttpNotFound("The User Profile you are looking for could not be found.");
		}

		[HttpPost]
		[Authorize]
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
            var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

            if (userProfile == null)
            {
                return HttpNotFound("The User Profile you are looking for could not be found.");
            }

            return View();
        }

		[Authorize]
		[HttpPost]
		public ActionResult Deactivate()
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
                                   BlogUrl= organization.BlogRssUrl
			                   }).SendAsync();

            TempData["UserFeedback"] = "Welcome back! We're glad you're with us again!";
			return RedirectToAction("LogOff", "Account");
		}

        [Authorize]
        public ActionResult ReactivateAccount()
        {
            var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

            if (userProfile == null)
            {
                return HttpNotFound("The User Profile you are looking for could not be found.");
            }

            return View();
        }

		[Authorize]
		[HttpPost]
		public ActionResult Reactivate()
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
			var userProfile = id != -1 
                ? userProfileRepository.GetUserProfileByID( id ) 
                : userProfileRepository.FindUserProfileByEmail( User.Identity.Name ).FirstOrDefault();

			if ( userProfile != null )
			{
				var causes = causeRepository.FindCausesByUserProfileID( userProfile.UserProfileID );
				//var model = causes.Select( Mapper.Map<Cause, CauseDetailsModel> ).ToList();
			    var model = new UserProfileProjectsModel
			                    {
                                    UserProfileID = userProfile.UserProfileID,
                                    FirstName = userProfile.FirstName,
                                    Causes = causes.Select(Mapper.Map<Cause, CauseDetailsModel>)
			                    };

				return View("Projects", model);
			}

			return HttpNotFound( "The person you are looking for could not be found." );
		}

        public ActionResult Raised(int id = -1)
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

            return HttpNotFound("The person you are looking for could not be found.");
        }

        public ActionResult Given(int id = -1)
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
	}
}
