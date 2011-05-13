//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Configuration;
using System.Linq;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public static class CacheFactory
    {
        public static ICache GetCache()
        {
            var className = typeof (ICache).ToString().Split(new[] { '.' }).LastOrDefault();

            if (string.IsNullOrEmpty(className))
            {
                throw new InvalidOperationException();
            }

            var setting = ConfigurationManager.AppSettings[className];
            var settingArray = setting.Split(new[] { ',' });
            var classPath = settingArray[0].Trim();
            var assemblyName = settingArray[1].Trim();
            return (ICache) Activator.CreateInstance(assemblyName, classPath).Unwrap();
        }
    }
}
