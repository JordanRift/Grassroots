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

class Grassroots.Views.FacebookConnectAccount extends Backbone.View
	constructor: ->
		# Rather than assigning @el here directly, set className and tagName.
		# @el needs get created dynamically, since it's not currently in the DOM.
		@className = 'connect-account'
		@tagName = 'div'
		@template = '''
					<div class="close"><a href="#" class="fb-close">X</a></div>
					<p><strong>Hey {{name}}!</strong> We thought you might be interested in connecting your Central account to Facebook. 
					This will allow you to log into your Central account with a single click.</p>
					<p><a href="#" class="facebook-connect fbbutton" title="Connect to Facebook">Connect</a>
					<a href="#" class="fb-opt-out" title="Please don't ask me again">Please don't ask me again</a></p>
					'''
		super
	initialize: (options) ->
		@ev = options.events
		@model = options.model
		_.bindAll(@)
		@ev.bind 'optOutSuccessful', @onOptedOut
	render: ->
		$el = $(@el)
		$el.prependTo 'body'
		renderedHtml = Mustache.to_html(@template, @model.toJSON())
		$el.html(renderedHtml)
		@inTimer = setTimeout(=>
			$el.fadeIn 'slow'
		, 5000)
		@outTimer = setTimeout(=>
			@close()
		,15000)
	events: 
		'click .facebook-connect': 'connectAccount'
		'click .fb-close': 'close'
		'click .fb-opt-out': 'optOut'
	connectAccount: -> 
		@ev.trigger 'facebookLogin'
		false
	close: ->
		$(@el).fadeOut 'slow', => $(@el).remove()
		clearTimeout(@outTimer)
		false
	optOut: ->  
		@ev.trigger 'optOut'
		false
	onOptedOut: -> @close()