using System.Web.Mvc;
using System.Web.Routing;
using JordanRift.Grassroots.Framework.Data;
using JordanRift.Grassroots.Framework.Entities;
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
#if !DEBUG 
            // Require https globally
            filters.Add(new RequireHttpsAttribute()); 
#endif
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "CampaignFor",
                "CampaignFor/{slug}",
                new { controller = "Campaign", action = "Index", slug = UrlParameter.Optional }
            );

            routes.MapRoute(
                "UserProfile",
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
        }

        private static void RegisterDependencyResolver()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ICampaignRepository>().To<CampaignRepository>();
            kernel.Bind<IOrganizationRepository>().To<OrganizationRepository>();
            kernel.Bind<IUserProfileRepository>().To<UserProfileRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IRoleRepository>().To<RoleRepository>();
            kernel.Bind<ICauseRepository>().To<CauseRepository>();
            kernel.Bind<ICauseTemplateRepository>().To<CauseTemplateRepository>();
            kernel.Bind<IPaymentProviderFactory>().To<PaymentProviderFactory>();
            kernel.Bind<IAccountMailer>().To<AccountMailer>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}