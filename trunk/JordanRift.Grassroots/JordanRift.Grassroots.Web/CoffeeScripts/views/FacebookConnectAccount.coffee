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