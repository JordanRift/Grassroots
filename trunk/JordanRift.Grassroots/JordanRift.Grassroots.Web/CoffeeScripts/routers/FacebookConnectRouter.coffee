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

class Grassroots.Routers.FacebookConnectRouter extends Backbone.Router
	routes:
		'': 'index'
		'*options': 'index'
	initialize: (options) ->
		if options.model then @user = options.model
		@ev = options.events
		@user = options.model
		@indexView = new Grassroots.Views.FacebookConnectAccount model: @user, events: @ev
		_.bindAll(@)
		@ev.bind 'facebookLoggedIn', @onLogin
		@ev.bind 'userDeclined', @onDecline
		@ev.bind 'connectAccountSuccessful', @onConnected
	index: -> @indexView.render()
	onLogin: (response) ->
		@user = new Grassroots.Models.FacebookUser response
		@ev.trigger 'connectAccounts'
	onConnected: ->
		console.log @user
		@connectedView = new Grassroots.Views.Connected model: @user, events: @ev
		@indexView.close()
		@connectedView.render()
	onDecline: -> @indexView.close()