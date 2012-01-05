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
using System.Web.Security;
using AutoMapper;
using Facebook;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Helpers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using Mvc.Mailer;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class FacebookController : GrassrootsControllerBase
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IAccountMailer accountMailer;

        public FacebookController(IUserProfileRepository userProfileRepository, IAccountMailer accountMailer)
        {
            this.userProfileRepository = userProfileRepository;
            this.accountMailer = accountMailer;
			Mapper.CreateMap<FacebookRegisterModel, UserProfile>();
            Mapper.CreateMap<FacebookRegisterModel, RegisterModel>();
        }

        ~FacebookController()
        {
            userProfileRepository.Dispose();
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

                    using (userProfileRepository)
                    {
                        var userProfile = userProfileRepository.GetUserProfileByFacebookID(me.id);

                        if (userProfile == null)
                        {
                            TempData["UserFeedback"] = "We can't find you in our system. Please sign in or register to create your account.";
                            return RedirectToAction("Register", "Account");
                        }

                        if (!userProfile.IsActivated)
                        {
                            return RedirectToAction("AwaitingActivation", "Account");
                        }

                        FormsAuthentication.SetAuthCookie(userProfile.Email, false);
                    }

                    if (Url.IsLocalUrl(state))
                    {
                        return Redirect(state);
                    }
                }
            }

            return RedirectToAction("Index", "UserProfile");
        }

        /// <summary>
        /// Log user out of Grassroots and notifies Facebook. 
        /// NOTE: oAuthClient.GetLogoutUrl() is deprecated. The official practice is to use the JS SDK to logout from FB.
        /// </summary>
        /// <returns>Redirect to home page</returns>
        //public ActionResult LogOff()
        //{
        //    FormsAuthentication.SignOut();
        //    var oAuthClient = new FacebookOAuthClient { RedirectUri = new Uri(GetLogOffUrl()) };
        //    var logoutUrl = oAuthClient.GetLogoutUrl();
        //    return Redirect(logoutUrl.AbsoluteUri);
        //}

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

                    using (userProfileRepository)
                    {
                        var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                        if (userProfile != null && string.IsNullOrEmpty(userProfile.FacebookID))
                        {
                            userProfile.FacebookID = me.id;
                            userProfileRepository.Save();
                            success = true;

                            if (Url.IsLocalUrl(state))
                            {
                                return Redirect(state);
                            }
                        }
                    }
                }
            }

            if (!success)
            {
                TempData["UserFeedback"] = "Sorry, we were not able to connect with your Facebook account. Please try again.";
            }

            return RedirectToAction("Index", "UserProfile");
        }

        [Authorize]
        public ActionResult Disconnect()
        {
            if (TempData["ModelErrors"] != null)
            {
                var errors = TempData["ModelErrors"] as IEnumerable<string>;

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            using (userProfileRepository)
            {
                var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile == null)
                {
                    return HttpNotFound("The user you are looking for could not be found.");
                }

                var hasUser = userProfile.Users.Any();
                const string defaultPassword = "unnecessary";
                return View(new FacebookDisconnectModel
                            {
                                Email = userProfile.Email,
                                FullName = userProfile.FullName,
                                HasUserRecord = hasUser,
                                Password = hasUser ? defaultPassword : string.Empty,
                                ConfirmPassword = hasUser ? defaultPassword : string.Empty
                            });
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "FacebookDisconnect")]
        public ActionResult DisconnectAccount(FacebookDisconnectModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ModelErrors"] = FindModelErrors();
                return RedirectToAction("Disconnect");
            }

            using (new UnitOfWorkScope())
            {
                var userProfile = userProfileRepository.FindUserProfileByEmail(User.Identity.Name).FirstOrDefault();

                if (userProfile == null)
                {
                    return HttpNotFound("The user you are looking for could not be found.");
                }

                // If user does not have a User record attached to their UserProfile, we need to create one for them...
                if (userProfile.Users.Count == 0)
                {
                    userProfile.Users.Add(new User
                                              {
                                                  Username = userProfile.Email,
                                                  Password = GrassrootsMembershipService.HashPassword(model.Password, null),
                                                  IsActive = true,
                                                  IsAuthorized = true,
                                                  RegisterDate = DateTime.Now,
                                                  LastLoggedIn = DateTime.Now
                                              });
                }

                userProfile.FacebookID = null;
                userProfileRepository.Save();
            }
            
            TempData["UserFeedback"] = "Your account has been successfully disconnected from Facebook.";
            return RedirectToAction("Index", "UserProfile");
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
                    var organization = OrganizationRepository.GetDefaultOrganization(readOnly: false);

                    if (organization.UserProfiles == null)
                    {
                        organization.UserProfiles = new List<UserProfile>();
                    }

                    userProfile.Active = true;
                    userProfile.IsActivated = false;
                    var service = new GrassrootsMembershipService();
                    userProfile.ActivationHash = service.GetUserAuthorizationHash();
                    userProfile.LastActivationAttempt = DateTime.Now;
                    organization.UserProfiles.Add(userProfile);
                    OrganizationRepository.Save();
                    accountMailer.Authorize(new AuthorizeModel
                                                {
                                                    Email = userProfile.Email,
                                                    FirstName = userProfile.FirstName,
                                                    LastName = userProfile.LastName,
                                                    SenderEmail = organization.ContactEmail,
                                                    SenderName = organization.Name,
                                                    Url = Url.ToPublicUrl(Url.Action("Activate", "Account", new { hash = userProfile.ActivationHash }))
                                                }).SendAsync();

                    return RedirectToAction("AwaitingActivation", "Account");
                }
            }

            return RedirectToAction("Register");
        }

        //private string GetLogOffUrl()
        //{
        //    return Url.ToPublicUrl(Url.Action("Index", "Home"));
        //}

        private string GetOAuthRedirectUrl()
        {
            return Url.ToPublicUrl(Url.Action("OAuth", "Facebook"));
        }

        private string GetAccountConnectUrl()
        {
            return Url.ToPublicUrl(Url.Action("ConnectAccount", "Facebook"));
        }

        private string GetNewAccountUrl()
        {
            return Url.ToPublicUrl(Url.Action("NewAccount", "Facebook"));
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
                                    Birthdate = DateTime.Parse(me.birthday)
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
    }
}
