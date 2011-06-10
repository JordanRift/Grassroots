using System.Web.Mvc;
using System.Web.Routing;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
using JordanRift.Grassroots.Framework.Services;
using JordanRift.Grassroots.Web.Helpers;
using JordanRift.Grassroots.Web.Mailers;
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
            filters.Add(new HandleErrorAttribute());
            // Require https globally
            filters.Add(new RequireHttpsAttribute()); 
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Campaigns",
                "Campaigns/{slug}",
                new { controller = "Campaign", action = "Index", slug = UrlParameter.Optional }
            );

            RegisterDonateRoutes(routes);
            RegisterHomeRoutes(routes);

            routes.MapRoute(
                "UserProfileIndex", 
                "UserProfile/{id}", 
                new { controller = "UserProfile", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "CampaignModify",
                "Campaign/{action}/{slug}", // URL with url-slug appended
                new { controller = "Campaign", action = "Edit", slug = UrlParameter.Optional }
            );

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

            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        private static void RegisterDependencyResolver()
        {
            var kernel = new StandardKernel();
            
            // Repository layer DI
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
            
            // Social service integration DI
            kernel.Bind<ITwitterService>().To<TwitterService>();
            kernel.Bind<IBlogService>().To<BlogService>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
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
                "Projects",
                "Projects",
                new { controller = "Home", action = "Projects" }
            );

            routes.MapRoute(
                "Terms",
                "Terms",
                new { controller = "Home", action = "Terms" }
            );
        }
    }
}