//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.ComponentModel.DataAnnotations;

namespace JordanRift.Grassroots.Framework.Entities.Validation
{
    public interface IOrganizationSettingValidation
    {
        [Required(ErrorMessage = "Please enter an Organization Setting Name.")]
        string Name { get; set; }

        [Required(ErrorMessage = "Please enter an Organization Setting Value.")]
        string Value { get; set; }

        [Required(ErrorMessage = "Please enter an Organization Setting Data Type.")]
        int DataType { get; set; }
    }
}
