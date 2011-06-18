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
using System.Web.Mvc;

namespace JordanRift.Grassroots.Web.Helpers
{
	public static class DateTimeHelperExtensions
	{
		public static string RelativeDate( this DateTime d )
		{
			DateTime now = DateTime.Now;
			TimeSpan timeSince = now - d;

			double inSeconds = timeSince.TotalSeconds;
			double inMinutes = timeSince.TotalMinutes;
			double inHours = timeSince.TotalHours;
			double inDays = timeSince.TotalDays;
			double inMonths = inDays / 30;
			double inYears = inDays / 365;

			if ( Math.Round( inSeconds ) == 1 )
			{
				return "1 second ago";
			}
			else if ( inMinutes < 1.0 )
			{
				return Math.Floor( inSeconds ) + " seconds ago";
			}
			else if ( Math.Floor( inMinutes ) == 1 )
			{
				return "1 minute ago";
			}
			else if ( inHours < 1.0 )
			{
				return Math.Floor( inMinutes ) + " minutes ago";
			}
			else if ( Math.Floor( inHours ) == 1 )
			{
				return "about an hour ago";
			}
			else if ( inDays < 1.0 )
			{
				return Math.Floor( inHours ) + " hours ago";
			}
			else if ( Math.Floor( inDays ) == 1 )
			{
				return "1 day ago";
			}
			else if ( inMonths < 3 )
			{
				return Math.Floor( inDays ) + " days ago";
			}
			else if ( inMonths <= 12 )
			{
				return Math.Floor( inMonths ) + " months ago ";
			}
			else if ( Math.Floor( inYears ) <= 1 )
			{
				return "1 year ago";
			}
			else
			{
				return Math.Floor( inYears ) + " years ago";
			}
		}
	}
}