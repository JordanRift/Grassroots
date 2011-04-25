//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JordanRift.Grassroots.Framework.Entities;
using Newtonsoft.Json.Linq;

namespace JordanRift.Grassroots.Framework.Services
{
    public class TwitterService : ITwitterService
    {
        /// <summary>
        /// Will communicate with Twitter to fetch the latest "x" tweets based on the account name passed in.
        /// </summary>
        /// <param name="twitterName">Twitter name</param>
        /// <param name="count">Number of tweets to get.</param>
        /// <returns>Collectin of tweets.</returns>
        public IEnumerable<Tweet> GetTweets(string twitterName, int count = 5)
        {
            if (twitterName.StartsWith("@"))
            {
                twitterName = twitterName.Replace("@", "");
            }

            var client = new WebClient();
            var response = client.DownloadString(new Uri(
                string.Format("https://api.twitter.com/1/statuses/user_timeline.json?screen_name={0}", twitterName)));
            dynamic json = JArray.Parse(response);
            var tweets = new List<Tweet>();

            foreach (var item in json)
            {
                tweets.Add(new Tweet
                               {
                                   Message = item.text,
                                   ImageUrl = item.user.profile_image_url,
                                   ScreenName = item.user.screen_name,
                                   RelativeDate = item.created_at
                               });
            }

            return tweets.Take(count);
        }

        private static string GetRelativeDate(string jsonDate)
        {
            // TODO: Parse date returned by Twitter REST API and create a friendly relative date.
            // ex: Mon Apr 25 08:37:12 +0000 2011
            return jsonDate;
        }
    }
}
