﻿//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Collections.Generic;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.Framework.Services
{
    public interface ITwitterService
    {
        IEnumerable<Tweet> GetTweets(string twitterName, int count = 5);
    }
}