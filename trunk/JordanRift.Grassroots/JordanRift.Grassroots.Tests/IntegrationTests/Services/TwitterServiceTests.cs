//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Linq;
using JordanRift.Grassroots.Framework.Services;
using NUnit.Framework;

namespace JordanRift.Grassroots.Tests.IntegrationTests.Services
{
    [TestFixture]
    class TwitterServiceTests
    {
        [Test]
        public void GetTweets_Sould_Load_Statuses_From_Twitter()
        {
            var service = new TwitterService();
            var tweets = service.GetTweets("grassrootsproj");
            Assert.IsNotEmpty(tweets.ToList());
        }
    }
}
