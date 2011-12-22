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