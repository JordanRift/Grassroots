﻿//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Globalization;
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
                                   RelativeDate = GetRelativeDate(item.created_at.ToString())
                               });
            }

            return tweets.Take(count);
        }

        private static string GetRelativeDate(string jsonDate)
        {
            // ex: Mon Apr 25 08:37:12 +0000 2011

            jsonDate = jsonDate.Replace("\\", "");
            jsonDate = jsonDate.Replace("\"", "");
            const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
            var date = DateTime.ParseExact(jsonDate, format, CultureInfo.InvariantCulture);

            var timespan = DateTime.Now - date;

            if (timespan <= TimeSpan.FromSeconds(60))
            {
                return timespan.Seconds + " seconds";
            }

            if (timespan <= TimeSpan.FromMinutes(60))
            {
                return timespan.Minutes + " minutes";
            }

            if (timespan <= TimeSpan.FromHours(24))
            {
                return timespan.Hours + " hours";
            }

            return timespan.Days + " days";
        }
    }
}
