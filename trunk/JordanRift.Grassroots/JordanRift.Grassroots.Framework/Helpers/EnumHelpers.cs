//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public static class EnumHelpers
    {
        public static T ToEnum<T>(this string value)
        {
            var theEnum = (T) Enum.Parse(typeof (T), value);
            return theEnum;
        }
    }
}
