class Grassroots.Helpers.UrlHelper
	@parseQueryString: ->
		urlParams = {}
		spaceExp = /\+/g
		paramExp = /([^&=]+)=?([^&]*)/g
		decode = (str) ->
			decodeURIComponent str.replace spaceExp, ' '
		querystring = window.location.search.substring 1
		while r = paramExp.exec querystring
			urlParams[decode r[1]] = decode r[2]
		urlParams