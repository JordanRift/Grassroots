//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.Mvc;
using AutoMapper;
using JordanRift.Grassroots.Framework.Entities.Models;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Controllers
{
    public class ErrorController : GrassrootsControllerBase
    {
        public ErrorController()
        {
            Mapper.CreateMap<Organization, OrganizationDetailsModel>();
        }

        public ActionResult Index()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View("Error", model);
        }

        public ActionResult NotFound()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View("404", model);
        }

        public ActionResult Forbidden()
        {
            var organization = OrganizationRepository.GetDefaultOrganization(readOnly: true);
            var model = Mapper.Map<Organization, OrganizationDetailsModel>(organization);
            return View("403", model);
        }
    }
}
