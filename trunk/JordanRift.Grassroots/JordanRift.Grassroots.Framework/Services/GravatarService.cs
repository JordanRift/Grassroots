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

using System.Security.Cryptography;
using System.Text;

namespace JordanRift.Grassroots.Framework.Services
{
    public class GravatarService : IGravatarService
    {
        private const string URL_FORMAT_STRING = "http://www.gravatar.com/avatar/{0}?d={1}&s={2}&r={3}";
        private enum IconSet { identicon, monsterid, wavatar }
        private enum Rating { g, pg, r, x }
        
        public string HashEmailForGravatar(string email)
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

        public string GetGravatarPictureUrl(string email, int size)
        {
            var hash = HashEmailForGravatar(email);
            return string.Format(URL_FORMAT_STRING, hash, IconSet.identicon, size, Rating.g);
        }
    }
}
