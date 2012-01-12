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

# TODO: Create view template for Login button...
class Grassroots.Views.FacebookLogin extends Backbone.View
	constructor: ->
		@el = '#log-in'
		@unbind()
		super
	initialize: (options) ->
		@ev = options.events
	render: ->
		$container = $(@el)
		$('.fb-auth > div').fadeOut 'slow', ->
			$container.fadeIn('slow')
			false
	events:
		'click .facebook-login': 'logIn'
	logIn: ->
		@ev.trigger 'facebookLogin'
		false
	unbind: -> 
		# Unbind events dynamically bound by Backbone. If the elements in quesiton do not share
		# the same lifecycle as the parent View object, events will be bound more than once.
		$(@el).undelegate '.facebook-login', 'click'