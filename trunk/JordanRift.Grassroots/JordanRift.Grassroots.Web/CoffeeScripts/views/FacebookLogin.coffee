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