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

class Grassroots.Helpers.Authentication
	@initConnect: (person) ->
		theModel = new Grassroots.Models.FacebookUser person
		window.facebookConnect = new Grassroots.Routers.FacebookConnectRouter model: theModel, events: facebookEvents
		try
			Backbone.history.start()
		catch error
		Authentication.registerFacebookApi()
	@initLogin: ->
		window.facebookAuth = new Grassroots.Routers.FacebookLoginRouter model: null, events: facebookEvents
		try
			Backbone.history.start()
		catch error
		Authentication.registerFacebookApi()
	@registerFacebookApi: ->
		window.fbAsyncInit = ->
			FB.init
				appId: Authentication.config.appID
				status: true
				cookie: true
				xfbml: true
				oauth: true
		e = document.createElement 'script'
		e.src = "#{document.location.protocol}//connect.facebook.net/en_US/all.js"
		e.async = true
		document.getElementById('fb-root').appendChild e