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