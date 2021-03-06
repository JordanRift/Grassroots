﻿//
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

using System.Web.Mvc;
using System.Web.Routing;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Helpers;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Helpers;
using JordanRift.Grassroots.Web.Mailers;
using JordanRift.Grassroots.Web.Models;
using JordanRift.Grassroots.Web.Models.ModelBinders;
using Ninject;

namespace JordanRift.Grassroots
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Require https globally, use custom filter to play nicely with load balanced cloud servers
            filters.Add(new LoadBalancedRequireHttpsAttribute());

            filters.Add(new ElmahHandleErrorAttribute());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Campaigns",
                "Campaigns/{slug}",
                new { controller = "Campaign", action = "Index", slug = UrlParameter.Optional }
            );

            routes.MapRoute(
                "CampaignModify",
                "Campaigns/Edit/{slug}", // URL with url-slug appended
                new { controller = "Campaign", action = "Edit", slug = UrlParameter.Optional }
            );

            RegisterDonateRoutes(routes);
            RegisterHomeRoutes(routes);

            routes.MapRoute(
                "Projects",
                "Projects/{action}/{id}",
                new { controller = "CauseTemplate", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "CauseSearch",
                "Projects/Search/{id}/{referenceNumber}",
                new { controller = "CauseTemplate", action = "CauseDetails", id = UrlParameter.Optional, referenceNumber = UrlParameter.Optional }
            );

            RegisterUserProfileRoutes(routes);
            RegisterAdminRoutes(routes);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            
            // Ninject Dependency Resolver
            RegisterDependencyResolver();

            // Custom ModelBinders
            ModelBinders.Binders[typeof(Payment)] = new PaymentModelBinder();
            ModelBinders.Binders[typeof(OrganizationDetailsModel)] = new OrganizationDetailsModelBinder();

            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        private static void RegisterDependencyResolver()
        {
            var kernel = new StandardKernel();
            
            // Repository layer DI
            kernel.Bind<ICampaignDonorRepository>().To<CampaignDonorRepository>();
            kernel.Bind<ICampaignRepository>().To<CampaignRepository>();
            kernel.Bind<IOrganizationRepository>().To<OrganizationRepository>();
            kernel.Bind<IUserProfileRepository>().To<UserProfileRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IRoleRepository>().To<RoleRepository>();
            kernel.Bind<ICauseRepository>().To<CauseRepository>();
            kernel.Bind<ICauseTemplateRepository>().To<CauseTemplateRepository>();
            
            // Payment Provider DI
            kernel.Bind<IPaymentProviderFactory>().To<PaymentProviderFactory>();
            
            // Notification System DI
            kernel.Bind<IAccountMailer>().To<AccountMailer>();
            kernel.Bind<ICampaignMailer>().To<CampaignMailer>();
            kernel.Bind<IDonateMailer>().To<DonateMailer>();
            kernel.Bind<IUserProfileMailer>().To<UserProfileMailer>();
            kernel.Bind<IUserMailer>().To<UserMailer>();
            
            // Social service integration DI
            kernel.Bind<ITwitterService>().To<TwitterService>();
            kernel.Bind<IBlogService>().To<BlogService>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

		private static void RegisterAdminRoutes( RouteCollection routes )
		{
            routes.MapRoute(
                "AdminProjects",
                "Admin/Projects/{action}/{id}",
                new { controller = "CauseTemplate", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "AdminUpdateCampaign",
                "Admin/Campaigns/Update",
                new { controller = "Campaign", action = "AdminUpdate" }
            );

            routes.MapRoute(
                "AdminCampaigns",
                "Admin/Campaigns/{action}/{id}",
                new { controller = "Campaign", action = "List", id = UrlParameter.Optional }
            );

		    routes.MapRoute(
                "AdminUpdateDonation", 
                "Admin/Donors/Update", new { controller = "Donate", action = "AdminUpdate" }
            );

		    routes.MapRoute(
                "AdminDonations", 
                "Admin/Donors/{action}/{id}", 
                new { controller = "Donate", action = "List", id = UrlParameter.Optional }
            );

		    routes.MapRoute(
                "AdminUpdateUserProfile", 
                "Admin/UserProfiles/Update", 
                new { controller = "UserProfile", action = "AdminUpdate" }
            );

		    routes.MapRoute(
                "AdminUserProfiles", 
                "Admin/UserProfiles/{action}/{id}", 
                new { controller = "UserProfile", action = "List", id = UrlParameter.Optional }
            );

		    routes.MapRoute(
                "AdminRoles", 
                "Admin/Roles/{action}/{id}", 
                new { controller = "Role", action = "List", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "AdminUsers", 
                "Admin/Users/{action}/{id}", 
                new { controller = "User", action = "ChangePassword", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "AdminMethods",
                "Admin/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional}
            );
		}

        private static void RegisterDonateRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "ProcessDonation",
                "Donate/ProcessDonation",
                new { controller = "Donate", action = "ProcessDonation" }
            );

            routes.MapRoute(
                "DonationThankYou",
                "Donate/ThankYou",
                new { controller = "Donate", action = "ThankYou" }
            );

            routes.MapRoute(
                "Donate",
                "Donate/{slug}",
                new { controller = "Donate", action = "Index", slug = UrlParameter.Optional }
            );
        }

        private static void RegisterHomeRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "About",
                "About",
                new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                "Terms",
                "Terms",
                new { controller = "Home", action = "Terms" }
            );
        }

        private static void RegisterUserProfileRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "AuthorizeAccount",
                "Account/Activate/{hash}",
                new { controller = "Account", action = "Activate", hash = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ForgotPassword",
                "Account/UpdatePassword/{hash}",
                new { controller = "Account", action = "UpdatePassword", hash = UrlParameter.Optional }
            );

            routes.MapRoute(
                "DeactivateAccount",
                "UserProfile/DeactivateAccount",
                new { controller = "UserProfile", action = "DeactivateAccount" }
            );

            routes.MapRoute(
                "Deactivate",
                "UserProfile/Deactivate",
                new { controller = "UserProfile", action = "Deactivate" }
            );

            routes.MapRoute(
                "ReactivateAccount",
                "UserProfile/ReactivateAccount",
                new { controller = "UserProfile", action = "ReactivateAccount" }
            );

            routes.MapRoute(
                "Reactivate",
                "UserProfile/Reactivate",
                new { controller = "UserProfile", action = "Reactivate" }
            );

            routes.MapRoute(
                "UserProfileEdit",
                "UserProfile/Edit",
                new { controller = "UserProfile", action = "Edit", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                "UserProfileIndex",
                "UserProfile/{id}",
                new { controller = "UserProfile", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}