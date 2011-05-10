//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using JordanRift.Grassroots.Framework.Entities.Models;

namespace JordanRift.Grassroots.Framework.Data
{
    public interface IOrganizationRepository
    {
        Organization GetOrganizationByID(int id);
        Organization GetDefaultOrganization(bool readOnly = true);
        void Add(Organization organization);
        void Delete(Organization organization);
        void Save();
    }
}
