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