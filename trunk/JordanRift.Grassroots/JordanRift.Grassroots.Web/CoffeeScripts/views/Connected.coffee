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

class Grassroots.Views.Connected extends Backbone.View
	constructor: -> 
		# Rather than assigning @el here directly, set className and tagName.
		# @el needs get created dynamically, since it's not currently in the DOM.
		@className = 'connect-success'
		@tagName = 'div'
		@template = '''
					<img src='https://graph.facebook.com/{{id}}/picture?type=square' class="imageSmall" height="50" width="50" alt={{name}} />
					<p><strong>Sweet!</strong> {{name}}, you're all set!</p>
					'''
		super
	initialize: (options) ->
		@ev = options.events
		@model = options.model
	render: ->
		$el = $(@el)
		$el.prependTo 'body'
		jsonData = @model.toJSON()
		renderedHtml = Mustache.to_html( @template, jsonData)
		$el.html( renderedHtml ) 
		$el.fadeIn 'fast', =>
			setTimeout(=>
				$el.fadeOut 'fast', => @remove()
			, 5000)