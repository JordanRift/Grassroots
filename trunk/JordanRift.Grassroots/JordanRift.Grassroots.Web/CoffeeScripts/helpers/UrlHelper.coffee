#
# Grassroots is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# Grassroots is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with Grassroots.  If not, see <http://www.gnu.org/licenses/>.
#

class Grassroots.Helpers.UrlHelper
	@parseQueryString: ->
		urlParams = {}
		spaceExp = /\+/g
		paramExp = /([^&=]+)=?([^&]*)/g
		decode = (str) ->
			decodeURIComponent str.replace spaceExp, ' '
		querystring = window.location.search.substring 1
		while r = paramExp.exec querystring
			urlParams[decode r[1]] = decode r[2]
		urlParams