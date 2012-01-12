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

# TODO: Create a more complete template for signup form...
class Grassroots.Views.FacebookSignUp extends Backbone.View
	constructor: ->
		@el = '#signup-form'
		@template = '''
					<h3>Hi {{name}}</h3>
					<img src='https://graph.facebook.com/{{id}}/picture?type=square' class="imageSmall" height="50" width="50" alt={{name}} />
					<p><strong>Almost done!</strong> Just need to link your Central account to Facebook. Which of these options sounds right?</p>
					'''
		@unbind()
		super
	initialize: (options) ->
		@ev = options.events
		_.bindAll(@, 'logOut')
	render: ->
		$('.greeting').empty()
		renderedHtml = Mustache.to_html(@template, @model.toJSON())
		$('.greeting').html(renderedHtml)
		$("input[id$='tbFirstName']").val @model.get('first_name')
		$("input[id$='tbLastName']").val @model.get('last_name')
		$("input[id$='tbEmail']").val @model.get('email')
		$("input[id$='tbBirthdate']").val @model.get('birthday')
		$("input[id$='ihAccessToken']").val Grassroots.Helpers.Authentication.config.accessToken
		$('.im-new, .have-account').next('div').hide()
		$that = @
		$(@el).siblings().fadeOut 'slow', ->
			$($that.el).fadeIn('slow')
			false
	events:
		'click .fblogout': 'logOut'
		'click .im-new': 'register'
		'click .have-account': 'signIn'
	logOut: ->
		@ev.trigger 'facebookLogOut'
		false
	register: ->
		$('.have-account').next('div').slideUp()
		$('.im-new').next('div').slideToggle()
		false
	signIn: ->
		$('.im-new').next('div').slideUp()
		$('.have-account').next('div').slideToggle()
		false
	unbind: ->
		# Unbind events dynamically bound by Backbone. If the elements in quesiton do not share
		# the same lifecycle as the parent View object, events will be bound more than once.
		$(@el).undelegate '.fb-logout', 'click'
		$(@el).undelegate '.im-new', 'click'
		$(@el).undelegate '.have-account', 'click'