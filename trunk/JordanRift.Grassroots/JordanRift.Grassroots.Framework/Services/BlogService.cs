//
// Grassroots is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Grassroots is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
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
            try
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
            catch (Exception)
            {
                return null;
            }
        }
    }
}
