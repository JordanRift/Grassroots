//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;

namespace JordanRift.Grassroots.Web.Controllers
{
	public class UserProfileController : GrassrootsControllerBase
	{
		private readonly IUserProfileRepository repository;
		private readonly IUserProfileMailer mailer;

		public UserProfileController(IUserProfileRepository repository, IUserProfileMailer mailer)
		{
			this.repository = repository;
			this.mailer = mailer;
			Mapper.CreateMap<UserProfile, UserProfileDetailsModel>();
			Mapper.CreateMap<Campaign, CampaignDetailsModel>();
		}

		[Authorize]
		public ActionResult Index()
		{
			var userProfile = repository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

			if (userProfile != null)
			{
				// TODO: Map campaigns to a viewmodel
				//ViewData["Campaigns"] = userProfile.Campaigns;
				var viewModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
				viewModel.Campaigns = userProfile.Campaigns.Select( Mapper.Map<Campaign, CampaignDetailsModel> ).ToList();
				viewModel.TotalRaised = userProfile.CalculateTotalDonations();
				viewModel.TotalHoursServed = userProfile.CalculateTotalHoursServed();
				viewModel.TotalDonationsMade = userProfile.CalculateTotalNumberOfDonationsMade();
				viewModel.TotalDonationsGiven = userProfile.CalculateTotalDonationsGiven();
				viewModel.TotalNumberCampaignsDonatedTo = userProfile.CalculateTotalNumberOfCampaignsDonatedTo();
				return View(viewModel);
			}

			return HttpNotFound("The User Profile you are looking for could not be found.");
		}

		[Authorize]
		public ActionResult Edit()
		{
			var userProfile = repository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

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
				var userProfile = repository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

				if (userProfile == null)
				{
					return HttpNotFound("The User Profile you are looking for could not be found.");
				}

				if (ModelState.IsValid)
				{
					Map(userProfile, model);
					repository.Save();
					return RedirectToAction("Index");
				}

				TempData["UserProfileDetailsModel"] = model;
				return RedirectToAction("Edit");
			}
		}

		[Authorize]
		[HttpPost]
		public ActionResult Deactivate()
		{
			var userProfile = repository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

			if (userProfile == null)
			{
				return HttpNotFound("The User Profile you are looking for could not be found.");
			}

			userProfile.Active = false;
			
			foreach (var user in userProfile.Users)
			{
				user.IsActive = false;
			}

			repository.Save();
			var mailerModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
			mailer.TaxInfo(mailerModel).SendAsync();
			return RedirectToAction("LogOff", "Account");
		}

		[Authorize]
		[HttpPost]
		public ActionResult Reactivate()
		{
			var userProfile = repository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

			if (userProfile == null)
			{
				return HttpNotFound("The User Profile you are looking for could not be found.");
			}

			userProfile.Active = true;

			foreach (var user in userProfile.Users)
			{
				user.IsActive = true;
			}

			repository.Save();
			var mailerModel = Mapper.Map<UserProfile, UserProfileDetailsModel>(userProfile);
			mailer.WelcomeBack(mailerModel).SendAsync();
			TempData["UserFeedback"] = "Welcome back! We're glad you're with us again!";
			return RedirectToAction("Index");
		}

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
	}
}
