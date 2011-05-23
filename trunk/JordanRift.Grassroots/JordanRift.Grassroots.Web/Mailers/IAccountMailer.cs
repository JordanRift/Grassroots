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

using System.Net.Mail;
using JordanRift.Grassroots.Web.Models;

namespace JordanRift.Grassroots.Web.Mailers
{ 
    public interface IAccountMailer
    {
		MailMessage Welcome(RegisterModel model);
        MailMessage PasswordReset(RegisterModel model);
        MailMessage PasswordChange(RegisterModel model);
	}
}