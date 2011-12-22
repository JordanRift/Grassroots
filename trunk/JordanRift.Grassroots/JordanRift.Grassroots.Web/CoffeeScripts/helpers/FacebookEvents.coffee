# Declare a new event aggrigator object
facebookEvents = _.extend Grassroots.Events, Backbone.Events

# Set of default AJAX callbacks to respond to Arena web service calls
facebookEvents.defaults =
	onLoginSuccess: (result) ->
		if result.d > -1 then window.location = Grassroots.Helpers.Authentication.config.redirectPath
		else facebookEvents.trigger 'arenaLoginFailed'
	onConnectSuccess: (result) ->
		facebookEvents.trigger 'connectAccountSuccessful'
	onOptOutSuccess: (result) ->
		facebookEvents.trigger 'optOutSuccessful'
	onError: (result, status, error) ->
		console.log JSON.stringify result
		console.log status
		console.log error

# Call Arena web service in attempt to authenticate against database given
# FB Access Token and 'state' hash to prevent CSRF hacks
facebookEvents.bind 'arenaLogin', (onSuccess = facebookEvents.defaults.onLoginSuccess, onError = facebookEvents.defaults.onError) ->
	accessToken = Grassroots.Helpers.Authentication.config.accessToken
	state = Grassroots.Helpers.Authentication.config.state
	$.ajax
		url: "webservices/custom/cccev/core/AuthenticationService.asmx/AuthenticateFacebook"
		type: 'POST'
		data: "{'accessToken': '#{accessToken}', 'state': '#{state}'}"
		contentType: 'application/json'
		dataType: 'json'
		success: onSuccess
		error: onError

# Will call against the Facebook JavaScript SDK to authenticate against Facebook and fetch Access Token
facebookEvents.bind 'facebookLogin', ->
	FB.getLoginStatus (response) ->
		# Is the user currently logged into Facebook?
		if response.status is 'connected' then facebookEvents.trigger 'facebookLoggingIn', response
		else 
			FB.login (response) ->
				# The user has granted permission, initialize the user to the client app
				if response.authResponse then facebookEvents.trigger 'facebookLoggingIn', response
				# The user has denied Grassroots app access to their info
				else facebookEvents.trigger 'userDeclined'
			, scope: 'email,user_birthday'

# We've gotten status back from Facebook at this point. The user is logged in now.
# Load up their Facebook data and fire the Logged In event
facebookEvents.bind 'facebookLoggingIn', (response) ->
	Grassroots.Helpers.Authentication.config.accessToken = response.authResponse.accessToken
	FB.api '/me', (res) -> facebookEvents.trigger 'facebookLoggedIn', res

# Attempts to log the user out of Facebook
facebookEvents.bind 'facebookLogOut', -> if FB then FB.logout()

# User is currently logged into Arena and Facebook. Attempt to stitch their Facebook account to their Arena account.
facebookEvents.bind 'connectAccounts', (onSuccess = facebookEvents.defaults.onConnectSuccess, onError = facebookEvents.defaults.onError) ->
	accessToken = Grassroots.Helpers.Authentication.config.accessToken
	state = Grassroots.Helpers.Authentication.config.state
	$.ajax
		url: 'webservices/custom/cccev/core/AuthenticationService.asmx/ConnectAccountToFacebook'
		type: 'POST'
		data: "{'accessToken': '#{accessToken}', 'state': '#{state}'}"
		contentType: 'application/json'
		dataType: 'json'
		success: onSuccess
		error: onError

# If user is currently logged into Arena, they can choose to opt out of Facebook login. If they opt out, they will
# not be asked to connect their accounts again.
facebookEvents.bind 'optOut', (onSuccess = facebookEvents.defaults.onOptOutSuccess, onError = facebookEvents.defaults.onError) ->
	state = Grassroots.Helpers.Authentication.config.state
	$.ajax
		url: 'webservices/custom/cccev/core/AuthenticationService.asmx/OptOutOfFacebookConnection'
		type: 'POST'
		data: "{'state': '#{state}'}"
		contentType: 'application/json'
		dataType: 'json'
		success: onSuccess
		error: onError