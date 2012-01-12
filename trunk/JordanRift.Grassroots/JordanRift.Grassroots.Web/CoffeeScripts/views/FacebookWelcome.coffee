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

class Grassroots.Views.FacebookWelcome extends Backbone.View
	constructor: ->
		@el = '#user-info'
		@template = '''
					<img src='https://graph.facebook.com/{{id}}/picture?type=square' class="imageSmall" height="50" width="50" alt={{name}} />
					<p>Welcome {{name}}! Hang tight, we're logging you in.</p>
					'''
		super
	initialize: (options) ->
		@ev = options.events
	render: ->
		$container = $(@el)
		renderedHtml = Mustache.to_html(@template, @model.toJSON())
		$container.siblings().fadeOut 'slow', ->
			$container.html(renderedHtml)
			$container.fadeIn('slow')
			false