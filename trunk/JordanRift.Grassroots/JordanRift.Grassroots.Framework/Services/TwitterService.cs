//
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
    public class TwitterService
    {
        /// <summary>
        /// Will communicate with Twitter to fetch the latest "x" tweets based on the account name passed in.
        /// </summary>
        /// <param name="twitterName">Twitter name</param>
        /// <param name="count">Number of tweets to get.</param>
        /// <returns>Collectin of tweets.</returns>
        public IEnumerable<Tweet> GetTweets(string twitterName, int count = 5)
        {
            return null;
        }
    }
}
