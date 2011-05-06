//
// Copyright © 2011 Jordan Rift, LLC - All Rights Reserved
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Security.Cryptography;
using System.Text;

namespace JordanRift.Grassroots.Framework.Services
{
    public class GravatarService : IGravatarService
    {
        private const string URL_FORMAT_STRING = "http://www.gravatar.com/avatar/{0}?d={1}&s={2}&r={3}";
        private const int AVATAR_SIZE = 48;
        private enum IconSet { identicon, monsterid, wavatar }
        private enum Rating { g, pg, r, x }
        
        public string HashEmail(string email)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.ASCII.GetBytes(email.ToLower());
            bytes = md5.ComputeHash(bytes);
            string result = string.Empty;

            foreach (byte b in bytes)
            {
                result += b.ToString("x2");
            }

            return result;
        }

        public string GetGravatarPictureUrl(string email)
        {
            var hash = HashEmail(email);
            return string.Format(URL_FORMAT_STRING, hash, IconSet.identicon, AVATAR_SIZE, Rating.g);
        }
    }
}
