//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;
using Argotic.Common;
using Argotic.Syndication;
using JordanRift.Grassroots.Framework.Entities;

namespace JordanRift.Grassroots.Framework.Services
{
    public class BlogService : IBlogService
    {
        public BlogPost GetLatestPost(string feedUrl)
        {
            var feed = GenericSyndicationFeed.Create(new Uri(feedUrl));
            var feedPost = feed.Items.First();

            var blogPost = new BlogPost
                               {
                                   Title = feedPost.Title,
                                   PostDate = feedPost.PublishedOn,
                                   Summary = feedPost.Summary,
                               };

            if (feed.Format == SyndicationContentFormat.Rss)
            {
                var rssFeed = feed.Resource as RssFeed;

                if (rssFeed != null)
                {
                    var rsspost = rssFeed.Channel.Items.First();
                    blogPost.Author = rsspost.Author;
                    blogPost.Body = rsspost.Description;
                    blogPost.Url = rsspost.Link.ToString();
                }
            }

            return blogPost;
        }
    }
}
