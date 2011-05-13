//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;

namespace JordanRift.Grassroots.Framework.Helpers
{
    public interface ICache
    {
        object Get(string key);
        void Add(string key, object value);
        void Remove(string key);
        bool Any(Func<KeyValuePair<string, object>, bool> predicate);
    }
}
