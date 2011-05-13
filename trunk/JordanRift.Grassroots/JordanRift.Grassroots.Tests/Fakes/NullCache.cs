//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using JordanRift.Grassroots.Framework.Helpers;

namespace JordanRift.Grassroots.Tests.Fakes
{
    public class NullCache : ICache
    {
        public object Get(string key)
        {
            return null;
        }

        public void Add(string key, object value)
        {
        }

        public void Remove(string key)
        {
        }

        public bool Any(Func<KeyValuePair<string, object>, bool> predicate)
        {
            return false;
        }
    }
}
