/*!
 * JavaScript Cookie v2.1.0
 * https://github.com/js-cookie/js-cookie
 *
 * Copyright 2006, 2015 Klaus Hartl & Fagner Brack
 * Released under the MIT license
 */
(function (factory) {
	if (typeof define === 'function' && define.amd) {
		define(factory);
	} else if (typeof exports === 'object') {
		module.exports = factory();
	} else {
		var _OldCookies = window.Cookies;
		var api = window.Cookies = factory();
		api.noConflict = function () {
			window.Cookies = _OldCookies;
			return api;
		};
	}
}(function () {
	function extend () {
		var i = 0;
		var result = {};
		for (; i < arguments.length; i++) {
			var attributes = arguments[ i ];
			for (var key in attributes) {
				result[key] = attributes[key];
			}
		}
		return result;
	}

	function init (converter) {
		function api (key, value, attributes) {
			var result;

			// Write

			if (arguments.length > 1) {
				attributes = extend({
					path: '/'
				}, api.defaults, attributes);

				if (typeof attributes.expires === 'number') {
					var expires = new Date();
					expires.setMilliseconds(expires.getMilliseconds() + attributes.expires * 864e+5);
					attributes.expires = expires;
				}

				try {
					result = JSON.stringify(value);
					if (/^[\{\[]/.test(result)) {
						value = result;
					}
				} catch (e) {}

				if (!converter.write) {
					value = encodeURIComponent(String(value))
						.replace(/%(23|24|26|2B|3A|3C|3E|3D|2F|3F|40|5B|5D|5E|60|7B|7D|7C)/g, decodeURIComponent);
				} else {
					value = converter.write(value, key);
				}

				key = encodeURIComponent(String(key));
				key = key.replace(/%(23|24|26|2B|5E|60|7C)/g, decodeURIComponent);
				key = key.replace(/[\(\)]/g, escape);

				return (document.cookie = [
					key, '=', value,
					attributes.expires && '; expires=' + attributes.expires.toUTCString(), // use expires attribute, max-age is not supported by IE
					attributes.path    && '; path=' + attributes.path,
					attributes.domain  && '; domain=' + attributes.domain,
					attributes.secure ? '; secure' : ''
				].join(''));
			}

			// Read

			if (!key) {
				result = {};
			}

			// To prevent the for loop in the first place assign an empty array
			// in case there are no cookies at all. Also prevents odd result when
			// calling "get()"
			var cookies = document.cookie ? document.cookie.split('; ') : [];
			var rdecode = /(%[0-9A-Z]{2})+/g;
			var i = 0;

			for (; i < cookies.length; i++) {
				var parts = cookies[i].split('=');
				var name = parts[0].replace(rdecode, decodeURIComponent);
				var cookie = parts.slice(1).join('=');

				if (cookie.charAt(0) === '"') {
					cookie = cookie.slice(1, -1);
				}

				try {
					cookie = converter.read ?
						converter.read(cookie, name) : converter(cookie, name) ||
						cookie.replace(rdecode, decodeURIComponent);

					if (this.json) {
						try {
							cookie = JSON.parse(cookie);
						} catch (e) {}
					}

					if (key === name) {
						result = cookie;
						break;
					}

					if (!key) {
						result[name] = cookie;
					}
				} catch (e) {}
			}

			return result;
		}

		api.get = api.set = api;
		api.getJSON = function () {
			return api.apply({
				json: true
			}, [].slice.call(arguments));
		};
		api.defaults = {};

		api.remove = function (key, attributes) {
			api(key, '', extend(attributes, {
				expires: -1
			}));
		};

		api.withConverter = init;

		return api;
	}

	return init(function () {});
}));
$(document).ready(function(){
	var cookieName = 'menunavigationcookie', // Name of our cookie
        cookieValue = 'yes'; // Value of cookie

        //Close navigation menu when cookie is set
        function CloseNavigationMenu(){
			$('.main-menu').removeClass('is-active');
			$('.main-menu').removeClass('fixed');
			$('.menu-toggler').removeClass('is-active');
			$('.header__wrapper .menu-toggler').removeClass('is-sticky');
			$('body').removeClass('is-frozen');
			$('.main-menu').removeClass('shift-main-content');
			$('.main__wrapper').css('margin-left','0');
			$('body').removeClass('shift-content');
	     }

	    //open navigation menu when no cookie is set
	    function OpenNavigationMenu(){
			$('.main-menu').addClass('is-active');
			$('.main-menu').addClass('fixed');
			$('.menu-toggler').addClass('is-active');
			$('.header__wrapper .menu-toggler').addClass('is-sticky');
			$('body').addClass('is-frozen');
			$('.main-menu.is-active').addClass('shift-main-content');
			$('.main__wrapper').css('margin-left','330px');
			$('body').addClass('shift-content');
        }

        // Create cookie
		function createCookie(name, value, days) {
		    var expires;
		    if (days) {
		        var date = new Date();
		        date.setTime(date.getTime()+(days*24*60*60*1000));
		        expires = "; expires="+date.toGMTString();
		    }
		    else {
		        expires = "";
		    }
		    document.cookie = name+"="+value+expires+"; path=/";
		}

		// Read cookie
		function readCookie(name) {
		    var nameEQ = name + "=";
		    var ca = document.cookie.split(';');
		    for(var i=0;i < ca.length;i++) {
		        var c = ca[i];
		        while (c.charAt(0) === ' ') {
		            c = c.substring(1,c.length);
		        }
		        if (c.indexOf(nameEQ) === 0) {
		            return c.substring(nameEQ.length,c.length);
		        }
		    }
		    return null;
		}

		// Erase cookie
		function eraseCookie(name) {
		    createCookie(name,"",-1);
		}


        if($(window).width() >= 1024){
			if($('input.menu-open-first-time-checked').val() == 'True'){
				if(readCookie("menunavigationcookie") !== cookieValue && window.location.pathname === '/'){
					OpenNavigationMenu();
				}else{
					CloseNavigationMenu();
				}
				createCookie("menunavigationcookie", "yes", 365);
			}
			else{
				eraseCookie("menunavigationcookie");
			}
			
		}      
})