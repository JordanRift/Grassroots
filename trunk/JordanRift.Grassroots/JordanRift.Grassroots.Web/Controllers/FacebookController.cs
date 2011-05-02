//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Facebook;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class FacebookController : GrassrootsControllerBase
    {
        private readonly IUserProfileRepository userProfileRepository;
        
        public FacebookController(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        /// <summary>
        /// Creates OAuth client and sends request with needed permissions to Facebook.
        /// </summary>
        /// <param name="returnUrl">Url to redirect to after login is complete</param>
        /// <returns>Redirect to Facebook</returns>
        public ActionResult LogOn(string returnUrl)
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current) { RedirectUri = new Uri(GetOAuthRedirectUrl()) };
            var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl } });
            return Redirect(loginUri.AbsoluteUri);
        }

        /// <summary>
        /// Awaits a respnose from Facebook OAuth API. If Facebook login is successful, create Grassroots auth cookie.
        /// </summary>
        /// <param name="code">Code sent to Facebook to obtain access token</param>
        /// <param name="state">Redirect Url</param>
        /// <returns>Redirect based on outcome of login request</returns>
        public ActionResult OAuth(string code, string state)
        {
            FacebookOAuthResult oAuthResult;

            if (FacebookOAuthResult.TryParse(Request.Url, out oAuthResult))
            {
                if (oAuthResult.IsSuccess)
                {
                    var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current) { RedirectUri = new Uri(GetOAuthRedirectUrl()) };
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
                    string accessToken = tokenResult.access_token;

                    FacebookClient fbClient = new FacebookClient(accessToken);
                    dynamic me = fbClient.Get("me");
                    var userProfile = userProfileRepository.GetUserProfileByFacebookID(me.id);

                    if (userProfile == null)
                    {
                        TempData["UserFeedback"] = "We can't find you in our system. Please register to create your account.";
                        return RedirectToAction("Register", "Account");
                    }

                    FormsAuthentication.SetAuthCookie(userProfile.Email, false);

                    if (Url.IsLocalUrl(state))
                    {
                        return Redirect(state);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Log user out of Grassroots and notifies Facebook
        /// </summary>
        /// <returns>Redirect to home page</returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            var oAuthClient = new FacebookOAuthClient { RedirectUri = new Uri(GetLogOffUrl()) };
            var logoutUrl = oAuthClient.GetLogoutUrl();
            return Redirect(logoutUrl.AbsoluteUri);
        }

        /// <summary>
        /// Connect existing user account with Facebook account. Sends required permissions to Facebook and requests authorization.
        /// </summary>
        /// <param name="returnUrl">Return Url</param>
        /// <returns>Redirect to Facebook</returns>
        [Authorize]
        public ActionResult Connect(string returnUrl)
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current) { RedirectUri = new Uri(GetAccountConnectUrl()) };
            var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl } });
            return Redirect(loginUri.AbsoluteUri);
        }

        /// <summary>
        /// Awaits response from Facebook. Based on outcome, will connect existing user account to Facebook via Facebook user ID.
        /// </summary>
        /// <param name="code">Code sent to Facebook to obtain access token</param>
        /// <param name="state">Redirect Url</param>
        /// <returns>Redirect based on outcome of connection request</returns>
        [Authorize]
        public ActionResult ConnectAccount(string code, string state)
        {
            FacebookOAuthResult oAuthResult;
            bool success = false;

            if (FacebookOAuthResult.TryParse(Request.Url, out oAuthResult))
            {
                if (oAuthResult.IsSuccess)
                {
                    var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current) { RedirectUri = new Uri(GetAccountConnectUrl()) };
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
                    string accessToken = tokenResult.access_token;

                    FacebookClient fbClient = new FacebookClient(accessToken);
                    dynamic me = fbClient.Get("me");
                    var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                    if (userProfile != null && string.IsNullOrEmpty(userProfile.FacebookID))
                    {
                        userProfile.FacebookID = me.id;
                        userProfile.ImagePath = GetFacebookImagePath(me, userProfile.ImagePath);
                        userProfileRepository.Save();
                        success = true;

                        if (Url.IsLocalUrl(state))
                        {
                            return Redirect(state);
                        }
                    }
                }
            }

            if (!success)
            {
                TempData["UserFeedback"] = "Sorry, we were not able to connect with your Facebook account. Please try again.";
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Sends request to obtain user information with extended permissions to Facebook.
        /// </summary>
        /// <param name="returnUrl">Return Url</param>
        /// <returns>Redirect to Facebook</returns>
        public ActionResult Register(string returnUrl)
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current) { RedirectUri = new Uri(GetNewAccountUrl()) };
            var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object>
                                                       {
                                                           { "state", returnUrl },
                                                           { "scope", "email,user_location,user_birthday" } // FB extended permissions
                                                       });
            return Redirect(loginUri.AbsoluteUri);
        }

        /// <summary>
        /// Attempts to populate registration ViewModel with data from Facebook and present to user for verification.
        /// </summary>
        /// <param name="code">Code sent to Facebook to obtain access token</param>
        /// <param name="state">Redirect Url</param>
        /// <returns>Registration view pre-populated w/ data from Facebook</returns>
        public ActionResult NewAccount(string code, string state)
        {
            FacebookOAuthResult oAuthResult;
            var viewModel = new FacebookRegisterModel();

            if (FacebookOAuthResult.TryParse(Request.Url, out oAuthResult))
            {
                if (oAuthResult.IsSuccess)
                {
                    var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current) { RedirectUri = new Uri(GetNewAccountUrl()) };
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
                    string accessToken = tokenResult.access_token;

                    FacebookClient fbClient = new FacebookClient(accessToken);
                    dynamic me = fbClient.Get("me");
                    viewModel = MapFacebookUser(me);
                }
            }

            return View("FacebookRegister", viewModel);
        }

        /// <summary>
        /// Creates a UserProfile record based on data gathered from Facebook and verified by user.
        /// </summary>
        /// <param name="model">ViewModel populated with user registraiton data</param>
        /// <param name="returnUrl">Return Url</param>
        /// <returns>Redirect based on the outcome of account creation</returns>
        [HttpPost]
        public ActionResult RegisterUser(FacebookRegisterModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var userProfile = Mapper.Map<FacebookRegisterModel, UserProfile>(model);

                using (new UnitOfWorkScope())
                {
                    var organization = OrganizationRepository.GetDefaultOrganization();

                    if (organization.UserProfiles == null)
                    {
                        organization.UserProfiles = new List<UserProfile>();
                    }

                    organization.UserProfiles.Add(userProfile);
                    OrganizationRepository.Save();
                    FormsAuthentication.SetAuthCookie(userProfile.Email, false);
                }

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "UserProfile", new { id = userProfile.UserProfileID });
            }

            return RedirectToAction("Register");
        }

        private string GetLogOffUrl()
        {
            return Url.Action("Index", "Home", new { }, Request.Url != null ? Request.Url.Scheme : "https");
        }

        private string GetOAuthRedirectUrl()
        {
            return Url.Action("OAuth", "Facebook", new { }, Request.Url != null ? Request.Url.Scheme : "https");
        }

        private string GetAccountConnectUrl()
        {
            return Url.Action("ConnectAccount", "Facebook", new { }, Request.Url != null ? Request.Url.Scheme : "https");
        }

        private string GetNewAccountUrl()
        {
            return Url.Action("NewAccount", "Facebook", new { }, Request.Url != null ? Request.Url.Scheme : "https");
        }

        private static FacebookRegisterModel MapFacebookUser(dynamic me)
        {
            var viewModel = new FacebookRegisterModel
                                {
                                    FirstName = me.first_name,
                                    LastName = me.last_name,
                                    FacebookID = me.id,
                                    Gender = me.gender,
                                    Email = me.email,
                                    Birthdate = DateTime.Parse(me.birthday),
                                    ImagePath = GetFacebookImagePath(me)
                                };

			// If the user's FB location is not set, we can't do anything.
			if ( me.location != null )
			{
				string location = me.location.name;
				var locArray = location.Split( new[] { ',' } );
				viewModel.City = locArray[0].Trim();

				var pair = UIHelpers.StateDictionary.FirstOrDefault( s => s.Key.ToLower() == locArray[1].Trim().ToLower() );
				viewModel.State = pair.Value;
			}
            return viewModel;
        }

        private static string GetFacebookImagePath(dynamic me, string existingImagePath = "content/images/avatar.jpg")
        {
            var path = existingImagePath;

            if (me.link != null)
            {
                var link = me.link.ToString();
                var linkPart = link.IndexOf("?id=") > -1 // If no friendly url available for user...
                    ? link.Substring(link.LastIndexOf('=') + 1) // use query string 
                    : link.Substring(link.LastIndexOf('/') + 1);
                
                path = string.Format("https://graph.facebook.com/{0}/picture", linkPart);
            }

            return path;
        }
    }
}
