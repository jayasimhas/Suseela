(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
/**
 * if this popup is outside of it's parent, nudge it back in
 * @param  {element} popup: DOM elmenet of the popup to be placed
 * @param  {number} top: The top coordinate of where the popup should point
 * @param  {number} left: The left coordinate of where the popup should point
 * @param  {number} offset: an offet to be added to top/bottom or left/right
 * @param  {string} triangle: "top", "right", "bottom", or "left"
 * @param  {number} triangleSize: used to calculate the position
 * @param  {boolean} flipToContain: will flip the popup if it goes outside the parent container
 * @return {object} {
 *     realTop       : with no offset adjustment, the popup should go here, based on triangleSide
 *     realLeft      : ^^
 *     popupTop      : with adjustment when the popup butts up agains the parent
 *     popupLeft     : ^^
 *     overflow      : "top", "right", "bottom", "left".  Positive numbers are overflows
 *     triangleOffset: Amount the triangle needs to move to be on the dot, relative from 50%
 *     triangleSide  : Will be the same as passed in triangle, unless it fliped via flipToContain
 * }
 * use popupTop, popupLeft, and triangleOffset to position the popup
 */
"use strict";

Object.defineProperty(exports, "__esModule", {
    value: true
});
function calculatePopupOffsets(_ref) {
    var popup = _ref.popup;
    var top = _ref.top;
    var left = _ref.left;
    var _ref$offset = _ref.offset;
    var offset = _ref$offset === undefined ? 0 : _ref$offset;
    var _ref$triangle = _ref.triangle;
    var triangle = _ref$triangle === undefined ? "bottom" : _ref$triangle;
    var triangleSize = _ref.triangleSize;
    var _ref$flipToContain = _ref.flipToContain;
    var flipToContain = _ref$flipToContain === undefined ? false : _ref$flipToContain;

    // make a copy of this
    var triangleSide = triangle;

    // get the width and height of this popup from the DOM
    var width = popup.offsetWidth;
    var height = popup.offsetHeight;

    // get the width/height of the parent container div
    var parent = popup.parentNode;
    var parentWidth = parent.clientWidth;
    var parentHeight = parent.offsetHeight; // client height of body will only be the viewport height

    // common calculations
    var popupOnTop = top - height - triangleSize + offset;
    var popupOnBottom = top + triangleSize - offset;
    var popupOnLeft = left - width - triangleSize + offset;
    var popupOnRight = left + triangleSize - offset;

    // calculate where the top of the popup should be based on top/left
    var realTop = triangleSide === "bottom" ? popupOnTop : triangleSide === "top" ? popupOnBottom : top - height / 2; //  left or right

    var realLeft = triangleSide === "right" ? popupOnLeft : triangleSide === "left" ? popupOnRight : left - width / 2; // center

    // the amounts that this popup is outside of it's parent.
    var overflow = {
        top: -realTop,
        right: -(parentWidth - (realLeft + width)),
        bottom: -(parentHeight - (realTop + height)),
        left: -realLeft
    };

    // calculate where the popup should go
    // start with popupLeft as realLeft before nudging
    var popupTop = realTop;
    var popupLeft = realLeft;
    var triangleOffset = 0;

    // if there is an overflow on the right, adjust the popup and triangle position
    if (overflow.right > 0) {
        if (triangleSide === "top" || triangleSide === "bottom") {
            popupLeft = realLeft - overflow.right;
            triangleOffset = overflow.right;
        }

        // for left, flip the popup
        if (triangleSide === "left" && flipToContain) {
            triangleSide = "right";
            popupLeft = popupOnLeft;
        }
    }

    // if there is an overflow on the left, adjust the popup and triangle position
    if (overflow.left > 0) {
        if (triangleSide === "top" || triangleSide === "bottom") {
            popupLeft = realLeft + overflow.left;
            triangleOffset = -overflow.left;
        }

        // for right, flip the popup
        if (triangleSide === "right" && flipToContain) {
            triangleSide = "left";
            popupLeft = popupOnRight;
        }
    }

    // if there is an overflow on the bottom
    if (overflow.bottom > 0) {
        // for left/right, butt the popup against the bottom
        if (triangleSide === "left" || triangleSide === "right") {
            popupTop = realTop - overflow.bottom;
            triangleOffset = overflow.bottom;
        }
        // for top, flip the popup
        if (triangleSide === "top" && flipToContain) {
            triangleSide = "bottom";
            popupTop = popupOnTop;
        }
    }

    // if there is an overflow on the top
    if (overflow.top > 0) {

        if (triangleSide === "left" || triangleSide === "right") {
            popupTop = realTop + overflow.top;
            triangleOffset = -overflow.top;
        }

        // for bottom, flip the popup
        if (triangleSide === "bottom" && flipToContain) {
            triangleSide = "top";
            popupTop = popupOnBottom;
        }
    }

    // return all the measurements
    return {
        realTop: realTop, realLeft: realLeft, popupTop: popupTop, popupLeft: popupLeft, overflow: overflow, triangleOffset: triangleOffset, triangleSide: triangleSide
    };
}

exports["default"] = calculatePopupOffsets;
module.exports = exports["default"];

},{}],2:[function(require,module,exports){
'use strict';

var articleSidebarAd, articleSidebarAdParent, lastActionFlagsBar, stickyFloor, sidebarIsTaller;
$(document).ready(function () {
    articleSidebarAdParent = $('.article-right-rail section:last-child');
    articleSidebarAd = articleSidebarAdParent.find('.advertising');
    lastActionFlagsBar = $('.action-flags-bar:last-of-type');
    sidebarIsTaller = $('.article-right-rail').height() > $('.article-left-rail').height();
});
$(window).on('scroll', function () {
    if (articleSidebarAdParent && articleSidebarAdParent.length && !sidebarIsTaller) {
        // pageYOffset instead of scrollY for IE / pre-Edge compatibility
        stickyFloor = lastActionFlagsBar.offset().top - window.pageYOffset - articleSidebarAd.height();
        if (articleSidebarAdParent.offset().top - window.pageYOffset <= 16) {
            articleSidebarAdParent.addClass('advertising--sticky');
        } else {
            articleSidebarAdParent.removeClass('advertising--sticky');
        }
        if (stickyFloor <= 40) {
            articleSidebarAd.css('top', stickyFloor - 40 + 'px');
        } else {
            articleSidebarAd.css('top', '');
        }
    }
});

},{}],3:[function(require,module,exports){
/* global analyticsEvent, analytics_data, angular */
'use strict';

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var _controllersFormController = require('../controllers/form-controller');

var _controllersFormController2 = _interopRequireDefault(_controllersFormController);

var _jscookie = require('../jscookie');

var _jscookie2 = _interopRequireDefault(_jscookie);

var _controllersAnalyticsController = require('../controllers/analytics-controller');

/* * *
SAVE SEARCH
This component handles saving searches from the Search page, as well as setting alerts
for topics from Home/Topic pages. Dispite the naming differences, the back-end functionality
is the same - topic alerts are actually just saved searches for the topic,
plus an email alert for new articles.
* * */

function getParameterByName(name, url) {
	if (!url) {
		url = window.location.href;
	}
	name = name.replace(/[\[\]]/g, "\\$&");
	var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
	    results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, " "));
}

$(document).ready(function () {

	// When the Save Search pop-out is toggled, need to update some form fields
	// with the most recent data. Used to use Angular for this, but for site-wide
	// reusability we need to do it in Zepto.
	$('.js-save-search').on('click', function (e) {
		$('.js-save-search-url').val(window.location.pathname + window.location.hash);
		$('.js-save-search-title').val($('#js-search-field').val());
	});

	// Populates topic alert data when a user is logging in and saving simultaneously
	$('.js-update-topic-alert').on('click', function (e) {
		$('.js-save-search-url').val($(this).data('topic-alert-url'));
		// Search/Topic title exists as <input> and <span>, needs two techniques to properly
		// update the values.
		$('.js-save-search-title').val($(this).data('topic-alert-title')).html($(this).data('topic-alert-title'));
	});

	$('.js-set-topic-alert').on('click', function (e) {

		var isSettingAlert = !$(this).data('has-topic-alert');
		var topicLabel = $(this).find('.js-set-topic-label');

		$('.js-save-search-url').val($(this).data('topic-alert-url'));
		$('.js-save-search-title').val($(this).data('topic-alert-title'));

		if (isSettingAlert) {
			$('.form-save-search').find('button[type=submit]').click();
			topicLabel.html(topicLabel.data('label-is-set'));
			$(this).data('has-topic-alert', 'true');
			$(this).find('.js-topic-icon-unset').removeClass('is-active');
			$(this).find('.js-topic-icon-set').addClass('is-active');
		} else {
			window.lightboxController.showLightbox($(this));
		}
	});

	var savedSearch = getParameterByName("ss");
	if (savedSearch != null && savedSearch == "true") {
		$('.js-saved-search-success-alert').addClass('is-active').on('animationend', function (e) {
			$(e.target).removeClass('is-active');
		}).addClass('a-fade-alert');
	}

	var removeTopicAlert = new _controllersFormController2['default']({
		observe: '.form-remove-topic-alert',
		successCallback: function successCallback(form, context, event) {
			$(form).find('.js-set-topic-label').html($(form).find('.js-set-topic-label').data('label-not-set'));
			$(form).find('.js-set-topic-alert').data('has-topic-alert', null);
			$(form).find('.js-topic-icon-unset').addClass('is-active');
			$(form).find('.js-topic-icon-set').removeClass('is-active');

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, $(form).data('analytics')));
		}
	});

	var saveSearchController = new _controllersFormController2['default']({
		observe: '.form-save-search',
		successCallback: function successCallback(form, context, event) {

			// If there's a stashed search, remove it.
			_jscookie2['default'].remove('saveStashedSearch');

			window.controlPopOuts.closePopOut($(form).closest('.pop-out'));
			$('.js-saved-search-success-alert').addClass('is-active').on('animationend', function (e) {
				$(e.target).removeClass('is-active');
			}).addClass('a-fade-alert');

			window.lightboxController.closeLightboxModal();

			if (typeof angular !== 'undefined') {
				angular.element($('.js-saved-search-controller')[0]).controller().searchIsSaved();
			}

			var event_data = {};

			if ($(form).data('is-search') === true) {
				event_data.event_name = "toolbar_use";
				event_data.toolbar_tool = "save_search";
			} else {
				event_data.event_name = "set_alert";
				event_data.alert_topic = $(form).find('.js-save-search-title').val();
			}

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		},
		beforeRequest: function beforeRequest(form) {
			if (!$(form).find('.js-save-search-title').val().trim()) {
				$('.js-form-error-EmptyTitle').show();
			}
		}
	});

	var saveSearchLoginController = new _controllersFormController2['default']({
		observe: '.form-save-search-login',
		successCallback: function successCallback(form, context, event) {
			_jscookie2['default'].set('saveStashedSearch', {
				'Title': $('.js-save-search-title').val(),
				'Url': $('.js-save-search-url').val(),
				'AlertEnabled': $('#AlertEnabled').prop('checked')
			});

			$.ajax({
				type: "POST",
				url: "/api/SavedSearches",
				data: {
					url: $('.js-save-search-url').val(),
					title: $('.js-save-search-title').val(),
					alertEnabled: $('#AlertEnabled').prop('checked')
				}
			});

			var loginAnalytics = {
				event_name: 'login',
				login_state: 'successful',
				userName: '"' + $(form).find('input[name=username]').val() + '"'
			};
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, loginAnalytics));

			var ssParam = getParameterByName("ss");
			var searchVal = window.location.search;
			if (ssParam == null) {
				searchVal = searchVal.length < 1 ? "?ss=true" : searchVal + "&ss=true";
			}

			if (ssParam == window.location.search) window.location.reload(true);else window.location = window.location.pathname + searchVal + window.location.hash;
		}
	});

	var toggleSavedSearchAlertController = new _controllersFormController2['default']({
		observe: '.form-toggle-saved-search-alert',
		successCallback: function successCallback(form, context, e) {
			var alertToggle = $(form).find('.js-saved-search-alert-toggle');
			var val = alertToggle.val();
			var event_data = {
				saved_search_alert_title: $(form).data('analytics-title'),
				saved_search_alert_publication: $(form).data('analytics-publication')
			};

			if (val === "on") {
				event_data.event_name = 'saved_search_alert_off';
				alertToggle.val('off');
			} else {
				event_data.event_name = 'saved_search_alert_on';
				alertToggle.val('on');
			}

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		}
	});

	$('.js-saved-search-alert-toggle').on('click', function (e) {
		$(e.target.form).find('button[type=submit]').click();
	});

	// On page load, check for any stashed searches that need to be saved
	var saveStashedSearch = _jscookie2['default'].getJSON('saveStashedSearch');

	if (saveStashedSearch) {
		// Set `Save Search` values from stashed search data
		$('.js-save-search-title').val(saveStashedSearch['Title']);
		$('.js-save-search-url').val(saveStashedSearch['Url']);
		$('#AlertEnabled').prop('checked', saveStashedSearch['AlertEnabled']);

		// Save the stashed search if Search (Angular) page
		if (typeof angular !== 'undefined') {
			$('.form-save-search').find('button[type=submit]').click();
		} else {
			$('.js-set-topic-alert').each(function (index, item) {
				if ($(item).data('topic-alert-url') === saveStashedSearch['Url']) {
					$(item).click();
					// If there's a stashed search, remove it.
					_jscookie2['default'].remove('saveStashedSearch');
				}
			});
		}
	}

	var removeSavedSearch = new _controllersFormController2['default']({
		observe: '.form-remove-saved-search',
		successCallback: function successCallback(form, context, evt) {
			$(evt.target).closest('tr').remove();

			window.controlPopOuts.closePopOut($(form).closest('.pop-out'));
			$('.js-saved-search-success-alert').addClass('is-active').on('animationend', function (e) {
				console.log("save search component:6");
				$(e.target).removeClass('is-active');
			}).addClass('a-fade-alert');

			window.lightboxController.closeLightboxModal();

			var event_data = {
				event_name: 'saved_search_alert_removal',
				saved_search_alert_title: $(form).data('analytics-title'),
				saved_search_alert_publication: $(form).data('analytics-publication')
			};

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		}
	});
});

},{"../controllers/analytics-controller":4,"../controllers/form-controller":6,"../jscookie":14}],4:[function(require,module,exports){
// * * *
//  ANALYTICS CONTROLLER
//  For ease-of-use, better DRY, better prevention of JS errors when ads are blocked
// * * *

'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
function analyticsEvent(dataObj) {
    if (typeof utag !== 'undefined') {
        utag.link(dataObj);
    }
};

exports.analyticsEvent = analyticsEvent;

},{}],5:[function(require,module,exports){
/* globals analytics_data */
'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});

var _analyticsController = require('./analytics-controller');

function bookmarkController() {

    // * * *
    //  Article bookmarking logic goes here
    // * * *
    this.toggle = function (e) {

        var bookmark = {
            elm: $(e)
        };

        // ID of the article we're bookmarking or un-bookmarking
        bookmark.id = bookmark.elm.closest('.js-bookmark-article').data('bookmark-id');

        // Stash the bookmark label data now, swap label text later
        bookmark.label = {
            elm: bookmark.elm.find('.js-bookmark-label')
        };
        bookmark.label.bookmark = bookmark.label.elm.data('label-bookmark');
        bookmark.label.bookmarked = bookmark.label.elm.data('label-bookmarked');

        // Are we bookmarking an article, or un-bookmarking?
        // Used later to know what API endpoint to hit, and what DOM changes are required
        bookmark.isBookmarking = bookmark.elm.data('is-bookmarked') ? false : true;

        var apiEndpoint = bookmark.isBookmarking ? '/Account/api/SavedDocumentApi/SaveItem/' : '/Account/api/SavedDocumentApi/RemoveItem/';

        if (bookmark.id) {
            $.ajax({
                url: apiEndpoint,
                type: 'POST',
                data: {
                    DocumentID: bookmark.id
                },
                context: this,
                success: function success(response) {
                    if (response.success) {

                        if (bookmark.isBookmarking) {
                            (0, _analyticsController.analyticsEvent)($.extend(analytics_data, $(bookmark.elm).data('analytics')));
                        }

                        this.flipIcon(bookmark);
                        return true;
                    } else {}
                },
                error: function error(response) {
                    return false;
                }
            });
        }
    };

    this.flipIcon = function (bookmark) {

        if (!bookmark.elm.hasClass('js-angular-bookmark')) {
            $(bookmark.elm).find('.article-bookmark').removeClass('is-visible');
        }

        if (bookmark.isBookmarking) {
            if (!bookmark.elm.hasClass('js-angular-bookmark')) {
                $(bookmark.elm).find('.article-bookmark__bookmarked').addClass('is-visible');
                bookmark.elm.data('is-bookmarked', true);
            }
            bookmark.label.elm.html(bookmark.label.bookmarked);
        } else {
            if (!bookmark.elm.hasClass('js-angular-bookmark')) {
                $(bookmark.elm).find('.article-bookmark').not('.article-bookmark__bookmarked').addClass('is-visible');
                bookmark.elm.data('is-bookmarked', null);
            }
            bookmark.label.elm.html(bookmark.label.bookmark);
        }
    };
}

exports['default'] = bookmarkController;
module.exports = exports['default'];

},{"./analytics-controller":4}],6:[function(require,module,exports){
/*

opts.observe — Form element(s) to observe
opts.beforeRequest — Function to execute before making Ajax request
opts.successCallback — If Ajax request is successful, callback
opts.failureCallback — If Ajax request fails / returns false, callback

*/

'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});
function formController(opts) {

	var showSuccessMessage = function showSuccessMessage(form) {
		$(form).find('.js-form-success').show();
	};

	var showError = function showError(form, error) {
		if ($(form).find(error)) {
			$(form).find(error).show();
		}
	};

	var hideErrors = function hideErrors(form) {
		$(form).find('.js-form-error').hide();
	};

	(function init() {

		var form = opts.observe;

		if (!form) return false;

		var formSubmit = $(form).find('button[type=submit]');

		$(formSubmit).on('click', function (event) {

			// Some forms will require user confirmation before action is taken
			// Default to true (confirmed), set to false later if confirmation is
			// required and user cancels action
			var actionConfirmed = true;

			var currentForm;
			if (event.target.form) {
				currentForm = event.target.form;
			} else {
				currentForm = $(event.target).closest('form');
			}

			if ($(currentForm).data('force-confirm')) {
				actionConfirmed = window.confirm($(currentForm).data('force-confirm'));
			}

			if (actionConfirmed) {

				event.preventDefault(); // Prevent form submitting

				hideErrors(currentForm); // Reset any visible errors

				if (opts.beforeRequest) {
					opts.beforeRequest(currentForm);
				}

				// Prevent user from re-submitting form, unless explicitly allowed
				if (!$(currentForm).data('prevent-disabling')) {
					$(formSubmit).attr('disabled', 'disabled');
				}

				var inputData = {};
				var IsValid = true; //Skip Validation if the form is not Update Contact Informatin Form
				if ($(currentForm).hasClass('form-update-account-contact')) {
					IsValid = ValidateContactInforForm();
				}
				if (IsValid) {
					$(currentForm).find('input, select, textarea').each(function () {

						var value = '';
						var field = $(this);

						if (field.data('checkbox-type') === 'boolean') {
							value = this.checked;

							if (field.data('checkbox-boolean-type') === 'reverse') {
								value = !value;
							}
						} else if (field.data('checkbox-type') === 'value') {
							value = this.checked ? field.val() : undefined;
						} else {
							value = field.val();
						}

						if (value !== undefined) {
							if (inputData[field.attr('name')] === undefined) {
								inputData[field.attr('name')] = value;
							} else if ($.isArray(inputData[field.attr('name')])) {
								inputData[field.attr('name')].push(value);
							} else {
								inputData[field.attr('name')] = [inputData[field.attr('name')]];
								inputData[field.attr('name')].push(value);
							}
						}
					});

					// add recaptcha if it exists in the form
					var captchaResponse = grecaptcha == null ? undefined : grecaptcha.getResponse();
					if (captchaResponse !== undefined) inputData['RecaptchaResponse'] = captchaResponse;

					if (!$(currentForm).data('on-submit')) {
						console.warn('No submit link for form');
					}
					try {
						for (var index in inputData) {
							if (inputData[index] == "- Select One -") {
								inputData[index] = "";
							}
						}
					} catch (ex) {
						console.log(ex);
					}

					$.ajax({
						url: $(currentForm).data('on-submit'),
						type: $(currentForm).data('submit-type') || 'POST',
						data: inputData,
						context: this,
						success: function success(response) {
							if (response.success) {

								showSuccessMessage(currentForm);

								// Passes the form response through with the "context"
								// successCallback is ripe for refactoring, improving parameters
								this.response = response;

								if (opts.successCallback) {
									opts.successCallback(currentForm, this, event);
								}

								if ($(form).data('on-success')) {
									window.location.href = $(currentForm).data('on-success');
								}
							} else {
								if (response.reasons && response.reasons.length > 0) {
									for (var reason in response.reasons) {
										showError(form, '.js-form-error-' + response.reasons[reason]);
									}
								} else {
									showError(currentForm, '.js-form-error-general');
								}

								if (opts.failureCallback) {
									opts.failureCallback(currentForm, response);
								}
							}
						},
						error: function error(response) {

							showError(currentForm, '.js-form-error-general');

							if (opts.failureCallback) {
								opts.failureCallback(currentForm, response);
							}
						},
						complete: function complete() {
							setTimeout(function () {
								$(formSubmit).removeAttr('disabled');
							}, 250);

							// reset captcha if available
							grecaptcha.reset();
						}

					});
				} // if actionConfirmed
			}
			return false;
		});
	})();
}
function ValidateContactInforForm() {
	var errorHtml = $('#errorMessage').html();
	var errors = 0;
	var result = false;
	var scrollTo = '';
	$('.required').each(function () {
		if ($(this).val() == '' || $(this).text().indexOf("- Select One -") >= 0) {
			$(this).parent().append(errorHtml);
			errors++;
			if (errors == 1) {
				scrollTo = $(this);
			}
		} else {
			$(this).parent().find('.js-form-error').remove();
		}
	});
	if (errors > 0) {
		window.scrollTo(0, scrollTo.offset().top - 30);
		result = false;
	} else {
		result = true;
	}
	return result;
}

exports['default'] = formController;
module.exports = exports['default'];

},{}],7:[function(require,module,exports){
/* global angular */
'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
function lightboxModalController() {

    this.closeLightboxModal = function () {
        $('body').removeClass('lightboxed');
        $('.lightbox-modal__backdrop').remove();
        $('.lightbox-modal').hide();
    };

    var closeLightboxModal = this.closeLightboxModal;

    this.showLightbox = function (lightbox) {
        // Freeze the page and add the dark overlay
        $('body').addClass('lightboxed').append('<div class="lightbox-modal__backdrop"></div>');

        // Find the specific modal for this trigger, and the associated form
        var targetModal = $(lightbox).data('lightbox-modal');
        var successForm = $(lightbox).closest('.' + $(lightbox).data('lightbox-modal-success-target'));

        // Show the modal, add an on-click listener for the "success" button
        $('.' + targetModal).show().find('.js-lightbox-modal-submit')
        // .one, not .on, to prevent stacking event listners
        .one('click', function (e) {
            successForm.find('button[type=submit]').click();
            closeLightboxModal();
        });

        return false;
    };

    var showLightbox = this.showLightbox;

    this.buildLightboxes = function () {
        $('.js-lightbox-modal-trigger').on('click', function (e) {

            if (e.target !== this) {
                this.click();
                return;
            }

            showLightbox(e.target);

            // Don't submit any forms for real.
            return false;
        });
    };

    // When the Dismiss button is clicked...
    $('.js-close-lightbox-modal').on('click', function (e) {
        closeLightboxModal();
    });

    this.buildLightboxes();

    this.clearLightboxes = function () {
        $('.js-lightbox-modal-trigger').off();
    };
}

exports['default'] = lightboxModalController;
module.exports = exports['default'];

},{}],8:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});
function popOutController(triggerElm) {
	var _this = this;

	// Toggle pop-out when trigger is clicked
	if (triggerElm) {
		$(triggerElm).off();
		$(triggerElm).on('click', function (event) {
			event.preventDefault();
			_this.togglePopOut($(event.target));
		});
	}

	// Reposition pop-out when browser window resizes
	$(window).on('resize', function (event) {
		_this.updatePopOut();
	});

	// Simulate CSS `rem` (16px)
	// TODO: change this from `rem` to tab padding value for clarity
	var rem = 16;

	// Keep track of the active pop-out element
	// This is an object instead of a var because there might be more "global"
	// state attributes to track in the future.
	var state = {
		activeElm: null,
		customized: {}
	};

	// PUBLIC
	// Get the current pop-out element, if there is one.
	// Lets other JS know what's up with the pop-out.
	this.getPopOutElement = function () {
		return state.activeElm;
	};

	// PUBLIC
	// Closes the pop-out.
	this.closePopOut = function (elm) {
		// Reset all z-indexes so new pop-outs are stacked on top properly
		$('.pop-out').removeClass('is-active').css("z-index", "");
		$('.js-pop-out-trigger').css("z-index", "");
	};

	// PUBLIC
	// Toggles the pop-out
	this.togglePopOut = function (e) {
		// Check if clicked element is the toggle itself
		// Otherwise, climb up DOM tree and find it
		var poParent = e.hasClass('js-pop-out-trigger') ? e : e.closest('.js-pop-out-trigger');

		/*  This is a little hacky, but if a user is trying to bookmark an article
  	but needs to sign in first, we need to capture and pass the article
  	ID as a URL param after a successful sign in attempt. That allows
  	us to automatically bookmark the article on page refresh. */

		if (poParent.data('pop-out-type') === 'sign-in' && poParent.data('bookmark-id')) {
			$('.sign-in__submit').data('pass-article-id', poParent.data('bookmark-id'));
		} else {
			poParent.data('bookmark-id', null);
		}

		// Close all pop-outs
		this.closePopOut();

		if (poParent[0] !== state.activeElm) {
			// Update the controller state and open it
			state.activeElm = poParent[0];
			updatePosition();
		} else {
			state.activeElm = null;
		}
	};

	// PUBLIC
	// Stores pop-out customization details for reference when rendering
	this.customize = function (obj) {
		state.customized[obj.id] = obj;
	};

	// PRIVATE
	// Update the visibility and position of the pop-out box and tab.
	var updatePosition = function updatePosition() {

		var trgr = { // The pop-out trigger
			e: $(state.activeElm)
		};
		// Get trigger height, width, offsetTop, offsetWidth
		trgr.offset = trgr.e.offset();

		trgr.hasStyles = state.customized[trgr.e.data('pop-out-id')];

		// Determine which pop-out template to use
		// TODO: Make this user-configurable
		// Let users assign a name to a template class
		var popOut;
		switch (trgr.e.data('pop-out-type')) {
			// SIGN IN
			// (Global sign-in, bookmarking when not signed in)
			case 'sign-in':
				popOut = $('.js-pop-out__sign-in');
				break;
			// Main Sign In button on top right
			case 'sign-in-header':
				popOut = $('.js-pop-out__sign-in-header');
				break;
			// EMAIL ARTICLE
			case 'email-article':
				popOut = $('.js-pop-out__email-article');
				break;
			// EMAIL ARTICLE
			case 'email-search':
				popOut = $('.js-pop-out__email-search');
				break;
			// EMAIL AUTHOR
			case 'email-author':
				popOut = $('.js-pop-out__email-author');
				break;
			// EMAIL COMPANy
			case 'email-company':
				popOut = $('.js-pop-out__email-company');
				break;
			// EMAIL DEAL
			case 'email-deal':
				popOut = $('.js-pop-out__email-deal');
				break;
			// GLOBAL HEADER REGISTRATION
			case 'register':
				popOut = $('.js-pop-out__register');
				break;
			// SEARCH PAGE - SAVE SEARCH
			case 'save-search':
				popOut = $('.js-pop-out__save-search');
				break;
			case 'ask-the-analyst':
				popOut = $('.js-pop-out__ask-the-analyst');
				break;
			default:
				console.warn('Attempting to fire unidentified pop-out.');
				return;
		}

		// Make pop-out visible so we can query for its width
		popOut.addClass('is-active');

		// Check if browser is less than or equal to `small` CSS breakpoint

		var isNarrow = $(window).width() <= 480;
		var isTablet = $(window).width() <= 800;

		// Set separate vertical/horizontal padding on mobile vs. desktop
		var vPad = isNarrow ? 10 : rem;
		var hPad = isNarrow ? 14 : rem;

		// Store output values after calculations, etc.
		var res = {
			offset: {
				box: {},
				tab: {}
			},
			css: {
				box: {},
				tab: {}
			}
		};

		// Box offset top is offsetTop of trigger, plus trigger height,
		// plus padding, minus 1px for border positioning
		res.offset.box.top = Math.floor(trgr.offset.top + trgr.offset.height + (vPad - 1));

		// Check if pop-out will bleed off-screen, causing horizontal scroll bar
		// If it will, force right-align to keep it on-screen
		if (popOut.width() + trgr.offset.left > $(window).width()) {
			trgr.e.data('pop-out-align', 'right');
		}

		// Check for pop-out alignment
		if (trgr.e.data('pop-out-align') === 'right' && !isNarrow) {
			// Pop-out box is flush right with trigger element
			// To flush right, first add trigger offset plus trigger width
			// This positions left edge of pop-out with right edge of trigger
			// Then subtract pop-out width and padding to align both right edges
			// (Flush-left automatically if narrow window)
			res.offset.box.left = isNarrow ? 0 : Math.floor(trgr.offset.left + trgr.offset.width - popOut.offset().width + (hPad - 1));
			// Tab left margin can be ignored, right margin 0 does what we need
			res.offset.tab.left = 'auto';
		} else {
			// Pop-out box is centered with trigger element
			// Box offset left is determined by subtracting the trigger width
			// from the pop-out width, dividing by 2 to find the halfway point,
			// then subtracting that from the trigger left offset.
			// (Flush-left automatically if narrow window)
			res.offset.box.left = isNarrow ? 0 : Math.floor(trgr.offset.left - (popOut.offset().width - trgr.offset.width) / 2);
			// Pop-out tab is aligned with trigger left edge, adjusted for padding
			// Tab width is set to trigger width below, so this centers the tab
			res.offset.tab.left = isNarrow ? Math.floor(trgr.offset.left - hPad) : 0;
		}

		// Blow up z-index to appear above other triggers
		trgr.e.css('z-index', '9999');

		// Box z-index set to 2 lower than trigger element
		// Box should render below trigger, under tab, above everything else
		res.css.box.zIndex = trgr.e.css('z-index') - 2;

		// Tab height equals trigger height plus padding (1rem top and bottom)

		// Check for custom tab styles
		var tS = trgr.hasStyles ? trgr.hasStyles.tabStyles : undefined;

		// If there are custom styles, and browser is desktop-width...
		if (tS && !isNarrow && !isTablet) {

			res.css.tab.height = tS.deskHeight || trgr.offset.height + vPad * 2 + "px";

			tS.deskHeight ? res.offset.box.top += tS.deskHeight - trgr.offset.height - vPad * 2 : null;

			res.css.tab.top = tS.deskHeight ? '-' + (tS.deskHeight - 1) + 'px' : '-' + (trgr.offset.height + vPad * 2 - 1) + 'px';

			// If there are custom styles, and browser is tablet-width...
		} else if (tS && !isNarrow && isTablet) {

				res.css.tab.height = tS.tabletHeight || trgr.offset.height + vPad * 2 + "px";

				tS.tabletHeight ? res.offset.box.top += tS.tabletHeight - trgr.offset.height - vPad * 2 : null;

				res.css.tab.top = tS.tabletHeight ? '-' + (tS.tabletHeight - 1) + 'px' : '-' + (trgr.offset.height + vPad * 2 - 1) + 'px';

				// If there are custom styles, and browser is phone-width...
			} else if (tS && isNarrow) {

					res.css.tab.height = tS.phoneHeight || trgr.offset.height + vPad * 2 + "px";

					tS.phoneHeight ? res.offset.box.top += tS.phoneHeight - trgr.offset.height - vPad * 2 : null;

					res.css.tab.top = tS.phoneHeight ? '-' + (tS.phoneHeight - 1) + 'px' : '-' + (trgr.offset.height + vPad * 2 - 1) + 'px';

					// Default padding/positioning
				} else {

						res.css.tab.height = trgr.offset.height + vPad * 2 + "px";

						// Move the tab upwards, equal to the trigger height plus padding
						// minus 1px to account for border and visually overlapping box
						res.css.tab.top = '-' + (trgr.offset.height + vPad * 2 - 1) + "px";
					}

		// Tab width equals trigger width plus padding (1rem left and right)
		res.css.tab.width = trgr.offset.width + hPad * 2 + "px";

		// Tab z-index is 1 less than trigger; above box, below trigger
		res.css.tab.zIndex = trgr.e.css('z-index') - 1;

		// `transform` to quickly position box, relative to top left corner
		res.css.box.transform = 'translate3d(' + res.offset.box.left + 'px, ' + res.offset.box.top + 'px, 0)';

		// Apply that giant blob of CSS
		popOut.css({
			zIndex: res.css.box.zIndex,
			transform: res.css.box.transform
		}).find('.pop-out__tab').css({ // find this pop-out's child tab
			height: res.css.tab.height,
			width: res.css.tab.width,
			left: res.offset.tab.left,
			right: 0, // This is always 0
			top: res.css.tab.top,
			zIndex: res.css.tab.zIndex
		});
		// Ugly hack for Safari 8, booo
		popOut.css('-webkit-transform', res.css.box.transform);
	};

	// If there is an active pop-out, update its position
	// Mostly useful for when the browser window resizes
	this.updatePopOut = function () {
		if (state.activeElm) {
			updatePosition();
		}
	};
}

exports['default'] = popOutController;
module.exports = exports['default'];

},{}],9:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});

var _analyticsController = require('./analytics-controller');

function loginController(requestVerificationToken) {
	this.addRegisterUserControl = function (triggerElement, successCallback, failureCallback) {
		var _this = this;

		if (triggerElement) {
			$(triggerElement).on('click', function (event) {
				_this.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('register-user-url');

				$(triggerElement).parents('.js-register-user-container').find('input').each(function () {
					var value = '';

					if ($(this).data('checkbox-type') === 'boolean') {
						value = this.checked;

						if ($(this).data('checkbox-boolean-type') === 'reverse') {
							value = !value;
						}
					} else {
						value = $(this).val();
					}

					inputData[$(this).attr('name')] = value;
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: _this,
					success: function success(response) {
						if (response.success) {

							var registerAnalytics = {
								event_name: 'register-step-1',
								registration_state: 'successful',
								userName: '"' + inputData.username + '"'
							};

							(0, _analyticsController.analyticsEvent)($.extend(analytics_data, registerAnalytics));

							if (successCallback) {
								successCallback(triggerElement);
							}

							var nextStepUrl = $(triggerElement).data('next-step-url');

							if (nextStepUrl) {
								window.location.href = nextStepUrl;
							}

							this.showSuccessMessage(triggerElement);
						} else {
							$(triggerElement).removeAttr('disabled');

							var specificErrorDisplayed = false;

							if (response.reasons && response.reasons.length > 0) {
								for (var reason in response.reasons) {
									this.showError(triggerElement, '.js-register-user-error-' + response.reasons[reason]);
								}

								specificErrorDisplayed = true;
							}

							if (!specificErrorDisplayed) {
								this.showError(triggerElement, '.js-register-user-error-general');
							}

							var registerAnalytics = {
								event_name: "registration failure",
								registration_errors: response.reasons
							};

							(0, _analyticsController.analyticsEvent)($.extend(analytics_data, registerAnalytics));

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function error(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-register-user-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.showSuccessMessage = function (triggerElement) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-success').show();
	};

	this.showError = function (triggerElement, error) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error-container').show();
		$(triggerElement).parents('.js-register-user-container').find(error).show();
	};

	this.hideErrors = function (triggerElement) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error-container').hide();
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error').hide();
	};
};

exports['default'] = loginController;
module.exports = exports['default'];

},{"./analytics-controller":4}],10:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});

var _analyticsController = require('./analytics-controller');

function loginController(requestVerificationToken) {
	this.addRequestControl = function (triggerElement, successCallback, failureCallback) {
		var _this = this;

		if (triggerElement) {
			$(triggerElement).on('click', function (event) {
				_this.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('reset-url');

				$(triggerElement).parents('.js-reset-password-container').find('input').each(function () {
					inputData[$(this).attr('name')] = $(this).val();
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: _this,
					success: function success(response) {
						if (response.success) {
							this.showSuccessMessage(triggerElement);

							if (successCallback) {
								successCallback(triggerElement);
							}
						} else {
							$(triggerElement).removeAttr('disabled');
							var resetPasswordAnalytics = {
								event_name: "password reset unsuccessful"
							};

							var specificErrorDisplayed = false;

							if ($.inArray('EmailRequirement', response.reasons) !== -1) {
								this.showError(triggerElement, '.js-reset-password-error-email');
								specificErrorDisplayed = true;
							}

							if (!specificErrorDisplayed) {
								this.showError(triggerElement, '.js-reset-password-error-general');
							}

							(0, _analyticsController.analyticsEvent)($.extend(analytics_data, resetPasswordAnalytics));

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function error(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-reset-password-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.addChangeControl = function (triggerElement, successCallback, failureCallback) {
		var _this2 = this;

		if (triggerElement) {
			$(triggerElement).on('click', function (event) {
				_this2.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('reset-url');

				$(triggerElement).parents('.js-reset-password-container').find('input').each(function () {
					inputData[$(this).attr('name')] = $(this).val();
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: _this2,
					success: function success(response) {
						if (response.success) {
							this.showSuccessMessage(triggerElement);

							if (successCallback) {
								successCallback(triggerElement);
							}
						} else {
							$(triggerElement).removeAttr('disabled');

							var specificErrorDisplayed = false;

							if ($.inArray('PasswordMismatch', response.reasons) !== -1) {
								this.showError(triggerElement, '.js-reset-password-error-mismatch');
								specificErrorDisplayed = true;
							}
							if ($.inArray('PasswordRequirements', response.reasons) !== -1) {
								this.showError(triggerElement, '.js-reset-password-error-requirements');
								specificErrorDisplayed = true;
							}

							if (!specificErrorDisplayed || $.inArray('MissingToken', response.reasons) !== -1) {
								this.showError(triggerElement, '.js-reset-password-error-general');
							}

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function error(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-reset-password-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.addRetryControl = function (triggerElement, successCallback, failureCallback) {
		var _this3 = this;

		if (triggerElement) {
			$(triggerElement).on('click', function (event) {
				_this3.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('retry-url');

				$(triggerElement).parents('.js-reset-password-container').find('input').each(function () {
					inputData[$(this).attr('name')] = $(this).val();
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: _this3,
					success: function success(response) {
						if (response.success) {
							this.showSuccessMessage(triggerElement);

							if (successCallback) {
								successCallback(triggerElement);
							}
						} else {
							$(triggerElement).removeAttr('disabled');

							this.showError(triggerElement, '.js-reset-password-error-general');

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function error(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-reset-password-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.showSuccessMessage = function (triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-success').show();
		var resetPasswordAnalytics = {
			event_name: "password reset success"
		};

		(0, _analyticsController.analyticsEvent)($.extend(analytics_data, resetPasswordAnalytics));
	};

	this.showError = function (triggerElement, error) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-container').show();
		$(triggerElement).parents('.js-reset-password-container').find(error).show();
		var resetPasswordAnalytics = {
			event_name: "password reset unsuccessful"
		};

		(0, _analyticsController.analyticsEvent)($.extend(analytics_data, resetPasswordAnalytics));
	};

	this.hideErrors = function (triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-container').hide();
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error').hide();
	};
};

exports['default'] = loginController;
module.exports = exports['default'];

},{"./analytics-controller":4}],11:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
	value: true
});
function sortableTableController() {

	/*
 Based on SortTable version 2
 7th April 2007
 Stuart Langridge, http://www.kryogenix.org/code/browser/sorttable/
 Licenced as X11: http://www.kryogenix.org/code/browser/licence.html
 */

	var isSortedTable = false;
	var tfo, mtch, sortfn, hasInputs;

	var sorttable = {

		init: function initing() {
			// quit if this function has already been called
			if (isSortedTable) return;
			// flag this function so we don't do the same thing twice
			isSortedTable = true;

			$('.js-sortable-table').each(function (indx, item) {
				sorttable.makeSortable(item);
			});
		},

		sortColumn: function sortColumn(table, col) {

			// build an array to sort. This is a Schwartzian transform thing,
			// i.e., we "decorate" each row with the actual sort key,
			// sort based on the sort keys, and then put the rows back in order
			// which is a lot faster because you only do getInnerText once per row

			var row_array = [];
			var headrow = table.tHead.rows[0].cells;
			var rows = [].slice.call(table.tBodies[0].rows);
			var guesstype = sorttable.guessType(table, col);

			for (var j = 0; j < rows.length; j++) {
				row_array[row_array.length] = [$(rows[j].cells[col]), rows[j]];
			}

			if ($(headrow[col]).data('sortable-type')) {
				row_array.sort(sorttable[$(headrow[col]).data('sortable-type')]);
			} else {
				row_array.sort(guesstype);
			}

			var tb = table.tBodies[0];
			for (var j = 0; j < row_array.length; j++) {
				tb.appendChild(row_array[j][1]);
			}

			row_array = undefined;
		},

		makeSortable: function makeSortable(table) {

			// Sorttable v1 put rows with a class of "sortbottom" at the bottom (as
			// "total" rows, for example). This is B&R, since what you're supposed
			// to do is put them in a tfoot. So, if there are sortbottom rows,
			// for backwards compatibility, move them to tfoot (creating it if needed).
			var sortbottomrows = [];
			for (var i = 0; i < table.rows.length; i++) {
				if ($(table.rows[i]).hasClass('.sortbottom')) {
					sortbottomrows[sortbottomrows.length] = table.rows[i];
				}
			}

			if (sortbottomrows) {
				if (table.tFoot == null) {
					// table doesn't have a tfoot. Create one.
					tfo = document.createElement('tfoot');
					table.appendChild(tfo);
				}
				for (var j = 0; j < sortbottomrows.length; j++) {
					tfo.appendChild(sortbottomrows[j]);
				}
				sortbottomrows = undefined;
			}

			// work through each column and calculate its type
			var headrow = table.tHead.rows[0].cells;
			for (var i = 0; i < headrow.length; i++) {

				// manually override the type with a sorttable_type attribute
				if (!headrow[i].className.match(/\bsorttable_nosort\b/)) {
					// skip this col
					headrow[i].sorttable_sortfunction = sorttable.guessType(table, i);
				}
			};

			$(table).find('.js-sortable-table-sorter').on('click', function (e) {

				// If child element is clicked, redirect the click to the
				// proper element: the parent itself.
				if (e.target !== this) {
					this.click();
					return;
				}

				var colNum = $(e.target).closest('.js-sortable-table-sorter').data('sortable-table-col') - 1;

				if ($(e.target).hasClass('sorttable_sorted')) {
					// This column is sorted top to bottom
					// Re-sort the column to catch any row changes...
					sorttable.sortColumn(table, colNum);
					// ...then reverse the column and update the classes (state).
					sorttable.reverse(table.tBodies[0]);
					$(e.target).removeClass('sorttable_sorted').addClass('sorttable_sorted_reverse');

					return;
				}

				if ($(e.target).hasClass('sorttable_sorted_reverse')) {
					// This column is sorted bottom to top
					// Flip the table back to top-to-bottom (default)...
					sorttable.reverse(table.tBodies[0]);
					// ...then re-sort it to catch any row changes.
					sorttable.sortColumn(table, colNum);
					$(e.target).removeClass('sorttable_sorted_reverse').addClass('sorttable_sorted');

					return;
				}

				// remove sorttable_sorted classes
				var theadrow = e.target.parentNode;
				forEach(theadrow.childNodes, function (cell) {
					if (cell.nodeType == 1) {
						// an element
						$(cell).removeClass('sorttable_sorted_reverse sorttable_sorted');
					}
				});

				if ($('.sorttable_sortfwdind')) {
					$('.sorttable_sortfwdind').remove();
				}

				if ($('.sorttable_sortrevind')) {
					$('.sorttable_sortrevind').remove();
				}

				$(e.target).addClass('sorttable_sorted');

				sorttable.sortColumn(table, colNum);
			});
		},

		guessType: function guessType(table, column) {

			// guess the type of a column based on its first non-blank row
			sortfn = sorttable.sort_alpha;

			for (var i = 0; i < table.tBodies[0].rows.length; i++) {

				var text = $(table.tBodies[0].rows[i].cells[column]).text().trim();
				if (text != '') {
					// If column is numeric or appears to be money, sort numeric
					if (text.match(/^-?[£$¤]?[\d,.]+%?$/)) {
						return sorttable.sort_numeric;
					} else if (Date.parse(text) > 0) {
						// Check for valid date
						// If found, assume column is full of dates, sort by date!
						return sorttable.sort_by_date;
					} else {
						return sorttable.sort_alpha;
					}
				}
			}
			return sortfn;
		},

		reverse: function reverse(tbody) {
			// reverse the rows in a tbody
			var newrows = [];
			for (var i = 0; i < tbody.rows.length; i++) {
				newrows[newrows.length] = tbody.rows[i];
			}
			for (var i = newrows.length - 1; i >= 0; i--) {
				tbody.appendChild(newrows[i]);
			}
			newrows = undefined;
		},

		/* sort functions
  each sort function takes two parameters, a and b
  you are comparing a[0] and b[0] */
		sort_numeric: function sort_numeric(a, b) {
			var aa = parseFloat(a[0].replace(/[^0-9.-]/g, ''));
			if (isNaN(aa)) aa = 0;
			var bb = parseFloat(b[0].replace(/[^0-9.-]/g, ''));
			if (isNaN(bb)) bb = 0;
			return aa - bb;
		},
		sort_alpha: function sort_alpha(a, b) {
			var aClean = a[0].text().trim().toUpperCase();
			var bClean = b[0].text().trim().toUpperCase();
			if (aClean == bClean) return 0;
			if (aClean < bClean) return -1;
			return 1;
		},

		sort_by_date: function sort_by_date(a, b) {
			// http://stackoverflow.com/questions/10123953/sort-javascript-object-array-by-date
			// Turn your strings into dates, and then subtract them
			// to get a value that is either negative, positive, or zero.
			return new Date(b[0].text().trim()) - new Date(a[0].text().trim());
		},

		sort_checkbox: function sort_checkbox(a, b) {
			var aChecked = a[0].find('input[type=checkbox]').prop('checked');
			var bChecked = b[0].find('input[type=checkbox]').prop('checked');
			if (aChecked && !bChecked) return 1;
			if (!aChecked && bChecked) return -1;

			return 0;
		},

		shaker_sort: function shaker_sort(list, comp_func) {
			// A stable sort function to allow multi-level sorting of data
			// see: http://en.wikipedia.org/wiki/Cocktail_sort
			// thanks to Joseph Nahmias
			var b = 0;
			var t = list.length - 1;
			var swap = true;

			while (swap) {
				swap = false;
				for (var i = b; i < t; ++i) {
					if (comp_func(list[i], list[i + 1]) > 0) {
						var q = list[i];list[i] = list[i + 1];list[i + 1] = q;
						swap = true;
					}
				} // for
				t--;

				if (!swap) break;

				for (var i = t; i > b; --i) {
					if (comp_func(list[i], list[i - 1]) < 0) {
						var q = list[i];list[i] = list[i - 1];list[i - 1] = q;
						swap = true;
					}
				} // for
				b++;
			} // while(swap)
		}
	};

	/// HELPER FUNCTIONS

	// Dean's forEach: http://dean.edwards.name/base/forEach.js
	/*
 	forEach, version 1.0
 	Copyright 2006, Dean Edwards
 	License: http://www.opensource.org/licenses/mit-license.php
 */

	// array-like enumeration
	if (!Array.forEach) {
		// mozilla already supports this
		Array.forEach = function (array, block, context) {
			for (var i = 0; i < array.length; i++) {
				block.call(context, array[i], i, array);
			}
		};
	}

	// generic enumeration
	Function.prototype.forEach = function (object, block, context) {
		for (var key in object) {
			if (typeof this.prototype[key] == "undefined") {
				block.call(context, object[key], key, object);
			}
		}
	};

	// character enumeration
	String.forEach = function (string, block, context) {
		Array.forEach(string.split(""), function (chr, index) {
			block.call(context, chr, index, string);
		});
	};

	// globally resolve forEach enumeration
	var forEach = function forEach(object, block, context) {
		if (object) {
			var resolve = Object; // default
			if (object instanceof Function) {
				// functions have a "length" property
				resolve = Function;
			} else if (object.forEach instanceof Function) {
				// the object implements a custom forEach method so use that
				object.forEach(block, context);
				return;
			} else if (typeof object == "string") {
				// the object is a string
				resolve = String;
			} else if (typeof object.length == "number") {
				// the object is array-like
				resolve = Array;
			}
			resolve.forEach(object, block, context);
		}
	};

	sorttable.init();
};

exports['default'] = sortableTableController;
module.exports = exports['default'];

},{}],12:[function(require,module,exports){
/* global tooltipController */

"use strict";

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports["default"] = createPopup;

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

var _calculatePopupOffsetsJs = require("../calculatePopupOffsets.js");

var _calculatePopupOffsetsJs2 = _interopRequireDefault(_calculatePopupOffsetsJs);

/**
 * creates a popup and injects it in to the document.
 * @param  {String} title : title of the popup, optional
 * @param  {String} html : content of the popup as html
 * @param  {Number} top  : top position relative to the document
 * @param  {Number} left : left position relative to the document
 * @param  {Number} offset : a vertical or horizonal distance from the top/left the triangle should point
 *                           depending on if the triangle is top/bottom or left/right
 * @param  {String} container : a selector of the container in which to inject the popup
 * @param  {String} triangle : "top", "right", "bottom", or "left"
 * @param  {Boolean} flipToContain: will flip the popup if it goes outside the parent container
 * @param  {Boolean} closeBtn : if the X should be shown.  If not, the popup with not have pointer events
 *                              this is useful for popups on hover that shouldn't fire mouseenter/leave
 * @param  {Boolean} isHidden: Whether or not the popup is visible.
 * @return {Object} {
 *     {Function} hidePopup   : will set isHidden to true
 *     {Function} removePopup : will remove the popup from the DOM),
 *     {Function} setState : pass an object with any keys from above to update the popup.
 * }
 */

function createPopup(initialState) {

    // defaults, and this object will hold the previous state after setState
    var prevState = {
        title: "",
        html: "",
        top: 0,
        left: 0,
        width: "",
        offset: 0,
        container: "body",
        triangle: "bottom",
        closeBtn: true,
        isHidden: false,
        flipToContain: false
    };

    var state = $.extend({}, prevState, initialState);

    function setState(newState) {

        // copy the old state into prevState
        prevState = $.extend({}, state);

        $.extend(state, newState);

        // console.log(state);

        render();
    }

    // initialize popup
    // always start hidden so it can animate in
    var $popup = $("<div class='popup'>").css({
        "opacity": 0,
        "width": state.width,
        "transform": "scale(0.89)",
        "pointer-events": state.closeBtn ? "auto" : "none"
    });
    var $titleDiv = $("<div>").addClass("popup__title");
    var $triangleDiv = $("<div>").addClass("popup__triangle");
    var $content = $("<div>").addClass("popup__content");

    // attach the close button if we're supposed to
    if (state.closeBtn) {
        var $xDiv = $("<div>").addClass("popup__x-icon").html("<svg class='svg-x'> <use xlink:href='build/img/svg-sprite.svg#x'></use> </svg>").on("click", removePopup);
        $popup.append($xDiv);

        window.addEventListener("click", handleClickAway, true);
        window.addEventListener("resize", handleClickAway, true);
    }

    $popup.append($titleDiv);
    $popup.append($triangleDiv);
    $popup.append($content);

    $(state.container).append($popup);

    // if the user clicked outside of the popup, close it
    function handleClickAway(e) {
        var inPopup = $(e.target).closest(".popup").length;
        if (!inPopup) {
            removePopup();
        }
    }

    function hidePopup() {

        window.removeEventListener("click", handleClickAway, true);
        window.removeEventListener("resize", handleClickAway, true);

        // only re-render if we need to
        if (state.isHidden !== true) {
            // will kick of the transition
            setState({
                isHidden: true
            });
        }
    }

    function removePopup() {

        // first close it
        hidePopup();

        // when the transition finishes, remove the popup from the DOM
        $popup.on("transitionend", function () {
            $popup.remove();
        });
    }

    // render the first time
    render();

    function render() {
        var top = state.top;
        var left = state.left;
        var offset = state.offset;
        var triangle = state.triangle;
        var isHidden = state.isHidden;
        var html = state.html;
        var title = state.title;
        var flipToContain = state.flipToContain;

        // update the content before calculating the offsets
        $content.html(html);
        $titleDiv.html(title);

        var offsets = (0, _calculatePopupOffsetsJs2["default"])({
            popup: $popup.get(0),
            triangleSize: $triangleDiv.height(),
            top: top, left: left, offset: offset, triangle: triangle, flipToContain: flipToContain
        });

        // if the popup was hidden, we want to place it where it needs to be
        // the update will fade it in
        // enter - put it in place before transitioning in
        if (prevState.isHidden && !isHidden) {
            $popup.css({
                "top": offsets.popupTop + "px",
                "left": offsets.popupLeft + "px"
            });
        }

        $popup
        // .stop().animate({
        .css({
            opacity: isHidden ? 0 : 1,
            transform: isHidden ? "scale(0.9)" : "scale(1)",
            top: offsets.popupTop + 'px',
            left: offsets.popupLeft + 'px'
        }, 250).removeClass(function (index, css) {
            return (css.match(/\bis-triangle-\S+/g) || []).join(" ");
        }).addClass("is-triangle-" + offsets.triangleSide).toggleClass("popup--hidden", isHidden);

        // adjust the triangle
        $triangleDiv.css({
            "transform": offsets.triangleSide === "top" || offsets.triangleSide === "bottom" ? "translateX(" + offsets.triangleOffset + "px)" // top/bottom
            : "translateY(" + offsets.triangleOffset + "px)" // left/right
        });

        $popup.toggleClass("no-title", !title);
    }

    // external api
    return {
        setState: setState,
        hidePopup: hidePopup,
        removePopup: removePopup
    };
}

module.exports = exports["default"];

},{"../calculatePopupOffsets.js":1}],13:[function(require,module,exports){
/* global angular, analytics_data */

// THIRD-PARTY / VENDOR
'use strict';

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var _zeptoMin = require('./zepto.min');

var _zeptoMin2 = _interopRequireDefault(_zeptoMin);

var _svg4everybody = require('./svg4everybody');

var _svg4everybody2 = _interopRequireDefault(_svg4everybody);

var _jscookie = require('./jscookie');

var _jscookie2 = _interopRequireDefault(_jscookie);

// CONTROLLERS

var _controllersFormController = require('./controllers/form-controller');

var _controllersFormController2 = _interopRequireDefault(_controllersFormController);

var _controllersPopOutController = require('./controllers/pop-out-controller');

var _controllersPopOutController2 = _interopRequireDefault(_controllersPopOutController);

var _controllersBookmarkController = require('./controllers/bookmark-controller');

var _controllersBookmarkController2 = _interopRequireDefault(_controllersBookmarkController);

var _controllersResetPasswordController = require('./controllers/reset-password-controller');

var _controllersResetPasswordController2 = _interopRequireDefault(_controllersResetPasswordController);

var _controllersRegisterController = require('./controllers/register-controller');

var _controllersRegisterController2 = _interopRequireDefault(_controllersRegisterController);

var _controllersSortableTableController = require('./controllers/sortable-table-controller');

var _controllersSortableTableController2 = _interopRequireDefault(_controllersSortableTableController);

var _controllersLightboxModalController = require('./controllers/lightbox-modal-controller');

var _controllersLightboxModalController2 = _interopRequireDefault(_controllersLightboxModalController);

var _controllersAnalyticsController = require('./controllers/analytics-controller');

var _controllersTooltipController = require('./controllers/tooltip-controller');

var _controllersTooltipController2 = _interopRequireDefault(_controllersTooltipController);

// COMPONENTS

require('./components/article-sidebar-component');

require('./components/save-search-component');

// OTHER CODE

var _newsletterSignup = require('./newsletter-signup');

var _newsletterSignup2 = _interopRequireDefault(_newsletterSignup);

var _searchPageJs = require('./search-page.js');

var _searchPageJs2 = _interopRequireDefault(_searchPageJs);

var _toggleIcons = require('./toggle-icons');

// Global scope to play nicely with Angular

var _selectivityFull = require('./selectivity-full');

var _selectivityFull2 = _interopRequireDefault(_selectivityFull);

// Make sure proper elm gets the click event
// When a user submits a Forgot Password request, this will display the proper
// success message and hide the form to prevent re-sending.
window.toggleIcons = _toggleIcons.toggleIcons;

/* Polyfill for scripts expecting `jQuery`. Also see: CSS selectors support in zepto.min.js */
window.jQuery = $;
var showForgotPassSuccess = function showForgotPassSuccess() {
	$('.pop-out__sign-in-forgot-password-nested').toggleClass('is-hidden');
	$('.pop-out__sign-in-forgot-password').find('.alert-success').toggleClass('is-active');
};

var renderIframeComponents = function renderIframeComponents() {
	$('.iframe-component').each(function (index, elm) {
		var desktopEmbed = $(elm).find('.iframe-component__desktop');
		var mobileEmbed = $(elm).find('.iframe-component__mobile');

		var isEditMode = $(this).hasClass('is-page-editor');

		var showMobile = $(window).width() <= 480 || isEditMode;
		var showDesktop = !showMobile || isEditMode;

		if (showMobile) {
			mobileEmbed.show();
			if (mobileEmbed.html() == '') mobileEmbed.html(decodeHtml(mobileEmbed.data('embed-link')));
		} else {
			mobileEmbed.hide();
		}

		if (showDesktop) {
			desktopEmbed.show();
			if (desktopEmbed.html() == '') desktopEmbed.html(decodeHtml(desktopEmbed.data('embed-link')));
		} else {
			desktopEmbed.hide();
		}

		var desktopMediaId = $(elm).find('.iframe-component__desktop').data("mediaid");

		var url = window.location.href;
		url.replace("#", "");
		if (url.indexOf("?") < 0) {
			url += "?";
		} else {
			url += "&";
		}

		url += "mobilemedia=true&selectedid=" + desktopMediaId;
		$(elm).find('.iframe-component__mobile a').data('mediaid', url).attr('href', null);
	});
};

var decodeHtml = function decodeHtml(html) {
	var txt = document.createElement("textarea");
	txt.innerHTML = html;
	return txt.value;
};
function getParameterByName(name, url) {
	if (!url) url = window.location.href;
	name = name.replace(/[\[\]]/g, "\\$&");
	var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
	    results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, " "));
}
$(document).ready(function () {

	var mediaTable = getParameterByName('mobilemedia');
	if (mediaTable == "true") {
		$("table").each(function () {
			$(this).attr("style", "display:block");
		});
	}

	// Anti Forgery Token
	var requestVerificationToken = $('.main__wrapper').data('request-verification-token');

	var sortTheTables = new _controllersSortableTableController2['default']();

	window.lightboxController = new _controllersLightboxModalController2['default']();

	/* * *
 	Traverses the DOM and registers event listeners for any pop-out triggers.
 	Bound explicitly to `window` for easier access by Angular.
 * * */
	window.indexPopOuts = function () {

		window.controlPopOuts = new _controllersPopOutController2['default']('.js-pop-out-trigger');

		window.controlPopOuts.customize({
			id: 'header-register',
			tabStyles: {
				deskHeight: 87,
				tabletHeight: 72,
				phoneHeight: '' // Default
			}
		});

		window.controlPopOuts.customize({
			id: 'header-signin',
			tabStyles: {
				deskHeight: 87,
				tabletHeight: 72,
				phoneHeight: '' // Default
			}
		});
	};

	window.indexPopOuts();

	window.bookmark = new _controllersBookmarkController2['default']();

	/* * *
 	Traverses the DOM and registers event listeners for any bookmarkable
 	articles. Bound explicitly to `window` for easier access by Angular.
 * * */
	window.indexBookmarks = function () {
		// Toggle bookmark icon
		$('.js-bookmark-article').on('click', function bookmarkArticle(e) {

			e.preventDefault();
			window.bookmark.toggle(this);
		});
	};

	window.indexBookmarks();

	/* * *
 	If a user tries bookmarking an article while logged out, they'll be
 	prompted to sign in first. This checks for any articles that have been
 	passed along for post-sign-in bookmarking. Bound explicitly to `window`
 	for easier access by Angular.
 * * */
	window.autoBookmark = function () {

		var bookmarkTheArticle = function bookmarkTheArticle(article) {
			$('.js-bookmark-article').each(function (indx, item) {
				if ($(item).data('bookmark-id') === article && !$(item).data('is-bookmarked')) {

					$(item).click();
				} else {
					// already bookmarked or not a match
				}
			});
		};

		var urlVars = window.location.href.split("?");
		var varsToParse = urlVars[1] ? urlVars[1].split("&") : null;
		if (varsToParse) {
			for (var i = 0; i < varsToParse.length; i++) {
				var pair = varsToParse[i].split("=");
				if (pair[0] === 'immb') {
					bookmarkTheArticle(pair[1]);
				}
			}
		}
	};

	window.autoBookmark();

	/* * *
 	Toggle global header search box
 	(toggles at tablet/smartphone sizes, always visible at desktop size)
 * * */
	$('.js-header-search-trigger').on('click', function toggleMenuItems(e) {
		if ($(window).width() <= 800) {
			$('.header-search__wrapper').toggleClass('is-active').focus();
		} else {
			$(e.target).closest('form').submit();
		}
		e.preventDefault();
		return false;
	});

	var newsletterSignup = new _newsletterSignup2['default']();
	newsletterSignup.checkForUserSignedUp();
	newsletterSignup.addControl('.js-newsletter-signup-submit', null, function (triggerElement) {});

	/* * *
 	Handle user sign-in attempts.
 * * */
	var userSignIn = new _controllersFormController2['default']({
		observe: '.js-sign-in-submit',
		successCallback: function successCallback(form, context, event) {

			var loginRegisterMethod = "login_register_component";
			if ($(form).parents('.pop-out__sign-in').length > 0) loginRegisterMethod = "global_login";

			var loginAnalytics = {
				event_name: 'login',
				login_state: 'successful',
				userName: '"' + $(form).find('input[name=username]').val() + '"',
				login_register_method: loginRegisterMethod
			};

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, loginAnalytics));

			/* * *
   	If `pass-article-id` is set, user is probably trying to sign in
   	after attempting to bookmark an article. Add the article ID to
   	the URL so `autoBookmark()` catches it.
   * * */
			var passArticleId = $(form).find('.sign-in__submit').data('pass-article-id');
			if (passArticleId) {
				var sep = window.location.href.indexOf('?') > -1 ? '&' : '?';

				window.location.href = window.location.href + sep + 'immb=' + passArticleId;

				// If Angular, need location.reload to force page refresh
				if (typeof angular !== 'undefined') {
					angular.element($('.search-results')[0]).controller().forceRefresh();
				}
			} else {
				window.location.reload(false);
			}
		},
		failureCallback: function failureCallback(form, context, event) {

			var loginAnalytics = {
				event_name: "Login Failure",
				login_state: "unsuccessful",
				userName: '"' + $(form).find('input[name=username]').val() + '"'
			};

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, loginAnalytics));
		}
	});

	var resetPassword = new _controllersFormController2['default']({
		observe: '.form-reset-password',
		successCallback: function successCallback() {
			$('.form-reset-password').find('.alert-success').show();
			var isPassword = $('.form-reset-password').data("is-password");
			if (isPassword) {
				(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "password reset success" }));
			}
		},
		failureCallback: function failureCallback() {
			var isPassword = $('.form-reset-password').data("is-password");
			if (isPassword) {
				(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "password reset failure" }));
			}
		}

	});

	var newResetPassToken = new _controllersFormController2['default']({
		observe: '.form-new-reset-pass-token',
		successCallback: function successCallback() {
			$('.form-new-reset-pass-token').find('.alert-success').show();
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "password reset success" }));
		},
		failureCallback: function failureCallback() {
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "password reset failure" }));
		}
	});

	$('.js-corporate-master-toggle').on('change', function () {
		if ($(this).prop('checked')) {
			$('.js-registration-corporate-wrapper').show();
		} else {
			$('.js-registration-corporate-wrapper').hide();
		}
	});

	var userRegistrationController = new _controllersFormController2['default']({
		observe: '.form-registration',
		successCallback: function successCallback(form, context, event) {

			// Stash registration type so next page can know it without
			// an additional Salesforce call
			_jscookie2['default'].set('registrationType', context.response.registration_type, {});

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
				registration_type: context.response.registration_type
			}));
		},
		failureCallback: function failureCallback(form, response) {

			var errorMsg = $(".page-registration__error").text();
			if (response.reasons && response.reasons.length > 0) {
				errorMsg = "[";
				for (var reason in response.reasons) {
					errorMsg += response.reasons[reason] + ",";
				}
				errorMsg = errorMsg.substring(0, errorMsg.length - 1);
				errorMsg += "]";
			}
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "registration failure", registraion_errors: errorMsg }));
		}
	});

	var userRegistrationFinalController = new _controllersFormController2['default']({
		observe: '.form-registration-optins',
		successCallback: function successCallback(form, context, event) {

			var registrationType = _jscookie2['default'].get('registrationType');

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
				registration_type: registrationType
			}));

			_jscookie2['default'].remove('registrationType');
		},
		failureCallback: function failureCallback(form, response) {
			var errorMsg = $(".page-registration__error").text();
			if (response.reasons && response.reasons.length > 0) {
				errorMsg = "[";
				for (var reason in response.reasons) {
					errorMsg += response.reasons[reason] + ",";
				}
				errorMsg = errorMsg.substring(0, errorMsg.length - 1);
				errorMsg += "]";
			}
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "registration failure", registraion_errors: errorMsg }));
		}
	});

	var userPreRegistrationController = new _controllersFormController2['default']({
		observe: '.form-pre-registration',
		successCallback: function successCallback(form) {
			var usernameInput = $(form).find('.js-register-username');

			var forwardingURL = $(form).data('forwarding-url');
			var sep = forwardingURL.indexOf('?') < 0 ? '?' : '&';
			var nextStepUrl = $(form).data('forwarding-url') + sep + usernameInput.attr('name') + '=' + encodeURIComponent(usernameInput.val());

			var loginRegisterMethod = "global_registration";
			if ($(form).hasClass("user-calltoaction")) loginRegisterMethod = "login_register_component";

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "registration", login_register_method: loginRegisterMethod }));

			window.location.href = nextStepUrl;
		}
	});

	$('.click-logout').on('click', function (e) {
		(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, { event_name: "logout" }));
	});

	var askTheAnalystController = new _controllersFormController2['default']({
		observe: '.form-ask-the-analyst',
		successCallback: function successCallback(form) {
			$('.js-ask-the-analyst-form-wrapper').hide();
			$('.js-ask-the-analyst-recip-success').html($('.js-ask-the-analyst-recip-addr').val());
			$('.js-ask-the-analyst-success').show();

			// Reset the Ask The Analyst pop-out to its default state when closed
			$('.js-dismiss-ask-the-analyst').one('click', function () {
				$('.js-ask-the-analyst-form-wrapper').show();
				$('.js-ask-the-analyst-success').hide();
			});
		}
	});

	var emailArticleController = new _controllersFormController2['default']({
		observe: '.form-email-article',
		successCallback: function successCallback(form) {
			$('.js-email-article-form-wrapper').hide();
			$('.js-email-article-recip-success').html($('.js-email-article-recip-addr').val());
			$('.js-email-article-success').show();

			// Reset the Email Article pop-out to its default state when closed
			$('.js-dismiss-email-article').one('click', function () {
				$('.js-email-article-form-wrapper').show();
				$('.js-email-article-success').hide();
			});
		}
	});

	var emailAuthorController = new _controllersFormController2['default']({
		observe: '.form-email-author',
		successCallback: function successCallback(form) {
			$('.js-email-author-form-wrapper').hide();
			$('.js-email-author-recip-success').html($('.js-email-author-recip-addr').val());
			$('.js-email-author-success').show();

			// Reset the Email Author pop-out to its default state when closed
			$('.js-dismiss-email-author').one('click', function () {
				$('.js-email-author-form-wrapper').show();
				$('.js-email-author-success').hide();
			});
		}
	});

	var emailCompanyController = new _controllersFormController2['default']({
		observe: '.form-email-company',
		successCallback: function successCallback(form) {
			$('.js-email-company-form-wrapper').hide();
			$('.js-email-company-recip-success').html($('.js-email-company-recip-addr').val());
			$('.js-email-company-success').show();

			// Reset the Email Company pop-out to its default state when closed
			$('.js-dismiss-email-company').one('click', function () {
				$('.js-email-company-form-wrapper').show();
				$('.js-email-company-success').hide();
			});
		}
	});

	var emailDealController = new _controllersFormController2['default']({
		observe: '.form-email-deal',
		successCallback: function successCallback(form) {
			$('.js-email-deal-form-wrapper').hide();
			$('.js-email-deal-recip-success').html($('.js-email-deal-recip-addr').val());
			$('.js-email-deal-success').show();

			// Reset the Email Deal pop-out to its default state when closed
			$('.js-dismiss-email-deal').one('click', function () {
				$('.js-email-deal-form-wrapper').show();
				$('.js-email-deal-success').hide();
			});
		}
	});

	var emailSearchController = new _controllersFormController2['default']({
		observe: '.form-email-search',
		successCallback: function successCallback(form) {

			$('.js-email-search-form-wrapper').hide();
			$('.js-email-search-recip-success').html($('.js-email-search-recip-addr').val());
			$('.js-email-search-success').show();
			$('.js-email-search-form-wrapper input, .js-email-search-form-wrapper textarea').val('');

			// Reset the Email Article pop-out to its default state when closed
			$('.js-dismiss-email-search').one('click', function () {
				$('.js-email-search-form-wrapper').show();
				$('.js-email-search-success').hide();
			});

			var event_data = {
				event_name: "toolbar_use",
				toolbar_tool: "email"
			};

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		},
		beforeRequest: function beforeRequest() {

			var resultIDs = null;

			$('.js-search-results-id').each(function (indx, item) {
				resultIDs = resultIDs ? resultIDs + ',' + $(item).data('bookmark-id') : $(item).data('bookmark-id');
			});

			$('.js-email-search-results-ids').val(resultIDs);
			$('.js-email-search-query').val($('.search-bar__field').val());
			$('.js-email-search-query-url').val(document.location.href);
		}
	});

	var accountEmailPreferencesController = new _controllersFormController2['default']({
		observe: '.form-email-preferences',
		successCallback: function successCallback(form, context, event) {

			var event_data = {};
			var optingIn = null;
			var optingOut = null;

			if ($('#DoNotSendOffersOptIn').prop('checked')) {
				event_data.event_name = 'email_preferences_opt_out';
			} else {

				event_data.event_name = 'email_preferences_update';

				$('.js-account-email-checkbox').each(function (index, item) {
					if (this.checked) {
						optingIn = optingIn ? optingIn + '|' + this.value : this.value;
					} else {
						optingOut = optingOut ? optingOut + '|' + this.value : this.value;
					}
				});

				event_data.email_preferences_optin = optingIn;
				event_data.email_preferences_optout = optingOut;
			}

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		}
	});

	var accountUpdatePassController = new _controllersFormController2['default']({
		observe: '.form-update-account-pass',
		successCallback: function successCallback(form, context, evt) {
			$(form).find('input, select, textarea').each(function () {
				$(this).val('');
			});
		}
	});

	var accountUpdateContactController = new _controllersFormController2['default']({
		observe: '.form-update-account-contact',
		successCallback: function successCallback(form, context, evt) {
			$(window).scrollTop($(evt.target).closest('form').find('.js-form-error-general').offset().top - 32);
		}
	});

	var savedDocumentsController = new _controllersFormController2['default']({
		observe: '.form-remove-saved-document',
		successCallback: function successCallback(form, context, evt) {
			$(evt.target).closest('tr').remove();
			if ($('.js-sortable-table tbody')[0].rows.length === 0) {
				$('.js-sortable-table').remove();
				$('.js-no-articles').show();
			}

			var event_data = {
				event_name: 'bookmark_removal',
				bookmark_title: $(form).data('analytics-title'),
				bookmark_publication: $(form).data('analytics-publication')
			};

			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
		}
	});

	(0, _svg4everybody2['default'])();

	/* * *
 	MAIN SITE MENU
 * * */
	(function MenuController() {

		var getHeaderEdge = function getHeaderEdge() {
			return $('.header__wrapper').offset().top + $('.header__wrapper').height();
		};

		var showMenu = function showMenu() {
			$('.main-menu').addClass('is-active');
			$('.menu-toggler').addClass('is-active');
			$('.header__wrapper .menu-toggler').addClass('is-sticky');
			$('body').addClass('is-frozen');
		};

		var hideMenu = function hideMenu() {
			$('.main-menu').removeClass('is-active');
			$('.menu-toggler').removeClass('is-active');
			$('body').removeClass('is-frozen');
			if ($(window).scrollTop() <= getHeaderEdge()) {
				$('.header__wrapper .menu-toggler').removeClass('is-sticky');
			}
		};

		/* Toggle menu visibility */
		$('.js-menu-toggle-button').on('click', function toggleMenu(e) {
			$('.main-menu').hasClass('is-active') ? hideMenu() : showMenu();
			e.preventDefault();
			e.stopPropagation();
		});

		/*  If the menu is closed, let any clicks on the menu element open
  	the menu. This includes the border—visible when the menu is closed—
  	so it's easier to open. */
		$('.js-full-menu-toggle').on('click', function toggleMenu() {
			$('.main-menu').hasClass('is-active') ? null : showMenu();
		});

		/* Attach / detach sticky menu */
		$(window).on('scroll', function windowScrolled() {
			// Only stick if the header (including toggler) isn't visible
			if ($(window).scrollTop() > getHeaderEdge() || $('.main-menu').hasClass('is-active')) {
				$('.header__wrapper .menu-toggler').addClass('is-sticky');
			} else {
				$('.header__wrapper .menu-toggler').removeClass('is-sticky');
			}
		});

		/* Toggle menu categories */
		$('.js-toggle-menu-section').on('click', function toggleMenuItems(e) {
			e.target !== this ? this.click() : $(e.target).toggleClass('is-active');
		});
	})();

	/* * *
 	When a banner is dismissed, the banner ID is stored in the
 	`dismissedBanners` cookie as a JSON object. Banners are invisible by default,
 	so on page load, this checks if a banner on the page is dismissed or not,
 	then makes the banner visible if not dismissed.
 * * */
	var dismissedBanners = _jscookie2['default'].getJSON('dismissedBanners') || {};
	$('.banner').each(function () {
		if ($(this).data('banner-id') in dismissedBanners === false) {
			$(this).addClass('is-visible');
		}
	});

	/* * *
 	Generic banner dismiss
 * * */
	$('.js-dismiss-banner').on('click', function dismissBanner(e) {
		var thisBanner = $(e.target).parents('.banner');
		thisBanner.removeClass('is-visible');

		var dismissedBanners = _jscookie2['default'].getJSON('dismissedBanners') || {};
		dismissedBanners[thisBanner.data('banner-id')] = true;

		// if banner has a 'dismiss-all-subdomains' attribute = true, set the domain of the cookie
		// to the top-level domain.
		var domain = document.location.hostname;
		if (thisBanner.data('dismiss-all-subdomains')) {
			var parts = domain.split('.');
			parts.shift();
			domain = parts.join('.');
		}
		_jscookie2['default'].set('dismissedBanners', dismissedBanners, { expires: 3650, domain: domain });
	});

	// For each article table, clone and append "view full table" markup
	$('.article-body-content table').not('.article-table--mobile-link').forEach(function (e) {
		var mediaId = $(e).data("mediaid");
		var tableLink = $('.js-mobile-table-template .article-table').clone();

		var url = window.location.href;
		url.replace("#", "");
		if (url.indexOf("?") < 0) url += "?";else url += "&";

		url += "mobilemedia=true&selectedid=" + mediaId;

		// $(tableLink).find('a').attr("href", url);
		$(tableLink).find('a').data("table-url", url).attr('href', null);
		$(e).after(tableLink);
	});

	// Find duplicate embeds on article page
	// IITS2-312
	$('[class^=ewf-desktop-iframe] ~ [class^=ewf-mobile-iframe]').each(function (index, item) {
		$(item).remove();
	});

	// When DOM loads, render the appropriate iFrame components
	// Also add a listener for winder resize, render appropriate containers
	renderIframeComponents();
	$(window).on('resize', function (event) {
		renderIframeComponents();
	});

	// Topic links
	var topicAnchors = $('.js-topic-anchor');

	$('.sub-topic-links').forEach(function (e) {
		var linkList = $(e).find('.bar-separated-link-list');

		topicAnchors.forEach(function (tc) {
			var id = tc.id;
			var text = $(tc).data('topic-link-text');
			var utagInfo = '{"event_name"="topic-jump-to-link-click","topic-name"="' + text + '"}';
			linkList.append('<a href="#' + id + '" class="click-utag" data-info=' + text + '>' + text + '</a>');
		});
	});

	// Display the Forgot Password block when "forgot your password" is clicked
	$('.js-show-forgot-password').on('click', function toggleForgotPass() {
		$('.js-reset-password-container').toggleClass('is-active');
		if ($(".informa-ribbon").hasClass("show")) {
			$("body").scrollTop($(".pop-out__forgot-password").position().top + 570);
		} else {
			$("body").scrollTop($(".pop-out__forgot-password").position().top);
		}
	});

	// Global dismiss button for pop-outs
	$('.dismiss-button').on('click', function (e) {
		if (e.target !== this) {
			this.click();
			return;
		}

		$($(e.target).data('target-element')).removeClass('is-active');
		window.controlPopOuts.closePopOut(e.target);
	});

	// Make sure all external links open in a new window/tab
	$("a[href^=http]").each(function () {
		if (this.href.indexOf(location.hostname) == -1) {
			$(this).attr({
				target: "_blank"
			});
		}
	});

	// Adds analytics for article page clicks
	$('.root').find('a').each(function (index, item) {

		$(this).addClass('click-utag');

		var linkString;

		if (this.href.indexOf(location.hostname) == -1) {
			linkString = 'External:' + this.href;
		} else {
			linkString = this.href;
		}

		if ($(this).data('info') == undefined) {
			$(this).data('info', '{ "event_name": "embeded_link_click_through", "click_through_source": "' + $('h1').text + '", "click_through_destination": "' + linkString + '"}');
		}
	});

	$('.general-header__navigation').each(function () {

		$(this).on('scroll', function () {
			var scrollLeft = $(this).scrollLeft();
			var scrollWidth = $(this)[0].scrollWidth;
			var winWidth = $(window).width();

			if (scrollLeft > 32) {
				$('.general-header__navigation-scroller--left').addClass('is-visible');
			} else {
				$('.general-header__navigation-scroller--left').removeClass('is-visible');
			}

			if (scrollLeft + winWidth < scrollWidth - 32) {
				$('.general-header__navigation-scroller--right').addClass('is-visible');
			} else {
				$('.general-header__navigation-scroller--right').removeClass('is-visible');
			}
		});

		var scrollLeft = $(this).scrollLeft();
		var scrollWidth = $(this)[0].scrollWidth;
		var winWidth = $(window).width();

		if (scrollLeft + winWidth < scrollWidth - 32) {
			$('.general-header__navigation-scroller--right').addClass('is-visible');
		} else {
			$('.general-header__navigation-scroller--right').removeClass('is-visible');
		}
	});

	// Smooth, clickable scrolling for General page headers
	var smoothScrollingNav = function smoothScrollingNav() {

		// Cache for less DOM checking
		var Scrollable = $('.general-header__navigation');
		var Container = $('.general-header');

		// Find current scroll distance is from left and right edges
		var scrollDistance = function scrollDistance() {
			return {
				left: Scrollable.scrollLeft(),
				right: Scrollable[0].scrollWidth - (Container.width() + Scrollable.scrollLeft())
			};
		};

		var init = function init() {

			$('.general-header__navigation-scroller--right').on('click', function () {
				if (scrollDistance().right > 0) {
					// Not on right edge
					smoothScroll(200, 'right');
				}
			});

			$('.general-header__navigation-scroller--left').on('click', function () {
				if (scrollDistance().left > 0) {
					smoothScroll(200, 'left');
				}
			});
		};

		var scrollToTimerCache;
		var totalTravel = null;
		var durationStart = null;

		// Quadratic ease-out algorithm
		var easing = function easing(time, distance) {
			return distance * (time * (2 - time));
		};

		var smoothScroll = function smoothScroll(duration, direction) {
			if (duration <= 0) {
				// Reset everything when duration time finishes
				totalTravel = null;
				durationStart = null;
				return;
			}

			// Store duration as durationStart on first loop
			durationStart = !durationStart ? duration : durationStart;

			// Store travel distance (container width) as totalTravel on first loop
			totalTravel = !totalTravel ? Container.width() : totalTravel;

			// Finds percentage of elapsed time since start
			var travelPcent = 1 - duration / durationStart;

			// Finds travel change on this loop, adjusted for ease-out
			var travel = easing(travelPcent, totalTravel / durationStart * 10);

			scrollToTimerCache = setTimeout((function () {
				if (!isNaN(parseInt(travel, 10))) {
					if (direction === 'right') {
						Scrollable.scrollLeft(Scrollable.scrollLeft() + travel);
						smoothScroll(duration - 10, direction);
					} else if (direction === 'left') {
						Scrollable.scrollLeft(Scrollable.scrollLeft() - travel);
						smoothScroll(duration - 10, direction);
					}
				}
			}).bind(this), 10);
		};

		// Bind event listeners
		init();
	};

	$('.js-register-final').on('click', function (e) {

		var eventDetails = {
			// event_name: "newsletter optins"
		};
		var chkDetails = {};
		if ($('#newsletters').is(':checked')) {
			chkDetails.newsletter_optin = "true";
			$.extend(eventDetails, chkDetails);
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, eventDetails));
		} else {
			chkDetails.newsletter_optin = "false";
			$.extend(eventDetails, chkDetails);
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, eventDetails));
		}
	});

	// TODO - Refactor this code, update class name to a `js-` name
	$('.manage-preferences').click(function (e) {
		var preferencesData = {
			event_name: "manage-preferences"
		};
		if ($("#NewsletterOptIn").is(':checked') && $("#DoNotSendOffersOptIn").is(':checked')) {
			preferencesData = {
				newsletter_optin: "true",
				donot_send_offers_optin: "true"
			};
		}
		if (!$("#NewsletterOptIn").is(':checked') && $("#DoNotSendOffersOptIn").is(':checked')) {
			preferencesData = {
				newsletter_optin: "false",
				donot_send_offers_optin: "true"
			};
		}
		if ($("#NewsletterOptIn").is(':checked') && !$("#DoNotSendOffersOptIn").is(':checked')) {
			preferencesData = {
				newsletter_optin: "true",
				donot_send_offers_optin: "false"
			};
		}
		if (!$("#NewsletterOptIn").is(':checked') && !$("#DoNotSendOffersOptIn").is(':checked')) {
			preferencesData = {
				newsletter_optin: "false",
				donot_send_offers_optin: "false"
			};
		}

		(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, preferencesData));
	});

	// Execute!
	smoothScrollingNav();

	// Toggle global Informa bar
	$('.informa-ribbon__title').on('click', function (e) {
		$('.informa-ribbon').toggleClass('show');
	});

	$('.js-toggle-list').on('click', function (e) {
		$(e.target).closest('.js-togglable-list-wrapper').toggleClass('is-active');
	});

	$('.click-utag').click(function (e) {
		(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, $(this).data('info')));
	});

	$('.search-results').on('click', '.click-utag', function (e) {
		(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, $(this).data('info')));
	});

	$('#chkASBilling').click(function (e) {
		if ($(this).is(':checked')) {
			$('#ddlShippingCountry').val($('#ddlBillingCountry').val());
			$('#txtShippingAddress1').val($('#txtBillingAddress1').val());
			$('#txtShippingAddress2').val($('#txtBillingAddress2').val());
			$('#txtShippingCity').val($('#txtBillingCity').val());
			$('#txtShippingState').val($('#txtBillingState').val());
			$('#txtShippingPostalCode').val($('#txtBillingPostalCode').val());
		}
	});

	// Account - Email Preferences toggler
	$('.js-account-email-toggle-all').on('click', function (e) {
		$('.js-update-email-prefs').attr('disabled', null);
	});

	$('.js-account-email-checkbox').on('click', function (e) {
		$('.js-update-email-prefs').attr('disabled', null);
	});

	window.findTooltips = function () {
		$('.js-toggle-tooltip').each(function (index, item) {
			var tooltip;
			$(item).data("ttVisible", false);
			$(item).data("ttTouchTriggered", false);

			$(item).on('mouseenter touchstart', function (e) {
				e.preventDefault();

				if (e.type === "touchstart") {
					$(item).data("ttTouchTriggered", true);
				}

				// Actual mouse events thrown can be any number of things...
				if ((e.type === "mouseover" || e.type === "mouseenter") && $(item).data("ttTouchTriggered")) {
					// Do nothing
				} else if ($(item).data("ttVisible") && e.type === "touchstart") {
						$(item).data("ttVisible", false);
						$(item).data("ttTouchTriggered", false);
						tooltip.hidePopup();
					} else {
						$(item).data("ttVisible", true);
						var offsets = $(item).offset();
						tooltip = (0, _controllersTooltipController2['default'])({
							isHidden: false,
							html: $(item).data('tooltip-text'),
							top: offsets.top,
							left: offsets.left + $(this).width() / 2,
							triangle: 'bottom'
						});
					}
			});

			$(item).on('mouseleave', function () {
				$(item).data("ttVisible", false);
				tooltip.hidePopup();
			});
		});
	};

	//////The following style updates are to fix quick-box and quote styling for migrated issues (IIS-366)
	$('.sidebox .quickfactstitle').parent().removeClass('sidebox').addClass('quick-facts');
	$('.quick-facts .quickfactstitle').removeClass('quickfactstitle').addClass('quick-facts__header');
	$('.quick-facts .quickfactstext').removeClass('quickfactstext').addClass('quick-facts__text');
	$('.quick-facts .quickfactsbulleted').removeClass('quickfactsbulleted').addClass('quick-facts__list--ul');
	$('.sidebox blockquote').parent().removeClass('sidebox').addClass('quote');

	$('.quote blockquote').each(function () {
		$(this).replaceWith("<span>" + $(this).html() + "</span>");
		// this function is executed for all 'code' elements, and
		// 'this' refers to one element from the set of all 'code'
		// elements each time it is called.
	});
	//////////////////////////////////////////////////////

	window.findTooltips();

	// Twitter sharing JS
	window.twttr = (function (t, e, r) {
		var n,
		    i = t.getElementsByTagName(e)[0],
		    w = window.twttr || {};
		return t.getElementById(r) ? w : (n = t.createElement(e), n.id = r, n.src = "https://platform.twitter.com/widgets.js", i.parentNode.insertBefore(n, i), w._e = [], w.ready = function (t) {
			w._e.push(t);
		}, w);
	})(document, "script", "twitter-wjs");

	$('.contactInfoNumericField').on('keypress', function (e) {
		e = e ? e : window.event;
		var charCode = e.which ? e.which : e.keyCode;
		if (charCode > 31 && (charCode < 48 || charCode > 57)) {
			return false;
		}
		return true;
	});

	// Pretty select boxes
	$('select:not(.ng-scope)').selectivity({
		showSearchInputInDropdown: false,
		positionDropdown: function positionDropdown($dropdownEl, $selectEl) {
			$dropdownEl.css("width", $selectEl.width() + "px");
		}
	});

	$(".selectivity-input .selectivity-single-select").each(function () {
		$(this).append('<span class="selectivity-arrow"><svg class="alert__icon"><use xlink:href="/dist/img/svg-sprite.svg#sort-down-arrow"></use></svg></span>');
	});
});

},{"./components/article-sidebar-component":2,"./components/save-search-component":3,"./controllers/analytics-controller":4,"./controllers/bookmark-controller":5,"./controllers/form-controller":6,"./controllers/lightbox-modal-controller":7,"./controllers/pop-out-controller":8,"./controllers/register-controller":9,"./controllers/reset-password-controller":10,"./controllers/sortable-table-controller":11,"./controllers/tooltip-controller":12,"./jscookie":14,"./newsletter-signup":15,"./search-page.js":16,"./selectivity-full":17,"./svg4everybody":18,"./toggle-icons":19,"./zepto.min":20}],14:[function(require,module,exports){
/*!
 * JavaScript Cookie v2.1.0
 * https://github.com/js-cookie/js-cookie
 *
 * Copyright 2006, 2015 Klaus Hartl & Fagner Brack
 * Released under the MIT license
 */
'use strict';

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
})(function () {
	function extend() {
		var i = 0;
		var result = {};
		for (; i < arguments.length; i++) {
			var attributes = arguments[i];
			for (var key in attributes) {
				result[key] = attributes[key];
			}
		}
		return result;
	}

	function init(converter) {
		function api(key, value, attributes) {
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
					value = encodeURIComponent(String(value)).replace(/%(23|24|26|2B|3A|3C|3E|3D|2F|3F|40|5B|5D|5E|60|7B|7D|7C)/g, decodeURIComponent);
				} else {
					value = converter.write(value, key);
				}

				key = encodeURIComponent(String(key));
				key = key.replace(/%(23|24|26|2B|5E|60|7C)/g, decodeURIComponent);
				key = key.replace(/[\(\)]/g, escape);

				return document.cookie = [key, '=', value, attributes.expires && '; expires=' + attributes.expires.toUTCString(), // use expires attribute, max-age is not supported by IE
				attributes.path && '; path=' + attributes.path, attributes.domain && '; domain=' + attributes.domain, attributes.secure ? '; secure' : ''].join('');
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
					cookie = converter.read ? converter.read(cookie, name) : converter(cookie, name) || cookie.replace(rdecode, decodeURIComponent);

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
});
$(document).ready(function () {
	var cookieName = 'menunavigationcookie',
	    // Name of our cookie
	cookieValue = 'yes'; // Value of cookie

	function CloseNavigationMenu() {
		$('.main-menu').removeClass('is-active');
		$('.menu-toggler').removeClass('is-active');
		$('.header__wrapper .menu-toggler').removeClass('is-sticky');
		$('body').removeClass('is-frozen');
	}

	function OpenNavigationMenu() {
		$('.main-menu').addClass('is-active');
		$('.menu-toggler').addClass('is-active');
		$('.header__wrapper .menu-toggler').addClass('is-sticky');
		$('body').addClass('is-frozen');
	}

	// Create cookie
	function createCookie(name, value, days) {
		var expires;
		if (days) {
			var date = new Date();
			date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
			expires = "; expires=" + date.toGMTString();
		} else {
			expires = "";
		}
		document.cookie = name + "=" + value + expires + "; path=/";
	}

	// Read cookie
	function readCookie(name) {
		var nameEQ = name + "=";
		var ca = document.cookie.split(';');
		for (var i = 0; i < ca.length; i++) {
			var c = ca[i];
			while (c.charAt(0) === ' ') {
				c = c.substring(1, c.length);
			}
			if (c.indexOf(nameEQ) === 0) {
				return c.substring(nameEQ.length, c.length);
			}
		}
		return null;
	}

	// Erase cookie
	function eraseCookie(name) {
		createCookie(name, "", -1);
	}

	if ($(window).width() >= 1024) {
		if ($('input.menu-open-first-time-checked').val() == 'True') {
			if (readCookie("menunavigationcookie") !== cookieValue && window.location.pathname === '/') {
				OpenNavigationMenu();
			} else {
				CloseNavigationMenu();
			}
			createCookie("menunavigationcookie", "yes", 365);
		} else {
			eraseCookie("menunavigationcookie");
		}
	}
});

},{}],15:[function(require,module,exports){
/* global analytics_data */

'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});

var _controllersAnalyticsController = require('./controllers/analytics-controller');

function newsletterSignupController() {

    this.checkForUserSignedUp = function () {
        $.get('/Account/api/PreferencesApi/IsUserSignedUp', function (response) {
            var res = response;
            if (response) {
                $(".newsletter-signup").hide();
            }
        });
    };

    this.IsValidEmail = function (email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };
    this.addControl = function (triggerElement, successCallback, failureCallback) {
        var _this = this;

        if (triggerElement) {
            $(triggerElement).on('click', function (event) {

                // Prevent form submit
                event.preventDefault();

                // Hide any errors
                $('.js-newsletter-signup-error').hide();

                var inputData = $("#newsletterUserName").val();
                var url = $(triggerElement).data('signup-url');

                //$(triggerElement).parents('.newsletter-signup').find('input').each(function() {
                //    inputData = $(this).val();
                //});

                if (inputData !== '' && _this.IsValidEmail(inputData)) {
                    $('.js-newsletter-signup--error-invalidemailformat').hide();
                    url = url + '?userName=' + inputData;

                    $.get(url, function (response) {
                        var newsletterAnalytics;

                        if (response == 'true') {

                            newsletterAnalytics = {
                                event_name: 'newsletter-signup',
                                newsletter_signup_state: 'successful',
                                userName: '"' + inputData + '"'
                            };

                            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, newsletterAnalytics));

                            $(".newsletter-signup-before-submit").hide();
                            $(".newsletter-signup-after-submit").show();
                        } else if (response == 'mustregister') {

                            newsletterAnalytics = {
                                event_name: 'newsletter-signup',
                                newsletter_signup_state: 'unsuccessful',
                                userName: '"' + inputData + '"'
                            };

                            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, newsletterAnalytics));

                            $('.newsletter-signup-needs-registration a').attr('href', $('.newsletter-signup-needs-registration a').attr('href') + $('.newsletter-signup-before-submit input').val());
                            $('.newsletter-signup-before-submit').hide();
                            $('.newsletter-signup-needs-registration').show();
                        } else {
                            newsletterAnalytics = {
                                event_name: 'newsletter-signup',
                                newsletter_signup_state: 'unsuccessful',
                                userName: '"' + inputData + '"'
                            };

                            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, newsletterAnalytics));

                            $('.js-newsletter-signup-error').show();
                        }
                    });
                } else {
                    $('.js-newsletter-signup--error-invalidemailformat').show();
                }
            });
        }
    };
}

exports['default'] = newsletterSignupController;
module.exports = exports['default'];

},{"./controllers/analytics-controller":4}],16:[function(require,module,exports){
'use strict';

var SearchScript = (function () {

	/* Toggle search tips visibility */
	$('.js-toggle-search-tips').on('click', function toggleTips() {
		$('.search-bar__tips').toggleClass('open');
		$('.search-bar').toggleClass('tips-open');
	});
})();

},{}],17:[function(require,module,exports){
(function (global){
/**
 * @license
 * Selectivity.js 2.1.0 <https://arendjr.github.io/selectivity/>
 * Copyright (c) 2014-2016 Arend van Beelen jr.
 *           (c) 2016 Speakap BV
 * Available under MIT license <https://github.com/arendjr/selectivity/blob/master/LICENSE>
 */"use strict";!(function(e){if("object" == typeof exports && "undefined" != typeof module)module.exports = e();else if("function" == typeof define && define.amd)define([],e);else {var f;"undefined" != typeof window?f = window:"undefined" != typeof global?f = global:"undefined" != typeof self && (f = self),f.selectivity = e();}})(function(){var define,module,exports;return (function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require == "function" && require;if(!u && a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '" + o + "'");throw (f.code = "MODULE_NOT_FOUND",f);}var l=n[o] = {exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e);},l,l.exports,e,t,n,r);}return n[o].exports;}var i=typeof require == "function" && require;for(var o=0;o < r.length;o++) s(r[o]);return s;})({1:[function(_dereq_,module,exports){_dereq_(5);_dereq_(6);_dereq_(7);_dereq_(9);_dereq_(10);_dereq_(11);_dereq_(12);_dereq_(13);_dereq_(14);_dereq_(15);_dereq_(16);_dereq_(17);_dereq_(18);_dereq_(19);module.exports = _dereq_(8);},{"10":10,"11":11,"12":12,"13":13,"14":14,"15":15,"16":16,"17":17,"18":18,"19":19,"5":5,"6":6,"7":7,"8":8,"9":9}],2:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto; /**
 * Event Delegator Constructor.
 */function EventDelegator(){this._events = [];this.delegateEvents();} /**
 * Methods.
 */$.extend(EventDelegator.prototype,{ /**
     * Attaches all listeners from the events map to the instance's element.
     *
     * Normally, you should not have to call this method yourself as it's called automatically in
     * the constructor.
     */delegateEvents:function delegateEvents(){this.undelegateEvents();$.each(this.events,(function(event,listener){var selector,index=event.indexOf(' ');if(index > -1){selector = event.slice(index + 1);event = event.slice(0,index);}if($.type(listener) === 'string'){listener = this[listener];}listener = listener.bind(this);if(selector){this.$el.on(event,selector,listener);}else {this.$el.on(event,listener);}this._events.push({event:event,selector:selector,listener:listener});}).bind(this));}, /**
     * Detaches all listeners from the events map from the instance's element.
     */undelegateEvents:function undelegateEvents(){this._events.forEach(function(event){if(event.selector){this.$el.off(event.event,event.selector,event.listener);}else {this.$el.off(event.event,event.listener);}},this);this._events = [];}});module.exports = EventDelegator;},{"jquery":"jquery"}],3:[function(_dereq_,module,exports){'use strict'; /**
 * @license
 * lodash 3.3.1 (Custom Build) <https://lodash.com/>
 * Copyright 2012-2015 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.8.2 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2015 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <https://lodash.com/license>
 */ /**
 * Gets the number of milliseconds that have elapsed since the Unix epoch
 *  (1 January 1970 00:00:00 UTC).
 *
 * @static
 * @memberOf _
 * @category Date
 * @example
 *
 * _.defer(function(stamp) {
 *   console.log(_.now() - stamp);
 * }, _.now());
 * // => logs the number of milliseconds it took for the deferred function to be invoked
 */var now=Date.now; /**
 * Creates a function that delays invoking `func` until after `wait` milliseconds
 * have elapsed since the last time it was invoked.
 *
 * See [David Corbacho's article]
 *                        (http://drupalmotion.com/article/debounce-and-throttle-visual-explanation)
 * for details over the differences between `_.debounce` and `_.throttle`.
 *
 * @static
 * @memberOf _
 * @category Function
 * @param {Function} func The function to debounce.
 * @param {number} [wait=0] The number of milliseconds to delay.
 * @returns {Function} Returns the new debounced function.
 * @example
 *
 * // avoid costly calculations while the window size is in flux
 * jQuery(window).on('resize', _.debounce(calculateLayout, 150));
 */function debounce(func,wait){var args,result,stamp,timeoutId,trailingCall,lastCalled=0;wait = wait < 0?0:+wait || 0;function delayed(){var remaining=wait - (now() - stamp);if(remaining <= 0 || remaining > wait){var isCalled=trailingCall;timeoutId = trailingCall = undefined;if(isCalled){lastCalled = now();result = func.apply(null,args);if(!timeoutId){args = null;}}}else {timeoutId = setTimeout(delayed,remaining);}}function debounced(){args = arguments;stamp = now();trailingCall = true;if(!timeoutId){timeoutId = setTimeout(delayed,wait);}return result;}return debounced;}module.exports = debounce;},{}],4:[function(_dereq_,module,exports){'use strict'; /**
 * @license
 * Lo-Dash 2.4.1 (Custom Build) <http://lodash.com/>
 * Copyright 2012-2013 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.5.2 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2013 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <http://lodash.com/license>
 */var htmlEscapes={'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}; /**
 * Used by `escape` to convert characters to HTML entities.
 *
 * @private
 * @param {string} match The matched character to escape.
 * @returns {string} Returns the escaped character.
 */function escapeHtmlChar(match){return htmlEscapes[match];}var reUnescapedHtml=new RegExp('[' + Object.keys(htmlEscapes).join('') + ']','g'); /**
 * Converts the characters `&`, `<`, `>`, `"`, and `'` in `string` to their
 * corresponding HTML entities.
 *
 * @static
 * @memberOf _
 * @category Utilities
 * @param {string} string The string to escape.
 * @returns {string} Returns the escaped string.
 * @example
 *
 * _.escape('Fred, Wilma, & Pebbles');
 * // => 'Fred, Wilma, &amp; Pebbles'
 */function escape(string){return string?String(string).replace(reUnescapedHtml,escapeHtmlChar):'';}module.exports = escape;},{}],5:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var debounce=_dereq_(3);var Selectivity=_dereq_(8);_dereq_(13); /**
 * Option listener that implements a convenience query function for performing AJAX requests.
 */Selectivity.OptionListeners.unshift(function(selectivity,options){var ajax=options.ajax;if(ajax && ajax.url){var formatError=ajax.formatError || Selectivity.Locale.ajaxError;var minimumInputLength=ajax.minimumInputLength || 0;var params=ajax.params;var processItem=ajax.processItem || function(item){return item;};var quietMillis=ajax.quietMillis || 0;var resultsCb=ajax.results || function(data){return {results:data,more:false};};var transport=ajax.transport || $.ajax;if(quietMillis){transport = debounce(transport,quietMillis);}options.query = function(queryOptions){var offset=queryOptions.offset;var term=queryOptions.term;if(term.length < minimumInputLength){queryOptions.error(Selectivity.Locale.needMoreCharacters(minimumInputLength - term.length));}else {var url=ajax.url instanceof Function?ajax.url(queryOptions):ajax.url;if(params){url += (url.indexOf('?') > -1?'&':'?') + $.param(params(term,offset));}var _success=ajax.success;var _error=ajax.error;transport($.extend({},ajax,{url:url,success:function success(data,textStatus,jqXHR){if(_success){_success(data,textStatus,jqXHR);}var results=resultsCb(data,offset);results.results = results.results.map(processItem);queryOptions.callback(results);},error:function error(jqXHR,textStatus,errorThrown){if(_error){_error(jqXHR,textStatus,errorThrown);}queryOptions.error(formatError(term,jqXHR,textStatus,errorThrown),{escape:false});}}));}};}});},{"13":13,"3":3,"8":8,"jquery":"jquery"}],6:[function(_dereq_,module,exports){'use strict';var Selectivity=_dereq_(8);var latestQueryNum=0; /**
 * Option listener that will discard any callbacks from the query function if another query has
 * been called afterwards. This prevents responses from remote sources arriving out-of-order.
 */Selectivity.OptionListeners.push(function(selectivity,options){var query=options.query;if(query && !query._async){options.query = function(queryOptions){latestQueryNum++;var queryNum=latestQueryNum;var callback=queryOptions.callback;var error=queryOptions.error;queryOptions.callback = function(){if(queryNum === latestQueryNum){callback.apply(null,arguments);}};queryOptions.error = function(){if(queryNum === latestQueryNum){error.apply(null,arguments);}};query(queryOptions);};options.query._async = true;}});},{"8":8}],7:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var SelectivityDropdown=_dereq_(10); /**
 * Methods.
 */$.extend(SelectivityDropdown.prototype,{ /**
     * @inherit
     */removeCloseHandler:function removeCloseHandler(){if(this._$backdrop && !this.parentMenu){this._$backdrop.remove();this._$backdrop = null;}}, /**
     * @inherit
     */setupCloseHandler:function setupCloseHandler(){var $backdrop;if(this.parentMenu){$backdrop = this.parentMenu._$backdrop;}else {$backdrop = $('<div>').addClass('selectivity-backdrop');$('body').append($backdrop);}$backdrop.on('click',this.close.bind(this));this._$backdrop = $backdrop;}});},{"10":10,"jquery":"jquery"}],8:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var EventDelegator=_dereq_(2); /**
 * Create a new Selectivity instance or invoke a method on an instance.
 *
 * @param methodName Optional name of a method to call. If omitted, a Selectivity instance is
 *                   created for each element in the set of matched elements. If an element in the
 *                   set already has a Selectivity instance, the result is the same as if the
 *                   setOptions() method is called. If a method name is given, the options
 *                   parameter is ignored and any additional parameters are passed to the given
 *                   method.
 * @param options Options object to pass to the constructor or the setOptions() method. In case
 *                a new instance is being created, the following properties are used:
 *                inputType - The input type to use. Default input types include 'Multiple' and
 *                            'Single', but you can add custom input types to the InputTypes map or
 *                            just specify one here as a function. The default value is 'Single',
 *                            unless multiple is true in which case it is 'Multiple'.
 *                multiple - Boolean determining whether multiple items may be selected
 *                           (default: false). If true, a MultipleSelectivity instance is created,
 *                           otherwise a SingleSelectivity instance is created.
 *
 * @return If the given method returns a value, this method returns the value of that method
 *         executed on the first element in the set of matched elements.
 */function selectivity(methodName,options){ /* jshint validthis: true */var methodArgs=Array.prototype.slice.call(arguments,1);var result;this.each(function(){var instance=this.selectivity;if(instance){if($.type(methodName) !== 'string'){methodArgs = [methodName];methodName = 'setOptions';}if($.type(instance[methodName]) === 'function'){if(result === undefined){result = instance[methodName].apply(instance,methodArgs);}}else {throw new Error('Unknown method: ' + methodName);}}else {if($.type(methodName) === 'string'){if(methodName !== 'destroy'){throw new Error('Cannot call method on element without Selectivity instance');}}else {options = $.extend({},methodName,{element:this}); // this is a one-time hack to facilitate the selectivity-traditional module, because
// the module is not able to hook this early into creation of the instance
var $this=$(this);if($this.is('select') && $this.prop('multiple')){options.multiple = true;}var InputTypes=Selectivity.InputTypes;var InputType=options.inputType || (options.multiple?'Multiple':'Single');if($.type(InputType) !== 'function'){if(InputTypes[InputType]){InputType = InputTypes[InputType];}else {throw new Error('Unknown Selectivity input type: ' + InputType);}}this.selectivity = new InputType(options);}}});return result === undefined?this:result;} /**
 * Selectivity Base Constructor.
 *
 * You will never use this constructor directly. Instead, you use $(selector).selectivity(options)
 * to create an instance of either MultipleSelectivity or SingleSelectivity. This class defines all
 * functionality that is common between both.
 *
 * @param options Options object. Accepts the same options as the setOptions method(), in addition
 *                to the following ones:
 *                data - Initial selection data to set. This should be an array of objects with 'id'
 *                       and 'text' properties. This option is mutually exclusive with 'value'.
 *                element - The DOM element to which to attach the Selectivity instance. This
 *                          property is set automatically by the $.fn.selectivity() function.
 *                value - Initial value to set. This should be an array of IDs. This property is
 *                        mutually exclusive with 'data'.
 */function Selectivity(options){if(!(this instanceof Selectivity)){return selectivity.apply(this,arguments);} /**
     * jQuery container for the element to which this instance is attached.
     */this.$el = $(options.element); /**
     * jQuery container for the search input.
     *
     * May be null as long as there is no visible search input. It is set by initSearchInput().
     */this.$searchInput = null; /**
     * Reference to the currently open dropdown.
     */this.dropdown = null; /**
     * Whether the input is enabled.
     *
     * This is false when the option readOnly is false or the option removeOnly is false.
     */this.enabled = true; /**
     * Boolean whether the browser has touch input.
     */this.hasTouch = typeof window !== 'undefined' && 'ontouchstart' in window; /**
     * Boolean whether the browser has a physical keyboard attached to it.
     *
     * Given that there is no way for JavaScript to reliably detect this yet, we just assume it's
     * the opposite of hasTouch for now.
     */this.hasKeyboard = !this.hasTouch; /**
     * Array of items from which to select. If set, this will be an array of objects with 'id' and
     * 'text' properties.
     *
     * If given, all items are expected to be available locally and all selection operations operate
     * on this local array only. If null, items are not available locally, and a query function
     * should be provided to fetch remote data.
     */this.items = null; /**
     * The function to be used for matching search results.
     */this.matcher = Selectivity.matcher; /**
     * Options passed to the Selectivity instance or set through setOptions().
     */this.options = {}; /**
     * Array of search input listeners.
     *
     * Custom listeners can be specified in the options object.
     */this.searchInputListeners = Selectivity.SearchInputListeners; /**
     * Mapping of templates.
     *
     * Custom templates can be specified in the options object.
     */this.templates = $.extend({},Selectivity.Templates); /**
     * The last used search term.
     */this.term = '';this.setOptions(options);if(options.value){this.value(options.value,{triggerChange:false});}else {this.data(options.data || null,{triggerChange:false});}this.$el.on('mouseover',this._mouseover.bind(this));this.$el.on('mouseleave',this._mouseout.bind(this));this.$el.on('selectivity-close',this._closed.bind(this));EventDelegator.call(this);} /**
 * Methods.
 */$.extend(Selectivity.prototype,EventDelegator.prototype,{ /**
     * Convenience shortcut for this.$el.find(selector).
     */$:function $(selector){return this.$el.find(selector);}, /**
     * Closes the dropdown.
     */close:function close(){if(this.dropdown){this.dropdown.close();}}, /**
     * Sets or gets the selection data.
     *
     * The selection data contains both IDs and text labels. If you only want to set or get the IDs,
     * you should use the value() method.
     *
     * @param newData Optional new data to set. For a MultipleSelectivity instance the data must be
     *                an array of objects with 'id' and 'text' properties, for a SingleSelectivity
     *                instance the data must be a single such object or null to indicate no item is
     *                selected.
     * @param options Optional options object. May contain the following property:
     *                triggerChange - Set to false to suppress the "change" event being triggered.
     *                                Note this will also cause the UI to not update automatically;
     *                                so you may want to call rerenderSelection() manually when
     *                                using this option.
     *
     * @return If newData is omitted, this method returns the current data.
     */data:function data(newData,options){options = options || {};if(newData === undefined){return this._data;}else {newData = this.validateData(newData);this._data = newData;this._value = this.getValueForData(newData);if(options.triggerChange !== false){this.triggerChange();}}}, /**
     * Destroys the Selectivity instance.
     */destroy:function destroy(){this.undelegateEvents();var $el=this.$el;$el.children().remove();$el[0].selectivity = null;$el = null;}, /**
     * Filters the results to be displayed in the dropdown.
     *
     * The default implementation simply returns the results unfiltered, but the MultipleSelectivity
     * class overrides this method to filter out any items that have already been selected.
     *
     * @param results Array of items with 'id' and 'text' properties.
     *
     * @return The filtered array.
     */filterResults:function filterResults(results){return results;}, /**
     * Applies focus to the input.
     */focus:function focus(){if(this.$searchInput){this.$searchInput.focus();}}, /**
     * Returns the correct item for a given ID.
     *
     * @param id The ID to get the item for.
     *
     * @return The corresponding item. Will be an object with 'id' and 'text' properties or null if
     *         the item cannot be found. Note that if no items are defined, this method assumes the
     *         text labels will be equal to the IDs.
     */getItemForId:function getItemForId(id){var items=this.items;if(items){return Selectivity.findNestedById(items,id);}else {return {id:id,text:'' + id};}}, /**
     * Initializes the search input element.
     *
     * Sets the $searchInput property, invokes all search input listeners and attaches the default
     * action of searching when something is typed.
     *
     * @param $input jQuery container for the input element.
     * @param options Optional options object. May contain the following property:
     *                noSearch - If true, no event handlers are setup to initiate searching when
     *                           the user types in the input field. This is useful if you want to
     *                           use the input only to handle keyboard support.
     */initSearchInput:function initSearchInput($input,options){this.$searchInput = $input;this.searchInputListeners.forEach((function(listener){listener(this,$input);}).bind(this));if(!options || !options.noSearch){$input.on('keyup',(function(event){if(!event.isDefaultPrevented()){this.search();}}).bind(this));}}, /**
     * Opens the dropdown.
     *
     * @param options Optional options object. May contain the following property:
     *                search - Boolean whether the dropdown should be initialized by performing a
     *                         search for the empty string (ie. display all results). Default is
     *                         true.
     *                showSearchInput - Boolean whether a search input should be shown in the
     *                                  dropdown. Default is false.
     */open:function open(options){options = options || {};if(!this.dropdown){if(this.triggerEvent('selectivity-opening')){var Dropdown=this.options.dropdown || Selectivity.Dropdown;if(Dropdown){this.dropdown = new Dropdown({items:this.items,position:this.options.positionDropdown,query:this.options.query,selectivity:this,showSearchInput:options.showSearchInput});}if(options.search !== false){this.search('');}}this.$el.children().toggleClass('open',true);}}, /**
     * (Re-)positions the dropdown.
     */positionDropdown:function positionDropdown(){if(this.dropdown){this.dropdown.position();}}, /**
     * Searches for results based on the term given or the term entered in the search input.
     *
     * If an items array has been passed with the options to the Selectivity instance, a local
     * search will be performed among those items. Otherwise, the query function specified in the
     * options will be used to perform the search. If neither is defined, nothing happens.
     *
     * @param term Optional term to search for. If ommitted, the value of the search input element
     *             is used as term.
     */search:function search(term){if(term === undefined){term = this.$searchInput?this.$searchInput.val():'';}this.open({search:false});if(this.dropdown){this.dropdown.search(term);}}, /**
     * Sets one or more options on this Selectivity instance.
     *
     * @param options Options object. May contain one or more of the following properties:
     *                closeOnSelect - Set to false to keep the dropdown open after the user has
     *                                selected an item. This is useful if you want to allow the user
     *                                to quickly select multiple items. The default value is true.
     *                dropdown - Custom dropdown implementation to use for this instance.
     *                initSelection - Function to map values by ID to selection data. This function
     *                                receives two arguments, 'value' and 'callback'. The value is
     *                                the current value of the selection, which is an ID or an array
     *                                of IDs depending on the input type. The callback should be
     *                                invoked with an object or array of objects, respectively,
     *                                containing 'id' and 'text' properties.
     *                items - Array of items from which to select. Should be an array of objects
     *                        with 'id' and 'text' properties. As convenience, you may also pass an
     *                        array of strings, in which case the same string is used for both the
     *                        'id' and 'text' properties. If items are given, all items are expected
     *                        to be available locally and all selection operations operate on this
     *                        local array only. If null, items are not available locally, and a
     *                        query function should be provided to fetch remote data.
     *                matcher - Function to determine whether text matches a given search term. Note
     *                          this function is only used if you have specified an array of items.
     *                          Receives two arguments:
     *                          item - The item that should match the search term.
     *                          term - The search term. Note that for performance reasons, the term
     *                                 has always been already processed using
     *                                 Selectivity.transformText().
     *                          The method should return the item if it matches, and null otherwise.
     *                          If the item has a children array, the matcher is expected to filter
     *                          those itself (be sure to only return the filtered array of children
     *                          in the returned item and not to modify the children of the item
     *                          argument).
     *                placeholder - Placeholder text to display when the element has no focus and
     *                              no selected items.
     *                positionDropdown - Function to position the dropdown. Receives two arguments:
     *                                   $dropdownEl - The element to be positioned.
     *                                   $selectEl - The element of the Selectivity instance, that
     *                                               you can position the dropdown to.
     *                                   The default implementation positions the dropdown element
     *                                   under the Selectivity's element and gives it the same
     *                                   width.
     *                query - Function to use for querying items. Receives a single object as
     *                        argument with the following properties:
     *                        callback - Callback to invoke when the results are available. This
     *                                   callback should be passed a single object as argument with
     *                                   the following properties:
     *                                   more - Boolean that can be set to true to indicate there
     *                                          are more results available. Additional results may
     *                                          be fetched by the user through pagination.
     *                                   results - Array of result items. The format for the result
     *                                             items is the same as for passing local items.
     *                        offset - This property is only used for pagination and indicates how
     *                                 many results should be skipped when returning more results.
     *                        selectivity - The Selectivity instance the query function is used on.
     *                        term - The search term the user is searching for. Unlike with the
     *                               matcher function, the term has not been processed using
     *                               Selectivity.transformText().
     *                readOnly - If true, disables any modification of the input.
     *                removeOnly - If true, disables any modification of the input except removing
     *                             of selected items.
     *                searchInputListeners - Array of search input listeners. By default, the global
     *                                       array Selectivity.SearchInputListeners is used.
     *                showDropdown - Set to false if you don't want to use any dropdown (you can
     *                               still open it programmatically using open()).
     *                templates - Object with instance-specific templates to override the global
     *                            templates assigned to Selectivity.Templates.
     */setOptions:function setOptions(options){options = options || {};Selectivity.OptionListeners.forEach((function(listener){listener(this,options);}).bind(this));$.extend(this.options,options);var allowedTypes=$.extend({closeOnSelect:'boolean',dropdown:'function|null',initSelection:'function|null',matcher:'function|null',placeholder:'string',positionDropdown:'function|null',query:'function|null',readOnly:'boolean',removeOnly:'boolean',searchInputListeners:'array'},options.allowedTypes);$.each(options,(function(key,value){var type=allowedTypes[key];if(type && !type.split('|').some(function(type){return $.type(value) === type;})){throw new Error(key + ' must be of type ' + type);}switch(key){case 'items':this.items = value === null?value:Selectivity.processItems(value);break;case 'matcher':this.matcher = value;break;case 'searchInputListeners':this.searchInputListeners = value;break;case 'templates':$.extend(this.templates,value);break;}}).bind(this));this.enabled = !this.options.readOnly && !this.options.removeOnly;}, /**
     * Returns the result of the given template.
     *
     * @param templateName Name of the template to process.
     * @param options Options to pass to the template.
     *
     * @return String containing HTML.
     */template:function template(templateName,options){var template=this.templates[templateName];if(template){if($.type(template) === 'function'){return template(options);}else if(template.render){return template.render(options);}else {return template.toString();}}else {throw new Error('Unknown template: ' + templateName);}}, /**
     * Triggers the change event.
     *
     * The event object at least contains the following property:
     * value - The new value of the Selectivity instance.
     *
     * @param Optional additional options added to the event object.
     */triggerChange:function triggerChange(options){this.triggerEvent('change',$.extend({value:this._value},options));}, /**
     * Triggers an event on the instance's element.
     *
     * @param Optional event data to be added to the event object.
     *
     * @return Whether the default action of the event may be executed, ie. returns false if
     *         preventDefault() has been called.
     */triggerEvent:function triggerEvent(eventName,data){var event=$.Event(eventName,data || {});this.$el.trigger(event);return !event.isDefaultPrevented();}, /**
     * Shorthand for value().
     */val:function val(newValue){return this.value(newValue);}, /**
     * Validates a single item. Throws an exception if the item is invalid.
     *
     * @param item The item to validate.
     *
     * @return The validated item. May differ from the input item.
     */validateItem:function validateItem(item){if(item && Selectivity.isValidId(item.id) && $.type(item.text) === 'string'){return item;}else {throw new Error('Item should have id (number or string) and text (string) properties');}}, /**
     * Sets or gets the value of the selection.
     *
     * The value of the selection only concerns the IDs of the selection items. If you are
     * interested in the IDs and the text labels, you should use the data() method.
     *
     * Note that if neither the items option nor the initSelection option have been set, Selectivity
     * will have no way to determine what text labels should be used with the given IDs in which
     * case it will assume the text is equal to the ID. This is useful if you're working with tags,
     * or selecting e-mail addresses for instance, but may not always be what you want.
     *
     * @param newValue Optional new value to set. For a MultipleSelectivity instance the value must
     *                 be an array of IDs, for a SingleSelectivity instance the value must be a
     *                 single ID (a string or a number) or null to indicate no item is selected.
     * @param options Optional options object. May contain the following property:
     *                triggerChange - Set to false to suppress the "change" event being triggered.
     *                                Note this will also cause the UI to not update automatically;
     *                                so you may want to call rerenderSelection() manually when
     *                                using this option.
     *
     * @return If newValue is omitted, this method returns the current value.
     */value:function value(newValue,options){options = options || {};if(newValue === undefined){return this._value;}else {newValue = this.validateValue(newValue);this._value = newValue;if(this.options.initSelection){this.options.initSelection(newValue,(function(data){if(this._value === newValue){this._data = this.validateData(data);if(options.triggerChange !== false){this.triggerChange();}}}).bind(this));}else {this._data = this.getDataForValue(newValue);if(options.triggerChange !== false){this.triggerChange();}}}}, /**
     * @private
     */_closed:function _closed(){this.dropdown = null;this.$el.children().toggleClass('open',false);}, /**
     * @private
     */_getItemId:function _getItemId(elementOrEvent){ // returns the item ID related to an element or event target.
// IDs can be either numbers or strings, but attribute values are always strings, so we
// will have to find out whether the item ID ought to be a number or string ourselves.
// $.fn.data() is a bit overzealous for our case, because it returns a number whenever the
// attribute value can be parsed as a number. however, it is possible an item had an ID
// which is a string but which is parseable as number, in which case we verify if the ID
// as number is actually found among the data or results. if it isn't, we assume it was
// supposed to be a string after all...
var $element;if(elementOrEvent.target){$element = $(elementOrEvent.target).closest('[data-item-id]');}else if(elementOrEvent.length){$element = elementOrEvent;}else {$element = $(elementOrEvent);}var id=$element.data('item-id');if($.type(id) === 'string'){return id;}else {if(Selectivity.findById(this._data || [],id)){return id;}else {var dropdown=this.dropdown;while(dropdown) {if(Selectivity.findNestedById(dropdown.results,id)){return id;} // FIXME: reference to submenu doesn't belong in base module
dropdown = dropdown.submenu;}return '' + id;}}}, /**
     * @private
     */_mouseout:function _mouseout(){this.$el.children().toggleClass('hover',false);}, /**
     * @private
     */_mouseover:function _mouseover(){this.$el.children().toggleClass('hover',true);}}); /**
 * Dropdown class to use for displaying dropdowns.
 *
 * The default implementation of a dropdown is defined in the selectivity-dropdown module.
 */Selectivity.Dropdown = null; /**
 * Mapping of input types.
 */Selectivity.InputTypes = {}; /**
 * Array of option listeners.
 *
 * Option listeners are invoked when setOptions() is called. Every listener receives two arguments:
 *
 * selectivity - The Selectivity instance.
 * options - The options that are about to be set. The listener may modify this options object.
 *
 * An example of an option listener is the selectivity-traditional module.
 */Selectivity.OptionListeners = []; /**
 * Array of search input listeners.
 *
 * Search input listeners are invoked when initSearchInput() is called (typically right after the
 * search input is created). Every listener receives two arguments:
 *
 * selectivity - The Selectivity instance.
 * $input - jQuery container with the search input.
 *
 * An example of a search input listener is the selectivity-keyboard module.
 */Selectivity.SearchInputListeners = []; /**
 * Mapping with templates to use for rendering select boxes and dropdowns. See
 * selectivity-templates.js for a useful set of default templates, as well as for documentation of
 * the individual templates.
 */Selectivity.Templates = {}; /**
 * Finds an item in the given array with the specified ID.
 *
 * @param array Array to search in.
 * @param id ID to search for.
 *
 * @return The item in the array with the given ID, or null if the item was not found.
 */Selectivity.findById = function(array,id){var index=Selectivity.findIndexById(array,id);return index > -1?array[index]:null;}; /**
 * Finds the index of an item in the given array with the specified ID.
 *
 * @param array Array to search in.
 * @param id ID to search for.
 *
 * @return The index of the item in the array with the given ID, or -1 if the item was not found.
 */Selectivity.findIndexById = function(array,id){for(var i=0,length=array.length;i < length;i++) {if(array[i].id === id){return i;}}return -1;}; /**
 * Finds an item in the given array with the specified ID. Items in the array may contain 'children'
 * properties which in turn will be searched for the item.
 *
 * @param array Array to search in.
 * @param id ID to search for.
 *
 * @return The item in the array with the given ID, or null if the item was not found.
 */Selectivity.findNestedById = null && function(array,id){for(var i=0,length=array.length;i < length;i++) {var item=array[i];if(item.id === id){return item;}else if(item.children){var result=Selectivity.findNestedById(item.children,id);if(result){return result;}}}return null;}; /**
 * Utility method for inheriting another class.
 *
 * @param SubClass Constructor function of the subclass.
 * @param SuperClass Optional constructor function of the superclass. If omitted, Selectivity is
 *                   used as superclass.
 * @param prototype Object with methods you want to add to the subclass prototype.
 *
 * @return A utility function for calling the methods of the superclass. This function receives two
 *         arguments: The this object on which you want to execute the method and the name of the
 *         method. Any arguments past those are passed to the superclass method.
 */Selectivity.inherits = function(SubClass,SuperClass,prototype){if(arguments.length === 2){prototype = SuperClass;SuperClass = Selectivity;}SubClass.prototype = $.extend(Object.create(SuperClass.prototype),{constructor:SubClass},prototype);return function(self,methodName){SuperClass.prototype[methodName].apply(self,Array.prototype.slice.call(arguments,2));};}; /**
 * Checks whether a value can be used as a valid ID for selection items. Only numbers and strings
 * are accepted to be used as IDs.
 *
 * @param id The value to check whether it is a valid ID.
 *
 * @return true if the value is a valid ID, false otherwise.
 */Selectivity.isValidId = function(id){var type=$.type(id);return type === 'number' || type === 'string';}; /**
 * Decides whether a given item matches a search term. The default implementation simply
 * checks whether the term is contained within the item's text, after transforming them using
 * transformText().
 *
 * @param item The item that should match the search term.
 * @param term The search term. Note that for performance reasons, the term has always been already
 *             processed using transformText().
 *
 * @return true if the text matches the term, false otherwise.
 */Selectivity.matcher = function(item,term){var result=null;if(Selectivity.transformText(item.text).indexOf(term) > -1){result = item;}else if(item.children){var matchingChildren=item.children.map(function(child){return Selectivity.matcher(child,term);}).filter(function(child){return !!child;});if(matchingChildren.length){result = {id:item.id,text:item.text,children:matchingChildren};}}return result;}; /**
 * Helper function for processing items.
 *
 * @param item The item to process, either as object containing 'id' and 'text' properties or just
 *             as ID. The 'id' property of an item is optional if it has a 'children' property
 *             containing an array of items.
 *
 * @return Object containing 'id' and 'text' properties.
 */Selectivity.processItem = function(item){if(Selectivity.isValidId(item)){return {id:item,text:'' + item};}else if(item && (Selectivity.isValidId(item.id) || item.children) && $.type(item.text) === 'string'){if(item.children){item.children = Selectivity.processItems(item.children);}return item;}else {throw new Error('invalid item');}}; /**
 * Helper function for processing an array of items.
 *
 * @param items Array of items to process. See processItem() for details about a single item.
 *
 * @return Array with items.
 */Selectivity.processItems = function(items){if($.type(items) === 'array'){return items.map(Selectivity.processItem);}else {throw new Error('invalid items');}}; /**
 * Quotes a string so it can be used in a CSS attribute selector. It adds double quotes to the
 * string and escapes all occurrences of the quote character inside the string.
 *
 * @param string The string to quote.
 *
 * @return The quoted string.
 */Selectivity.quoteCssAttr = function(string){return '"' + ('' + string).replace(/\\/g,'\\\\').replace(/"/g,'\\"') + '"';}; /**
 * Transforms text in order to find matches. The default implementation casts all strings to
 * lower-case so that any matches found will be case-insensitive.
 *
 * @param string The string to transform.
 *
 * @return The transformed string.
 */Selectivity.transformText = function(string){return string.toLowerCase();};module.exports = $.fn.selectivity = Selectivity;},{"2":2,"jquery":"jquery"}],9:[function(_dereq_,module,exports){'use strict';var DIACRITICS={"Ⓐ":'A',"Ａ":'A',"À":'A',"Á":'A',"Â":'A',"Ầ":'A',"Ấ":'A',"Ẫ":'A',"Ẩ":'A',"Ã":'A',"Ā":'A',"Ă":'A',"Ằ":'A',"Ắ":'A',"Ẵ":'A',"Ẳ":'A',"Ȧ":'A',"Ǡ":'A',"Ä":'A',"Ǟ":'A',"Ả":'A',"Å":'A',"Ǻ":'A',"Ǎ":'A',"Ȁ":'A',"Ȃ":'A',"Ạ":'A',"Ậ":'A',"Ặ":'A',"Ḁ":'A',"Ą":'A',"Ⱥ":'A',"Ɐ":'A',"Ꜳ":'AA',"Æ":'AE',"Ǽ":'AE',"Ǣ":'AE',"Ꜵ":'AO',"Ꜷ":'AU',"Ꜹ":'AV',"Ꜻ":'AV',"Ꜽ":'AY',"Ⓑ":'B',"Ｂ":'B',"Ḃ":'B',"Ḅ":'B',"Ḇ":'B',"Ƀ":'B',"Ƃ":'B',"Ɓ":'B',"Ⓒ":'C',"Ｃ":'C',"Ć":'C',"Ĉ":'C',"Ċ":'C',"Č":'C',"Ç":'C',"Ḉ":'C',"Ƈ":'C',"Ȼ":'C',"Ꜿ":'C',"Ⓓ":'D',"Ｄ":'D',"Ḋ":'D',"Ď":'D',"Ḍ":'D',"Ḑ":'D',"Ḓ":'D',"Ḏ":'D',"Đ":'D',"Ƌ":'D',"Ɗ":'D',"Ɖ":'D',"Ꝺ":'D',"Ǳ":'DZ',"Ǆ":'DZ',"ǲ":'Dz',"ǅ":'Dz',"Ⓔ":'E',"Ｅ":'E',"È":'E',"É":'E',"Ê":'E',"Ề":'E',"Ế":'E',"Ễ":'E',"Ể":'E',"Ẽ":'E',"Ē":'E',"Ḕ":'E',"Ḗ":'E',"Ĕ":'E',"Ė":'E',"Ë":'E',"Ẻ":'E',"Ě":'E',"Ȅ":'E',"Ȇ":'E',"Ẹ":'E',"Ệ":'E',"Ȩ":'E',"Ḝ":'E',"Ę":'E',"Ḙ":'E',"Ḛ":'E',"Ɛ":'E',"Ǝ":'E',"Ⓕ":'F',"Ｆ":'F',"Ḟ":'F',"Ƒ":'F',"Ꝼ":'F',"Ⓖ":'G',"Ｇ":'G',"Ǵ":'G',"Ĝ":'G',"Ḡ":'G',"Ğ":'G',"Ġ":'G',"Ǧ":'G',"Ģ":'G',"Ǥ":'G',"Ɠ":'G',"Ꞡ":'G',"Ᵹ":'G',"Ꝿ":'G',"Ⓗ":'H',"Ｈ":'H',"Ĥ":'H',"Ḣ":'H',"Ḧ":'H',"Ȟ":'H',"Ḥ":'H',"Ḩ":'H',"Ḫ":'H',"Ħ":'H',"Ⱨ":'H',"Ⱶ":'H',"Ɥ":'H',"Ⓘ":'I',"Ｉ":'I',"Ì":'I',"Í":'I',"Î":'I',"Ĩ":'I',"Ī":'I',"Ĭ":'I',"İ":'I',"Ï":'I',"Ḯ":'I',"Ỉ":'I',"Ǐ":'I',"Ȉ":'I',"Ȋ":'I',"Ị":'I',"Į":'I',"Ḭ":'I',"Ɨ":'I',"Ⓙ":'J',"Ｊ":'J',"Ĵ":'J',"Ɉ":'J',"Ⓚ":'K',"Ｋ":'K',"Ḱ":'K',"Ǩ":'K',"Ḳ":'K',"Ķ":'K',"Ḵ":'K',"Ƙ":'K',"Ⱪ":'K',"Ꝁ":'K',"Ꝃ":'K',"Ꝅ":'K',"Ꞣ":'K',"Ⓛ":'L',"Ｌ":'L',"Ŀ":'L',"Ĺ":'L',"Ľ":'L',"Ḷ":'L',"Ḹ":'L',"Ļ":'L',"Ḽ":'L',"Ḻ":'L',"Ł":'L',"Ƚ":'L',"Ɫ":'L',"Ⱡ":'L',"Ꝉ":'L',"Ꝇ":'L',"Ꞁ":'L',"Ǉ":'LJ',"ǈ":'Lj',"Ⓜ":'M',"Ｍ":'M',"Ḿ":'M',"Ṁ":'M',"Ṃ":'M',"Ɱ":'M',"Ɯ":'M',"Ⓝ":'N',"Ｎ":'N',"Ǹ":'N',"Ń":'N',"Ñ":'N',"Ṅ":'N',"Ň":'N',"Ṇ":'N',"Ņ":'N',"Ṋ":'N',"Ṉ":'N',"Ƞ":'N',"Ɲ":'N',"Ꞑ":'N',"Ꞥ":'N',"Ǌ":'NJ',"ǋ":'Nj',"Ⓞ":'O',"Ｏ":'O',"Ò":'O',"Ó":'O',"Ô":'O',"Ồ":'O',"Ố":'O',"Ỗ":'O',"Ổ":'O',"Õ":'O',"Ṍ":'O',"Ȭ":'O',"Ṏ":'O',"Ō":'O',"Ṑ":'O',"Ṓ":'O',"Ŏ":'O',"Ȯ":'O',"Ȱ":'O',"Ö":'O',"Ȫ":'O',"Ỏ":'O',"Ő":'O',"Ǒ":'O',"Ȍ":'O',"Ȏ":'O',"Ơ":'O',"Ờ":'O',"Ớ":'O',"Ỡ":'O',"Ở":'O',"Ợ":'O',"Ọ":'O',"Ộ":'O',"Ǫ":'O',"Ǭ":'O',"Ø":'O',"Ǿ":'O',"Ɔ":'O',"Ɵ":'O',"Ꝋ":'O',"Ꝍ":'O',"Ƣ":'OI',"Ꝏ":'OO',"Ȣ":'OU',"Ⓟ":'P',"Ｐ":'P',"Ṕ":'P',"Ṗ":'P',"Ƥ":'P',"Ᵽ":'P',"Ꝑ":'P',"Ꝓ":'P',"Ꝕ":'P',"Ⓠ":'Q',"Ｑ":'Q',"Ꝗ":'Q',"Ꝙ":'Q',"Ɋ":'Q',"Ⓡ":'R',"Ｒ":'R',"Ŕ":'R',"Ṙ":'R',"Ř":'R',"Ȑ":'R',"Ȓ":'R',"Ṛ":'R',"Ṝ":'R',"Ŗ":'R',"Ṟ":'R',"Ɍ":'R',"Ɽ":'R',"Ꝛ":'R',"Ꞧ":'R',"Ꞃ":'R',"Ⓢ":'S',"Ｓ":'S',"ẞ":'S',"Ś":'S',"Ṥ":'S',"Ŝ":'S',"Ṡ":'S',"Š":'S',"Ṧ":'S',"Ṣ":'S',"Ṩ":'S',"Ș":'S',"Ş":'S',"Ȿ":'S',"Ꞩ":'S',"Ꞅ":'S',"Ⓣ":'T',"Ｔ":'T',"Ṫ":'T',"Ť":'T',"Ṭ":'T',"Ț":'T',"Ţ":'T',"Ṱ":'T',"Ṯ":'T',"Ŧ":'T',"Ƭ":'T',"Ʈ":'T',"Ⱦ":'T',"Ꞇ":'T',"Ꜩ":'TZ',"Ⓤ":'U',"Ｕ":'U',"Ù":'U',"Ú":'U',"Û":'U',"Ũ":'U',"Ṹ":'U',"Ū":'U',"Ṻ":'U',"Ŭ":'U',"Ü":'U',"Ǜ":'U',"Ǘ":'U',"Ǖ":'U',"Ǚ":'U',"Ủ":'U',"Ů":'U',"Ű":'U',"Ǔ":'U',"Ȕ":'U',"Ȗ":'U',"Ư":'U',"Ừ":'U',"Ứ":'U',"Ữ":'U',"Ử":'U',"Ự":'U',"Ụ":'U',"Ṳ":'U',"Ų":'U',"Ṷ":'U',"Ṵ":'U',"Ʉ":'U',"Ⓥ":'V',"Ｖ":'V',"Ṽ":'V',"Ṿ":'V',"Ʋ":'V',"Ꝟ":'V',"Ʌ":'V',"Ꝡ":'VY',"Ⓦ":'W',"Ｗ":'W',"Ẁ":'W',"Ẃ":'W',"Ŵ":'W',"Ẇ":'W',"Ẅ":'W',"Ẉ":'W',"Ⱳ":'W',"Ⓧ":'X',"Ｘ":'X',"Ẋ":'X',"Ẍ":'X',"Ⓨ":'Y',"Ｙ":'Y',"Ỳ":'Y',"Ý":'Y',"Ŷ":'Y',"Ỹ":'Y',"Ȳ":'Y',"Ẏ":'Y',"Ÿ":'Y',"Ỷ":'Y',"Ỵ":'Y',"Ƴ":'Y',"Ɏ":'Y',"Ỿ":'Y',"Ⓩ":'Z',"Ｚ":'Z',"Ź":'Z',"Ẑ":'Z',"Ż":'Z',"Ž":'Z',"Ẓ":'Z',"Ẕ":'Z',"Ƶ":'Z',"Ȥ":'Z',"Ɀ":'Z',"Ⱬ":'Z',"Ꝣ":'Z',"ⓐ":'a',"ａ":'a',"ẚ":'a',"à":'a',"á":'a',"â":'a',"ầ":'a',"ấ":'a',"ẫ":'a',"ẩ":'a',"ã":'a',"ā":'a',"ă":'a',"ằ":'a',"ắ":'a',"ẵ":'a',"ẳ":'a',"ȧ":'a',"ǡ":'a',"ä":'a',"ǟ":'a',"ả":'a',"å":'a',"ǻ":'a',"ǎ":'a',"ȁ":'a',"ȃ":'a',"ạ":'a',"ậ":'a',"ặ":'a',"ḁ":'a',"ą":'a',"ⱥ":'a',"ɐ":'a',"ꜳ":'aa',"æ":'ae',"ǽ":'ae',"ǣ":'ae',"ꜵ":'ao',"ꜷ":'au',"ꜹ":'av',"ꜻ":'av',"ꜽ":'ay',"ⓑ":'b',"ｂ":'b',"ḃ":'b',"ḅ":'b',"ḇ":'b',"ƀ":'b',"ƃ":'b',"ɓ":'b',"ⓒ":'c',"ｃ":'c',"ć":'c',"ĉ":'c',"ċ":'c',"č":'c',"ç":'c',"ḉ":'c',"ƈ":'c',"ȼ":'c',"ꜿ":'c',"ↄ":'c',"ⓓ":'d',"ｄ":'d',"ḋ":'d',"ď":'d',"ḍ":'d',"ḑ":'d',"ḓ":'d',"ḏ":'d',"đ":'d',"ƌ":'d',"ɖ":'d',"ɗ":'d',"ꝺ":'d',"ǳ":'dz',"ǆ":'dz',"ⓔ":'e',"ｅ":'e',"è":'e',"é":'e',"ê":'e',"ề":'e',"ế":'e',"ễ":'e',"ể":'e',"ẽ":'e',"ē":'e',"ḕ":'e',"ḗ":'e',"ĕ":'e',"ė":'e',"ë":'e',"ẻ":'e',"ě":'e',"ȅ":'e',"ȇ":'e',"ẹ":'e',"ệ":'e',"ȩ":'e',"ḝ":'e',"ę":'e',"ḙ":'e',"ḛ":'e',"ɇ":'e',"ɛ":'e',"ǝ":'e',"ⓕ":'f',"ｆ":'f',"ḟ":'f',"ƒ":'f',"ꝼ":'f',"ⓖ":'g',"ｇ":'g',"ǵ":'g',"ĝ":'g',"ḡ":'g',"ğ":'g',"ġ":'g',"ǧ":'g',"ģ":'g',"ǥ":'g',"ɠ":'g',"ꞡ":'g',"ᵹ":'g',"ꝿ":'g',"ⓗ":'h',"ｈ":'h',"ĥ":'h',"ḣ":'h',"ḧ":'h',"ȟ":'h',"ḥ":'h',"ḩ":'h',"ḫ":'h',"ẖ":'h',"ħ":'h',"ⱨ":'h',"ⱶ":'h',"ɥ":'h',"ƕ":'hv',"ⓘ":'i',"ｉ":'i',"ì":'i',"í":'i',"î":'i',"ĩ":'i',"ī":'i',"ĭ":'i',"ï":'i',"ḯ":'i',"ỉ":'i',"ǐ":'i',"ȉ":'i',"ȋ":'i',"ị":'i',"į":'i',"ḭ":'i',"ɨ":'i',"ı":'i',"ⓙ":'j',"ｊ":'j',"ĵ":'j',"ǰ":'j',"ɉ":'j',"ⓚ":'k',"ｋ":'k',"ḱ":'k',"ǩ":'k',"ḳ":'k',"ķ":'k',"ḵ":'k',"ƙ":'k',"ⱪ":'k',"ꝁ":'k',"ꝃ":'k',"ꝅ":'k',"ꞣ":'k',"ⓛ":'l',"ｌ":'l',"ŀ":'l',"ĺ":'l',"ľ":'l',"ḷ":'l',"ḹ":'l',"ļ":'l',"ḽ":'l',"ḻ":'l',"ſ":'l',"ł":'l',"ƚ":'l',"ɫ":'l',"ⱡ":'l',"ꝉ":'l',"ꞁ":'l',"ꝇ":'l',"ǉ":'lj',"ⓜ":'m',"ｍ":'m',"ḿ":'m',"ṁ":'m',"ṃ":'m',"ɱ":'m',"ɯ":'m',"ⓝ":'n',"ｎ":'n',"ǹ":'n',"ń":'n',"ñ":'n',"ṅ":'n',"ň":'n',"ṇ":'n',"ņ":'n',"ṋ":'n',"ṉ":'n',"ƞ":'n',"ɲ":'n',"ŉ":'n',"ꞑ":'n',"ꞥ":'n',"ǌ":'nj',"ⓞ":'o',"ｏ":'o',"ò":'o',"ó":'o',"ô":'o',"ồ":'o',"ố":'o',"ỗ":'o',"ổ":'o',"õ":'o',"ṍ":'o',"ȭ":'o',"ṏ":'o',"ō":'o',"ṑ":'o',"ṓ":'o',"ŏ":'o',"ȯ":'o',"ȱ":'o',"ö":'o',"ȫ":'o',"ỏ":'o',"ő":'o',"ǒ":'o',"ȍ":'o',"ȏ":'o',"ơ":'o',"ờ":'o',"ớ":'o',"ỡ":'o',"ở":'o',"ợ":'o',"ọ":'o',"ộ":'o',"ǫ":'o',"ǭ":'o',"ø":'o',"ǿ":'o',"ɔ":'o',"ꝋ":'o',"ꝍ":'o',"ɵ":'o',"ƣ":'oi',"ȣ":'ou',"ꝏ":'oo',"ⓟ":'p',"ｐ":'p',"ṕ":'p',"ṗ":'p',"ƥ":'p',"ᵽ":'p',"ꝑ":'p',"ꝓ":'p',"ꝕ":'p',"ⓠ":'q',"ｑ":'q',"ɋ":'q',"ꝗ":'q',"ꝙ":'q',"ⓡ":'r',"ｒ":'r',"ŕ":'r',"ṙ":'r',"ř":'r',"ȑ":'r',"ȓ":'r',"ṛ":'r',"ṝ":'r',"ŗ":'r',"ṟ":'r',"ɍ":'r',"ɽ":'r',"ꝛ":'r',"ꞧ":'r',"ꞃ":'r',"ⓢ":'s',"ｓ":'s',"ß":'s',"ś":'s',"ṥ":'s',"ŝ":'s',"ṡ":'s',"š":'s',"ṧ":'s',"ṣ":'s',"ṩ":'s',"ș":'s',"ş":'s',"ȿ":'s',"ꞩ":'s',"ꞅ":'s',"ẛ":'s',"ⓣ":'t',"ｔ":'t',"ṫ":'t',"ẗ":'t',"ť":'t',"ṭ":'t',"ț":'t',"ţ":'t',"ṱ":'t',"ṯ":'t',"ŧ":'t',"ƭ":'t',"ʈ":'t',"ⱦ":'t',"ꞇ":'t',"ꜩ":'tz',"ⓤ":'u',"ｕ":'u',"ù":'u',"ú":'u',"û":'u',"ũ":'u',"ṹ":'u',"ū":'u',"ṻ":'u',"ŭ":'u',"ü":'u',"ǜ":'u',"ǘ":'u',"ǖ":'u',"ǚ":'u',"ủ":'u',"ů":'u',"ű":'u',"ǔ":'u',"ȕ":'u',"ȗ":'u',"ư":'u',"ừ":'u',"ứ":'u',"ữ":'u',"ử":'u',"ự":'u',"ụ":'u',"ṳ":'u',"ų":'u',"ṷ":'u',"ṵ":'u',"ʉ":'u',"ⓥ":'v',"ｖ":'v',"ṽ":'v',"ṿ":'v',"ʋ":'v',"ꝟ":'v',"ʌ":'v',"ꝡ":'vy',"ⓦ":'w',"ｗ":'w',"ẁ":'w',"ẃ":'w',"ŵ":'w',"ẇ":'w',"ẅ":'w',"ẘ":'w',"ẉ":'w',"ⱳ":'w',"ⓧ":'x',"ｘ":'x',"ẋ":'x',"ẍ":'x',"ⓨ":'y',"ｙ":'y',"ỳ":'y',"ý":'y',"ŷ":'y',"ỹ":'y',"ȳ":'y',"ẏ":'y',"ÿ":'y',"ỷ":'y',"ẙ":'y',"ỵ":'y',"ƴ":'y',"ɏ":'y',"ỿ":'y',"ⓩ":'z',"ｚ":'z',"ź":'z',"ẑ":'z',"ż":'z',"ž":'z',"ẓ":'z',"ẕ":'z',"ƶ":'z',"ȥ":'z',"ɀ":'z',"ⱬ":'z',"ꝣ":'z',"Ά":"Α","Έ":"Ε","Ή":"Η","Ί":"Ι","Ϊ":"Ι","Ό":"Ο","Ύ":"Υ","Ϋ":"Υ","Ώ":"Ω","ά":"α","έ":"ε","ή":"η","ί":"ι","ϊ":"ι","ΐ":"ι","ό":"ο","ύ":"υ","ϋ":"υ","ΰ":"υ","ω":"ω","ς":"σ"};var Selectivity=_dereq_(8);var previousTransform=Selectivity.transformText; /**
 * Extended version of the transformText() function that simplifies diacritics to their latin1
 * counterparts.
 *
 * Note that if all query functions fetch their results from a remote server, you may not need this
 * function, because it makes sense to remove diacritics server-side in such cases.
 */Selectivity.transformText = function(string){var result='';for(var i=0,length=string.length;i < length;i++) {var character=string[i];result += DIACRITICS[character] || character;}return previousTransform(result);};},{"8":8}],10:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var debounce=_dereq_(3);var EventDelegator=_dereq_(2);var Selectivity=_dereq_(8); /**
 * selectivity Dropdown Constructor.
 *
 * @param options Options object. Should have the following properties:
 *                selectivity - Selectivity instance to show the dropdown for.
 *                showSearchInput - Boolean whether a search input should be shown.
 */function SelectivityDropdown(options){var selectivity=options.selectivity;this.$el = $(selectivity.template('dropdown',{dropdownCssClass:selectivity.options.dropdownCssClass,searchInputPlaceholder:selectivity.options.searchInputPlaceholder,showSearchInput:options.showSearchInput})); /**
     * jQuery container to add the results to.
     */this.$results = this.$('.selectivity-results-container'); /**
     * Boolean indicating whether more results are available than currently displayed in the
     * dropdown.
     */this.hasMore = false; /**
     * The currently highlighted result item.
     */this.highlightedResult = null; /**
     * Boolean whether the load more link is currently highlighted.
     */this.loadMoreHighlighted = false; /**
     * Options passed to the dropdown constructor.
     */this.options = options; /**
     * The results displayed in the dropdown.
     */this.results = []; /**
     * Selectivity instance.
     */this.selectivity = selectivity;this._closed = false;this._closeProxy = this.close.bind(this);if(selectivity.options.closeOnSelect !== false){selectivity.$el.on('selectivity-selecting',this._closeProxy);}this._lastMousePosition = {};this.addToDom();this.position();this.setupCloseHandler();this._suppressMouseWheel();if(options.showSearchInput){selectivity.initSearchInput(this.$('.selectivity-search-input'));selectivity.focus();}EventDelegator.call(this);this.$results.on('scroll touchmove touchend',debounce(this._scrolled.bind(this),50));this.showLoading();setTimeout(this.triggerOpen.bind(this),1);} /**
 * Methods.
 */$.extend(SelectivityDropdown.prototype,EventDelegator.prototype,{ /**
     * Convenience shortcut for this.$el.find(selector).
     */$:function $(selector){return this.$el.find(selector);}, /**
     * Adds the dropdown to the DOM.
     */addToDom:function addToDom(){var $next;var $anchor=this.selectivity.$el;while(($next = $anchor.next('.selectivity-dropdown')).length) {$anchor = $next;}this.$el.insertAfter($anchor);}, /**
     * Closes the dropdown.
     */close:function close(){if(!this._closed){this._closed = true;this.$el.remove();this.removeCloseHandler();this.selectivity.$el.off('selectivity-selecting',this._closeProxy);this.triggerClose();}}, /**
     * Events map.
     *
     * Follows the same format as Backbone: http://backbonejs.org/#View-delegateEvents
     */events:{'click .selectivity-load-more':'_loadMoreClicked','click .selectivity-result-item':'_resultClicked','mouseenter .selectivity-load-more':'_loadMoreHovered','mouseenter .selectivity-result-item':'_resultHovered'}, /**
     * Highlights a result item.
     *
     * @param item The item to highlight.
     */highlight:function highlight(item){if(this.loadMoreHighlighted){this.$('.selectivity-load-more').removeClass('highlight');}this.$('.selectivity-result-item').removeClass('highlight').filter('[data-item-id=' + Selectivity.quoteCssAttr(item.id) + ']').addClass('highlight');this.highlightedResult = item;this.loadMoreHighlighted = false;this.selectivity.triggerEvent('selectivity-highlight',{item:item,id:item.id});}, /**
     * Highlights the load more link.
     *
     * @param item The item to highlight.
     */highlightLoadMore:function highlightLoadMore(){this.$('.selectivity-result-item').removeClass('highlight');this.$('.selectivity-load-more').addClass('highlight');this.highlightedResult = null;this.loadMoreHighlighted = true;}, /**
     * Loads a follow-up page with results after a search.
     *
     * This method should only be called after a call to search() when the callback has indicated
     * more results are available.
     */loadMore:function loadMore(){this.options.query({callback:(function(response){if(response && response.results){this._showResults(Selectivity.processItems(response.results),{add:true,hasMore:!!response.more});}else {throw new Error('callback must be passed a response object');}}).bind(this),error:this._showResults.bind(this,[],{add:true}),offset:this.results.length,selectivity:this.selectivity,term:this.term});}, /**
     * Positions the dropdown inside the DOM.
     */position:function position(){var position=this.options.position;if(position){position(this.$el,this.selectivity.$el);}this._scrolled();}, /**
     * Removes the event handler to close the dropdown.
     */removeCloseHandler:function removeCloseHandler(){$('body').off('click',this._closeProxy);}, /**
     * Renders an array of result items.
     *
     * @param items Array of result items.
     *
     * @return HTML-formatted string to display the result items.
     */renderItems:function renderItems(items){var selectivity=this.selectivity;return items.map(function(item){var result=selectivity.template(item.id?'resultItem':'resultLabel',item);if(item.children){result += selectivity.template('resultChildren',{childrenHtml:this.renderItems(item.children)});}return result;},this).join('');}, /**
     * Searches for results based on the term given or the term entered in the search input.
     *
     * If an items array has been passed with the options to the Selectivity instance, a local
     * search will be performed among those items. Otherwise, the query function specified in the
     * options will be used to perform the search. If neither is defined, nothing happens.
     *
     * @param term Term to search for.
     */search:function search(term){var self=this;term = term || '';self.term = term;if(self.options.items){term = Selectivity.transformText(term);var matcher=self.selectivity.matcher;self._showResults(self.options.items.map(function(item){return matcher(item,term);}).filter(function(item){return !!item;}),{term:term});}else if(self.options.query){self.options.query({callback:function callback(response){if(response && response.results){self._showResults(Selectivity.processItems(response.results),{hasMore:!!response.more,term:term});}else {throw new Error('callback must be passed a response object');}},error:self.showError.bind(self),offset:0,selectivity:self.selectivity,term:term});}}, /**
     * Selects the highlighted item.
     */selectHighlight:function selectHighlight(){if(this.highlightedResult){this.selectItem(this.highlightedResult.id);}else if(this.loadMoreHighlighted){this._loadMoreClicked();}}, /**
     * Selects the item with the given ID.
     *
     * @param id ID of the item to select.
     */selectItem:function selectItem(id){var item=Selectivity.findNestedById(this.results,id);if(item && !item.disabled){var options={id:id,item:item};if(this.selectivity.triggerEvent('selectivity-selecting',options)){this.selectivity.triggerEvent('selectivity-selected',options);}}}, /**
     * Sets up an event handler that will close the dropdown when the Selectivity control loses
     * focus.
     */setupCloseHandler:function setupCloseHandler(){$('body').on('click',this._closeProxy);}, /**
     * Shows an error message.
     *
     * @param message Error message to display.
     * @param options Options object. May contain the following property:
     *                escape - Set to false to disable HTML-escaping of the message. Useful if you
     *                         want to set raw HTML as the message, but may open you up to XSS
     *                         attacks if you're not careful with escaping user input.
     */showError:function showError(message,options){options = options || {};this.$results.html(this.selectivity.template('error',{escape:options.escape !== false,message:message}));this.hasMore = false;this.results = [];this.highlightedResult = null;this.loadMoreHighlighted = false;this.position();}, /**
     * Shows a loading indicator in the dropdown.
     */showLoading:function showLoading(){this.$results.html(this.selectivity.template('loading'));this.hasMore = false;this.results = [];this.highlightedResult = null;this.loadMoreHighlighted = false;this.position();}, /**
     * Shows the results from a search query.
     *
     * @param results Array of result items.
     * @param options Options object. May contain the following properties:
     *                add - True if the results should be added to any already shown results.
     *                hasMore - Boolean whether more results can be fetched using the query()
     *                          function.
     *                term - The search term for which the results are displayed.
     */showResults:function showResults(results,options){var resultsHtml=this.renderItems(results);if(options.hasMore){resultsHtml += this.selectivity.template('loadMore');}else {if(!resultsHtml && !options.add){resultsHtml = this.selectivity.template('noResults',{term:options.term});}}if(options.add){this.$('.selectivity-loading').replaceWith(resultsHtml);this.results = this.results.concat(results);}else {this.$results.html(resultsHtml);this.results = results;}this.hasMore = options.hasMore;if(!options.add || this.loadMoreHighlighted){this._highlightFirstItem(results);}this.position();}, /**
     * Triggers the 'selectivity-close' event.
     */triggerClose:function triggerClose(){this.selectivity.$el.trigger('selectivity-close');}, /**
     * Triggers the 'selectivity-open' event.
     */triggerOpen:function triggerOpen(){this.selectivity.$el.trigger('selectivity-open');}, /**
     * @private
     */_highlightFirstItem:function _highlightFirstItem(results){function findFirstItem(results){for(var i=0,length=results.length;i < length;i++) {var result=results[i];if(result.id){return result;}else if(result.children){var item=findFirstItem(result.children);if(item){return item;}}}}var firstItem=findFirstItem(results);if(firstItem){this.highlight(firstItem);}else {this.highlightedResult = null;this.loadMoreHighlighted = false;}}, /**
     * @private
     */_loadMoreClicked:function _loadMoreClicked(){this.$('.selectivity-load-more').replaceWith(this.selectivity.template('loading'));this.loadMore();this.selectivity.focus();return false;}, /**
     * @private
     */_loadMoreHovered:function _loadMoreHovered(event){if(event.screenX === undefined || event.screenX !== this._lastMousePosition.x || event.screenY === undefined || event.screenY !== this._lastMousePosition.y){this.highlightLoadMore();this._recordMousePosition(event);}}, /**
     * @private
     */_recordMousePosition:function _recordMousePosition(event){this._lastMousePosition = {x:event.screenX,y:event.screenY};}, /**
     * @private
     */_resultClicked:function _resultClicked(event){this.selectItem(this.selectivity._getItemId(event));return false;}, /**
     * @private
     */_resultHovered:function _resultHovered(event){if(event.screenX === undefined || event.screenX !== this._lastMousePosition.x || event.screenY === undefined || event.screenY !== this._lastMousePosition.y){var id=this.selectivity._getItemId(event);var item=Selectivity.findNestedById(this.results,id);if(item && !item.disabled){this.highlight(item);}this._recordMousePosition(event);}}, /**
     * @private
     */_scrolled:function _scrolled(){var $loadMore=this.$('.selectivity-load-more');if($loadMore.length){if($loadMore[0].offsetTop - this.$results[0].scrollTop < this.$el.height()){this._loadMoreClicked();}}}, /**
     * @private
     */_showResults:function _showResults(results,options){this.showResults(this.selectivity.filterResults(results),options);}, /**
     * @private
     */_suppressMouseWheel:function _suppressMouseWheel(){var suppressMouseWheelSelector=this.selectivity.options.suppressMouseWheelSelector;if(suppressMouseWheelSelector === null){return;}var selector=suppressMouseWheelSelector || '.selectivity-results-container';this.$el.on('DOMMouseScroll mousewheel',selector,function(event){ // Thanks to Troy Alford:
// http://stackoverflow.com/questions/5802467/prevent-scrolling-of-parent-element
var $el=$(this),scrollTop=this.scrollTop,scrollHeight=this.scrollHeight,height=$el.height(),originalEvent=event.originalEvent,delta=event.type === 'DOMMouseScroll'?originalEvent.detail * -40:originalEvent.wheelDelta,up=delta > 0;function prevent(){event.stopPropagation();event.preventDefault();event.returnValue = false;return false;}if(scrollHeight > height){if(!up && -delta > scrollHeight - height - scrollTop){ // Scrolling down, but this will take us past the bottom.
$el.scrollTop(scrollHeight);return prevent();}else if(up && delta > scrollTop){ // Scrolling up, but this will take us past the top.
$el.scrollTop(0);return prevent();}}});}});module.exports = Selectivity.Dropdown = SelectivityDropdown;},{"2":2,"3":3,"8":8,"jquery":"jquery"}],11:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8);var MultipleSelectivity=_dereq_(14);function isValidEmail(email){var atIndex=email.indexOf('@');var dotIndex=email.lastIndexOf('.');var spaceIndex=email.indexOf(' ');return atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < email.length - 2 && spaceIndex === -1;}function lastWord(token,length){length = length === undefined?token.length:length;for(var i=length - 1;i >= 0;i--) {if(/\s/.test(token[i])){return token.slice(i + 1,length);}}return token.slice(0,length);}function stripEnclosure(token,enclosure){if(token.slice(0,1) === enclosure[0] && token.slice(-1) === enclosure[1]){return token.slice(1,-1).trim();}else {return token.trim();}}function createEmailItem(token){var email=lastWord(token);var name=token.slice(0,-email.length).trim();if(isValidEmail(email)){email = stripEnclosure(stripEnclosure(email,'()'),'<>');name = stripEnclosure(name,'""').trim() || email;return {id:email,text:name};}else {return token.trim()?{id:token,text:token}:null;}}function emailTokenizer(input,selection,createToken){function hasToken(input){if(input){for(var i=0,length=input.length;i < length;i++) {switch(input[i]){case ';':case ',':case '\n':return true;case ' ':case '\t':if(isValidEmail(lastWord(input,i))){return true;}break;case '"':do {i++;}while(i < length && input[i] !== '"');break;default:continue;}}}return false;}function takeToken(input){for(var i=0,length=input.length;i < length;i++) {switch(input[i]){case ';':case ',':case '\n':return {term:input.slice(0,i),input:input.slice(i + 1)};case ' ':case '\t':if(isValidEmail(lastWord(input,i))){return {term:input.slice(0,i),input:input.slice(i + 1)};}break;case '"':do {i++;}while(i < length && input[i] !== '"');break;default:continue;}}return {};}while(hasToken(input)) {var token=takeToken(input);if(token.term){var item=createEmailItem(token.term);if(item && !(item.id && Selectivity.findById(selection,item.id))){createToken(item);}}input = token.input;}return input;} /**
 * Emailselectivity Constructor.
 *
 * @param options Options object. Accepts all options from the MultipleSelectivity Constructor.
 */function Emailselectivity(options){MultipleSelectivity.call(this,options);} /**
 * Methods.
 */var callSuper=Selectivity.inherits(Emailselectivity,MultipleSelectivity,{ /**
     * @inherit
     */initSearchInput:function initSearchInput($input){callSuper(this,'initSearchInput',$input);$input.on('blur',(function(){var term=$input.val();if(isValidEmail(lastWord(term))){this.add(createEmailItem(term));}}).bind(this));}, /**
     * @inherit
     *
     * Note that for the Email input type the option showDropdown is set to false and the tokenizer
     * option is set to a tokenizer specialized for email addresses.
     */setOptions:function setOptions(options){options = $.extend({createTokenItem:createEmailItem,showDropdown:false,tokenizer:emailTokenizer},options);callSuper(this,'setOptions',options);}});module.exports = Selectivity.InputTypes.Email = Emailselectivity;},{"14":14,"8":8,"jquery":"jquery"}],12:[function(_dereq_,module,exports){'use strict';var Selectivity=_dereq_(8);var KEY_BACKSPACE=8;var KEY_DOWN_ARROW=40;var KEY_ENTER=13;var KEY_ESCAPE=27;var KEY_TAB=9;var KEY_UP_ARROW=38; /**
 * Search input listener providing keyboard support for navigating the dropdown.
 */function listener(selectivity,$input){var keydownCanceled=false;var closeSubmenu=null; /**
     * Moves a dropdown's highlight to the next or previous result item.
     *
     * @param delta Either 1 to move to the next item, or -1 to move to the previous item.
     */function moveHighlight(dropdown,delta){function findElementIndex($elements,selector){for(var i=0,length=$elements.length;i < length;i++) {if($elements.eq(i).is(selector)){return i;}}return -1;}function scrollToHighlight(){var $el;if(dropdown.highlightedResult){var quotedId=Selectivity.quoteCssAttr(dropdown.highlightedResult.id);$el = dropdown.$('.selectivity-result-item[data-item-id=' + quotedId + ']');}else if(dropdown.loadMoreHighlighted){$el = dropdown.$('.selectivity-load-more');}else {return; // no highlight to scroll to
}var position=$el.position();if(!position){return;}var top=position.top;var resultsHeight=dropdown.$results.height();var elHeight=$el.outerHeight?$el.outerHeight():$el.height();if(top < 0 || top > resultsHeight - elHeight){top += dropdown.$results.scrollTop();dropdown.$results.scrollTop(delta < 0?top:top - resultsHeight + elHeight);}}if(dropdown.submenu){moveHighlight(dropdown.submenu,delta);return;}var results=dropdown.results;if(results.length){var $results=dropdown.$('.selectivity-result-item');var defaultIndex=delta > 0?0:$results.length - 1;var index=defaultIndex;var highlightedResult=dropdown.highlightedResult;if(highlightedResult){var quotedId=Selectivity.quoteCssAttr(highlightedResult.id);index = findElementIndex($results,'[data-item-id=' + quotedId + ']') + delta;if(delta > 0?index >= $results.length:index < 0){if(dropdown.hasMore){dropdown.highlightLoadMore();scrollToHighlight();return;}else {index = defaultIndex;}}}var result=Selectivity.findNestedById(results,selectivity._getItemId($results[index]));if(result){dropdown.highlight(result,{delay:!!result.submenu});scrollToHighlight();}}}function keyHeld(event){var dropdown=selectivity.dropdown;if(dropdown){if(event.keyCode === KEY_BACKSPACE){if(!$input.val()){if(dropdown.submenu){var submenu=dropdown.submenu;while(submenu.submenu) {submenu = submenu.submenu;}closeSubmenu = submenu;}event.preventDefault();keydownCanceled = true;}}else if(event.keyCode === KEY_DOWN_ARROW){moveHighlight(dropdown,1);}else if(event.keyCode === KEY_UP_ARROW){moveHighlight(dropdown,-1);}else if(event.keyCode === KEY_TAB){setTimeout(function(){selectivity.close({keepFocus:false});},1);}else if(event.keyCode === KEY_ENTER){event.preventDefault(); // don't submit forms on keydown
}}}function keyReleased(event){function open(){if(selectivity.options.showDropdown !== false){selectivity.open();}}var dropdown=selectivity.dropdown;if(keydownCanceled){event.preventDefault();keydownCanceled = false;if(closeSubmenu){closeSubmenu.close();selectivity.focus();closeSubmenu = null;}}else if(event.keyCode === KEY_BACKSPACE){if(!dropdown && selectivity.options.allowClear){selectivity.clear();}}else if(event.keyCode === KEY_ENTER && !event.ctrlKey){if(dropdown){dropdown.selectHighlight();}else if(selectivity.options.showDropdown !== false){open();}event.preventDefault();}else if(event.keyCode === KEY_ESCAPE){selectivity.close();event.preventDefault();}else if(event.keyCode === KEY_DOWN_ARROW || event.keyCode === KEY_UP_ARROW){ // handled in keyHeld() because the response feels faster and it works with repeated
// events if the user holds the key for a longer period
// still, we issue an open() call here in case the dropdown was not yet open...
open();event.preventDefault();}else {open();}}$input.on('keydown',keyHeld).on('keyup',keyReleased);}Selectivity.SearchInputListeners.push(listener);},{"8":8}],13:[function(_dereq_,module,exports){'use strict';var escape=_dereq_(4);var Selectivity=_dereq_(8); /**
 * Localizable elements of the Selectivity Templates.
 *
 * Be aware that these strings are added straight to the HTML output of the templates, so any
 * non-safe strings should be escaped.
 */Selectivity.Locale = {ajaxError:function ajaxError(term){return 'Failed to fetch results for <b>' + escape(term) + '</b>';},loading:'Loading...',loadMore:'Load more...',needMoreCharacters:function needMoreCharacters(numCharacters){return 'Enter ' + numCharacters + ' more characters to search';},noResults:'No results found',noResultsForTerm:function noResultsForTerm(term){return 'No results for <b>' + escape(term) + '</b>';}};},{"4":4,"8":8}],14:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8);var KEY_BACKSPACE=8;var KEY_DELETE=46;var KEY_ENTER=13; /**
 * MultipleSelectivity Constructor.
 *
 * @param options Options object. Accepts all options from the Selectivity Base Constructor in
 *                addition to those accepted by MultipleSelectivity.setOptions().
 */function MultipleSelectivity(options){Selectivity.call(this,options);this.$el.html(this.template('multipleSelectInput',{enabled:this.enabled})).trigger('selectivity-init','multiple');this._highlightedItemId = null;this.initSearchInput(this.$('.selectivity-multiple-input:not(.selectivity-width-detector)'));this.rerenderSelection();if(!options.positionDropdown){ // dropdowns for multiple-value inputs should open below the select box,
// unless there is not enough space below, but there is space enough above, then it should
// open upwards
this.options.positionDropdown = function($el,$selectEl){var position=$selectEl.position(),dropdownHeight=$el.height(),selectHeight=$selectEl.height(),top=$selectEl[0].getBoundingClientRect().top,bottom=top + selectHeight + dropdownHeight,openUpwards=typeof window !== 'undefined' && bottom > $(window).height() && top - dropdownHeight > 0;var width=$selectEl.outerWidth?$selectEl.outerWidth():$selectEl.width();$el.css({left:position.left + 'px',top:position.top + (openUpwards?-dropdownHeight:selectHeight) + 'px'}).width(width);};}} /**
 * Methods.
 */var callSuper=Selectivity.inherits(MultipleSelectivity,{ /**
     * Adds an item to the selection, if it's not selected yet.
     *
     * @param item The item to add. May be an item with 'id' and 'text' properties or just an ID.
     */add:function add(item){var itemIsId=Selectivity.isValidId(item);var id=itemIsId?item:this.validateItem(item) && item.id;if(this._value.indexOf(id) === -1){this._value.push(id);if(itemIsId && this.options.initSelection){this.options.initSelection([id],(function(data){if(this._value.indexOf(id) > -1){item = this.validateItem(data[0]);this._data.push(item);this.triggerChange({added:item});}}).bind(this));}else {if(itemIsId){item = this.getItemForId(id);}this._data.push(item);this.triggerChange({added:item});}}this.$searchInput.val('');}, /**
     * Clears the data and value.
     */clear:function clear(){this.data([]);}, /**
     * Events map.
     *
     * Follows the same format as Backbone: http://backbonejs.org/#View-delegateEvents
     */events:{'change':'rerenderSelection','change .selectivity-multiple-input':function changeSelectivityMultipleInput(){return false;},'click':'_clicked','click .selectivity-multiple-selected-item':'_itemClicked','keydown .selectivity-multiple-input':'_keyHeld','keyup .selectivity-multiple-input':'_keyReleased','paste .selectivity-multiple-input':'_onPaste','selectivity-selected':'_resultSelected'}, /**
     * @inherit
     */filterResults:function filterResults(results){return results.filter(function(item){return !Selectivity.findById(this._data,item.id);},this);}, /**
     * Returns the correct data for a given value.
     *
     * @param value The value to get the data for. Should be an array of IDs.
     *
     * @return The corresponding data. Will be an array of objects with 'id' and 'text' properties.
     *         Note that if no items are defined, this method assumes the text labels will be equal
     *         to the IDs.
     */getDataForValue:function getDataForValue(value){return value.map(this.getItemForId,this).filter(function(item){return !!item;});}, /**
     * Returns the correct value for the given data.
     *
     * @param data The data to get the value for. Should be an array of objects with 'id' and 'text'
     *             properties.
     *
     * @return The corresponding value. Will be an array of IDs.
     */getValueForData:function getValueForData(data){return data.map(function(item){return item.id;});}, /**
     * Removes an item from the selection, if it is selected.
     *
     * @param item The item to remove. May be an item with 'id' and 'text' properties or just an ID.
     */remove:function remove(item){var id=$.type(item) === 'object'?item.id:item;var removedItem;var index=Selectivity.findIndexById(this._data,id);if(index > -1){removedItem = this._data[index];this._data.splice(index,1);}if(this._value[index] !== id){index = this._value.indexOf(id);}if(index > -1){this._value.splice(index,1);}if(removedItem){this.triggerChange({removed:removedItem});}if(id === this._highlightedItemId){this._highlightedItemId = null;}}, /**
     * Re-renders the selection.
     *
     * Normally the UI is automatically updated whenever the selection changes, but you may want to
     * call this method explicitly if you've updated the selection with the triggerChange option set
     * to false.
     */rerenderSelection:function rerenderSelection(event){event = event || {};if(event.added){this._renderSelectedItem(event.added);this._scrollToBottom();}else if(event.removed){var quotedId=Selectivity.quoteCssAttr(event.removed.id);this.$('.selectivity-multiple-selected-item[data-item-id=' + quotedId + ']').remove();}else {this.$('.selectivity-multiple-selected-item').remove();this._data.forEach(this._renderSelectedItem,this);this._updateInputWidth();}if(event.added || event.removed){if(this.dropdown){this.dropdown.showResults(this.filterResults(this.dropdown.results),{hasMore:this.dropdown.hasMore});}if(this.hasKeyboard){this.focus();}}this.positionDropdown();this._updatePlaceholder();}, /**
     * @inherit
     */search:function search(){var term=this.$searchInput.val();if(this.options.tokenizer){term = this.options.tokenizer(term,this._data,this.add.bind(this),this.options);if($.type(term) === 'string' && term !== this.$searchInput.val()){this.$searchInput.val(term);}}if(this.dropdown){callSuper(this,'search');}}, /**
     * @inherit
     *
     * @param options Options object. In addition to the options supported in the base
     *                implementation, this may contain the following properties:
     *                backspaceHighlightsBeforeDelete - If set to true, when the user enters a
     *                                                  backspace while there is no text in the
     *                                                  search field but there are selected items,
     *                                                  the last selected item will be highlighted
     *                                                  and when a second backspace is entered the
     *                                                  item is deleted. If false, the item gets
     *                                                  deleted on the first backspace. The default
     *                                                  value is true on devices that have touch
     *                                                  input and false on devices that don't.
     *                createTokenItem - Function to create a new item from a user's search term.
     *                                  This is used to turn the term into an item when dropdowns
     *                                  are disabled and the user presses Enter. It is also used by
     *                                  the default tokenizer to create items for individual tokens.
     *                                  The function receives a 'token' parameter which is the
     *                                  search term (or part of a search term) to create an item for
     *                                  and must return an item object with 'id' and 'text'
     *                                  properties or null if no token can be created from the term.
     *                                  The default is a function that returns an item where the id
     *                                  and text both match the token for any non-empty string and
     *                                  which returns null otherwise.
     *                tokenizer - Function for tokenizing search terms. Will receive the following
     *                            parameters:
     *                            input - The input string to tokenize.
     *                            selection - The current selection data.
     *                            createToken - Callback to create a token from the search terms.
     *                                          Should be passed an item object with 'id' and 'text'
     *                                          properties.
     *                            options - The options set on the Selectivity instance.
     *                            Any string returned by the tokenizer function is treated as the
     *                            remainder of untokenized input.
     */setOptions:function setOptions(options){options = options || {};var backspaceHighlightsBeforeDelete='backspaceHighlightsBeforeDelete';if(options[backspaceHighlightsBeforeDelete] === undefined){options[backspaceHighlightsBeforeDelete] = this.hasTouch;}options.allowedTypes = options.allowedTypes || {};options.allowedTypes[backspaceHighlightsBeforeDelete] = 'boolean';var wasEnabled=this.enabled;callSuper(this,'setOptions',options);if(wasEnabled !== this.enabled){this.$el.html(this.template('multipleSelectInput',{enabled:this.enabled}));}}, /**
     * Validates data to set. Throws an exception if the data is invalid.
     *
     * @param data The data to validate. Should be an array of objects with 'id' and 'text'
     *             properties.
     *
     * @return The validated data. This may differ from the input data.
     */validateData:function validateData(data){if(data === null){return [];}else if($.type(data) === 'array'){return data.map(this.validateItem,this);}else {throw new Error('Data for MultiSelectivity instance should be array');}}, /**
     * Validates a value to set. Throws an exception if the value is invalid.
     *
     * @param value The value to validate. Should be an array of IDs.
     *
     * @return The validated value. This may differ from the input value.
     */validateValue:function validateValue(value){if(value === null){return [];}else if($.type(value) === 'array'){if(value.every(Selectivity.isValidId)){return value;}else {throw new Error('Value contains invalid IDs');}}else {throw new Error('Value for MultiSelectivity instance should be an array');}}, /**
     * @private
     */_backspacePressed:function _backspacePressed(){if(this.options.backspaceHighlightsBeforeDelete){if(this._highlightedItemId){this._deletePressed();}else if(this._value.length){this._highlightItem(this._value.slice(-1)[0]);}}else if(this._value.length){this.remove(this._value.slice(-1)[0]);}}, /**
     * @private
     */_clicked:function _clicked(){if(this.enabled){this.focus();this._open();return false;}}, /**
     * @private
     */_createToken:function _createToken(){var term=this.$searchInput.val();var createTokenItem=this.options.createTokenItem;if(term && createTokenItem){var item=createTokenItem(term);if(item){this.add(item);}}}, /**
     * @private
     */_deletePressed:function _deletePressed(){if(this._highlightedItemId){this.remove(this._highlightedItemId);}}, /**
     * @private
     */_highlightItem:function _highlightItem(id){this._highlightedItemId = id;this.$('.selectivity-multiple-selected-item').removeClass('highlighted').filter('[data-item-id=' + Selectivity.quoteCssAttr(id) + ']').addClass('highlighted');if(this.hasKeyboard){this.focus();}}, /**
     * @private
     */_itemClicked:function _itemClicked(event){if(this.enabled){this._highlightItem(this._getItemId(event));}}, /**
     * @private
     */_itemRemoveClicked:function _itemRemoveClicked(event){this.remove(this._getItemId(event));this._updateInputWidth();return false;}, /**
     * @private
     */_keyHeld:function _keyHeld(event){this._originalValue = this.$searchInput.val();if(event.keyCode === KEY_ENTER && !event.ctrlKey){event.preventDefault();}}, /**
     * @private
     */_keyReleased:function _keyReleased(event){var inputHadText=!!this._originalValue;if(event.keyCode === KEY_ENTER && !event.ctrlKey){if(this.options.createTokenItem){this._createToken();}}else if(event.keyCode === KEY_BACKSPACE && !inputHadText){this._backspacePressed();}else if(event.keyCode === KEY_DELETE && !inputHadText){this._deletePressed();}this._updateInputWidth();}, /**
     * @private
     */_onPaste:function _onPaste(){setTimeout((function(){this.search();if(this.options.createTokenItem){this._createToken();}}).bind(this),10);}, /**
     * @private
     */_open:function _open(){if(this.options.showDropdown !== false){this.open();}},_renderSelectedItem:function _renderSelectedItem(item){this.$searchInput.before(this.template('multipleSelectedItem',$.extend({highlighted:item.id === this._highlightedItemId,removable:!this.options.readOnly},item)));var quotedId=Selectivity.quoteCssAttr(item.id);this.$('.selectivity-multiple-selected-item[data-item-id=' + quotedId + ']').find('.selectivity-multiple-selected-item-remove').on('click',this._itemRemoveClicked.bind(this));}, /**
     * @private
     */_resultSelected:function _resultSelected(event){if(this._value.indexOf(event.id) === -1){this.add(event.item);}else {this.remove(event.item);}}, /**
     * @private
     */_scrollToBottom:function _scrollToBottom(){var $inputContainer=this.$('.selectivity-multiple-input-container');$inputContainer.scrollTop($inputContainer.height());}, /**
     * @private
     */_updateInputWidth:function _updateInputWidth(){if(this.enabled){var $input=this.$searchInput,$widthDetector=this.$('.selectivity-width-detector');$widthDetector.text($input.val() || !this._data.length && this.options.placeholder || '');$input.width($widthDetector.width() + 20);this.positionDropdown();}}, /**
     * @private
     */_updatePlaceholder:function _updatePlaceholder(){var placeholder=this._data.length?'':this.options.placeholder;if(this.enabled){this.$searchInput.attr('placeholder',placeholder);}else {this.$('.selectivity-placeholder').text(placeholder);}}});module.exports = Selectivity.InputTypes.Multiple = MultipleSelectivity;},{"8":8,"jquery":"jquery"}],15:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8); /**
 * SingleSelectivity Constructor.
 *
 * @param options Options object. Accepts all options from the Selectivity Base Constructor in
 *                addition to those accepted by SingleSelectivity.setOptions().
 */function SingleSelectivity(options){Selectivity.call(this,options);this.$el.html(this.template('singleSelectInput',this.options)).trigger('selectivity-init','single');this.rerenderSelection();if(!options.positionDropdown){ // dropdowns for single-value inputs should open below the select box,
// unless there is not enough space below, in which case the dropdown should be moved up
// just enough so it fits in the window, but never so much that it reaches above the top
this.options.positionDropdown = function($el,$selectEl){var position=$selectEl.position(),dropdownHeight=$el.height(),selectHeight=$selectEl.height(),top=$selectEl[0].getBoundingClientRect().top,bottom=top + selectHeight + dropdownHeight,deltaUp=0;if(typeof window !== 'undefined'){deltaUp = Math.min(Math.max(bottom - $(window).height(),0),top + selectHeight);}var width=$selectEl.outerWidth?$selectEl.outerWidth():$selectEl.width();$el.css({left:position.left + 'px',top:position.top + selectHeight - deltaUp + 'px'}).width(width);};}if(options.showSearchInputInDropdown === false){this.initSearchInput(this.$('.selectivity-single-select-input'),{noSearch:true});}} /**
 * Methods.
 */var callSuper=Selectivity.inherits(SingleSelectivity,{ /**
     * Events map.
     *
     * Follows the same format as Backbone: http://backbonejs.org/#View-delegateEvents
     */events:{'change':'rerenderSelection','click':'_clicked','focus .selectivity-single-select-input':'_focused','selectivity-selected':'_resultSelected'}, /**
     * Clears the data and value.
     */clear:function clear(){this.data(null);}, /**
     * @inherit
     *
     * @param options Optional options object. May contain the following property:
     *                keepFocus - If false, the focus won't remain on the input.
     */close:function close(options){this._closing = true;callSuper(this,'close');if(!options || options.keepFocus !== false){this.$searchInput.focus();}this._closing = false;}, /**
     * Returns the correct data for a given value.
     *
     * @param value The value to get the data for. Should be an ID.
     *
     * @return The corresponding data. Will be an object with 'id' and 'text' properties. Note that
     *         if no items are defined, this method assumes the text label will be equal to the ID.
     */getDataForValue:function getDataForValue(value){return this.getItemForId(value);}, /**
     * Returns the correct value for the given data.
     *
     * @param data The data to get the value for. Should be an object with 'id' and 'text'
     *             properties or null.
     *
     * @return The corresponding value. Will be an ID or null.
     */getValueForData:function getValueForData(data){return data?data.id:null;}, /**
     * @inherit
     */open:function open(options){this._opening = true;var showSearchInput=this.options.showSearchInputInDropdown !== false;callSuper(this,'open',$.extend({showSearchInput:showSearchInput},options));if(!showSearchInput){this.focus();}this._opening = false;}, /**
     * Re-renders the selection.
     *
     * Normally the UI is automatically updated whenever the selection changes, but you may want to
     * call this method explicitly if you've updated the selection with the triggerChange option set
     * to false.
     */rerenderSelection:function rerenderSelection(){var $container=this.$('.selectivity-single-result-container');if(this._data){$container.html(this.template('singleSelectedItem',$.extend({removable:this.options.allowClear && !this.options.readOnly},this._data)));$container.find('.selectivity-single-selected-item-remove').on('click',this._itemRemoveClicked.bind(this));}else {$container.html(this.template('singleSelectPlaceholder',{placeholder:this.options.placeholder}));}}, /**
     * @inherit
     *
     * @param options Options object. In addition to the options supported in the base
     *                implementation, this may contain the following properties:
     *                allowClear - Boolean whether the selected item may be removed.
     *                showSearchInputInDropdown - Set to false to remove the search input used in
     *                                            dropdowns. The default is true.
     */setOptions:function setOptions(options){options = options || {};options.allowedTypes = $.extend(options.allowedTypes || {},{allowClear:'boolean',showSearchInputInDropdown:'boolean'});callSuper(this,'setOptions',options);}, /**
     * Validates data to set. Throws an exception if the data is invalid.
     *
     * @param data The data to validate. Should be an object with 'id' and 'text' properties or null
     *             to indicate no item is selected.
     *
     * @return The validated data. This may differ from the input data.
     */validateData:function validateData(data){return data === null?data:this.validateItem(data);}, /**
     * Validates a value to set. Throws an exception if the value is invalid.
     *
     * @param value The value to validate. Should be null or a valid ID.
     *
     * @return The validated value. This may differ from the input value.
     */validateValue:function validateValue(value){if(value === null || Selectivity.isValidId(value)){return value;}else {throw new Error('Value for SingleSelectivity instance should be a valid ID or null');}}, /**
     * @private
     */_clicked:function _clicked(){if(this.enabled){if(this.dropdown){this.close();}else if(this.options.showDropdown !== false){this.open();}return false;}}, /**
     * @private
     */_focused:function _focused(){if(this.enabled && !this._closing && !this._opening && this.options.showDropdown !== false){this.open();}}, /**
     * @private
     */_itemRemoveClicked:function _itemRemoveClicked(){this.data(null);return false;}, /**
     * @private
     */_resultSelected:function _resultSelected(event){this.data(event.item);this.close();}});module.exports = Selectivity.InputTypes.Single = SingleSelectivity;},{"8":8,"jquery":"jquery"}],16:[function(_dereq_,module,exports){'use strict';var Selectivity=_dereq_(8);var SelectivityDropdown=_dereq_(10); /**
 * Extended dropdown that supports submenus.
 */function SelectivitySubmenu(options){ /**
     * Optional parent dropdown menu from which this dropdown was opened.
     */this.parentMenu = options.parentMenu;SelectivityDropdown.call(this,options);this._closeSubmenuTimeout = 0;this._openSubmenuTimeout = 0;}var callSuper=Selectivity.inherits(SelectivitySubmenu,SelectivityDropdown,{ /**
     * @inherit
     */close:function close(){if(this.submenu){this.submenu.close();}callSuper(this,'close');if(this.parentMenu){this.parentMenu.submenu = null;this.parentMenu = null;}clearTimeout(this._closeSubmenuTimeout);clearTimeout(this._openSubmenuTimeout);}, /**
     * @inherit
     *
     * @param options Optional options object. May contain the following property:
     *                delay - If true, indicates any submenu should not be opened until after some
     *                        delay.
     */highlight:function highlight(item,options){if(options && options.delay){callSuper(this,'highlight',item);clearTimeout(this._openSubmenuTimeout);this._openSubmenuTimeout = setTimeout(this._doHighlight.bind(this,item),300);}else if(this.submenu){if(this.highlightedResult && this.highlightedResult.id === item.id){this._doHighlight(item);}else {clearTimeout(this._closeSubmenuTimeout);this._closeSubmenuTimeout = setTimeout(this._closeSubmenuAndHighlight.bind(this,item),100);}}else {if(this.parentMenu && this.parentMenu._closeSubmenuTimeout){clearTimeout(this.parentMenu._closeSubmenuTimeout);this.parentMenu._closeSubmenuTimeout = 0;}this._doHighlight(item);}}, /**
     * @inherit
     */search:function search(term){if(this.submenu){this.submenu.search(term);}else {callSuper(this,'search',term);}}, /**
     * @inherit
     */selectHighlight:function selectHighlight(){if(this.submenu){this.submenu.selectHighlight();}else {callSuper(this,'selectHighlight');}}, /**
     * @inherit
     */selectItem:function selectItem(id){var item=Selectivity.findNestedById(this.results,id);if(item && !item.disabled && !item.submenu){var options={id:id,item:item};if(this.selectivity.triggerEvent('selectivity-selecting',options)){this.selectivity.triggerEvent('selectivity-selected',options);}}}, /**
     * @inherit
     */showResults:function showResults(results,options){if(this.submenu){this.submenu.showResults(results,options);}else {callSuper(this,'showResults',results,options);}}, /**
     * @inherit
     */triggerClose:function triggerClose(){if(this.parentMenu){this.selectivity.$el.trigger('selectivity-close-submenu');}else {callSuper(this,'triggerClose');}}, /**
     * @inherit
     */triggerOpen:function triggerOpen(){if(this.parentMenu){this.selectivity.$el.trigger('selectivity-open-submenu');}else {callSuper(this,'triggerOpen');}}, /**
     * @private
     */_closeSubmenuAndHighlight:function _closeSubmenuAndHighlight(item){if(this.submenu){this.submenu.close();}this._doHighlight(item);}, /**
     * @private
     */_doHighlight:function _doHighlight(item){callSuper(this,'highlight',item);if(item.submenu && !this.submenu){var selectivity=this.selectivity;var Dropdown=selectivity.options.dropdown || Selectivity.Dropdown;if(Dropdown){var quotedId=Selectivity.quoteCssAttr(item.id);var $item=this.$('.selectivity-result-item[data-item-id=' + quotedId + ']');var $dropdownEl=this.$el;this.submenu = new Dropdown({items:item.submenu.items || null,parentMenu:this,position:item.submenu.positionDropdown || function($el){var dropdownPosition=$dropdownEl.position();var width=$dropdownEl.width();$el.css({left:dropdownPosition.left + width + 'px',top:$item.position().top + dropdownPosition.top + 'px'}).width(width);},query:item.submenu.query || null,selectivity:selectivity,showSearchInput:item.submenu.showSearchInput});this.submenu.search('');}}}});Selectivity.Dropdown = SelectivitySubmenu;Selectivity.findNestedById = function(array,id){for(var i=0,length=array.length;i < length;i++) {var item=array[i],result;if(item.id === id){result = item;}else if(item.children){result = Selectivity.findNestedById(item.children,id);}else if(item.submenu && item.submenu.items){result = Selectivity.findNestedById(item.submenu.items,id);}if(result){return result;}}return null;};module.exports = SelectivitySubmenu;},{"10":10,"8":8}],17:[function(_dereq_,module,exports){'use strict';var escape=_dereq_(4);var Selectivity=_dereq_(8);_dereq_(13); /**
 * Default set of templates to use with Selectivity.js.
 *
 * Note that every template can be defined as either a string, a function returning a string (like
 * Handlebars templates, for instance) or as an object containing a render function (like Hogan.js
 * templates, for instance).
 */Selectivity.Templates = { /**
     * Renders the dropdown.
     *
     * The template is expected to have at least one element with the class
     * 'selectivity-results-container', which is where all results will be added to.
     *
     * @param options Options object containing the following properties:
     *                dropdownCssClass - Optional CSS class to add to the top-level element.
     *                searchInputPlaceholder - Optional placeholder text to display in the search
     *                                         input in the dropdown.
     *                showSearchInput - Boolean whether a search input should be shown. If true,
     *                                  an input element with the 'selectivity-search-input' is
     *                                  expected.
     */dropdown:function dropdown(options){var extraClass=options.dropdownCssClass?' ' + options.dropdownCssClass:'',searchInput='';if(options.showSearchInput){extraClass += ' has-search-input';var placeholder=options.searchInputPlaceholder;searchInput = '<div class="selectivity-search-input-container">' + '<input type="text" class="selectivity-search-input"' + (placeholder?' placeholder="' + escape(placeholder) + '"':'') + '>' + '</div>';}return '<div class="selectivity-dropdown' + extraClass + '">' + searchInput + '<div class="selectivity-results-container"></div>' + '</div>';}, /**
     * Renders an error message in the dropdown.
     *
     * @param options Options object containing the following properties:
     *                escape - Boolean whether the message should be HTML-escaped.
     *                message - The message to display.
     */error:function error(options){return '<div class="selectivity-error">' + (options.escape?escape(options.message):options.message) + '</div>';}, /**
     * Renders a loading indicator in the dropdown.
     *
     * This template is expected to have an element with a 'selectivity-loading' class which may be
     * replaced with actual results.
     */loading:function loading(){return '<div class="selectivity-loading">' + Selectivity.Locale.loading + '</div>';}, /**
     * Load more indicator.
     *
     * This template is expected to have an element with a 'selectivity-load-more' class which, when
     * clicked, will load more results.
     */loadMore:function loadMore(){return '<div class="selectivity-load-more">' + Selectivity.Locale.loadMore + '</div>';}, /**
     * Renders multi-selection input boxes.
     *
     * The template is expected to have at least have elements with the following classes:
     * 'selectivity-multiple-input-container' - The element containing all the selected items and
     *                                          the input for selecting additional items.
     * 'selectivity-multiple-input' - The actual input element that allows the user to type to
     *                                search for more items. When selected items are added, they are
     *                                inserted right before this element.
     * 'selectivity-width-detector' - This element is optional, but important to make sure the
     *                                '.selectivity-multiple-input' element will fit in the
     *                                container. The width detector also has the
     *                                'select2-multiple-input' class on purpose to be able to detect
     *                                the width of text entered in the input element.
     *
     * @param options Options object containing the following property:
     *                enabled - Boolean whether the input is enabled.
     */multipleSelectInput:function multipleSelectInput(options){return '<div class="selectivity-multiple-input-container">' + (options.enabled?'<input type="text" autocomplete="off" autocorrect="off" ' + 'autocapitalize="off" ' + 'class="selectivity-multiple-input">' + '<span class="selectivity-multiple-input ' + 'selectivity-width-detector"></span>':'<div class="selectivity-multiple-input ' + 'selectivity-placeholder"></div>') + '<div class="selectivity-clearfix"></div>' + '</div>';}, /**
     * Renders a selected item in multi-selection input boxes.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-multiple-selected-item'. This element is also required to have a 'data-item-id'
     * attribute with the ID set to that passed through the options object.
     *
     * An element with the class 'selectivity-multiple-selected-item-remove' should be present
     * which, when clicked, will cause the element to be removed.
     *
     * @param options Options object containing the following properties:
     *                highlighted - Boolean whether this item is currently highlighted.
     *                id - Identifier for the item.
     *                removable - Boolean whether a remove icon should be displayed.
     *                text - Text label which the user sees.
     */multipleSelectedItem:function multipleSelectedItem(options){var extraClass=options.highlighted?' highlighted':'';return '<span class="selectivity-multiple-selected-item' + extraClass + '" ' + 'data-item-id="' + escape(options.id) + '">' + (options.removable?'<a class="selectivity-multiple-selected-item-remove">' + '<i class="fa fa-remove"></i>' + '</a>':'') + escape(options.text) + '</span>';}, /**
     * Renders a message there are no results for the given query.
     *
     * @param options Options object containing the following property:
     *                term - Search term the user is searching for.
     */noResults:function noResults(options){var Locale=Selectivity.Locale;return '<div class="selectivity-error">' + (options.term?Locale.noResultsForTerm(options.term):Locale.noResults) + '</div>';}, /**
     * Renders a container for item children.
     *
     * The template is expected to have an element with the class 'selectivity-result-children'.
     *
     * @param options Options object containing the following property:
     *                childrenHtml - Rendered HTML for the children.
     */resultChildren:function resultChildren(options){return '<div class="selectivity-result-children">' + options.childrenHtml + '</div>';}, /**
     * Render a result item in the dropdown.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-result-item'. This element is also required to have a 'data-item-id' attribute
     * with the ID set to that passed through the options object.
     *
     * @param options Options object containing the following properties:
     *                id - Identifier for the item.
     *                text - Text label which the user sees.
     *                disabled - Truthy if the item should be disabled.
     *                submenu - Truthy if the result item has a menu with subresults.
     */resultItem:function resultItem(options){return '<div class="selectivity-result-item' + (options.disabled?' disabled':'') + '"' + ' data-item-id="' + escape(options.id) + '">' + escape(options.text) + (options.submenu?'<i class="selectivity-submenu-icon fa fa-chevron-right"></i>':'') + '</div>';}, /**
     * Render a result label in the dropdown.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-result-label'.
     *
     * @param options Options object containing the following properties:
     *                text - Text label.
     */resultLabel:function resultLabel(options){return '<div class="selectivity-result-label">' + escape(options.text) + '</div>';}, /**
     * Renders single-select input boxes.
     *
     * The template is expected to have at least one element with the class
     * 'selectivity-single-result-container' which is the element containing the selected item or
     * the placeholder.
     */singleSelectInput:'<div class="selectivity-single-select">' + '<input type="text" class="selectivity-single-select-input">' + '<div class="selectivity-single-result-container"></div>' + '<i class="fa fa-sort-desc selectivity-caret"></i>' + '</div>', /**
     * Renders the placeholder for single-select input boxes.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-placeholder'.
     *
     * @param options Options object containing the following property:
     *                placeholder - The placeholder text.
     */singleSelectPlaceholder:function singleSelectPlaceholder(options){return '<div class="selectivity-placeholder">' + escape(options.placeholder) + '</div>';}, /**
     * Renders the selected item in single-select input boxes.
     *
     * The template is expected to have a top-level element with the class
     * 'selectivity-single-selected-item'. This element is also required to have a 'data-item-id'
     * attribute with the ID set to that passed through the options object.
     *
     * @param options Options object containing the following properties:
     *                id - Identifier for the item.
     *                removable - Boolean whether a remove icon should be displayed.
     *                text - Text label which the user sees.
     */singleSelectedItem:function singleSelectedItem(options){return '<span class="selectivity-single-selected-item" ' + 'data-item-id="' + escape(options.id) + '">' + (options.removable?'<a class="selectivity-single-selected-item-remove">' + '<i class="fa fa-remove"></i>' + '</a>':'') + escape(options.text) + '</span>';}, /**
     * Renders select-box inside single-select input that was initialized on
     * traditional <select> element.
     *
     * @param options Options object containing the following properties:
     *                name - Name of the <select> element.
     *                mode - Mode in which select exists, single or multiple.
     */selectCompliance:function selectCompliance(options){var mode=options.mode;var name=options.name;if(mode === 'multiple' && name.slice(-2) !== '[]'){name += '[]';}return '<select name="' + name + '"' + (mode === 'multiple'?' multiple':'') + '></select>';}, /**
     * Renders the selected item in compliance <select> element as <option>.
     *
     * @param options Options object containing the following properties
     *                id - Identifier for the item.
     *                text - Text label which the user sees.
     */selectOptionCompliance:function selectOptionCompliance(options){return '<option value="' + escape(options.id) + '" selected>' + escape(options.text) + '</option>';}};},{"13":13,"4":4,"8":8}],18:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8);function defaultTokenizer(input,selection,createToken,options){var createTokenItem=options.createTokenItem || function(token){return token?{id:token,text:token}:null;};var separators=options.tokenSeparators;function hasToken(input){return input?separators.some(function(separator){return input.indexOf(separator) > -1;}):false;}function takeToken(input){for(var i=0,length=input.length;i < length;i++) {if(separators.indexOf(input[i]) > -1){return {term:input.slice(0,i),input:input.slice(i + 1)};}}return {};}while(hasToken(input)) {var token=takeToken(input);if(token.term){var item=createTokenItem(token.term);if(item && !Selectivity.findById(selection,item.id)){createToken(item);}}input = token.input;}return input;} /**
 * Option listener that provides a default tokenizer which is used when the tokenSeparators option
 * is specified.
 *
 * @param options Options object. In addition to the options supported in the multi-input
 *                implementation, this may contain the following property:
 *                tokenSeparators - Array of string separators which are used to separate the search
 *                                  string into tokens. If specified and the tokenizer property is
 *                                  not set, the tokenizer property will be set to a function which
 *                                  splits the search term into tokens separated by any of the given
 *                                  separators. The tokens will be converted into selectable items
 *                                  using the 'createTokenItem' function. The default tokenizer also
 *                                  filters out already selected items.
 */Selectivity.OptionListeners.push(function(selectivity,options){if(options.tokenSeparators){options.allowedTypes = $.extend({tokenSeparators:'array'},options.allowedTypes);options.tokenizer = options.tokenizer || defaultTokenizer;}});},{"8":8,"jquery":"jquery"}],19:[function(_dereq_,module,exports){'use strict';var $=window.jQuery || window.Zepto;var Selectivity=_dereq_(8);function replaceSelectElement($el,options){var data=options.multiple?[]:null;var mapOptions=function mapOptions(){var $this=$(this);if($this.is('option')){var text=$this.text();var id=$this.attr('value') || text;if($this.prop('selected')){var item={id:id,text:text};if(options.multiple){data.push(item);}else {data = item;}}return {id:id,text:$this.attr('label') || text};}else {return {text:$this.attr('label'),children:$this.children('option,optgroup').map(mapOptions).get()};}};options.allowClear = 'allowClear' in options?options.allowClear:!$el.prop('required');var items=$el.children('option,optgroup').map(mapOptions).get();options.items = options.query?null:items;options.placeholder = options.placeholder || $el.data('placeholder') || '';options.data = data;var classes=($el.attr('class') || 'selectivity-input').split(' ');if(classes.indexOf('selectivity-input') === -1){classes.push('selectivity-input');}var $div=$('<div>').attr({'id':$el.attr('id'),'class':classes.join(' '),'style':$el.attr('style'),'data-name':$el.attr('name')});$el.replaceWith($div);return $div;}function bindTraditionalSelectEvents(selectivity){var $el=selectivity.$el;$el.on('selectivity-init',function(event,mode){$el.append(selectivity.template('selectCompliance',{mode:mode,name:$el.attr('data-name')})).removeAttr('data-name');}).on('selectivity-init change',function(){var data=selectivity._data;var $select=$el.find('select');if(data instanceof Array){$select.empty();data.forEach(function(item){$select.append(selectivity.template('selectOptionCompliance',item));});}else {if(data){$select.html(selectivity.template('selectOptionCompliance',data));}else {$select.empty();}}});} /**
 * Option listener providing support for converting traditional <select> boxes into Selectivity
 * instances.
 */Selectivity.OptionListeners.push(function(selectivity,options){var $el=selectivity.$el;if($el.is('select')){if($el.attr('autofocus')){setTimeout(function(){selectivity.focus();},1);}selectivity.$el = replaceSelectElement($el,options);selectivity.$el[0].selectivity = selectivity;bindTraditionalSelectEvents(selectivity);}});},{"8":8,"jquery":"jquery"}]},{},[1])(1);});

}).call(this,typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

},{}],18:[function(require,module,exports){
"use strict";

!(function (root, factory) {
    "function" == typeof define && define.amd ? // AMD. Register as an anonymous module unless amdModuleId is set
    define([], function () {
        return root.svg4everybody = factory();
    }) : "object" == typeof exports ? module.exports = factory() : root.svg4everybody = factory();
})(undefined, function () {
    /*! svg4everybody v2.0.3 | github.com/jonathantneal/svg4everybody */
    function embed(svg, target) {
        // if the target exists
        if (target) {
            // create a document fragment to hold the contents of the target
            var fragment = document.createDocumentFragment(),
                viewBox = !svg.getAttribute("viewBox") && target.getAttribute("viewBox");
            // conditionally set the viewBox on the svg
            viewBox && svg.setAttribute("viewBox", viewBox);
            // copy the contents of the clone into the fragment
            for ( // clone the target
            var clone = target.cloneNode(!0); clone.childNodes.length;) {
                fragment.appendChild(clone.firstChild);
            }
            // append the fragment into the svg
            svg.appendChild(fragment);
        }
    }
    function loadreadystatechange(xhr) {
        // listen to changes in the request
        xhr.onreadystatechange = function () {
            // if the request is ready
            if (4 === xhr.readyState) {
                // get the cached html document
                var cachedDocument = xhr._cachedDocument;
                // ensure the cached html document based on the xhr response
                cachedDocument || (cachedDocument = xhr._cachedDocument = document.implementation.createHTMLDocument(""), cachedDocument.body.innerHTML = xhr.responseText, xhr._cachedTarget = {}), // clear the xhr embeds list and embed each item
                xhr._embeds.splice(0).map(function (item) {
                    // get the cached target
                    var target = xhr._cachedTarget[item.id];
                    // ensure the cached target
                    target || (target = xhr._cachedTarget[item.id] = cachedDocument.getElementById(item.id)),
                    // embed the target into the svg
                    embed(item.svg, target);
                });
            }
        }, // test the ready state change immediately
        xhr.onreadystatechange();
    }
    function svg4everybody(rawopts) {
        function oninterval() {
            // while the index exists in the live <use> collection
            for ( // get the cached <use> index
            var index = 0; index < uses.length;) {
                // get the current <use>
                var use = uses[index],
                    svg = use.parentNode;
                if (svg && /svg/i.test(svg.nodeName)) {
                    var src = use.getAttribute("xlink:href");
                    if (polyfill && (!opts.validate || opts.validate(src, svg, use))) {
                        // remove the <use> element
                        svg.removeChild(use);
                        // parse the src and get the url and id
                        var srcSplit = src.split("#"),
                            url = srcSplit.shift(),
                            id = srcSplit.join("#");
                        // if the link is external
                        if (url.length) {
                            // get the cached xhr request
                            var xhr = requests[url];
                            // ensure the xhr request exists
                            xhr || (xhr = requests[url] = new XMLHttpRequest(), xhr.open("GET", url), xhr.send(), xhr._embeds = []), // add the svg and id as an item to the xhr embeds list
                            xhr._embeds.push({
                                svg: svg,
                                id: id
                            }), // prepare the xhr ready state change event
                            loadreadystatechange(xhr);
                        } else {
                            // embed the local id into the svg
                            embed(svg, document.getElementById(id));
                        }
                    }
                } else {
                    // increase the index when the previous value was not "valid"
                    ++index;
                }
            }
            // continue the interval
            requestAnimationFrame(oninterval, 67);
        }
        var polyfill,
            opts = Object(rawopts),
            newerIEUA = /\bTrident\/[567]\b|\bMSIE (?:9|10)\.0\b/,
            webkitUA = /\bAppleWebKit\/(\d+)\b/,
            olderEdgeUA = /\bEdge\/12\.(\d+)\b/;
        polyfill = "polyfill" in opts ? opts.polyfill : newerIEUA.test(navigator.userAgent) || (navigator.userAgent.match(olderEdgeUA) || [])[1] < 10547 || (navigator.userAgent.match(webkitUA) || [])[1] < 537;
        // create xhr requests object
        var requests = {},
            requestAnimationFrame = window.requestAnimationFrame || setTimeout,
            uses = document.getElementsByTagName("use");
        // conditionally start the interval if the polyfill is active
        polyfill && oninterval();
    }
    return svg4everybody;
});

},{}],19:[function(require,module,exports){
'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
var toggleIcons = function toggleIcons(container) {

    if (!container) console.warn('toggleIcons missing container element');

    $(container).find('.toggleable-icon').each(function (indx, item) {
        $(item).hasClass('is-visible') ? $(item).removeClass('is-visible') : $(item).addClass('is-visible');
    });
};

exports.toggleIcons = toggleIcons;

},{}],20:[function(require,module,exports){
/* Zepto v1.1.6 - zepto event ajax form ie - zeptojs.com/license */
"use strict";

var Zepto = (function () {
  function L(t) {
    return null == t ? String(t) : j[S.call(t)] || "object";
  }function Z(t) {
    return "function" == L(t);
  }function _(t) {
    return null != t && t == t.window;
  }function $(t) {
    return null != t && t.nodeType == t.DOCUMENT_NODE;
  }function D(t) {
    return "object" == L(t);
  }function M(t) {
    return D(t) && !_(t) && Object.getPrototypeOf(t) == Object.prototype;
  }function R(t) {
    return "number" == typeof t.length;
  }function k(t) {
    return s.call(t, function (t) {
      return null != t;
    });
  }function z(t) {
    return t.length > 0 ? n.fn.concat.apply([], t) : t;
  }function F(t) {
    return t.replace(/::/g, "/").replace(/([A-Z]+)([A-Z][a-z])/g, "$1_$2").replace(/([a-z\d])([A-Z])/g, "$1_$2").replace(/_/g, "-").toLowerCase();
  }function q(t) {
    return t in f ? f[t] : f[t] = new RegExp("(^|\\s)" + t + "(\\s|$)");
  }function H(t, e) {
    return "number" != typeof e || c[F(t)] ? e : e + "px";
  }function I(t) {
    var e, n;return u[t] || (e = a.createElement(t), a.body.appendChild(e), n = getComputedStyle(e, "").getPropertyValue("display"), e.parentNode.removeChild(e), "none" == n && (n = "block"), u[t] = n), u[t];
  }function V(t) {
    return "children" in t ? o.call(t.children) : n.map(t.childNodes, function (t) {
      return 1 == t.nodeType ? t : void 0;
    });
  }function B(n, i, r) {
    for (e in i) r && (M(i[e]) || A(i[e])) ? (M(i[e]) && !M(n[e]) && (n[e] = {}), A(i[e]) && !A(n[e]) && (n[e] = []), B(n[e], i[e], r)) : i[e] !== t && (n[e] = i[e]);
  }function U(t, e) {
    return null == e ? n(t) : n(t).filter(e);
  }function J(t, e, n, i) {
    return Z(e) ? e.call(t, n, i) : e;
  }function X(t, e, n) {
    null == n ? t.removeAttribute(e) : t.setAttribute(e, n);
  }function W(e, n) {
    var i = e.className || "",
        r = i && i.baseVal !== t;return n === t ? r ? i.baseVal : i : void (r ? i.baseVal = n : e.className = n);
  }function Y(t) {
    try {
      return t ? "true" == t || ("false" == t ? !1 : "null" == t ? null : +t + "" == t ? +t : /^[\[\{]/.test(t) ? n.parseJSON(t) : t) : t;
    } catch (e) {
      return t;
    }
  }function G(t, e) {
    e(t);for (var n = 0, i = t.childNodes.length; i > n; n++) G(t.childNodes[n], e);
  }var t,
      e,
      n,
      i,
      C,
      N,
      r = [],
      o = r.slice,
      s = r.filter,
      a = window.document,
      u = {},
      f = {},
      c = { "column-count": 1, columns: 1, "font-weight": 1, "line-height": 1, opacity: 1, "z-index": 1, zoom: 1 },
      l = /^\s*<(\w+|!)[^>]*>/,
      h = /^<(\w+)\s*\/?>(?:<\/\1>|)$/,
      p = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/gi,
      d = /^(?:body|html)$/i,
      m = /([A-Z])/g,
      g = ["val", "css", "html", "text", "data", "width", "height", "offset"],
      v = ["after", "prepend", "before", "append"],
      y = a.createElement("table"),
      x = a.createElement("tr"),
      b = { tr: a.createElement("tbody"), tbody: y, thead: y, tfoot: y, td: x, th: x, "*": a.createElement("div") },
      w = /complete|loaded|interactive/,
      E = /^[\w-]*$/,
      j = {},
      S = j.toString,
      T = {},
      O = a.createElement("div"),
      P = { tabindex: "tabIndex", readonly: "readOnly", "for": "htmlFor", "class": "className", maxlength: "maxLength", cellspacing: "cellSpacing", cellpadding: "cellPadding", rowspan: "rowSpan", colspan: "colSpan", usemap: "useMap", frameborder: "frameBorder", contenteditable: "contentEditable" },
      A = Array.isArray || function (t) {
    return t instanceof Array;
  };return T.matches = function (t, e) {
    if (!e || !t || 1 !== t.nodeType) return !1;var n = t.webkitMatchesSelector || t.mozMatchesSelector || t.oMatchesSelector || t.matchesSelector;if (n) return n.call(t, e);var i,
        r = t.parentNode,
        o = !r;return o && (r = O).appendChild(t), i = ~T.qsa(r, e).indexOf(t), o && O.removeChild(t), i;
  }, C = function (t) {
    return t.replace(/-+(.)?/g, function (t, e) {
      return e ? e.toUpperCase() : "";
    });
  }, N = function (t) {
    return s.call(t, function (e, n) {
      return t.indexOf(e) == n;
    });
  }, T.fragment = function (e, i, r) {
    var s, u, f;return h.test(e) && (s = n(a.createElement(RegExp.$1))), s || (e.replace && (e = e.replace(p, "<$1></$2>")), i === t && (i = l.test(e) && RegExp.$1), i in b || (i = "*"), f = b[i], f.innerHTML = "" + e, s = n.each(o.call(f.childNodes), function () {
      f.removeChild(this);
    })), M(r) && (u = n(s), n.each(r, function (t, e) {
      g.indexOf(t) > -1 ? u[t](e) : u.attr(t, e);
    })), s;
  }, T.Z = function (t, e) {
    return t = t || [], t.__proto__ = n.fn, t.selector = e || "", t;
  }, T.isZ = function (t) {
    return t instanceof T.Z;
  }, T.init = function (e, i) {
    var r;if (!e) return T.Z();if ("string" == typeof e) if ((e = e.trim(), "<" == e[0] && l.test(e))) r = T.fragment(e, RegExp.$1, i), e = null;else {
      if (i !== t) return n(i).find(e);r = T.qsa(a, e);
    } else {
      if (Z(e)) return n(a).ready(e);if (T.isZ(e)) return e;if (A(e)) r = k(e);else if (D(e)) r = [e], e = null;else if (l.test(e)) r = T.fragment(e.trim(), RegExp.$1, i), e = null;else {
        if (i !== t) return n(i).find(e);r = T.qsa(a, e);
      }
    }return T.Z(r, e);
  }, n = function (t, e) {
    return T.init(t, e);
  }, n.extend = function (t) {
    var e,
        n = o.call(arguments, 1);return "boolean" == typeof t && (e = t, t = n.shift()), n.forEach(function (n) {
      B(t, n, e);
    }), t;
  }, T.qsa = function (t, e) {
    var n,
        i = "#" == e[0],
        r = !i && "." == e[0],
        s = i || r ? e.slice(1) : e,
        a = E.test(s);return $(t) && a && i ? (n = t.getElementById(s)) ? [n] : [] : 1 !== t.nodeType && 9 !== t.nodeType ? [] : o.call(a && !i ? r ? t.getElementsByClassName(s) : t.getElementsByTagName(e) : t.querySelectorAll(e));
  }, n.contains = a.documentElement.contains ? function (t, e) {
    return t !== e && t.contains(e);
  } : function (t, e) {
    for (; e && (e = e.parentNode);) if (e === t) return !0;return !1;
  }, n.type = L, n.isFunction = Z, n.isWindow = _, n.isArray = A, n.isPlainObject = M, n.isEmptyObject = function (t) {
    var e;for (e in t) return !1;return !0;
  }, n.inArray = function (t, e, n) {
    return r.indexOf.call(e, t, n);
  }, n.camelCase = C, n.trim = function (t) {
    return null == t ? "" : String.prototype.trim.call(t);
  }, n.uuid = 0, n.support = {}, n.expr = {}, n.map = function (t, e) {
    var n,
        r,
        o,
        i = [];if (R(t)) for (r = 0; r < t.length; r++) n = e(t[r], r), null != n && i.push(n);else for (o in t) n = e(t[o], o), null != n && i.push(n);return z(i);
  }, n.each = function (t, e) {
    var n, i;if (R(t)) {
      for (n = 0; n < t.length; n++) if (e.call(t[n], n, t[n]) === !1) return t;
    } else for (i in t) if (e.call(t[i], i, t[i]) === !1) return t;return t;
  }, n.grep = function (t, e) {
    return s.call(t, e);
  }, window.JSON && (n.parseJSON = JSON.parse), n.each("Boolean Number String Function Array Date RegExp Object Error".split(" "), function (t, e) {
    j["[object " + e + "]"] = e.toLowerCase();
  }), n.fn = { forEach: r.forEach, reduce: r.reduce, push: r.push, sort: r.sort, indexOf: r.indexOf, concat: r.concat, map: function map(t) {
      return n(n.map(this, function (e, n) {
        return t.call(e, n, e);
      }));
    }, slice: function slice() {
      return n(o.apply(this, arguments));
    }, ready: function ready(t) {
      return w.test(a.readyState) && a.body ? t(n) : a.addEventListener("DOMContentLoaded", function () {
        t(n);
      }, !1), this;
    }, get: function get(e) {
      return e === t ? o.call(this) : this[e >= 0 ? e : e + this.length];
    }, toArray: function toArray() {
      return this.get();
    }, size: function size() {
      return this.length;
    }, remove: function remove() {
      return this.each(function () {
        null != this.parentNode && this.parentNode.removeChild(this);
      });
    }, each: function each(t) {
      return r.every.call(this, function (e, n) {
        return t.call(e, n, e) !== !1;
      }), this;
    }, filter: function filter(t) {
      return Z(t) ? this.not(this.not(t)) : n(s.call(this, function (e) {
        return T.matches(e, t);
      }));
    }, add: function add(t, e) {
      return n(N(this.concat(n(t, e))));
    }, is: function is(t) {
      return this.length > 0 && T.matches(this[0], t);
    }, not: function not(e) {
      var i = [];if (Z(e) && e.call !== t) this.each(function (t) {
        e.call(this, t) || i.push(this);
      });else {
        var r = "string" == typeof e ? this.filter(e) : R(e) && Z(e.item) ? o.call(e) : n(e);this.forEach(function (t) {
          r.indexOf(t) < 0 && i.push(t);
        });
      }return n(i);
    }, has: function has(t) {
      return this.filter(function () {
        return D(t) ? n.contains(this, t) : n(this).find(t).size();
      });
    }, eq: function eq(t) {
      return -1 === t ? this.slice(t) : this.slice(t, +t + 1);
    }, first: function first() {
      var t = this[0];return t && !D(t) ? t : n(t);
    }, last: function last() {
      var t = this[this.length - 1];return t && !D(t) ? t : n(t);
    }, find: function find(t) {
      var e,
          i = this;return e = t ? "object" == typeof t ? n(t).filter(function () {
        var t = this;return r.some.call(i, function (e) {
          return n.contains(e, t);
        });
      }) : 1 == this.length ? n(T.qsa(this[0], t)) : this.map(function () {
        return T.qsa(this, t);
      }) : n();
    }, closest: function closest(t, e) {
      var i = this[0],
          r = !1;for ("object" == typeof t && (r = n(t)); i && !(r ? r.indexOf(i) >= 0 : T.matches(i, t));) i = i !== e && !$(i) && i.parentNode;return n(i);
    }, parents: function parents(t) {
      for (var e = [], i = this; i.length > 0;) i = n.map(i, function (t) {
        return (t = t.parentNode) && !$(t) && e.indexOf(t) < 0 ? (e.push(t), t) : void 0;
      });return U(e, t);
    }, parent: function parent(t) {
      return U(N(this.pluck("parentNode")), t);
    }, children: function children(t) {
      return U(this.map(function () {
        return V(this);
      }), t);
    }, contents: function contents() {
      return this.map(function () {
        return o.call(this.childNodes);
      });
    }, siblings: function siblings(t) {
      return U(this.map(function (t, e) {
        return s.call(V(e.parentNode), function (t) {
          return t !== e;
        });
      }), t);
    }, empty: function empty() {
      return this.each(function () {
        this.innerHTML = "";
      });
    }, pluck: function pluck(t) {
      return n.map(this, function (e) {
        return e[t];
      });
    }, show: function show() {
      return this.each(function () {
        "none" == this.style.display && (this.style.display = ""), "none" == getComputedStyle(this, "").getPropertyValue("display") && (this.style.display = I(this.nodeName));
      });
    }, replaceWith: function replaceWith(t) {
      return this.before(t).remove();
    }, wrap: function wrap(t) {
      var e = Z(t);if (this[0] && !e) var i = n(t).get(0),
          r = i.parentNode || this.length > 1;return this.each(function (o) {
        n(this).wrapAll(e ? t.call(this, o) : r ? i.cloneNode(!0) : i);
      });
    }, wrapAll: function wrapAll(t) {
      if (this[0]) {
        n(this[0]).before(t = n(t));for (var e; (e = t.children()).length;) t = e.first();n(t).append(this);
      }return this;
    }, wrapInner: function wrapInner(t) {
      var e = Z(t);return this.each(function (i) {
        var r = n(this),
            o = r.contents(),
            s = e ? t.call(this, i) : t;o.length ? o.wrapAll(s) : r.append(s);
      });
    }, unwrap: function unwrap() {
      return this.parent().each(function () {
        n(this).replaceWith(n(this).children());
      }), this;
    }, clone: function clone() {
      return this.map(function () {
        return this.cloneNode(!0);
      });
    }, hide: function hide() {
      return this.css("display", "none");
    }, toggle: function toggle(e) {
      return this.each(function () {
        var i = n(this);(e === t ? "none" == i.css("display") : e) ? i.show() : i.hide();
      });
    }, prev: function prev(t) {
      return n(this.pluck("previousElementSibling")).filter(t || "*");
    }, next: function next(t) {
      return n(this.pluck("nextElementSibling")).filter(t || "*");
    }, html: function html(t) {
      return 0 in arguments ? this.each(function (e) {
        var i = this.innerHTML;n(this).empty().append(J(this, t, e, i));
      }) : 0 in this ? this[0].innerHTML : null;
    }, text: function text(t) {
      return 0 in arguments ? this.each(function (e) {
        var n = J(this, t, e, this.textContent);this.textContent = null == n ? "" : "" + n;
      }) : 0 in this ? this[0].textContent : null;
    }, attr: function attr(n, i) {
      var r;return "string" != typeof n || 1 in arguments ? this.each(function (t) {
        if (1 === this.nodeType) if (D(n)) for (e in n) X(this, e, n[e]);else X(this, n, J(this, i, t, this.getAttribute(n)));
      }) : this.length && 1 === this[0].nodeType ? !(r = this[0].getAttribute(n)) && n in this[0] ? this[0][n] : r : t;
    }, removeAttr: function removeAttr(t) {
      return this.each(function () {
        1 === this.nodeType && t.split(" ").forEach(function (t) {
          X(this, t);
        }, this);
      });
    }, prop: function prop(t, e) {
      return t = P[t] || t, 1 in arguments ? this.each(function (n) {
        this[t] = J(this, e, n, this[t]);
      }) : this[0] && this[0][t];
    }, data: function data(e, n) {
      var i = "data-" + e.replace(m, "-$1").toLowerCase(),
          r = 1 in arguments ? this.attr(i, n) : this.attr(i);return null !== r ? Y(r) : t;
    }, val: function val(t) {
      return 0 in arguments ? this.each(function (e) {
        this.value = J(this, t, e, this.value);
      }) : this[0] && (this[0].multiple ? n(this[0]).find("option").filter(function () {
        return this.selected;
      }).pluck("value") : this[0].value);
    }, offset: function offset(t) {
      if (t) return this.each(function (e) {
        var i = n(this),
            r = J(this, t, e, i.offset()),
            o = i.offsetParent().offset(),
            s = { top: r.top - o.top, left: r.left - o.left };"static" == i.css("position") && (s.position = "relative"), i.css(s);
      });if (!this.length) return null;var e = this[0].getBoundingClientRect();return { left: e.left + window.pageXOffset, top: e.top + window.pageYOffset, width: Math.round(e.width), height: Math.round(e.height) };
    }, css: function css(t, i) {
      if (arguments.length < 2) {
        var r,
            o = this[0];if (!o) return;if ((r = getComputedStyle(o, ""), "string" == typeof t)) return o.style[C(t)] || r.getPropertyValue(t);if (A(t)) {
          var s = {};return n.each(t, function (t, e) {
            s[e] = o.style[C(e)] || r.getPropertyValue(e);
          }), s;
        }
      }var a = "";if ("string" == L(t)) i || 0 === i ? a = F(t) + ":" + H(t, i) : this.each(function () {
        this.style.removeProperty(F(t));
      });else for (e in t) t[e] || 0 === t[e] ? a += F(e) + ":" + H(e, t[e]) + ";" : this.each(function () {
        this.style.removeProperty(F(e));
      });return this.each(function () {
        this.style.cssText += ";" + a;
      });
    }, index: function index(t) {
      return t ? this.indexOf(n(t)[0]) : this.parent().children().indexOf(this[0]);
    }, hasClass: function hasClass(t) {
      return t ? r.some.call(this, function (t) {
        return this.test(W(t));
      }, q(t)) : !1;
    }, addClass: function addClass(t) {
      return t ? this.each(function (e) {
        if ("className" in this) {
          i = [];var r = W(this),
              o = J(this, t, e, r);o.split(/\s+/g).forEach(function (t) {
            n(this).hasClass(t) || i.push(t);
          }, this), i.length && W(this, r + (r ? " " : "") + i.join(" "));
        }
      }) : this;
    }, removeClass: function removeClass(e) {
      return this.each(function (n) {
        if ("className" in this) {
          if (e === t) return W(this, "");i = W(this), J(this, e, n, i).split(/\s+/g).forEach(function (t) {
            i = i.replace(q(t), " ");
          }), W(this, i.trim());
        }
      });
    }, toggleClass: function toggleClass(e, i) {
      return e ? this.each(function (r) {
        var o = n(this),
            s = J(this, e, r, W(this));s.split(/\s+/g).forEach(function (e) {
          (i === t ? !o.hasClass(e) : i) ? o.addClass(e) : o.removeClass(e);
        });
      }) : this;
    }, scrollTop: function scrollTop(e) {
      if (this.length) {
        var n = ("scrollTop" in this[0]);return e === t ? n ? this[0].scrollTop : this[0].pageYOffset : this.each(n ? function () {
          this.scrollTop = e;
        } : function () {
          this.scrollTo(this.scrollX, e);
        });
      }
    }, scrollLeft: function scrollLeft(e) {
      if (this.length) {
        var n = ("scrollLeft" in this[0]);return e === t ? n ? this[0].scrollLeft : this[0].pageXOffset : this.each(n ? function () {
          this.scrollLeft = e;
        } : function () {
          this.scrollTo(e, this.scrollY);
        });
      }
    }, position: function position() {
      if (this.length) {
        var t = this[0],
            e = this.offsetParent(),
            i = this.offset(),
            r = d.test(e[0].nodeName) ? { top: 0, left: 0 } : e.offset();return i.top -= parseFloat(n(t).css("margin-top")) || 0, i.left -= parseFloat(n(t).css("margin-left")) || 0, r.top += parseFloat(n(e[0]).css("border-top-width")) || 0, r.left += parseFloat(n(e[0]).css("border-left-width")) || 0, { top: i.top - r.top, left: i.left - r.left };
      }
    }, offsetParent: function offsetParent() {
      return this.map(function () {
        for (var t = this.offsetParent || a.body; t && !d.test(t.nodeName) && "static" == n(t).css("position");) t = t.offsetParent;return t;
      });
    } }, n.fn.detach = n.fn.remove, ["width", "height"].forEach(function (e) {
    var i = e.replace(/./, function (t) {
      return t[0].toUpperCase();
    });n.fn[e] = function (r) {
      var o,
          s = this[0];return r === t ? _(s) ? s["inner" + i] : $(s) ? s.documentElement["scroll" + i] : (o = this.offset()) && o[e] : this.each(function (t) {
        s = n(this), s.css(e, J(this, r, t, s[e]()));
      });
    };
  }), v.forEach(function (t, e) {
    var i = e % 2;n.fn[t] = function () {
      var t,
          o,
          r = n.map(arguments, function (e) {
        return t = L(e), "object" == t || "array" == t || null == e ? e : T.fragment(e);
      }),
          s = this.length > 1;return r.length < 1 ? this : this.each(function (t, u) {
        o = i ? u : u.parentNode, u = 0 == e ? u.nextSibling : 1 == e ? u.firstChild : 2 == e ? u : null;var f = n.contains(a.documentElement, o);r.forEach(function (t) {
          if (s) t = t.cloneNode(!0);else if (!o) return n(t).remove();o.insertBefore(t, u), f && G(t, function (t) {
            null == t.nodeName || "SCRIPT" !== t.nodeName.toUpperCase() || t.type && "text/javascript" !== t.type || t.src || window.eval.call(window, t.innerHTML);
          });
        });
      });
    }, n.fn[i ? t + "To" : "insert" + (e ? "Before" : "After")] = function (e) {
      return n(e)[t](this), this;
    };
  }), T.Z.prototype = n.fn, T.uniq = N, T.deserializeValue = Y, n.zepto = T, n;
})();window.Zepto = Zepto, void 0 === window.$ && (window.$ = Zepto), (function (t) {
  function l(t) {
    return t._zid || (t._zid = e++);
  }function h(t, e, n, i) {
    if ((e = p(e), e.ns)) var r = d(e.ns);return (s[l(t)] || []).filter(function (t) {
      return !(!t || e.e && t.e != e.e || e.ns && !r.test(t.ns) || n && l(t.fn) !== l(n) || i && t.sel != i);
    });
  }function p(t) {
    var e = ("" + t).split(".");return { e: e[0], ns: e.slice(1).sort().join(" ") };
  }function d(t) {
    return new RegExp("(?:^| )" + t.replace(" ", " .* ?") + "(?: |$)");
  }function m(t, e) {
    return t.del && !u && t.e in f || !!e;
  }function g(t) {
    return c[t] || u && f[t] || t;
  }function v(e, i, r, o, a, u, f) {
    var h = l(e),
        d = s[h] || (s[h] = []);i.split(/\s/).forEach(function (i) {
      if ("ready" == i) return t(document).ready(r);var s = p(i);s.fn = r, s.sel = a, s.e in c && (r = function (e) {
        var n = e.relatedTarget;return !n || n !== this && !t.contains(this, n) ? s.fn.apply(this, arguments) : void 0;
      }), s.del = u;var l = u || r;s.proxy = function (t) {
        if ((t = j(t), !t.isImmediatePropagationStopped())) {
          t.data = o;var i = l.apply(e, t._args == n ? [t] : [t].concat(t._args));return i === !1 && (t.preventDefault(), t.stopPropagation()), i;
        }
      }, s.i = d.length, d.push(s), "addEventListener" in e && e.addEventListener(g(s.e), s.proxy, m(s, f));
    });
  }function y(t, e, n, i, r) {
    var o = l(t);(e || "").split(/\s/).forEach(function (e) {
      h(t, e, n, i).forEach(function (e) {
        delete s[o][e.i], "removeEventListener" in t && t.removeEventListener(g(e.e), e.proxy, m(e, r));
      });
    });
  }function j(e, i) {
    return (i || !e.isDefaultPrevented) && (i || (i = e), t.each(E, function (t, n) {
      var r = i[t];e[t] = function () {
        return this[n] = x, r && r.apply(i, arguments);
      }, e[n] = b;
    }), (i.defaultPrevented !== n ? i.defaultPrevented : "returnValue" in i ? i.returnValue === !1 : i.getPreventDefault && i.getPreventDefault()) && (e.isDefaultPrevented = x)), e;
  }function S(t) {
    var e,
        i = { originalEvent: t };for (e in t) w.test(e) || t[e] === n || (i[e] = t[e]);return j(i, t);
  }var n,
      e = 1,
      i = Array.prototype.slice,
      r = t.isFunction,
      o = function o(t) {
    return "string" == typeof t;
  },
      s = {},
      a = {},
      u = ("onfocusin" in window),
      f = { focus: "focusin", blur: "focusout" },
      c = { mouseenter: "mouseover", mouseleave: "mouseout" };a.click = a.mousedown = a.mouseup = a.mousemove = "MouseEvents", t.event = { add: v, remove: y }, t.proxy = function (e, n) {
    var s = 2 in arguments && i.call(arguments, 2);if (r(e)) {
      var a = function a() {
        return e.apply(n, s ? s.concat(i.call(arguments)) : arguments);
      };return a._zid = l(e), a;
    }if (o(n)) return s ? (s.unshift(e[n], e), t.proxy.apply(null, s)) : t.proxy(e[n], e);throw new TypeError("expected function");
  }, t.fn.bind = function (t, e, n) {
    return this.on(t, e, n);
  }, t.fn.unbind = function (t, e) {
    return this.off(t, e);
  }, t.fn.one = function (t, e, n, i) {
    return this.on(t, e, n, i, 1);
  };var x = function x() {
    return !0;
  },
      b = function b() {
    return !1;
  },
      w = /^([A-Z]|returnValue$|layer[XY]$)/,
      E = { preventDefault: "isDefaultPrevented", stopImmediatePropagation: "isImmediatePropagationStopped", stopPropagation: "isPropagationStopped" };t.fn.delegate = function (t, e, n) {
    return this.on(e, t, n);
  }, t.fn.undelegate = function (t, e, n) {
    return this.off(e, t, n);
  }, t.fn.live = function (e, n) {
    return t(document.body).delegate(this.selector, e, n), this;
  }, t.fn.die = function (e, n) {
    return t(document.body).undelegate(this.selector, e, n), this;
  }, t.fn.on = function (e, s, a, u, f) {
    var c,
        l,
        h = this;return e && !o(e) ? (t.each(e, function (t, e) {
      h.on(t, s, a, e, f);
    }), h) : (o(s) || r(u) || u === !1 || (u = a, a = s, s = n), (r(a) || a === !1) && (u = a, a = n), u === !1 && (u = b), h.each(function (n, r) {
      f && (c = function (t) {
        return y(r, t.type, u), u.apply(this, arguments);
      }), s && (l = function (e) {
        var n,
            o = t(e.target).closest(s, r).get(0);return o && o !== r ? (n = t.extend(S(e), { currentTarget: o, liveFired: r }), (c || u).apply(o, [n].concat(i.call(arguments, 1)))) : void 0;
      }), v(r, e, u, a, s, l || c);
    }));
  }, t.fn.off = function (e, i, s) {
    var a = this;return e && !o(e) ? (t.each(e, function (t, e) {
      a.off(t, i, e);
    }), a) : (o(i) || r(s) || s === !1 || (s = i, i = n), s === !1 && (s = b), a.each(function () {
      y(this, e, s, i);
    }));
  }, t.fn.trigger = function (e, n) {
    return e = o(e) || t.isPlainObject(e) ? t.Event(e) : j(e), e._args = n, this.each(function () {
      e.type in f && "function" == typeof this[e.type] ? this[e.type]() : "dispatchEvent" in this ? this.dispatchEvent(e) : t(this).triggerHandler(e, n);
    });
  }, t.fn.triggerHandler = function (e, n) {
    var i, r;return this.each(function (s, a) {
      i = S(o(e) ? t.Event(e) : e), i._args = n, i.target = a, t.each(h(a, e.type || e), function (t, e) {
        return r = e.proxy(i), i.isImmediatePropagationStopped() ? !1 : void 0;
      });
    }), r;
  }, "focusin focusout focus blur load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select keydown keypress keyup error".split(" ").forEach(function (e) {
    t.fn[e] = function (t) {
      return 0 in arguments ? this.bind(e, t) : this.trigger(e);
    };
  }), t.Event = function (t, e) {
    o(t) || (e = t, t = e.type);var n = document.createEvent(a[t] || "Events"),
        i = !0;if (e) for (var r in e) "bubbles" == r ? i = !!e[r] : n[r] = e[r];return n.initEvent(t, i, !0), j(n);
  };
})(Zepto), (function (t) {
  function h(e, n, i) {
    var r = t.Event(n);return t(e).trigger(r, i), !r.isDefaultPrevented();
  }function p(t, e, i, r) {
    return t.global ? h(e || n, i, r) : void 0;
  }function d(e) {
    e.global && 0 === t.active++ && p(e, null, "ajaxStart");
  }function m(e) {
    e.global && ! --t.active && p(e, null, "ajaxStop");
  }function g(t, e) {
    var n = e.context;return e.beforeSend.call(n, t, e) === !1 || p(e, n, "ajaxBeforeSend", [t, e]) === !1 ? !1 : void p(e, n, "ajaxSend", [t, e]);
  }function v(t, e, n, i) {
    var r = n.context,
        o = "success";n.success.call(r, t, o, e), i && i.resolveWith(r, [t, o, e]), p(n, r, "ajaxSuccess", [e, n, t]), x(o, e, n);
  }function y(t, e, n, i, r) {
    var o = i.context;i.error.call(o, n, e, t), r && r.rejectWith(o, [n, e, t]), p(i, o, "ajaxError", [n, i, t || e]), x(e, n, i);
  }function x(t, e, n) {
    var i = n.context;n.complete.call(i, e, t), p(n, i, "ajaxComplete", [e, n]), m(n);
  }function b() {}function w(t) {
    return t && (t = t.split(";", 2)[0]), t && (t == f ? "html" : t == u ? "json" : s.test(t) ? "script" : a.test(t) && "xml") || "text";
  }function E(t, e) {
    return "" == e ? t : (t + "&" + e).replace(/[&?]{1,2}/, "?");
  }function j(e) {
    e.processData && e.data && "string" != t.type(e.data) && (e.data = t.param(e.data, e.traditional)), !e.data || e.type && "GET" != e.type.toUpperCase() || (e.url = E(e.url, e.data), e.data = void 0);
  }function S(e, n, i, r) {
    return t.isFunction(n) && (r = i, i = n, n = void 0), t.isFunction(i) || (r = i, i = void 0), { url: e, data: n, success: i, dataType: r };
  }function C(e, n, i, r) {
    var o,
        s = t.isArray(n),
        a = t.isPlainObject(n);t.each(n, function (n, u) {
      o = t.type(u), r && (n = i ? r : r + "[" + (a || "object" == o || "array" == o ? n : "") + "]"), !r && s ? e.add(u.name, u.value) : "array" == o || !i && "object" == o ? C(e, u, i, n) : e.add(n, u);
    });
  }var i,
      r,
      e = 0,
      n = window.document,
      o = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,
      s = /^(?:text|application)\/javascript/i,
      a = /^(?:text|application)\/xml/i,
      u = "application/json",
      f = "text/html",
      c = /^\s*$/,
      l = n.createElement("a");l.href = window.location.href, t.active = 0, t.ajaxJSONP = function (i, r) {
    if (!("type" in i)) return t.ajax(i);var f,
        h,
        o = i.jsonpCallback,
        s = (t.isFunction(o) ? o() : o) || "jsonp" + ++e,
        a = n.createElement("script"),
        u = window[s],
        c = function c(e) {
      t(a).triggerHandler("error", e || "abort");
    },
        l = { abort: c };return r && r.promise(l), t(a).on("load error", function (e, n) {
      clearTimeout(h), t(a).off().remove(), "error" != e.type && f ? v(f[0], l, i, r) : y(null, n || "error", l, i, r), window[s] = u, f && t.isFunction(u) && u(f[0]), u = f = void 0;
    }), g(l, i) === !1 ? (c("abort"), l) : (window[s] = function () {
      f = arguments;
    }, a.src = i.url.replace(/\?(.+)=\?/, "?$1=" + s), n.head.appendChild(a), i.timeout > 0 && (h = setTimeout(function () {
      c("timeout");
    }, i.timeout)), l);
  }, t.ajaxSettings = { type: "GET", beforeSend: b, success: b, error: b, complete: b, context: null, global: !0, xhr: function xhr() {
      return new window.XMLHttpRequest();
    }, accepts: { script: "text/javascript, application/javascript, application/x-javascript", json: u, xml: "application/xml, text/xml", html: f, text: "text/plain" }, crossDomain: !1, timeout: 0, processData: !0, cache: !0 }, t.ajax = function (e) {
    var a,
        o = t.extend({}, e || {}),
        s = t.Deferred && t.Deferred();for (i in t.ajaxSettings) void 0 === o[i] && (o[i] = t.ajaxSettings[i]);d(o), o.crossDomain || (a = n.createElement("a"), a.href = o.url, a.href = a.href, o.crossDomain = l.protocol + "//" + l.host != a.protocol + "//" + a.host), o.url || (o.url = window.location.toString()), j(o);var u = o.dataType,
        f = /\?.+=\?/.test(o.url);if ((f && (u = "jsonp"), o.cache !== !1 && (e && e.cache === !0 || "script" != u && "jsonp" != u) || (o.url = E(o.url, "_=" + Date.now())), "jsonp" == u)) return f || (o.url = E(o.url, o.jsonp ? o.jsonp + "=?" : o.jsonp === !1 ? "" : "callback=?")), t.ajaxJSONP(o, s);var C,
        h = o.accepts[u],
        p = {},
        m = function m(t, e) {
      p[t.toLowerCase()] = [t, e];
    },
        x = /^([\w-]+:)\/\//.test(o.url) ? RegExp.$1 : window.location.protocol,
        S = o.xhr(),
        T = S.setRequestHeader;if ((s && s.promise(S), o.crossDomain || m("X-Requested-With", "XMLHttpRequest"), m("Accept", h || "*/*"), (h = o.mimeType || h) && (h.indexOf(",") > -1 && (h = h.split(",", 2)[0]), S.overrideMimeType && S.overrideMimeType(h)), (o.contentType || o.contentType !== !1 && o.data && "GET" != o.type.toUpperCase()) && m("Content-Type", o.contentType || "application/x-www-form-urlencoded"), o.headers)) for (r in o.headers) m(r, o.headers[r]);if ((S.setRequestHeader = m, S.onreadystatechange = function () {
      if (4 == S.readyState) {
        S.onreadystatechange = b, clearTimeout(C);var e,
            n = !1;if (S.status >= 200 && S.status < 300 || 304 == S.status || 0 == S.status && "file:" == x) {
          u = u || w(o.mimeType || S.getResponseHeader("content-type")), e = S.responseText;try {
            "script" == u ? (1, eval)(e) : "xml" == u ? e = S.responseXML : "json" == u && (e = c.test(e) ? null : t.parseJSON(e));
          } catch (i) {
            n = i;
          }n ? y(n, "parsererror", S, o, s) : v(e, S, o, s);
        } else y(S.statusText || null, S.status ? "error" : "abort", S, o, s);
      }
    }, g(S, o) === !1)) return S.abort(), y(null, "abort", S, o, s), S;if (o.xhrFields) for (r in o.xhrFields) S[r] = o.xhrFields[r];var N = "async" in o ? o.async : !0;S.open(o.type, o.url, N, o.username, o.password);for (r in p) T.apply(S, p[r]);return o.timeout > 0 && (C = setTimeout(function () {
      S.onreadystatechange = b, S.abort(), y(null, "timeout", S, o, s);
    }, o.timeout)), S.send(o.data ? o.data : null), S;
  }, t.get = function () {
    return t.ajax(S.apply(null, arguments));
  }, t.post = function () {
    var e = S.apply(null, arguments);return e.type = "POST", t.ajax(e);
  }, t.getJSON = function () {
    var e = S.apply(null, arguments);return e.dataType = "json", t.ajax(e);
  }, t.fn.load = function (e, n, i) {
    if (!this.length) return this;var a,
        r = this,
        s = e.split(/\s/),
        u = S(e, n, i),
        f = u.success;return s.length > 1 && (u.url = s[0], a = s[1]), u.success = function (e) {
      r.html(a ? t("<div>").html(e.replace(o, "")).find(a) : e), f && f.apply(r, arguments);
    }, t.ajax(u), this;
  };var T = encodeURIComponent;t.param = function (e, n) {
    var i = [];return i.add = function (e, n) {
      t.isFunction(n) && (n = n()), null == n && (n = ""), this.push(T(e) + "=" + T(n));
    }, C(i, e, n), i.join("&").replace(/%20/g, "+");
  };
})(Zepto), (function (t) {
  t.fn.serializeArray = function () {
    var e,
        n,
        i = [],
        r = function r(t) {
      return t.forEach ? t.forEach(r) : void i.push({ name: e, value: t });
    };return this[0] && t.each(this[0].elements, function (i, o) {
      n = o.type, e = o.name, e && "fieldset" != o.nodeName.toLowerCase() && !o.disabled && "submit" != n && "reset" != n && "button" != n && "file" != n && ("radio" != n && "checkbox" != n || o.checked) && r(t(o).val());
    }), i;
  }, t.fn.serialize = function () {
    var t = [];return this.serializeArray().forEach(function (e) {
      t.push(encodeURIComponent(e.name) + "=" + encodeURIComponent(e.value));
    }), t.join("&");
  }, t.fn.submit = function (e) {
    if (0 in arguments) this.bind("submit", e);else if (this.length) {
      var n = t.Event("submit");this.eq(0).trigger(n), n.isDefaultPrevented() || this.get(0).submit();
    }return this;
  };
})(Zepto), (function (t) {
  "__proto__" in {} || t.extend(t.zepto, { Z: function Z(e, n) {
      return e = e || [], t.extend(e, t.fn), e.selector = n || "", e.__Z = !0, e;
    }, isZ: function isZ(e) {
      return "array" === t.type(e) && "__Z" in e;
    } });try {
    getComputedStyle(void 0);
  } catch (e) {
    var n = getComputedStyle;window.getComputedStyle = function (t) {
      try {
        return n(t);
      } catch (e) {
        return null;
      }
    };
  }
})(Zepto);

//     Zepto.js
//     (c) 2010-2016 Thomas Fuchs
//     Zepto.js may be freely distributed under the MIT license.
//
//     https://github.com/madrobby/zepto/blob/master/src/selector.js

!(function (t) {
  function n(n) {
    return n = t(n), !(!n.width() && !n.height()) && "none" !== n.css("display");
  }function e(t, n) {
    t = t.replace(/=#\]/g, '="#"]');var e,
        i,
        r = u.exec(t);if (r && r[2] in s && (e = s[r[2]], i = r[3], t = r[1], i)) {
      var o = Number(i);i = isNaN(o) ? i.replace(/^["']|["']$/g, "") : o;
    }return n(t, e, i);
  }var i = t.zepto,
      r = i.qsa,
      o = i.matches,
      s = t.expr[":"] = { visible: function visible() {
      return n(this) ? this : void 0;
    }, hidden: function hidden() {
      return n(this) ? void 0 : this;
    }, selected: function selected() {
      return this.selected ? this : void 0;
    }, checked: function checked() {
      return this.checked ? this : void 0;
    }, parent: function parent() {
      return this.parentNode;
    }, first: function first(t) {
      return 0 === t ? this : void 0;
    }, last: function last(t, n) {
      return t === n.length - 1 ? this : void 0;
    }, eq: function eq(t, n, e) {
      return t === e ? this : void 0;
    }, contains: function contains(n, e, i) {
      return t(this).text().indexOf(i) > -1 ? this : void 0;
    }, has: function has(t, n, e) {
      return i.qsa(this, e).length ? this : void 0;
    } },
      u = new RegExp("(.*):(\\w+)(?:\\(([^)]+)\\))?$\\s*"),
      c = /^\s*>/,
      h = "Zepto" + +new Date();i.qsa = function (n, o) {
    return e(o, function (e, s, u) {
      try {
        var a;!e && s ? e = "*" : c.test(e) && (a = t(n).addClass(h), e = "." + h + " " + e);var f = r(n, e);
      } catch (d) {
        throw (console.error("error performing selector: %o", o), d);
      } finally {
        a && a.removeClass(h);
      }return s ? i.uniq(t.map(f, function (t, n) {
        return s.call(t, n, f, u);
      })) : f;
    });
  }, i.matches = function (t, n) {
    return e(n, function (n, e, i) {
      return (!n || o(t, n)) && (!e || e.call(t, null, i) === t);
    });
  };
})(Zepto);

},{}]},{},[13])

//# sourceMappingURL=data:application/json;charset:utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJEOi9JbnNpZ2h0cy9JbnNpZ2h0cy0wNi4wMy4yMDE3L3dlYi9XZWJzaXRlL2pzL2NhbGN1bGF0ZVBvcHVwT2Zmc2V0cy5qcyIsIkQ6L0luc2lnaHRzL0luc2lnaHRzLTA2LjAzLjIwMTcvd2ViL1dlYnNpdGUvanMvY29tcG9uZW50cy9hcnRpY2xlLXNpZGViYXItY29tcG9uZW50LmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy9jb21wb25lbnRzL3NhdmUtc2VhcmNoLWNvbXBvbmVudC5qcyIsIkQ6L0luc2lnaHRzL0luc2lnaHRzLTA2LjAzLjIwMTcvd2ViL1dlYnNpdGUvanMvY29udHJvbGxlcnMvYW5hbHl0aWNzLWNvbnRyb2xsZXIuanMiLCJEOi9JbnNpZ2h0cy9JbnNpZ2h0cy0wNi4wMy4yMDE3L3dlYi9XZWJzaXRlL2pzL2NvbnRyb2xsZXJzL2Jvb2ttYXJrLWNvbnRyb2xsZXIuanMiLCJEOi9JbnNpZ2h0cy9JbnNpZ2h0cy0wNi4wMy4yMDE3L3dlYi9XZWJzaXRlL2pzL2NvbnRyb2xsZXJzL2Zvcm0tY29udHJvbGxlci5qcyIsIkQ6L0luc2lnaHRzL0luc2lnaHRzLTA2LjAzLjIwMTcvd2ViL1dlYnNpdGUvanMvY29udHJvbGxlcnMvbGlnaHRib3gtbW9kYWwtY29udHJvbGxlci5qcyIsIkQ6L0luc2lnaHRzL0luc2lnaHRzLTA2LjAzLjIwMTcvd2ViL1dlYnNpdGUvanMvY29udHJvbGxlcnMvcG9wLW91dC1jb250cm9sbGVyLmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy9jb250cm9sbGVycy9yZWdpc3Rlci1jb250cm9sbGVyLmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy9jb250cm9sbGVycy9yZXNldC1wYXNzd29yZC1jb250cm9sbGVyLmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy9jb250cm9sbGVycy9zb3J0YWJsZS10YWJsZS1jb250cm9sbGVyLmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy9jb250cm9sbGVycy90b29sdGlwLWNvbnRyb2xsZXIuanMiLCJEOi9JbnNpZ2h0cy9JbnNpZ2h0cy0wNi4wMy4yMDE3L3dlYi9XZWJzaXRlL2pzL2luZGV4LmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy9qc2Nvb2tpZS5qcyIsIkQ6L0luc2lnaHRzL0luc2lnaHRzLTA2LjAzLjIwMTcvd2ViL1dlYnNpdGUvanMvbmV3c2xldHRlci1zaWdudXAuanMiLCJEOi9JbnNpZ2h0cy9JbnNpZ2h0cy0wNi4wMy4yMDE3L3dlYi9XZWJzaXRlL2pzL3NlYXJjaC1wYWdlLmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy9zZWxlY3Rpdml0eS1mdWxsLmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy9zdmc0ZXZlcnlib2R5LmpzIiwiRDovSW5zaWdodHMvSW5zaWdodHMtMDYuMDMuMjAxNy93ZWIvV2Vic2l0ZS9qcy90b2dnbGUtaWNvbnMuanMiLCJEOi9JbnNpZ2h0cy9JbnNpZ2h0cy0wNi4wMy4yMDE3L3dlYi9XZWJzaXRlL2pzL3plcHRvLm1pbi5qcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7QUNvQkEsU0FBUyxxQkFBcUIsQ0FBQyxJQUF3RixFQUFDO1FBQXhGLEtBQUssR0FBTixJQUF3RixDQUF2RixLQUFLO1FBQUUsR0FBRyxHQUFYLElBQXdGLENBQWhGLEdBQUc7UUFBRSxJQUFJLEdBQWpCLElBQXdGLENBQTNFLElBQUk7c0JBQWpCLElBQXdGLENBQXJF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QUMsR0FBRyxDQUFDLElBQUksR0FBRyxRQUFRLEdBQUcsQ0FBQyxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxLQUFLLEdBQUcsTUFBTSxDQUFDLE1BQU0sRUFBRSxDQUFDLEtBQUssSUFBSSxJQUFJLEdBQUcsQ0FBQyxDQUFBLEFBQUMsQ0FBQyxDQUFDOztBQUUzSCxNQUFHLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxJQUFJLEdBQUcsTUFBTSxDQUFDO0dBQzdCLE1BQU07Ozs7OztBQU1OLE1BQUcsQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLElBQUksR0FBRyxRQUFRLEdBQUcsQ0FBQyxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLEdBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxFQUFFLENBQUMsS0FBSyxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsS0FBSyxDQUFBLEdBQUksQ0FBQyxBQUFDLENBQUMsQ0FBQzs7O0FBR3RILE1BQUcsQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLElBQUksR0FBRyxRQUFRLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLElBQUksR0FBRyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUM7R0FDekU7OztBQUdELE1BQUksQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLFNBQVMsRUFBRSxNQUFNLENBQUMsQ0FBQzs7OztBQUk5QixLQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUcsSUFBSSxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsU0FBUyxDQUFDLEdBQUcsQ0FBQyxDQUFDOzs7OztBQUsvQyxNQUFJLEVBQUUsR0FBRyxJQUFJLENBQUMsU0FBUyxHQUFHLElBQUksQ0FBQyxTQUFTLENBQUMsU0FBUyxHQUFHLFNBQVMsQ0FBQzs7O0FBRy9ELE1BQUcsRUFBRSxJQUFJLENBQUMsUUFBUSxJQUFJLENBQUMsUUFBUSxFQUFFOztBQUVoQyxNQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUcsRUFBRSxDQUFDLFVBQVUsSUFBSSxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sR0FBSSxJQUFJLEdBQUcsQ0FBQyxBQUFDLEdBQUcsSUFBSSxDQUFDOztBQUU3RSxLQUFFLENBQUMsVUFBVSxHQUNWLEdBQUcsQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxFQUFFLENBQUMsVUFBVSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxHQUFJLElBQUksR0FBRyxDQUFDLEFBQUMsR0FDckUsSUFBSSxDQUFDOztBQUVSLE1BQUcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsR0FBRyxFQUFFLENBQUMsVUFBVSxHQUM1QixHQUFHLElBQUksRUFBRSxDQUFDLFVBQVUsR0FBRyxDQUFDLENBQUEsQUFBQyxHQUFHLElBQUksR0FDaEMsR0FBRyxJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxHQUFJLElBQUksR0FBRyxDQUFDLEFBQUMsR0FBRyxDQUFDLENBQUEsQUFBQyxHQUFHLElBQUksQ0FBQzs7O0dBR3RELE1BQU0sSUFBRyxFQUFFLElBQUksQ0FBQyxRQUFRLElBQUksUUFBUSxFQUFFOztBQUV0QyxPQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUcsRUFBRSxDQUFDLFlBQVksSUFBSSxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sR0FBSSxJQUFJLEdBQUcsQ0FBQyxBQUFDLEdBQUcsSUFBSSxDQUFDOztBQUUvRSxNQUFFLENBQUMsWUFBWSxHQUNaLEdBQUcsQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxFQUFFLENBQUMsWUFBWSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxHQUFJLElBQUksR0FBRyxDQUFDLEFBQUMsR0FDdkUsSUFBSSxDQUFDOztBQUVSLE9BQUcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsR0FBRyxFQUFFLENBQUMsWUFBWSxHQUM5QixHQUFHLElBQUksRUFBRSxDQUFDLFlBQVksR0FBRyxDQUFDLENBQUEsQUFBQyxHQUFHLElBQUksR0FDbEMsR0FBRyxJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxHQUFJLElBQUksR0FBRyxDQUFDLEFBQUMsR0FBRyxDQUFDLENBQUEsQUFBQyxHQUFHLElBQUksQ0FBQzs7O0lBR3RELE1BQU0sSUFBRyxFQUFFLElBQUksUUFBUSxFQUFFOztBQUV6QixRQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxNQUFNLEdBQUcsRUFBRSxDQUFDLFdBQVcsSUFBSSxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sR0FBSSxJQUFJLEdBQUcsQ0FBQyxBQUFDLEdBQUcsSUFBSSxDQUFDOztBQUU5RSxPQUFFLENBQUMsV0FBVyxHQUNYLEdBQUcsQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLEdBQUcsSUFBSSxFQUFFLENBQUMsV0FBVyxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxHQUFJLElBQUksR0FBRyxDQUFDLEFBQUMsR0FDdEUsSUFBSSxDQUFDOztBQUVSLFFBQUcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsR0FBRyxFQUFFLENBQUMsV0FBVyxHQUM3QixHQUFHLElBQUksRUFBRSxDQUFDLFdBQVcsR0FBRyxDQUFDLENBQUEsQUFBQyxHQUFHLElBQUksR0FDakMsR0FBRyxJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxHQUFJLElBQUksR0FBRyxDQUFDLEFBQUMsR0FBRyxDQUFDLENBQUEsQUFBQyxHQUFHLElBQUksQ0FBQzs7O0tBR3RELE1BQU07O0FBRU4sU0FBRyxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsTUFBTSxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxHQUFJLElBQUksR0FBRyxDQUFDLEFBQUMsR0FBRyxJQUFJLENBQUM7Ozs7QUFJNUQsU0FBRyxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsR0FBRyxHQUFHLEdBQUcsSUFBSSxJQUFJLENBQUMsTUFBTSxDQUFDLE1BQU0sR0FBSSxJQUFJLEdBQUcsQ0FBQyxBQUFDLEdBQUcsQ0FBQyxDQUFBLEFBQUMsR0FBRyxJQUFJLENBQUM7TUFDckU7OztBQUdELEtBQUcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUMsTUFBTSxDQUFDLEtBQUssR0FBSSxJQUFJLEdBQUcsQ0FBQyxBQUFDLEdBQUcsSUFBSSxDQUFDOzs7QUFHMUQsS0FBRyxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsTUFBTSxHQUFHLElBQUksQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLFNBQVMsQ0FBQyxHQUFHLENBQUMsQ0FBQzs7O0FBRy9DLEtBQUcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLFNBQVMsR0FBRyxjQUFjLEdBQUcsR0FBRyxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsSUFBSSxHQUFFLE1BQU0sR0FBRyxHQUFHLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxHQUFHLEdBQUcsUUFBUSxDQUFDOzs7QUFHckcsUUFBTSxDQUFDLEdBQUcsQ0FBQztBQUNWLFNBQU0sRUFBRSxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxNQUFNO0FBQzFCLFlBQVMsRUFBRSxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxTQUFTO0dBQ2hDLENBQUMsQ0FBQyxJQUFJLENBQUMsZUFBZSxDQUFDLENBQUMsR0FBRyxDQUFDO0FBQzVCLFNBQU0sRUFBRSxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxNQUFNO0FBQzFCLFFBQUssRUFBRSxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxLQUFLO0FBQ3hCLE9BQUksRUFBRSxHQUFHLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxJQUFJO0FBQ3pCLFFBQUssRUFBRSxDQUFDO0FBQ1IsTUFBRyxFQUFFLEdBQUcsQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLEdBQUc7QUFDcEIsU0FBTSxFQUFF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QUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsT0FBTyxPQUFPLElBQUUsVUFBVSxJQUFFLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxJQUFJLEtBQUssQ0FBQyxzQkFBc0IsR0FBQyxDQUFDLEdBQUMsR0FBRyxDQUFDLENBQUMsT0FBTSxDQUFDLENBQUMsSUFBSSxHQUFDLGtCQUFrQixDQUFDLENBQUMsQ0FBQSxDQUFBLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFBLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQSxDQUFDLElBQUksQ0FBQyxDQUFDLE9BQU8sT0FBTyxJQUFFLFVBQVUsSUFBRSxPQUFPLENBQUMsSUFBSSxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxFQUFFLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFBLENBQUMsQ0FBQSxDQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsU0FBUyxPQUFPLENBQUMsTUFBTSxDQUFDLE9BQU8sQ0FBQyxDQUN4eUIsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxPQUFPLENBQUMsRUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxPQUFPLENBQUMsRUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxPQUFPLENBQUMsRUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxNQUFNLENBQUMsT0FBTyxHQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUM3TCxDQUFDLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLENBQUMsRUFBRSxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLENBQUMsRUFBRSxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxJQUFJLENBQUMsRUFBRSxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMs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QUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLElBQUksQ0FDZCxHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxJQUFJLENBQ2QsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsSUFBSSxDQUNkLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdBQUcsQ0FDYixHQUFRLENBQUUsR0FBRyxDQUNiLEdBQVEsQ0FBRSxHQUFHLENBQ2IsR0FBUSxDQUFFLEdB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QyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxRQUFRLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUM7QUFBQyxXQUFNLFVBQVUsSUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLElBQUksSUFBRSxDQUFDLElBQUUsQ0FBQyxJQUFFLENBQUMsQ0FBQyxNQUFNLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLElBQUksSUFBRSxDQUFDLElBQUUsQ0FBQyxDQUFDLFFBQVEsSUFBRSxDQUFDLENBQUMsYUFBYSxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTSxRQUFRLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsTUFBTSxDQUFDLGNBQWMsQ0FBQyxDQUFDLENBQUMsSUFBRSxNQUFNLENBQUMsU0FBUyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTSxRQUFRLElBQUUsT0FBTyxDQUFDLENBQUMsTUFBTSxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxVQUFTLENBQUMsRUFBQztBQUFDLGFBQU8sSUFBSSxJQUFFLENBQUMsQ0FBQTtLQUFDLENBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQztBQUFDLFdBQU8sQ0FBQyxDQUFDLE1BQU0sR0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxNQUFNLENBQUMsS0FBSyxDQUFDLEVBQUUsRUFBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLENBQUMsQ0FBQyxPQUFPLENBQUMsS0FBSyxFQUFDLEdBQUcsQ0FBQyxDQUFDLE9BQU8sQ0FBQyx1QkFBdUIsRUFBQyxPQUFPLENBQUMsQ0FBQyxPQUFPLENBQUMsbUJBQW1CLEVBQUMsT0FBTyxDQUFDLENBQUMsT0FBTyxDQUFDLElBQUksRUFBQyxHQUFHLENBQUMsQ0FBQyxXQUFXLEVBQUUsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQztBQUFDLFdBQU8sQ0FBQyxJQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLElBQUksTUFBTSxDQUFDLFNBQVMsR0FBQyxDQUFDLEdBQUMsU0FBUyxDQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTSxRQUFRLElBQUUsT0FBTyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEVBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsYUFBYSxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsV0FBVyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsR0FBQyxnQkFBZ0IsQ0FBQyxDQUFDLEVBQUMsRUFBRSxDQUFDLENBQUMsZ0JBQWdCLENBQUMsU0FBUyxDQUFDLEVBQUMsQ0FBQyxDQUFDLFVBQVUsQ0FBQyxXQUFXLENBQUMsQ0FBQyxDQUFDLEVBQUMsTUFBTSxJQUFFLENBQUMsS0FBRyxDQUFDLEdBQUMsT0FBTyxDQUFBLEFBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFBLEFBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUM7QUFBQyxXQUFNLFVBQVUsSUFBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsUUFBUSxDQUFDLEdBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsVUFBVSxFQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLElBQUUsQ0FBQyxDQUFDLFFBQVEsR0FBQyxDQUFDLEdBQUMsS0FBSyxDQUFDLENBQUE7S0FBQyxDQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFNBQUksQ0FBQyxJQUFJLENBQUMsRUFBQyxDQUFDLEtBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQSxBQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxFQUFFLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsRUFBRSxDQUFBLEFBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQSxHQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBRyxDQUFDLEtBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQSxBQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxJQUFJLElBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxJQUFFLENBQUMsR0FBQyxDQUFDLENBQUMsZUFBZSxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxZQUFZLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFFBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxTQUFTLElBQUUsRUFBRTtRQUFDLENBQUMsR0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLE9BQU8sS0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsT0FBTyxHQUFDLENBQUMsR0FBQyxNQUFLLENBQUMsR0FBQyxDQUFDLENBQUMsT0FBTyxHQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsU0FBUyxHQUFDLENBQUMsQ0FBQSxBQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUM7QUFBQyxRQUFHO0FBQUMsYUFBTyxDQUFDLEdBQUMsTUFBTSxJQUFFLENBQUMsS0FBRyxPQUFPLElBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxHQUFDLE1BQU0sSUFBRSxDQUFDLEdBQUMsSUFBSSxHQUFDLENBQUMs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QUMsQ0FBQyxFQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLGFBQU8sQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUE7S0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxRQUFRLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFFBQUksQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLGFBQWEsQ0FBQyxNQUFNLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxLQUFHLENBQUMsQ0FBQyxPQUFPLEtBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxFQUFDLFdBQVcsQ0FBQyxDQUFBLEFBQUMsRUFBQyxDQUFDLEtBQUcsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFFLE1BQU0sQ0FBQyxFQUFFLENBQUEsQUFBQyxFQUFDLENBQUMsSUFBSSxDQUFDLEtBQUcsQ0FBQyxHQUFDLEdBQUcsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsU0FBUyxHQUFDLEVBQUUsR0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsVUFBVSxDQUFDLEVBQUMsWUFBVTtBQUFDLE9BQUMsQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLENBQUE7S0FBQyxDQUFDLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxFQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLE9BQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO0tBQUMsQ0FBQyxDQUFBLEFBQUMsRUFBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLEdBQUMsQ0FBQyxJQUFFLEVBQUUsRUFBQyxDQUFDLENBQUMsU0FBUyxHQUFDLENBQUMsQ0FBQyxFQUFFLEVBQUMsQ0FBQyxDQUFDLFFBQVEsR0FBQyxDQUFDLElBQUUsRUFBRSxFQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFdBQU8sQ0FBQyxZQUFZLENBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLENBQUMsSUFBRyxDQUFDLENBQUMsRUFBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxJQUFHLFFBQVEsSUFBRSxPQUFPLENBQUMsRUFBQyxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxFQUFFLEVBQUMsR0FBRyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFBLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxRQUFRLENBQUMsQ0FBQyxFQUFDLE1BQU0sQ0FBQyxFQUFFLEVBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLElBQUksQ0FBQyxLQUFJO0FBQUMsVUFBRyxDQUFDLEtBQUcsQ0FBQyxFQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxNQUFJO0FBQUMsVUFBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUcsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsRUFBQyxPQUFPLENBQUMsQ0FBQyxJQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEtBQUssSUFBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLElBQUksQ0FBQyxLQUFLLElBQUcsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUFFLEVBQUMsTUFBTSxDQUFDLEVBQUUsRUFBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLEtBQUk7QUFBQyxZQUFHLENBQUMsS0FBRyxDQUFDLEVBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtPQUFDO0tBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxHQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFdBQU8sQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxNQUFNLEdBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxRQUFJLENBQUM7UUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLEVBQUMsQ0FBQyxDQUFDLENBQUMsT0FBTSxTQUFTLElBQUUsT0FBTyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsRUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEtBQUssRUFBRSxDQUFBLEFBQUMsRUFBQyxDQUFDLENBQUMsT0FBTyxDQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsT0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFBO0dBQUMsRUFBQyxDQUFDLENBQUMsR0FBRyxHQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFFBQUksQ0FBQztRQUFDLENBQUMsR0FBQyxHQUFHLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQztRQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBRSxHQUFHLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQztRQUFDLENBQUMsR0FBQyxDQUFDLElBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQztRQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsSUFBRSxDQUFDLEdBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLGNBQWMsQ0FBQyxDQUFDLENBQUMsQ0FBQSxHQUFFLENBQUMsQ0FBQyxDQUFDLEdBQUMsRUFBRSxHQUFDLENBQUMsS0FBRyxDQUFDLENBQUMsUUFBUSxJQUFFLENBQUMsS0FBRyxDQUFDLENBQUMsUUFBUSxHQUFDLEVBQUUsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsR0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLHNCQUFzQixDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLFFBQVEsR0FBQyxDQUFDLENBQUMsZUFBZSxDQUFDLFFBQVEsR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLENBQUMsS0FBRyxDQUFDLElBQUUsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBSyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQyxVQUFVLENBQUEsQUFBQyxHQUFFLElBQUcsQ0FBQyxLQUFHLENBQUMsRUFBQyxPQUFNLENBQUMsQ0FBQyxDQUFDLE9BQU0sQ0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxVQUFVLEdBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxRQUFRLEdBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxPQUFPLEdBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxhQUFhLEdBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxhQUFhLEdBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxRQUFJLENBQUMsQ0FBQyxLQUFJLENBQUMsSUFBSSxDQUFDLEVBQUMsT0FBTSxDQUFDLENBQUMsQ0FBQyxPQUFNLENBQUMsQ0FBQyxDQUFBO0dBQUMsRUFBQyxDQUFDLENBQUMsT0FBTyxHQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLENBQUMsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxTQUFTLEdBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxXQUFPLElBQUksSUFBRSxDQUFDLEdBQUMsRUFBRSxHQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksR0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLE9BQU8sR0FBQyxFQUFFLEVBQUMsQ0FBQyxDQUFDLElBQUksR0FBQyxFQUFFLEVBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxRQUFJLENBQUM7UUFBQyxDQUFDO1FBQUMsQ0FBQztRQUFDLENBQUMsR0FBQyxFQUFFLENBQUMsSUFBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsS0FBSSxDQUFDLEdBQUMsQ0FBQyxFQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsTUFBTSxFQUFDLENBQUMsRUFBRSxFQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFDLElBQUksSUFBRSxDQUFDLElBQUUsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFLLEtBQUksQ0FBQyxJQUFJLENBQUMsRUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsRUFBQyxJQUFJLElBQUUsQ0FBQyxJQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBSSxDQUFDLEdBQUMsQ0FBQyxFQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsTUFBTSxFQUFDLENBQUMsRUFBRSxFQUFDLElBQUcsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsQ0FBQyxFQUFDLE9BQU8sQ0FBQyxDQUFBO0tBQUMsTUFBSyxLQUFJLENBQUMsSUFBSSxDQUFDLEVBQUMsSUFBRyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxDQUFDLEVBQUMsT0FBTyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsTUFBTSxDQUFDLElBQUksS0FBRyxDQUFDLENBQUMsU0FBUyxHQUFDLElBQUksQ0FBQyxLQUFLLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsK0RBQStELENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxFQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLEtBQUMsQ0FBQyxVQUFVLEdBQUMsQ0FBQyxHQUFDLEdBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQyxXQUFXLEVBQUUsQ0FBQTtHQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsRUFBRSxHQUFDLEVBQUMsT0FBTyxFQUFDLENBQUMsQ0FBQyxPQUFPLEVBQUMsTUFBTSxFQUFDLENBQUMsQ0FBQyxNQUFNLEVBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUMsT0FBTyxFQUFDLENBQUMsQ0FBQyxPQUFPLEVBQUMsTUFBTSxFQUFDLENBQUMsQ0FBQyxNQUFNLEVBQUMsR0FBRyxFQUFDLGFBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxJQUFJLEVBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsZUFBTyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsS0FBSyxFQUFDLGlCQUFVO0FBQUMsYUFBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxJQUFJLEVBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsS0FBSyxFQUFDLGVBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxVQUFVLENBQUMsSUFBRSxDQUFDLENBQUMsSUFBSSxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsZ0JBQWdCLENBQUMsa0JBQWtCLEVBQUMsWUFBVTtBQUFDLFNBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtPQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxJQUFJLENBQUE7S0FBQyxFQUFDLEdBQUcsRUFBQyxhQUFTLENBQUMsRUFBQztBQUFDLGFBQU8sQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxHQUFDLElBQUksQ0FBQyxDQUFDLElBQUUsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFBO0tBQUMsRUFBQyxPQUFPLEVBQUMsbUJBQVU7QUFBQyxhQUFPLElBQUksQ0FBQyxHQUFHLEVBQUUsQ0FBQTtLQUFDLEVBQUMsSUFBSSxFQUFDLGdCQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsTUFBTSxDQUFBO0tBQUMsRUFBQyxNQUFNLEVBQUMsa0JBQVU7QUFBQyxhQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsWUFBVTtBQUFDLFlBQUksSUFBRSxJQUFJLENBQUMsVUFBVSxJQUFFLElBQUksQ0FBQyxVQUFVLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxDQUFBO09BQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxJQUFJLEVBQUMsY0FBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLElBQUksRUFBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxlQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsS0FBRyxDQUFDLENBQUMsQ0FBQTtPQUFDLENBQUMsRUFBQyxJQUFJLENBQUE7S0FBQyxFQUFDLE1BQU0sRUFBQyxnQkFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxlQUFPLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO09BQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLEdBQUcsRUFBQyxhQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxFQUFFLEVBQUMsWUFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLElBQUksQ0FBQyxNQUFNLEdBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxHQUFHLEVBQUMsYUFBUyxDQUFDLEVBQUM7QUFBQyxVQUFJLENBQUMsR0FBQyxFQUFFLENBQUMsSUFBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLElBQUksS0FBRyxDQUFDLEVBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFNBQUMsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUE7T0FBQyxDQUFDLENBQUMsS0FBSTtBQUFDLFlBQUksQ0FBQyxHQUFDLFFBQVEsSUFBRSxPQUFPLENBQUMsR0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxXQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFBO1NBQUMsQ0FBQyxDQUFBO09BQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLEdBQUcsRUFBQyxhQUFTLENBQUMsRUFBQztBQUFDLGFBQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQyxZQUFVO0FBQUMsZUFBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQTtPQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsRUFBRSxFQUFDLFlBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTSxDQUFDLENBQUMsS0FBRyxDQUFDLEdBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsS0FBSyxFQUFDLGlCQUFVO0FBQUMsVUFBSSxDQUFDLEdBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLElBQUksRUFBQyxnQkFBVTtBQUFDLFVBQUksQ0FBQyxHQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxHQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLElBQUksRUFBQyxjQUFTLENBQUMsRUFBQztBQUFDLFVBQUksQ0FBQztVQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLEdBQUMsQ0FBQyxHQUFDLFFBQVEsSUFBRSxPQUFPLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLFlBQVU7QUFBQyxZQUFJLENBQUMsR0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDLEVBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxpQkFBTyxDQUFDLENBQUMsUUFBUSxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtTQUFDLENBQUMsQ0FBQTtPQUFDLENBQUMsR0FBQyxDQUFDLElBQUUsSUFBSSxDQUFDLE1BQU0sR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLFlBQVU7QUFBQyxlQUFPLENBQUMsQ0FBQyxHQUFHLENBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxDQUFBO09BQUMsQ0FBQyxHQUFDLENBQUMsRUFBRSxDQUFBO0tBQUMsRUFBQyxPQUFPLEVBQUMsaUJBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFVBQUksQ0FBQyxHQUFDLElBQUksQ0FBQyxDQUFDLENBQUM7VUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSSxRQUFRLElBQUUsT0FBTyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxJQUFFLEVBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBLEFBQUMsR0FBRSxDQUFDLEdBQUMsQ0FBQyxLQUFHLENBQUMsSUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsVUFBVSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxPQUFPLEVBQUMsaUJBQVMsQ0FBQyxFQUFDO0FBQUMsV0FBSSxJQUFJLENBQUMsR0FBQyxFQUFFLEVBQUMsQ0FBQyxHQUFDLElBQUksRUFBQyxDQUFDLENBQUMsTUFBTSxHQUFDLENBQUMsR0FBRSxDQUFDLEdBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEVBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxlQUFNLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxVQUFVLENBQUEsSUFBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQSxHQUFFLEtBQUssQ0FBQyxDQUFBO09BQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsTUFBTSxFQUFDLGdCQUFTLENBQUMsRUFBQztBQUFDLGFBQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLFlBQVksQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLFFBQVEsRUFBQyxrQkFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLFlBQVU7QUFBQyxlQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQTtPQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsUUFBUSxFQUFDLG9CQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsR0FBRyxDQUFDLFlBQVU7QUFBQyxlQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxDQUFBO09BQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxRQUFRLEVBQUMsa0JBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxlQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxVQUFVLENBQUMsRUFBQyxVQUFTLENBQUMsRUFBQztBQUFDLGlCQUFPLENBQUMsS0FBRyxDQUFDLENBQUE7U0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLEtBQUssRUFBQyxpQkFBVTtBQUFDLGFBQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxZQUFVO0FBQUMsWUFBSSxDQUFDLFNBQVMsR0FBQyxFQUFFLENBQUE7T0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLEtBQUssRUFBQyxlQUFTLENBQUMsRUFBQztBQUFDLGFBQU8sQ0FBQyxDQUFDLEdBQUcsQ0FBQyxJQUFJLEVBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxlQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtPQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsSUFBSSxFQUFDLGdCQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsSUFBSSxDQUFDLFlBQVU7QUFBQyxjQUFNLElBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLEtBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxPQUFPLEdBQUMsRUFBRSxDQUFBLEFBQUMsRUFBQyxNQUFNLElBQUUsZ0JBQWdCLENBQUMsSUFBSSxFQUFDLEVBQUUsQ0FBQyxDQUFDLGdCQUFnQixDQUFDLFNBQVMsQ0FBQyxLQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUEsQUFBQyxDQUFBO09BQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxXQUFXLEVBQUMscUJBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDLE1BQU0sRUFBRSxDQUFBO0tBQUMsRUFBQyxJQUFJLEVBQUMsY0FBUyxDQUFDLEVBQUM7QUFBQyxVQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRyxJQUFJLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLEVBQUMsSUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7VUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLFVBQVUsSUFBRSxJQUFJLENBQUMsTUFBTSxHQUFDLENBQUMsQ0FBQyxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxTQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLElBQUksRUFBQyxDQUFDLENBQUMsR0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFBO09BQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxPQUFPLEVBQUMsaUJBQVMsQ0FBQyxFQUFDO0FBQUMsVUFBRyxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQUM7QUFBQyxTQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFJLElBQUksQ0FBQyxFQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxRQUFRLEVBQUUsQ0FBQSxDQUFFLE1BQU0sR0FBRSxDQUFDLEdBQUMsQ0FBQyxDQUFDLEtBQUssRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDLENBQUE7T0FBQyxPQUFPLElBQUksQ0FBQTtLQUFDLEVBQUMsU0FBUyxFQUFDLG1CQUFTLENBQUMsRUFBQztBQUFDLFVBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxZQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDO1lBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxRQUFRLEVBQUU7WUFBQyxDQUFDLEdBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsTUFBTSxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsQ0FBQTtPQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsTUFBTSxFQUFDLGtCQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsTUFBTSxFQUFFLENBQUMsSUFBSSxDQUFDLFlBQVU7QUFBQyxTQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsV0FBVyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxRQUFRLEVBQUUsQ0FBQyxDQUFBO09BQUMsQ0FBQyxFQUFDLElBQUksQ0FBQTtLQUFDLEVBQUMsS0FBSyxFQUFDLGlCQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsR0FBRyxDQUFDLFlBQVU7QUFBQyxlQUFPLElBQUksQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtPQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsSUFBSSxFQUFDLGdCQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsR0FBRyxDQUFDLFNBQVMsRUFBQyxNQUFNLENBQUMsQ0FBQTtLQUFDLEVBQUMsTUFBTSxFQUFDLGdCQUFTLENBQUMsRUFBQztBQUFDLGFBQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxZQUFVO0FBQUMsWUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxNQUFNLElBQUUsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxTQUFTLENBQUMsR0FBQyxDQUFDLENBQUEsR0FBRSxDQUFDLENBQUMsSUFBSSxFQUFFLEdBQUMsQ0FBQyxDQUFDLElBQUksRUFBRSxDQUFBO09BQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxJQUFJLEVBQUMsY0FBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLHdCQUF3QixDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxJQUFFLEdBQUcsQ0FBQyxDQUFBO0tBQUMsRUFBQyxJQUFJLEVBQUMsY0FBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLG9CQUFvQixDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxJQUFFLEdBQUcsQ0FBQyxDQUFBO0tBQUMsRUFBQyxJQUFJLEVBQUMsY0FBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsSUFBSSxTQUFTLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUksQ0FBQyxHQUFDLElBQUksQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLEtBQUssRUFBRSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtPQUFDLENBQUMsR0FBQyxDQUFDLElBQUksSUFBSSxHQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxTQUFTLEdBQUMsSUFBSSxDQUFBO0tBQUMsRUFBQyxJQUFJLEVBQUMsY0FBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsSUFBSSxTQUFTLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxJQUFJLENBQUMsV0FBVyxDQUFDLENBQUMsSUFBSSxDQUFDLFdBQVcsR0FBQyxJQUFJLElBQUUsQ0FBQyxHQUFDLEVBQUUsR0FBQyxFQUFFLEdBQUMsQ0FBQyxDQUFBO09BQUMsQ0FBQyxHQUFDLENBQUMsSUFBSSxJQUFJLEdBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLFdBQVcsR0FBQyxJQUFJLENBQUE7S0FBQyxFQUFDLElBQUksRUFBQyxjQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxVQUFJLENBQUMsQ0FBQyxPQUFNLFFBQVEsSUFBRSxPQUFPLENBQUMsSUFBRSxDQUFDLElBQUksU0FBUyxHQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxZQUFHLENBQUMsS0FBRyxJQUFJLENBQUMsUUFBUSxFQUFDLElBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLEtBQUksQ0FBQyxJQUFJLENBQUMsRUFBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxJQUFJLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxJQUFJLENBQUMsWUFBWSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtPQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsTUFBTSxJQUFFLENBQUMsS0FBRyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsUUFBUSxHQUFDLEVBQUUsQ0FBQyxHQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxZQUFZLENBQUMsQ0FBQyxDQUFDLENBQUEsQUFBQyxJQUFFLENBQUMsSUFBSSxJQUFJLENBQUMsQ0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLFVBQVUsRUFBQyxvQkFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsWUFBVTtBQUFDLFNBQUMsS0FBRyxJQUFJLENBQUMsUUFBUSxJQUFFLENBQUMsQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsV0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLENBQUMsQ0FBQTtTQUFDLEVBQUMsSUFBSSxDQUFDLENBQUE7T0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLElBQUksRUFBQyxjQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxFQUFDLENBQUMsSUFBSSxTQUFTLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUksQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFFLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsSUFBSSxFQUFDLGNBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFVBQUksQ0FBQyxHQUFDLE9BQU8sR0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsRUFBQyxLQUFLLENBQUMsQ0FBQyxXQUFXLEVBQUU7VUFBQyxDQUFDLEdBQUMsQ0FBQyxJQUFJLFNBQVMsR0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sSUFBSSxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxHQUFHLEVBQUMsYUFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsSUFBSSxTQUFTLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUksQ0FBQyxLQUFLLEdBQUMsQ0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQTtPQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLEtBQUcsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLFFBQVEsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxZQUFVO0FBQUMsZUFBTyxJQUFJLENBQUMsUUFBUSxDQUFBO09BQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsR0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFBLEFBQUMsQ0FBQTtLQUFDLEVBQUMsTUFBTSxFQUFDLGdCQUFTLENBQUMsRUFBQztBQUFDLFVBQUcsQ0FBQyxFQUFDLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUM7WUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxNQUFNLEVBQUUsQ0FBQztZQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsWUFBWSxFQUFFLENBQUMsTUFBTSxFQUFFO1lBQUMsQ0FBQyxHQUFDLEVBQUMsR0FBRyxFQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUMsQ0FBQyxDQUFDLEdBQUcsRUFBQyxJQUFJLEVBQUMsQ0FBQyxDQUFDLElBQUksR0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsUUFBUSxJQUFFLENBQUMsQ0FBQyxHQUFHLENBQUMsVUFBVSxDQUFDLEtBQUcsQ0FBQyxDQUFDLFFBQVEsR0FBQyxVQUFVLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLENBQUMsSUFBRyxDQUFDLElBQUksQ0FBQyxNQUFNLEVBQUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLHFCQUFxQixFQUFFLENBQUMsT0FBTSxFQUFDLElBQUksRUFBQyxDQUFDLENBQUMsSUFBSSxHQUFDLE1BQU0sQ0FBQyxXQUFXLEVBQUMsR0FBRyxFQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUMsTUFBTSxDQUFDLFdBQVcsRUFBQyxLQUFLLEVBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLEVBQUMsTUFBTSxFQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxFQUFDLENBQUE7S0FBQyxFQUFDLEdBQUcsRUFBQyxhQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxVQUFHLFNBQVMsQ0FBQyxNQUFNLEdBQUMsQ0FBQyxFQUFDO0FBQUMsWUFBSSxDQUFDO1lBQUMsQ0FBQyxHQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFHLENBQUMsQ0FBQyxFQUFDLE9BQU8sS0FBRyxDQUFDLEdBQUMsZ0JBQWdCLENBQUMsQ0FBQyxFQUFDLEVBQUUsQ0FBQyxFQUFDLFFBQVEsSUFBRSxPQUFPLENBQUMsQ0FBQSxFQUFDLE9BQU8sQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUM7QUFBQyxjQUFJLENBQUMsR0FBQyxFQUFFLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxhQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDLENBQUE7V0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFBO1NBQUM7T0FBQyxJQUFJLENBQUMsR0FBQyxFQUFFLENBQUMsSUFBRyxRQUFRLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsSUFBRSxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsR0FBRyxHQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxZQUFVO0FBQUMsWUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLENBQUMsS0FBSyxLQUFJLENBQUMsSUFBSSxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsS0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxHQUFHLEdBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxHQUFHLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxZQUFVO0FBQUMsWUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLENBQUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLFlBQVU7QUFBQyxZQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sSUFBRSxHQUFHLEdBQUMsQ0FBQyxDQUFBO09BQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxLQUFLLEVBQUMsZUFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsR0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLElBQUksQ0FBQyxNQUFNLEVBQUUsQ0FBQyxRQUFRLEVBQUUsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLFFBQVEsRUFBQyxrQkFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxlQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7T0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFBO0tBQUMsRUFBQyxRQUFRLEVBQUMsa0JBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUcsV0FBVyxJQUFHLElBQUksRUFBQztBQUFDLFdBQUMsR0FBQyxFQUFFLENBQUMsSUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQztjQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsQ0FBQyxPQUFPLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxhQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUE7V0FBQyxFQUFDLElBQUksQ0FBQyxFQUFDLENBQUMsQ0FBQyxNQUFNLElBQUUsQ0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLElBQUUsQ0FBQyxHQUFDLEdBQUcsR0FBQyxFQUFFLENBQUEsQUFBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQTtTQUFDO09BQUMsQ0FBQyxHQUFDLElBQUksQ0FBQTtLQUFDLEVBQUMsV0FBVyxFQUFDLHFCQUFTLENBQUMsRUFBQztBQUFDLGFBQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUcsV0FBVyxJQUFHLElBQUksRUFBQztBQUFDLGNBQUcsQ0FBQyxLQUFHLENBQUMsRUFBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLEVBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsRUFBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDLE9BQU8sQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLGFBQUMsR0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxHQUFHLENBQUMsQ0FBQTtXQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQyxDQUFBO1NBQUM7T0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLFdBQVcsRUFBQyxxQkFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUM7WUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsTUFBTSxDQUFDLENBQUMsT0FBTyxDQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsV0FBQyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUEsR0FBRSxDQUFDLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxXQUFXLENBQUMsQ0FBQyxDQUFDLENBQUE7U0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFBO0tBQUMsRUFBQyxTQUFTLEVBQUMsbUJBQVMsQ0FBQyxFQUFDO0FBQUMsVUFBRyxJQUFJLENBQUMsTUFBTSxFQUFDO0FBQUMsWUFBSSxDQUFDLElBQUMsV0FBVyxJQUFHLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQSxDQUFDLE9BQU8sQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLFNBQVMsR0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsV0FBVyxHQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxHQUFDLFlBQVU7QUFBQyxjQUFJLENBQUMsU0FBUyxHQUFDLENBQUMsQ0FBQTtTQUFDLEdBQUMsWUFBVTtBQUFDLGNBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBQyxDQUFDLENBQUMsQ0FBQTtTQUFDLENBQUMsQ0FBQTtPQUFDO0tBQUMsRUFBQyxVQUFVLEVBQUMsb0JBQVMsQ0FBQyxFQUFDO0FBQUMsVUFBRyxJQUFJLENBQUMsTUFBTSxFQUFDO0FBQUMsWUFBSSxDQUFDLElBQUMsWUFBWSxJQUFHLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQSxDQUFDLE9BQU8sQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLFVBQVUsR0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsV0FBVyxHQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxHQUFDLFlBQVU7QUFBQyxjQUFJLENBQUMsVUFBVSxHQUFDLENBQUMsQ0FBQTtTQUFDLEdBQUMsWUFBVTtBQUFDLGNBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxFQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQTtTQUFDLENBQUMsQ0FBQTtPQUFDO0tBQUMsRUFBQyxRQUFRLEVBQUMsb0JBQVU7QUFBQyxVQUFHLElBQUksQ0FBQyxNQUFNLEVBQUM7QUFBQyxZQUFJLENBQUMsR0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDO1lBQUMsQ0FBQyxHQUFDLElBQUksQ0FBQyxZQUFZLEVBQUU7WUFBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLE1BQU0sRUFBRTtZQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxRQUFRLENBQUMsR0FBQyxFQUFDLEdBQUcsRUFBQyxDQUFDLEVBQUMsSUFBSSxFQUFDLENBQUMsRUFBQyxHQUFDLENBQUMsQ0FBQyxNQUFNLEVBQUUsQ0FBQyxPQUFPLENBQUMsQ0FBQyxHQUFHLElBQUUsVUFBVSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsWUFBWSxDQUFDLENBQUMsSUFBRSxDQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksSUFBRSxVQUFVLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxhQUFhLENBQUMsQ0FBQyxJQUFFLENBQUMsRUFBQyxDQUFDLENBQUMsR0FBRyxJQUFFLFVBQVUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLGtCQUFrQixDQUFDLENBQUMsSUFBRSxDQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksSUFBRSxVQUFVLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxtQkFBbUIsQ0FBQyxDQUFDLElBQUUsQ0FBQyxFQUFDLEVBQUMsR0FBRyxFQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUMsQ0FBQyxDQUFDLEdBQUcsRUFBQyxJQUFJLEVBQUMsQ0FBQyxDQUFDLElBQUksR0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUE7T0FBQztLQUFDLEVBQUMsWUFBWSxFQUFDLHdCQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsR0FBRyxDQUFDLFlBQVU7QUFBQyxhQUFJLElBQUksQ0FBQyxHQUFDLElBQUksQ0FBQyxZQUFZLElBQUUsQ0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxRQUFRLENBQUMsSUFBRSxRQUFRLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsR0FBRSxDQUFDLEdBQUMsQ0FBQyxDQUFDLFlBQVksQ0FBQyxPQUFPLENBQUMsQ0FBQTtPQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsRUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLE1BQU0sR0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLE1BQU0sRUFBQyxDQUFDLE9BQU8sRUFBQyxRQUFRLENBQUMsQ0FBQyxPQUFPLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxRQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLEdBQUcsRUFBQyxVQUFTLENBQUMsRUFBQztBQUFDLGFBQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLFdBQVcsRUFBRSxDQUFBO0tBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLEdBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxVQUFJLENBQUM7VUFBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLE9BQU8sR0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLGVBQWUsQ0FBQyxRQUFRLEdBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLE1BQU0sRUFBRSxDQUFBLElBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxTQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxFQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLENBQUE7S0FBQyxDQUFBO0dBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEdBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxHQUFDLFlBQVU7QUFBQyxVQUFJLENBQUM7VUFBQyxDQUFDO1VBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsU0FBUyxFQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsZUFBTyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLFFBQVEsSUFBRSxDQUFDLElBQUUsT0FBTyxJQUFFLENBQUMsSUFBRSxJQUFJLElBQUUsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFBO09BQUMsQ0FBQztVQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsTUFBTSxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxNQUFNLEdBQUMsQ0FBQyxHQUFDLElBQUksR0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFNBQUMsR0FBQyxDQUFDLEdBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxVQUFVLEVBQUMsQ0FBQyxHQUFDLENBQUMsSUFBRSxDQUFDLEdBQUMsQ0FBQyxDQUFDLFdBQVcsR0FBQyxDQUFDLElBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxVQUFVLEdBQUMsQ0FBQyxJQUFFLENBQUMsR0FBQyxDQUFDLEdBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLGVBQWUsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsY0FBRyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFLLElBQUcsQ0FBQyxDQUFDLEVBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsTUFBTSxFQUFFLENBQUMsQ0FBQyxDQUFDLFlBQVksQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLEVBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxnQkFBSSxJQUFFLENBQUMsQ0FBQyxRQUFRLElBQUUsUUFBUSxLQUFHLENBQUMsQ0FBQyxRQUFRLENBQUMsV0FBVyxFQUFFLElBQUUsQ0FBQyxDQUFDLElBQUksSUFBRSxpQkFBaUIsS0FBRyxDQUFDLENBQUMsSUFBSSxJQUFFLENBQUMsQ0FBQyxHQUFHLElBQUUsTUFBTSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxFQUFDLENBQUMsQ0FBQyxTQUFTLENBQUMsQ0FBQTtXQUFDLENBQUMsQ0FBQTtTQUFDLENBQUMsQ0FBQTtPQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxHQUFDLElBQUksR0FBQyxRQUFRLElBQUUsQ0FBQyxHQUFDLFFBQVEsR0FBQyxPQUFPLENBQUEsQUFBQyxDQUFDLEdBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsRUFBQyxJQUFJLENBQUE7S0FBQyxDQUFBO0dBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsU0FBUyxHQUFDLENBQUMsQ0FBQyxFQUFFLEVBQUMsQ0FBQyxDQUFDLElBQUksR0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLGdCQUFnQixHQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsS0FBSyxHQUFDLENBQUMsRUFBQyxDQUFDLENBQUE7Q0FBQyxDQUFBLEVBQUUsQ0FBQyxNQUFNLENBQUMsS0FBSyxHQUFDLEtBQUssRUFBQyxLQUFLLENBQUMsS0FBRyxNQUFNLENBQUMsQ0FBQyxLQUFHLE1BQU0sQ0FBQyxDQUFDLEdBQUMsS0FBSyxDQUFBLEFBQUMsRUFBQyxDQUFBLFVBQVMsQ0FBQyxFQUFDO0FBQUMsV0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsSUFBSSxLQUFHLENBQUMsQ0FBQyxJQUFJLEdBQUMsQ0FBQyxFQUFFLENBQUEsQUFBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsU0FBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFFLENBQUEsRUFBQyxJQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLE9BQU0sQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsRUFBRSxDQUFBLENBQUUsTUFBTSxDQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTSxFQUFFLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxFQUFFLElBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsSUFBRSxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsS0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxJQUFFLENBQUMsQ0FBQyxHQUFHLElBQUUsQ0FBQyxDQUFBLEFBQUMsQ0FBQTtLQUFDLENBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQztBQUFDLFFBQUksQ0FBQyxHQUFDLENBQUMsRUFBRSxHQUFDLENBQUMsQ0FBQSxDQUFFLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFNLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxFQUFFLEVBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLEVBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQztBQUFDLFdBQU8sSUFBSSxNQUFNLENBQUMsU0FBUyxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxFQUFDLE9BQU8sQ0FBQyxHQUFDLFNBQVMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFdBQU8sQ0FBQyxDQUFDLEdBQUcsSUFBRSxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsSUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxRQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO1FBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsRUFBRSxDQUFBLEFBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFVBQUcsT0FBTyxJQUFFLENBQUMsRUFBQyxPQUFPLENBQUMsQ0FBQyxRQUFRLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFFLEdBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFHLENBQUMsR0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxhQUFhLENBQUMsT0FBTSxDQUFDLENBQUMsSUFBRSxDQUFDLEtBQUcsSUFBSSxJQUFFLENBQUMsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUMsSUFBSSxFQUFDLFNBQVMsQ0FBQyxHQUFDLEtBQUssQ0FBQyxDQUFBO09BQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLEdBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxHQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLDZCQUE2QixFQUFFLENBQUEsRUFBQztBQUFDLFdBQUMsQ0FBQyxJQUFJLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxLQUFLLElBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLEtBQUcsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxDQUFDLGNBQWMsRUFBRSxFQUFDLENBQUMsQ0FBQyxlQUFlLEVBQUUsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFBO1NBQUM7T0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLE1BQU0sRUFBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxFQUFDLGtCQUFrQixJQUFHLENBQUMsSUFBRSxDQUFDLENBQUMsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsS0FBSyxFQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtLQUFDLENBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxRQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsRUFBRSxDQUFBLENBQUUsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLE9BQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxlQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMscUJBQXFCLElBQUcsQ0FBQyxJQUFFLENBQUMsQ0FBQyxtQkFBbUIsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxLQUFLLEVBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFBO09BQUMsQ0FBQyxDQUFBO0tBQUMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFdBQU0sQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsa0JBQWtCLENBQUEsS0FBSSxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLEVBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsVUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxZQUFVO0FBQUMsZUFBTyxJQUFJLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxFQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsS0FBSyxDQUFDLENBQUMsRUFBQyxTQUFTLENBQUMsQ0FBQTtPQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQTtLQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxnQkFBZ0IsS0FBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLGdCQUFnQixHQUFDLGFBQWEsSUFBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLFdBQVcsS0FBRyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsaUJBQWlCLElBQUUsQ0FBQyxDQUFDLGlCQUFpQixFQUFFLENBQUEsS0FBSSxDQUFDLENBQUMsa0JBQWtCLEdBQUMsQ0FBQyxDQUFBLEFBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDO1FBQUMsQ0FBQyxHQUFDLEVBQUMsYUFBYSxFQUFDLENBQUMsRUFBQyxDQUFDLEtBQUksQ0FBQyxJQUFJLENBQUMsRUFBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsS0FBRyxDQUFDLEtBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQSxBQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsSUFBSSxDQUFDO01BQUMsQ0FBQyxHQUFDLENBQUM7TUFBQyxDQUFDLEdBQUMsS0FBSyxDQUFDLFNBQVMsQ0FBQyxLQUFLO01BQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxVQUFVO01BQUMsQ0FBQyxHQUFDLFNBQUYsQ0FBQyxDQUFVLENBQUMsRUFBQztBQUFDLFdBQU0sUUFBUSxJQUFFLE9BQU8sQ0FBQyxDQUFBO0dBQUM7TUFBQyxDQUFDLEdBQUMsRUFBRTtNQUFDLENBQUMsR0FBQyxFQUFFO01BQUMsQ0FBQyxJQUFDLFdBQVcsSUFBRyxNQUFNLENBQUE7TUFBQyxDQUFDLEdBQUMsRUFBQyxLQUFLLEVBQUMsU0FBUyxFQUFDLElBQUksRUFBQyxVQUFVLEVBQUM7TUFBQyxDQUFDLEdBQUMsRUFBQyxVQUFVLEVBQUMsV0FBVyxFQUFDLFVBQVUsRUFBQyxVQUFVLEVBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxHQUFDLENBQUMsQ0FBQyxTQUFTLEdBQUMsQ0FBQyxDQUFDLE9BQU8sR0FBQyxDQUFDLENBQUMsU0FBUyxHQUFDLGFBQWEsRUFBQyxDQUFDLENBQUMsS0FBSyxHQUFDLEVBQUMsR0FBRyxFQUFDLENBQUMsRUFBQyxNQUFNLEVBQUMsQ0FBQyxFQUFDLEVBQUMsQ0FBQyxDQUFDLEtBQUssR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxRQUFJLENBQUMsR0FBQyxDQUFDLElBQUksU0FBUyxJQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxFQUFDLENBQUMsQ0FBQyxDQUFDLElBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsVUFBSSxDQUFDLEdBQUMsU0FBRixDQUFDLEdBQVc7QUFBQyxlQUFPLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxFQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUMsR0FBQyxTQUFTLENBQUMsQ0FBQTtPQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUE7S0FBQyxJQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxPQUFPLENBQUMsSUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsS0FBSyxDQUFDLEtBQUssQ0FBQyxJQUFJLEVBQUMsQ0FBQyxDQUFDLENBQUEsR0FBRSxDQUFDLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxNQUFNLElBQUksU0FBUyxDQUFDLG1CQUFtQixDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSSxHQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxNQUFNLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxHQUFHLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsQ0FBQyxJQUFJLENBQUMsR0FBQyxTQUFGLENBQUMsR0FBVztBQUFDLFdBQU0sQ0FBQyxDQUFDLENBQUE7R0FBQztNQUFDLENBQUMsR0FBQyxTQUFGLENBQUMsR0FBVztBQUFDLFdBQU0sQ0FBQyxDQUFDLENBQUE7R0FBQztNQUFDLENBQUMsR0FBQyxrQ0FBa0M7TUFBQyxDQUFDLEdBQUMsRUFBQyxjQUFjLEVBQUMsb0JBQW9CLEVBQUMsd0JBQXdCLEVBQUMsK0JBQStCLEVBQUMsZUFBZSxFQUFDLHNCQUFzQixFQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxRQUFRLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFdBQU8sSUFBSSxDQUFDLEVBQUUsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsRUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLFVBQVUsR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSSxHQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFdBQU8sQ0FBQyxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLFFBQVEsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUMsSUFBSSxDQUFBO0dBQUMsRUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLEdBQUcsR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLENBQUMsQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLENBQUMsVUFBVSxDQUFDLElBQUksQ0FBQyxRQUFRLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFDLElBQUksQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFFLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDO1FBQUMsQ0FBQztRQUFDLENBQUMsR0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLEVBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsT0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFBLElBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLEtBQUcsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsRUFBQyxDQUFDLEdBQUMsQ0FBQyxFQUFDLENBQUMsR0FBQyxDQUFDLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsS0FBRyxDQUFDLENBQUMsQ0FBQSxLQUFJLENBQUMsR0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxLQUFHLENBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsT0FBQyxLQUFHLENBQUMsR0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLGVBQU8sQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsSUFBSSxFQUFDLFNBQVMsQ0FBQyxDQUFBO09BQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFlBQUksQ0FBQztZQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxJQUFFLENBQUMsS0FBRyxDQUFDLElBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLEVBQUMsYUFBYSxFQUFDLENBQUMsRUFBQyxTQUFTLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUEsQ0FBRSxLQUFLLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQSxHQUFFLEtBQUssQ0FBQyxDQUFBO09BQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFBO0tBQUMsQ0FBQyxDQUFBLEFBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxHQUFHLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFFBQUksQ0FBQyxHQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFDLENBQUMsR0FBRyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFBLElBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLEtBQUcsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsRUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFBLEFBQUMsRUFBQyxDQUFDLEtBQUcsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxZQUFVO0FBQUMsT0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO0tBQUMsQ0FBQyxDQUFBLEFBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxPQUFPLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxhQUFhLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEtBQUssR0FBQyxDQUFDLEVBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxZQUFVO0FBQUMsT0FBQyxDQUFDLElBQUksSUFBSSxDQUFDLElBQUUsVUFBVSxJQUFFLE9BQU8sSUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsR0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxFQUFFLEdBQUMsZUFBZSxJQUFHLElBQUksR0FBQyxJQUFJLENBQUMsYUFBYSxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxjQUFjLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO0tBQUMsQ0FBQyxDQUFBO0dBQUMsRUFBQyxDQUFDLENBQUMsRUFBRSxDQUFDLGNBQWMsR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxRQUFJLENBQUMsRUFBQyxDQUFDLENBQUMsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLE9BQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEtBQUssR0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLE1BQU0sR0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLElBQUUsQ0FBQyxDQUFDLEVBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsZUFBTyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsNkJBQTZCLEVBQUUsR0FBQyxDQUFDLENBQUMsR0FBQyxLQUFLLENBQUMsQ0FBQTtPQUFDLENBQUMsQ0FBQTtLQUFDLENBQUMsRUFBQyxDQUFDLENBQUE7R0FBQyxFQUFDLHNMQUFzTCxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxLQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxHQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLElBQUksU0FBUyxHQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxHQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxDQUFBO0dBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsS0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUEsQUFBQyxDQUFDLElBQUksQ0FBQyxHQUFDLFFBQVEsQ0FBQyxXQUFXLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFFLFFBQVEsQ0FBQztRQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFHLENBQUMsRUFBQyxLQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsRUFBQyxTQUFTLElBQUUsQ0FBQyxHQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsU0FBUyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxDQUFBO0NBQUMsQ0FBQSxDQUFDLEtBQUssQ0FBQyxFQUFDLENBQUEsVUFBUyxDQUFDLEVBQUM7QUFBQyxXQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFFBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxrQkFBa0IsRUFBRSxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsTUFBTSxHQUFDLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsR0FBQyxLQUFLLENBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQztBQUFDLEtBQUMsQ0FBQyxNQUFNLElBQUUsQ0FBQyxLQUFHLENBQUMsQ0FBQyxNQUFNLEVBQUUsSUFBRSxDQUFDLENBQUMsQ0FBQyxFQUFDLElBQUksRUFBQyxXQUFXLENBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQztBQUFDLEtBQUMsQ0FBQyxNQUFNLElBQUUsRUFBQyxFQUFFLENBQUMsQ0FBQyxNQUFNLElBQUUsQ0FBQyxDQUFDLENBQUMsRUFBQyxJQUFJLEVBQUMsVUFBVSxDQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsQ0FBQyxVQUFVLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsZ0JBQWdCLEVBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsS0FBRyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsR0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLFVBQVUsRUFBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLE9BQU87UUFBQyxDQUFDLEdBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsV0FBVyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxhQUFhLEVBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsRUFBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLFVBQVUsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsV0FBVyxFQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLGNBQWMsRUFBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLFNBQVMsQ0FBQyxHQUFFLEVBQUUsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsR0FBRyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFBLEFBQUMsRUFBQyxDQUFDLEtBQUcsQ0FBQyxJQUFFLENBQUMsR0FBQyxNQUFNLEdBQUMsQ0FBQyxJQUFFLENBQUMsR0FBQyxNQUFNLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsR0FBQyxRQUFRLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsSUFBRSxLQUFLLENBQUEsQUFBQyxJQUFFLE1BQU0sQ0FBQTtHQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxXQUFNLEVBQUUsSUFBRSxDQUFDLEdBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxHQUFDLEdBQUcsR0FBQyxDQUFDLENBQUEsQ0FBRSxPQUFPLENBQUMsV0FBVyxFQUFDLEdBQUcsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsS0FBQyxDQUFDLFdBQVcsSUFBRSxDQUFDLENBQUMsSUFBSSxJQUFFLFFBQVEsSUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBRyxDQUFDLENBQUMsSUFBSSxHQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLENBQUMsV0FBVyxDQUFDLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksSUFBRSxDQUFDLENBQUMsSUFBSSxJQUFFLEtBQUssSUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLFdBQVcsRUFBRSxLQUFHLENBQUMsQ0FBQyxHQUFHLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEVBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUMsS0FBSyxDQUFDLENBQUEsQUFBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsVUFBVSxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsRUFBQyxDQUFDLEdBQUMsS0FBSyxDQUFDLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxVQUFVLENBQUMsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxHQUFDLENBQUMsRUFBQyxDQUFDLEdBQUMsS0FBSyxDQUFDLENBQUEsQUFBQyxFQUFDLEVBQUMsR0FBRyxFQUFDLENBQUMsRUFBQyxJQUFJLEVBQUMsQ0FBQyxFQUFDLE9BQU8sRUFBQyxDQUFDLEVBQUMsUUFBUSxFQUFDLENBQUMsRUFBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDO1FBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDO1FBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxhQUFhLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLEVBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsT0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLEdBQUMsQ0FBQyxHQUFDLENBQUMsR0FBQyxHQUFHLElBQUUsQ0FBQyxJQUFFLFFBQVEsSUFBRSxDQUFDLElBQUUsT0FBTyxJQUFFLENBQUMsR0FBQyxDQUFDLEdBQUMsRUFBRSxDQUFBLEFBQUMsR0FBQyxHQUFHLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxJQUFFLENBQUMsR0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxHQUFDLE9BQU8sSUFBRSxDQUFDLElBQUUsQ0FBQyxDQUFDLElBQUUsUUFBUSxJQUFFLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxDQUFDLENBQUE7R0FBQyxJQUFJLENBQUM7TUFBQyxDQUFDO01BQUMsQ0FBQyxHQUFDLENBQUM7TUFBQyxDQUFDLEdBQUMsTUFBTSxDQUFDLFFBQVE7TUFBQyxDQUFDLEdBQUMscURBQXFEO01BQUMsQ0FBQyxHQUFDLG9DQUFvQztNQUFDLENBQUMsR0FBQyw2QkFBNkI7TUFBQyxDQUFDLEdBQUMsa0JBQWtCO01BQUMsQ0FBQyxHQUFDLFdBQVc7TUFBQyxDQUFDLEdBQUMsT0FBTztNQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsYUFBYSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUMsTUFBTSxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUMsQ0FBQyxDQUFDLE1BQU0sR0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLFNBQVMsR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxRQUFHLEVBQUUsTUFBTSxJQUFHLENBQUMsQ0FBQSxBQUFDLEVBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQztRQUFDLENBQUM7UUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLGFBQWE7UUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsVUFBVSxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsRUFBRSxHQUFDLENBQUMsQ0FBQSxJQUFHLE9BQU8sR0FBRSxFQUFFLENBQUM7UUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLGFBQWEsQ0FBQyxRQUFRLENBQUM7UUFBQyxDQUFDLEdBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQztRQUFDLENBQUMsR0FBQyxTQUFGLENBQUMsQ0FBVSxDQUFDLEVBQUM7QUFBQyxPQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsY0FBYyxDQUFDLE9BQU8sRUFBQyxDQUFDLElBQUUsT0FBTyxDQUFDLENBQUE7S0FBQztRQUFDLENBQUMsR0FBQyxFQUFDLEtBQUssRUFBQyxDQUFDLEVBQUMsQ0FBQyxPQUFPLENBQUMsSUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsWUFBWSxFQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLGtCQUFZLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsRUFBRSxDQUFDLE1BQU0sRUFBRSxFQUFDLE9BQU8sSUFBRSxDQUFDLENBQUMsSUFBSSxJQUFFLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksRUFBQyxDQUFDLElBQUUsT0FBTyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsRUFBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLFVBQVUsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsR0FBQyxLQUFLLENBQUMsQ0FBQTtLQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxPQUFPLENBQUMsRUFBQyxDQUFDLENBQUEsSUFBRyxNQUFNLENBQUMsQ0FBQyxDQUFDLEdBQUMsWUFBVTtBQUFDLE9BQUMsR0FBQyxTQUFTLENBQUE7S0FBQyxFQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxPQUFPLENBQUMsV0FBVyxFQUFDLE1BQU0sR0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsSUFBSSxDQUFDLFdBQVcsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsT0FBTyxHQUFDLENBQUMsS0FBRyxDQUFDLEdBQUMsVUFBVSxDQUFDLFlBQVU7QUFBQyxPQUFDLENBQUMsU0FBUyxDQUFDLENBQUE7S0FBQyxFQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFBLEFBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLFlBQVksR0FBQyxFQUFDLElBQUksRUFBQyxLQUFLLEVBQUMsVUFBVSxFQUFDLENBQUMsRUFBQyxPQUFPLEVBQUMsQ0FBQyxFQUFDLEtBQUssRUFBQyxDQUFDLEVBQUMsUUFBUSxFQUFDLENBQUMsRUFBQyxPQUFPLEVBQUMsSUFBSSxFQUFDLE1BQU0sRUFBQyxDQUFDLENBQUMsRUFBQyxHQUFHLEVBQUMsZUFBVTtBQUFDLGFBQU8sSUFBSSxNQUFNLENBQUMsY0FBYyxFQUFBLENBQUE7S0FBQyxFQUFDLE9BQU8sRUFBQyxFQUFDLE1BQU0sRUFBQyxtRUFBbUUsRUFBQyxJQUFJLEVBQUMsQ0FBQyxFQUFDLEdBQUcsRUFBQywyQkFBMkIsRUFBQyxJQUFJLEVBQUMsQ0FBQyxFQUFDLElBQUksRUFBQyxZQUFZLEVBQUMsRUFBQyxXQUFXLEVBQUMsQ0FBQyxDQUFDLEVBQUMsT0FBTyxFQUFDLENBQUMsRUFBQyxXQUFXLEVBQUMsQ0FBQyxDQUFDLEVBQUMsS0FBSyxFQUFDLENBQUMsQ0FBQyxFQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksR0FBQyxVQUFTLENBQUMsRUFBQztBQUFDLFFBQUksQ0FBQztRQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsTUFBTSxDQUFDLEVBQUUsRUFBQyxDQUFDLElBQUUsRUFBRSxDQUFDO1FBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxRQUFRLElBQUUsQ0FBQyxDQUFDLFFBQVEsRUFBRSxDQUFDLEtBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxZQUFZLEVBQUMsS0FBSyxDQUFDLEtBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsWUFBWSxDQUFDLENBQUMsQ0FBQyxDQUFBLEFBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLFdBQVcsS0FBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLGFBQWEsQ0FBQyxHQUFHLENBQUMsRUFBQyxDQUFDLENBQUMsSUFBSSxHQUFDLENBQUMsQ0FBQyxHQUFHLEVBQUMsQ0FBQyxDQUFDLElBQUksR0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxXQUFXLEdBQUMsQ0FBQyxDQUFDLFFBQVEsR0FBQyxJQUFJLEdBQUMsQ0FBQyxDQUFDLElBQUksSUFBRSxDQUFDLENBQUMsUUFBUSxHQUFDLElBQUksR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFBLEFBQUMsRUFBQyxDQUFDLENBQUMsR0FBRyxLQUFHLENBQUMsQ0FBQyxHQUFHLEdBQUMsTUFBTSxDQUFDLFFBQVEsQ0FBQyxRQUFRLEVBQUUsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxRQUFRO1FBQUMsQ0FBQyxHQUFDLFNBQVMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxLQUFHLENBQUMsR0FBQyxPQUFPLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxLQUFLLEtBQUcsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxJQUFFLENBQUMsQ0FBQyxLQUFLLEtBQUcsQ0FBQyxDQUFDLElBQUUsUUFBUSxJQUFFLENBQUMsSUFBRSxPQUFPLElBQUUsQ0FBQyxDQUFBLEFBQUMsS0FBRyxDQUFDLENBQUMsR0FBRyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBRyxFQUFDLElBQUksR0FBQyxJQUFJLENBQUMsR0FBRyxFQUFFLENBQUMsQ0FBQSxBQUFDLEVBQUMsT0FBTyxJQUFFLENBQUMsQ0FBQSxFQUFDLE9BQU8sQ0FBQyxLQUFHLENBQUMsQ0FBQyxHQUFHLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEVBQUMsQ0FBQyxDQUFDLEtBQUssR0FBQyxDQUFDLENBQUMsS0FBSyxHQUFDLElBQUksR0FBQyxDQUFDLENBQUMsS0FBSyxLQUFHLENBQUMsQ0FBQyxHQUFDLEVBQUUsR0FBQyxZQUFZLENBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFDLFNBQVMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDO1FBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDO1FBQUMsQ0FBQyxHQUFDLEVBQUU7UUFBQyxDQUFDLEdBQUMsU0FBRixDQUFDLENBQVUsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLE9BQUMsQ0FBQyxDQUFDLENBQUMsV0FBVyxFQUFFLENBQUMsR0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtLQUFDO1FBQUMsQ0FBQyxHQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLEdBQUMsTUFBTSxDQUFDLEVBQUUsR0FBQyxNQUFNLENBQUMsUUFBUSxDQUFDLFFBQVE7UUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEdBQUcsRUFBRTtRQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsZ0JBQWdCLENBQUMsS0FBRyxDQUFDLElBQUUsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsV0FBVyxJQUFFLENBQUMsQ0FBQyxrQkFBa0IsRUFBQyxnQkFBZ0IsQ0FBQyxFQUFDLENBQUMsQ0FBQyxRQUFRLEVBQUMsQ0FBQyxJQUFFLEtBQUssQ0FBQyxFQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxRQUFRLElBQUUsQ0FBQyxDQUFBLEtBQUksQ0FBQyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsS0FBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxHQUFHLEVBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxnQkFBZ0IsSUFBRSxDQUFDLENBQUMsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLFdBQVcsSUFBRSxDQUFDLENBQUMsV0FBVyxLQUFHLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxJQUFJLElBQUUsS0FBSyxJQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFLENBQUEsSUFBRyxDQUFDLENBQUMsY0FBYyxFQUFDLENBQUMsQ0FBQyxXQUFXLElBQUUsbUNBQW1DLENBQUMsRUFBQyxDQUFDLENBQUMsT0FBTyxDQUFBLEVBQUMsS0FBSSxDQUFDLElBQUksQ0FBQyxDQUFDLE9BQU8sRUFBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsQ0FBQyxnQkFBZ0IsR0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLGtCQUFrQixHQUFDLFlBQVU7QUFBQyxVQUFHLENBQUMsSUFBRSxDQUFDLENBQUMsVUFBVSxFQUFDO0FBQUMsU0FBQyxDQUFDLGtCQUFrQixHQUFDLENBQUMsRUFBQyxZQUFZLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDO1lBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLElBQUcsQ0FBQyxDQUFDLE1BQU0sSUFBRSxHQUFHLElBQUUsQ0FBQyxDQUFDLE1BQU0sR0FBQyxHQUFHLElBQUUsR0FBRyxJQUFFLENBQUMsQ0FBQyxNQUFNLElBQUUsQ0FBQyxJQUFFLENBQUMsQ0FBQyxNQUFNLElBQUUsT0FBTyxJQUFFLENBQUMsRUFBQztBQUFDLFdBQUMsR0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxRQUFRLElBQUUsQ0FBQyxDQUFDLGlCQUFpQixDQUFDLGNBQWMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxZQUFZLENBQUMsSUFBRztBQUFDLG9CQUFRLElBQUUsQ0FBQyxHQUFDLENBQUMsQ0FBQyxFQUFDLElBQUksQ0FBQSxDQUFFLENBQUMsQ0FBQyxHQUFDLEtBQUssSUFBRSxDQUFDLEdBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxXQUFXLEdBQUMsTUFBTSxJQUFFLENBQUMsS0FBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsR0FBQyxJQUFJLEdBQUMsQ0FBQyxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQSxBQUFDLENBQUE7V0FBQyxDQUFBLE9BQU0sQ0FBQyxFQUFDO0FBQUMsYUFBQyxHQUFDLENBQUMsQ0FBQTtXQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLGFBQWEsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtTQUFDLE1BQUssQ0FBQyxDQUFDLENBQUMsQ0FBQyxVQUFVLElBQUUsSUFBSSxFQUFDLENBQUMsQ0FBQyxNQUFNLEdBQUMsT0FBTyxHQUFDLE9BQU8sRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFBO09BQUM7S0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEtBQUcsQ0FBQyxDQUFDLENBQUEsRUFBQyxPQUFPLENBQUMsQ0FBQyxLQUFLLEVBQUUsRUFBQyxDQUFDLENBQUMsSUFBSSxFQUFDLE9BQU8sRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFHLENBQUMsQ0FBQyxTQUFTLEVBQUMsS0FBSSxDQUFDLElBQUksQ0FBQyxDQUFDLFNBQVMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLFNBQVMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsR0FBQyxPQUFPLElBQUcsQ0FBQyxHQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsQ0FBQyxHQUFHLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxRQUFRLEVBQUMsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxDQUFDLEtBQUksQ0FBQyxJQUFJLENBQUMsRUFBQyxDQUFDLENBQUMsS0FBSyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxPQUFPLEdBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxVQUFVLENBQUMsWUFBVTtBQUFDLE9BQUMsQ0FBQyxrQkFBa0IsR0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEtBQUssRUFBRSxFQUFDLENBQUMsQ0FBQyxJQUFJLEVBQUMsU0FBUyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQSxBQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsSUFBSSxHQUFDLENBQUMsQ0FBQyxJQUFJLEdBQUMsSUFBSSxDQUFDLEVBQUMsQ0FBQyxDQUFBO0dBQUMsRUFBQyxDQUFDLENBQUMsR0FBRyxHQUFDLFlBQVU7QUFBQyxXQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxJQUFJLEVBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksR0FBQyxZQUFVO0FBQUMsUUFBSSxDQUFDLEdBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxJQUFJLEVBQUMsU0FBUyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxHQUFDLE1BQU0sRUFBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFBO0dBQUMsRUFBQyxDQUFDLENBQUMsT0FBTyxHQUFDLFlBQVU7QUFBQyxRQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLElBQUksRUFBQyxTQUFTLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxRQUFRLEdBQUMsTUFBTSxFQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSSxHQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxRQUFHLENBQUMsSUFBSSxDQUFDLE1BQU0sRUFBQyxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUM7UUFBQyxDQUFDLEdBQUMsSUFBSTtRQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQztRQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUM7UUFBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsQ0FBQyxNQUFNLEdBQUMsQ0FBQyxLQUFHLENBQUMsQ0FBQyxHQUFHLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUEsQUFBQyxFQUFDLENBQUMsQ0FBQyxPQUFPLEdBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxPQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxFQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsSUFBRSxDQUFDLENBQUMsS0FBSyxDQUFDLENBQUMsRUFBQyxTQUFTLENBQUMsQ0FBQTtLQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsRUFBQyxJQUFJLENBQUE7R0FBQyxDQUFDLElBQUksQ0FBQyxHQUFDLGtCQUFrQixDQUFDLENBQUMsQ0FBQyxLQUFLLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEdBQUMsRUFBRSxDQUFDLE9BQU8sQ0FBQyxDQUFDLEdBQUcsR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxPQUFDLENBQUMsVUFBVSxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLEVBQUUsQ0FBQSxBQUFDLEVBQUMsSUFBSSxJQUFFLENBQUMsS0FBRyxDQUFDLEdBQUMsRUFBRSxDQUFBLEFBQUMsRUFBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxHQUFHLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUE7S0FBQyxFQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLENBQUMsT0FBTyxDQUFDLE1BQU0sRUFBQyxHQUFHLENBQUMsQ0FBQTtHQUFDLENBQUE7Q0FBQyxDQUFBLENBQUMsS0FBSyxDQUFDLEVBQUMsQ0FBQSxVQUFTLENBQUMsRUFBQztBQUFDLEdBQUMsQ0FBQyxFQUFFLENBQUMsY0FBYyxHQUFDLFlBQVU7QUFBQyxRQUFJLENBQUM7UUFBQyxDQUFDO1FBQUMsQ0FBQyxHQUFDLEVBQUU7UUFBQyxDQUFDLEdBQUMsU0FBRixDQUFDLENBQVUsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLENBQUMsT0FBTyxHQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLEdBQUMsS0FBSyxDQUFDLENBQUMsSUFBSSxDQUFDLEVBQUMsSUFBSSxFQUFDLENBQUMsRUFBQyxLQUFLLEVBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQTtLQUFDLENBQUMsT0FBTyxJQUFJLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsUUFBUSxFQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLE9BQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsSUFBSSxFQUFDLENBQUMsSUFBRSxVQUFVLElBQUUsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxXQUFXLEVBQUUsSUFBRSxDQUFDLENBQUMsQ0FBQyxRQUFRLElBQUUsUUFBUSxJQUFFLENBQUMsSUFBRSxPQUFPLElBQUUsQ0FBQyxJQUFFLFFBQVEsSUFBRSxDQUFDLElBQUUsTUFBTSxJQUFFLENBQUMsS0FBRyxPQUFPLElBQUUsQ0FBQyxJQUFFLFVBQVUsSUFBRSxDQUFDLElBQUUsQ0FBQyxDQUFDLE9BQU8sQ0FBQSxBQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEVBQUUsQ0FBQyxDQUFBO0tBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxTQUFTLEdBQUMsWUFBVTtBQUFDLFFBQUksQ0FBQyxHQUFDLEVBQUUsQ0FBQyxPQUFPLElBQUksQ0FBQyxjQUFjLEVBQUUsQ0FBQyxPQUFPLENBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxPQUFDLENBQUMsSUFBSSxDQUFDLGtCQUFrQixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsR0FBQyxHQUFHLEdBQUMsa0JBQWtCLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUE7S0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxHQUFHLENBQUMsQ0FBQTtHQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxNQUFNLEdBQUMsVUFBUyxDQUFDLEVBQUM7QUFBQyxRQUFHLENBQUMsSUFBSSxTQUFTLEVBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxRQUFRLEVBQUMsQ0FBQyxDQUFDLENBQUMsS0FBSyxJQUFHLElBQUksQ0FBQyxNQUFNLEVBQUM7QUFBQyxVQUFJLENBQUMsR0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQyxrQkFBa0IsRUFBRSxJQUFFLElBQUksQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsTUFBTSxFQUFFLENBQUE7S0FBQyxPQUFPLElBQUksQ0FBQTtHQUFDLENBQUE7Q0FBQyxDQUFBLENBQUMsS0FBSyxDQUFDLEVBQUMsQ0FBQSxVQUFTLENBQUMsRUFBQztBQUFDLGFBQVcsSUFBRSxFQUFFLElBQUUsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsS0FBSyxFQUFDLEVBQUMsQ0FBQyxFQUFDLFdBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLGFBQU8sQ0FBQyxHQUFDLENBQUMsSUFBRSxFQUFFLEVBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFDLENBQUMsQ0FBQyxRQUFRLEdBQUMsQ0FBQyxJQUFFLEVBQUUsRUFBQyxDQUFDLENBQUMsR0FBRyxHQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQTtLQUFDLEVBQUMsR0FBRyxFQUFDLGFBQVMsQ0FBQyxFQUFDO0FBQUMsYUFBTSxPQUFPLEtBQUcsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsSUFBRSxLQUFLLElBQUcsQ0FBQyxDQUFBO0tBQUMsRUFBQyxDQUFDLENBQUMsSUFBRztBQUFDLG9CQUFnQixDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUE7R0FBQyxDQUFBLE9BQU0sQ0FBQyxFQUFDO0FBQUMsUUFBSSxDQUFDLEdBQUMsZ0JBQWdCLENBQUMsTUFBTSxDQUFDLGdCQUFnQixHQUFDLFVBQVMsQ0FBQyxFQUFDO0FBQUMsVUFBRztBQUFDLGVBQU8sQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFBO09BQUMsQ0FBQSxPQUFNLENBQUMsRUFBQztBQUFDLGVBQU8sSUFBSSxDQUFBO09BQUM7S0FBQyxDQUFBO0dBQUM7Q0FBQyxDQUFBLENBQUMsS0FBSyxDQUFDLENBQUM7Ozs7Ozs7O0FBUTkrd0IsQ0FBQyxDQUFBLFVBQVMsQ0FBQyxFQUFDO0FBQUMsV0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsS0FBSyxFQUFFLElBQUUsQ0FBQyxDQUFDLENBQUMsTUFBTSxFQUFFLENBQUEsQUFBQyxJQUFFLE1BQU0sS0FBRyxDQUFDLENBQUMsR0FBRyxDQUFDLFNBQVMsQ0FBQyxDQUFBO0dBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLEtBQUMsR0FBQyxDQUFDLENBQUMsT0FBTyxDQUFDLE9BQU8sRUFBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUM7UUFBQyxDQUFDO1FBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFHLENBQUMsS0FBRyxDQUFDLEdBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUEsQUFBQyxFQUFDO0FBQUMsVUFBSSxDQUFDLEdBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLEdBQUMsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxjQUFjLEVBQUMsRUFBRSxDQUFDLEdBQUMsQ0FBQyxDQUFBO0tBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQTtHQUFDLElBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxLQUFLO01BQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxHQUFHO01BQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxPQUFPO01BQUMsQ0FBQyxHQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLEdBQUMsRUFBQyxPQUFPLEVBQUMsbUJBQVU7QUFBQyxhQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsR0FBQyxJQUFJLEdBQUMsS0FBSyxDQUFDLENBQUE7S0FBQyxFQUFDLE1BQU0sRUFBQyxrQkFBVTtBQUFDLGFBQU8sQ0FBQyxDQUFDLElBQUksQ0FBQyxHQUFDLEtBQUssQ0FBQyxHQUFDLElBQUksQ0FBQTtLQUFDLEVBQUMsUUFBUSxFQUFDLG9CQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsUUFBUSxHQUFDLElBQUksR0FBQyxLQUFLLENBQUMsQ0FBQTtLQUFDLEVBQUMsT0FBTyxFQUFDLG1CQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsT0FBTyxHQUFDLElBQUksR0FBQyxLQUFLLENBQUMsQ0FBQTtLQUFDLEVBQUMsTUFBTSxFQUFDLGtCQUFVO0FBQUMsYUFBTyxJQUFJLENBQUMsVUFBVSxDQUFBO0tBQUMsRUFBQyxLQUFLLEVBQUMsZUFBUyxDQUFDLEVBQUM7QUFBQyxhQUFPLENBQUMsS0FBRyxDQUFDLEdBQUMsSUFBSSxHQUFDLEtBQUssQ0FBQyxDQUFBO0tBQUMsRUFBQyxJQUFJLEVBQUMsY0FBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLEtBQUcsQ0FBQyxDQUFDLE1BQU0sR0FBQyxDQUFDLEdBQUMsSUFBSSxHQUFDLEtBQUssQ0FBQyxDQUFBO0tBQUMsRUFBQyxFQUFFLEVBQUMsWUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLGFBQU8sQ0FBQyxLQUFHLENBQUMsR0FBQyxJQUFJLEdBQUMsS0FBSyxDQUFDLENBQUE7S0FBQyxFQUFDLFFBQVEsRUFBQyxrQkFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLGFBQU8sQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLElBQUksRUFBRSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUMsR0FBQyxJQUFJLEdBQUMsS0FBSyxDQUFDLENBQUE7S0FBQyxFQUFDLEdBQUcsRUFBQyxhQUFTLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsYUFBTyxDQUFDLENBQUMsR0FBRyxDQUFDLElBQUksRUFBQyxDQUFDLENBQUMsQ0FBQyxNQUFNLEdBQUMsSUFBSSxHQUFDLEtBQUssQ0FBQyxDQUFBO0tBQUMsRUFBQztNQUFDLENBQUMsR0FBQyxJQUFJLE1BQU0sQ0FBQyxvQ0FBb0MsQ0FBQztNQUFDLENBQUMsR0FBQyxPQUFPO01BQUMsQ0FBQyxHQUFDLE9BQU8sR0FBRSxDQUFDLElBQUksSUFBSSxFQUFBLENBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxXQUFPLENBQUMsQ0FBQyxDQUFDLEVBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQztBQUFDLFVBQUc7QUFBQyxZQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBRSxDQUFDLEdBQUMsQ0FBQyxHQUFDLEdBQUcsR0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxLQUFHLENBQUMsR0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsR0FBQyxHQUFHLEdBQUMsQ0FBQyxHQUFDLEdBQUcsR0FBQyxDQUFDLENBQUEsQUFBQyxDQUFDLElBQUksQ0FBQyxHQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7T0FBQyxDQUFBLE9BQU0sQ0FBQyxFQUFDO0FBQUMsZUFBTSxPQUFPLENBQUMsS0FBSyxDQUFDLCtCQUErQixFQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsQ0FBQSxDQUFBO09BQUMsU0FBTztBQUFDLFNBQUMsSUFBRSxDQUFDLENBQUMsV0FBVyxDQUFDLENBQUMsQ0FBQyxDQUFBO09BQUMsT0FBTyxDQUFDLEdBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsR0FBRyxDQUFDLENBQUMsRUFBQyxVQUFTLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxlQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUMsQ0FBQyxDQUFDLENBQUE7T0FBQyxDQUFDLENBQUMsR0FBQyxDQUFDLENBQUE7S0FBQyxDQUFDLENBQUE7R0FBQyxFQUFDLENBQUMsQ0FBQyxPQUFPLEdBQUMsVUFBUyxDQUFDLEVBQUMsQ0FBQyxFQUFDO0FBQUMsV0FBTyxDQUFDLENBQUMsQ0FBQyxFQUFDLFVBQVMsQ0FBQyxFQUFDLENBQUMsRUFBQyxDQUFDLEVBQUM7QUFBQyxhQUFNLENBQUMsQ0FBQyxDQUFDLElBQUUsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLENBQUMsQ0FBQSxLQUFJLENBQUMsQ0FBQyxJQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxFQUFDLElBQUksRUFBQyxDQUFDLENBQUMsS0FBRyxDQUFDLENBQUEsQUFBQyxDQUFBO0tBQUMsQ0FBQyxDQUFBO0dBQUMsQ0FBQTtDQUFDLENBQUEsQ0FBQyxLQUFLLENBQUMsQ0FBQyIsImZpbGUiOiJnZW5lcmF0ZWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uIGUodCxuLHIpe2Z1bmN0aW9uIHMobyx1KXtpZighbltvXSl7aWYoIXRbb10pe3ZhciBhPXR5cGVvZiByZXF1aXJlPT1cImZ1bmN0aW9uXCImJnJlcXVpcmU7aWYoIXUmJmEpcmV0dXJuIGEobywhMCk7aWYoaSlyZXR1cm4gaShvLCEwKTt2YXIgZj1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK28rXCInXCIpO3Rocm93IGYuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixmfXZhciBsPW5bb109e2V4cG9ydHM6e319O3Rbb11bMF0uY2FsbChsLmV4cG9ydHMsZnVuY3Rpb24oZSl7dmFyIG49dFtvXVsxXVtlXTtyZXR1cm4gcyhuP246ZSl9LGwsbC5leHBvcnRzLGUsdCxuLHIpfXJldHVybiBuW29dLmV4cG9ydHN9dmFyIGk9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtmb3IodmFyIG89MDtvPHIubGVuZ3RoO28rKylzKHJbb10pO3JldHVybiBzfSkiLCIvKipcclxuICogaWYgdGhpcyBwb3B1cCBpcyBvdXRzaWRlIG9mIGl0J3MgcGFyZW50LCBudWRnZSBpdCBiYWNrIGluXHJcbiAqIEBwYXJhbSAge2VsZW1lbnR9IHBvcHVwOiBET00gZWxtZW5ldCBvZiB0aGUgcG9wdXAgdG8gYmUgcGxhY2VkXHJcbiAqIEBwYXJhbSAge251bWJlcn0gdG9wOiBUaGUgdG9wIGNvb3JkaW5hdGUgb2Ygd2hlcmUgdGhlIHBvcHVwIHNob3VsZCBwb2ludFxyXG4gKiBAcGFyYW0gIHtudW1iZXJ9IGxlZnQ6IFRoZSBsZWZ0IGNvb3JkaW5hdGUgb2Ygd2hlcmUgdGhlIHBvcHVwIHNob3VsZCBwb2ludFxyXG4gKiBAcGFyYW0gIHtudW1iZXJ9IG9mZnNldDogYW4gb2ZmZXQgdG8gYmUgYWRkZWQgdG8gdG9wL2JvdHRvbSBvciBsZWZ0L3JpZ2h0XHJcbiAqIEBwYXJhbSAge3N0cmluZ30gdHJpYW5nbGU6IFwidG9wXCIsIFwicmlnaHRcIiwgXCJib3R0b21cIiwgb3IgXCJsZWZ0XCJcclxuICogQHBhcmFtICB7bnVtYmVyfSB0cmlhbmdsZVNpemU6IHVzZWQgdG8gY2FsY3VsYXRlIHRoZSBwb3NpdGlvblxyXG4gKiBAcGFyYW0gIHtib29sZWFufSBmbGlwVG9Db250YWluOiB3aWxsIGZsaXAgdGhlIHBvcHVwIGlmIGl0IGdvZXMgb3V0c2lkZSB0aGUgcGFyZW50IGNvbnRhaW5lclxyXG4gKiBAcmV0dXJuIHtvYmplY3R9IHtcclxuICogICAgIHJlYWxUb3AgICAgICAgOiB3aXRoIG5vIG9mZnNldCBhZGp1c3RtZW50LCB0aGUgcG9wdXAgc2hvdWxkIGdvIGhlcmUsIGJhc2VkIG9uIHRyaWFuZ2xlU2lkZVxyXG4gKiAgICAgcmVhbExlZnQgICAgICA6IF5eXHJcbiAqICAgICBwb3B1cFRvcCAgICAgIDogd2l0aCBhZGp1c3RtZW50IHdoZW4gdGhlIHBvcHVwIGJ1dHRzIHVwIGFnYWlucyB0aGUgcGFyZW50XHJcbiAqICAgICBwb3B1cExlZnQgICAgIDogXl5cclxuICogICAgIG92ZXJmbG93ICAgICAgOiBcInRvcFwiLCBcInJpZ2h0XCIsIFwiYm90dG9tXCIsIFwibGVmdFwiLiAgUG9zaXRpdmUgbnVtYmVycyBhcmUgb3ZlcmZsb3dzXHJcbiAqICAgICB0cmlhbmdsZU9mZnNldDogQW1vdW50IHRoZSB0cmlhbmdsZSBuZWVkcyB0byBtb3ZlIHRvIGJlIG9uIHRoZSBkb3QsIHJlbGF0aXZlIGZyb20gNTAlXHJcbiAqICAgICB0cmlhbmdsZVNpZGUgIDogV2lsbCBiZSB0aGUgc2FtZSBhcyBwYXNzZWQgaW4gdHJpYW5nbGUsIHVubGVzcyBpdCBmbGlwZWQgdmlhIGZsaXBUb0NvbnRhaW5cclxuICogfVxyXG4gKiB1c2UgcG9wdXBUb3AsIHBvcHVwTGVmdCwgYW5kIHRyaWFuZ2xlT2Zmc2V0IHRvIHBvc2l0aW9uIHRoZSBwb3B1cFxyXG4gKi9cclxuZnVuY3Rpb24gY2FsY3VsYXRlUG9wdXBPZmZzZXRzKHtwb3B1cCwgdG9wLCBsZWZ0LCBvZmZzZXQgPSAwLCB0cmlhbmdsZSA9IFwiYm90dG9tXCIsIHRyaWFuZ2xlU2l6ZSwgZmxpcFRvQ29udGFpbiA9IGZhbHNlfSl7XHJcblxyXG5cclxuICAgIC8vIG1ha2UgYSBjb3B5IG9mIHRoaXNcclxuICAgIGxldCB0cmlhbmdsZVNpZGUgPSB0cmlhbmdsZTtcclxuXHJcbiAgICAvLyBnZXQgdGhlIHdpZHRoIGFuZCBoZWlnaHQgb2YgdGhpcyBwb3B1cCBmcm9tIHRoZSBET01cclxuICAgIGNvbnN0IHdpZHRoID0gcG9wdXAub2Zmc2V0V2lkdGg7XHJcbiAgICBjb25zdCBoZWlnaHQgPSBwb3B1cC5vZmZzZXRIZWlnaHQ7XHJcblxyXG4gICAgLy8gZ2V0IHRoZSB3aWR0aC9oZWlnaHQgb2YgdGhlIHBhcmVudCBjb250YWluZXIgZGl2XHJcbiAgICBjb25zdCBwYXJlbnQgPSBwb3B1cC5wYXJlbnROb2RlO1xyXG4gICAgY29uc3QgcGFyZW50V2lkdGggPSBwYXJlbnQuY2xpZW50V2lkdGg7XHJcbiAgICBjb25zdCBwYXJlbnRIZWlnaHQgPSBwYXJlbnQub2Zmc2V0SGVpZ2h0OyAvLyBjbGllbnQgaGVpZ2h0IG9mIGJvZHkgd2lsbCBvbmx5IGJlIHRoZSB2aWV3cG9ydCBoZWlnaHRcclxuXHJcbiAgICAvLyBjb21tb24gY2FsY3VsYXRpb25zXHJcbiAgICBjb25zdCBwb3B1cE9uVG9wID0gdG9wIC0gaGVpZ2h0IC0gdHJpYW5nbGVTaXplICsgb2Zmc2V0O1xyXG4gICAgY29uc3QgcG9wdXBPbkJvdHRvbSA9IHRvcCArIHRyaWFuZ2xlU2l6ZSAtIG9mZnNldDtcclxuICAgIGNvbnN0IHBvcHVwT25MZWZ0ID0gbGVmdCAtIHdpZHRoIC0gdHJpYW5nbGVTaXplICsgb2Zmc2V0O1xyXG4gICAgY29uc3QgcG9wdXBPblJpZ2h0ID0gbGVmdCArIHRyaWFuZ2xlU2l6ZSAtIG9mZnNldDtcclxuXHJcbiAgICAvLyBjYWxjdWxhdGUgd2hlcmUgdGhlIHRvcCBvZiB0aGUgcG9wdXAgc2hvdWxkIGJlIGJhc2VkIG9uIHRvcC9sZWZ0XHJcbiAgICBjb25zdCByZWFsVG9wID0gKHRyaWFuZ2xlU2lkZSA9PT0gXCJib3R0b21cIikgPyBwb3B1cE9uVG9wXHJcbiAgICAgICAgICAgICAgICAgIDogKHRyaWFuZ2xlU2lkZSA9PT0gXCJ0b3BcIikgICAgPyBwb3B1cE9uQm90dG9tXHJcbiAgICAgICAgICAgICAgICAgIDogdG9wIC0gaGVpZ2h0LzI7IC8vICBsZWZ0IG9yIHJpZ2h0XHJcblxyXG4gICAgY29uc3QgcmVhbExlZnQgPSAodHJpYW5nbGVTaWRlID09PSBcInJpZ2h0XCIpID8gcG9wdXBPbkxlZnRcclxuICAgICAgICAgICAgICAgICAgIDogKHRyaWFuZ2xlU2lkZSA9PT0gXCJsZWZ0XCIpICA/IHBvcHVwT25SaWdodFxyXG4gICAgICAgICAgICAgICAgICAgOiBsZWZ0IC0gKHdpZHRoLzIpOyAvLyBjZW50ZXJcclxuXHJcbiAgICAvLyB0aGUgYW1vdW50cyB0aGF0IHRoaXMgcG9wdXAgaXMgb3V0c2lkZSBvZiBpdCdzIHBhcmVudC5cclxuICAgIGNvbnN0IG92ZXJmbG93ID0ge1xyXG4gICAgICAgIHRvcDogLXJlYWxUb3AsXHJcbiAgICAgICAgcmlnaHQ6IC0ocGFyZW50V2lkdGggLSAocmVhbExlZnQgKyB3aWR0aCkpLFxyXG4gICAgICAgIGJvdHRvbTogLShwYXJlbnRIZWlnaHQgLSAocmVhbFRvcCArIGhlaWdodCkpLFxyXG4gICAgICAgIGxlZnQ6IC1yZWFsTGVmdFxyXG4gICAgfTtcclxuXHJcblxyXG4gICAgLy8gY2FsY3VsYXRlIHdoZXJlIHRoZSBwb3B1cCBzaG91bGQgZ29cclxuICAgIC8vIHN0YXJ0IHdpdGggcG9wdXBMZWZ0IGFzIHJlYWxMZWZ0IGJlZm9yZSBudWRnaW5nXHJcbiAgICBsZXQgcG9wdXBUb3AgPSByZWFsVG9wO1xyXG4gICAgbGV0IHBvcHVwTGVmdCA9IHJlYWxMZWZ0O1xyXG4gICAgbGV0IHRyaWFuZ2xlT2Zmc2V0ID0gMDtcclxuXHJcblxyXG4gICAgLy8gaWYgdGhlcmUgaXMgYW4gb3ZlcmZsb3cgb24gdGhlIHJpZ2h0LCBhZGp1c3QgdGhlIHBvcHVwIGFuZCB0cmlhbmdsZSBwb3NpdGlvblxyXG4gICAgaWYgKG92ZXJmbG93LnJpZ2h0ID4gMCl7XHJcbiAgICAgICAgaWYgKHRyaWFuZ2xlU2lkZSA9PT0gXCJ0b3BcIiB8fCB0cmlhbmdsZVNpZGUgPT09IFwiYm90dG9tXCIpe1xyXG4gICAgICAgICAgICBwb3B1cExlZnQgPSByZWFsTGVmdCAtIG92ZXJmbG93LnJpZ2h0O1xyXG4gICAgICAgICAgICB0cmlhbmdsZU9mZnNldCA9IG92ZXJmbG93LnJpZ2h0O1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgLy8gZm9yIGxlZnQsIGZsaXAgdGhlIHBvcHVwXHJcbiAgICAgICAgaWYgKHRyaWFuZ2xlU2lkZSA9PT0gXCJsZWZ0XCIgJiYgZmxpcFRvQ29udGFpbil7XHJcbiAgICAgICAgICAgIHRyaWFuZ2xlU2lkZSA9IFwicmlnaHRcIjtcclxuICAgICAgICAgICAgcG9wdXBMZWZ0ID0gcG9wdXBPbkxlZnQ7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIC8vIGlmIHRoZXJlIGlzIGFuIG92ZXJmbG93IG9uIHRoZSBsZWZ0LCBhZGp1c3QgdGhlIHBvcHVwIGFuZCB0cmlhbmdsZSBwb3NpdGlvblxyXG4gICAgaWYgKG92ZXJmbG93LmxlZnQgPiAwKXtcclxuICAgICAgICBpZiAodHJpYW5nbGVTaWRlID09PSBcInRvcFwiIHx8IHRyaWFuZ2xlU2lkZSA9PT0gXCJib3R0b21cIil7XHJcbiAgICAgICAgICAgIHBvcHVwTGVmdCA9IHJlYWxMZWZ0ICsgb3ZlcmZsb3cubGVmdDtcclxuICAgICAgICAgICAgdHJpYW5nbGVPZmZzZXQgPSAtb3ZlcmZsb3cubGVmdDtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC8vIGZvciByaWdodCwgZmxpcCB0aGUgcG9wdXBcclxuICAgICAgICBpZiAodHJpYW5nbGVTaWRlID09PSBcInJpZ2h0XCIgJiYgZmxpcFRvQ29udGFpbil7XHJcbiAgICAgICAgICAgIHRyaWFuZ2xlU2lkZSA9IFwibGVmdFwiO1xyXG4gICAgICAgICAgICBwb3B1cExlZnQgPSBwb3B1cE9uUmlnaHQ7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIC8vIGlmIHRoZXJlIGlzIGFuIG92ZXJmbG93IG9uIHRoZSBib3R0b21cclxuICAgIGlmIChvdmVyZmxvdy5ib3R0b20gPiAwKXtcclxuICAgICAgICAvLyBmb3IgbGVmdC9yaWdodCwgYnV0dCB0aGUgcG9wdXAgYWdhaW5zdCB0aGUgYm90dG9tXHJcbiAgICAgICAgaWYgKHRyaWFuZ2xlU2lkZSA9PT0gXCJsZWZ0XCIgfHwgdHJpYW5nbGVTaWRlID09PSBcInJpZ2h0XCIpIHtcclxuICAgICAgICAgICAgcG9wdXBUb3AgPSByZWFsVG9wIC0gb3ZlcmZsb3cuYm90dG9tO1xyXG4gICAgICAgICAgICB0cmlhbmdsZU9mZnNldCA9IG92ZXJmbG93LmJvdHRvbTtcclxuICAgICAgICB9XHJcbiAgICAgICAgLy8gZm9yIHRvcCwgZmxpcCB0aGUgcG9wdXBcclxuICAgICAgICBpZiAodHJpYW5nbGVTaWRlID09PSBcInRvcFwiICYmIGZsaXBUb0NvbnRhaW4pe1xyXG4gICAgICAgICAgICB0cmlhbmdsZVNpZGUgPSBcImJvdHRvbVwiO1xyXG4gICAgICAgICAgICBwb3B1cFRvcCA9IHBvcHVwT25Ub3A7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIC8vIGlmIHRoZXJlIGlzIGFuIG92ZXJmbG93IG9uIHRoZSB0b3BcclxuICAgIGlmIChvdmVyZmxvdy50b3AgPiAwKXtcclxuXHJcbiAgICAgICAgaWYgKHRyaWFuZ2xlU2lkZSA9PT0gXCJsZWZ0XCIgfHwgdHJpYW5nbGVTaWRlID09PSBcInJpZ2h0XCIpIHtcclxuICAgICAgICAgICAgcG9wdXBUb3AgPSByZWFsVG9wICsgb3ZlcmZsb3cudG9wO1xyXG4gICAgICAgICAgICB0cmlhbmdsZU9mZnNldCA9IC1vdmVyZmxvdy50b3A7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAvLyBmb3IgYm90dG9tLCBmbGlwIHRoZSBwb3B1cFxyXG4gICAgICAgIGlmICh0cmlhbmdsZVNpZGUgPT09IFwiYm90dG9tXCIgJiYgZmxpcFRvQ29udGFpbil7XHJcbiAgICAgICAgICAgIHRyaWFuZ2xlU2lkZSA9IFwidG9wXCI7XHJcbiAgICAgICAgICAgIHBvcHVwVG9wID0gcG9wdXBPbkJvdHRvbTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgLy8gcmV0dXJuIGFsbCB0aGUgbWVhc3VyZW1lbnRzXHJcbiAgICByZXR1cm4ge1xyXG4gICAgICAgIHJlYWxUb3AsIHJlYWxMZWZ0LCBwb3B1cFRvcCwgcG9wdXBMZWZ0LCBvdmVyZmxvdywgdHJpYW5nbGVPZmZzZXQsIHRyaWFuZ2xlU2lkZVxyXG4gICAgfTtcclxuXHJcbn1cclxuXHJcbmV4cG9ydCBkZWZhdWx0IGNhbGN1bGF0ZVBvcHVwT2Zmc2V0cztcclxuIiwidmFyIGFydGljbGVTaWRlYmFyQWQsXHJcbiAgICBhcnRpY2xlU2lkZWJhckFkUGFyZW50LFxyXG4gICAgbGFzdEFjdGlvbkZsYWdzQmFyLFxyXG4gICAgc3RpY2t5Rmxvb3IsXHJcbiAgICBzaWRlYmFySXNUYWxsZXI7XHJcbiQoZG9jdW1lbnQpLnJlYWR5KGZ1bmN0aW9uICgpIHtcclxuICAgIGFydGljbGVTaWRlYmFyQWRQYXJlbnQgPSAkKCcuYXJ0aWNsZS1yaWdodC1yYWlsIHNlY3Rpb246bGFzdC1jaGlsZCcpO1xyXG4gICAgYXJ0aWNsZVNpZGViYXJBZCA9IGFydGljbGVTaWRlYmFyQWRQYXJlbnQuZmluZCgnLmFkdmVydGlzaW5nJyk7XHJcbiAgICBsYXN0QWN0aW9uRmxhZ3NCYXIgPSAkKCcuYWN0aW9uLWZsYWdzLWJhcjpsYXN0LW9mLXR5cGUnKTtcclxuICAgIHNpZGViYXJJc1RhbGxlciA9ICQoJy5hcnRpY2xlLXJpZ2h0LXJhaWwnKS5oZWlnaHQoKSA+ICQoJy5hcnRpY2xlLWxlZnQtcmFpbCcpLmhlaWdodCgpO1xyXG59KTtcclxuJCh3aW5kb3cpLm9uKCdzY3JvbGwnLCBmdW5jdGlvbiAoKSB7XHJcbiAgICBpZiAoYXJ0aWNsZVNpZGViYXJBZFBhcmVudCAmJiBhcnRpY2xlU2lkZWJhckFkUGFyZW50Lmxlbmd0aCAmJiAhc2lkZWJhcklzVGFsbGVyKSB7XHJcbiAgICAgICAgLy8gcGFnZVlPZmZzZXQgaW5zdGVhZCBvZiBzY3JvbGxZIGZvciBJRSAvIHByZS1FZGdlIGNvbXBhdGliaWxpdHlcclxuICAgICAgICBzdGlja3lGbG9vciA9IGxhc3RBY3Rpb25GbGFnc0Jhci5vZmZzZXQoKS50b3AgLSB3aW5kb3cucGFnZVlPZmZzZXQgLSBhcnRpY2xlU2lkZWJhckFkLmhlaWdodCgpO1xyXG4gICAgICAgIGlmIChhcnRpY2xlU2lkZWJhckFkUGFyZW50Lm9mZnNldCgpLnRvcCAtIHdpbmRvdy5wYWdlWU9mZnNldCA8PSAxNikge1xyXG4gICAgICAgICAgICBhcnRpY2xlU2lkZWJhckFkUGFyZW50LmFkZENsYXNzKCdhZHZlcnRpc2luZy0tc3RpY2t5Jyk7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgYXJ0aWNsZVNpZGViYXJBZFBhcmVudC5yZW1vdmVDbGFzcygnYWR2ZXJ0aXNpbmctLXN0aWNreScpO1xyXG4gICAgICAgIH1cclxuICAgICAgICBpZiAoc3RpY2t5Rmxvb3IgPD0gNDApIHtcclxuICAgICAgICAgICAgYXJ0aWNsZVNpZGViYXJBZC5jc3MoJ3RvcCcsIChzdGlja3lGbG9vciAtIDQwKSArICdweCcpO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIGFydGljbGVTaWRlYmFyQWQuY3NzKCd0b3AnLCAnJyk7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG59KTsiLCIvKiBnbG9iYWwgYW5hbHl0aWNzRXZlbnQsIGFuYWx5dGljc19kYXRhLCBhbmd1bGFyICovXHJcbmltcG9ydCBGb3JtQ29udHJvbGxlciBmcm9tICcuLi9jb250cm9sbGVycy9mb3JtLWNvbnRyb2xsZXInO1xyXG5pbXBvcnQgQ29va2llcyBmcm9tICcuLi9qc2Nvb2tpZSc7XHJcbmltcG9ydCB7IGFuYWx5dGljc0V2ZW50IH0gZnJvbSAnLi4vY29udHJvbGxlcnMvYW5hbHl0aWNzLWNvbnRyb2xsZXInO1xyXG5cclxuLyogKiAqXHJcblNBVkUgU0VBUkNIXHJcblRoaXMgY29tcG9uZW50IGhhbmRsZXMgc2F2aW5nIHNlYXJjaGVzIGZyb20gdGhlIFNlYXJjaCBwYWdlLCBhcyB3ZWxsIGFzIHNldHRpbmcgYWxlcnRzXHJcbmZvciB0b3BpY3MgZnJvbSBIb21lL1RvcGljIHBhZ2VzLiBEaXNwaXRlIHRoZSBuYW1pbmcgZGlmZmVyZW5jZXMsIHRoZSBiYWNrLWVuZCBmdW5jdGlvbmFsaXR5XHJcbmlzIHRoZSBzYW1lIC0gdG9waWMgYWxlcnRzIGFyZSBhY3R1YWxseSBqdXN0IHNhdmVkIHNlYXJjaGVzIGZvciB0aGUgdG9waWMsXHJcbnBsdXMgYW4gZW1haWwgYWxlcnQgZm9yIG5ldyBhcnRpY2xlcy5cclxuKiAqICovXHJcblxyXG5mdW5jdGlvbiBnZXRQYXJhbWV0ZXJCeU5hbWUobmFtZSwgdXJsKSB7XHJcbiAgICBpZiAoIXVybCkge1xyXG4gICAgICAgIHVybCA9IHdpbmRvdy5sb2NhdGlvbi5ocmVmO1xyXG4gICAgfVxyXG4gICAgbmFtZSA9IG5hbWUucmVwbGFjZSgvW1xcW1xcXV0vZywgXCJcXFxcJCZcIik7XHJcbiAgICB2YXIgcmVnZXggPSBuZXcgUmVnRXhwKFwiWz8mXVwiICsgbmFtZSArIFwiKD0oW14mI10qKXwmfCN8JClcIiksXHJcbiAgICAgICAgcmVzdWx0cyA9IHJlZ2V4LmV4ZWModXJsKTtcclxuICAgIGlmICghcmVzdWx0cykgcmV0dXJuIG51bGw7XHJcbiAgICBpZiAoIXJlc3VsdHNbMl0pIHJldHVybiAnJztcclxuICAgIHJldHVybiBkZWNvZGVVUklDb21wb25lbnQocmVzdWx0c1syXS5yZXBsYWNlKC9cXCsvZywgXCIgXCIpKTtcclxufVxyXG5cclxuJChkb2N1bWVudCkucmVhZHkoZnVuY3Rpb24oKSB7XHJcblxyXG5cdC8vIFdoZW4gdGhlIFNhdmUgU2VhcmNoIHBvcC1vdXQgaXMgdG9nZ2xlZCwgbmVlZCB0byB1cGRhdGUgc29tZSBmb3JtIGZpZWxkc1xyXG5cdC8vIHdpdGggdGhlIG1vc3QgcmVjZW50IGRhdGEuIFVzZWQgdG8gdXNlIEFuZ3VsYXIgZm9yIHRoaXMsIGJ1dCBmb3Igc2l0ZS13aWRlXHJcblx0Ly8gcmV1c2FiaWxpdHkgd2UgbmVlZCB0byBkbyBpdCBpbiBaZXB0by5cclxuXHQkKCcuanMtc2F2ZS1zZWFyY2gnKS5vbignY2xpY2snLCBmdW5jdGlvbihlKSB7XHJcblx0XHQkKCcuanMtc2F2ZS1zZWFyY2gtdXJsJykudmFsKHdpbmRvdy5sb2NhdGlvbi5wYXRobmFtZSArIHdpbmRvdy5sb2NhdGlvbi5oYXNoKTtcclxuXHRcdCQoJy5qcy1zYXZlLXNlYXJjaC10aXRsZScpLnZhbCgkKCcjanMtc2VhcmNoLWZpZWxkJykudmFsKCkpO1xyXG5cdH0pO1xyXG5cclxuXHQvLyBQb3B1bGF0ZXMgdG9waWMgYWxlcnQgZGF0YSB3aGVuIGEgdXNlciBpcyBsb2dnaW5nIGluIGFuZCBzYXZpbmcgc2ltdWx0YW5lb3VzbHlcclxuXHQkKCcuanMtdXBkYXRlLXRvcGljLWFsZXJ0Jykub24oJ2NsaWNrJywgZnVuY3Rpb24oZSkge1xyXG5cdFx0JCgnLmpzLXNhdmUtc2VhcmNoLXVybCcpLnZhbCgkKHRoaXMpLmRhdGEoJ3RvcGljLWFsZXJ0LXVybCcpKTtcclxuXHRcdC8vIFNlYXJjaC9Ub3BpYyB0aXRsZSBleGlzdHMgYXMgPGlucHV0PiBhbmQgPHNwYW4+LCBuZWVkcyB0d28gdGVjaG5pcXVlcyB0byBwcm9wZXJseVxyXG5cdFx0Ly8gdXBkYXRlIHRoZSB2YWx1ZXMuXHJcblx0XHQkKCcuanMtc2F2ZS1zZWFyY2gtdGl0bGUnKS52YWwoJCh0aGlzKS5kYXRhKCd0b3BpYy1hbGVydC10aXRsZScpKS5odG1sKCQodGhpcykuZGF0YSgndG9waWMtYWxlcnQtdGl0bGUnKSk7XHJcblx0fSk7XHJcblxyXG5cdCQoJy5qcy1zZXQtdG9waWMtYWxlcnQnKS5vbignY2xpY2snLCBmdW5jdGlvbihlKSB7XHJcblxyXG5cdFx0dmFyIGlzU2V0dGluZ0FsZXJ0ID0gISQodGhpcykuZGF0YSgnaGFzLXRvcGljLWFsZXJ0Jyk7XHJcblx0XHR2YXIgdG9waWNMYWJlbCA9ICQodGhpcykuZmluZCgnLmpzLXNldC10b3BpYy1sYWJlbCcpO1xyXG5cclxuXHRcdCQoJy5qcy1zYXZlLXNlYXJjaC11cmwnKS52YWwoJCh0aGlzKS5kYXRhKCd0b3BpYy1hbGVydC11cmwnKSk7XHJcblx0XHQkKCcuanMtc2F2ZS1zZWFyY2gtdGl0bGUnKS52YWwoJCh0aGlzKS5kYXRhKCd0b3BpYy1hbGVydC10aXRsZScpKTtcclxuXHJcblx0XHRpZihpc1NldHRpbmdBbGVydCkge1xyXG5cdFx0XHQkKCcuZm9ybS1zYXZlLXNlYXJjaCcpLmZpbmQoJ2J1dHRvblt0eXBlPXN1Ym1pdF0nKS5jbGljaygpO1xyXG5cdFx0XHR0b3BpY0xhYmVsLmh0bWwodG9waWNMYWJlbC5kYXRhKCdsYWJlbC1pcy1zZXQnKSk7XHJcblx0XHRcdCQodGhpcykuZGF0YSgnaGFzLXRvcGljLWFsZXJ0JywgJ3RydWUnKTtcclxuXHRcdFx0JCh0aGlzKS5maW5kKCcuanMtdG9waWMtaWNvbi11bnNldCcpLnJlbW92ZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuXHRcdFx0JCh0aGlzKS5maW5kKCcuanMtdG9waWMtaWNvbi1zZXQnKS5hZGRDbGFzcygnaXMtYWN0aXZlJyk7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHR3aW5kb3cubGlnaHRib3hDb250cm9sbGVyLnNob3dMaWdodGJveCgkKHRoaXMpKTtcclxuXHRcdH1cclxuXHJcblx0fSk7XHJcblxyXG5cdHZhciBzYXZlZFNlYXJjaCA9IGdldFBhcmFtZXRlckJ5TmFtZShcInNzXCIpO1xyXG5cdGlmIChzYXZlZFNlYXJjaCAhPSBudWxsICYmIHNhdmVkU2VhcmNoID09IFwidHJ1ZVwiKSB7XHJcblx0ICAgICQoJy5qcy1zYXZlZC1zZWFyY2gtc3VjY2Vzcy1hbGVydCcpXHJcblx0XHRcdFx0LmFkZENsYXNzKCdpcy1hY3RpdmUnKVxyXG5cdFx0XHRcdC5vbignYW5pbWF0aW9uZW5kJywgZnVuY3Rpb24gKGUpIHtcclxuXHRcdFx0XHQgICAgJChlLnRhcmdldCkucmVtb3ZlQ2xhc3MoJ2lzLWFjdGl2ZScpO1xyXG5cdFx0XHRcdH0pLmFkZENsYXNzKCdhLWZhZGUtYWxlcnQnKTtcclxuXHR9XHJcblxyXG5cdHZhciByZW1vdmVUb3BpY0FsZXJ0ID0gbmV3IEZvcm1Db250cm9sbGVyKHtcclxuXHRcdG9ic2VydmU6ICcuZm9ybS1yZW1vdmUtdG9waWMtYWxlcnQnLFxyXG5cdFx0c3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbihmb3JtLCBjb250ZXh0LCBldmVudCkge1xyXG5cdFx0XHQkKGZvcm0pLmZpbmQoJy5qcy1zZXQtdG9waWMtbGFiZWwnKS5odG1sKCQoZm9ybSkuZmluZCgnLmpzLXNldC10b3BpYy1sYWJlbCcpLmRhdGEoJ2xhYmVsLW5vdC1zZXQnKSk7XHJcblx0XHRcdCQoZm9ybSkuZmluZCgnLmpzLXNldC10b3BpYy1hbGVydCcpLmRhdGEoJ2hhcy10b3BpYy1hbGVydCcsIG51bGwpO1xyXG5cdFx0XHQkKGZvcm0pLmZpbmQoJy5qcy10b3BpYy1pY29uLXVuc2V0JykuYWRkQ2xhc3MoJ2lzLWFjdGl2ZScpO1xyXG5cdFx0XHQkKGZvcm0pLmZpbmQoJy5qcy10b3BpYy1pY29uLXNldCcpLnJlbW92ZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuXHJcblx0XHRcdGFuYWx5dGljc0V2ZW50KFx0JC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsICQoZm9ybSkuZGF0YSgnYW5hbHl0aWNzJykpICk7XHJcblxyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHR2YXIgc2F2ZVNlYXJjaENvbnRyb2xsZXIgPSBuZXcgRm9ybUNvbnRyb2xsZXIoe1xyXG5cdFx0b2JzZXJ2ZTogJy5mb3JtLXNhdmUtc2VhcmNoJyxcclxuXHRcdHN1Y2Nlc3NDYWxsYmFjazogZnVuY3Rpb24oZm9ybSwgY29udGV4dCwgZXZlbnQpIHtcclxuXHJcblx0XHRcdC8vIElmIHRoZXJlJ3MgYSBzdGFzaGVkIHNlYXJjaCwgcmVtb3ZlIGl0LlxyXG5cdFx0XHRDb29raWVzLnJlbW92ZSgnc2F2ZVN0YXNoZWRTZWFyY2gnKTtcclxuXHJcblx0XHRcdHdpbmRvdy5jb250cm9sUG9wT3V0cy5jbG9zZVBvcE91dCgkKGZvcm0pLmNsb3Nlc3QoJy5wb3Atb3V0JykpO1xyXG5cdFx0XHQkKCcuanMtc2F2ZWQtc2VhcmNoLXN1Y2Nlc3MtYWxlcnQnKVxyXG5cdFx0XHRcdC5hZGRDbGFzcygnaXMtYWN0aXZlJylcclxuXHRcdFx0XHQub24oJ2FuaW1hdGlvbmVuZCcsIGZ1bmN0aW9uKGUpIHtcclxuXHRcdFx0XHRcdCQoZS50YXJnZXQpLnJlbW92ZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuXHRcdFx0XHR9KS5hZGRDbGFzcygnYS1mYWRlLWFsZXJ0Jyk7XHJcblxyXG5cdFx0XHR3aW5kb3cubGlnaHRib3hDb250cm9sbGVyLmNsb3NlTGlnaHRib3hNb2RhbCgpO1xyXG5cclxuXHRcdFx0aWYodHlwZW9mIGFuZ3VsYXIgIT09ICd1bmRlZmluZWQnKSB7XHJcblx0XHRcdFx0YW5ndWxhci5lbGVtZW50KCQoJy5qcy1zYXZlZC1zZWFyY2gtY29udHJvbGxlcicpWzBdKS5jb250cm9sbGVyKCkuc2VhcmNoSXNTYXZlZCgpO1xyXG5cdFx0XHR9XHJcblxyXG5cdFx0XHR2YXIgZXZlbnRfZGF0YSA9IHt9O1xyXG5cclxuXHRcdFx0aWYoJChmb3JtKS5kYXRhKCdpcy1zZWFyY2gnKSA9PT0gdHJ1ZSkge1xyXG5cdFx0XHRcdGV2ZW50X2RhdGEuZXZlbnRfbmFtZSA9IFwidG9vbGJhcl91c2VcIjtcclxuXHRcdFx0XHRldmVudF9kYXRhLnRvb2xiYXJfdG9vbCA9IFwic2F2ZV9zZWFyY2hcIjtcclxuXHRcdFx0fSBlbHNlIHtcclxuXHRcdFx0XHRldmVudF9kYXRhLmV2ZW50X25hbWUgPSBcInNldF9hbGVydFwiO1xyXG5cdFx0XHRcdGV2ZW50X2RhdGEuYWxlcnRfdG9waWMgPSAkKGZvcm0pLmZpbmQoJy5qcy1zYXZlLXNlYXJjaC10aXRsZScpLnZhbCgpO1xyXG5cdFx0XHR9XHJcblxyXG5cdFx0XHRhbmFseXRpY3NFdmVudChcdCQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCBldmVudF9kYXRhKSApO1xyXG5cclxuXHRcdH0sXHJcblx0XHRiZWZvcmVSZXF1ZXN0OiBmdW5jdGlvbihmb3JtKSB7XHJcblx0XHRcdGlmKCEkKGZvcm0pLmZpbmQoJy5qcy1zYXZlLXNlYXJjaC10aXRsZScpLnZhbCgpLnRyaW0oKSkge1xyXG5cdFx0XHRcdCQoJy5qcy1mb3JtLWVycm9yLUVtcHR5VGl0bGUnKS5zaG93KCk7XHJcblx0XHRcdH1cclxuXHRcdH1cclxuXHR9KTtcclxuXHJcblx0dmFyIHNhdmVTZWFyY2hMb2dpbkNvbnRyb2xsZXIgPSBuZXcgRm9ybUNvbnRyb2xsZXIoe1xyXG5cdFx0b2JzZXJ2ZTogJy5mb3JtLXNhdmUtc2VhcmNoLWxvZ2luJyxcclxuXHRcdHN1Y2Nlc3NDYWxsYmFjazogZnVuY3Rpb24oZm9ybSwgY29udGV4dCwgZXZlbnQpIHtcclxuXHRcdFx0Q29va2llcy5zZXQoJ3NhdmVTdGFzaGVkU2VhcmNoJywge1xyXG5cdFx0XHRcdCdUaXRsZSc6ICQoJy5qcy1zYXZlLXNlYXJjaC10aXRsZScpLnZhbCgpLFxyXG5cdFx0XHRcdCdVcmwnOiAkKCcuanMtc2F2ZS1zZWFyY2gtdXJsJykudmFsKCksXHJcblx0XHRcdFx0J0FsZXJ0RW5hYmxlZCc6ICQoJyNBbGVydEVuYWJsZWQnKS5wcm9wKCdjaGVja2VkJylcclxuXHRcdFx0fSk7XHJcblxyXG5cdFx0XHQkLmFqYXgoe1xyXG5cdFx0XHQgICAgdHlwZTogXCJQT1NUXCIsXHJcblx0XHRcdCAgICB1cmw6IFwiL2FwaS9TYXZlZFNlYXJjaGVzXCIsXHJcblx0XHRcdCAgICBkYXRhOiB7XHJcblx0XHRcdCAgICAgICAgdXJsOiAkKCcuanMtc2F2ZS1zZWFyY2gtdXJsJykudmFsKCksXHJcblx0XHRcdCAgICAgICAgdGl0bGU6ICQoJy5qcy1zYXZlLXNlYXJjaC10aXRsZScpLnZhbCgpLFxyXG5cdFx0XHQgICAgICAgIGFsZXJ0RW5hYmxlZDogJCgnI0FsZXJ0RW5hYmxlZCcpLnByb3AoJ2NoZWNrZWQnKVxyXG5cdFx0XHQgICAgfVxyXG5cdFx0XHR9KTtcclxuICAgICAgICAgICAgXHJcblx0XHRcdHZhciBsb2dpbkFuYWx5dGljcyA9ICB7XHJcblx0XHRcdFx0ZXZlbnRfbmFtZTogJ2xvZ2luJyxcclxuXHRcdFx0XHRsb2dpbl9zdGF0ZTogJ3N1Y2Nlc3NmdWwnLFxyXG5cdFx0XHRcdHVzZXJOYW1lOiAnXCInICsgJChmb3JtKS5maW5kKCdpbnB1dFtuYW1lPXVzZXJuYW1lXScpLnZhbCgpICsgJ1wiJ1xyXG5cdFx0XHR9O1xyXG5cdFx0XHRhbmFseXRpY3NFdmVudChcdCQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCBsb2dpbkFuYWx5dGljcykgKTtcclxuXHJcblx0XHRcdHZhciBzc1BhcmFtID0gZ2V0UGFyYW1ldGVyQnlOYW1lKFwic3NcIik7XHJcblx0XHRcdHZhciBzZWFyY2hWYWwgPSB3aW5kb3cubG9jYXRpb24uc2VhcmNoO1xyXG5cdFx0XHRpZiAoc3NQYXJhbSA9PSBudWxsKSB7XHJcblx0XHRcdCAgICBzZWFyY2hWYWwgPSAoc2VhcmNoVmFsLmxlbmd0aCA8IDEpIFxyXG5cdFx0XHQgICAgICAgID8gXCI/c3M9dHJ1ZVwiXHJcbiAgICAgICAgICAgICAgICAgICAgOiBzZWFyY2hWYWwgKyBcIiZzcz10cnVlXCI7XHJcblx0XHRcdH0gICAgICAgICAgICAgICAgICAgXHJcbiAgICAgICAgICAgIFxyXG5cdFx0XHRpZiAoc3NQYXJhbSA9PSB3aW5kb3cubG9jYXRpb24uc2VhcmNoKVxyXG5cdFx0XHQgICAgd2luZG93LmxvY2F0aW9uLnJlbG9hZCh0cnVlKTtcclxuXHRcdFx0ZWxzZSBcclxuXHRcdFx0ICAgIHdpbmRvdy5sb2NhdGlvbiA9IHdpbmRvdy5sb2NhdGlvbi5wYXRobmFtZSArIHNlYXJjaFZhbCArIHdpbmRvdy5sb2NhdGlvbi5oYXNoO1x0XHJcblx0XHR9XHJcblx0fSk7XHJcblxyXG5cdHZhciB0b2dnbGVTYXZlZFNlYXJjaEFsZXJ0Q29udHJvbGxlciA9IG5ldyBGb3JtQ29udHJvbGxlcih7XHJcblx0XHRvYnNlcnZlOiAnLmZvcm0tdG9nZ2xlLXNhdmVkLXNlYXJjaC1hbGVydCcsXHJcblx0XHRzdWNjZXNzQ2FsbGJhY2s6IGZ1bmN0aW9uKGZvcm0sIGNvbnRleHQsIGUpIHtcclxuXHRcdFx0dmFyIGFsZXJ0VG9nZ2xlID0gJChmb3JtKS5maW5kKCcuanMtc2F2ZWQtc2VhcmNoLWFsZXJ0LXRvZ2dsZScpO1xyXG5cdFx0XHR2YXIgdmFsID0gYWxlcnRUb2dnbGUudmFsKCk7XHJcblx0XHRcdHZhciBldmVudF9kYXRhID0ge1xyXG5cdFx0XHRcdHNhdmVkX3NlYXJjaF9hbGVydF90aXRsZTogJChmb3JtKS5kYXRhKCdhbmFseXRpY3MtdGl0bGUnKSxcclxuXHRcdFx0XHRzYXZlZF9zZWFyY2hfYWxlcnRfcHVibGljYXRpb246ICQoZm9ybSkuZGF0YSgnYW5hbHl0aWNzLXB1YmxpY2F0aW9uJylcclxuXHRcdFx0fTtcclxuXHJcblx0XHRcdGlmICh2YWwgPT09IFwib25cIikge1xyXG5cdFx0XHRcdGV2ZW50X2RhdGEuZXZlbnRfbmFtZSA9ICdzYXZlZF9zZWFyY2hfYWxlcnRfb2ZmJztcclxuXHRcdFx0XHRhbGVydFRvZ2dsZS52YWwoJ29mZicpO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdGV2ZW50X2RhdGEuZXZlbnRfbmFtZSA9ICdzYXZlZF9zZWFyY2hfYWxlcnRfb24nO1xyXG5cdFx0XHRcdGFsZXJ0VG9nZ2xlLnZhbCgnb24nKTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCBldmVudF9kYXRhKSApO1xyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHQkKCcuanMtc2F2ZWQtc2VhcmNoLWFsZXJ0LXRvZ2dsZScpLm9uKCdjbGljaycsIGZ1bmN0aW9uKGUpIHtcclxuXHRcdCQoZS50YXJnZXQuZm9ybSkuZmluZCgnYnV0dG9uW3R5cGU9c3VibWl0XScpLmNsaWNrKCk7XHJcblx0fSk7XHJcblxyXG5cdC8vIE9uIHBhZ2UgbG9hZCwgY2hlY2sgZm9yIGFueSBzdGFzaGVkIHNlYXJjaGVzIHRoYXQgbmVlZCB0byBiZSBzYXZlZFxyXG5cdHZhciBzYXZlU3Rhc2hlZFNlYXJjaCA9IENvb2tpZXMuZ2V0SlNPTignc2F2ZVN0YXNoZWRTZWFyY2gnKTtcclxuXHJcblx0aWYoc2F2ZVN0YXNoZWRTZWFyY2gpIHtcclxuXHRcdC8vIFNldCBgU2F2ZSBTZWFyY2hgIHZhbHVlcyBmcm9tIHN0YXNoZWQgc2VhcmNoIGRhdGFcclxuXHRcdCQoJy5qcy1zYXZlLXNlYXJjaC10aXRsZScpLnZhbChzYXZlU3Rhc2hlZFNlYXJjaFsnVGl0bGUnXSk7XHJcblx0XHQkKCcuanMtc2F2ZS1zZWFyY2gtdXJsJykudmFsKHNhdmVTdGFzaGVkU2VhcmNoWydVcmwnXSk7XHJcblx0XHQkKCcjQWxlcnRFbmFibGVkJykucHJvcCgnY2hlY2tlZCcsIHNhdmVTdGFzaGVkU2VhcmNoWydBbGVydEVuYWJsZWQnXSk7XHJcblxyXG5cdFx0Ly8gU2F2ZSB0aGUgc3Rhc2hlZCBzZWFyY2ggaWYgU2VhcmNoIChBbmd1bGFyKSBwYWdlXHJcblx0XHRpZih0eXBlb2YgYW5ndWxhciAhPT0gJ3VuZGVmaW5lZCcpIHtcclxuXHRcdFx0JCgnLmZvcm0tc2F2ZS1zZWFyY2gnKS5maW5kKCdidXR0b25bdHlwZT1zdWJtaXRdJykuY2xpY2soKTtcclxuXHRcdH0gZWxzZSB7XHJcblx0XHRcdCQoJy5qcy1zZXQtdG9waWMtYWxlcnQnKS5lYWNoKGZ1bmN0aW9uKGluZGV4LCBpdGVtKSB7XHJcblx0XHRcdFx0aWYoJChpdGVtKS5kYXRhKCd0b3BpYy1hbGVydC11cmwnKSA9PT0gc2F2ZVN0YXNoZWRTZWFyY2hbJ1VybCddKSB7XHJcblx0XHRcdFx0XHQkKGl0ZW0pLmNsaWNrKCk7XHJcblx0XHRcdFx0XHQvLyBJZiB0aGVyZSdzIGEgc3Rhc2hlZCBzZWFyY2gsIHJlbW92ZSBpdC5cclxuXHRcdFx0XHRcdENvb2tpZXMucmVtb3ZlKCdzYXZlU3Rhc2hlZFNlYXJjaCcpO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fSk7XHJcblx0XHR9XHJcblx0fVxyXG5cclxuXHJcblx0dmFyIHJlbW92ZVNhdmVkU2VhcmNoID0gbmV3IEZvcm1Db250cm9sbGVyKHtcclxuICAgICAgICBvYnNlcnZlOiAnLmZvcm0tcmVtb3ZlLXNhdmVkLXNlYXJjaCcsXHJcbiAgICAgICAgc3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbihmb3JtLCBjb250ZXh0LCBldnQpIHtcclxuICAgICAgICAgICAgJChldnQudGFyZ2V0KS5jbG9zZXN0KCd0cicpLnJlbW92ZSgpO1xyXG5cclxuICAgICAgICAgICAgd2luZG93LmNvbnRyb2xQb3BPdXRzLmNsb3NlUG9wT3V0KCQoZm9ybSkuY2xvc2VzdCgnLnBvcC1vdXQnKSk7XHJcbiAgICAgICAgICAgICQoJy5qcy1zYXZlZC1zZWFyY2gtc3VjY2Vzcy1hbGVydCcpXHJcblx0XHRcdFx0LmFkZENsYXNzKCdpcy1hY3RpdmUnKVxyXG5cdFx0XHRcdC5vbignYW5pbWF0aW9uZW5kJywgZnVuY3Rpb24gKGUpIHtcclxuXHRcdFx0XHQgICAgY29uc29sZS5sb2coXCJzYXZlIHNlYXJjaCBjb21wb25lbnQ6NlwiKTtcclxuXHRcdFx0XHQgICAgJChlLnRhcmdldCkucmVtb3ZlQ2xhc3MoJ2lzLWFjdGl2ZScpO1xyXG5cdFx0XHRcdH0pLmFkZENsYXNzKCdhLWZhZGUtYWxlcnQnKTtcclxuXHJcbiAgICAgICAgICAgIHdpbmRvdy5saWdodGJveENvbnRyb2xsZXIuY2xvc2VMaWdodGJveE1vZGFsKCk7XHJcblxyXG5cdFx0XHR2YXIgZXZlbnRfZGF0YSA9IHtcclxuXHRcdFx0XHRldmVudF9uYW1lOiAnc2F2ZWRfc2VhcmNoX2FsZXJ0X3JlbW92YWwnLFxyXG5cdFx0XHRcdHNhdmVkX3NlYXJjaF9hbGVydF90aXRsZTogJChmb3JtKS5kYXRhKCdhbmFseXRpY3MtdGl0bGUnKSxcclxuXHRcdFx0XHRzYXZlZF9zZWFyY2hfYWxlcnRfcHVibGljYXRpb246ICQoZm9ybSkuZGF0YSgnYW5hbHl0aWNzLXB1YmxpY2F0aW9uJylcclxuXHRcdFx0fTtcclxuXHJcblx0XHRcdGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgZXZlbnRfZGF0YSkgKTtcclxuXHJcbiAgICAgICAgfVxyXG4gICAgfSk7XHJcbn0pO1xyXG4iLCIvLyAqICogKlxyXG4vLyAgQU5BTFlUSUNTIENPTlRST0xMRVJcclxuLy8gIEZvciBlYXNlLW9mLXVzZSwgYmV0dGVyIERSWSwgYmV0dGVyIHByZXZlbnRpb24gb2YgSlMgZXJyb3JzIHdoZW4gYWRzIGFyZSBibG9ja2VkXHJcbi8vICogKiAqXHJcblxyXG5mdW5jdGlvbiBhbmFseXRpY3NFdmVudChkYXRhT2JqKSB7XHJcbiAgICBpZih0eXBlb2YgdXRhZyAhPT0gJ3VuZGVmaW5lZCcpIHtcclxuICAgICAgICB1dGFnLmxpbmsoZGF0YU9iaik7XHJcbiAgICB9XHJcbn07XHJcblxyXG5leHBvcnQgeyBhbmFseXRpY3NFdmVudCB9O1xyXG4iLCIvKiBnbG9iYWxzIGFuYWx5dGljc19kYXRhICovXHJcbmltcG9ydCB7IGFuYWx5dGljc0V2ZW50IH0gZnJvbSAnLi9hbmFseXRpY3MtY29udHJvbGxlcic7XHJcblxyXG5mdW5jdGlvbiBib29rbWFya0NvbnRyb2xsZXIoKSB7XHJcblxyXG4gICAgLy8gKiAqICpcclxuICAgIC8vICBBcnRpY2xlIGJvb2ttYXJraW5nIGxvZ2ljIGdvZXMgaGVyZVxyXG4gICAgLy8gKiAqICpcclxuICAgIHRoaXMudG9nZ2xlID0gZnVuY3Rpb24oZSkge1xyXG5cclxuICAgICAgICB2YXIgYm9va21hcmsgPSB7XHJcbiAgICAgICAgICAgIGVsbTogJChlKVxyXG4gICAgICAgIH07XHJcblxyXG4gICAgICAgIC8vIElEIG9mIHRoZSBhcnRpY2xlIHdlJ3JlIGJvb2ttYXJraW5nIG9yIHVuLWJvb2ttYXJraW5nXHJcbiAgICAgICAgYm9va21hcmsuaWQgPSBib29rbWFyay5lbG0uY2xvc2VzdCgnLmpzLWJvb2ttYXJrLWFydGljbGUnKS5kYXRhKCdib29rbWFyay1pZCcpO1xyXG5cclxuICAgICAgICAvLyBTdGFzaCB0aGUgYm9va21hcmsgbGFiZWwgZGF0YSBub3csIHN3YXAgbGFiZWwgdGV4dCBsYXRlclxyXG4gICAgICAgIGJvb2ttYXJrLmxhYmVsID0ge1xyXG4gICAgICAgICAgICBlbG06IGJvb2ttYXJrLmVsbS5maW5kKCcuanMtYm9va21hcmstbGFiZWwnKVxyXG4gICAgICAgIH07XHJcbiAgICAgICAgYm9va21hcmsubGFiZWwuYm9va21hcmsgPSBib29rbWFyay5sYWJlbC5lbG0uZGF0YSgnbGFiZWwtYm9va21hcmsnKTtcclxuICAgICAgICBib29rbWFyay5sYWJlbC5ib29rbWFya2VkID0gYm9va21hcmsubGFiZWwuZWxtLmRhdGEoJ2xhYmVsLWJvb2ttYXJrZWQnKTtcclxuXHJcbiAgICAgICAgLy8gQXJlIHdlIGJvb2ttYXJraW5nIGFuIGFydGljbGUsIG9yIHVuLWJvb2ttYXJraW5nP1xyXG4gICAgICAgIC8vIFVzZWQgbGF0ZXIgdG8ga25vdyB3aGF0IEFQSSBlbmRwb2ludCB0byBoaXQsIGFuZCB3aGF0IERPTSBjaGFuZ2VzIGFyZSByZXF1aXJlZFxyXG4gICAgICAgIGJvb2ttYXJrLmlzQm9va21hcmtpbmcgPSBib29rbWFyay5lbG0uZGF0YSgnaXMtYm9va21hcmtlZCcpID8gZmFsc2UgOiB0cnVlO1xyXG5cclxuICAgICAgICB2YXIgYXBpRW5kcG9pbnQgPSBib29rbWFyay5pc0Jvb2ttYXJraW5nID9cclxuICAgICAgICAgICAgJy9BY2NvdW50L2FwaS9TYXZlZERvY3VtZW50QXBpL1NhdmVJdGVtLycgOlxyXG4gICAgICAgICAgICAnL0FjY291bnQvYXBpL1NhdmVkRG9jdW1lbnRBcGkvUmVtb3ZlSXRlbS8nO1xyXG5cclxuICAgICAgICBpZihib29rbWFyay5pZCkge1xyXG4gICAgICAgICAgICAkLmFqYXgoe1xyXG4gICAgICAgICAgICAgICAgdXJsOiBhcGlFbmRwb2ludCxcclxuICAgICAgICAgICAgICAgIHR5cGU6ICdQT1NUJyxcclxuICAgICAgICAgICAgICAgIGRhdGE6IHtcclxuICAgICAgICAgICAgICAgICAgICBEb2N1bWVudElEOiBib29rbWFyay5pZFxyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIGNvbnRleHQ6IHRoaXMsXHJcbiAgICAgICAgICAgICAgICBzdWNjZXNzOiBmdW5jdGlvbiAocmVzcG9uc2UpIHtcclxuICAgICAgICAgICAgICAgICAgICBpZiAocmVzcG9uc2Uuc3VjY2Vzcykge1xyXG5cclxuXHRcdFx0XHRcdFx0aWYoYm9va21hcmsuaXNCb29rbWFya2luZykge1xyXG5cdFx0XHRcdFx0XHRcdGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgJChib29rbWFyay5lbG0pLmRhdGEoJ2FuYWx5dGljcycpKSApO1xyXG5cdFx0XHRcdFx0XHR9XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICB0aGlzLmZsaXBJY29uKGJvb2ttYXJrKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuIHRydWU7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgIGVsc2Uge1xyXG5cclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAgZXJyb3I6IGZ1bmN0aW9uKHJlc3BvbnNlKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9KTtcclxuXHJcbiAgICAgICAgfVxyXG4gICAgfTtcclxuXHJcbiAgICB0aGlzLmZsaXBJY29uID0gZnVuY3Rpb24oYm9va21hcmspIHtcclxuXHJcblx0XHRpZighYm9va21hcmsuZWxtLmhhc0NsYXNzKCdqcy1hbmd1bGFyLWJvb2ttYXJrJykpIHtcclxuXHRcdFx0JChib29rbWFyay5lbG0pLmZpbmQoJy5hcnRpY2xlLWJvb2ttYXJrJykucmVtb3ZlQ2xhc3MoJ2lzLXZpc2libGUnKTtcclxuXHRcdH1cclxuXHJcbiAgICAgICAgaWYoYm9va21hcmsuaXNCb29rbWFya2luZykge1xyXG4gICAgICAgICAgICBpZighYm9va21hcmsuZWxtLmhhc0NsYXNzKCdqcy1hbmd1bGFyLWJvb2ttYXJrJykpIHtcclxuICAgICAgICAgICAgICAgICQoYm9va21hcmsuZWxtKS5maW5kKCcuYXJ0aWNsZS1ib29rbWFya19fYm9va21hcmtlZCcpLmFkZENsYXNzKCdpcy12aXNpYmxlJyk7XHJcbiAgICAgICAgICAgICAgICBib29rbWFyay5lbG0uZGF0YSgnaXMtYm9va21hcmtlZCcsIHRydWUpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIGJvb2ttYXJrLmxhYmVsLmVsbS5odG1sKGJvb2ttYXJrLmxhYmVsLmJvb2ttYXJrZWQpO1xyXG5cclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBpZighYm9va21hcmsuZWxtLmhhc0NsYXNzKCdqcy1hbmd1bGFyLWJvb2ttYXJrJykpIHtcclxuICAgICAgICAgICAgICAgICQoYm9va21hcmsuZWxtKS5maW5kKCcuYXJ0aWNsZS1ib29rbWFyaycpLm5vdCgnLmFydGljbGUtYm9va21hcmtfX2Jvb2ttYXJrZWQnKS5hZGRDbGFzcygnaXMtdmlzaWJsZScpO1xyXG4gICAgICAgICAgICAgICAgYm9va21hcmsuZWxtLmRhdGEoJ2lzLWJvb2ttYXJrZWQnLCBudWxsKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBib29rbWFyay5sYWJlbC5lbG0uaHRtbChib29rbWFyay5sYWJlbC5ib29rbWFyayk7XHJcblxyXG4gICAgICAgIH1cclxuICAgIH07XHJcbn1cclxuXHJcbmV4cG9ydCBkZWZhdWx0IGJvb2ttYXJrQ29udHJvbGxlcjtcclxuIiwiLypcclxuXHJcbm9wdHMub2JzZXJ2ZSDigJQgRm9ybSBlbGVtZW50KHMpIHRvIG9ic2VydmVcclxub3B0cy5iZWZvcmVSZXF1ZXN0IOKAlCBGdW5jdGlvbiB0byBleGVjdXRlIGJlZm9yZSBtYWtpbmcgQWpheCByZXF1ZXN0XHJcbm9wdHMuc3VjY2Vzc0NhbGxiYWNrIOKAlCBJZiBBamF4IHJlcXVlc3QgaXMgc3VjY2Vzc2Z1bCwgY2FsbGJhY2tcclxub3B0cy5mYWlsdXJlQ2FsbGJhY2sg4oCUIElmIEFqYXggcmVxdWVzdCBmYWlscyAvIHJldHVybnMgZmFsc2UsIGNhbGxiYWNrXHJcblxyXG4qL1xyXG5cclxuZnVuY3Rpb24gZm9ybUNvbnRyb2xsZXIob3B0cykge1xyXG5cclxuXHR2YXIgc2hvd1N1Y2Nlc3NNZXNzYWdlID0gZnVuY3Rpb24oZm9ybSkge1xyXG5cdFx0JChmb3JtKS5maW5kKCcuanMtZm9ybS1zdWNjZXNzJykuc2hvdygpO1xyXG5cdH07XHJcblxyXG5cdHZhciBzaG93RXJyb3IgPSBmdW5jdGlvbihmb3JtLCBlcnJvcikge1xyXG5cdFx0aWYoJChmb3JtKS5maW5kKGVycm9yKSkge1xyXG5cdFx0XHQkKGZvcm0pLmZpbmQoZXJyb3IpLnNob3coKTtcclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHR2YXIgaGlkZUVycm9ycyA9IGZ1bmN0aW9uKGZvcm0pIHtcclxuXHRcdCQoZm9ybSkuZmluZCgnLmpzLWZvcm0tZXJyb3InKS5oaWRlKCk7XHJcblx0fTtcclxuXHJcblx0KGZ1bmN0aW9uIGluaXQoKSB7XHJcblxyXG5cdFx0dmFyIGZvcm0gPSBvcHRzLm9ic2VydmU7XHJcblxyXG5cdFx0aWYgKCFmb3JtKSByZXR1cm4gZmFsc2U7XHJcblxyXG5cdFx0dmFyIGZvcm1TdWJtaXQgPSAkKGZvcm0pLmZpbmQoJ2J1dHRvblt0eXBlPXN1Ym1pdF0nKTtcclxuXHJcblx0XHQkKGZvcm1TdWJtaXQpLm9uKCdjbGljaycsIGZ1bmN0aW9uKGV2ZW50KSB7XHJcblxyXG5cdFx0XHQvLyBTb21lIGZvcm1zIHdpbGwgcmVxdWlyZSB1c2VyIGNvbmZpcm1hdGlvbiBiZWZvcmUgYWN0aW9uIGlzIHRha2VuXHJcblx0XHRcdC8vIERlZmF1bHQgdG8gdHJ1ZSAoY29uZmlybWVkKSwgc2V0IHRvIGZhbHNlIGxhdGVyIGlmIGNvbmZpcm1hdGlvbiBpc1xyXG5cdFx0XHQvLyByZXF1aXJlZCBhbmQgdXNlciBjYW5jZWxzIGFjdGlvblxyXG5cdFx0XHR2YXIgYWN0aW9uQ29uZmlybWVkID0gdHJ1ZTtcclxuXHJcblx0XHRcdHZhciBjdXJyZW50Rm9ybTtcclxuXHRcdFx0aWYoZXZlbnQudGFyZ2V0LmZvcm0pIHtcclxuXHRcdFx0XHRjdXJyZW50Rm9ybSA9IGV2ZW50LnRhcmdldC5mb3JtO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdGN1cnJlbnRGb3JtID0gJChldmVudC50YXJnZXQpLmNsb3Nlc3QoJ2Zvcm0nKTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0aWYoJChjdXJyZW50Rm9ybSkuZGF0YSgnZm9yY2UtY29uZmlybScpKSB7XHJcblx0XHRcdFx0YWN0aW9uQ29uZmlybWVkID0gd2luZG93LmNvbmZpcm0oJChjdXJyZW50Rm9ybSkuZGF0YSgnZm9yY2UtY29uZmlybScpKTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0aWYoYWN0aW9uQ29uZmlybWVkKSB7XHJcblxyXG5cdFx0XHRcdGV2ZW50LnByZXZlbnREZWZhdWx0KCk7IC8vIFByZXZlbnQgZm9ybSBzdWJtaXR0aW5nXHJcblxyXG5cdFx0XHRcdGhpZGVFcnJvcnMoY3VycmVudEZvcm0pOyAvLyBSZXNldCBhbnkgdmlzaWJsZSBlcnJvcnNcclxuXHJcblx0XHRcdFx0aWYob3B0cy5iZWZvcmVSZXF1ZXN0KSB7XHJcblx0XHRcdFx0XHRvcHRzLmJlZm9yZVJlcXVlc3QoY3VycmVudEZvcm0pO1xyXG5cdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0Ly8gUHJldmVudCB1c2VyIGZyb20gcmUtc3VibWl0dGluZyBmb3JtLCB1bmxlc3MgZXhwbGljaXRseSBhbGxvd2VkXHJcblx0XHRcdFx0aWYoISQoY3VycmVudEZvcm0pLmRhdGEoJ3ByZXZlbnQtZGlzYWJsaW5nJykpIHtcclxuXHRcdFx0XHRcdCQoZm9ybVN1Ym1pdCkuYXR0cignZGlzYWJsZWQnLCAnZGlzYWJsZWQnKTtcclxuXHRcdFx0XHR9XHJcblxyXG5cdFx0XHRcdHZhciBpbnB1dERhdGEgPSB7fTtcclxuXHRcdFx0XHR2YXIgSXNWYWxpZCA9IHRydWU7Ly9Ta2lwIFZhbGlkYXRpb24gaWYgdGhlIGZvcm0gaXMgbm90IFVwZGF0ZSBDb250YWN0IEluZm9ybWF0aW4gRm9ybVxyXG5cdFx0XHRcdGlmKCQoY3VycmVudEZvcm0pLmhhc0NsYXNzKCdmb3JtLXVwZGF0ZS1hY2NvdW50LWNvbnRhY3QnKSlcclxuXHRcdFx0XHR7XHJcblx0XHRcdFx0XHRJc1ZhbGlkID0gVmFsaWRhdGVDb250YWN0SW5mb3JGb3JtKCk7XHJcblx0XHRcdFx0fVxyXG5cdFx0XHRcdGlmKElzVmFsaWQpe1xyXG5cdFx0XHRcdFx0JChjdXJyZW50Rm9ybSkuZmluZCgnaW5wdXQsIHNlbGVjdCwgdGV4dGFyZWEnKS5lYWNoKGZ1bmN0aW9uKCkge1xyXG5cclxuXHRcdFx0XHRcdFx0dmFyIHZhbHVlID0gJyc7XHJcblx0XHRcdFx0XHRcdHZhciBmaWVsZCA9ICQodGhpcyk7XHJcblxyXG5cdFx0XHRcdFx0XHRpZiAoZmllbGQuZGF0YSgnY2hlY2tib3gtdHlwZScpID09PSAnYm9vbGVhbicpIHtcclxuXHRcdFx0XHRcdFx0XHR2YWx1ZSA9IHRoaXMuY2hlY2tlZDtcclxuXHJcblx0XHRcdFx0XHRcdFx0aWYgKGZpZWxkLmRhdGEoJ2NoZWNrYm94LWJvb2xlYW4tdHlwZScpID09PSAncmV2ZXJzZScpIHtcclxuXHRcdFx0XHRcdFx0XHRcdHZhbHVlID0gIXZhbHVlO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0fSBlbHNlIGlmIChmaWVsZC5kYXRhKCdjaGVja2JveC10eXBlJykgPT09ICd2YWx1ZScpIHtcclxuXHRcdFx0XHRcdFx0XHR2YWx1ZSA9IHRoaXMuY2hlY2tlZCA/IGZpZWxkLnZhbCgpIDogdW5kZWZpbmVkO1xyXG5cdFx0XHRcdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdFx0XHRcdHZhbHVlID0gZmllbGQudmFsKCk7XHJcblx0XHRcdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0XHRcdGlmICh2YWx1ZSAhPT0gdW5kZWZpbmVkKSB7XHJcblx0XHRcdFx0XHRcdFx0aWYgKGlucHV0RGF0YVtmaWVsZC5hdHRyKCduYW1lJyldID09PSB1bmRlZmluZWQpIHtcclxuXHRcdFx0XHRcdFx0XHRcdGlucHV0RGF0YVtmaWVsZC5hdHRyKCduYW1lJyldID0gdmFsdWU7XHJcblx0XHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFx0XHRcdGVsc2UgaWYgKCQuaXNBcnJheShpbnB1dERhdGFbZmllbGQuYXR0cignbmFtZScpXSkpIHtcclxuXHRcdFx0XHRcdFx0XHRcdGlucHV0RGF0YVtmaWVsZC5hdHRyKCduYW1lJyldLnB1c2godmFsdWUpO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0XHRlbHNlIHtcclxuXHRcdFx0XHRcdFx0XHRcdGlucHV0RGF0YVtmaWVsZC5hdHRyKCduYW1lJyldID0gWyBpbnB1dERhdGFbZmllbGQuYXR0cignbmFtZScpXSBdO1xyXG5cdFx0XHRcdFx0XHRcdFx0aW5wdXREYXRhW2ZpZWxkLmF0dHIoJ25hbWUnKV0ucHVzaCh2YWx1ZSk7XHJcblx0XHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHR9KTtcclxuXHJcblx0XHRcdFx0XHQvLyBhZGQgcmVjYXB0Y2hhIGlmIGl0IGV4aXN0cyBpbiB0aGUgZm9ybVxyXG5cdFx0XHRcdFx0dmFyIGNhcHRjaGFSZXNwb25zZSA9IChncmVjYXB0Y2hhID09IG51bGwpID8gdW5kZWZpbmVkIDogZ3JlY2FwdGNoYS5nZXRSZXNwb25zZSgpO1xyXG5cdFx0XHRcdFx0aWYgKGNhcHRjaGFSZXNwb25zZSAhPT0gdW5kZWZpbmVkKVxyXG5cdFx0XHRcdFx0XHRpbnB1dERhdGFbJ1JlY2FwdGNoYVJlc3BvbnNlJ10gPSBjYXB0Y2hhUmVzcG9uc2U7XHJcblxyXG5cdFx0XHRcdFx0aWYoISQoY3VycmVudEZvcm0pLmRhdGEoJ29uLXN1Ym1pdCcpKSB7XHJcblx0XHRcdFx0XHRcdGNvbnNvbGUud2FybignTm8gc3VibWl0IGxpbmsgZm9yIGZvcm0nKTtcclxuXHRcdFx0XHRcdH1cclxuXHRcdFx0XHQgICAgdHJ5e1xyXG5cdFx0XHRcdCAgICAgICAgZm9yKHZhciBpbmRleCBpbiBpbnB1dERhdGEpXHJcblx0XHRcdFx0ICAgICAgICB7XHJcblx0XHRcdFx0ICAgICAgICAgICAgaWYoaW5wdXREYXRhW2luZGV4XSA9PSBcIi0gU2VsZWN0IE9uZSAtXCIpXHJcblx0XHRcdFx0ICAgICAgICAgICAge1xyXG5cdFx0XHRcdCAgICAgICAgICAgICAgICBpbnB1dERhdGFbaW5kZXhdID0gXCJcIjtcclxuXHRcdFx0XHQgICAgICAgICAgICB9XHJcblx0XHRcdFx0ICAgICAgICB9XHJcblx0XHRcdFx0ICAgIH1jYXRjaChleCl7Y29uc29sZS5sb2coZXgpO31cclxuXHJcblx0XHRcdFx0XHQkLmFqYXgoe1xyXG5cdFx0XHRcdFx0XHR1cmw6ICQoY3VycmVudEZvcm0pLmRhdGEoJ29uLXN1Ym1pdCcpLFxyXG5cdFx0XHRcdFx0XHR0eXBlOiAkKGN1cnJlbnRGb3JtKS5kYXRhKCdzdWJtaXQtdHlwZScpIHx8ICdQT1NUJyxcclxuXHRcdFx0XHRcdFx0ZGF0YTogaW5wdXREYXRhLFxyXG5cdFx0XHRcdFx0XHRjb250ZXh0OiB0aGlzLFxyXG5cdFx0XHRcdFx0XHRzdWNjZXNzOiBmdW5jdGlvbiAocmVzcG9uc2UpIHtcclxuXHRcdFx0XHRcdFx0XHRpZiAocmVzcG9uc2Uuc3VjY2Vzcykge1xyXG5cclxuXHRcdFx0XHRcdFx0XHRcdHNob3dTdWNjZXNzTWVzc2FnZShjdXJyZW50Rm9ybSk7XHJcblxyXG5cdFx0XHRcdFx0XHRcdFx0Ly8gUGFzc2VzIHRoZSBmb3JtIHJlc3BvbnNlIHRocm91Z2ggd2l0aCB0aGUgXCJjb250ZXh0XCJcclxuXHRcdFx0XHRcdFx0XHRcdC8vIHN1Y2Nlc3NDYWxsYmFjayBpcyByaXBlIGZvciByZWZhY3RvcmluZywgaW1wcm92aW5nIHBhcmFtZXRlcnNcclxuXHRcdFx0XHRcdFx0XHRcdHRoaXMucmVzcG9uc2UgPSByZXNwb25zZTtcclxuXHJcblx0XHRcdFx0XHRcdFx0XHRpZiAob3B0cy5zdWNjZXNzQ2FsbGJhY2spIHtcclxuXHRcdFx0XHRcdFx0XHRcdFx0b3B0cy5zdWNjZXNzQ2FsbGJhY2soY3VycmVudEZvcm0sIHRoaXMsIGV2ZW50KTtcclxuXHRcdFx0XHRcdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0XHRcdFx0XHRpZigkKGZvcm0pLmRhdGEoJ29uLXN1Y2Nlc3MnKSkge1xyXG5cdFx0XHRcdFx0XHRcdFx0XHR3aW5kb3cubG9jYXRpb24uaHJlZiA9ICQoY3VycmVudEZvcm0pLmRhdGEoJ29uLXN1Y2Nlc3MnKTtcclxuXHRcdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdFx0ZWxzZSB7XHJcblx0XHRcdFx0XHRcdFx0XHRpZiAocmVzcG9uc2UucmVhc29ucyAmJiByZXNwb25zZS5yZWFzb25zLmxlbmd0aCA+IDApIHtcclxuXHRcdFx0XHRcdFx0XHRcdFx0Zm9yICh2YXIgcmVhc29uIGluIHJlc3BvbnNlLnJlYXNvbnMpIHtcclxuXHRcdFx0XHRcdFx0XHRcdFx0XHRzaG93RXJyb3IoZm9ybSwgJy5qcy1mb3JtLWVycm9yLScgKyByZXNwb25zZS5yZWFzb25zW3JlYXNvbl0pO1xyXG5cdFx0XHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdFx0XHRcdFx0XHRzaG93RXJyb3IoY3VycmVudEZvcm0sICcuanMtZm9ybS1lcnJvci1nZW5lcmFsJyk7XHJcblx0XHRcdFx0XHRcdFx0XHR9XHJcblxyXG5cdFx0XHRcdFx0XHRcdFx0aWYgKG9wdHMuZmFpbHVyZUNhbGxiYWNrKSB7XHJcblx0XHRcdFx0XHRcdFx0XHRcdG9wdHMuZmFpbHVyZUNhbGxiYWNrKGN1cnJlbnRGb3JtLHJlc3BvbnNlKTtcclxuXHRcdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdH0sXHJcblx0XHRcdFx0XHRcdGVycm9yOiBmdW5jdGlvbihyZXNwb25zZSkge1xyXG5cclxuXHRcdFx0XHRcdFx0XHRzaG93RXJyb3IoY3VycmVudEZvcm0sICcuanMtZm9ybS1lcnJvci1nZW5lcmFsJyk7XHJcblxyXG5cdFx0XHRcdFx0XHRcdGlmIChvcHRzLmZhaWx1cmVDYWxsYmFjaykge1xyXG5cdFx0XHRcdFx0XHRcdFx0b3B0cy5mYWlsdXJlQ2FsbGJhY2soY3VycmVudEZvcm0scmVzcG9uc2UpO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0fSxcclxuXHRcdFx0XHRcdFx0Y29tcGxldGU6IGZ1bmN0aW9uKCkge1xyXG5cdFx0XHRcdFx0XHRcdHNldFRpbWVvdXQoKGZ1bmN0aW9uKCkge1xyXG5cdFx0XHRcdFx0XHRcdFx0JChmb3JtU3VibWl0KS5yZW1vdmVBdHRyKCdkaXNhYmxlZCcpO1xyXG5cdFx0XHRcdFx0XHRcdH0pLCAyNTApO1x0XHRcdFx0XHRcdFx0XHJcblxyXG5cdFx0XHRcdFx0XHRcdC8vIHJlc2V0IGNhcHRjaGEgaWYgYXZhaWxhYmxlXHJcblx0XHRcdFx0XHRcdFx0Z3JlY2FwdGNoYS5yZXNldCgpO1xyXG5cdFx0XHRcdFx0XHR9XHJcblxyXG5cdFx0XHRcdFx0fSk7XHJcblx0XHRcdFxyXG5cdFx0XHRcdH0gLy8gaWYgYWN0aW9uQ29uZmlybWVkXHJcblx0XHRcdH1cclxuXHRcdFx0cmV0dXJuIGZhbHNlO1xyXG5cclxuXHRcdH0pO1xyXG5cdH0pKCk7XHJcbn1cclxuZnVuY3Rpb24gVmFsaWRhdGVDb250YWN0SW5mb3JGb3JtKCkge1xyXG5cdHZhciBlcnJvckh0bWwgPSAkKCcjZXJyb3JNZXNzYWdlJykuaHRtbCgpO1xyXG5cdHZhciBlcnJvcnMgPSAwO1xyXG5cdHZhciByZXN1bHQgPSBmYWxzZTtcclxuXHR2YXIgc2Nyb2xsVG8gPSAnJztcclxuXHQkKCcucmVxdWlyZWQnKS5lYWNoKGZ1bmN0aW9uICgpIHtcclxuICAgICAgICAgICAgaWYgKCQodGhpcykudmFsKCkgPT0gJycgfHwgJCh0aGlzKS50ZXh0KCkuaW5kZXhPZihcIi0gU2VsZWN0IE9uZSAtXCIpID49IDApIHtcclxuXHRcdFx0JCh0aGlzKS5wYXJlbnQoKS5hcHBlbmQoZXJyb3JIdG1sKTtcclxuXHRcdFx0ZXJyb3JzKys7XHJcblx0XHRcdGlmKGVycm9ycz09MSlcclxuXHRcdFx0e1xyXG5cdFx0XHRcdHNjcm9sbFRvID0gJCh0aGlzKTtcclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdFx0ZWxzZSB7XHJcblx0XHRcdCQodGhpcykucGFyZW50KCkuZmluZCgnLmpzLWZvcm0tZXJyb3InKS5yZW1vdmUoKTtcclxuXHRcdH1cclxuXHJcblx0fSk7XHJcblx0aWYgKGVycm9ycyA+IDApIHtcclxuXHRcdHdpbmRvdy5zY3JvbGxUbygwLHNjcm9sbFRvLm9mZnNldCgpLnRvcC0zMCk7XHJcblx0XHRyZXN1bHQgPSBmYWxzZTtcclxuXHR9XHJcblx0ZWxzZSB7XHJcblx0XHRyZXN1bHQgPSB0cnVlO1xyXG5cdH1cclxuXHRyZXR1cm4gcmVzdWx0O1xyXG59XHJcblxyXG5leHBvcnQgZGVmYXVsdCBmb3JtQ29udHJvbGxlcjtcclxuIiwiLyogZ2xvYmFsIGFuZ3VsYXIgKi9cclxuZnVuY3Rpb24gbGlnaHRib3hNb2RhbENvbnRyb2xsZXIoKSB7XHJcblxyXG4gICAgdGhpcy5jbG9zZUxpZ2h0Ym94TW9kYWwgPSBmdW5jdGlvbigpIHtcclxuICAgICAgICAkKCdib2R5JykucmVtb3ZlQ2xhc3MoJ2xpZ2h0Ym94ZWQnKTtcclxuICAgICAgICAkKCcubGlnaHRib3gtbW9kYWxfX2JhY2tkcm9wJykucmVtb3ZlKCk7XHJcbiAgICAgICAgJCgnLmxpZ2h0Ym94LW1vZGFsJykuaGlkZSgpO1xyXG4gICAgfTtcclxuXHJcbiAgICB2YXIgY2xvc2VMaWdodGJveE1vZGFsID0gdGhpcy5jbG9zZUxpZ2h0Ym94TW9kYWw7XHJcblxyXG4gICAgdGhpcy5zaG93TGlnaHRib3ggPSBmdW5jdGlvbihsaWdodGJveCkge1xyXG4gICAgICAgIC8vIEZyZWV6ZSB0aGUgcGFnZSBhbmQgYWRkIHRoZSBkYXJrIG92ZXJsYXlcclxuICAgICAgICAkKCdib2R5JylcclxuICAgICAgICAgICAgLmFkZENsYXNzKCdsaWdodGJveGVkJylcclxuICAgICAgICAgICAgLmFwcGVuZCgnPGRpdiBjbGFzcz1cImxpZ2h0Ym94LW1vZGFsX19iYWNrZHJvcFwiPjwvZGl2PicpO1xyXG5cclxuICAgICAgICAvLyBGaW5kIHRoZSBzcGVjaWZpYyBtb2RhbCBmb3IgdGhpcyB0cmlnZ2VyLCBhbmQgdGhlIGFzc29jaWF0ZWQgZm9ybVxyXG4gICAgICAgIHZhciB0YXJnZXRNb2RhbCA9ICQobGlnaHRib3gpLmRhdGEoJ2xpZ2h0Ym94LW1vZGFsJyk7XHJcbiAgICAgICAgdmFyIHN1Y2Nlc3NGb3JtID0gJChsaWdodGJveCkuY2xvc2VzdCgnLicgKyAkKGxpZ2h0Ym94KS5kYXRhKCdsaWdodGJveC1tb2RhbC1zdWNjZXNzLXRhcmdldCcpKTtcclxuXHJcbiAgICAgICAgLy8gU2hvdyB0aGUgbW9kYWwsIGFkZCBhbiBvbi1jbGljayBsaXN0ZW5lciBmb3IgdGhlIFwic3VjY2Vzc1wiIGJ1dHRvblxyXG4gICAgICAgICQoJy4nICsgdGFyZ2V0TW9kYWwpXHJcbiAgICAgICAgICAgIC5zaG93KClcclxuICAgICAgICAgICAgLmZpbmQoJy5qcy1saWdodGJveC1tb2RhbC1zdWJtaXQnKVxyXG4gICAgICAgICAgICAvLyAub25lLCBub3QgLm9uLCB0byBwcmV2ZW50IHN0YWNraW5nIGV2ZW50IGxpc3RuZXJzXHJcbiAgICAgICAgICAgIC5vbmUoJ2NsaWNrJywgZnVuY3Rpb24oZSkge1xyXG4gICAgICAgICAgICAgICAgc3VjY2Vzc0Zvcm0uZmluZCgnYnV0dG9uW3R5cGU9c3VibWl0XScpLmNsaWNrKCk7XHJcbiAgICAgICAgICAgICAgICBjbG9zZUxpZ2h0Ym94TW9kYWwoKTtcclxuICAgICAgICAgICAgfSk7XHJcblxyXG4gICAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH07XHJcblxyXG4gICAgdmFyIHNob3dMaWdodGJveCA9IHRoaXMuc2hvd0xpZ2h0Ym94O1xyXG5cclxuICAgIHRoaXMuYnVpbGRMaWdodGJveGVzID0gZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgJCgnLmpzLWxpZ2h0Ym94LW1vZGFsLXRyaWdnZXInKS5vbignY2xpY2snLCBmdW5jdGlvbihlKSB7XHJcblxyXG4gICAgICAgICAgICBpZiAoZS50YXJnZXQgIT09IHRoaXMpIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xpY2soKTtcclxuICAgICAgICAgICAgICAgIHJldHVybjtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgc2hvd0xpZ2h0Ym94KGUudGFyZ2V0KTtcclxuXHJcbiAgICAgICAgICAgIC8vIERvbid0IHN1Ym1pdCBhbnkgZm9ybXMgZm9yIHJlYWwuXHJcbiAgICAgICAgICAgIHJldHVybiBmYWxzZTtcclxuICAgICAgICB9KTtcclxuICAgIH07XHJcblxyXG4gICAgLy8gV2hlbiB0aGUgRGlzbWlzcyBidXR0b24gaXMgY2xpY2tlZC4uLlxyXG4gICAgJCgnLmpzLWNsb3NlLWxpZ2h0Ym94LW1vZGFsJykub24oJ2NsaWNrJywgZnVuY3Rpb24oZSkge1xyXG4gICAgICAgIGNsb3NlTGlnaHRib3hNb2RhbCgpO1xyXG4gICAgfSk7XHJcblxyXG4gICAgdGhpcy5idWlsZExpZ2h0Ym94ZXMoKTtcclxuXHJcblx0dGhpcy5jbGVhckxpZ2h0Ym94ZXMgPSBmdW5jdGlvbigpIHtcclxuXHRcdCQoJy5qcy1saWdodGJveC1tb2RhbC10cmlnZ2VyJykub2ZmKCk7XHJcblx0fTtcclxuXHJcbn1cclxuXHJcbmV4cG9ydCBkZWZhdWx0IGxpZ2h0Ym94TW9kYWxDb250cm9sbGVyO1xyXG4iLCJmdW5jdGlvbiBwb3BPdXRDb250cm9sbGVyKHRyaWdnZXJFbG0pIHtcclxuXHJcblx0Ly8gVG9nZ2xlIHBvcC1vdXQgd2hlbiB0cmlnZ2VyIGlzIGNsaWNrZWRcclxuICAgIGlmKHRyaWdnZXJFbG0pIHtcclxuICAgICAgICAkKHRyaWdnZXJFbG0pLm9mZigpO1xyXG5cdFx0JCh0cmlnZ2VyRWxtKS5vbignY2xpY2snLCAoZXZlbnQpID0+IHtcclxuXHRcdFx0ZXZlbnQucHJldmVudERlZmF1bHQoKTtcclxuXHRcdFx0dGhpcy50b2dnbGVQb3BPdXQoJChldmVudC50YXJnZXQpKTtcclxuXHRcdH0pO1xyXG5cdH1cclxuXHJcblx0Ly8gUmVwb3NpdGlvbiBwb3Atb3V0IHdoZW4gYnJvd3NlciB3aW5kb3cgcmVzaXplc1xyXG5cdCQod2luZG93KS5vbigncmVzaXplJywgKGV2ZW50KSA9PiB7XHJcblx0XHR0aGlzLnVwZGF0ZVBvcE91dCgpO1xyXG5cdH0pO1xyXG5cclxuXHQvLyBTaW11bGF0ZSBDU1MgYHJlbWAgKDE2cHgpXHJcblx0Ly8gVE9ETzogY2hhbmdlIHRoaXMgZnJvbSBgcmVtYCB0byB0YWIgcGFkZGluZyB2YWx1ZSBmb3IgY2xhcml0eVxyXG5cdHZhciByZW0gPSAxNjtcclxuXHJcblx0Ly8gS2VlcCB0cmFjayBvZiB0aGUgYWN0aXZlIHBvcC1vdXQgZWxlbWVudFxyXG5cdC8vIFRoaXMgaXMgYW4gb2JqZWN0IGluc3RlYWQgb2YgYSB2YXIgYmVjYXVzZSB0aGVyZSBtaWdodCBiZSBtb3JlIFwiZ2xvYmFsXCJcclxuXHQvLyBzdGF0ZSBhdHRyaWJ1dGVzIHRvIHRyYWNrIGluIHRoZSBmdXR1cmUuXHJcblx0dmFyIHN0YXRlID0ge1xyXG5cdFx0YWN0aXZlRWxtOiBudWxsLFxyXG5cdFx0Y3VzdG9taXplZDoge1xyXG5cclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHQvLyBQVUJMSUNcclxuXHQvLyBHZXQgdGhlIGN1cnJlbnQgcG9wLW91dCBlbGVtZW50LCBpZiB0aGVyZSBpcyBvbmUuXHJcblx0Ly8gTGV0cyBvdGhlciBKUyBrbm93IHdoYXQncyB1cCB3aXRoIHRoZSBwb3Atb3V0LlxyXG5cdHRoaXMuZ2V0UG9wT3V0RWxlbWVudCA9IGZ1bmN0aW9uKCkge1xyXG5cdFx0cmV0dXJuIHN0YXRlLmFjdGl2ZUVsbTtcclxuXHR9O1xyXG5cclxuXHQvLyBQVUJMSUNcclxuXHQvLyBDbG9zZXMgdGhlIHBvcC1vdXQuXHJcblx0dGhpcy5jbG9zZVBvcE91dCA9IGZ1bmN0aW9uKGVsbSkge1xyXG5cdFx0Ly8gUmVzZXQgYWxsIHotaW5kZXhlcyBzbyBuZXcgcG9wLW91dHMgYXJlIHN0YWNrZWQgb24gdG9wIHByb3Blcmx5XHJcblx0XHQkKCcucG9wLW91dCcpLnJlbW92ZUNsYXNzKCdpcy1hY3RpdmUnKS5jc3MoXCJ6LWluZGV4XCIsIFwiXCIpO1xyXG5cdFx0JCgnLmpzLXBvcC1vdXQtdHJpZ2dlcicpLmNzcyhcInotaW5kZXhcIiwgXCJcIik7XHJcblx0fTtcclxuXHJcblx0Ly8gUFVCTElDXHJcblx0Ly8gVG9nZ2xlcyB0aGUgcG9wLW91dFxyXG5cdHRoaXMudG9nZ2xlUG9wT3V0ID0gZnVuY3Rpb24oZSkge1xyXG5cdFx0Ly8gQ2hlY2sgaWYgY2xpY2tlZCBlbGVtZW50IGlzIHRoZSB0b2dnbGUgaXRzZWxmXHJcblx0XHQvLyBPdGhlcndpc2UsIGNsaW1iIHVwIERPTSB0cmVlIGFuZCBmaW5kIGl0XHJcblx0XHR2YXIgcG9QYXJlbnQgPSBlLmhhc0NsYXNzKCdqcy1wb3Atb3V0LXRyaWdnZXInKSA/IGUgOiBlLmNsb3Nlc3QoJy5qcy1wb3Atb3V0LXRyaWdnZXInKTtcclxuXHJcblx0XHQvKiAgVGhpcyBpcyBhIGxpdHRsZSBoYWNreSwgYnV0IGlmIGEgdXNlciBpcyB0cnlpbmcgdG8gYm9va21hcmsgYW4gYXJ0aWNsZVxyXG5cdFx0XHRidXQgbmVlZHMgdG8gc2lnbiBpbiBmaXJzdCwgd2UgbmVlZCB0byBjYXB0dXJlIGFuZCBwYXNzIHRoZSBhcnRpY2xlXHJcblx0XHRcdElEIGFzIGEgVVJMIHBhcmFtIGFmdGVyIGEgc3VjY2Vzc2Z1bCBzaWduIGluIGF0dGVtcHQuIFRoYXQgYWxsb3dzXHJcblx0XHRcdHVzIHRvIGF1dG9tYXRpY2FsbHkgYm9va21hcmsgdGhlIGFydGljbGUgb24gcGFnZSByZWZyZXNoLiAqL1xyXG5cclxuXHRcdGlmKHBvUGFyZW50LmRhdGEoJ3BvcC1vdXQtdHlwZScpID09PSAnc2lnbi1pbicgJiYgcG9QYXJlbnQuZGF0YSgnYm9va21hcmstaWQnKSkge1xyXG5cdFx0XHQkKCcuc2lnbi1pbl9fc3VibWl0JykuZGF0YSgncGFzcy1hcnRpY2xlLWlkJywgcG9QYXJlbnQuZGF0YSgnYm9va21hcmstaWQnKSk7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHRwb1BhcmVudC5kYXRhKCdib29rbWFyay1pZCcsIG51bGwpO1xyXG5cdFx0fVxyXG5cclxuXHRcdC8vIENsb3NlIGFsbCBwb3Atb3V0c1xyXG5cdFx0dGhpcy5jbG9zZVBvcE91dCgpO1xyXG5cclxuXHRcdGlmKHBvUGFyZW50WzBdICE9PSBzdGF0ZS5hY3RpdmVFbG0pIHtcclxuXHRcdFx0Ly8gVXBkYXRlIHRoZSBjb250cm9sbGVyIHN0YXRlIGFuZCBvcGVuIGl0XHJcblx0XHRcdHN0YXRlLmFjdGl2ZUVsbSA9IHBvUGFyZW50WzBdO1xyXG5cdFx0XHR1cGRhdGVQb3NpdGlvbigpO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0c3RhdGUuYWN0aXZlRWxtID0gbnVsbDtcclxuXHRcdH1cclxuXHJcblx0fTtcclxuXHJcblxyXG5cdC8vIFBVQkxJQ1xyXG5cdC8vIFN0b3JlcyBwb3Atb3V0IGN1c3RvbWl6YXRpb24gZGV0YWlscyBmb3IgcmVmZXJlbmNlIHdoZW4gcmVuZGVyaW5nXHJcblx0dGhpcy5jdXN0b21pemUgPSBmdW5jdGlvbihvYmopIHtcclxuXHRcdHN0YXRlLmN1c3RvbWl6ZWRbb2JqLmlkXSA9IG9iajtcclxuXHR9O1xyXG5cclxuXHJcblx0Ly8gUFJJVkFURVxyXG5cdC8vIFVwZGF0ZSB0aGUgdmlzaWJpbGl0eSBhbmQgcG9zaXRpb24gb2YgdGhlIHBvcC1vdXQgYm94IGFuZCB0YWIuXHJcblx0dmFyIHVwZGF0ZVBvc2l0aW9uID0gZnVuY3Rpb24oKSB7XHJcblxyXG5cdFx0dmFyIHRyZ3IgPSB7IC8vIFRoZSBwb3Atb3V0IHRyaWdnZXJcclxuXHRcdFx0ZTogJChzdGF0ZS5hY3RpdmVFbG0pXHJcblx0XHR9O1xyXG5cdFx0Ly8gR2V0IHRyaWdnZXIgaGVpZ2h0LCB3aWR0aCwgb2Zmc2V0VG9wLCBvZmZzZXRXaWR0aFxyXG5cdFx0dHJnci5vZmZzZXQgPSB0cmdyLmUub2Zmc2V0KCk7XHJcblxyXG5cdFx0dHJnci5oYXNTdHlsZXMgPSBzdGF0ZS5jdXN0b21pemVkW3RyZ3IuZS5kYXRhKCdwb3Atb3V0LWlkJyldO1xyXG5cclxuXHRcdC8vIERldGVybWluZSB3aGljaCBwb3Atb3V0IHRlbXBsYXRlIHRvIHVzZVxyXG5cdFx0Ly8gVE9ETzogTWFrZSB0aGlzIHVzZXItY29uZmlndXJhYmxlXHJcblx0XHQvLyBMZXQgdXNlcnMgYXNzaWduIGEgbmFtZSB0byBhIHRlbXBsYXRlIGNsYXNzXHJcblx0XHR2YXIgcG9wT3V0O1xyXG5cdFx0c3dpdGNoICh0cmdyLmUuZGF0YSgncG9wLW91dC10eXBlJykpIHtcclxuXHRcdFx0Ly8gU0lHTiBJTlxyXG5cdFx0XHQvLyAoR2xvYmFsIHNpZ24taW4sIGJvb2ttYXJraW5nIHdoZW4gbm90IHNpZ25lZCBpbilcclxuXHRcdFx0Y2FzZSAnc2lnbi1pbic6XHJcblx0XHRcdFx0cG9wT3V0ID0gJCgnLmpzLXBvcC1vdXRfX3NpZ24taW4nKTtcclxuXHRcdFx0XHRicmVhaztcclxuXHRcdFx0Ly8gTWFpbiBTaWduIEluIGJ1dHRvbiBvbiB0b3AgcmlnaHRcclxuXHRcdCAgICBjYXNlICdzaWduLWluLWhlYWRlcic6XHJcblx0XHQgICAgICAgIHBvcE91dCA9ICQoJy5qcy1wb3Atb3V0X19zaWduLWluLWhlYWRlcicpO1xyXG5cdFx0ICAgICAgICBicmVhaztcclxuXHRcdFx0Ly8gRU1BSUwgQVJUSUNMRVxyXG5cdFx0XHRjYXNlICdlbWFpbC1hcnRpY2xlJzpcclxuXHRcdFx0XHRwb3BPdXQgPSAkKCcuanMtcG9wLW91dF9fZW1haWwtYXJ0aWNsZScpO1xyXG5cdFx0XHRcdGJyZWFrO1xyXG5cdFx0XHQvLyBFTUFJTCBBUlRJQ0xFXHJcblx0XHRcdGNhc2UgJ2VtYWlsLXNlYXJjaCc6XHJcblx0XHRcdFx0cG9wT3V0ID0gJCgnLmpzLXBvcC1vdXRfX2VtYWlsLXNlYXJjaCcpO1xyXG5cdFx0XHRcdGJyZWFrO1xyXG5cdFx0XHQvLyBFTUFJTCBBVVRIT1JcclxuXHRcdFx0Y2FzZSAnZW1haWwtYXV0aG9yJzpcclxuXHRcdFx0XHRwb3BPdXQgPSAkKCcuanMtcG9wLW91dF9fZW1haWwtYXV0aG9yJyk7XHJcblx0XHRcdFx0YnJlYWs7XHJcblx0XHQgICAgLy8gRU1BSUwgQ09NUEFOeVxyXG5cdFx0ICAgIGNhc2UgJ2VtYWlsLWNvbXBhbnknOlxyXG5cdFx0ICAgICAgICBwb3BPdXQgPSAkKCcuanMtcG9wLW91dF9fZW1haWwtY29tcGFueScpO1xyXG5cdFx0ICAgICAgICBicmVhaztcclxuXHRcdCAgICAvLyBFTUFJTCBERUFMXHJcblx0XHQgICAgY2FzZSAnZW1haWwtZGVhbCc6XHJcblx0XHQgICAgICAgIHBvcE91dCA9ICQoJy5qcy1wb3Atb3V0X19lbWFpbC1kZWFsJyk7XHJcblx0XHQgICAgICAgIGJyZWFrO1xyXG5cdFx0XHQvLyBHTE9CQUwgSEVBREVSIFJFR0lTVFJBVElPTlxyXG5cdFx0XHRjYXNlICdyZWdpc3Rlcic6XHJcblx0XHRcdFx0cG9wT3V0ID0gJCgnLmpzLXBvcC1vdXRfX3JlZ2lzdGVyJyk7XHJcblx0XHRcdFx0YnJlYWs7XHJcblx0XHRcdC8vIFNFQVJDSCBQQUdFIC0gU0FWRSBTRUFSQ0hcclxuXHRcdFx0Y2FzZSAnc2F2ZS1zZWFyY2gnOlxyXG5cdFx0XHRcdHBvcE91dCA9ICQoJy5qcy1wb3Atb3V0X19zYXZlLXNlYXJjaCcpO1xyXG5cdFx0XHRcdGJyZWFrO1xyXG5cdFx0XHRkZWZhdWx0OlxyXG5cdFx0XHRcdGNvbnNvbGUud2FybignQXR0ZW1wdGluZyB0byBmaXJlIHVuaWRlbnRpZmllZCBwb3Atb3V0LicpO1xyXG5cdFx0XHRcdHJldHVybjtcclxuXHRcdH1cclxuXHJcblxyXG5cdFx0Ly8gTWFrZSBwb3Atb3V0IHZpc2libGUgc28gd2UgY2FuIHF1ZXJ5IGZvciBpdHMgd2lkdGhcclxuXHRcdHBvcE91dC5hZGRDbGFzcygnaXMtYWN0aXZlJyk7XHJcblxyXG5cdFx0Ly8gQ2hlY2sgaWYgYnJvd3NlciBpcyBsZXNzIHRoYW4gb3IgZXF1YWwgdG8gYHNtYWxsYCBDU1MgYnJlYWtwb2ludFxyXG5cclxuXHRcdHZhciBpc05hcnJvdyA9ICQod2luZG93KS53aWR0aCgpIDw9IDQ4MDtcclxuXHRcdHZhciBpc1RhYmxldCA9ICQod2luZG93KS53aWR0aCgpIDw9IDgwMDtcclxuXHJcblx0XHQvLyBTZXQgc2VwYXJhdGUgdmVydGljYWwvaG9yaXpvbnRhbCBwYWRkaW5nIG9uIG1vYmlsZSB2cy4gZGVza3RvcFxyXG5cdFx0dmFyIHZQYWQgPSBpc05hcnJvdyA/IDEwIDogcmVtO1xyXG5cdFx0dmFyIGhQYWQgPSBpc05hcnJvdyA/IDE0IDogcmVtO1xyXG5cclxuXHRcdC8vIFN0b3JlIG91dHB1dCB2YWx1ZXMgYWZ0ZXIgY2FsY3VsYXRpb25zLCBldGMuXHJcblx0XHR2YXIgcmVzID0ge1xyXG5cdFx0XHRvZmZzZXQ6IHtcclxuXHRcdFx0XHRib3g6IHt9LFxyXG5cdFx0XHRcdHRhYjoge31cclxuXHRcdFx0fSxcclxuXHRcdFx0Y3NzOiB7XHJcblx0XHRcdFx0Ym94OiB7fSxcclxuXHRcdFx0XHR0YWI6IHt9XHJcblx0XHRcdH1cclxuXHRcdH07XHJcblxyXG5cdFx0Ly8gQm94IG9mZnNldCB0b3AgaXMgb2Zmc2V0VG9wIG9mIHRyaWdnZXIsIHBsdXMgdHJpZ2dlciBoZWlnaHQsXHJcblx0XHQvLyBwbHVzIHBhZGRpbmcsIG1pbnVzIDFweCBmb3IgYm9yZGVyIHBvc2l0aW9uaW5nXHJcblx0XHRyZXMub2Zmc2V0LmJveC50b3AgPSBNYXRoLmZsb29yKHRyZ3Iub2Zmc2V0LnRvcCArIHRyZ3Iub2Zmc2V0LmhlaWdodCArICh2UGFkIC0gMSkpO1xyXG5cclxuXHRcdC8vIENoZWNrIGlmIHBvcC1vdXQgd2lsbCBibGVlZCBvZmYtc2NyZWVuLCBjYXVzaW5nIGhvcml6b250YWwgc2Nyb2xsIGJhclxyXG5cdFx0Ly8gSWYgaXQgd2lsbCwgZm9yY2UgcmlnaHQtYWxpZ24gdG8ga2VlcCBpdCBvbi1zY3JlZW5cclxuXHRcdGlmKHBvcE91dC53aWR0aCgpICsgdHJnci5vZmZzZXQubGVmdCA+ICQod2luZG93KS53aWR0aCgpKSB7XHJcblx0XHRcdHRyZ3IuZS5kYXRhKCdwb3Atb3V0LWFsaWduJywgJ3JpZ2h0Jyk7XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gQ2hlY2sgZm9yIHBvcC1vdXQgYWxpZ25tZW50XHJcblx0XHRpZih0cmdyLmUuZGF0YSgncG9wLW91dC1hbGlnbicpID09PSAncmlnaHQnICYmICFpc05hcnJvdykge1xyXG5cdFx0XHQvLyBQb3Atb3V0IGJveCBpcyBmbHVzaCByaWdodCB3aXRoIHRyaWdnZXIgZWxlbWVudFxyXG5cdFx0XHQvLyBUbyBmbHVzaCByaWdodCwgZmlyc3QgYWRkIHRyaWdnZXIgb2Zmc2V0IHBsdXMgdHJpZ2dlciB3aWR0aFxyXG5cdFx0XHQvLyBUaGlzIHBvc2l0aW9ucyBsZWZ0IGVkZ2Ugb2YgcG9wLW91dCB3aXRoIHJpZ2h0IGVkZ2Ugb2YgdHJpZ2dlclxyXG5cdFx0XHQvLyBUaGVuIHN1YnRyYWN0IHBvcC1vdXQgd2lkdGggYW5kIHBhZGRpbmcgdG8gYWxpZ24gYm90aCByaWdodCBlZGdlc1xyXG5cdFx0XHQvLyAoRmx1c2gtbGVmdCBhdXRvbWF0aWNhbGx5IGlmIG5hcnJvdyB3aW5kb3cpXHJcblx0XHRcdHJlcy5vZmZzZXQuYm94LmxlZnQgPSBpc05hcnJvdyA/IDAgOiBNYXRoLmZsb29yKHRyZ3Iub2Zmc2V0LmxlZnQgKyB0cmdyLm9mZnNldC53aWR0aCAtIHBvcE91dC5vZmZzZXQoKS53aWR0aCArIChoUGFkIC0gMSkpO1xyXG5cdFx0XHQvLyBUYWIgbGVmdCBtYXJnaW4gY2FuIGJlIGlnbm9yZWQsIHJpZ2h0IG1hcmdpbiAwIGRvZXMgd2hhdCB3ZSBuZWVkXHJcblx0XHRcdHJlcy5vZmZzZXQudGFiLmxlZnQgPSAnYXV0byc7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHQvLyBQb3Atb3V0IGJveCBpcyBjZW50ZXJlZCB3aXRoIHRyaWdnZXIgZWxlbWVudFxyXG5cdFx0XHQvLyBCb3ggb2Zmc2V0IGxlZnQgaXMgZGV0ZXJtaW5lZCBieSBzdWJ0cmFjdGluZyB0aGUgdHJpZ2dlciB3aWR0aFxyXG5cdFx0XHQvLyBmcm9tIHRoZSBwb3Atb3V0IHdpZHRoLCBkaXZpZGluZyBieSAyIHRvIGZpbmQgdGhlIGhhbGZ3YXkgcG9pbnQsXHJcblx0XHRcdC8vIHRoZW4gc3VidHJhY3RpbmcgdGhhdCBmcm9tIHRoZSB0cmlnZ2VyIGxlZnQgb2Zmc2V0LlxyXG5cdFx0XHQvLyAoRmx1c2gtbGVmdCBhdXRvbWF0aWNhbGx5IGlmIG5hcnJvdyB3aW5kb3cpXHJcblx0XHRcdHJlcy5vZmZzZXQuYm94LmxlZnQgPSBpc05hcnJvdyA/IDAgOiBNYXRoLmZsb29yKHRyZ3Iub2Zmc2V0LmxlZnQgLSAoKHBvcE91dC5vZmZzZXQoKS53aWR0aCAtIHRyZ3Iub2Zmc2V0LndpZHRoKSAvIDIpKTtcclxuXHRcdFx0Ly8gUG9wLW91dCB0YWIgaXMgYWxpZ25lZCB3aXRoIHRyaWdnZXIgbGVmdCBlZGdlLCBhZGp1c3RlZCBmb3IgcGFkZGluZ1xyXG5cdFx0XHQvLyBUYWIgd2lkdGggaXMgc2V0IHRvIHRyaWdnZXIgd2lkdGggYmVsb3csIHNvIHRoaXMgY2VudGVycyB0aGUgdGFiXHJcblx0XHRcdHJlcy5vZmZzZXQudGFiLmxlZnQgPSBpc05hcnJvdyA/IE1hdGguZmxvb3IodHJnci5vZmZzZXQubGVmdCAtIGhQYWQpIDogMDtcclxuXHRcdH1cclxuXHJcblx0XHQvLyBCbG93IHVwIHotaW5kZXggdG8gYXBwZWFyIGFib3ZlIG90aGVyIHRyaWdnZXJzXHJcblx0XHR0cmdyLmUuY3NzKCd6LWluZGV4JywgJzk5OTknKTtcclxuXHJcblx0XHQvLyBCb3ggei1pbmRleCBzZXQgdG8gMiBsb3dlciB0aGFuIHRyaWdnZXIgZWxlbWVudFxyXG5cdFx0Ly8gQm94IHNob3VsZCByZW5kZXIgYmVsb3cgdHJpZ2dlciwgdW5kZXIgdGFiLCBhYm92ZSBldmVyeXRoaW5nIGVsc2VcclxuXHRcdHJlcy5jc3MuYm94LnpJbmRleCA9IHRyZ3IuZS5jc3MoJ3otaW5kZXgnKSAtIDI7XHJcblxyXG5cdFx0Ly8gVGFiIGhlaWdodCBlcXVhbHMgdHJpZ2dlciBoZWlnaHQgcGx1cyBwYWRkaW5nICgxcmVtIHRvcCBhbmQgYm90dG9tKVxyXG5cclxuXHRcdC8vIENoZWNrIGZvciBjdXN0b20gdGFiIHN0eWxlc1xyXG5cdFx0dmFyIHRTID0gdHJnci5oYXNTdHlsZXMgPyB0cmdyLmhhc1N0eWxlcy50YWJTdHlsZXMgOiB1bmRlZmluZWQ7XHJcblxyXG5cdFx0Ly8gSWYgdGhlcmUgYXJlIGN1c3RvbSBzdHlsZXMsIGFuZCBicm93c2VyIGlzIGRlc2t0b3Atd2lkdGguLi5cclxuXHRcdGlmKHRTICYmICFpc05hcnJvdyAmJiAhaXNUYWJsZXQpIHtcclxuXHJcblx0XHRcdHJlcy5jc3MudGFiLmhlaWdodCA9IHRTLmRlc2tIZWlnaHQgfHwgdHJnci5vZmZzZXQuaGVpZ2h0ICsgKHZQYWQgKiAyKSArIFwicHhcIjtcclxuXHJcblx0XHRcdHRTLmRlc2tIZWlnaHRcclxuXHRcdFx0XHQ/IHJlcy5vZmZzZXQuYm94LnRvcCArPSB0Uy5kZXNrSGVpZ2h0IC0gdHJnci5vZmZzZXQuaGVpZ2h0IC0gKHZQYWQgKiAyKVxyXG5cdFx0XHRcdDogbnVsbDtcclxuXHJcblx0XHRcdHJlcy5jc3MudGFiLnRvcCA9IHRTLmRlc2tIZWlnaHRcclxuXHRcdFx0XHQ/ICctJyArICh0Uy5kZXNrSGVpZ2h0IC0gMSkgKyAncHgnXHJcblx0XHRcdFx0OiAnLScgKyAodHJnci5vZmZzZXQuaGVpZ2h0ICsgKHZQYWQgKiAyKSAtIDEpICsgJ3B4JztcclxuXHJcblx0XHQvLyBJZiB0aGVyZSBhcmUgY3VzdG9tIHN0eWxlcywgYW5kIGJyb3dzZXIgaXMgdGFibGV0LXdpZHRoLi4uXHJcblx0XHR9IGVsc2UgaWYodFMgJiYgIWlzTmFycm93ICYmIGlzVGFibGV0KSB7XHJcblxyXG5cdFx0XHRyZXMuY3NzLnRhYi5oZWlnaHQgPSB0Uy50YWJsZXRIZWlnaHQgfHwgdHJnci5vZmZzZXQuaGVpZ2h0ICsgKHZQYWQgKiAyKSArIFwicHhcIjtcclxuXHJcblx0XHRcdHRTLnRhYmxldEhlaWdodFxyXG5cdFx0XHRcdD8gcmVzLm9mZnNldC5ib3gudG9wICs9IHRTLnRhYmxldEhlaWdodCAtIHRyZ3Iub2Zmc2V0LmhlaWdodCAtICh2UGFkICogMilcclxuXHRcdFx0XHQ6IG51bGw7XHJcblxyXG5cdFx0XHRyZXMuY3NzLnRhYi50b3AgPSB0Uy50YWJsZXRIZWlnaHRcclxuXHRcdFx0XHQ/ICctJyArICh0Uy50YWJsZXRIZWlnaHQgLSAxKSArICdweCdcclxuXHRcdFx0XHQ6ICctJyArICh0cmdyLm9mZnNldC5oZWlnaHQgKyAodlBhZCAqIDIpIC0gMSkgKyAncHgnO1xyXG5cclxuXHRcdC8vIElmIHRoZXJlIGFyZSBjdXN0b20gc3R5bGVzLCBhbmQgYnJvd3NlciBpcyBwaG9uZS13aWR0aC4uLlxyXG5cdFx0fSBlbHNlIGlmKHRTICYmIGlzTmFycm93KSB7XHJcblxyXG5cdFx0XHRyZXMuY3NzLnRhYi5oZWlnaHQgPSB0Uy5waG9uZUhlaWdodCB8fCB0cmdyLm9mZnNldC5oZWlnaHQgKyAodlBhZCAqIDIpICsgXCJweFwiO1xyXG5cclxuXHRcdFx0dFMucGhvbmVIZWlnaHRcclxuXHRcdFx0XHQ/IHJlcy5vZmZzZXQuYm94LnRvcCArPSB0Uy5waG9uZUhlaWdodCAtIHRyZ3Iub2Zmc2V0LmhlaWdodCAtICh2UGFkICogMilcclxuXHRcdFx0XHQ6IG51bGw7XHJcblxyXG5cdFx0XHRyZXMuY3NzLnRhYi50b3AgPSB0Uy5waG9uZUhlaWdodFxyXG5cdFx0XHRcdD8gJy0nICsgKHRTLnBob25lSGVpZ2h0IC0gMSkgKyAncHgnXHJcblx0XHRcdFx0OiAnLScgKyAodHJnci5vZmZzZXQuaGVpZ2h0ICsgKHZQYWQgKiAyKSAtIDEpICsgJ3B4JztcclxuXHJcblx0XHQvLyBEZWZhdWx0IHBhZGRpbmcvcG9zaXRpb25pbmdcclxuXHRcdH0gZWxzZSB7XHJcblxyXG5cdFx0XHRyZXMuY3NzLnRhYi5oZWlnaHQgPSB0cmdyLm9mZnNldC5oZWlnaHQgKyAodlBhZCAqIDIpICsgXCJweFwiO1xyXG5cclxuXHRcdFx0Ly8gTW92ZSB0aGUgdGFiIHVwd2FyZHMsIGVxdWFsIHRvIHRoZSB0cmlnZ2VyIGhlaWdodCBwbHVzIHBhZGRpbmdcclxuXHRcdFx0Ly8gbWludXMgMXB4IHRvIGFjY291bnQgZm9yIGJvcmRlciBhbmQgdmlzdWFsbHkgb3ZlcmxhcHBpbmcgYm94XHJcblx0XHRcdHJlcy5jc3MudGFiLnRvcCA9ICctJyArICh0cmdyLm9mZnNldC5oZWlnaHQgKyAodlBhZCAqIDIpIC0gMSkgKyBcInB4XCI7XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gVGFiIHdpZHRoIGVxdWFscyB0cmlnZ2VyIHdpZHRoIHBsdXMgcGFkZGluZyAoMXJlbSBsZWZ0IGFuZCByaWdodClcclxuXHRcdHJlcy5jc3MudGFiLndpZHRoID0gdHJnci5vZmZzZXQud2lkdGggKyAoaFBhZCAqIDIpICsgXCJweFwiO1xyXG5cclxuXHRcdC8vIFRhYiB6LWluZGV4IGlzIDEgbGVzcyB0aGFuIHRyaWdnZXI7IGFib3ZlIGJveCwgYmVsb3cgdHJpZ2dlclxyXG5cdFx0cmVzLmNzcy50YWIuekluZGV4ID0gdHJnci5lLmNzcygnei1pbmRleCcpIC0gMTtcclxuXHJcblx0XHQvLyBgdHJhbnNmb3JtYCB0byBxdWlja2x5IHBvc2l0aW9uIGJveCwgcmVsYXRpdmUgdG8gdG9wIGxlZnQgY29ybmVyXHJcblx0XHRyZXMuY3NzLmJveC50cmFuc2Zvcm0gPSAndHJhbnNsYXRlM2QoJyArIHJlcy5vZmZzZXQuYm94LmxlZnQgKydweCwgJyArIHJlcy5vZmZzZXQuYm94LnRvcCArICdweCwgMCknO1xyXG5cclxuXHRcdC8vIEFwcGx5IHRoYXQgZ2lhbnQgYmxvYiBvZiBDU1NcclxuXHRcdHBvcE91dC5jc3Moe1xyXG5cdFx0XHR6SW5kZXg6IHJlcy5jc3MuYm94LnpJbmRleCxcclxuXHRcdFx0dHJhbnNmb3JtOiByZXMuY3NzLmJveC50cmFuc2Zvcm1cclxuXHRcdH0pLmZpbmQoJy5wb3Atb3V0X190YWInKS5jc3MoeyAvLyBmaW5kIHRoaXMgcG9wLW91dCdzIGNoaWxkIHRhYlxyXG5cdFx0XHRoZWlnaHQ6IHJlcy5jc3MudGFiLmhlaWdodCxcclxuXHRcdFx0d2lkdGg6IHJlcy5jc3MudGFiLndpZHRoLFxyXG5cdFx0XHRsZWZ0OiByZXMub2Zmc2V0LnRhYi5sZWZ0LFxyXG5cdFx0XHRyaWdodDogMCwgLy8gVGhpcyBpcyBhbHdheXMgMFxyXG5cdFx0XHR0b3A6IHJlcy5jc3MudGFiLnRvcCxcclxuXHRcdFx0ekluZGV4OiByZXMuY3NzLnRhYi56SW5kZXhcclxuXHRcdH0pO1xyXG5cdFx0Ly8gVWdseSBoYWNrIGZvciBTYWZhcmkgOCwgYm9vb1xyXG5cdFx0cG9wT3V0LmNzcygnLXdlYmtpdC10cmFuc2Zvcm0nLCByZXMuY3NzLmJveC50cmFuc2Zvcm0pO1xyXG5cclxuXHR9O1xyXG5cclxuXHQvLyBJZiB0aGVyZSBpcyBhbiBhY3RpdmUgcG9wLW91dCwgdXBkYXRlIGl0cyBwb3NpdGlvblxyXG5cdC8vIE1vc3RseSB1c2VmdWwgZm9yIHdoZW4gdGhlIGJyb3dzZXIgd2luZG93IHJlc2l6ZXNcclxuXHR0aGlzLnVwZGF0ZVBvcE91dCA9IGZ1bmN0aW9uKCkge1xyXG5cdFx0aWYoc3RhdGUuYWN0aXZlRWxtKSB7XHJcblx0XHRcdHVwZGF0ZVBvc2l0aW9uKCk7XHJcblx0XHR9XHJcblx0fTtcclxufVxyXG5cclxuZXhwb3J0IGRlZmF1bHQgcG9wT3V0Q29udHJvbGxlcjtcclxuIiwiaW1wb3J0IHsgYW5hbHl0aWNzRXZlbnQgfSBmcm9tICcuL2FuYWx5dGljcy1jb250cm9sbGVyJztcclxuXHJcbmZ1bmN0aW9uIGxvZ2luQ29udHJvbGxlcihyZXF1ZXN0VmVyaWZpY2F0aW9uVG9rZW4pIHtcclxuXHR0aGlzLmFkZFJlZ2lzdGVyVXNlckNvbnRyb2wgPSBmdW5jdGlvbih0cmlnZ2VyRWxlbWVudCwgc3VjY2Vzc0NhbGxiYWNrLCBmYWlsdXJlQ2FsbGJhY2spIHtcclxuXHRcdGlmICh0cmlnZ2VyRWxlbWVudCkge1xyXG5cdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5vbignY2xpY2snLCAoZXZlbnQpID0+IHtcclxuXHRcdFx0XHR0aGlzLmhpZGVFcnJvcnModHJpZ2dlckVsZW1lbnQpO1xyXG5cdFx0XHRcdCQodHJpZ2dlckVsZW1lbnQpLmF0dHIoJ2Rpc2FibGVkJywgJ2Rpc2FibGVkJyk7XHJcblxyXG5cdFx0XHRcdHZhciBpbnB1dERhdGEgPSB7fTtcclxuXHRcdFx0XHR2YXIgdXJsID0gJCh0cmlnZ2VyRWxlbWVudCkuZGF0YSgncmVnaXN0ZXItdXNlci11cmwnKTtcclxuXHJcblx0XHRcdFx0JCh0cmlnZ2VyRWxlbWVudCkucGFyZW50cygnLmpzLXJlZ2lzdGVyLXVzZXItY29udGFpbmVyJykuZmluZCgnaW5wdXQnKS5lYWNoKGZ1bmN0aW9uKCkge1xyXG5cdFx0XHRcdFx0dmFyIHZhbHVlID0gJyc7XHJcblxyXG5cdFx0XHRcdFx0aWYgKCQodGhpcykuZGF0YSgnY2hlY2tib3gtdHlwZScpID09PSAnYm9vbGVhbicpIHtcclxuXHRcdFx0XHRcdFx0dmFsdWUgPSB0aGlzLmNoZWNrZWQ7XHJcblxyXG5cdFx0XHRcdFx0XHRpZiAoJCh0aGlzKS5kYXRhKCdjaGVja2JveC1ib29sZWFuLXR5cGUnKSA9PT0gJ3JldmVyc2UnKSB7XHJcblx0XHRcdFx0XHRcdFx0dmFsdWUgPSAhdmFsdWU7XHJcblx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdGVsc2Uge1xyXG5cdFx0XHRcdFx0XHR2YWx1ZSA9ICQodGhpcykudmFsKCk7XHJcblx0XHRcdFx0XHR9XHJcblxyXG5cdFx0XHRcdFx0aW5wdXREYXRhWyQodGhpcykuYXR0cignbmFtZScpXSA9IHZhbHVlO1xyXG5cdFx0XHRcdH0pO1xyXG5cclxuXHRcdFx0XHQkLmFqYXgoe1xyXG5cdFx0XHRcdFx0dXJsOiB1cmwsXHJcblx0XHRcdFx0XHR0eXBlOiAnUE9TVCcsXHJcblx0XHRcdFx0XHRkYXRhOiBpbnB1dERhdGEsXHJcblx0XHRcdFx0XHRjb250ZXh0OiB0aGlzLFxyXG5cdFx0XHRcdFx0c3VjY2VzczogZnVuY3Rpb24gKHJlc3BvbnNlKSB7XHJcblx0XHRcdFx0XHRcdGlmIChyZXNwb25zZS5zdWNjZXNzKSB7XHJcblxyXG5cdFx0XHRcdFx0XHRcdHZhciByZWdpc3RlckFuYWx5dGljcyA9IHtcclxuXHRcdFx0XHRcdFx0XHRcdGV2ZW50X25hbWU6ICdyZWdpc3Rlci1zdGVwLTEnLFxyXG5cdFx0XHRcdFx0XHRcdFx0cmVnaXN0cmF0aW9uX3N0YXRlOiAnc3VjY2Vzc2Z1bCcsXHJcblx0XHRcdFx0XHRcdFx0XHR1c2VyTmFtZTogJ1wiJyArIGlucHV0RGF0YS51c2VybmFtZSArICdcIidcclxuXHRcdFx0XHRcdFx0XHR9O1xyXG5cclxuXHRcdFx0XHRcdFx0XHRhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIHJlZ2lzdGVyQW5hbHl0aWNzKSApO1xyXG5cclxuXHRcdFx0XHRcdFx0XHRpZiAoc3VjY2Vzc0NhbGxiYWNrKSB7XHJcblx0XHRcdFx0XHRcdFx0XHRzdWNjZXNzQ2FsbGJhY2sodHJpZ2dlckVsZW1lbnQpO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0XHRcdFx0dmFyIG5leHRTdGVwVXJsID0gJCh0cmlnZ2VyRWxlbWVudCkuZGF0YSgnbmV4dC1zdGVwLXVybCcpO1xyXG5cclxuXHRcdFx0XHRcdFx0XHRpZiAobmV4dFN0ZXBVcmwpIHtcclxuXHRcdFx0XHRcdFx0XHRcdHdpbmRvdy5sb2NhdGlvbi5ocmVmID0gbmV4dFN0ZXBVcmw7XHJcblx0XHRcdFx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHRcdFx0XHR0aGlzLnNob3dTdWNjZXNzTWVzc2FnZSh0cmlnZ2VyRWxlbWVudCk7XHJcblx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0ZWxzZSB7XHJcblx0XHRcdFx0XHRcdFx0JCh0cmlnZ2VyRWxlbWVudCkucmVtb3ZlQXR0cignZGlzYWJsZWQnKTtcclxuXHJcblxyXG5cclxuXHRcdFx0XHRcdFx0XHR2YXIgc3BlY2lmaWNFcnJvckRpc3BsYXllZCA9IGZhbHNlO1xyXG5cclxuXHRcdFx0XHRcdFx0XHRpZiAocmVzcG9uc2UucmVhc29ucyAmJiByZXNwb25zZS5yZWFzb25zLmxlbmd0aCA+IDApIHtcclxuXHRcdFx0XHRcdFx0XHRcdGZvciAodmFyIHJlYXNvbiBpbiByZXNwb25zZS5yZWFzb25zKSB7XHJcblx0XHRcdFx0XHRcdFx0XHRcdHRoaXMuc2hvd0Vycm9yKHRyaWdnZXJFbGVtZW50LCAnLmpzLXJlZ2lzdGVyLXVzZXItZXJyb3ItJyArIHJlc3BvbnNlLnJlYXNvbnNbcmVhc29uXSk7XHJcblx0XHRcdFx0XHRcdFx0XHR9XHJcblxyXG5cdFx0XHRcdFx0XHRcdFx0c3BlY2lmaWNFcnJvckRpc3BsYXllZCA9IHRydWU7XHJcblx0XHRcdFx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHRcdFx0XHRpZiAoIXNwZWNpZmljRXJyb3JEaXNwbGF5ZWQpXHJcblx0XHRcdFx0XHRcdFx0e1xyXG5cdFx0XHRcdFx0XHRcdFx0dGhpcy5zaG93RXJyb3IodHJpZ2dlckVsZW1lbnQsICcuanMtcmVnaXN0ZXItdXNlci1lcnJvci1nZW5lcmFsJyk7XHJcblx0XHRcdFx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHRcdFx0XHR2YXIgcmVnaXN0ZXJBbmFseXRpY3MgPSB7XHJcblx0XHRcdFx0XHRcdFx0XHRldmVudF9uYW1lOiBcInJlZ2lzdHJhdGlvbiBmYWlsdXJlXCIsXHJcblx0XHRcdFx0XHRcdFx0XHRyZWdpc3RyYXRpb25fZXJyb3JzOiByZXNwb25zZS5yZWFzb25zXHJcblx0XHRcdFx0XHRcdFx0fTtcclxuXHJcblx0XHRcdFx0XHRcdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCByZWdpc3RlckFuYWx5dGljcykgKTtcclxuXHJcblx0XHRcdFx0XHRcdFx0aWYgKGZhaWx1cmVDYWxsYmFjaykge1xyXG5cdFx0XHRcdFx0XHRcdFx0ZmFpbHVyZUNhbGxiYWNrKHRyaWdnZXJFbGVtZW50KTtcclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdH0sXHJcblx0XHRcdFx0XHRlcnJvcjogZnVuY3Rpb24ocmVzcG9uc2UpIHtcclxuXHRcdFx0XHRcdFx0JCh0cmlnZ2VyRWxlbWVudCkucmVtb3ZlQXR0cignZGlzYWJsZWQnKTtcclxuXHJcblx0XHRcdFx0XHRcdHRoaXMuc2hvd0Vycm9yKHRyaWdnZXJFbGVtZW50LCAnLmpzLXJlZ2lzdGVyLXVzZXItZXJyb3ItZ2VuZXJhbCcpO1xyXG5cclxuXHRcdFx0XHRcdFx0aWYgKGZhaWx1cmVDYWxsYmFjaykge1xyXG5cdFx0XHRcdFx0XHRcdGZhaWx1cmVDYWxsYmFjayh0cmlnZ2VyRWxlbWVudCk7XHJcblx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdH1cclxuXHRcdFx0XHR9KTtcclxuXHRcdFx0fSk7XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0dGhpcy5zaG93U3VjY2Vzc01lc3NhZ2UgPSBmdW5jdGlvbih0cmlnZ2VyRWxlbWVudCkge1xyXG5cdFx0JCh0cmlnZ2VyRWxlbWVudCkucGFyZW50cygnLmpzLXJlZ2lzdGVyLXVzZXItY29udGFpbmVyJykuZmluZCgnLmpzLXJlZ2lzdGVyLXVzZXItc3VjY2VzcycpLnNob3coKTtcclxuXHR9XHJcblxyXG5cdHRoaXMuc2hvd0Vycm9yID0gZnVuY3Rpb24odHJpZ2dlckVsZW1lbnQsIGVycm9yKSB7XHJcblx0XHQkKHRyaWdnZXJFbGVtZW50KS5wYXJlbnRzKCcuanMtcmVnaXN0ZXItdXNlci1jb250YWluZXInKS5maW5kKCcuanMtcmVnaXN0ZXItdXNlci1lcnJvci1jb250YWluZXInKS5zaG93KCk7XHJcblx0XHQkKHRyaWdnZXJFbGVtZW50KS5wYXJlbnRzKCcuanMtcmVnaXN0ZXItdXNlci1jb250YWluZXInKS5maW5kKGVycm9yKS5zaG93KCk7XHJcblx0fVxyXG5cclxuXHR0aGlzLmhpZGVFcnJvcnMgPSBmdW5jdGlvbih0cmlnZ2VyRWxlbWVudCkge1xyXG5cdFx0JCh0cmlnZ2VyRWxlbWVudCkucGFyZW50cygnLmpzLXJlZ2lzdGVyLXVzZXItY29udGFpbmVyJykuZmluZCgnLmpzLXJlZ2lzdGVyLXVzZXItZXJyb3ItY29udGFpbmVyJykuaGlkZSgpO1xyXG5cdFx0JCh0cmlnZ2VyRWxlbWVudCkucGFyZW50cygnLmpzLXJlZ2lzdGVyLXVzZXItY29udGFpbmVyJykuZmluZCgnLmpzLXJlZ2lzdGVyLXVzZXItZXJyb3InKS5oaWRlKCk7XHJcblx0fVxyXG59O1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgbG9naW5Db250cm9sbGVyO1xyXG4iLCJpbXBvcnQgeyBhbmFseXRpY3NFdmVudCB9IGZyb20gJy4vYW5hbHl0aWNzLWNvbnRyb2xsZXInO1xyXG5cclxuZnVuY3Rpb24gbG9naW5Db250cm9sbGVyKHJlcXVlc3RWZXJpZmljYXRpb25Ub2tlbikge1xyXG5cdHRoaXMuYWRkUmVxdWVzdENvbnRyb2wgPSBmdW5jdGlvbih0cmlnZ2VyRWxlbWVudCwgc3VjY2Vzc0NhbGxiYWNrLCBmYWlsdXJlQ2FsbGJhY2spIHtcclxuXHRcdGlmICh0cmlnZ2VyRWxlbWVudCkge1xyXG5cdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5vbignY2xpY2snLCAoZXZlbnQpID0+IHtcclxuXHRcdFx0XHR0aGlzLmhpZGVFcnJvcnModHJpZ2dlckVsZW1lbnQpO1xyXG5cdFx0XHRcdCQodHJpZ2dlckVsZW1lbnQpLmF0dHIoJ2Rpc2FibGVkJywgJ2Rpc2FibGVkJyk7XHJcblxyXG5cdFx0XHRcdHZhciBpbnB1dERhdGEgPSB7fTtcclxuXHRcdFx0XHR2YXIgdXJsID0gJCh0cmlnZ2VyRWxlbWVudCkuZGF0YSgncmVzZXQtdXJsJyk7XHJcblxyXG5cdFx0XHRcdCQodHJpZ2dlckVsZW1lbnQpLnBhcmVudHMoJy5qcy1yZXNldC1wYXNzd29yZC1jb250YWluZXInKS5maW5kKCdpbnB1dCcpLmVhY2goZnVuY3Rpb24oKSB7XHJcblx0XHRcdFx0XHRpbnB1dERhdGFbJCh0aGlzKS5hdHRyKCduYW1lJyldID0gJCh0aGlzKS52YWwoKTtcclxuXHRcdFx0XHR9KVxyXG5cclxuXHRcdFx0XHQkLmFqYXgoe1xyXG5cdFx0XHRcdFx0dXJsOiB1cmwsXHJcblx0XHRcdFx0XHR0eXBlOiAnUE9TVCcsXHJcblx0XHRcdFx0XHRkYXRhOiBpbnB1dERhdGEsXHJcblx0XHRcdFx0XHRjb250ZXh0OiB0aGlzLFxyXG5cdFx0XHRcdFx0c3VjY2VzczogZnVuY3Rpb24gKHJlc3BvbnNlKSB7XHJcblx0XHRcdFx0XHRcdGlmIChyZXNwb25zZS5zdWNjZXNzKSB7XHJcblx0XHRcdFx0XHRcdCAgICB0aGlzLnNob3dTdWNjZXNzTWVzc2FnZSh0cmlnZ2VyRWxlbWVudCk7XHJcblx0XHRcdFx0XHRcdCAgXHJcblx0XHRcdFx0XHRcdFx0XHJcblx0XHRcdFx0XHRcdFx0aWYgKHN1Y2Nlc3NDYWxsYmFjaykge1xyXG5cdFx0XHRcdFx0XHRcdFx0c3VjY2Vzc0NhbGxiYWNrKHRyaWdnZXJFbGVtZW50KTtcclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0ZWxzZSB7XHJcblx0XHRcdFx0XHRcdFx0JCh0cmlnZ2VyRWxlbWVudCkucmVtb3ZlQXR0cignZGlzYWJsZWQnKTtcclxuXHRcdFx0XHRcdFx0XHR2YXIgcmVzZXRQYXNzd29yZEFuYWx5dGljcyA9IHtcclxuXHRcdFx0XHRcdFx0XHQgICAgZXZlbnRfbmFtZTogXCJwYXNzd29yZCByZXNldCB1bnN1Y2Nlc3NmdWxcIlx0XHRcdFx0XHRcdCAgICAgICBcclxuXHRcdFx0XHRcdFx0XHR9O1xyXG5cclxuXHRcdFx0XHRcdFx0XHR2YXIgc3BlY2lmaWNFcnJvckRpc3BsYXllZCA9IGZhbHNlO1xyXG5cclxuXHRcdFx0XHRcdFx0XHRpZiAoJC5pbkFycmF5KCdFbWFpbFJlcXVpcmVtZW50JywgcmVzcG9uc2UucmVhc29ucykgIT09IC0xKVxyXG5cdFx0XHRcdFx0XHRcdHtcclxuXHRcdFx0XHRcdFx0XHRcdHRoaXMuc2hvd0Vycm9yKHRyaWdnZXJFbGVtZW50LCAnLmpzLXJlc2V0LXBhc3N3b3JkLWVycm9yLWVtYWlsJyk7XHJcblx0XHRcdFx0XHRcdFx0XHRzcGVjaWZpY0Vycm9yRGlzcGxheWVkID0gdHJ1ZTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuXHJcblx0XHRcdFx0XHRcdFx0aWYgKCFzcGVjaWZpY0Vycm9yRGlzcGxheWVkKVxyXG5cdFx0XHRcdFx0XHRcdHtcclxuXHRcdFx0XHRcdFx0XHRcdHRoaXMuc2hvd0Vycm9yKHRyaWdnZXJFbGVtZW50LCAnLmpzLXJlc2V0LXBhc3N3b3JkLWVycm9yLWdlbmVyYWwnKTtcclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdFx0XHJcblx0XHRcdFx0XHRcdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCByZXNldFBhc3N3b3JkQW5hbHl0aWNzKSApO1xyXG5cclxuXHRcdFx0XHRcdFx0XHRpZiAoZmFpbHVyZUNhbGxiYWNrKSB7XHJcblx0XHRcdFx0XHRcdFx0XHRmYWlsdXJlQ2FsbGJhY2sodHJpZ2dlckVsZW1lbnQpO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFx0fSxcclxuXHRcdFx0XHRcdGVycm9yOiBmdW5jdGlvbihyZXNwb25zZSkge1xyXG5cdFx0XHRcdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5yZW1vdmVBdHRyKCdkaXNhYmxlZCcpO1xyXG5cdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdFx0dGhpcy5zaG93RXJyb3IodHJpZ2dlckVsZW1lbnQsICcuanMtcmVzZXQtcGFzc3dvcmQtZXJyb3ItZ2VuZXJhbCcpO1xyXG5cclxuXHRcdFx0XHRcdFx0aWYgKGZhaWx1cmVDYWxsYmFjaykge1xyXG5cdFx0XHRcdFx0XHRcdGZhaWx1cmVDYWxsYmFjayh0cmlnZ2VyRWxlbWVudCk7XHJcblx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdH1cclxuXHRcdFx0XHR9KTtcclxuXHRcdFx0fSk7XHJcblx0XHR9XHJcblx0fTtcclxuXHJcblx0dGhpcy5hZGRDaGFuZ2VDb250cm9sID0gZnVuY3Rpb24odHJpZ2dlckVsZW1lbnQsIHN1Y2Nlc3NDYWxsYmFjaywgZmFpbHVyZUNhbGxiYWNrKSB7XHJcblx0XHRpZiAodHJpZ2dlckVsZW1lbnQpIHtcclxuXHRcdFx0JCh0cmlnZ2VyRWxlbWVudCkub24oJ2NsaWNrJywgKGV2ZW50KSA9PiB7XHJcblx0XHRcdFx0dGhpcy5oaWRlRXJyb3JzKHRyaWdnZXJFbGVtZW50KTtcclxuXHRcdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5hdHRyKCdkaXNhYmxlZCcsICdkaXNhYmxlZCcpO1xyXG5cclxuXHRcdFx0XHR2YXIgaW5wdXREYXRhID0ge307XHJcblx0XHRcdFx0dmFyIHVybCA9ICQodHJpZ2dlckVsZW1lbnQpLmRhdGEoJ3Jlc2V0LXVybCcpO1xyXG5cclxuXHRcdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5wYXJlbnRzKCcuanMtcmVzZXQtcGFzc3dvcmQtY29udGFpbmVyJykuZmluZCgnaW5wdXQnKS5lYWNoKGZ1bmN0aW9uKCkge1xyXG5cdFx0XHRcdFx0aW5wdXREYXRhWyQodGhpcykuYXR0cignbmFtZScpXSA9ICQodGhpcykudmFsKCk7XHJcblx0XHRcdFx0fSlcclxuXHJcblx0XHRcdFx0JC5hamF4KHtcclxuXHRcdFx0XHRcdHVybDogdXJsLFxyXG5cdFx0XHRcdFx0dHlwZTogJ1BPU1QnLFxyXG5cdFx0XHRcdFx0ZGF0YTogaW5wdXREYXRhLFxyXG5cdFx0XHRcdFx0Y29udGV4dDogdGhpcyxcclxuXHRcdFx0XHRcdHN1Y2Nlc3M6IGZ1bmN0aW9uIChyZXNwb25zZSkge1xyXG5cdFx0XHRcdFx0XHRpZiAocmVzcG9uc2Uuc3VjY2Vzcykge1xyXG5cdFx0XHRcdFx0XHRcdHRoaXMuc2hvd1N1Y2Nlc3NNZXNzYWdlKHRyaWdnZXJFbGVtZW50KTtcclxuXHRcdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdFx0XHRpZiAoc3VjY2Vzc0NhbGxiYWNrKSB7XHJcblx0XHRcdFx0XHRcdFx0XHRzdWNjZXNzQ2FsbGJhY2sodHJpZ2dlckVsZW1lbnQpO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFx0XHRlbHNlIHtcclxuXHRcdFx0XHRcdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5yZW1vdmVBdHRyKCdkaXNhYmxlZCcpO1xyXG5cdFx0XHRcdFx0XHRcdFxyXG5cdFx0XHRcdFx0XHRcdHZhciBzcGVjaWZpY0Vycm9yRGlzcGxheWVkID0gZmFsc2U7XHJcblxyXG5cdFx0XHRcdFx0XHRcdGlmICgkLmluQXJyYXkoJ1Bhc3N3b3JkTWlzbWF0Y2gnLCByZXNwb25zZS5yZWFzb25zKSAhPT0gLTEpXHJcblx0XHRcdFx0XHRcdFx0e1xyXG5cdFx0XHRcdFx0XHRcdFx0dGhpcy5zaG93RXJyb3IodHJpZ2dlckVsZW1lbnQsICcuanMtcmVzZXQtcGFzc3dvcmQtZXJyb3ItbWlzbWF0Y2gnKTtcclxuXHRcdFx0XHRcdFx0XHRcdHNwZWNpZmljRXJyb3JEaXNwbGF5ZWQgPSB0cnVlO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0XHRpZiAoJC5pbkFycmF5KCdQYXNzd29yZFJlcXVpcmVtZW50cycsIHJlc3BvbnNlLnJlYXNvbnMpICE9PSAtMSlcclxuXHRcdFx0XHRcdFx0XHR7XHJcblx0XHRcdFx0XHRcdFx0XHR0aGlzLnNob3dFcnJvcih0cmlnZ2VyRWxlbWVudCwgJy5qcy1yZXNldC1wYXNzd29yZC1lcnJvci1yZXF1aXJlbWVudHMnKTtcclxuXHRcdFx0XHRcdFx0XHRcdHNwZWNpZmljRXJyb3JEaXNwbGF5ZWQgPSB0cnVlO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0XHRcdFx0aWYgKCFzcGVjaWZpY0Vycm9yRGlzcGxheWVkIHx8ICgkLmluQXJyYXkoJ01pc3NpbmdUb2tlbicsIHJlc3BvbnNlLnJlYXNvbnMpICE9PSAtMSkpXHJcblx0XHRcdFx0XHRcdFx0e1xyXG5cdFx0XHRcdFx0XHRcdFx0dGhpcy5zaG93RXJyb3IodHJpZ2dlckVsZW1lbnQsICcuanMtcmVzZXQtcGFzc3dvcmQtZXJyb3ItZ2VuZXJhbCcpO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0XHRcdFx0aWYgKGZhaWx1cmVDYWxsYmFjaykge1xyXG5cdFx0XHRcdFx0XHRcdFx0ZmFpbHVyZUNhbGxiYWNrKHRyaWdnZXJFbGVtZW50KTtcclxuXHRcdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdH0sXHJcblx0XHRcdFx0XHRlcnJvcjogZnVuY3Rpb24ocmVzcG9uc2UpIHtcclxuXHRcdFx0XHRcdFx0JCh0cmlnZ2VyRWxlbWVudCkucmVtb3ZlQXR0cignZGlzYWJsZWQnKTtcclxuXHRcdFx0XHRcdFx0XHJcblx0XHRcdFx0XHRcdHRoaXMuc2hvd0Vycm9yKHRyaWdnZXJFbGVtZW50LCAnLmpzLXJlc2V0LXBhc3N3b3JkLWVycm9yLWdlbmVyYWwnKTtcclxuXHJcblx0XHRcdFx0XHRcdGlmIChmYWlsdXJlQ2FsbGJhY2spIHtcclxuXHRcdFx0XHRcdFx0XHRmYWlsdXJlQ2FsbGJhY2sodHJpZ2dlckVsZW1lbnQpO1xyXG5cdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHR9XHJcblx0XHRcdFx0fSk7XHJcblx0XHRcdH0pO1xyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdHRoaXMuYWRkUmV0cnlDb250cm9sID0gZnVuY3Rpb24odHJpZ2dlckVsZW1lbnQsIHN1Y2Nlc3NDYWxsYmFjaywgZmFpbHVyZUNhbGxiYWNrKSB7XHJcblx0XHRpZiAodHJpZ2dlckVsZW1lbnQpIHtcclxuXHRcdFx0JCh0cmlnZ2VyRWxlbWVudCkub24oJ2NsaWNrJywgKGV2ZW50KSA9PiB7XHJcblx0XHRcdFx0dGhpcy5oaWRlRXJyb3JzKHRyaWdnZXJFbGVtZW50KTtcclxuXHRcdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5hdHRyKCdkaXNhYmxlZCcsICdkaXNhYmxlZCcpO1xyXG5cclxuXHRcdFx0XHR2YXIgaW5wdXREYXRhID0ge307XHJcblx0XHRcdFx0dmFyIHVybCA9ICQodHJpZ2dlckVsZW1lbnQpLmRhdGEoJ3JldHJ5LXVybCcpO1xyXG5cclxuXHRcdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5wYXJlbnRzKCcuanMtcmVzZXQtcGFzc3dvcmQtY29udGFpbmVyJykuZmluZCgnaW5wdXQnKS5lYWNoKGZ1bmN0aW9uKCkge1xyXG5cdFx0XHRcdFx0aW5wdXREYXRhWyQodGhpcykuYXR0cignbmFtZScpXSA9ICQodGhpcykudmFsKCk7XHJcblx0XHRcdFx0fSlcclxuXHJcblx0XHRcdFx0JC5hamF4KHtcclxuXHRcdFx0XHRcdHVybDogdXJsLFxyXG5cdFx0XHRcdFx0dHlwZTogJ1BPU1QnLFxyXG5cdFx0XHRcdFx0ZGF0YTogaW5wdXREYXRhLFxyXG5cdFx0XHRcdFx0Y29udGV4dDogdGhpcyxcclxuXHRcdFx0XHRcdHN1Y2Nlc3M6IGZ1bmN0aW9uIChyZXNwb25zZSkge1xyXG5cdFx0XHRcdFx0XHRpZiAocmVzcG9uc2Uuc3VjY2Vzcykge1xyXG5cdFx0XHRcdFx0XHRcdHRoaXMuc2hvd1N1Y2Nlc3NNZXNzYWdlKHRyaWdnZXJFbGVtZW50KTtcclxuXHRcdFx0XHRcdFx0XHRcclxuXHRcdFx0XHRcdFx0XHRpZiAoc3VjY2Vzc0NhbGxiYWNrKSB7XHJcblx0XHRcdFx0XHRcdFx0XHRzdWNjZXNzQ2FsbGJhY2sodHJpZ2dlckVsZW1lbnQpO1xyXG5cdFx0XHRcdFx0XHRcdH1cclxuXHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFx0XHRlbHNlIHtcclxuXHRcdFx0XHRcdFx0XHQkKHRyaWdnZXJFbGVtZW50KS5yZW1vdmVBdHRyKCdkaXNhYmxlZCcpO1xyXG5cclxuXHRcdFx0XHRcdFx0XHR0aGlzLnNob3dFcnJvcih0cmlnZ2VyRWxlbWVudCwgJy5qcy1yZXNldC1wYXNzd29yZC1lcnJvci1nZW5lcmFsJyk7XHJcblxyXG5cdFx0XHRcdFx0XHRcdGlmIChmYWlsdXJlQ2FsbGJhY2spIHtcclxuXHRcdFx0XHRcdFx0XHRcdGZhaWx1cmVDYWxsYmFjayh0cmlnZ2VyRWxlbWVudCk7XHJcblx0XHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFx0XHR9XHJcblx0XHRcdFx0XHR9LFxyXG5cdFx0XHRcdFx0ZXJyb3I6IGZ1bmN0aW9uKHJlc3BvbnNlKSB7XHJcblx0XHRcdFx0XHRcdCQodHJpZ2dlckVsZW1lbnQpLnJlbW92ZUF0dHIoJ2Rpc2FibGVkJyk7XHJcblx0XHRcdFx0XHRcdFxyXG5cdFx0XHRcdFx0XHR0aGlzLnNob3dFcnJvcih0cmlnZ2VyRWxlbWVudCwgJy5qcy1yZXNldC1wYXNzd29yZC1lcnJvci1nZW5lcmFsJyk7XHJcblxyXG5cdFx0XHRcdFx0XHRpZiAoZmFpbHVyZUNhbGxiYWNrKSB7XHJcblx0XHRcdFx0XHRcdFx0ZmFpbHVyZUNhbGxiYWNrKHRyaWdnZXJFbGVtZW50KTtcclxuXHRcdFx0XHRcdFx0fVxyXG5cdFx0XHRcdFx0fVxyXG5cdFx0XHRcdH0pO1xyXG5cdFx0XHR9KTtcclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHR0aGlzLnNob3dTdWNjZXNzTWVzc2FnZSA9IGZ1bmN0aW9uKHRyaWdnZXJFbGVtZW50KSB7XHJcblx0ICAgICQodHJpZ2dlckVsZW1lbnQpLnBhcmVudHMoJy5qcy1yZXNldC1wYXNzd29yZC1jb250YWluZXInKS5maW5kKCcuanMtcmVzZXQtcGFzc3dvcmQtc3VjY2VzcycpLnNob3coKTtcclxuXHQgICAgdmFyIHJlc2V0UGFzc3dvcmRBbmFseXRpY3MgPSB7XHJcblx0ICAgICAgICBldmVudF9uYW1lOiBcInBhc3N3b3JkIHJlc2V0IHN1Y2Nlc3NcIlx0XHRcdFx0XHRcdCAgICAgICBcclxuXHQgICAgfTtcclxuXHJcblx0ICAgIGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgcmVzZXRQYXNzd29yZEFuYWx5dGljcykgKTtcclxuXHR9XHJcblxyXG5cdHRoaXMuc2hvd0Vycm9yID0gZnVuY3Rpb24odHJpZ2dlckVsZW1lbnQsIGVycm9yKSB7XHJcblx0XHQkKHRyaWdnZXJFbGVtZW50KS5wYXJlbnRzKCcuanMtcmVzZXQtcGFzc3dvcmQtY29udGFpbmVyJykuZmluZCgnLmpzLXJlc2V0LXBhc3N3b3JkLWVycm9yLWNvbnRhaW5lcicpLnNob3coKTtcclxuXHRcdCQodHJpZ2dlckVsZW1lbnQpLnBhcmVudHMoJy5qcy1yZXNldC1wYXNzd29yZC1jb250YWluZXInKS5maW5kKGVycm9yKS5zaG93KCk7XHJcblx0XHR2YXIgcmVzZXRQYXNzd29yZEFuYWx5dGljcyA9IHtcclxuXHRcdCAgICBldmVudF9uYW1lOiBcInBhc3N3b3JkIHJlc2V0IHVuc3VjY2Vzc2Z1bFwiXHRcclxuICAgICAgICAgfTtcclxuXHJcblx0XHRhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIHJlc2V0UGFzc3dvcmRBbmFseXRpY3MpICk7XHJcblx0fVxyXG5cdFxyXG5cdHRoaXMuaGlkZUVycm9ycyA9IGZ1bmN0aW9uKHRyaWdnZXJFbGVtZW50KSB7XHJcblx0XHQkKHRyaWdnZXJFbGVtZW50KS5wYXJlbnRzKCcuanMtcmVzZXQtcGFzc3dvcmQtY29udGFpbmVyJykuZmluZCgnLmpzLXJlc2V0LXBhc3N3b3JkLWVycm9yLWNvbnRhaW5lcicpLmhpZGUoKTtcclxuXHRcdCQodHJpZ2dlckVsZW1lbnQpLnBhcmVudHMoJy5qcy1yZXNldC1wYXNzd29yZC1jb250YWluZXInKS5maW5kKCcuanMtcmVzZXQtcGFzc3dvcmQtZXJyb3InKS5oaWRlKCk7XHJcblx0fVxyXG59O1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgbG9naW5Db250cm9sbGVyOyIsImZ1bmN0aW9uIHNvcnRhYmxlVGFibGVDb250cm9sbGVyKCkge1xyXG5cclxuXHQvKlxyXG5cdEJhc2VkIG9uIFNvcnRUYWJsZSB2ZXJzaW9uIDJcclxuXHQ3dGggQXByaWwgMjAwN1xyXG5cdFN0dWFydCBMYW5ncmlkZ2UsIGh0dHA6Ly93d3cua3J5b2dlbml4Lm9yZy9jb2RlL2Jyb3dzZXIvc29ydHRhYmxlL1xyXG5cdExpY2VuY2VkIGFzIFgxMTogaHR0cDovL3d3dy5rcnlvZ2VuaXgub3JnL2NvZGUvYnJvd3Nlci9saWNlbmNlLmh0bWxcclxuXHQqL1xyXG5cclxuXHR2YXIgaXNTb3J0ZWRUYWJsZSA9IGZhbHNlO1xyXG5cdHZhciB0Zm8sIG10Y2gsIHNvcnRmbiwgaGFzSW5wdXRzO1xyXG5cclxuXHR2YXIgc29ydHRhYmxlID0ge1xyXG5cclxuXHRcdGluaXQ6IGZ1bmN0aW9uIGluaXRpbmcoKSB7XHJcblx0XHRcdC8vIHF1aXQgaWYgdGhpcyBmdW5jdGlvbiBoYXMgYWxyZWFkeSBiZWVuIGNhbGxlZFxyXG5cdFx0XHRpZiAoaXNTb3J0ZWRUYWJsZSkgcmV0dXJuO1xyXG5cdFx0XHQvLyBmbGFnIHRoaXMgZnVuY3Rpb24gc28gd2UgZG9uJ3QgZG8gdGhlIHNhbWUgdGhpbmcgdHdpY2VcclxuXHRcdFx0aXNTb3J0ZWRUYWJsZSA9IHRydWU7XHJcblxyXG5cdFx0XHQkKCcuanMtc29ydGFibGUtdGFibGUnKS5lYWNoKGZ1bmN0aW9uIChpbmR4LCBpdGVtKSB7XHJcblx0XHRcdFx0c29ydHRhYmxlLm1ha2VTb3J0YWJsZShpdGVtKTtcclxuXHRcdFx0fSk7XHJcblxyXG5cdFx0fSxcclxuXHJcblx0XHRzb3J0Q29sdW1uOiBmdW5jdGlvbih0YWJsZSwgY29sKSB7XHJcblxyXG5cdFx0XHQvLyBidWlsZCBhbiBhcnJheSB0byBzb3J0LiBUaGlzIGlzIGEgU2Nod2FydHppYW4gdHJhbnNmb3JtIHRoaW5nLFxyXG5cdFx0XHQvLyBpLmUuLCB3ZSBcImRlY29yYXRlXCIgZWFjaCByb3cgd2l0aCB0aGUgYWN0dWFsIHNvcnQga2V5LFxyXG5cdFx0XHQvLyBzb3J0IGJhc2VkIG9uIHRoZSBzb3J0IGtleXMsIGFuZCB0aGVuIHB1dCB0aGUgcm93cyBiYWNrIGluIG9yZGVyXHJcblx0XHRcdC8vIHdoaWNoIGlzIGEgbG90IGZhc3RlciBiZWNhdXNlIHlvdSBvbmx5IGRvIGdldElubmVyVGV4dCBvbmNlIHBlciByb3dcclxuXHJcblx0XHRcdHZhciByb3dfYXJyYXkgPSBbXTtcclxuXHRcdFx0dmFyIGhlYWRyb3cgPSB0YWJsZS50SGVhZC5yb3dzWzBdLmNlbGxzO1xyXG5cdFx0XHR2YXIgcm93cyA9IFtdLnNsaWNlLmNhbGwodGFibGUudEJvZGllc1swXS5yb3dzKTtcclxuXHRcdFx0dmFyIGd1ZXNzdHlwZSA9IHNvcnR0YWJsZS5ndWVzc1R5cGUodGFibGUsIGNvbCk7XHJcblxyXG5cdFx0XHRmb3IgKHZhciBqID0gMDsgaiA8IHJvd3MubGVuZ3RoOyBqKyspIHtcclxuXHRcdFx0XHRyb3dfYXJyYXlbcm93X2FycmF5Lmxlbmd0aF0gPSBbJChyb3dzW2pdLmNlbGxzW2NvbF0pLCByb3dzW2pdXTtcclxuXHRcdFx0fVxyXG5cclxuXHJcblx0XHRcdGlmKCQoaGVhZHJvd1tjb2xdKS5kYXRhKCdzb3J0YWJsZS10eXBlJykpIHtcclxuXHRcdFx0XHRyb3dfYXJyYXkuc29ydChzb3J0dGFibGVbJChoZWFkcm93W2NvbF0pLmRhdGEoJ3NvcnRhYmxlLXR5cGUnKV0pO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdHJvd19hcnJheS5zb3J0KGd1ZXNzdHlwZSk7XHJcblx0XHRcdH1cclxuXHJcblx0XHRcdHZhciB0YiA9IHRhYmxlLnRCb2RpZXNbMF07XHJcblx0XHRcdGZvciAodmFyIGogPSAwOyBqIDwgcm93X2FycmF5Lmxlbmd0aDsgaisrKSB7XHJcblx0XHRcdFx0dGIuYXBwZW5kQ2hpbGQocm93X2FycmF5W2pdWzFdKTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0cm93X2FycmF5ID0gdW5kZWZpbmVkO1xyXG5cdFx0fSxcclxuXHJcblx0XHRtYWtlU29ydGFibGU6IGZ1bmN0aW9uKHRhYmxlKSB7XHJcblxyXG5cdFx0XHQvLyBTb3J0dGFibGUgdjEgcHV0IHJvd3Mgd2l0aCBhIGNsYXNzIG9mIFwic29ydGJvdHRvbVwiIGF0IHRoZSBib3R0b20gKGFzXHJcblx0XHRcdC8vIFwidG90YWxcIiByb3dzLCBmb3IgZXhhbXBsZSkuIFRoaXMgaXMgQiZSLCBzaW5jZSB3aGF0IHlvdSdyZSBzdXBwb3NlZFxyXG5cdFx0XHQvLyB0byBkbyBpcyBwdXQgdGhlbSBpbiBhIHRmb290LiBTbywgaWYgdGhlcmUgYXJlIHNvcnRib3R0b20gcm93cyxcclxuXHRcdFx0Ly8gZm9yIGJhY2t3YXJkcyBjb21wYXRpYmlsaXR5LCBtb3ZlIHRoZW0gdG8gdGZvb3QgKGNyZWF0aW5nIGl0IGlmIG5lZWRlZCkuXHJcblx0XHRcdHZhciBzb3J0Ym90dG9tcm93cyA9IFtdO1xyXG5cdFx0XHRmb3IgKHZhciBpID0gMDsgaSA8IHRhYmxlLnJvd3MubGVuZ3RoOyBpKyspIHtcclxuXHRcdFx0XHRpZiAoJCh0YWJsZS5yb3dzW2ldKS5oYXNDbGFzcygnLnNvcnRib3R0b20nKSkge1xyXG5cdFx0XHRcdFx0c29ydGJvdHRvbXJvd3Nbc29ydGJvdHRvbXJvd3MubGVuZ3RoXSA9IHRhYmxlLnJvd3NbaV07XHJcblx0XHRcdFx0fVxyXG5cdFx0XHR9XHJcblxyXG5cdFx0XHRpZiAoc29ydGJvdHRvbXJvd3MpIHtcclxuXHRcdFx0XHRpZiAodGFibGUudEZvb3QgPT0gbnVsbCkge1xyXG5cdFx0XHRcdFx0Ly8gdGFibGUgZG9lc24ndCBoYXZlIGEgdGZvb3QuIENyZWF0ZSBvbmUuXHJcblx0XHRcdFx0XHR0Zm8gPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCd0Zm9vdCcpO1xyXG5cdFx0XHRcdFx0dGFibGUuYXBwZW5kQ2hpbGQodGZvKTtcclxuXHRcdFx0XHR9XHJcblx0XHRcdFx0Zm9yICh2YXIgaiA9IDA7IGogPCBzb3J0Ym90dG9tcm93cy5sZW5ndGg7IGorKykge1xyXG5cdFx0XHRcdFx0dGZvLmFwcGVuZENoaWxkKHNvcnRib3R0b21yb3dzW2pdKTtcclxuXHRcdFx0XHR9XHJcblx0XHRcdFx0c29ydGJvdHRvbXJvd3MgPSB1bmRlZmluZWQ7XHJcblx0XHRcdH1cclxuXHJcblx0XHRcdC8vIHdvcmsgdGhyb3VnaCBlYWNoIGNvbHVtbiBhbmQgY2FsY3VsYXRlIGl0cyB0eXBlXHJcblx0XHRcdHZhciBoZWFkcm93ID0gdGFibGUudEhlYWQucm93c1swXS5jZWxscztcclxuXHRcdFx0Zm9yICh2YXIgaSA9IDA7IGkgPCBoZWFkcm93Lmxlbmd0aDsgaSsrKSB7XHJcblxyXG5cdFx0XHRcdC8vIG1hbnVhbGx5IG92ZXJyaWRlIHRoZSB0eXBlIHdpdGggYSBzb3J0dGFibGVfdHlwZSBhdHRyaWJ1dGVcclxuXHRcdFx0XHRpZiAoIWhlYWRyb3dbaV0uY2xhc3NOYW1lLm1hdGNoKC9cXGJzb3J0dGFibGVfbm9zb3J0XFxiLykpIHsgLy8gc2tpcCB0aGlzIGNvbFxyXG5cdFx0XHRcdFx0aGVhZHJvd1tpXS5zb3J0dGFibGVfc29ydGZ1bmN0aW9uID0gc29ydHRhYmxlLmd1ZXNzVHlwZSh0YWJsZSxpKTtcclxuXHRcdFx0XHR9XHJcblxyXG5cdFx0XHR9O1xyXG5cclxuXHRcdFx0JCh0YWJsZSkuZmluZCgnLmpzLXNvcnRhYmxlLXRhYmxlLXNvcnRlcicpLm9uKCdjbGljaycsIGZ1bmN0aW9uKGUpIHtcclxuXHJcblx0XHRcdFx0Ly8gSWYgY2hpbGQgZWxlbWVudCBpcyBjbGlja2VkLCByZWRpcmVjdCB0aGUgY2xpY2sgdG8gdGhlXHJcblx0XHRcdFx0Ly8gcHJvcGVyIGVsZW1lbnQ6IHRoZSBwYXJlbnQgaXRzZWxmLlxyXG5cdFx0XHRcdGlmKGUudGFyZ2V0ICE9PSB0aGlzKSB7XHJcblx0XHRcdFx0XHR0aGlzLmNsaWNrKCk7XHJcblx0XHRcdFx0XHRyZXR1cm47XHJcblx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHR2YXIgY29sTnVtID0gJChlLnRhcmdldCkuY2xvc2VzdCgnLmpzLXNvcnRhYmxlLXRhYmxlLXNvcnRlcicpLmRhdGEoJ3NvcnRhYmxlLXRhYmxlLWNvbCcpIC0gMTtcclxuXHJcblxyXG5cclxuXHRcdFx0XHRpZiAoJChlLnRhcmdldCkuaGFzQ2xhc3MoJ3NvcnR0YWJsZV9zb3J0ZWQnKSkge1xyXG5cdFx0XHRcdFx0Ly8gVGhpcyBjb2x1bW4gaXMgc29ydGVkIHRvcCB0byBib3R0b21cclxuXHRcdFx0XHRcdC8vIFJlLXNvcnQgdGhlIGNvbHVtbiB0byBjYXRjaCBhbnkgcm93IGNoYW5nZXMuLi5cclxuXHRcdFx0XHRcdHNvcnR0YWJsZS5zb3J0Q29sdW1uKHRhYmxlLCBjb2xOdW0pO1xyXG5cdFx0XHRcdFx0Ly8gLi4udGhlbiByZXZlcnNlIHRoZSBjb2x1bW4gYW5kIHVwZGF0ZSB0aGUgY2xhc3NlcyAoc3RhdGUpLlxyXG5cdFx0XHRcdFx0c29ydHRhYmxlLnJldmVyc2UodGFibGUudEJvZGllc1swXSk7XHJcblx0XHRcdFx0XHQkKGUudGFyZ2V0KS5yZW1vdmVDbGFzcygnc29ydHRhYmxlX3NvcnRlZCcpLmFkZENsYXNzKCdzb3J0dGFibGVfc29ydGVkX3JldmVyc2UnKTtcclxuXHRcdFx0XHRcdFxyXG5cdFx0XHRcdFx0cmV0dXJuO1xyXG5cdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0aWYgKCQoZS50YXJnZXQpLmhhc0NsYXNzKCdzb3J0dGFibGVfc29ydGVkX3JldmVyc2UnKSkge1xyXG5cdFx0XHRcdFx0Ly8gVGhpcyBjb2x1bW4gaXMgc29ydGVkIGJvdHRvbSB0byB0b3BcclxuXHRcdFx0XHRcdC8vIEZsaXAgdGhlIHRhYmxlIGJhY2sgdG8gdG9wLXRvLWJvdHRvbSAoZGVmYXVsdCkuLi5cclxuXHRcdFx0XHRcdHNvcnR0YWJsZS5yZXZlcnNlKHRhYmxlLnRCb2RpZXNbMF0pO1xyXG5cdFx0XHRcdFx0Ly8gLi4udGhlbiByZS1zb3J0IGl0IHRvIGNhdGNoIGFueSByb3cgY2hhbmdlcy5cclxuXHRcdFx0XHRcdHNvcnR0YWJsZS5zb3J0Q29sdW1uKHRhYmxlLCBjb2xOdW0pO1xyXG5cdFx0XHRcdFx0JChlLnRhcmdldCkucmVtb3ZlQ2xhc3MoJ3NvcnR0YWJsZV9zb3J0ZWRfcmV2ZXJzZScpLmFkZENsYXNzKCdzb3J0dGFibGVfc29ydGVkJyk7XHJcblxyXG5cdFx0XHRcdFx0cmV0dXJuO1xyXG5cdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0Ly8gcmVtb3ZlIHNvcnR0YWJsZV9zb3J0ZWQgY2xhc3Nlc1xyXG5cdFx0XHRcdHZhciB0aGVhZHJvdyA9IGUudGFyZ2V0LnBhcmVudE5vZGU7XHJcblx0XHRcdFx0Zm9yRWFjaCh0aGVhZHJvdy5jaGlsZE5vZGVzLCBmdW5jdGlvbihjZWxsKSB7XHJcblx0XHRcdFx0XHRpZiAoY2VsbC5ub2RlVHlwZSA9PSAxKSB7IC8vIGFuIGVsZW1lbnRcclxuXHRcdFx0XHRcdFx0JChjZWxsKS5yZW1vdmVDbGFzcygnc29ydHRhYmxlX3NvcnRlZF9yZXZlcnNlIHNvcnR0YWJsZV9zb3J0ZWQnKTtcclxuXHRcdFx0XHRcdH1cclxuXHRcdFx0XHR9KTtcclxuXHJcblx0XHRcdFx0aWYgKCQoJy5zb3J0dGFibGVfc29ydGZ3ZGluZCcpKSB7XHJcblx0XHRcdFx0XHQkKCcuc29ydHRhYmxlX3NvcnRmd2RpbmQnKS5yZW1vdmUoKTtcclxuXHRcdFx0XHR9XHJcblxyXG5cdFx0XHRcdGlmICgkKCcuc29ydHRhYmxlX3NvcnRyZXZpbmQnKSkge1xyXG5cdFx0XHRcdFx0JCgnLnNvcnR0YWJsZV9zb3J0cmV2aW5kJykucmVtb3ZlKCk7XHJcblx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHQkKGUudGFyZ2V0KS5hZGRDbGFzcygnc29ydHRhYmxlX3NvcnRlZCcpO1xyXG5cclxuXHRcdFx0XHRzb3J0dGFibGUuc29ydENvbHVtbih0YWJsZSwgY29sTnVtKTtcclxuXHJcblx0XHRcdH0pO1xyXG5cclxuXHRcdH0sXHJcblxyXG5cdGd1ZXNzVHlwZTogZnVuY3Rpb24odGFibGUsIGNvbHVtbikge1xyXG5cclxuXHRcdC8vIGd1ZXNzIHRoZSB0eXBlIG9mIGEgY29sdW1uIGJhc2VkIG9uIGl0cyBmaXJzdCBub24tYmxhbmsgcm93XHJcblx0XHRzb3J0Zm4gPSBzb3J0dGFibGUuc29ydF9hbHBoYTtcclxuXHJcblx0XHRmb3IgKHZhciBpID0gMDsgaSA8IHRhYmxlLnRCb2RpZXNbMF0ucm93cy5sZW5ndGg7IGkrKykge1xyXG5cclxuXHRcdFx0dmFyIHRleHQgPSAkKHRhYmxlLnRCb2RpZXNbMF0ucm93c1tpXS5jZWxsc1tjb2x1bW5dKS50ZXh0KCkudHJpbSgpO1xyXG5cdFx0XHRpZiAodGV4dCAhPSAnJykge1xyXG5cdFx0XHRcdC8vIElmIGNvbHVtbiBpcyBudW1lcmljIG9yIGFwcGVhcnMgdG8gYmUgbW9uZXksIHNvcnQgbnVtZXJpY1xyXG5cdFx0XHRcdGlmICh0ZXh0Lm1hdGNoKC9eLT9bwqMkwqRdP1tcXGQsLl0rJT8kLykpIHtcclxuXHRcdFx0XHRcdHJldHVybiBzb3J0dGFibGUuc29ydF9udW1lcmljO1xyXG5cdFx0XHRcdH0gZWxzZSBpZihEYXRlLnBhcnNlKHRleHQpID4gMCkge1xyXG5cdFx0XHRcdFx0Ly8gQ2hlY2sgZm9yIHZhbGlkIGRhdGVcclxuXHRcdFx0XHRcdC8vIElmIGZvdW5kLCBhc3N1bWUgY29sdW1uIGlzIGZ1bGwgb2YgZGF0ZXMsIHNvcnQgYnkgZGF0ZSFcclxuXHRcdFx0XHRcdHJldHVybiBzb3J0dGFibGUuc29ydF9ieV9kYXRlO1xyXG5cdFx0XHRcdH0gZWxzZSB7XHJcblx0XHRcdFx0XHRyZXR1cm4gc29ydHRhYmxlLnNvcnRfYWxwaGE7XHJcblx0XHRcdFx0fVxyXG5cclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdFx0cmV0dXJuIHNvcnRmbjtcclxuXHR9LFxyXG5cclxuXHRyZXZlcnNlOiBmdW5jdGlvbih0Ym9keSkge1xyXG5cdFx0Ly8gcmV2ZXJzZSB0aGUgcm93cyBpbiBhIHRib2R5XHJcblx0XHR2YXIgbmV3cm93cyA9IFtdO1xyXG5cdFx0Zm9yICh2YXIgaT0wOyBpPHRib2R5LnJvd3MubGVuZ3RoOyBpKyspIHtcclxuXHRcdFx0bmV3cm93c1tuZXdyb3dzLmxlbmd0aF0gPSB0Ym9keS5yb3dzW2ldO1xyXG5cdFx0fVxyXG5cdFx0Zm9yICh2YXIgaT1uZXdyb3dzLmxlbmd0aC0xOyBpPj0wOyBpLS0pIHtcclxuXHRcdFx0dGJvZHkuYXBwZW5kQ2hpbGQobmV3cm93c1tpXSk7XHJcblx0XHR9XHJcblx0XHRuZXdyb3dzID0gdW5kZWZpbmVkO1xyXG5cdH0sXHJcblxyXG5cdC8qIHNvcnQgZnVuY3Rpb25zXHJcblx0ZWFjaCBzb3J0IGZ1bmN0aW9uIHRha2VzIHR3byBwYXJhbWV0ZXJzLCBhIGFuZCBiXHJcblx0eW91IGFyZSBjb21wYXJpbmcgYVswXSBhbmQgYlswXSAqL1xyXG5cdHNvcnRfbnVtZXJpYzogZnVuY3Rpb24oYSxiKSB7XHJcblx0XHR2YXIgYWEgPSBwYXJzZUZsb2F0KGFbMF0ucmVwbGFjZSgvW14wLTkuLV0vZywnJykpO1xyXG5cdFx0aWYgKGlzTmFOKGFhKSkgYWEgPSAwO1xyXG5cdFx0dmFyIGJiID0gcGFyc2VGbG9hdChiWzBdLnJlcGxhY2UoL1teMC05Li1dL2csJycpKTtcclxuXHRcdGlmIChpc05hTihiYikpIGJiID0gMDtcclxuXHRcdHJldHVybiBhYSAtIGJiO1xyXG5cdH0sXHJcblx0c29ydF9hbHBoYTogZnVuY3Rpb24oYSxiKSB7XHJcblx0XHR2YXIgYUNsZWFuID0gYVswXS50ZXh0KCkudHJpbSgpLnRvVXBwZXJDYXNlKCk7XHJcblx0XHR2YXIgYkNsZWFuID0gYlswXS50ZXh0KCkudHJpbSgpLnRvVXBwZXJDYXNlKCk7XHJcblx0XHRpZiAoYUNsZWFuID09IGJDbGVhbikgcmV0dXJuIDA7XHJcblx0XHRpZiAoYUNsZWFuIDwgYkNsZWFuKSByZXR1cm4gLTE7XHJcblx0XHRyZXR1cm4gMTtcclxuXHR9LFxyXG5cclxuXHRzb3J0X2J5X2RhdGU6IGZ1bmN0aW9uKGEsIGIpIHtcclxuXHRcdC8vIGh0dHA6Ly9zdGFja292ZXJmbG93LmNvbS9xdWVzdGlvbnMvMTAxMjM5NTMvc29ydC1qYXZhc2NyaXB0LW9iamVjdC1hcnJheS1ieS1kYXRlXHJcblx0XHQvLyBUdXJuIHlvdXIgc3RyaW5ncyBpbnRvIGRhdGVzLCBhbmQgdGhlbiBzdWJ0cmFjdCB0aGVtXHJcblx0XHQvLyB0byBnZXQgYSB2YWx1ZSB0aGF0IGlzIGVpdGhlciBuZWdhdGl2ZSwgcG9zaXRpdmUsIG9yIHplcm8uXHJcblx0XHRyZXR1cm4gbmV3IERhdGUoYlswXS50ZXh0KCkudHJpbSgpKSAtIG5ldyBEYXRlKGFbMF0udGV4dCgpLnRyaW0oKSk7XHJcblx0fSxcclxuXHJcblx0c29ydF9jaGVja2JveDogZnVuY3Rpb24oYSwgYikge1xyXG5cdFx0dmFyIGFDaGVja2VkID0gYVswXS5maW5kKCdpbnB1dFt0eXBlPWNoZWNrYm94XScpLnByb3AoJ2NoZWNrZWQnKTtcclxuXHRcdHZhciBiQ2hlY2tlZCA9IGJbMF0uZmluZCgnaW5wdXRbdHlwZT1jaGVja2JveF0nKS5wcm9wKCdjaGVja2VkJyk7XHJcblx0ICAgIGlmKGFDaGVja2VkICYmICFiQ2hlY2tlZCkgcmV0dXJuIDE7XHJcblx0XHRpZighYUNoZWNrZWQgJiYgYkNoZWNrZWQpIHJldHVybiAtMTtcclxuXHJcblx0XHRyZXR1cm4gMDtcclxuXHR9LFxyXG5cclxuXHRzaGFrZXJfc29ydDogZnVuY3Rpb24obGlzdCwgY29tcF9mdW5jKSB7XHJcblx0XHQvLyBBIHN0YWJsZSBzb3J0IGZ1bmN0aW9uIHRvIGFsbG93IG11bHRpLWxldmVsIHNvcnRpbmcgb2YgZGF0YVxyXG5cdFx0Ly8gc2VlOiBodHRwOi8vZW4ud2lraXBlZGlhLm9yZy93aWtpL0NvY2t0YWlsX3NvcnRcclxuXHRcdC8vIHRoYW5rcyB0byBKb3NlcGggTmFobWlhc1xyXG5cdFx0dmFyIGIgPSAwO1xyXG5cdFx0dmFyIHQgPSBsaXN0Lmxlbmd0aCAtIDE7XHJcblx0XHR2YXIgc3dhcCA9IHRydWU7XHJcblxyXG5cdFx0d2hpbGUoc3dhcCkge1xyXG5cdFx0XHRzd2FwID0gZmFsc2U7XHJcblx0XHRcdGZvcih2YXIgaSA9IGI7IGkgPCB0OyArK2kpIHtcclxuXHRcdFx0XHRpZiAoIGNvbXBfZnVuYyhsaXN0W2ldLCBsaXN0W2krMV0pID4gMCApIHtcclxuXHRcdFx0XHRcdHZhciBxID0gbGlzdFtpXTsgbGlzdFtpXSA9IGxpc3RbaSsxXTsgbGlzdFtpKzFdID0gcTtcclxuXHRcdFx0XHRcdHN3YXAgPSB0cnVlO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fSAvLyBmb3JcclxuXHRcdFx0dC0tO1xyXG5cclxuXHRcdFx0aWYgKCFzd2FwKSBicmVhaztcclxuXHJcblx0XHRcdGZvcih2YXIgaSA9IHQ7IGkgPiBiOyAtLWkpIHtcclxuXHRcdFx0XHRpZiAoIGNvbXBfZnVuYyhsaXN0W2ldLCBsaXN0W2ktMV0pIDwgMCApIHtcclxuXHRcdFx0XHRcdHZhciBxID0gbGlzdFtpXTsgbGlzdFtpXSA9IGxpc3RbaS0xXTsgbGlzdFtpLTFdID0gcTtcclxuXHRcdFx0XHRcdHN3YXAgPSB0cnVlO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fSAvLyBmb3JcclxuXHRcdFx0YisrO1xyXG5cclxuXHRcdH0gLy8gd2hpbGUoc3dhcClcclxuXHR9XHJcbn1cclxuXHJcblx0Ly8vIEhFTFBFUiBGVU5DVElPTlNcclxuXHJcblx0Ly8gRGVhbidzIGZvckVhY2g6IGh0dHA6Ly9kZWFuLmVkd2FyZHMubmFtZS9iYXNlL2ZvckVhY2guanNcclxuXHQvKlxyXG5cdFx0Zm9yRWFjaCwgdmVyc2lvbiAxLjBcclxuXHRcdENvcHlyaWdodCAyMDA2LCBEZWFuIEVkd2FyZHNcclxuXHRcdExpY2Vuc2U6IGh0dHA6Ly93d3cub3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvbWl0LWxpY2Vuc2UucGhwXHJcblx0Ki9cclxuXHJcblx0Ly8gYXJyYXktbGlrZSBlbnVtZXJhdGlvblxyXG5cdGlmICghQXJyYXkuZm9yRWFjaCkgeyAvLyBtb3ppbGxhIGFscmVhZHkgc3VwcG9ydHMgdGhpc1xyXG5cdFx0QXJyYXkuZm9yRWFjaCA9IGZ1bmN0aW9uKGFycmF5LCBibG9jaywgY29udGV4dCkge1xyXG5cdFx0XHRmb3IgKHZhciBpID0gMDsgaSA8IGFycmF5Lmxlbmd0aDsgaSsrKSB7XHJcblx0XHRcdFx0YmxvY2suY2FsbChjb250ZXh0LCBhcnJheVtpXSwgaSwgYXJyYXkpO1xyXG5cdFx0XHR9XHJcblx0XHR9O1xyXG5cdH1cclxuXHJcblx0Ly8gZ2VuZXJpYyBlbnVtZXJhdGlvblxyXG5cdEZ1bmN0aW9uLnByb3RvdHlwZS5mb3JFYWNoID0gZnVuY3Rpb24ob2JqZWN0LCBibG9jaywgY29udGV4dCkge1xyXG5cdFx0Zm9yICh2YXIga2V5IGluIG9iamVjdCkge1xyXG5cdFx0XHRpZiAodHlwZW9mIHRoaXMucHJvdG90eXBlW2tleV0gPT0gXCJ1bmRlZmluZWRcIikge1xyXG5cdFx0XHRcdGJsb2NrLmNhbGwoY29udGV4dCwgb2JqZWN0W2tleV0sIGtleSwgb2JqZWN0KTtcclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdC8vIGNoYXJhY3RlciBlbnVtZXJhdGlvblxyXG5cdFN0cmluZy5mb3JFYWNoID0gZnVuY3Rpb24oc3RyaW5nLCBibG9jaywgY29udGV4dCkge1xyXG5cdFx0QXJyYXkuZm9yRWFjaChzdHJpbmcuc3BsaXQoXCJcIiksIGZ1bmN0aW9uKGNociwgaW5kZXgpIHtcclxuXHRcdFx0YmxvY2suY2FsbChjb250ZXh0LCBjaHIsIGluZGV4LCBzdHJpbmcpO1xyXG5cdFx0fSk7XHJcblx0fTtcclxuXHJcblx0Ly8gZ2xvYmFsbHkgcmVzb2x2ZSBmb3JFYWNoIGVudW1lcmF0aW9uXHJcblx0dmFyIGZvckVhY2ggPSBmdW5jdGlvbihvYmplY3QsIGJsb2NrLCBjb250ZXh0KSB7XHJcblx0XHRpZiAob2JqZWN0KSB7XHJcblx0XHRcdHZhciByZXNvbHZlID0gT2JqZWN0OyAvLyBkZWZhdWx0XHJcblx0XHRcdGlmIChvYmplY3QgaW5zdGFuY2VvZiBGdW5jdGlvbikge1xyXG5cdFx0XHRcdC8vIGZ1bmN0aW9ucyBoYXZlIGEgXCJsZW5ndGhcIiBwcm9wZXJ0eVxyXG5cdFx0XHRcdHJlc29sdmUgPSBGdW5jdGlvbjtcclxuXHRcdFx0fSBlbHNlIGlmIChvYmplY3QuZm9yRWFjaCBpbnN0YW5jZW9mIEZ1bmN0aW9uKSB7XHJcblx0XHRcdFx0Ly8gdGhlIG9iamVjdCBpbXBsZW1lbnRzIGEgY3VzdG9tIGZvckVhY2ggbWV0aG9kIHNvIHVzZSB0aGF0XHJcblx0XHRcdFx0b2JqZWN0LmZvckVhY2goYmxvY2ssIGNvbnRleHQpO1xyXG5cdFx0XHRcdHJldHVybjtcclxuXHRcdFx0fSBlbHNlIGlmICh0eXBlb2Ygb2JqZWN0ID09IFwic3RyaW5nXCIpIHtcclxuXHRcdFx0XHQvLyB0aGUgb2JqZWN0IGlzIGEgc3RyaW5nXHJcblx0XHRcdFx0cmVzb2x2ZSA9IFN0cmluZztcclxuXHRcdFx0fSBlbHNlIGlmICh0eXBlb2Ygb2JqZWN0Lmxlbmd0aCA9PSBcIm51bWJlclwiKSB7XHJcblx0XHRcdFx0Ly8gdGhlIG9iamVjdCBpcyBhcnJheS1saWtlXHJcblx0XHRcdFx0cmVzb2x2ZSA9IEFycmF5O1xyXG5cdFx0XHR9XHJcblx0XHRcdHJlc29sdmUuZm9yRWFjaChvYmplY3QsIGJsb2NrLCBjb250ZXh0KTtcclxuXHRcdH1cclxuXHR9O1xyXG5cclxuXHRzb3J0dGFibGUuaW5pdCgpO1xyXG59O1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgc29ydGFibGVUYWJsZUNvbnRyb2xsZXI7XHJcbiIsIi8qIGdsb2JhbCB0b29sdGlwQ29udHJvbGxlciAqL1xyXG5cclxuaW1wb3J0IGNhbGN1bGF0ZVBvcHVwT2Zmc2V0cyBmcm9tIFwiLi4vY2FsY3VsYXRlUG9wdXBPZmZzZXRzLmpzXCI7XHJcblxyXG4vKipcclxuICogY3JlYXRlcyBhIHBvcHVwIGFuZCBpbmplY3RzIGl0IGluIHRvIHRoZSBkb2N1bWVudC5cclxuICogQHBhcmFtICB7U3RyaW5nfSB0aXRsZSA6IHRpdGxlIG9mIHRoZSBwb3B1cCwgb3B0aW9uYWxcclxuICogQHBhcmFtICB7U3RyaW5nfSBodG1sIDogY29udGVudCBvZiB0aGUgcG9wdXAgYXMgaHRtbFxyXG4gKiBAcGFyYW0gIHtOdW1iZXJ9IHRvcCAgOiB0b3AgcG9zaXRpb24gcmVsYXRpdmUgdG8gdGhlIGRvY3VtZW50XHJcbiAqIEBwYXJhbSAge051bWJlcn0gbGVmdCA6IGxlZnQgcG9zaXRpb24gcmVsYXRpdmUgdG8gdGhlIGRvY3VtZW50XHJcbiAqIEBwYXJhbSAge051bWJlcn0gb2Zmc2V0IDogYSB2ZXJ0aWNhbCBvciBob3Jpem9uYWwgZGlzdGFuY2UgZnJvbSB0aGUgdG9wL2xlZnQgdGhlIHRyaWFuZ2xlIHNob3VsZCBwb2ludFxyXG4gKiAgICAgICAgICAgICAgICAgICAgICAgICAgIGRlcGVuZGluZyBvbiBpZiB0aGUgdHJpYW5nbGUgaXMgdG9wL2JvdHRvbSBvciBsZWZ0L3JpZ2h0XHJcbiAqIEBwYXJhbSAge1N0cmluZ30gY29udGFpbmVyIDogYSBzZWxlY3RvciBvZiB0aGUgY29udGFpbmVyIGluIHdoaWNoIHRvIGluamVjdCB0aGUgcG9wdXBcclxuICogQHBhcmFtICB7U3RyaW5nfSB0cmlhbmdsZSA6IFwidG9wXCIsIFwicmlnaHRcIiwgXCJib3R0b21cIiwgb3IgXCJsZWZ0XCJcclxuICogQHBhcmFtICB7Qm9vbGVhbn0gZmxpcFRvQ29udGFpbjogd2lsbCBmbGlwIHRoZSBwb3B1cCBpZiBpdCBnb2VzIG91dHNpZGUgdGhlIHBhcmVudCBjb250YWluZXJcclxuICogQHBhcmFtICB7Qm9vbGVhbn0gY2xvc2VCdG4gOiBpZiB0aGUgWCBzaG91bGQgYmUgc2hvd24uICBJZiBub3QsIHRoZSBwb3B1cCB3aXRoIG5vdCBoYXZlIHBvaW50ZXIgZXZlbnRzXHJcbiAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgdGhpcyBpcyB1c2VmdWwgZm9yIHBvcHVwcyBvbiBob3ZlciB0aGF0IHNob3VsZG4ndCBmaXJlIG1vdXNlZW50ZXIvbGVhdmVcclxuICogQHBhcmFtICB7Qm9vbGVhbn0gaXNIaWRkZW46IFdoZXRoZXIgb3Igbm90IHRoZSBwb3B1cCBpcyB2aXNpYmxlLlxyXG4gKiBAcmV0dXJuIHtPYmplY3R9IHtcclxuICogICAgIHtGdW5jdGlvbn0gaGlkZVBvcHVwICAgOiB3aWxsIHNldCBpc0hpZGRlbiB0byB0cnVlXHJcbiAqICAgICB7RnVuY3Rpb259IHJlbW92ZVBvcHVwIDogd2lsbCByZW1vdmUgdGhlIHBvcHVwIGZyb20gdGhlIERPTSksXHJcbiAqICAgICB7RnVuY3Rpb259IHNldFN0YXRlIDogcGFzcyBhbiBvYmplY3Qgd2l0aCBhbnkga2V5cyBmcm9tIGFib3ZlIHRvIHVwZGF0ZSB0aGUgcG9wdXAuXHJcbiAqIH1cclxuICovXHJcbmV4cG9ydCBkZWZhdWx0IGZ1bmN0aW9uIGNyZWF0ZVBvcHVwKGluaXRpYWxTdGF0ZSkge1xyXG5cclxuICAgIC8vIGRlZmF1bHRzLCBhbmQgdGhpcyBvYmplY3Qgd2lsbCBob2xkIHRoZSBwcmV2aW91cyBzdGF0ZSBhZnRlciBzZXRTdGF0ZVxyXG4gICAgbGV0IHByZXZTdGF0ZSA9IHtcclxuICAgICAgICB0aXRsZSAgICAgOiBcIlwiLFxyXG4gICAgICAgIGh0bWwgICAgICA6IFwiXCIsXHJcbiAgICAgICAgdG9wICAgICAgIDogMCxcclxuICAgICAgICBsZWZ0ICAgICAgOiAwLFxyXG4gICAgICAgIHdpZHRoICAgICA6IFwiXCIsXHJcbiAgICAgICAgb2Zmc2V0ICAgIDogMCxcclxuICAgICAgICBjb250YWluZXIgOiBcImJvZHlcIixcclxuICAgICAgICB0cmlhbmdsZSAgOiBcImJvdHRvbVwiLFxyXG4gICAgICAgIGNsb3NlQnRuICA6IHRydWUsXHJcbiAgICAgICAgaXNIaWRkZW4gIDogZmFsc2UsXHJcbiAgICAgICAgZmxpcFRvQ29udGFpbjogZmFsc2VcclxuICAgIH07XHJcblxyXG4gICAgbGV0IHN0YXRlID0gJC5leHRlbmQoe30sIHByZXZTdGF0ZSwgaW5pdGlhbFN0YXRlKTtcclxuXHJcblxyXG4gICAgZnVuY3Rpb24gc2V0U3RhdGUobmV3U3RhdGUpe1xyXG5cclxuICAgICAgICAvLyBjb3B5IHRoZSBvbGQgc3RhdGUgaW50byBwcmV2U3RhdGVcclxuICAgICAgICBwcmV2U3RhdGUgPSAkLmV4dGVuZCh7fSwgc3RhdGUpO1xyXG5cclxuICAgICAgICAkLmV4dGVuZChzdGF0ZSwgbmV3U3RhdGUpO1xyXG5cclxuICAgICAgICAvLyBjb25zb2xlLmxvZyhzdGF0ZSk7XHJcblxyXG4gICAgICAgIHJlbmRlcigpO1xyXG4gICAgfVxyXG5cclxuICAgIC8vIGluaXRpYWxpemUgcG9wdXBcclxuICAgIC8vIGFsd2F5cyBzdGFydCBoaWRkZW4gc28gaXQgY2FuIGFuaW1hdGUgaW5cclxuICAgIGNvbnN0ICRwb3B1cCA9ICQoXCI8ZGl2IGNsYXNzPSdwb3B1cCc+XCIpXHJcbiAgICAgICAgLmNzcyh7XHJcbiAgICAgICAgICAgIFwib3BhY2l0eVwiOiAwLFxyXG4gICAgICAgICAgICBcIndpZHRoXCI6IHN0YXRlLndpZHRoLFxyXG4gICAgICAgICAgICBcInRyYW5zZm9ybVwiOiBcInNjYWxlKDAuODkpXCIsXHJcbiAgICAgICAgICAgIFwicG9pbnRlci1ldmVudHNcIjogKHN0YXRlLmNsb3NlQnRuKSA/IFwiYXV0b1wiIDogXCJub25lXCJcclxuICAgICAgICB9KTtcclxuICAgIGNvbnN0ICR0aXRsZURpdiAgICA9ICQoXCI8ZGl2PlwiKS5hZGRDbGFzcyhcInBvcHVwX190aXRsZVwiKTtcclxuICAgIGNvbnN0ICR0cmlhbmdsZURpdiA9ICQoXCI8ZGl2PlwiKS5hZGRDbGFzcyhcInBvcHVwX190cmlhbmdsZVwiKTtcclxuICAgIGNvbnN0ICRjb250ZW50ICAgICA9ICQoXCI8ZGl2PlwiKS5hZGRDbGFzcyhcInBvcHVwX19jb250ZW50XCIpO1xyXG5cclxuICAgIC8vIGF0dGFjaCB0aGUgY2xvc2UgYnV0dG9uIGlmIHdlJ3JlIHN1cHBvc2VkIHRvXHJcbiAgICBpZiAoc3RhdGUuY2xvc2VCdG4pe1xyXG4gICAgICAgIGNvbnN0ICR4RGl2ID0gJChcIjxkaXY+XCIpLmFkZENsYXNzKFwicG9wdXBfX3gtaWNvblwiKVxyXG4gICAgICAgICAgICAuaHRtbChcIjxzdmcgY2xhc3M9J3N2Zy14Jz4gPHVzZSB4bGluazpocmVmPSdidWlsZC9pbWcvc3ZnLXNwcml0ZS5zdmcjeCc+PC91c2U+IDwvc3ZnPlwiKVxyXG4gICAgICAgICAgICAub24oXCJjbGlja1wiLCByZW1vdmVQb3B1cCk7XHJcbiAgICAgICAgJHBvcHVwLmFwcGVuZCgkeERpdik7XHJcblxyXG5cclxuICAgICAgICB3aW5kb3cuYWRkRXZlbnRMaXN0ZW5lcihcImNsaWNrXCIsIGhhbmRsZUNsaWNrQXdheSwgdHJ1ZSk7XHJcbiAgICAgICAgd2luZG93LmFkZEV2ZW50TGlzdGVuZXIoXCJyZXNpemVcIiwgaGFuZGxlQ2xpY2tBd2F5LCB0cnVlKTtcclxuICAgIH1cclxuXHJcbiAgICAkcG9wdXAuYXBwZW5kKCR0aXRsZURpdik7XHJcbiAgICAkcG9wdXAuYXBwZW5kKCR0cmlhbmdsZURpdik7XHJcbiAgICAkcG9wdXAuYXBwZW5kKCRjb250ZW50KTtcclxuXHJcbiAgICAkKHN0YXRlLmNvbnRhaW5lcikuYXBwZW5kKCRwb3B1cCk7XHJcblxyXG5cclxuXHJcbiAgICAvLyBpZiB0aGUgdXNlciBjbGlja2VkIG91dHNpZGUgb2YgdGhlIHBvcHVwLCBjbG9zZSBpdFxyXG4gICAgZnVuY3Rpb24gaGFuZGxlQ2xpY2tBd2F5KGUpIHtcclxuICAgICAgICBjb25zdCBpblBvcHVwID0gJChlLnRhcmdldCkuY2xvc2VzdChcIi5wb3B1cFwiKS5sZW5ndGg7XHJcbiAgICAgICAgaWYgKCFpblBvcHVwKXsgcmVtb3ZlUG9wdXAoKTsgfVxyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIGhpZGVQb3B1cCgpe1xyXG5cclxuICAgICAgICB3aW5kb3cucmVtb3ZlRXZlbnRMaXN0ZW5lcihcImNsaWNrXCIsIGhhbmRsZUNsaWNrQXdheSwgdHJ1ZSk7XHJcbiAgICAgICAgd2luZG93LnJlbW92ZUV2ZW50TGlzdGVuZXIoXCJyZXNpemVcIiwgaGFuZGxlQ2xpY2tBd2F5LCB0cnVlKTtcclxuXHJcbiAgICAgICAgLy8gb25seSByZS1yZW5kZXIgaWYgd2UgbmVlZCB0b1xyXG4gICAgICAgIGlmIChzdGF0ZS5pc0hpZGRlbiAhPT0gdHJ1ZSl7XHJcbiAgICAgICAgICAgIC8vIHdpbGwga2ljayBvZiB0aGUgdHJhbnNpdGlvblxyXG4gICAgICAgICAgICBzZXRTdGF0ZSh7XHJcbiAgICAgICAgICAgICAgICBpc0hpZGRlbjogdHJ1ZVxyXG4gICAgICAgICAgICB9KTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gcmVtb3ZlUG9wdXAoKXtcclxuXHJcbiAgICAgICAgLy8gZmlyc3QgY2xvc2UgaXRcclxuICAgICAgICBoaWRlUG9wdXAoKTtcclxuXHJcbiAgICAgICAgLy8gd2hlbiB0aGUgdHJhbnNpdGlvbiBmaW5pc2hlcywgcmVtb3ZlIHRoZSBwb3B1cCBmcm9tIHRoZSBET01cclxuICAgICAgICAkcG9wdXAub24oXCJ0cmFuc2l0aW9uZW5kXCIsICgpID0+IHtcclxuICAgICAgICAgICAgJHBvcHVwLnJlbW92ZSgpO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIC8vIHJlbmRlciB0aGUgZmlyc3QgdGltZVxyXG4gICAgcmVuZGVyKCk7XHJcblxyXG4gICAgZnVuY3Rpb24gcmVuZGVyKCl7XHJcblxyXG4gICAgICAgIGNvbnN0IHsgdG9wLCBsZWZ0LCBvZmZzZXQsIHRyaWFuZ2xlLCBpc0hpZGRlbiwgaHRtbCwgdGl0bGUsIGZsaXBUb0NvbnRhaW4gfSA9IHN0YXRlO1xyXG5cclxuXHJcbiAgICAgICAgLy8gdXBkYXRlIHRoZSBjb250ZW50IGJlZm9yZSBjYWxjdWxhdGluZyB0aGUgb2Zmc2V0c1xyXG4gICAgICAgICRjb250ZW50Lmh0bWwoaHRtbCk7XHJcbiAgICAgICAgJHRpdGxlRGl2Lmh0bWwodGl0bGUpO1xyXG5cclxuICAgICAgICBjb25zdCBvZmZzZXRzID0gY2FsY3VsYXRlUG9wdXBPZmZzZXRzKHtcclxuICAgICAgICAgICAgcG9wdXA6ICRwb3B1cC5nZXQoMCksXHJcbiAgICAgICAgICAgIHRyaWFuZ2xlU2l6ZTogJHRyaWFuZ2xlRGl2LmhlaWdodCgpLFxyXG4gICAgICAgICAgICB0b3AsIGxlZnQsIG9mZnNldCwgdHJpYW5nbGUsIGZsaXBUb0NvbnRhaW5cclxuICAgICAgICB9KTtcclxuXHJcbiAgICAgICAgLy8gaWYgdGhlIHBvcHVwIHdhcyBoaWRkZW4sIHdlIHdhbnQgdG8gcGxhY2UgaXQgd2hlcmUgaXQgbmVlZHMgdG8gYmVcclxuICAgICAgICAvLyB0aGUgdXBkYXRlIHdpbGwgZmFkZSBpdCBpblxyXG4gICAgICAgIC8vIGVudGVyIC0gcHV0IGl0IGluIHBsYWNlIGJlZm9yZSB0cmFuc2l0aW9uaW5nIGluXHJcbiAgICAgICAgaWYgKHByZXZTdGF0ZS5pc0hpZGRlbiAmJiAhaXNIaWRkZW4pe1xyXG4gICAgICAgICAgICAkcG9wdXAuY3NzKHtcclxuICAgICAgICAgICAgICAgIFwidG9wXCIgOiBgJHtvZmZzZXRzLnBvcHVwVG9wfXB4YCxcclxuICAgICAgICAgICAgICAgIFwibGVmdFwiOiBgJHtvZmZzZXRzLnBvcHVwTGVmdH1weGBcclxuICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICAkcG9wdXBcclxuICAgICAgICAgICAgLy8gLnN0b3AoKS5hbmltYXRlKHtcclxuICAgICAgICAgICAgLmNzcyh7XHJcbiAgICAgICAgICAgICAgICBvcGFjaXR5OiAoaXNIaWRkZW4pID8gMCA6IDEsXHJcbiAgICAgICAgICAgICAgICB0cmFuc2Zvcm06IChpc0hpZGRlbikgPyBcInNjYWxlKDAuOSlcIiA6IFwic2NhbGUoMSlcIixcclxuICAgICAgICAgICAgICAgIHRvcDogb2Zmc2V0cy5wb3B1cFRvcCArICdweCcsXHJcbiAgICAgICAgICAgICAgICBsZWZ0OiBvZmZzZXRzLnBvcHVwTGVmdCArICdweCdcclxuICAgICAgICAgICAgfSwgMjUwKVxyXG4gICAgICAgICAgICAucmVtb3ZlQ2xhc3MoZnVuY3Rpb24oaW5kZXgsIGNzcyl7XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gKGNzcy5tYXRjaCgvXFxiaXMtdHJpYW5nbGUtXFxTKy9nKSB8fCBbXSkuam9pbihcIiBcIik7XHJcbiAgICAgICAgICAgIH0pXHJcbiAgICAgICAgICAgIC5hZGRDbGFzcyhgaXMtdHJpYW5nbGUtJHtvZmZzZXRzLnRyaWFuZ2xlU2lkZX1gKVxyXG4gICAgICAgICAgICAudG9nZ2xlQ2xhc3MoXCJwb3B1cC0taGlkZGVuXCIsIGlzSGlkZGVuKTtcclxuXHJcblxyXG4gICAgICAgIC8vIGFkanVzdCB0aGUgdHJpYW5nbGVcclxuICAgICAgICAkdHJpYW5nbGVEaXYuY3NzKHtcclxuICAgICAgICAgICAgXCJ0cmFuc2Zvcm1cIjogKG9mZnNldHMudHJpYW5nbGVTaWRlID09PSBcInRvcFwiIHx8IG9mZnNldHMudHJpYW5nbGVTaWRlID09PSBcImJvdHRvbVwiKVxyXG4gICAgICAgICAgICAgICAgPyBgdHJhbnNsYXRlWCgke29mZnNldHMudHJpYW5nbGVPZmZzZXR9cHgpYCAvLyB0b3AvYm90dG9tXHJcbiAgICAgICAgICAgICAgICA6IGB0cmFuc2xhdGVZKCR7b2Zmc2V0cy50cmlhbmdsZU9mZnNldH1weClgIC8vIGxlZnQvcmlnaHRcclxuICAgICAgICB9KTtcclxuXHJcblxyXG4gICAgICAgICRwb3B1cC50b2dnbGVDbGFzcyhcIm5vLXRpdGxlXCIsICF0aXRsZSk7XHJcblxyXG4gICAgfVxyXG5cclxuICAgIC8vIGV4dGVybmFsIGFwaVxyXG4gICAgcmV0dXJuIHtcclxuICAgICAgICBzZXRTdGF0ZSxcclxuICAgICAgICBoaWRlUG9wdXAsXHJcbiAgICAgICAgcmVtb3ZlUG9wdXBcclxuICAgIH07XHJcblxyXG59XHJcbiIsIi8qIGdsb2JhbCBhbmd1bGFyLCBhbmFseXRpY3NfZGF0YSAqL1xyXG5cclxuLy8gVEhJUkQtUEFSVFkgLyBWRU5ET1JcclxuaW1wb3J0IFplcHRvIGZyb20gJy4vemVwdG8ubWluJztcclxuaW1wb3J0IHN2ZzRldmVyeWJvZHkgZnJvbSAnLi9zdmc0ZXZlcnlib2R5JztcclxuaW1wb3J0IENvb2tpZXMgZnJvbSAnLi9qc2Nvb2tpZSc7XHJcblxyXG5cclxuLy8gQ09OVFJPTExFUlNcclxuaW1wb3J0IEZvcm1Db250cm9sbGVyIGZyb20gJy4vY29udHJvbGxlcnMvZm9ybS1jb250cm9sbGVyJztcclxuaW1wb3J0IFBvcE91dENvbnRyb2xsZXIgZnJvbSAnLi9jb250cm9sbGVycy9wb3Atb3V0LWNvbnRyb2xsZXInO1xyXG5pbXBvcnQgQm9va21hcmtDb250cm9sbGVyIGZyb20gJy4vY29udHJvbGxlcnMvYm9va21hcmstY29udHJvbGxlcic7XHJcbmltcG9ydCBSZXNldFBhc3N3b3JkQ29udHJvbGxlciBmcm9tICcuL2NvbnRyb2xsZXJzL3Jlc2V0LXBhc3N3b3JkLWNvbnRyb2xsZXInO1xyXG5pbXBvcnQgUmVnaXN0ZXJDb250cm9sbGVyIGZyb20gJy4vY29udHJvbGxlcnMvcmVnaXN0ZXItY29udHJvbGxlcic7XHJcbmltcG9ydCBTb3J0YWJsZVRhYmxlQ29udHJvbGxlciBmcm9tICcuL2NvbnRyb2xsZXJzL3NvcnRhYmxlLXRhYmxlLWNvbnRyb2xsZXInO1xyXG5pbXBvcnQgTGlnaHRib3hNb2RhbENvbnRyb2xsZXIgZnJvbSAnLi9jb250cm9sbGVycy9saWdodGJveC1tb2RhbC1jb250cm9sbGVyJztcclxuaW1wb3J0IHsgYW5hbHl0aWNzRXZlbnQgfSBmcm9tICcuL2NvbnRyb2xsZXJzL2FuYWx5dGljcy1jb250cm9sbGVyJztcclxuaW1wb3J0IHRvb2x0aXBDb250cm9sbGVyIGZyb20gJy4vY29udHJvbGxlcnMvdG9vbHRpcC1jb250cm9sbGVyJztcclxuXHJcbi8vIENPTVBPTkVOVFNcclxuaW1wb3J0ICcuL2NvbXBvbmVudHMvYXJ0aWNsZS1zaWRlYmFyLWNvbXBvbmVudCc7XHJcbmltcG9ydCAnLi9jb21wb25lbnRzL3NhdmUtc2VhcmNoLWNvbXBvbmVudCc7XHJcblxyXG5cclxuLy8gT1RIRVIgQ09ERVxyXG5pbXBvcnQgTmV3c2xldHRlclNpZ251cENvbnRyb2xsZXIgIGZyb20gJy4vbmV3c2xldHRlci1zaWdudXAnO1xyXG5pbXBvcnQgU2VhcmNoU2NyaXB0IGZyb20gJy4vc2VhcmNoLXBhZ2UuanMnO1xyXG5cclxuaW1wb3J0IHsgdG9nZ2xlSWNvbnMgfSBmcm9tICcuL3RvZ2dsZS1pY29ucyc7XHJcbi8vIEdsb2JhbCBzY29wZSB0byBwbGF5IG5pY2VseSB3aXRoIEFuZ3VsYXJcclxud2luZG93LnRvZ2dsZUljb25zID0gdG9nZ2xlSWNvbnM7XHJcblxyXG4vKiBQb2x5ZmlsbCBmb3Igc2NyaXB0cyBleHBlY3RpbmcgYGpRdWVyeWAuIEFsc28gc2VlOiBDU1Mgc2VsZWN0b3JzIHN1cHBvcnQgaW4gemVwdG8ubWluLmpzICovXHJcbndpbmRvdy5qUXVlcnkgPSAkO1xyXG5pbXBvcnQgc2VsZWN0aXZpdHkgZnJvbSAnLi9zZWxlY3Rpdml0eS1mdWxsJztcclxuXHJcblxyXG4vLyBNYWtlIHN1cmUgcHJvcGVyIGVsbSBnZXRzIHRoZSBjbGljayBldmVudFxyXG4vLyBXaGVuIGEgdXNlciBzdWJtaXRzIGEgRm9yZ290IFBhc3N3b3JkIHJlcXVlc3QsIHRoaXMgd2lsbCBkaXNwbGF5IHRoZSBwcm9wZXJcclxuLy8gc3VjY2VzcyBtZXNzYWdlIGFuZCBoaWRlIHRoZSBmb3JtIHRvIHByZXZlbnQgcmUtc2VuZGluZy5cclxudmFyIHNob3dGb3Jnb3RQYXNzU3VjY2VzcyA9IGZ1bmN0aW9uKCkge1xyXG5cdCQoJy5wb3Atb3V0X19zaWduLWluLWZvcmdvdC1wYXNzd29yZC1uZXN0ZWQnKS50b2dnbGVDbGFzcygnaXMtaGlkZGVuJyk7XHJcblx0JCgnLnBvcC1vdXRfX3NpZ24taW4tZm9yZ290LXBhc3N3b3JkJylcclxuXHRcdC5maW5kKCcuYWxlcnQtc3VjY2VzcycpXHJcblx0XHQudG9nZ2xlQ2xhc3MoJ2lzLWFjdGl2ZScpO1xyXG59O1xyXG5cclxudmFyIHJlbmRlcklmcmFtZUNvbXBvbmVudHMgPSBmdW5jdGlvbigpIHtcclxuXHQkKCcuaWZyYW1lLWNvbXBvbmVudCcpLmVhY2goZnVuY3Rpb24oaW5kZXgsIGVsbSkge1xyXG5cdFx0dmFyIGRlc2t0b3BFbWJlZCA9ICQoZWxtKS5maW5kKCcuaWZyYW1lLWNvbXBvbmVudF9fZGVza3RvcCcpO1xyXG5cdFx0dmFyIG1vYmlsZUVtYmVkID0gJChlbG0pLmZpbmQoJy5pZnJhbWUtY29tcG9uZW50X19tb2JpbGUnKTtcclxuXHJcblx0XHR2YXIgaXNFZGl0TW9kZSA9ICQodGhpcykuaGFzQ2xhc3MoJ2lzLXBhZ2UtZWRpdG9yJyk7XHJcblxyXG5cdFx0dmFyIHNob3dNb2JpbGUgPSAoJCh3aW5kb3cpLndpZHRoKCkgPD0gNDgwKSB8fCBpc0VkaXRNb2RlO1xyXG5cdFx0dmFyIHNob3dEZXNrdG9wID0gIXNob3dNb2JpbGUgfHwgaXNFZGl0TW9kZTtcclxuXHJcblx0XHRpZiAoc2hvd01vYmlsZSkge1xyXG5cdFx0XHRtb2JpbGVFbWJlZC5zaG93KCk7XHJcblx0XHRcdGlmIChtb2JpbGVFbWJlZC5odG1sKCkgPT0gJycpXHJcblx0XHRcdFx0bW9iaWxlRW1iZWQuaHRtbChkZWNvZGVIdG1sKG1vYmlsZUVtYmVkLmRhdGEoJ2VtYmVkLWxpbmsnKSkpO1xyXG5cdFx0fSBlbHNlIHtcclxuICAgICAgICAgICAgbW9iaWxlRW1iZWQuaGlkZSgpO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmIChzaG93RGVza3RvcCkge1xyXG5cdFx0XHRkZXNrdG9wRW1iZWQuc2hvdygpO1xyXG5cdFx0XHRpZiAoZGVza3RvcEVtYmVkLmh0bWwoKSA9PSAnJylcclxuXHRcdFx0XHRkZXNrdG9wRW1iZWQuaHRtbChkZWNvZGVIdG1sKGRlc2t0b3BFbWJlZC5kYXRhKCdlbWJlZC1saW5rJykpKTtcclxuXHRcdH0gZWxzZSB7XHJcbiAgICAgICAgICAgIGRlc2t0b3BFbWJlZC5oaWRlKCk7XHJcblx0XHR9XHJcblxyXG5cdFx0dmFyIGRlc2t0b3BNZWRpYUlkID0gJChlbG0pLmZpbmQoJy5pZnJhbWUtY29tcG9uZW50X19kZXNrdG9wJykuZGF0YShcIm1lZGlhaWRcIik7XHJcblxyXG5cdFx0dmFyIHVybCA9IHdpbmRvdy5sb2NhdGlvbi5ocmVmO1xyXG5cdFx0dXJsLnJlcGxhY2UoXCIjXCIsIFwiXCIpO1xyXG5cdFx0aWYgKHVybC5pbmRleE9mKFwiP1wiKSA8IDApIHtcclxuXHRcdFx0dXJsICs9IFwiP1wiO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0dXJsICs9IFwiJlwiO1xyXG5cdFx0fVxyXG5cclxuXHRcdHVybCArPSBcIm1vYmlsZW1lZGlhPXRydWUmc2VsZWN0ZWRpZD1cIiArIGRlc2t0b3BNZWRpYUlkO1xyXG5cdFx0JChlbG0pLmZpbmQoJy5pZnJhbWUtY29tcG9uZW50X19tb2JpbGUgYScpLmRhdGEoJ21lZGlhaWQnLCB1cmwpLmF0dHIoJ2hyZWYnLCBudWxsKTtcclxuXHR9KTtcclxufTtcclxuXHJcbnZhciBkZWNvZGVIdG1sID0gZnVuY3Rpb24oaHRtbCkge1xyXG5cdHZhciB0eHQgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KFwidGV4dGFyZWFcIik7XHJcblx0dHh0LmlubmVySFRNTCA9IGh0bWw7XHJcblx0cmV0dXJuIHR4dC52YWx1ZTtcclxufVxyXG5mdW5jdGlvbiBnZXRQYXJhbWV0ZXJCeU5hbWUobmFtZSwgdXJsKSB7XHJcbiAgICBpZiAoIXVybCkgdXJsID0gd2luZG93LmxvY2F0aW9uLmhyZWY7XHJcbiAgICBuYW1lID0gbmFtZS5yZXBsYWNlKC9bXFxbXFxdXS9nLCBcIlxcXFwkJlwiKTtcclxuICAgIHZhciByZWdleCA9IG5ldyBSZWdFeHAoXCJbPyZdXCIgKyBuYW1lICsgXCIoPShbXiYjXSopfCZ8I3wkKVwiKSxcclxuICAgICAgICByZXN1bHRzID0gcmVnZXguZXhlYyh1cmwpO1xyXG4gICAgaWYgKCFyZXN1bHRzKSByZXR1cm4gbnVsbDtcclxuICAgIGlmICghcmVzdWx0c1syXSkgcmV0dXJuICcnO1xyXG4gICAgcmV0dXJuIGRlY29kZVVSSUNvbXBvbmVudChyZXN1bHRzWzJdLnJlcGxhY2UoL1xcKy9nLCBcIiBcIikpO1xyXG59XHJcbiQoZG9jdW1lbnQpLnJlYWR5KGZ1bmN0aW9uKCkge1xyXG5cclxuICAgIHZhciBtZWRpYVRhYmxlID0gZ2V0UGFyYW1ldGVyQnlOYW1lKCdtb2JpbGVtZWRpYScpO1xyXG4gICAgaWYobWVkaWFUYWJsZT09XCJ0cnVlXCIpe1xyXG4gICAgICAgICQoXCJ0YWJsZVwiKS5lYWNoKGZ1bmN0aW9uKCl7XHJcbiAgICAgICAgICAgICQodGhpcykuYXR0cihcInN0eWxlXCIsXCJkaXNwbGF5OmJsb2NrXCIpO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuXHQvLyBBbnRpIEZvcmdlcnkgVG9rZW5cclxuXHR2YXIgcmVxdWVzdFZlcmlmaWNhdGlvblRva2VuID0gJCgnLm1haW5fX3dyYXBwZXInKS5kYXRhKCdyZXF1ZXN0LXZlcmlmaWNhdGlvbi10b2tlbicpO1xyXG5cclxuXHR2YXIgc29ydFRoZVRhYmxlcyA9IG5ldyBTb3J0YWJsZVRhYmxlQ29udHJvbGxlcigpO1xyXG5cclxuXHR3aW5kb3cubGlnaHRib3hDb250cm9sbGVyID0gbmV3IExpZ2h0Ym94TW9kYWxDb250cm9sbGVyKCk7XHJcblxyXG5cdC8qICogKlxyXG5cdFx0VHJhdmVyc2VzIHRoZSBET00gYW5kIHJlZ2lzdGVycyBldmVudCBsaXN0ZW5lcnMgZm9yIGFueSBwb3Atb3V0IHRyaWdnZXJzLlxyXG5cdFx0Qm91bmQgZXhwbGljaXRseSB0byBgd2luZG93YCBmb3IgZWFzaWVyIGFjY2VzcyBieSBBbmd1bGFyLlxyXG5cdCogKiAqL1xyXG5cdHdpbmRvdy5pbmRleFBvcE91dHMgPSBmdW5jdGlvbigpIHtcclxuXHJcblx0XHR3aW5kb3cuY29udHJvbFBvcE91dHMgPSBuZXcgUG9wT3V0Q29udHJvbGxlcignLmpzLXBvcC1vdXQtdHJpZ2dlcicpO1xyXG5cclxuXHRcdHdpbmRvdy5jb250cm9sUG9wT3V0cy5jdXN0b21pemUoe1xyXG5cdFx0XHRpZDogJ2hlYWRlci1yZWdpc3RlcicsXHJcblx0XHRcdHRhYlN0eWxlczoge1xyXG5cdFx0XHRcdGRlc2tIZWlnaHQ6IDg3LFxyXG5cdFx0XHRcdHRhYmxldEhlaWdodDogNzIsXHJcblx0XHRcdFx0cGhvbmVIZWlnaHQ6ICcnIC8vIERlZmF1bHRcclxuXHRcdFx0fVxyXG5cdFx0fSk7XHJcblxyXG5cdFx0d2luZG93LmNvbnRyb2xQb3BPdXRzLmN1c3RvbWl6ZSh7XHJcblx0XHRcdGlkOiAnaGVhZGVyLXNpZ25pbicsXHJcblx0XHRcdHRhYlN0eWxlczoge1xyXG5cdFx0XHRcdGRlc2tIZWlnaHQ6IDg3LFxyXG5cdFx0XHRcdHRhYmxldEhlaWdodDogNzIsXHJcblx0XHRcdFx0cGhvbmVIZWlnaHQ6ICcnIC8vIERlZmF1bHRcclxuXHRcdFx0fVxyXG5cdFx0fSk7XHJcblx0fTtcclxuXHJcblx0d2luZG93LmluZGV4UG9wT3V0cygpO1xyXG5cclxuXHJcblx0d2luZG93LmJvb2ttYXJrID0gbmV3IEJvb2ttYXJrQ29udHJvbGxlcigpO1xyXG5cclxuXHQvKiAqICpcclxuXHRcdFRyYXZlcnNlcyB0aGUgRE9NIGFuZCByZWdpc3RlcnMgZXZlbnQgbGlzdGVuZXJzIGZvciBhbnkgYm9va21hcmthYmxlXHJcblx0XHRhcnRpY2xlcy4gQm91bmQgZXhwbGljaXRseSB0byBgd2luZG93YCBmb3IgZWFzaWVyIGFjY2VzcyBieSBBbmd1bGFyLlxyXG5cdCogKiAqL1xyXG5cdHdpbmRvdy5pbmRleEJvb2ttYXJrcyA9IGZ1bmN0aW9uKCkgeyAvLyBUb2dnbGUgYm9va21hcmsgaWNvblxyXG5cdFx0JCgnLmpzLWJvb2ttYXJrLWFydGljbGUnKS5vbignY2xpY2snLCBmdW5jdGlvbiBib29rbWFya0FydGljbGUoZSkge1xyXG5cclxuXHRcdFx0ZS5wcmV2ZW50RGVmYXVsdCgpO1xyXG5cdFx0XHR3aW5kb3cuYm9va21hcmsudG9nZ2xlKHRoaXMpO1xyXG5cclxuXHRcdH0pO1xyXG5cdH07XHJcblxyXG5cdHdpbmRvdy5pbmRleEJvb2ttYXJrcygpO1xyXG5cclxuXHJcblx0LyogKiAqXHJcblx0XHRJZiBhIHVzZXIgdHJpZXMgYm9va21hcmtpbmcgYW4gYXJ0aWNsZSB3aGlsZSBsb2dnZWQgb3V0LCB0aGV5J2xsIGJlXHJcblx0XHRwcm9tcHRlZCB0byBzaWduIGluIGZpcnN0LiBUaGlzIGNoZWNrcyBmb3IgYW55IGFydGljbGVzIHRoYXQgaGF2ZSBiZWVuXHJcblx0XHRwYXNzZWQgYWxvbmcgZm9yIHBvc3Qtc2lnbi1pbiBib29rbWFya2luZy4gQm91bmQgZXhwbGljaXRseSB0byBgd2luZG93YFxyXG5cdFx0Zm9yIGVhc2llciBhY2Nlc3MgYnkgQW5ndWxhci5cclxuXHQqICogKi9cclxuXHR3aW5kb3cuYXV0b0Jvb2ttYXJrID0gZnVuY3Rpb24oKSB7XHJcblxyXG5cdFx0dmFyIGJvb2ttYXJrVGhlQXJ0aWNsZSA9IGZ1bmN0aW9uKGFydGljbGUpIHtcclxuXHRcdFx0JCgnLmpzLWJvb2ttYXJrLWFydGljbGUnKS5lYWNoKGZ1bmN0aW9uKGluZHgsIGl0ZW0pIHtcclxuXHRcdFx0XHRpZigkKGl0ZW0pLmRhdGEoJ2Jvb2ttYXJrLWlkJykgPT09IGFydGljbGVcclxuXHRcdFx0XHRcdCYmICEkKGl0ZW0pLmRhdGEoJ2lzLWJvb2ttYXJrZWQnKSkge1xyXG5cclxuXHRcdFx0XHRcdCQoaXRlbSkuY2xpY2soKTtcclxuXHJcblx0XHRcdFx0fSBlbHNlIHtcclxuXHRcdFx0XHRcdC8vIGFscmVhZHkgYm9va21hcmtlZCBvciBub3QgYSBtYXRjaFxyXG5cdFx0XHRcdH1cclxuXHRcdFx0fSk7XHJcblx0XHR9O1xyXG5cclxuXHRcdHZhciB1cmxWYXJzID0gd2luZG93LmxvY2F0aW9uLmhyZWYuc3BsaXQoXCI/XCIpO1xyXG5cdFx0dmFyIHZhcnNUb1BhcnNlID0gdXJsVmFyc1sxXSA/IHVybFZhcnNbMV0uc3BsaXQoXCImXCIpIDogbnVsbDtcclxuXHRcdGlmKHZhcnNUb1BhcnNlKSB7XHJcblx0XHRcdGZvciAodmFyIGk9MDsgaTx2YXJzVG9QYXJzZS5sZW5ndGg7IGkrKykge1xyXG5cdFx0XHRcdHZhciBwYWlyID0gdmFyc1RvUGFyc2VbaV0uc3BsaXQoXCI9XCIpO1xyXG5cdFx0XHRcdGlmKHBhaXJbMF0gPT09ICdpbW1iJykge1xyXG5cdFx0XHRcdFx0Ym9va21hcmtUaGVBcnRpY2xlKHBhaXJbMV0pO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdH07XHJcblxyXG5cdHdpbmRvdy5hdXRvQm9va21hcmsoKTtcclxuXHJcblxyXG5cdC8qICogKlxyXG5cdFx0VG9nZ2xlIGdsb2JhbCBoZWFkZXIgc2VhcmNoIGJveFxyXG5cdFx0KHRvZ2dsZXMgYXQgdGFibGV0L3NtYXJ0cGhvbmUgc2l6ZXMsIGFsd2F5cyB2aXNpYmxlIGF0IGRlc2t0b3Agc2l6ZSlcclxuXHQqICogKi9cclxuXHQkKCcuanMtaGVhZGVyLXNlYXJjaC10cmlnZ2VyJykub24oJ2NsaWNrJywgZnVuY3Rpb24gdG9nZ2xlTWVudUl0ZW1zKGUpIHtcclxuXHRcdGlmKCQod2luZG93KS53aWR0aCgpIDw9IDgwMCkge1xyXG5cdFx0XHQkKCcuaGVhZGVyLXNlYXJjaF9fd3JhcHBlcicpLnRvZ2dsZUNsYXNzKCdpcy1hY3RpdmUnKS5mb2N1cygpO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0JChlLnRhcmdldCkuY2xvc2VzdCgnZm9ybScpLnN1Ym1pdCgpO1xyXG5cdFx0fVxyXG5cdFx0ZS5wcmV2ZW50RGVmYXVsdCgpO1xyXG5cdFx0cmV0dXJuIGZhbHNlO1xyXG5cdH0pO1xyXG5cclxuXHR2YXIgbmV3c2xldHRlclNpZ251cCA9IG5ldyBOZXdzbGV0dGVyU2lnbnVwQ29udHJvbGxlcigpO1xyXG5cdG5ld3NsZXR0ZXJTaWdudXAuY2hlY2tGb3JVc2VyU2lnbmVkVXAoKTtcclxuXHRuZXdzbGV0dGVyU2lnbnVwLmFkZENvbnRyb2woJy5qcy1uZXdzbGV0dGVyLXNpZ251cC1zdWJtaXQnLCBudWxsLGZ1bmN0aW9uKHRyaWdnZXJFbGVtZW50KSB7XHJcblx0fSk7XHJcblxyXG5cclxuXHQvKiAqICpcclxuXHRcdEhhbmRsZSB1c2VyIHNpZ24taW4gYXR0ZW1wdHMuXHJcblx0KiAqICovXHJcblx0dmFyIHVzZXJTaWduSW4gPSBuZXcgRm9ybUNvbnRyb2xsZXIoe1xyXG5cdFx0b2JzZXJ2ZTogJy5qcy1zaWduLWluLXN1Ym1pdCcsXHJcblx0XHRzdWNjZXNzQ2FsbGJhY2s6IGZ1bmN0aW9uKGZvcm0sIGNvbnRleHQsIGV2ZW50KSB7XHJcblxyXG5cdFx0XHR2YXIgbG9naW5SZWdpc3Rlck1ldGhvZCA9IFwibG9naW5fcmVnaXN0ZXJfY29tcG9uZW50XCI7XHJcblx0XHRcdGlmKCQoZm9ybSkucGFyZW50cygnLnBvcC1vdXRfX3NpZ24taW4nKS5sZW5ndGggPiAwKVxyXG5cdFx0XHRcdGxvZ2luUmVnaXN0ZXJNZXRob2QgPSBcImdsb2JhbF9sb2dpblwiO1xyXG5cclxuXHRcdFx0dmFyIGxvZ2luQW5hbHl0aWNzID0gIHtcclxuXHRcdFx0XHRldmVudF9uYW1lOiAnbG9naW4nLFxyXG5cdFx0XHRcdGxvZ2luX3N0YXRlOiAnc3VjY2Vzc2Z1bCcsXHJcblx0XHRcdFx0dXNlck5hbWU6ICdcIicgKyAkKGZvcm0pLmZpbmQoJ2lucHV0W25hbWU9dXNlcm5hbWVdJykudmFsKCkgKyAnXCInLFxyXG5cdFx0XHRcdGxvZ2luX3JlZ2lzdGVyX21ldGhvZDogbG9naW5SZWdpc3Rlck1ldGhvZFxyXG5cdFx0XHR9O1xyXG5cclxuXHRcdFx0YW5hbHl0aWNzRXZlbnQoXHQkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgbG9naW5BbmFseXRpY3MpICk7XHJcblxyXG5cdFx0XHQvKiAqICpcclxuXHRcdFx0XHRJZiBgcGFzcy1hcnRpY2xlLWlkYCBpcyBzZXQsIHVzZXIgaXMgcHJvYmFibHkgdHJ5aW5nIHRvIHNpZ24gaW5cclxuXHRcdFx0XHRhZnRlciBhdHRlbXB0aW5nIHRvIGJvb2ttYXJrIGFuIGFydGljbGUuIEFkZCB0aGUgYXJ0aWNsZSBJRCB0b1xyXG5cdFx0XHRcdHRoZSBVUkwgc28gYGF1dG9Cb29rbWFyaygpYCBjYXRjaGVzIGl0LlxyXG5cdFx0XHQqICogKi9cclxuXHRcdFx0dmFyIHBhc3NBcnRpY2xlSWQgPSAkKGZvcm0pLmZpbmQoJy5zaWduLWluX19zdWJtaXQnKS5kYXRhKCdwYXNzLWFydGljbGUtaWQnKTtcclxuXHRcdFx0aWYocGFzc0FydGljbGVJZCkge1xyXG5cdFx0XHRcdHZhciBzZXAgPSAod2luZG93LmxvY2F0aW9uLmhyZWYuaW5kZXhPZignPycpID4gLTEpID8gJyYnIDogJz8nO1xyXG5cclxuXHRcdFx0XHR3aW5kb3cubG9jYXRpb24uaHJlZiA9IHdpbmRvdy5sb2NhdGlvbi5ocmVmICsgc2VwICsgJ2ltbWI9JyArIHBhc3NBcnRpY2xlSWQ7XHJcblxyXG5cdFx0XHRcdC8vIElmIEFuZ3VsYXIsIG5lZWQgbG9jYXRpb24ucmVsb2FkIHRvIGZvcmNlIHBhZ2UgcmVmcmVzaFxyXG5cdFx0XHRcdGlmKHR5cGVvZiBhbmd1bGFyICE9PSAndW5kZWZpbmVkJykge1xyXG5cdFx0XHRcdFx0YW5ndWxhci5lbGVtZW50KCQoJy5zZWFyY2gtcmVzdWx0cycpWzBdKS5jb250cm9sbGVyKCkuZm9yY2VSZWZyZXNoKCk7XHJcblx0XHRcdFx0fVxyXG5cclxuXHRcdFx0fSBlbHNlIHtcclxuXHRcdFx0XHR3aW5kb3cubG9jYXRpb24ucmVsb2FkKGZhbHNlKTtcclxuXHRcdFx0fVxyXG5cdFx0fSxcclxuXHRcdGZhaWx1cmVDYWxsYmFjazogZnVuY3Rpb24oZm9ybSwgY29udGV4dCwgZXZlbnQpIHtcclxuXHJcblx0XHRcdHZhciBsb2dpbkFuYWx5dGljcyA9IHtcclxuXHRcdFx0XHRldmVudF9uYW1lOiBcIkxvZ2luIEZhaWx1cmVcIixcclxuXHRcdFx0XHRsb2dpbl9zdGF0ZTogXCJ1bnN1Y2Nlc3NmdWxcIixcclxuXHRcdFx0XHR1c2VyTmFtZTogJ1wiJyArICQoZm9ybSkuZmluZCgnaW5wdXRbbmFtZT11c2VybmFtZV0nKS52YWwoKSArICdcIidcclxuXHRcdFx0fTtcclxuXHJcblx0XHRcdGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgbG9naW5BbmFseXRpY3MpICk7XHJcblxyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHR2YXIgcmVzZXRQYXNzd29yZCA9IG5ldyBGb3JtQ29udHJvbGxlcih7XHJcblx0XHRvYnNlcnZlOiAnLmZvcm0tcmVzZXQtcGFzc3dvcmQnLFxyXG5cdFx0c3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbigpIHtcclxuXHRcdFx0JCgnLmZvcm0tcmVzZXQtcGFzc3dvcmQnKS5maW5kKCcuYWxlcnQtc3VjY2VzcycpLnNob3coKTtcclxuXHRcdFx0dmFyIGlzUGFzc3dvcmQgPSAkKCcuZm9ybS1yZXNldC1wYXNzd29yZCcpLmRhdGEoXCJpcy1wYXNzd29yZFwiKTtcclxuXHRcdFx0aWYoaXNQYXNzd29yZClcclxuXHRcdFx0e1xyXG5cdFx0XHRcdGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgeyBldmVudF9uYW1lOiBcInBhc3N3b3JkIHJlc2V0IHN1Y2Nlc3NcIiB9KSApO1xyXG5cdFx0XHR9XHJcblx0XHR9LFxyXG5cdFx0ZmFpbHVyZUNhbGxiYWNrOiBmdW5jdGlvbigpIHtcclxuXHRcdFx0dmFyIGlzUGFzc3dvcmQgPSAkKCcuZm9ybS1yZXNldC1wYXNzd29yZCcpLmRhdGEoXCJpcy1wYXNzd29yZFwiKTtcclxuXHRcdFx0aWYoaXNQYXNzd29yZClcclxuXHRcdFx0e1xyXG5cdFx0XHRcdGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgeyBldmVudF9uYW1lOiBcInBhc3N3b3JkIHJlc2V0IGZhaWx1cmVcIiB9KSApO1xyXG5cdFx0XHR9XHJcblx0XHR9XHJcblxyXG5cdH0pO1xyXG5cclxuXHR2YXIgbmV3UmVzZXRQYXNzVG9rZW4gPSBuZXcgRm9ybUNvbnRyb2xsZXIoe1xyXG5cdFx0b2JzZXJ2ZTogJy5mb3JtLW5ldy1yZXNldC1wYXNzLXRva2VuJyxcclxuXHRcdHN1Y2Nlc3NDYWxsYmFjazogZnVuY3Rpb24oKSB7XHJcblx0XHRcdCQoJy5mb3JtLW5ldy1yZXNldC1wYXNzLXRva2VuJykuZmluZCgnLmFsZXJ0LXN1Y2Nlc3MnKS5zaG93KCk7XHJcblx0XHRcdGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgeyBldmVudF9uYW1lOiBcInBhc3N3b3JkIHJlc2V0IHN1Y2Nlc3NcIiB9KSApO1xyXG5cdFx0fSxcclxuXHRcdGZhaWx1cmVDYWxsYmFjazogZnVuY3Rpb24oKSB7XHJcblx0XHRcdGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgeyBldmVudF9uYW1lOiBcInBhc3N3b3JkIHJlc2V0IGZhaWx1cmVcIiB9KSApO1xyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHQkKCcuanMtY29ycG9yYXRlLW1hc3Rlci10b2dnbGUnKS5vbignY2hhbmdlJywgZnVuY3Rpb24oKSB7XHJcblx0XHRpZigkKHRoaXMpLnByb3AoJ2NoZWNrZWQnKSkge1xyXG5cdFx0XHQkKCcuanMtcmVnaXN0cmF0aW9uLWNvcnBvcmF0ZS13cmFwcGVyJykuc2hvdygpO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0JCgnLmpzLXJlZ2lzdHJhdGlvbi1jb3Jwb3JhdGUtd3JhcHBlcicpLmhpZGUoKTtcclxuXHRcdH1cclxuXHR9KTtcclxuXHJcblx0dmFyIHVzZXJSZWdpc3RyYXRpb25Db250cm9sbGVyID0gbmV3IEZvcm1Db250cm9sbGVyKHtcclxuXHRcdG9ic2VydmU6ICcuZm9ybS1yZWdpc3RyYXRpb24nLFxyXG5cdFx0c3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbihmb3JtLCBjb250ZXh0LCBldmVudCkge1xyXG5cclxuXHRcdFx0Ly8gU3Rhc2ggcmVnaXN0cmF0aW9uIHR5cGUgc28gbmV4dCBwYWdlIGNhbiBrbm93IGl0IHdpdGhvdXRcclxuXHRcdFx0Ly8gYW4gYWRkaXRpb25hbCBTYWxlc2ZvcmNlIGNhbGxcclxuXHRcdFx0Q29va2llcy5zZXQoJ3JlZ2lzdHJhdGlvblR5cGUnLCBjb250ZXh0LnJlc3BvbnNlLnJlZ2lzdHJhdGlvbl90eXBlLCB7fSApO1xyXG5cclxuXHRcdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCB7XHJcblx0XHRcdFx0cmVnaXN0cmF0aW9uX3R5cGU6IGNvbnRleHQucmVzcG9uc2UucmVnaXN0cmF0aW9uX3R5cGVcclxuXHRcdFx0fSkgKTtcclxuXHJcblx0XHR9LFxyXG5cdFx0ZmFpbHVyZUNhbGxiYWNrOiBmdW5jdGlvbihmb3JtLHJlc3BvbnNlKSB7XHJcblxyXG5cdFx0XHR2YXIgZXJyb3JNc2cgPSAkKFwiLnBhZ2UtcmVnaXN0cmF0aW9uX19lcnJvclwiKS50ZXh0KCk7XHJcblx0XHRcdGlmIChyZXNwb25zZS5yZWFzb25zICYmIHJlc3BvbnNlLnJlYXNvbnMubGVuZ3RoID4gMCkge1xyXG5cdFx0XHRcdGVycm9yTXNnID0gXCJbXCI7XHJcblx0XHRcdFx0Zm9yICh2YXIgcmVhc29uIGluIHJlc3BvbnNlLnJlYXNvbnMpIHtcclxuXHRcdFx0XHRcdGVycm9yTXNnICs9IHJlc3BvbnNlLnJlYXNvbnNbcmVhc29uXSArIFwiLFwiO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0XHRlcnJvck1zZyA9IGVycm9yTXNnLnN1YnN0cmluZygwLCBlcnJvck1zZy5sZW5ndGggLSAxKTtcclxuXHRcdFx0XHRlcnJvck1zZyArPSBcIl1cIjtcclxuXHRcdFx0fVxyXG5cdFx0XHRhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIHsgZXZlbnRfbmFtZTogXCJyZWdpc3RyYXRpb24gZmFpbHVyZVwiLCByZWdpc3RyYWlvbl9lcnJvcnMgOiBlcnJvck1zZyB9KSApO1xyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHR2YXIgdXNlclJlZ2lzdHJhdGlvbkZpbmFsQ29udHJvbGxlciA9IG5ldyBGb3JtQ29udHJvbGxlcih7XHJcblx0XHRvYnNlcnZlOiAnLmZvcm0tcmVnaXN0cmF0aW9uLW9wdGlucycsXHJcblx0XHRzdWNjZXNzQ2FsbGJhY2s6IGZ1bmN0aW9uKGZvcm0sIGNvbnRleHQsIGV2ZW50KSB7XHJcblxyXG5cdFx0XHR2YXIgcmVnaXN0cmF0aW9uVHlwZSA9IENvb2tpZXMuZ2V0KCdyZWdpc3RyYXRpb25UeXBlJyk7XHJcblxyXG5cdFx0XHRhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIHtcclxuXHRcdFx0XHRyZWdpc3RyYXRpb25fdHlwZTogcmVnaXN0cmF0aW9uVHlwZVxyXG5cdFx0XHR9KSApO1xyXG5cclxuXHRcdFx0Q29va2llcy5yZW1vdmUoJ3JlZ2lzdHJhdGlvblR5cGUnKTtcclxuXHRcdH0sXHJcblx0XHRmYWlsdXJlQ2FsbGJhY2s6IGZ1bmN0aW9uKGZvcm0sIHJlc3BvbnNlKSB7XHJcblx0XHRcdHZhciBlcnJvck1zZyA9ICQoXCIucGFnZS1yZWdpc3RyYXRpb25fX2Vycm9yXCIpLnRleHQoKTtcclxuXHRcdFx0aWYgKHJlc3BvbnNlLnJlYXNvbnMgJiYgcmVzcG9uc2UucmVhc29ucy5sZW5ndGggPiAwKSB7XHJcblx0XHRcdFx0ZXJyb3JNc2cgPSBcIltcIjtcclxuXHRcdFx0XHRmb3IgKHZhciByZWFzb24gaW4gcmVzcG9uc2UucmVhc29ucykge1xyXG5cdFx0XHRcdFx0ZXJyb3JNc2cgKz0gcmVzcG9uc2UucmVhc29uc1tyZWFzb25dICsgXCIsXCI7XHJcblx0XHRcdFx0fVxyXG5cdFx0XHRcdGVycm9yTXNnID0gZXJyb3JNc2cuc3Vic3RyaW5nKDAsIGVycm9yTXNnLmxlbmd0aCAtIDEpO1xyXG5cdFx0XHRcdGVycm9yTXNnICs9IFwiXVwiO1xyXG5cdFx0XHR9XHJcblx0XHRcdGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgeyBldmVudF9uYW1lOiBcInJlZ2lzdHJhdGlvbiBmYWlsdXJlXCIsIHJlZ2lzdHJhaW9uX2Vycm9ycyA6IGVycm9yTXNnfSkgKTtcclxuXHRcdH1cclxuXHR9KTtcclxuXHJcblxyXG5cdHZhciB1c2VyUHJlUmVnaXN0cmF0aW9uQ29udHJvbGxlciA9IG5ldyBGb3JtQ29udHJvbGxlcih7XHJcblx0XHRvYnNlcnZlOiAnLmZvcm0tcHJlLXJlZ2lzdHJhdGlvbicsXHJcblx0XHRzdWNjZXNzQ2FsbGJhY2s6IGZ1bmN0aW9uKGZvcm0pIHtcclxuXHRcdFx0dmFyIHVzZXJuYW1lSW5wdXQgPSAkKGZvcm0pLmZpbmQoJy5qcy1yZWdpc3Rlci11c2VybmFtZScpO1xyXG5cclxuXHRcdFx0dmFyIGZvcndhcmRpbmdVUkwgPSAkKGZvcm0pLmRhdGEoJ2ZvcndhcmRpbmctdXJsJyk7XHJcblx0XHRcdHZhciBzZXAgPSBmb3J3YXJkaW5nVVJMLmluZGV4T2YoJz8nKSA8IDAgPyAnPycgOiAnJic7XHJcblx0XHRcdHZhciBuZXh0U3RlcFVybCA9ICQoZm9ybSkuZGF0YSgnZm9yd2FyZGluZy11cmwnKSArIHNlcCArIHVzZXJuYW1lSW5wdXQuYXR0cignbmFtZScpICsgJz0nICsgZW5jb2RlVVJJQ29tcG9uZW50KHVzZXJuYW1lSW5wdXQudmFsKCkpO1xyXG5cclxuXHRcdFx0dmFyIGxvZ2luUmVnaXN0ZXJNZXRob2QgPSBcImdsb2JhbF9yZWdpc3RyYXRpb25cIjtcclxuXHRcdFx0aWYoJChmb3JtKS5oYXNDbGFzcyhcInVzZXItY2FsbHRvYWN0aW9uXCIpKVxyXG5cdFx0XHRcdGxvZ2luUmVnaXN0ZXJNZXRob2QgPSBcImxvZ2luX3JlZ2lzdGVyX2NvbXBvbmVudFwiO1xyXG5cclxuXHRcdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCB7IGV2ZW50X25hbWU6IFwicmVnaXN0cmF0aW9uXCIsIGxvZ2luX3JlZ2lzdGVyX21ldGhvZCA6IGxvZ2luUmVnaXN0ZXJNZXRob2QgfSkgKTtcclxuXHJcblx0XHRcdHdpbmRvdy5sb2NhdGlvbi5ocmVmID0gbmV4dFN0ZXBVcmw7XHJcblx0XHR9XHJcblx0fSk7XHJcblxyXG5cdCQoJy5jbGljay1sb2dvdXQnKS5vbignY2xpY2snLCBmdW5jdGlvbihlKSB7XHJcblx0XHRhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIHsgZXZlbnRfbmFtZTogXCJsb2dvdXRcIiB9KSApO1xyXG5cdH0pO1xyXG5cclxuXHJcblx0dmFyIGVtYWlsQXJ0aWNsZUNvbnRyb2xsZXIgPSBuZXcgRm9ybUNvbnRyb2xsZXIoe1xyXG5cdFx0b2JzZXJ2ZTogJy5mb3JtLWVtYWlsLWFydGljbGUnLFxyXG5cdFx0c3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbihmb3JtKSB7XHJcblx0XHRcdCQoJy5qcy1lbWFpbC1hcnRpY2xlLWZvcm0td3JhcHBlcicpLmhpZGUoKTtcclxuXHRcdFx0JCgnLmpzLWVtYWlsLWFydGljbGUtcmVjaXAtc3VjY2VzcycpLmh0bWwoJCgnLmpzLWVtYWlsLWFydGljbGUtcmVjaXAtYWRkcicpLnZhbCgpKTtcclxuXHRcdFx0JCgnLmpzLWVtYWlsLWFydGljbGUtc3VjY2VzcycpLnNob3coKTtcclxuXHJcblx0XHRcdC8vIFJlc2V0IHRoZSBFbWFpbCBBcnRpY2xlIHBvcC1vdXQgdG8gaXRzIGRlZmF1bHQgc3RhdGUgd2hlbiBjbG9zZWRcclxuXHRcdFx0JCgnLmpzLWRpc21pc3MtZW1haWwtYXJ0aWNsZScpLm9uZSgnY2xpY2snLCBmdW5jdGlvbigpIHtcclxuXHRcdFx0XHQkKCcuanMtZW1haWwtYXJ0aWNsZS1mb3JtLXdyYXBwZXInKS5zaG93KCk7XHJcblx0XHRcdFx0JCgnLmpzLWVtYWlsLWFydGljbGUtc3VjY2VzcycpLmhpZGUoKTtcclxuXHRcdFx0fSk7XHJcblx0XHR9XHJcblx0fSk7XHJcblxyXG5cdHZhciBlbWFpbEF1dGhvckNvbnRyb2xsZXIgPSBuZXcgRm9ybUNvbnRyb2xsZXIoe1xyXG5cdCAgICBvYnNlcnZlOiAnLmZvcm0tZW1haWwtYXV0aG9yJyxcclxuXHQgICAgc3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbihmb3JtKSB7XHJcblx0ICAgICAgICAkKCcuanMtZW1haWwtYXV0aG9yLWZvcm0td3JhcHBlcicpLmhpZGUoKTtcclxuXHQgICAgICAgICQoJy5qcy1lbWFpbC1hdXRob3ItcmVjaXAtc3VjY2VzcycpLmh0bWwoJCgnLmpzLWVtYWlsLWF1dGhvci1yZWNpcC1hZGRyJykudmFsKCkpO1xyXG5cdCAgICAgICAgJCgnLmpzLWVtYWlsLWF1dGhvci1zdWNjZXNzJykuc2hvdygpO1xyXG5cclxuXHQgICAgICAgIC8vIFJlc2V0IHRoZSBFbWFpbCBBdXRob3IgcG9wLW91dCB0byBpdHMgZGVmYXVsdCBzdGF0ZSB3aGVuIGNsb3NlZFxyXG5cdCAgICAgICAgJCgnLmpzLWRpc21pc3MtZW1haWwtYXV0aG9yJykub25lKCdjbGljaycsIGZ1bmN0aW9uKCkge1xyXG5cdCAgICAgICAgICAgICQoJy5qcy1lbWFpbC1hdXRob3ItZm9ybS13cmFwcGVyJykuc2hvdygpO1xyXG5cdCAgICAgICAgICAgICQoJy5qcy1lbWFpbC1hdXRob3Itc3VjY2VzcycpLmhpZGUoKTtcclxuXHQgICAgICAgIH0pO1xyXG5cdCAgICB9XHJcblx0fSk7XHJcblxyXG5cdHZhciBlbWFpbENvbXBhbnlDb250cm9sbGVyID0gbmV3IEZvcm1Db250cm9sbGVyKHtcclxuXHQgICAgb2JzZXJ2ZTogJy5mb3JtLWVtYWlsLWNvbXBhbnknLFxyXG5cdCAgICBzdWNjZXNzQ2FsbGJhY2s6IGZ1bmN0aW9uKGZvcm0pIHtcclxuXHQgICAgICAgICQoJy5qcy1lbWFpbC1jb21wYW55LWZvcm0td3JhcHBlcicpLmhpZGUoKTtcclxuXHQgICAgICAgICQoJy5qcy1lbWFpbC1jb21wYW55LXJlY2lwLXN1Y2Nlc3MnKS5odG1sKCQoJy5qcy1lbWFpbC1jb21wYW55LXJlY2lwLWFkZHInKS52YWwoKSk7XHJcblx0ICAgICAgICAkKCcuanMtZW1haWwtY29tcGFueS1zdWNjZXNzJykuc2hvdygpO1xyXG5cclxuXHQgICAgICAgIC8vIFJlc2V0IHRoZSBFbWFpbCBDb21wYW55IHBvcC1vdXQgdG8gaXRzIGRlZmF1bHQgc3RhdGUgd2hlbiBjbG9zZWRcclxuXHQgICAgICAgICQoJy5qcy1kaXNtaXNzLWVtYWlsLWNvbXBhbnknKS5vbmUoJ2NsaWNrJywgZnVuY3Rpb24oKSB7XHJcblx0ICAgICAgICAgICAgJCgnLmpzLWVtYWlsLWNvbXBhbnktZm9ybS13cmFwcGVyJykuc2hvdygpO1xyXG5cdCAgICAgICAgICAgICQoJy5qcy1lbWFpbC1jb21wYW55LXN1Y2Nlc3MnKS5oaWRlKCk7XHJcblx0ICAgICAgICB9KTtcclxuXHQgICAgfVxyXG5cdH0pO1xyXG5cclxuXHR2YXIgZW1haWxEZWFsQ29udHJvbGxlciA9IG5ldyBGb3JtQ29udHJvbGxlcih7XHJcblx0ICAgIG9ic2VydmU6ICcuZm9ybS1lbWFpbC1kZWFsJyxcclxuXHQgICAgc3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbihmb3JtKSB7XHJcblx0ICAgICAgICAkKCcuanMtZW1haWwtZGVhbC1mb3JtLXdyYXBwZXInKS5oaWRlKCk7XHJcblx0ICAgICAgICAkKCcuanMtZW1haWwtZGVhbC1yZWNpcC1zdWNjZXNzJykuaHRtbCgkKCcuanMtZW1haWwtZGVhbC1yZWNpcC1hZGRyJykudmFsKCkpO1xyXG5cdCAgICAgICAgJCgnLmpzLWVtYWlsLWRlYWwtc3VjY2VzcycpLnNob3coKTtcclxuXHJcblx0ICAgICAgICAvLyBSZXNldCB0aGUgRW1haWwgRGVhbCBwb3Atb3V0IHRvIGl0cyBkZWZhdWx0IHN0YXRlIHdoZW4gY2xvc2VkXHJcblx0ICAgICAgICAkKCcuanMtZGlzbWlzcy1lbWFpbC1kZWFsJykub25lKCdjbGljaycsIGZ1bmN0aW9uKCkge1xyXG5cdCAgICAgICAgICAgICQoJy5qcy1lbWFpbC1kZWFsLWZvcm0td3JhcHBlcicpLnNob3coKTtcclxuXHQgICAgICAgICAgICAkKCcuanMtZW1haWwtZGVhbC1zdWNjZXNzJykuaGlkZSgpO1xyXG5cdCAgICAgICAgfSk7XHJcblx0ICAgIH1cclxuXHR9KTtcclxuXHJcblx0dmFyIGVtYWlsU2VhcmNoQ29udHJvbGxlciA9IG5ldyBGb3JtQ29udHJvbGxlcih7XHJcblx0XHRvYnNlcnZlOiAnLmZvcm0tZW1haWwtc2VhcmNoJyxcclxuXHRcdHN1Y2Nlc3NDYWxsYmFjazogZnVuY3Rpb24oZm9ybSkge1xyXG5cclxuXHRcdFx0JCgnLmpzLWVtYWlsLXNlYXJjaC1mb3JtLXdyYXBwZXInKS5oaWRlKCk7XHJcblx0XHRcdCQoJy5qcy1lbWFpbC1zZWFyY2gtcmVjaXAtc3VjY2VzcycpLmh0bWwoJCgnLmpzLWVtYWlsLXNlYXJjaC1yZWNpcC1hZGRyJykudmFsKCkpO1xyXG5cdFx0XHQkKCcuanMtZW1haWwtc2VhcmNoLXN1Y2Nlc3MnKS5zaG93KCk7XHJcblx0XHRcdCQoJy5qcy1lbWFpbC1zZWFyY2gtZm9ybS13cmFwcGVyIGlucHV0LCAuanMtZW1haWwtc2VhcmNoLWZvcm0td3JhcHBlciB0ZXh0YXJlYScpLnZhbCgnJyk7XHJcblxyXG5cdFx0XHQvLyBSZXNldCB0aGUgRW1haWwgQXJ0aWNsZSBwb3Atb3V0IHRvIGl0cyBkZWZhdWx0IHN0YXRlIHdoZW4gY2xvc2VkXHJcblx0XHRcdCQoJy5qcy1kaXNtaXNzLWVtYWlsLXNlYXJjaCcpLm9uZSgnY2xpY2snLCBmdW5jdGlvbigpIHtcclxuXHRcdFx0XHQkKCcuanMtZW1haWwtc2VhcmNoLWZvcm0td3JhcHBlcicpLnNob3coKTtcclxuXHRcdFx0XHQkKCcuanMtZW1haWwtc2VhcmNoLXN1Y2Nlc3MnKS5oaWRlKCk7XHJcblx0XHRcdH0pO1xyXG5cclxuXHRcdFx0dmFyIGV2ZW50X2RhdGEgPSB7XHJcblx0XHRcdFx0ZXZlbnRfbmFtZTogXCJ0b29sYmFyX3VzZVwiLFxyXG5cdFx0XHRcdHRvb2xiYXJfdG9vbDogXCJlbWFpbFwiXHJcblx0XHRcdH07XHJcblxyXG5cdFx0XHRhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIGV2ZW50X2RhdGEpICk7XHJcblxyXG5cdFx0fSxcclxuXHRcdGJlZm9yZVJlcXVlc3Q6IGZ1bmN0aW9uKCkge1xyXG5cclxuXHRcdFx0dmFyIHJlc3VsdElEcyA9IG51bGw7XHJcblxyXG5cdFx0XHQkKCcuanMtc2VhcmNoLXJlc3VsdHMtaWQnKS5lYWNoKGZ1bmN0aW9uKGluZHgsIGl0ZW0pIHtcclxuXHRcdFx0XHRyZXN1bHRJRHMgPSByZXN1bHRJRHMgPyByZXN1bHRJRHMgKyAnLCcgKyAkKGl0ZW0pLmRhdGEoJ2Jvb2ttYXJrLWlkJykgOiAkKGl0ZW0pLmRhdGEoJ2Jvb2ttYXJrLWlkJyk7XHJcblx0XHRcdH0pO1xyXG5cclxuXHRcdFx0JCgnLmpzLWVtYWlsLXNlYXJjaC1yZXN1bHRzLWlkcycpLnZhbChyZXN1bHRJRHMpO1xyXG5cdFx0XHQkKCcuanMtZW1haWwtc2VhcmNoLXF1ZXJ5JykudmFsKCQoJy5zZWFyY2gtYmFyX19maWVsZCcpLnZhbCgpKTtcclxuXHRcdFx0JCgnLmpzLWVtYWlsLXNlYXJjaC1xdWVyeS11cmwnKS52YWwoZG9jdW1lbnQubG9jYXRpb24uaHJlZik7XHJcblxyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHJcblx0dmFyIGFjY291bnRFbWFpbFByZWZlcmVuY2VzQ29udHJvbGxlciA9IG5ldyBGb3JtQ29udHJvbGxlcih7XHJcblx0XHRvYnNlcnZlOiAnLmZvcm0tZW1haWwtcHJlZmVyZW5jZXMnLFxyXG5cdFx0c3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbihmb3JtLCBjb250ZXh0LCBldmVudCkge1xyXG5cclxuXHRcdFx0dmFyIGV2ZW50X2RhdGEgPSB7fTtcclxuXHRcdFx0dmFyIG9wdGluZ0luID0gbnVsbDtcclxuXHRcdFx0dmFyIG9wdGluZ091dCA9IG51bGw7XHJcblxyXG5cdFx0XHRpZigkKCcjRG9Ob3RTZW5kT2ZmZXJzT3B0SW4nKS5wcm9wKCdjaGVja2VkJykpIHtcclxuXHRcdFx0XHRldmVudF9kYXRhLmV2ZW50X25hbWUgPSAnZW1haWxfcHJlZmVyZW5jZXNfb3B0X291dCc7XHJcblx0XHRcdH0gZWxzZSB7XHJcblxyXG5cdFx0XHRcdGV2ZW50X2RhdGEuZXZlbnRfbmFtZSA9ICdlbWFpbF9wcmVmZXJlbmNlc191cGRhdGUnO1xyXG5cclxuXHRcdFx0XHQkKCcuanMtYWNjb3VudC1lbWFpbC1jaGVja2JveCcpLmVhY2goZnVuY3Rpb24oaW5kZXgsIGl0ZW0pIHtcclxuXHRcdFx0XHRcdGlmKHRoaXMuY2hlY2tlZCkge1xyXG5cdFx0XHRcdFx0XHRvcHRpbmdJbiA9IG9wdGluZ0luID8gb3B0aW5nSW4gKyAnfCcgKyB0aGlzLnZhbHVlIDogdGhpcy52YWx1ZTtcclxuXHRcdFx0XHRcdH0gZWxzZSB7XHJcblx0XHRcdFx0XHRcdG9wdGluZ091dCA9IG9wdGluZ091dCA/IG9wdGluZ091dCArICd8JyArIHRoaXMudmFsdWUgOiB0aGlzLnZhbHVlO1xyXG5cdFx0XHRcdFx0fVxyXG5cdFx0XHRcdH0pO1xyXG5cclxuXHRcdFx0XHRldmVudF9kYXRhLmVtYWlsX3ByZWZlcmVuY2VzX29wdGluID0gb3B0aW5nSW47XHJcblx0XHRcdFx0ZXZlbnRfZGF0YS5lbWFpbF9wcmVmZXJlbmNlc19vcHRvdXQgPSBvcHRpbmdPdXQ7XHJcblxyXG5cdFx0XHR9XHJcblxyXG5cdFx0XHRhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIGV2ZW50X2RhdGEpICk7XHJcblxyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHJcblx0dmFyIGFjY291bnRVcGRhdGVQYXNzQ29udHJvbGxlciA9IG5ldyBGb3JtQ29udHJvbGxlcih7XHJcblx0XHRvYnNlcnZlOiAnLmZvcm0tdXBkYXRlLWFjY291bnQtcGFzcycsXHJcblx0XHRzdWNjZXNzQ2FsbGJhY2s6IGZ1bmN0aW9uKGZvcm0sIGNvbnRleHQsIGV2dCkge1xyXG5cdFx0XHQkKGZvcm0pLmZpbmQoJ2lucHV0LCBzZWxlY3QsIHRleHRhcmVhJykuZWFjaChmdW5jdGlvbigpIHtcclxuXHRcdFx0XHQkKHRoaXMpLnZhbCgnJyk7XHJcblx0XHRcdH0pO1xyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHR2YXIgYWNjb3VudFVwZGF0ZUNvbnRhY3RDb250cm9sbGVyID0gbmV3IEZvcm1Db250cm9sbGVyKHtcclxuXHRcdG9ic2VydmU6ICcuZm9ybS11cGRhdGUtYWNjb3VudC1jb250YWN0JyxcclxuXHRcdHN1Y2Nlc3NDYWxsYmFjazogZnVuY3Rpb24oZm9ybSwgY29udGV4dCwgZXZ0KSB7XHJcblx0XHRcdCQod2luZG93KS5zY3JvbGxUb3AoKCQoZXZ0LnRhcmdldCkuY2xvc2VzdCgnZm9ybScpLmZpbmQoJy5qcy1mb3JtLWVycm9yLWdlbmVyYWwnKS5vZmZzZXQoKS50b3AgLSAzMikpO1xyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHR2YXIgc2F2ZWREb2N1bWVudHNDb250cm9sbGVyID0gbmV3IEZvcm1Db250cm9sbGVyKHtcclxuXHRcdG9ic2VydmU6ICcuZm9ybS1yZW1vdmUtc2F2ZWQtZG9jdW1lbnQnLFxyXG5cdFx0c3VjY2Vzc0NhbGxiYWNrOiBmdW5jdGlvbihmb3JtLCBjb250ZXh0LCBldnQpIHtcclxuXHRcdFx0JChldnQudGFyZ2V0KS5jbG9zZXN0KCd0cicpLnJlbW92ZSgpO1xyXG5cdFx0XHRpZigkKCcuanMtc29ydGFibGUtdGFibGUgdGJvZHknKVswXS5yb3dzLmxlbmd0aCA9PT0gMCkge1xyXG5cdFx0XHRcdCQoJy5qcy1zb3J0YWJsZS10YWJsZScpLnJlbW92ZSgpO1xyXG5cdFx0XHRcdCQoJy5qcy1uby1hcnRpY2xlcycpLnNob3coKTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0dmFyIGV2ZW50X2RhdGEgPSB7XHJcblx0XHRcdFx0ZXZlbnRfbmFtZTogJ2Jvb2ttYXJrX3JlbW92YWwnLFxyXG5cdFx0XHRcdGJvb2ttYXJrX3RpdGxlOiAkKGZvcm0pLmRhdGEoJ2FuYWx5dGljcy10aXRsZScpLFxyXG5cdFx0XHRcdGJvb2ttYXJrX3B1YmxpY2F0aW9uOiAkKGZvcm0pLmRhdGEoJ2FuYWx5dGljcy1wdWJsaWNhdGlvbicpXHJcblx0XHRcdH07XHJcblxyXG5cdFx0XHRhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIGV2ZW50X2RhdGEpICk7XHJcblx0XHR9XHJcblx0fSk7XHJcblxyXG5cdHN2ZzRldmVyeWJvZHkoKTtcclxuXHJcblx0LyogKiAqXHJcblx0XHRNQUlOIFNJVEUgTUVOVVxyXG5cdCogKiAqL1xyXG5cdChmdW5jdGlvbiBNZW51Q29udHJvbGxlcigpIHtcclxuXHJcblx0XHR2YXIgZ2V0SGVhZGVyRWRnZSA9IGZ1bmN0aW9uKCkge1xyXG5cdFx0XHRyZXR1cm4gJCgnLmhlYWRlcl9fd3JhcHBlcicpLm9mZnNldCgpLnRvcCArICQoJy5oZWFkZXJfX3dyYXBwZXInKS5oZWlnaHQoKTtcclxuXHRcdH07XHJcblxyXG5cdFx0dmFyIHNob3dNZW51ID0gZnVuY3Rpb24oKSB7XHJcblx0XHRcdCQoJy5tYWluLW1lbnUnKS5hZGRDbGFzcygnaXMtYWN0aXZlJyk7XHJcblx0XHRcdCQoJy5tZW51LXRvZ2dsZXInKS5hZGRDbGFzcygnaXMtYWN0aXZlJyk7XHJcblx0XHRcdCQoJy5oZWFkZXJfX3dyYXBwZXIgLm1lbnUtdG9nZ2xlcicpLmFkZENsYXNzKCdpcy1zdGlja3knKTtcclxuXHRcdFx0JCgnYm9keScpLmFkZENsYXNzKCdpcy1mcm96ZW4nKTtcclxuXHRcdH07XHJcblxyXG5cdFx0dmFyIGhpZGVNZW51ID0gZnVuY3Rpb24oKSB7XHJcblx0XHRcdCQoJy5tYWluLW1lbnUnKS5yZW1vdmVDbGFzcygnaXMtYWN0aXZlJyk7XHJcblx0XHRcdCQoJy5tZW51LXRvZ2dsZXInKS5yZW1vdmVDbGFzcygnaXMtYWN0aXZlJyk7XHJcblx0XHRcdCQoJ2JvZHknKS5yZW1vdmVDbGFzcygnaXMtZnJvemVuJyk7XHJcblx0XHRcdGlmKCQod2luZG93KS5zY3JvbGxUb3AoKSA8PSBnZXRIZWFkZXJFZGdlKCkpIHtcclxuXHRcdFx0XHQkKCcuaGVhZGVyX193cmFwcGVyIC5tZW51LXRvZ2dsZXInKS5yZW1vdmVDbGFzcygnaXMtc3RpY2t5Jyk7XHJcblx0XHRcdH1cclxuXHRcdH07XHJcblxyXG5cdFx0LyogVG9nZ2xlIG1lbnUgdmlzaWJpbGl0eSAqL1xyXG5cdFx0JCgnLmpzLW1lbnUtdG9nZ2xlLWJ1dHRvbicpLm9uKCdjbGljaycsIGZ1bmN0aW9uIHRvZ2dsZU1lbnUoZSkge1xyXG5cdFx0XHQkKCcubWFpbi1tZW51JykuaGFzQ2xhc3MoJ2lzLWFjdGl2ZScpID8gaGlkZU1lbnUoKSA6IHNob3dNZW51KCk7XHJcblx0XHRcdGUucHJldmVudERlZmF1bHQoKTtcclxuXHRcdFx0ZS5zdG9wUHJvcGFnYXRpb24oKTtcclxuXHRcdH0pO1xyXG5cclxuXHRcdC8qICBJZiB0aGUgbWVudSBpcyBjbG9zZWQsIGxldCBhbnkgY2xpY2tzIG9uIHRoZSBtZW51IGVsZW1lbnQgb3BlblxyXG5cdFx0XHR0aGUgbWVudS4gVGhpcyBpbmNsdWRlcyB0aGUgYm9yZGVy4oCUdmlzaWJsZSB3aGVuIHRoZSBtZW51IGlzIGNsb3NlZOKAlFxyXG5cdFx0XHRzbyBpdCdzIGVhc2llciB0byBvcGVuLiAqL1xyXG5cdFx0JCgnLmpzLWZ1bGwtbWVudS10b2dnbGUnKS5vbignY2xpY2snLCBmdW5jdGlvbiB0b2dnbGVNZW51KCkge1xyXG5cdFx0XHQkKCcubWFpbi1tZW51JykuaGFzQ2xhc3MoJ2lzLWFjdGl2ZScpID8gbnVsbCA6IHNob3dNZW51KCk7XHJcblx0XHR9KTtcclxuXHJcblx0XHQvKiBBdHRhY2ggLyBkZXRhY2ggc3RpY2t5IG1lbnUgKi9cclxuXHRcdCQod2luZG93KS5vbignc2Nyb2xsJywgZnVuY3Rpb24gd2luZG93U2Nyb2xsZWQoKSB7XHJcblx0XHRcdC8vIE9ubHkgc3RpY2sgaWYgdGhlIGhlYWRlciAoaW5jbHVkaW5nIHRvZ2dsZXIpIGlzbid0IHZpc2libGVcclxuXHRcdFx0aWYgKCQod2luZG93KS5zY3JvbGxUb3AoKSA+IGdldEhlYWRlckVkZ2UoKSB8fCAkKCcubWFpbi1tZW51JykuaGFzQ2xhc3MoJ2lzLWFjdGl2ZScpKSB7XHJcblx0XHRcdFx0JCgnLmhlYWRlcl9fd3JhcHBlciAubWVudS10b2dnbGVyJykuYWRkQ2xhc3MoJ2lzLXN0aWNreScpO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdCQoJy5oZWFkZXJfX3dyYXBwZXIgLm1lbnUtdG9nZ2xlcicpLnJlbW92ZUNsYXNzKCdpcy1zdGlja3knKTtcclxuXHRcdFx0fVxyXG5cdFx0fSk7XHJcblxyXG5cdFx0LyogVG9nZ2xlIG1lbnUgY2F0ZWdvcmllcyAqL1xyXG5cdFx0JCgnLmpzLXRvZ2dsZS1tZW51LXNlY3Rpb24nKS5vbignY2xpY2snLCBmdW5jdGlvbiB0b2dnbGVNZW51SXRlbXMoZSkge1xyXG5cdFx0XHRlLnRhcmdldCAhPT0gdGhpcyA/IHRoaXMuY2xpY2soKSA6ICQoZS50YXJnZXQpLnRvZ2dsZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuXHRcdH0pO1xyXG5cclxuXHR9KSgpO1xyXG5cclxuXHJcblx0LyogKiAqXHJcblx0XHRXaGVuIGEgYmFubmVyIGlzIGRpc21pc3NlZCwgdGhlIGJhbm5lciBJRCBpcyBzdG9yZWQgaW4gdGhlXHJcblx0XHRgZGlzbWlzc2VkQmFubmVyc2AgY29va2llIGFzIGEgSlNPTiBvYmplY3QuIEJhbm5lcnMgYXJlIGludmlzaWJsZSBieSBkZWZhdWx0LFxyXG5cdFx0c28gb24gcGFnZSBsb2FkLCB0aGlzIGNoZWNrcyBpZiBhIGJhbm5lciBvbiB0aGUgcGFnZSBpcyBkaXNtaXNzZWQgb3Igbm90LFxyXG5cdFx0dGhlbiBtYWtlcyB0aGUgYmFubmVyIHZpc2libGUgaWYgbm90IGRpc21pc3NlZC5cclxuXHQqICogKi9cclxuXHR2YXIgZGlzbWlzc2VkQmFubmVycyA9IENvb2tpZXMuZ2V0SlNPTignZGlzbWlzc2VkQmFubmVycycpIHx8IHt9O1xyXG5cdCQoJy5iYW5uZXInKS5lYWNoKGZ1bmN0aW9uKCkge1xyXG5cdFx0aWYoJCh0aGlzKS5kYXRhKCdiYW5uZXItaWQnKSBpbiBkaXNtaXNzZWRCYW5uZXJzID09PSBmYWxzZSkge1xyXG5cdFx0XHQkKHRoaXMpLmFkZENsYXNzKCdpcy12aXNpYmxlJyk7XHJcblx0XHR9XHJcblx0fSk7XHJcblxyXG5cdC8qICogKlxyXG5cdFx0R2VuZXJpYyBiYW5uZXIgZGlzbWlzc1xyXG5cdCogKiAqL1xyXG5cdCQoJy5qcy1kaXNtaXNzLWJhbm5lcicpLm9uKCdjbGljaycsIGZ1bmN0aW9uIGRpc21pc3NCYW5uZXIoZSkge1xyXG5cdFx0dmFyIHRoaXNCYW5uZXIgPSAkKGUudGFyZ2V0KS5wYXJlbnRzKCcuYmFubmVyJyk7XHJcblx0XHR0aGlzQmFubmVyLnJlbW92ZUNsYXNzKCdpcy12aXNpYmxlJyk7XHJcblxyXG5cdFx0dmFyIGRpc21pc3NlZEJhbm5lcnMgPSBDb29raWVzLmdldEpTT04oJ2Rpc21pc3NlZEJhbm5lcnMnKSB8fCB7fTtcclxuXHRcdGRpc21pc3NlZEJhbm5lcnNbdGhpc0Jhbm5lci5kYXRhKCdiYW5uZXItaWQnKV0gPSB0cnVlO1xyXG5cclxuICAgICAgICAvLyBpZiBiYW5uZXIgaGFzIGEgJ2Rpc21pc3MtYWxsLXN1YmRvbWFpbnMnIGF0dHJpYnV0ZSA9IHRydWUsIHNldCB0aGUgZG9tYWluIG9mIHRoZSBjb29raWVcclxuICAgICAgICAvLyB0byB0aGUgdG9wLWxldmVsIGRvbWFpbi5cclxuICAgICAgICB2YXIgZG9tYWluID0gZG9jdW1lbnQubG9jYXRpb24uaG9zdG5hbWU7XHJcbiAgICAgICAgaWYgKHRoaXNCYW5uZXIuZGF0YSgnZGlzbWlzcy1hbGwtc3ViZG9tYWlucycpKSB7XHJcbiAgICAgICAgICAgIHZhciBwYXJ0cyA9IGRvbWFpbi5zcGxpdCgnLicpO1xyXG4gICAgICAgICAgICBwYXJ0cy5zaGlmdCgpO1xyXG4gICAgICAgICAgICBkb21haW4gPSBwYXJ0cy5qb2luKCcuJyk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIENvb2tpZXMuc2V0KCdkaXNtaXNzZWRCYW5uZXJzJywgZGlzbWlzc2VkQmFubmVycywge2V4cGlyZXM6IDM2NTAsIGRvbWFpbjogZG9tYWluIH0gKTtcclxuXHR9KTtcclxuXHJcblx0Ly8gRm9yIGVhY2ggYXJ0aWNsZSB0YWJsZSwgY2xvbmUgYW5kIGFwcGVuZCBcInZpZXcgZnVsbCB0YWJsZVwiIG1hcmt1cFxyXG5cdCQoJy5hcnRpY2xlLWJvZHktY29udGVudCB0YWJsZScpLm5vdCgnLmFydGljbGUtdGFibGUtLW1vYmlsZS1saW5rJykuZm9yRWFjaChmdW5jdGlvbihlKSB7XHJcblx0XHR2YXIgbWVkaWFJZCA9ICQoZSkuZGF0YShcIm1lZGlhaWRcIik7XHJcblx0XHR2YXIgdGFibGVMaW5rID0gJCgnLmpzLW1vYmlsZS10YWJsZS10ZW1wbGF0ZSAuYXJ0aWNsZS10YWJsZScpLmNsb25lKCk7XHJcblxyXG5cdFx0dmFyIHVybCA9IHdpbmRvdy5sb2NhdGlvbi5ocmVmO1xyXG5cdFx0dXJsLnJlcGxhY2UoXCIjXCIsIFwiXCIpO1xyXG5cdFx0aWYgKHVybC5pbmRleE9mKFwiP1wiKSA8IDApXHJcblx0XHRcdHVybCArPSBcIj9cIjtcclxuXHRcdGVsc2VcclxuXHRcdFx0dXJsICs9IFwiJlwiO1xyXG5cclxuXHRcdHVybCs9IFwibW9iaWxlbWVkaWE9dHJ1ZSZzZWxlY3RlZGlkPVwiICsgbWVkaWFJZDtcclxuXHJcblx0XHQvLyAkKHRhYmxlTGluaykuZmluZCgnYScpLmF0dHIoXCJocmVmXCIsIHVybCk7XHJcblx0XHQkKHRhYmxlTGluaykuZmluZCgnYScpLmRhdGEoXCJ0YWJsZS11cmxcIiwgdXJsKS5hdHRyKCdocmVmJywgbnVsbCk7XHJcblx0XHQkKGUpLmFmdGVyKHRhYmxlTGluayk7XHJcblx0fSk7XHJcblxyXG5cclxuICAgIC8vIEZpbmQgZHVwbGljYXRlIGVtYmVkcyBvbiBhcnRpY2xlIHBhZ2VcclxuICAgIC8vIElJVFMyLTMxMlxyXG4gICAgJCgnW2NsYXNzXj1ld2YtZGVza3RvcC1pZnJhbWVdIH4gW2NsYXNzXj1ld2YtbW9iaWxlLWlmcmFtZV0nKS5lYWNoKGZ1bmN0aW9uKGluZGV4LCBpdGVtKSB7XHJcbiAgICAgICAgJChpdGVtKS5yZW1vdmUoKTtcclxuICAgIH0pO1xyXG5cclxuXHQvLyBXaGVuIERPTSBsb2FkcywgcmVuZGVyIHRoZSBhcHByb3ByaWF0ZSBpRnJhbWUgY29tcG9uZW50c1xyXG5cdC8vIEFsc28gYWRkIGEgbGlzdGVuZXIgZm9yIHdpbmRlciByZXNpemUsIHJlbmRlciBhcHByb3ByaWF0ZSBjb250YWluZXJzXHJcblx0cmVuZGVySWZyYW1lQ29tcG9uZW50cygpO1xyXG5cdCQod2luZG93KS5vbigncmVzaXplJywgKGV2ZW50KSA9PiB7XHJcblx0XHRyZW5kZXJJZnJhbWVDb21wb25lbnRzKCk7XHJcblx0fSk7XHJcblxyXG5cdC8vIFRvcGljIGxpbmtzXHJcblx0dmFyIHRvcGljQW5jaG9ycyA9ICQoJy5qcy10b3BpYy1hbmNob3InKTtcclxuXHJcblx0JCgnLnN1Yi10b3BpYy1saW5rcycpLmZvckVhY2goZnVuY3Rpb24oZSkge1xyXG5cdFx0dmFyIGxpbmtMaXN0ID0gJChlKS5maW5kKCcuYmFyLXNlcGFyYXRlZC1saW5rLWxpc3QnKTtcclxuXHJcblx0XHR0b3BpY0FuY2hvcnMuZm9yRWFjaChmdW5jdGlvbih0Yykge1xyXG5cdFx0XHR2YXIgaWQgPSB0Yy5pZDtcclxuXHRcdFx0dmFyIHRleHQgPSAkKHRjKS5kYXRhKCd0b3BpYy1saW5rLXRleHQnKTtcclxuXHRcdFx0dmFyIHV0YWdJbmZvID0gJ3tcImV2ZW50X25hbWVcIj1cInRvcGljLWp1bXAtdG8tbGluay1jbGlja1wiLFwidG9waWMtbmFtZVwiPVwiJyt0ZXh0KydcIn0nO1xyXG5cdFx0XHRsaW5rTGlzdC5hcHBlbmQoJzxhIGhyZWY9XCIjJyArIGlkICsgJ1wiIGNsYXNzPVwiY2xpY2stdXRhZ1wiIGRhdGEtaW5mbz0nK3RleHQrJz4nICsgdGV4dCArICc8L2E+Jyk7XHJcblx0XHR9KTtcclxuXHR9KTtcclxuXHJcblx0Ly8gRGlzcGxheSB0aGUgRm9yZ290IFBhc3N3b3JkIGJsb2NrIHdoZW4gXCJmb3Jnb3QgeW91ciBwYXNzd29yZFwiIGlzIGNsaWNrZWRcclxuXHQkKCcuanMtc2hvdy1mb3Jnb3QtcGFzc3dvcmQnKS5vbignY2xpY2snLCBmdW5jdGlvbiB0b2dnbGVGb3Jnb3RQYXNzKCkge1xyXG5cdFx0JCgnLmpzLXJlc2V0LXBhc3N3b3JkLWNvbnRhaW5lcicpLnRvZ2dsZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuICAgICAgICBpZigkKFwiLmluZm9ybWEtcmliYm9uXCIpLmhhc0NsYXNzKFwic2hvd1wiKSl7XHJcbiAgICAgICAgICAgICQoXCJib2R5XCIpLnNjcm9sbFRvcCgkKFwiLnBvcC1vdXRfX2ZvcmdvdC1wYXNzd29yZFwiKS5wb3NpdGlvbigpLnRvcCArIDU3MCk7XHJcbiAgICAgICAgfWVsc2V7XHJcbiAgICAgICAgICAgICQoXCJib2R5XCIpLnNjcm9sbFRvcCgkKFwiLnBvcC1vdXRfX2ZvcmdvdC1wYXNzd29yZFwiKS5wb3NpdGlvbigpLnRvcCk7XHJcbiAgICAgICAgfVxyXG5cclxuXHR9KTtcclxuXHJcblx0Ly8gR2xvYmFsIGRpc21pc3MgYnV0dG9uIGZvciBwb3Atb3V0c1xyXG5cdCQoJy5kaXNtaXNzLWJ1dHRvbicpLm9uKCdjbGljaycsIGZ1bmN0aW9uKGUpIHtcclxuXHRcdGlmIChlLnRhcmdldCAhPT0gdGhpcykge1xyXG5cdFx0XHR0aGlzLmNsaWNrKCk7XHJcblx0XHRcdHJldHVybjtcclxuXHRcdH1cclxuXHJcblx0XHQkKCQoZS50YXJnZXQpLmRhdGEoJ3RhcmdldC1lbGVtZW50JykpLnJlbW92ZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuXHRcdHdpbmRvdy5jb250cm9sUG9wT3V0cy5jbG9zZVBvcE91dChlLnRhcmdldCk7XHJcblx0fSk7XHJcblxyXG5cclxuXHQvLyBNYWtlIHN1cmUgYWxsIGV4dGVybmFsIGxpbmtzIG9wZW4gaW4gYSBuZXcgd2luZG93L3RhYlxyXG5cdCQoXCJhW2hyZWZePWh0dHBdXCIpLmVhY2goZnVuY3Rpb24oKXtcclxuXHRcdGlmKHRoaXMuaHJlZi5pbmRleE9mKGxvY2F0aW9uLmhvc3RuYW1lKSA9PSAtMSkge1xyXG5cdFx0XHQkKHRoaXMpLmF0dHIoe1xyXG5cdFx0XHRcdHRhcmdldDogXCJfYmxhbmtcIlxyXG5cdFx0XHR9KTtcclxuXHRcdH1cclxuXHR9KTtcclxuXHJcblx0Ly8gQWRkcyBhbmFseXRpY3MgZm9yIGFydGljbGUgcGFnZSBjbGlja3NcclxuXHQkKCcucm9vdCcpLmZpbmQoJ2EnKS5lYWNoKGZ1bmN0aW9uKGluZGV4LCBpdGVtKSB7XHJcblxyXG5cdFx0JCh0aGlzKS5hZGRDbGFzcygnY2xpY2stdXRhZycpO1xyXG5cclxuXHRcdHZhciBsaW5rU3RyaW5nO1xyXG5cclxuXHRcdGlmKHRoaXMuaHJlZi5pbmRleE9mKGxvY2F0aW9uLmhvc3RuYW1lKSA9PSAtMSkge1xyXG5cdFx0XHRsaW5rU3RyaW5nID0gJ0V4dGVybmFsOicgKyB0aGlzLmhyZWY7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHRsaW5rU3RyaW5nID0gdGhpcy5ocmVmO1xyXG5cdFx0fVxyXG5cclxuXHRcdGlmICgkKHRoaXMpLmRhdGEoJ2luZm8nKSA9PSB1bmRlZmluZWQpIHtcclxuXHRcdFx0JCh0aGlzKS5kYXRhKCdpbmZvJywgJ3sgXCJldmVudF9uYW1lXCI6IFwiZW1iZWRlZF9saW5rX2NsaWNrX3Rocm91Z2hcIiwgXCJjbGlja190aHJvdWdoX3NvdXJjZVwiOiBcIicgKyAkKCdoMScpLnRleHQgKyAnXCIsIFwiY2xpY2tfdGhyb3VnaF9kZXN0aW5hdGlvblwiOiBcIicgKyBsaW5rU3RyaW5nICsgJ1wifScpO1xyXG5cdFx0fVxyXG5cdH0pO1xyXG5cclxuXHQkKCcuZ2VuZXJhbC1oZWFkZXJfX25hdmlnYXRpb24nKS5lYWNoKGZ1bmN0aW9uKCkge1xyXG5cclxuXHRcdCQodGhpcykub24oJ3Njcm9sbCcsIGZ1bmN0aW9uKCkge1xyXG5cdFx0XHR2YXIgc2Nyb2xsTGVmdCA9ICQodGhpcykuc2Nyb2xsTGVmdCgpO1xyXG5cdFx0XHR2YXIgc2Nyb2xsV2lkdGggPSAkKHRoaXMpWzBdLnNjcm9sbFdpZHRoO1xyXG5cdFx0XHR2YXIgd2luV2lkdGggPSAkKHdpbmRvdykud2lkdGgoKTtcclxuXHJcblx0XHRcdGlmKHNjcm9sbExlZnQgPiAzMikge1xyXG5cdFx0XHRcdCQoJy5nZW5lcmFsLWhlYWRlcl9fbmF2aWdhdGlvbi1zY3JvbGxlci0tbGVmdCcpLmFkZENsYXNzKCdpcy12aXNpYmxlJyk7XHJcblx0XHRcdH0gZWxzZSB7XHJcblx0XHRcdFx0JCgnLmdlbmVyYWwtaGVhZGVyX19uYXZpZ2F0aW9uLXNjcm9sbGVyLS1sZWZ0JykucmVtb3ZlQ2xhc3MoJ2lzLXZpc2libGUnKTtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0aWYoc2Nyb2xsTGVmdCArIHdpbldpZHRoIDwgc2Nyb2xsV2lkdGggLSAzMikge1xyXG5cdFx0XHRcdCQoJy5nZW5lcmFsLWhlYWRlcl9fbmF2aWdhdGlvbi1zY3JvbGxlci0tcmlnaHQnKS5hZGRDbGFzcygnaXMtdmlzaWJsZScpO1xyXG5cdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdCQoJy5nZW5lcmFsLWhlYWRlcl9fbmF2aWdhdGlvbi1zY3JvbGxlci0tcmlnaHQnKS5yZW1vdmVDbGFzcygnaXMtdmlzaWJsZScpO1xyXG5cdFx0XHR9XHJcblxyXG5cdFx0fSk7XHJcblxyXG5cdFx0dmFyIHNjcm9sbExlZnQgPSAkKHRoaXMpLnNjcm9sbExlZnQoKTtcclxuXHRcdHZhciBzY3JvbGxXaWR0aCA9ICQodGhpcylbMF0uc2Nyb2xsV2lkdGg7XHJcblx0XHR2YXIgd2luV2lkdGggPSAkKHdpbmRvdykud2lkdGgoKTtcclxuXHJcblx0XHRpZihzY3JvbGxMZWZ0ICsgd2luV2lkdGggPCBzY3JvbGxXaWR0aCAtIDMyKSB7XHJcblx0XHRcdCQoJy5nZW5lcmFsLWhlYWRlcl9fbmF2aWdhdGlvbi1zY3JvbGxlci0tcmlnaHQnKS5hZGRDbGFzcygnaXMtdmlzaWJsZScpO1xyXG5cdFx0fSBlbHNlIHtcclxuXHRcdFx0JCgnLmdlbmVyYWwtaGVhZGVyX19uYXZpZ2F0aW9uLXNjcm9sbGVyLS1yaWdodCcpLnJlbW92ZUNsYXNzKCdpcy12aXNpYmxlJyk7XHJcblx0XHR9XHJcblx0fSk7XHJcblxyXG5cclxuXHQvLyBTbW9vdGgsIGNsaWNrYWJsZSBzY3JvbGxpbmcgZm9yIEdlbmVyYWwgcGFnZSBoZWFkZXJzXHJcblx0dmFyIHNtb290aFNjcm9sbGluZ05hdiA9IGZ1bmN0aW9uKCkge1xyXG5cclxuXHRcdC8vIENhY2hlIGZvciBsZXNzIERPTSBjaGVja2luZ1xyXG5cdFx0dmFyIFNjcm9sbGFibGUgPSAkKCcuZ2VuZXJhbC1oZWFkZXJfX25hdmlnYXRpb24nKTtcclxuXHRcdHZhciBDb250YWluZXIgPSAkKCcuZ2VuZXJhbC1oZWFkZXInKTtcclxuXHJcblx0XHQvLyBGaW5kIGN1cnJlbnQgc2Nyb2xsIGRpc3RhbmNlIGlzIGZyb20gbGVmdCBhbmQgcmlnaHQgZWRnZXNcclxuXHRcdHZhciBzY3JvbGxEaXN0YW5jZSA9IGZ1bmN0aW9uKCkge1xyXG5cdFx0XHRyZXR1cm4ge1xyXG5cdFx0XHRcdGxlZnQ6IFNjcm9sbGFibGUuc2Nyb2xsTGVmdCgpLFxyXG5cdFx0XHRcdHJpZ2h0OiBTY3JvbGxhYmxlWzBdLnNjcm9sbFdpZHRoIC0gKENvbnRhaW5lci53aWR0aCgpICsgU2Nyb2xsYWJsZS5zY3JvbGxMZWZ0KCkpXHJcblx0XHRcdH07XHJcblx0XHR9O1xyXG5cclxuXHRcdHZhciBpbml0ID0gZnVuY3Rpb24oKSB7XHJcblxyXG5cdFx0XHQkKCcuZ2VuZXJhbC1oZWFkZXJfX25hdmlnYXRpb24tc2Nyb2xsZXItLXJpZ2h0Jykub24oJ2NsaWNrJywgZnVuY3Rpb24oKSB7XHJcblx0XHRcdFx0aWYoc2Nyb2xsRGlzdGFuY2UoKS5yaWdodCA+IDApIHsgLy8gTm90IG9uIHJpZ2h0IGVkZ2VcclxuXHRcdFx0XHRcdHNtb290aFNjcm9sbCgyMDAsICdyaWdodCcpO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fSk7XHJcblxyXG5cdFx0XHQkKCcuZ2VuZXJhbC1oZWFkZXJfX25hdmlnYXRpb24tc2Nyb2xsZXItLWxlZnQnKS5vbignY2xpY2snLCBmdW5jdGlvbigpIHtcclxuXHRcdFx0XHRpZihzY3JvbGxEaXN0YW5jZSgpLmxlZnQgPiAwKSB7XHJcblx0XHRcdFx0XHRzbW9vdGhTY3JvbGwoMjAwLCAnbGVmdCcpO1xyXG5cdFx0XHRcdH1cclxuXHRcdFx0fSk7XHJcblx0XHR9O1xyXG5cclxuXHRcdHZhciBzY3JvbGxUb1RpbWVyQ2FjaGU7XHJcblx0XHR2YXIgdG90YWxUcmF2ZWwgPSBudWxsO1xyXG5cdFx0dmFyIGR1cmF0aW9uU3RhcnQgPSBudWxsO1xyXG5cclxuXHRcdC8vIFF1YWRyYXRpYyBlYXNlLW91dCBhbGdvcml0aG1cclxuXHRcdHZhciBlYXNpbmcgPSBmdW5jdGlvbih0aW1lLCBkaXN0YW5jZSkge1xyXG5cdFx0XHRyZXR1cm4gZGlzdGFuY2UgKiAodGltZSAqICgyIC0gdGltZSkpO1xyXG5cdFx0fTtcclxuXHJcblx0XHR2YXIgc21vb3RoU2Nyb2xsID0gZnVuY3Rpb24oZHVyYXRpb24sIGRpcmVjdGlvbikge1xyXG5cdFx0XHRpZiAoZHVyYXRpb24gPD0gMCkge1xyXG5cdFx0XHRcdC8vIFJlc2V0IGV2ZXJ5dGhpbmcgd2hlbiBkdXJhdGlvbiB0aW1lIGZpbmlzaGVzXHJcblx0XHRcdFx0dG90YWxUcmF2ZWwgPSBudWxsO1xyXG5cdFx0XHRcdGR1cmF0aW9uU3RhcnQgPSBudWxsO1xyXG5cdFx0XHRcdHJldHVybjtcclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0Ly8gU3RvcmUgZHVyYXRpb24gYXMgZHVyYXRpb25TdGFydCBvbiBmaXJzdCBsb29wXHJcblx0XHRcdGR1cmF0aW9uU3RhcnQgPSAhZHVyYXRpb25TdGFydCA/IGR1cmF0aW9uIDogZHVyYXRpb25TdGFydDtcclxuXHJcblx0XHRcdC8vIFN0b3JlIHRyYXZlbCBkaXN0YW5jZSAoY29udGFpbmVyIHdpZHRoKSBhcyB0b3RhbFRyYXZlbCBvbiBmaXJzdCBsb29wXHJcblx0XHRcdHRvdGFsVHJhdmVsID0gIXRvdGFsVHJhdmVsID8gQ29udGFpbmVyLndpZHRoKCkgOiB0b3RhbFRyYXZlbDtcclxuXHJcblx0XHRcdC8vIEZpbmRzIHBlcmNlbnRhZ2Ugb2YgZWxhcHNlZCB0aW1lIHNpbmNlIHN0YXJ0XHJcblx0XHRcdHZhciB0cmF2ZWxQY2VudCA9IDEgLSAoZHVyYXRpb24gLyBkdXJhdGlvblN0YXJ0KTtcclxuXHJcblx0XHRcdC8vIEZpbmRzIHRyYXZlbCBjaGFuZ2Ugb24gdGhpcyBsb29wLCBhZGp1c3RlZCBmb3IgZWFzZS1vdXRcclxuXHRcdFx0dmFyIHRyYXZlbCA9IGVhc2luZyh0cmF2ZWxQY2VudCwgKCh0b3RhbFRyYXZlbCAvIGR1cmF0aW9uU3RhcnQpICogMTApKTtcclxuXHJcblx0XHRcdHNjcm9sbFRvVGltZXJDYWNoZSA9IHNldFRpbWVvdXQoZnVuY3Rpb24oKSB7XHJcblx0XHRcdFx0aWYgKCFpc05hTihwYXJzZUludCh0cmF2ZWwsIDEwKSkpIHtcclxuXHRcdFx0XHRcdGlmKGRpcmVjdGlvbiA9PT0gJ3JpZ2h0Jykge1xyXG5cdFx0XHRcdFx0XHRTY3JvbGxhYmxlLnNjcm9sbExlZnQoU2Nyb2xsYWJsZS5zY3JvbGxMZWZ0KCkgKyB0cmF2ZWwpO1xyXG5cdFx0XHRcdFx0XHRzbW9vdGhTY3JvbGwoZHVyYXRpb24gLSAxMCwgZGlyZWN0aW9uKTtcclxuXHRcdFx0XHRcdH0gZWxzZSBpZihkaXJlY3Rpb24gPT09ICdsZWZ0Jykge1xyXG5cdFx0XHRcdFx0XHRTY3JvbGxhYmxlLnNjcm9sbExlZnQoU2Nyb2xsYWJsZS5zY3JvbGxMZWZ0KCkgLSB0cmF2ZWwpO1xyXG5cdFx0XHRcdFx0XHRzbW9vdGhTY3JvbGwoZHVyYXRpb24gLSAxMCwgZGlyZWN0aW9uKTtcclxuXHRcdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0fVxyXG5cdFx0XHR9LmJpbmQodGhpcyksIDEwKTtcclxuXHRcdH07XHJcblxyXG5cdFx0Ly8gQmluZCBldmVudCBsaXN0ZW5lcnNcclxuXHRcdGluaXQoKTtcclxuXHR9O1xyXG5cclxuXHQkKCcuanMtcmVnaXN0ZXItZmluYWwnKS5vbignY2xpY2snLGZ1bmN0aW9uKGUpe1xyXG5cclxuXHRcdHZhciBldmVudERldGFpbHMgPSB7XHJcblx0XHRcdC8vIGV2ZW50X25hbWU6IFwibmV3c2xldHRlciBvcHRpbnNcIlxyXG5cdFx0fTtcclxuXHRcdHZhciBjaGtEZXRhaWxzID0ge307XHJcblx0XHRpZiAoJCgnI25ld3NsZXR0ZXJzJykuaXMoJzpjaGVja2VkJykpIHtcclxuXHRcdFx0Y2hrRGV0YWlscy5uZXdzbGV0dGVyX29wdGluID0gXCJ0cnVlXCI7XHJcblx0XHRcdCQuZXh0ZW5kKGV2ZW50RGV0YWlscyxjaGtEZXRhaWxzKTtcclxuXHRcdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCBldmVudERldGFpbHMpICk7XHJcblx0XHR9IGVsc2Uge1xyXG5cdFx0XHRjaGtEZXRhaWxzLm5ld3NsZXR0ZXJfb3B0aW4gPSBcImZhbHNlXCI7XHJcblx0XHRcdCQuZXh0ZW5kKGV2ZW50RGV0YWlscyxjaGtEZXRhaWxzKTtcclxuXHRcdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCBldmVudERldGFpbHMpICk7XHJcblx0XHR9XHJcblx0fSk7XHJcblxyXG5cclxuXHQvLyBUT0RPIC0gUmVmYWN0b3IgdGhpcyBjb2RlLCB1cGRhdGUgY2xhc3MgbmFtZSB0byBhIGBqcy1gIG5hbWVcclxuXHQkKCcubWFuYWdlLXByZWZlcmVuY2VzJykuY2xpY2soZnVuY3Rpb24oZSkge1xyXG5cdFx0dmFyIHByZWZlcmVuY2VzRGF0YSA9IHtcclxuXHRcdFx0ZXZlbnRfbmFtZTogXCJtYW5hZ2UtcHJlZmVyZW5jZXNcIlxyXG5cdFx0fTtcclxuXHRcdGlmKCQoXCIjTmV3c2xldHRlck9wdEluXCIpLmlzKCc6Y2hlY2tlZCcpICYmICQoXCIjRG9Ob3RTZW5kT2ZmZXJzT3B0SW5cIikuaXMoJzpjaGVja2VkJykpIHtcclxuXHRcdFx0cHJlZmVyZW5jZXNEYXRhID0ge1xyXG5cdFx0XHRcdG5ld3NsZXR0ZXJfb3B0aW46IFwidHJ1ZVwiLFxyXG5cdFx0XHRcdGRvbm90X3NlbmRfb2ZmZXJzX29wdGluOiBcInRydWVcIlxyXG5cdFx0XHR9O1xyXG5cdFx0fVxyXG5cdFx0aWYoISQoXCIjTmV3c2xldHRlck9wdEluXCIpLmlzKCc6Y2hlY2tlZCcpICYmICQoXCIjRG9Ob3RTZW5kT2ZmZXJzT3B0SW5cIikuaXMoJzpjaGVja2VkJykpIHtcclxuXHRcdFx0cHJlZmVyZW5jZXNEYXRhID0ge1xyXG5cdFx0XHRcdG5ld3NsZXR0ZXJfb3B0aW46IFwiZmFsc2VcIixcclxuXHRcdFx0XHRkb25vdF9zZW5kX29mZmVyc19vcHRpbjogXCJ0cnVlXCJcclxuXHRcdFx0fTtcclxuXHRcdH1cclxuXHRcdGlmKCQoXCIjTmV3c2xldHRlck9wdEluXCIpLmlzKCc6Y2hlY2tlZCcpICYmICEkKFwiI0RvTm90U2VuZE9mZmVyc09wdEluXCIpLmlzKCc6Y2hlY2tlZCcpKSB7XHJcblx0XHRcdHByZWZlcmVuY2VzRGF0YSA9IHtcclxuXHRcdFx0XHRuZXdzbGV0dGVyX29wdGluOiBcInRydWVcIixcclxuXHRcdFx0XHRkb25vdF9zZW5kX29mZmVyc19vcHRpbjogXCJmYWxzZVwiXHJcblx0XHRcdH07XHJcblx0XHR9XHJcblx0XHRpZighJChcIiNOZXdzbGV0dGVyT3B0SW5cIikuaXMoJzpjaGVja2VkJykgJiYgISQoXCIjRG9Ob3RTZW5kT2ZmZXJzT3B0SW5cIikuaXMoJzpjaGVja2VkJykpIHtcclxuXHRcdFx0cHJlZmVyZW5jZXNEYXRhID0ge1xyXG5cdFx0XHRcdG5ld3NsZXR0ZXJfb3B0aW46IFwiZmFsc2VcIixcclxuXHRcdFx0XHRkb25vdF9zZW5kX29mZmVyc19vcHRpbjogXCJmYWxzZVwiXHJcblx0XHRcdH07XHJcblx0XHR9XHJcblxyXG5cdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCBwcmVmZXJlbmNlc0RhdGEpICk7XHJcblxyXG5cdH0pO1xyXG5cclxuXHJcblx0Ly8gRXhlY3V0ZSFcclxuXHRzbW9vdGhTY3JvbGxpbmdOYXYoKTtcclxuXHJcblxyXG5cdC8vIFRvZ2dsZSBnbG9iYWwgSW5mb3JtYSBiYXJcclxuXHQkKCcuaW5mb3JtYS1yaWJib25fX3RpdGxlJykub24oJ2NsaWNrJywgZnVuY3Rpb24gKGUpIHtcclxuXHRcdCQoJy5pbmZvcm1hLXJpYmJvbicpLnRvZ2dsZUNsYXNzKCdzaG93Jyk7XHJcblx0fSk7XHJcblxyXG5cclxuXHQkKCcuanMtdG9nZ2xlLWxpc3QnKS5vbignY2xpY2snLCBmdW5jdGlvbihlKSB7XHJcblx0XHQkKGUudGFyZ2V0KS5jbG9zZXN0KCcuanMtdG9nZ2xhYmxlLWxpc3Qtd3JhcHBlcicpLnRvZ2dsZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuXHR9KTtcclxuXHJcblx0JCgnLmNsaWNrLXV0YWcnKS5jbGljayhmdW5jdGlvbiAoZSkge1xyXG5cdFx0YW5hbHl0aWNzRXZlbnQoICQuZXh0ZW5kKGFuYWx5dGljc19kYXRhLCAkKHRoaXMpLmRhdGEoJ2luZm8nKSkgKTtcclxuXHR9KTtcclxuXHJcbiAgICAkKCcuc2VhcmNoLXJlc3VsdHMnKS5vbignY2xpY2snLCAnLmNsaWNrLXV0YWcnLCBmdW5jdGlvbiAoZSkge1xyXG4gICAgICAgIGFuYWx5dGljc0V2ZW50KCAkLmV4dGVuZChhbmFseXRpY3NfZGF0YSwgJCh0aGlzKS5kYXRhKCdpbmZvJykpICk7XHJcbiAgICB9KTtcclxuXHJcblx0JCgnI2Noa0FTQmlsbGluZycpLmNsaWNrKGZ1bmN0aW9uKGUpe1xyXG5cdFx0aWYoJCh0aGlzKS5pcygnOmNoZWNrZWQnKSlcclxuXHRcdHtcclxuXHRcdFx0JCgnI2RkbFNoaXBwaW5nQ291bnRyeScpLnZhbCgkKCcjZGRsQmlsbGluZ0NvdW50cnknKS52YWwoKSk7XHJcblx0XHRcdCQoJyN0eHRTaGlwcGluZ0FkZHJlc3MxJykudmFsKCQoJyN0eHRCaWxsaW5nQWRkcmVzczEnKS52YWwoKSk7XHJcblx0XHRcdCQoJyN0eHRTaGlwcGluZ0FkZHJlc3MyJykudmFsKCQoJyN0eHRCaWxsaW5nQWRkcmVzczInKS52YWwoKSk7XHJcblx0XHRcdCQoJyN0eHRTaGlwcGluZ0NpdHknKS52YWwoJCgnI3R4dEJpbGxpbmdDaXR5JykudmFsKCkpO1xyXG5cdFx0XHQkKCcjdHh0U2hpcHBpbmdTdGF0ZScpLnZhbCgkKCcjdHh0QmlsbGluZ1N0YXRlJykudmFsKCkpO1xyXG5cdFx0XHQkKCcjdHh0U2hpcHBpbmdQb3N0YWxDb2RlJykudmFsKCQoJyN0eHRCaWxsaW5nUG9zdGFsQ29kZScpLnZhbCgpKTtcclxuXHRcdH1cclxuXHR9KTtcclxuXHJcblx0Ly8gQWNjb3VudCAtIEVtYWlsIFByZWZlcmVuY2VzIHRvZ2dsZXJcclxuXHQkKCcuanMtYWNjb3VudC1lbWFpbC10b2dnbGUtYWxsJykub24oJ2NsaWNrJywgZnVuY3Rpb24oZSkge1xyXG5cdFx0JCgnLmpzLXVwZGF0ZS1lbWFpbC1wcmVmcycpLmF0dHIoJ2Rpc2FibGVkJywgbnVsbCk7XHJcblx0fSk7XHJcblxyXG5cdCQoJy5qcy1hY2NvdW50LWVtYWlsLWNoZWNrYm94Jykub24oJ2NsaWNrJywgZnVuY3Rpb24oZSkge1xyXG5cdFx0JCgnLmpzLXVwZGF0ZS1lbWFpbC1wcmVmcycpLmF0dHIoJ2Rpc2FibGVkJywgbnVsbCk7XHJcblx0fSk7XHJcblxyXG53aW5kb3cuZmluZFRvb2x0aXBzID0gZnVuY3Rpb24oKSB7XHJcblx0JCgnLmpzLXRvZ2dsZS10b29sdGlwJykuZWFjaChmdW5jdGlvbihpbmRleCwgaXRlbSkge1xyXG5cdFx0dmFyIHRvb2x0aXA7XHJcblx0XHQkKGl0ZW0pLmRhdGEoXCJ0dFZpc2libGVcIiwgZmFsc2UpO1xyXG5cdFx0JChpdGVtKS5kYXRhKFwidHRUb3VjaFRyaWdnZXJlZFwiLCBmYWxzZSk7XHJcblxyXG5cdFx0JChpdGVtKS5vbignbW91c2VlbnRlciB0b3VjaHN0YXJ0JywgZnVuY3Rpb24oZSkge1xyXG5cdFx0XHRlLnByZXZlbnREZWZhdWx0KCk7XHJcblxyXG5cdFx0XHRpZiAoZS50eXBlID09PSBcInRvdWNoc3RhcnRcIikge1xyXG5cdFx0XHRcdCQoaXRlbSkuZGF0YShcInR0VG91Y2hUcmlnZ2VyZWRcIiwgdHJ1ZSk7XHJcblx0XHRcdH1cclxuXHJcblx0XHRcdC8vIEFjdHVhbCBtb3VzZSBldmVudHMgdGhyb3duIGNhbiBiZSBhbnkgbnVtYmVyIG9mIHRoaW5ncy4uLlxyXG5cdFx0XHRpZiAoKGUudHlwZSA9PT0gKFwibW91c2VvdmVyXCIpIHx8IGUudHlwZSA9PT0gKFwibW91c2VlbnRlclwiKSkgJiYgJChpdGVtKS5kYXRhKFwidHRUb3VjaFRyaWdnZXJlZFwiKSkge1xyXG5cdFx0XHRcdC8vIERvIG5vdGhpbmdcclxuXHRcdFx0fVxyXG5cdFx0XHRlbHNlIGlmICgkKGl0ZW0pLmRhdGEoXCJ0dFZpc2libGVcIikgJiYgZS50eXBlID09PSBcInRvdWNoc3RhcnRcIikge1xyXG5cdFx0XHRcdCQoaXRlbSkuZGF0YShcInR0VmlzaWJsZVwiLCBmYWxzZSk7XHJcblx0XHRcdFx0JChpdGVtKS5kYXRhKFwidHRUb3VjaFRyaWdnZXJlZFwiLCBmYWxzZSk7XHJcblx0XHRcdFx0dG9vbHRpcC5oaWRlUG9wdXAoKTtcclxuXHRcdFx0fVxyXG5cdFx0XHRlbHNlIHtcclxuXHRcdFx0XHQkKGl0ZW0pLmRhdGEoXCJ0dFZpc2libGVcIiwgdHJ1ZSk7XHJcblx0XHRcdFx0Y29uc3Qgb2Zmc2V0cyA9ICQoaXRlbSkub2Zmc2V0KCk7XHJcblx0XHRcdFx0dG9vbHRpcCA9IHRvb2x0aXBDb250cm9sbGVyKHtcclxuXHRcdFx0XHRcdGlzSGlkZGVuOiBmYWxzZSxcclxuXHRcdFx0XHRcdGh0bWw6ICQoaXRlbSkuZGF0YSgndG9vbHRpcC10ZXh0JyksXHJcblx0XHRcdFx0XHR0b3A6IG9mZnNldHMudG9wLFxyXG5cdFx0XHRcdFx0bGVmdDogb2Zmc2V0cy5sZWZ0ICsgJCh0aGlzKS53aWR0aCgpLzIsXHJcblx0XHRcdFx0XHR0cmlhbmdsZTogJ2JvdHRvbSdcclxuXHRcdFx0XHR9KTtcclxuXHRcdFx0fVxyXG5cdFx0fSk7XHJcblxyXG5cdFx0JChpdGVtKS5vbignbW91c2VsZWF2ZScsIGZ1bmN0aW9uKCkge1xyXG5cdFx0XHQkKGl0ZW0pLmRhdGEoXCJ0dFZpc2libGVcIiwgZmFsc2UpO1xyXG5cdFx0XHR0b29sdGlwLmhpZGVQb3B1cCgpO1xyXG5cdFx0fSk7XHJcblx0fSk7XHJcblxyXG5cclxufTtcclxuXHJcbiAgICAvLy8vLy9UaGUgZm9sbG93aW5nIHN0eWxlIHVwZGF0ZXMgYXJlIHRvIGZpeCBxdWljay1ib3ggYW5kIHF1b3RlIHN0eWxpbmcgZm9yIG1pZ3JhdGVkIGlzc3VlcyAoSUlTLTM2NilcclxuJCgnLnNpZGVib3ggLnF1aWNrZmFjdHN0aXRsZScpLnBhcmVudCgpLnJlbW92ZUNsYXNzKCdzaWRlYm94JykuYWRkQ2xhc3MoJ3F1aWNrLWZhY3RzJyk7XHJcbiQoJy5xdWljay1mYWN0cyAucXVpY2tmYWN0c3RpdGxlJykucmVtb3ZlQ2xhc3MoJ3F1aWNrZmFjdHN0aXRsZScpLmFkZENsYXNzKCdxdWljay1mYWN0c19faGVhZGVyJyk7XHJcbiQoJy5xdWljay1mYWN0cyAucXVpY2tmYWN0c3RleHQnKS5yZW1vdmVDbGFzcygncXVpY2tmYWN0c3RleHQnKS5hZGRDbGFzcygncXVpY2stZmFjdHNfX3RleHQnKTtcclxuJCgnLnF1aWNrLWZhY3RzIC5xdWlja2ZhY3RzYnVsbGV0ZWQnKS5yZW1vdmVDbGFzcygncXVpY2tmYWN0c2J1bGxldGVkJykuYWRkQ2xhc3MoJ3F1aWNrLWZhY3RzX19saXN0LS11bCcpO1xyXG4kKCcuc2lkZWJveCBibG9ja3F1b3RlJykucGFyZW50KCkucmVtb3ZlQ2xhc3MoJ3NpZGVib3gnKS5hZGRDbGFzcygncXVvdGUnKTtcclxuXHJcbiQoJy5xdW90ZSBibG9ja3F1b3RlJykuZWFjaChmdW5jdGlvbiAoKSB7XHJcbiAgICAkKHRoaXMpLnJlcGxhY2VXaXRoKFwiPHNwYW4+XCIgKyAkKHRoaXMpLmh0bWwoKSArIFwiPC9zcGFuPlwiKTtcclxuICAgIC8vIHRoaXMgZnVuY3Rpb24gaXMgZXhlY3V0ZWQgZm9yIGFsbCAnY29kZScgZWxlbWVudHMsIGFuZFxyXG4gICAgLy8gJ3RoaXMnIHJlZmVycyB0byBvbmUgZWxlbWVudCBmcm9tIHRoZSBzZXQgb2YgYWxsICdjb2RlJ1xyXG4gICAgLy8gZWxlbWVudHMgZWFjaCB0aW1lIGl0IGlzIGNhbGxlZC5cclxufSk7XHJcbiAgICAvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9cclxuXHJcbndpbmRvdy5maW5kVG9vbHRpcHMoKTtcclxuXHJcblxyXG5cclxuXHQvLyBUd2l0dGVyIHNoYXJpbmcgSlNcclxuXHR3aW5kb3cudHd0dHIgPSBmdW5jdGlvbih0LGUscil7dmFyIG4saT10LmdldEVsZW1lbnRzQnlUYWdOYW1lKGUpWzBdLFxyXG5cdFx0dz13aW5kb3cudHd0dHJ8fHt9O1xyXG5cdFx0cmV0dXJuIHQuZ2V0RWxlbWVudEJ5SWQocikgPyB3IDogKG49dC5jcmVhdGVFbGVtZW50KGUpLFxyXG5cdFx0bi5pZD1yLG4uc3JjPVwiaHR0cHM6Ly9wbGF0Zm9ybS50d2l0dGVyLmNvbS93aWRnZXRzLmpzXCIsXHJcblx0XHRpLnBhcmVudE5vZGUuaW5zZXJ0QmVmb3JlKG4saSksdy5fZT1bXSxcclxuXHRcdHcucmVhZHk9ZnVuY3Rpb24odCkgeyB3Ll9lLnB1c2godCk7IH0sXHJcblx0XHR3KTsgfSAoZG9jdW1lbnQsXCJzY3JpcHRcIixcInR3aXR0ZXItd2pzXCIpO1xyXG5cclxuXHJcbiAgICAkKCcuY29udGFjdEluZm9OdW1lcmljRmllbGQnKS5vbigna2V5cHJlc3MnLCBmdW5jdGlvbiAoZSkge1xyXG4gICAgICAgIGUgPSAoZSkgPyBlIDogd2luZG93LmV2ZW50O1xyXG4gICAgICAgIHZhciBjaGFyQ29kZSA9IChlLndoaWNoKSA/IGUud2hpY2ggOiBlLmtleUNvZGU7XHJcbiAgICAgICAgaWYgKGNoYXJDb2RlID4gMzEgJiYgKGNoYXJDb2RlIDwgNDggfHwgY2hhckNvZGUgPiA1NykpIHtcclxuICAgICAgICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gdHJ1ZTtcclxuICAgIH0pO1xyXG5cclxuXHQvLyBQcmV0dHkgc2VsZWN0IGJveGVzXHJcblx0JCgnc2VsZWN0Om5vdCgubmctc2NvcGUpJykuc2VsZWN0aXZpdHkoe1xyXG5cdFx0c2hvd1NlYXJjaElucHV0SW5Ecm9wZG93bjogZmFsc2UsXHJcblx0XHRwb3NpdGlvbkRyb3Bkb3duOiBmdW5jdGlvbigkZHJvcGRvd25FbCwgICRzZWxlY3RFbCkge1xyXG5cdFx0XHQkZHJvcGRvd25FbC5jc3MoXCJ3aWR0aFwiLCAkc2VsZWN0RWwud2lkdGgoKSArIFwicHhcIik7XHJcblx0XHR9XHJcblx0fSk7XHJcblxyXG5cdCQoXCIuc2VsZWN0aXZpdHktaW5wdXQgLnNlbGVjdGl2aXR5LXNpbmdsZS1zZWxlY3RcIikuZWFjaChmdW5jdGlvbigpIHtcclxuXHRcdCQodGhpcykuYXBwZW5kKCc8c3BhbiBjbGFzcz1cInNlbGVjdGl2aXR5LWFycm93XCI+PHN2ZyBjbGFzcz1cImFsZXJ0X19pY29uXCI+PHVzZSB4bGluazpocmVmPVwiL2Rpc3QvaW1nL3N2Zy1zcHJpdGUuc3ZnI3NvcnQtZG93bi1hcnJvd1wiPjwvdXNlPjwvc3ZnPjwvc3Bhbj4nKTtcclxuXHR9KTtcclxufSk7XHJcbiAgICAiLCIvKiFcclxuICogSmF2YVNjcmlwdCBDb29raWUgdjIuMS4wXHJcbiAqIGh0dHBzOi8vZ2l0aHViLmNvbS9qcy1jb29raWUvanMtY29va2llXHJcbiAqXHJcbiAqIENvcHlyaWdodCAyMDA2LCAyMDE1IEtsYXVzIEhhcnRsICYgRmFnbmVyIEJyYWNrXHJcbiAqIFJlbGVhc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZVxyXG4gKi9cclxuKGZ1bmN0aW9uIChmYWN0b3J5KSB7XHJcblx0aWYgKHR5cGVvZiBkZWZpbmUgPT09ICdmdW5jdGlvbicgJiYgZGVmaW5lLmFtZCkge1xyXG5cdFx0ZGVmaW5lKGZhY3RvcnkpO1xyXG5cdH0gZWxzZSBpZiAodHlwZW9mIGV4cG9ydHMgPT09ICdvYmplY3QnKSB7XHJcblx0XHRtb2R1bGUuZXhwb3J0cyA9IGZhY3RvcnkoKTtcclxuXHR9IGVsc2Uge1xyXG5cdFx0dmFyIF9PbGRDb29raWVzID0gd2luZG93LkNvb2tpZXM7XHJcblx0XHR2YXIgYXBpID0gd2luZG93LkNvb2tpZXMgPSBmYWN0b3J5KCk7XHJcblx0XHRhcGkubm9Db25mbGljdCA9IGZ1bmN0aW9uICgpIHtcclxuXHRcdFx0d2luZG93LkNvb2tpZXMgPSBfT2xkQ29va2llcztcclxuXHRcdFx0cmV0dXJuIGFwaTtcclxuXHRcdH07XHJcblx0fVxyXG59KGZ1bmN0aW9uICgpIHtcclxuXHRmdW5jdGlvbiBleHRlbmQgKCkge1xyXG5cdFx0dmFyIGkgPSAwO1xyXG5cdFx0dmFyIHJlc3VsdCA9IHt9O1xyXG5cdFx0Zm9yICg7IGkgPCBhcmd1bWVudHMubGVuZ3RoOyBpKyspIHtcclxuXHRcdFx0dmFyIGF0dHJpYnV0ZXMgPSBhcmd1bWVudHNbIGkgXTtcclxuXHRcdFx0Zm9yICh2YXIga2V5IGluIGF0dHJpYnV0ZXMpIHtcclxuXHRcdFx0XHRyZXN1bHRba2V5XSA9IGF0dHJpYnV0ZXNba2V5XTtcclxuXHRcdFx0fVxyXG5cdFx0fVxyXG5cdFx0cmV0dXJuIHJlc3VsdDtcclxuXHR9XHJcblxyXG5cdGZ1bmN0aW9uIGluaXQgKGNvbnZlcnRlcikge1xyXG5cdFx0ZnVuY3Rpb24gYXBpIChrZXksIHZhbHVlLCBhdHRyaWJ1dGVzKSB7XHJcblx0XHRcdHZhciByZXN1bHQ7XHJcblxyXG5cdFx0XHQvLyBXcml0ZVxyXG5cclxuXHRcdFx0aWYgKGFyZ3VtZW50cy5sZW5ndGggPiAxKSB7XHJcblx0XHRcdFx0YXR0cmlidXRlcyA9IGV4dGVuZCh7XHJcblx0XHRcdFx0XHRwYXRoOiAnLydcclxuXHRcdFx0XHR9LCBhcGkuZGVmYXVsdHMsIGF0dHJpYnV0ZXMpO1xyXG5cclxuXHRcdFx0XHRpZiAodHlwZW9mIGF0dHJpYnV0ZXMuZXhwaXJlcyA9PT0gJ251bWJlcicpIHtcclxuXHRcdFx0XHRcdHZhciBleHBpcmVzID0gbmV3IERhdGUoKTtcclxuXHRcdFx0XHRcdGV4cGlyZXMuc2V0TWlsbGlzZWNvbmRzKGV4cGlyZXMuZ2V0TWlsbGlzZWNvbmRzKCkgKyBhdHRyaWJ1dGVzLmV4cGlyZXMgKiA4NjRlKzUpO1xyXG5cdFx0XHRcdFx0YXR0cmlidXRlcy5leHBpcmVzID0gZXhwaXJlcztcclxuXHRcdFx0XHR9XHJcblxyXG5cdFx0XHRcdHRyeSB7XHJcblx0XHRcdFx0XHRyZXN1bHQgPSBKU09OLnN0cmluZ2lmeSh2YWx1ZSk7XHJcblx0XHRcdFx0XHRpZiAoL15bXFx7XFxbXS8udGVzdChyZXN1bHQpKSB7XHJcblx0XHRcdFx0XHRcdHZhbHVlID0gcmVzdWx0O1xyXG5cdFx0XHRcdFx0fVxyXG5cdFx0XHRcdH0gY2F0Y2ggKGUpIHt9XHJcblxyXG5cdFx0XHRcdGlmICghY29udmVydGVyLndyaXRlKSB7XHJcblx0XHRcdFx0XHR2YWx1ZSA9IGVuY29kZVVSSUNvbXBvbmVudChTdHJpbmcodmFsdWUpKVxyXG5cdFx0XHRcdFx0XHQucmVwbGFjZSgvJSgyM3wyNHwyNnwyQnwzQXwzQ3wzRXwzRHwyRnwzRnw0MHw1Qnw1RHw1RXw2MHw3Qnw3RHw3QykvZywgZGVjb2RlVVJJQ29tcG9uZW50KTtcclxuXHRcdFx0XHR9IGVsc2Uge1xyXG5cdFx0XHRcdFx0dmFsdWUgPSBjb252ZXJ0ZXIud3JpdGUodmFsdWUsIGtleSk7XHJcblx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHRrZXkgPSBlbmNvZGVVUklDb21wb25lbnQoU3RyaW5nKGtleSkpO1xyXG5cdFx0XHRcdGtleSA9IGtleS5yZXBsYWNlKC8lKDIzfDI0fDI2fDJCfDVFfDYwfDdDKS9nLCBkZWNvZGVVUklDb21wb25lbnQpO1xyXG5cdFx0XHRcdGtleSA9IGtleS5yZXBsYWNlKC9bXFwoXFwpXS9nLCBlc2NhcGUpO1xyXG5cclxuXHRcdFx0XHRyZXR1cm4gKGRvY3VtZW50LmNvb2tpZSA9IFtcclxuXHRcdFx0XHRcdGtleSwgJz0nLCB2YWx1ZSxcclxuXHRcdFx0XHRcdGF0dHJpYnV0ZXMuZXhwaXJlcyAmJiAnOyBleHBpcmVzPScgKyBhdHRyaWJ1dGVzLmV4cGlyZXMudG9VVENTdHJpbmcoKSwgLy8gdXNlIGV4cGlyZXMgYXR0cmlidXRlLCBtYXgtYWdlIGlzIG5vdCBzdXBwb3J0ZWQgYnkgSUVcclxuXHRcdFx0XHRcdGF0dHJpYnV0ZXMucGF0aCAgICAmJiAnOyBwYXRoPScgKyBhdHRyaWJ1dGVzLnBhdGgsXHJcblx0XHRcdFx0XHRhdHRyaWJ1dGVzLmRvbWFpbiAgJiYgJzsgZG9tYWluPScgKyBhdHRyaWJ1dGVzLmRvbWFpbixcclxuXHRcdFx0XHRcdGF0dHJpYnV0ZXMuc2VjdXJlID8gJzsgc2VjdXJlJyA6ICcnXHJcblx0XHRcdFx0XS5qb2luKCcnKSk7XHJcblx0XHRcdH1cclxuXHJcblx0XHRcdC8vIFJlYWRcclxuXHJcblx0XHRcdGlmICgha2V5KSB7XHJcblx0XHRcdFx0cmVzdWx0ID0ge307XHJcblx0XHRcdH1cclxuXHJcblx0XHRcdC8vIFRvIHByZXZlbnQgdGhlIGZvciBsb29wIGluIHRoZSBmaXJzdCBwbGFjZSBhc3NpZ24gYW4gZW1wdHkgYXJyYXlcclxuXHRcdFx0Ly8gaW4gY2FzZSB0aGVyZSBhcmUgbm8gY29va2llcyBhdCBhbGwuIEFsc28gcHJldmVudHMgb2RkIHJlc3VsdCB3aGVuXHJcblx0XHRcdC8vIGNhbGxpbmcgXCJnZXQoKVwiXHJcblx0XHRcdHZhciBjb29raWVzID0gZG9jdW1lbnQuY29va2llID8gZG9jdW1lbnQuY29va2llLnNwbGl0KCc7ICcpIDogW107XHJcblx0XHRcdHZhciByZGVjb2RlID0gLyglWzAtOUEtWl17Mn0pKy9nO1xyXG5cdFx0XHR2YXIgaSA9IDA7XHJcblxyXG5cdFx0XHRmb3IgKDsgaSA8IGNvb2tpZXMubGVuZ3RoOyBpKyspIHtcclxuXHRcdFx0XHR2YXIgcGFydHMgPSBjb29raWVzW2ldLnNwbGl0KCc9Jyk7XHJcblx0XHRcdFx0dmFyIG5hbWUgPSBwYXJ0c1swXS5yZXBsYWNlKHJkZWNvZGUsIGRlY29kZVVSSUNvbXBvbmVudCk7XHJcblx0XHRcdFx0dmFyIGNvb2tpZSA9IHBhcnRzLnNsaWNlKDEpLmpvaW4oJz0nKTtcclxuXHJcblx0XHRcdFx0aWYgKGNvb2tpZS5jaGFyQXQoMCkgPT09ICdcIicpIHtcclxuXHRcdFx0XHRcdGNvb2tpZSA9IGNvb2tpZS5zbGljZSgxLCAtMSk7XHJcblx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHR0cnkge1xyXG5cdFx0XHRcdFx0Y29va2llID0gY29udmVydGVyLnJlYWQgP1xyXG5cdFx0XHRcdFx0XHRjb252ZXJ0ZXIucmVhZChjb29raWUsIG5hbWUpIDogY29udmVydGVyKGNvb2tpZSwgbmFtZSkgfHxcclxuXHRcdFx0XHRcdFx0Y29va2llLnJlcGxhY2UocmRlY29kZSwgZGVjb2RlVVJJQ29tcG9uZW50KTtcclxuXHJcblx0XHRcdFx0XHRpZiAodGhpcy5qc29uKSB7XHJcblx0XHRcdFx0XHRcdHRyeSB7XHJcblx0XHRcdFx0XHRcdFx0Y29va2llID0gSlNPTi5wYXJzZShjb29raWUpO1xyXG5cdFx0XHRcdFx0XHR9IGNhdGNoIChlKSB7fVxyXG5cdFx0XHRcdFx0fVxyXG5cclxuXHRcdFx0XHRcdGlmIChrZXkgPT09IG5hbWUpIHtcclxuXHRcdFx0XHRcdFx0cmVzdWx0ID0gY29va2llO1xyXG5cdFx0XHRcdFx0XHRicmVhaztcclxuXHRcdFx0XHRcdH1cclxuXHJcblx0XHRcdFx0XHRpZiAoIWtleSkge1xyXG5cdFx0XHRcdFx0XHRyZXN1bHRbbmFtZV0gPSBjb29raWU7XHJcblx0XHRcdFx0XHR9XHJcblx0XHRcdFx0fSBjYXRjaCAoZSkge31cclxuXHRcdFx0fVxyXG5cclxuXHRcdFx0cmV0dXJuIHJlc3VsdDtcclxuXHRcdH1cclxuXHJcblx0XHRhcGkuZ2V0ID0gYXBpLnNldCA9IGFwaTtcclxuXHRcdGFwaS5nZXRKU09OID0gZnVuY3Rpb24gKCkge1xyXG5cdFx0XHRyZXR1cm4gYXBpLmFwcGx5KHtcclxuXHRcdFx0XHRqc29uOiB0cnVlXHJcblx0XHRcdH0sIFtdLnNsaWNlLmNhbGwoYXJndW1lbnRzKSk7XHJcblx0XHR9O1xyXG5cdFx0YXBpLmRlZmF1bHRzID0ge307XHJcblxyXG5cdFx0YXBpLnJlbW92ZSA9IGZ1bmN0aW9uIChrZXksIGF0dHJpYnV0ZXMpIHtcclxuXHRcdFx0YXBpKGtleSwgJycsIGV4dGVuZChhdHRyaWJ1dGVzLCB7XHJcblx0XHRcdFx0ZXhwaXJlczogLTFcclxuXHRcdFx0fSkpO1xyXG5cdFx0fTtcclxuXHJcblx0XHRhcGkud2l0aENvbnZlcnRlciA9IGluaXQ7XHJcblxyXG5cdFx0cmV0dXJuIGFwaTtcclxuXHR9XHJcblxyXG5cdHJldHVybiBpbml0KGZ1bmN0aW9uICgpIHt9KTtcclxufSkpO1xyXG4kKGRvY3VtZW50KS5yZWFkeShmdW5jdGlvbigpe1xyXG5cdHZhciBjb29raWVOYW1lID0gJ21lbnVuYXZpZ2F0aW9uY29va2llJywgLy8gTmFtZSBvZiBvdXIgY29va2llXHJcbiAgICAgICAgY29va2llVmFsdWUgPSAneWVzJzsgLy8gVmFsdWUgb2YgY29va2llXHJcblxyXG4gICAgICAgIGZ1bmN0aW9uIENsb3NlTmF2aWdhdGlvbk1lbnUoKXtcclxuXHRcdFx0JCgnLm1haW4tbWVudScpLnJlbW92ZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuXHRcdFx0JCgnLm1lbnUtdG9nZ2xlcicpLnJlbW92ZUNsYXNzKCdpcy1hY3RpdmUnKTtcclxuXHRcdFx0JCgnLmhlYWRlcl9fd3JhcHBlciAubWVudS10b2dnbGVyJykucmVtb3ZlQ2xhc3MoJ2lzLXN0aWNreScpO1xyXG5cdFx0XHQkKCdib2R5JykucmVtb3ZlQ2xhc3MoJ2lzLWZyb3plbicpO1xyXG5cdCAgICAgfVxyXG5cclxuXHQgICAgZnVuY3Rpb24gT3Blbk5hdmlnYXRpb25NZW51KCl7XHJcblx0XHRcdCQoJy5tYWluLW1lbnUnKS5hZGRDbGFzcygnaXMtYWN0aXZlJyk7XHJcblx0XHRcdCQoJy5tZW51LXRvZ2dsZXInKS5hZGRDbGFzcygnaXMtYWN0aXZlJyk7XHJcblx0XHRcdCQoJy5oZWFkZXJfX3dyYXBwZXIgLm1lbnUtdG9nZ2xlcicpLmFkZENsYXNzKCdpcy1zdGlja3knKTtcclxuXHRcdFx0JCgnYm9keScpLmFkZENsYXNzKCdpcy1mcm96ZW4nKTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIC8vIENyZWF0ZSBjb29raWVcclxuXHRcdGZ1bmN0aW9uIGNyZWF0ZUNvb2tpZShuYW1lLCB2YWx1ZSwgZGF5cykge1xyXG5cdFx0ICAgIHZhciBleHBpcmVzO1xyXG5cdFx0ICAgIGlmIChkYXlzKSB7XHJcblx0XHQgICAgICAgIHZhciBkYXRlID0gbmV3IERhdGUoKTtcclxuXHRcdCAgICAgICAgZGF0ZS5zZXRUaW1lKGRhdGUuZ2V0VGltZSgpKyhkYXlzKjI0KjYwKjYwKjEwMDApKTtcclxuXHRcdCAgICAgICAgZXhwaXJlcyA9IFwiOyBleHBpcmVzPVwiK2RhdGUudG9HTVRTdHJpbmcoKTtcclxuXHRcdCAgICB9XHJcblx0XHQgICAgZWxzZSB7XHJcblx0XHQgICAgICAgIGV4cGlyZXMgPSBcIlwiO1xyXG5cdFx0ICAgIH1cclxuXHRcdCAgICBkb2N1bWVudC5jb29raWUgPSBuYW1lK1wiPVwiK3ZhbHVlK2V4cGlyZXMrXCI7IHBhdGg9L1wiO1xyXG5cdFx0fVxyXG5cclxuXHRcdC8vIFJlYWQgY29va2llXHJcblx0XHRmdW5jdGlvbiByZWFkQ29va2llKG5hbWUpIHtcclxuXHRcdCAgICB2YXIgbmFtZUVRID0gbmFtZSArIFwiPVwiO1xyXG5cdFx0ICAgIHZhciBjYSA9IGRvY3VtZW50LmNvb2tpZS5zcGxpdCgnOycpO1xyXG5cdFx0ICAgIGZvcih2YXIgaT0wO2kgPCBjYS5sZW5ndGg7aSsrKSB7XHJcblx0XHQgICAgICAgIHZhciBjID0gY2FbaV07XHJcblx0XHQgICAgICAgIHdoaWxlIChjLmNoYXJBdCgwKSA9PT0gJyAnKSB7XHJcblx0XHQgICAgICAgICAgICBjID0gYy5zdWJzdHJpbmcoMSxjLmxlbmd0aCk7XHJcblx0XHQgICAgICAgIH1cclxuXHRcdCAgICAgICAgaWYgKGMuaW5kZXhPZihuYW1lRVEpID09PSAwKSB7XHJcblx0XHQgICAgICAgICAgICByZXR1cm4gYy5zdWJzdHJpbmcobmFtZUVRLmxlbmd0aCxjLmxlbmd0aCk7XHJcblx0XHQgICAgICAgIH1cclxuXHRcdCAgICB9XHJcblx0XHQgICAgcmV0dXJuIG51bGw7XHJcblx0XHR9XHJcblxyXG5cdFx0Ly8gRXJhc2UgY29va2llXHJcblx0XHRmdW5jdGlvbiBlcmFzZUNvb2tpZShuYW1lKSB7XHJcblx0XHQgICAgY3JlYXRlQ29va2llKG5hbWUsXCJcIiwtMSk7XHJcblx0XHR9XHJcblxyXG5cclxuICAgICAgICBpZigkKHdpbmRvdykud2lkdGgoKSA+PSAxMDI0KXtcclxuXHRcdFx0aWYoJCgnaW5wdXQubWVudS1vcGVuLWZpcnN0LXRpbWUtY2hlY2tlZCcpLnZhbCgpID09ICdUcnVlJyl7XHJcblx0XHRcdFx0aWYocmVhZENvb2tpZShcIm1lbnVuYXZpZ2F0aW9uY29va2llXCIpICE9PSBjb29raWVWYWx1ZSAmJiB3aW5kb3cubG9jYXRpb24ucGF0aG5hbWUgPT09ICcvJyl7XHJcblx0XHRcdFx0XHRPcGVuTmF2aWdhdGlvbk1lbnUoKTtcclxuXHRcdFx0XHR9ZWxzZXtcclxuXHRcdFx0XHRcdENsb3NlTmF2aWdhdGlvbk1lbnUoKTtcclxuXHRcdFx0XHR9XHJcblx0XHRcdFx0Y3JlYXRlQ29va2llKFwibWVudW5hdmlnYXRpb25jb29raWVcIiwgXCJ5ZXNcIiwgMzY1KTtcclxuXHRcdFx0fVxyXG5cdFx0XHRlbHNle1xyXG5cdFx0XHRcdGVyYXNlQ29va2llKFwibWVudW5hdmlnYXRpb25jb29raWVcIik7XHJcblx0XHRcdH1cclxuXHRcdFx0XHJcblx0XHR9ICAgICAgXHJcbn0pIiwiLyogZ2xvYmFsIGFuYWx5dGljc19kYXRhICovXHJcblxyXG5pbXBvcnQgeyBhbmFseXRpY3NFdmVudCB9IGZyb20gJy4vY29udHJvbGxlcnMvYW5hbHl0aWNzLWNvbnRyb2xsZXInO1xyXG5cclxuZnVuY3Rpb24gbmV3c2xldHRlclNpZ251cENvbnRyb2xsZXIoKSB7XHJcblxyXG4gICAgdGhpcy5jaGVja0ZvclVzZXJTaWduZWRVcCA9IGZ1bmN0aW9uKCl7XHJcbiAgICAgICAgJC5nZXQoJy9BY2NvdW50L2FwaS9QcmVmZXJlbmNlc0FwaS9Jc1VzZXJTaWduZWRVcCcsIGZ1bmN0aW9uKHJlc3BvbnNlKSB7XHJcbiAgICAgICAgICAgIHZhciByZXMgPSByZXNwb25zZTtcclxuICAgICAgICAgICAgaWYocmVzcG9uc2UpIHtcclxuICAgICAgICAgICAgICAgICQoXCIubmV3c2xldHRlci1zaWdudXBcIikuaGlkZSgpO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgIH0pO1xyXG4gICAgfTtcclxuXHJcbiAgICB0aGlzLklzVmFsaWRFbWFpbCA9IGZ1bmN0aW9uKGVtYWlsKXtcclxuICAgICAgICB2YXIgcmUgPSAvXigoW148PigpXFxbXFxdXFxcXC4sOzpcXHNAXCJdKyhcXC5bXjw+KClcXFtcXF1cXFxcLiw7Olxcc0BcIl0rKSopfChcIi4rXCIpKUAoKFxcW1swLTldezEsM31cXC5bMC05XXsxLDN9XFwuWzAtOV17MSwzfVxcLlswLTldezEsM31dKXwoKFthLXpBLVpcXC0wLTldK1xcLikrW2EtekEtWl17Mix9KSkkLztcclxuICAgICAgICByZXR1cm4gcmUudGVzdChlbWFpbCk7XHJcbiAgICAgICAgXHJcbiAgICB9O1xyXG4gICAgdGhpcy5hZGRDb250cm9sID0gZnVuY3Rpb24odHJpZ2dlckVsZW1lbnQsIHN1Y2Nlc3NDYWxsYmFjaywgZmFpbHVyZUNhbGxiYWNrKSB7XHJcbiAgICAgICAgaWYgKHRyaWdnZXJFbGVtZW50KSB7XHJcbiAgICAgICAgICAgICQodHJpZ2dlckVsZW1lbnQpLm9uKCdjbGljaycsIChldmVudCkgPT4ge1xyXG5cclxuICAgICAgICAgICAgICAgIC8vIFByZXZlbnQgZm9ybSBzdWJtaXRcclxuICAgICAgICAgICAgICAgIGV2ZW50LnByZXZlbnREZWZhdWx0KCk7XHJcblxyXG4gICAgICAgICAgICAgICAgLy8gSGlkZSBhbnkgZXJyb3JzXHJcbiAgICAgICAgICAgICAgICAkKCcuanMtbmV3c2xldHRlci1zaWdudXAtZXJyb3InKS5oaWRlKCk7XHJcblxyXG4gICAgICAgICAgICAgICAgdmFyIGlucHV0RGF0YSA9ICQoXCIjbmV3c2xldHRlclVzZXJOYW1lXCIpLnZhbCgpO1xyXG4gICAgICAgICAgICAgICAgdmFyIHVybCA9ICQodHJpZ2dlckVsZW1lbnQpLmRhdGEoJ3NpZ251cC11cmwnKTtcclxuXHJcbiAgICAgICAgICAgICAgICAvLyQodHJpZ2dlckVsZW1lbnQpLnBhcmVudHMoJy5uZXdzbGV0dGVyLXNpZ251cCcpLmZpbmQoJ2lucHV0JykuZWFjaChmdW5jdGlvbigpIHtcclxuICAgICAgICAgICAgICAgIC8vICAgIGlucHV0RGF0YSA9ICQodGhpcykudmFsKCk7XHJcbiAgICAgICAgICAgICAgICAvL30pO1xyXG5cclxuICAgICAgICAgICAgICAgIGlmKGlucHV0RGF0YSE9PScnICYmIHRoaXMuSXNWYWxpZEVtYWlsKGlucHV0RGF0YSkpe1xyXG4gICAgICAgICAgICAgICAgICAgICQoJy5qcy1uZXdzbGV0dGVyLXNpZ251cC0tZXJyb3ItaW52YWxpZGVtYWlsZm9ybWF0JykuaGlkZSgpO1xyXG4gICAgICAgICAgICAgICAgdXJsID0gdXJsICsgJz91c2VyTmFtZT0nICsgaW5wdXREYXRhO1xyXG5cclxuICAgICAgICAgICAgICAgICQuZ2V0KHVybCwgZnVuY3Rpb24ocmVzcG9uc2UpIHtcclxuICAgICAgICAgICAgICAgICAgICB2YXIgbmV3c2xldHRlckFuYWx5dGljcztcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKHJlc3BvbnNlID09ICd0cnVlJykge1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgbmV3c2xldHRlckFuYWx5dGljcyA9IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGV2ZW50X25hbWU6ICduZXdzbGV0dGVyLXNpZ251cCcsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBuZXdzbGV0dGVyX3NpZ251cF9zdGF0ZTogJ3N1Y2Nlc3NmdWwnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdXNlck5hbWU6ICdcIicgKyBpbnB1dERhdGEgKyAnXCInXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH07XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICBhbmFseXRpY3NFdmVudCggJC5leHRlbmQoIGFuYWx5dGljc19kYXRhLCBuZXdzbGV0dGVyQW5hbHl0aWNzKSApO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgJChcIi5uZXdzbGV0dGVyLXNpZ251cC1iZWZvcmUtc3VibWl0XCIpLmhpZGUoKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgJChcIi5uZXdzbGV0dGVyLXNpZ251cC1hZnRlci1zdWJtaXRcIikuc2hvdygpO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICB9IGVsc2UgaWYgKHJlc3BvbnNlID09ICdtdXN0cmVnaXN0ZXInKXtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIG5ld3NsZXR0ZXJBbmFseXRpY3MgPSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBldmVudF9uYW1lOiAnbmV3c2xldHRlci1zaWdudXAnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbmV3c2xldHRlcl9zaWdudXBfc3RhdGU6ICd1bnN1Y2Nlc3NmdWwnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdXNlck5hbWU6ICdcIicgKyBpbnB1dERhdGEgKyAnXCInXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH07XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICBhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIG5ld3NsZXR0ZXJBbmFseXRpY3MpICk7XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAkKCcubmV3c2xldHRlci1zaWdudXAtbmVlZHMtcmVnaXN0cmF0aW9uIGEnKS5hdHRyKCdocmVmJywgJCgnLm5ld3NsZXR0ZXItc2lnbnVwLW5lZWRzLXJlZ2lzdHJhdGlvbiBhJykuYXR0cignaHJlZicpICsgJCgnLm5ld3NsZXR0ZXItc2lnbnVwLWJlZm9yZS1zdWJtaXQgaW5wdXQnKS52YWwoKSlcclxuICAgICAgICAgICAgICAgICAgICAgICAgJCgnLm5ld3NsZXR0ZXItc2lnbnVwLWJlZm9yZS1zdWJtaXQnKS5oaWRlKCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICQoJy5uZXdzbGV0dGVyLXNpZ251cC1uZWVkcy1yZWdpc3RyYXRpb24nKS5zaG93KCk7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgIGVsc2VcclxuICAgICAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIG5ld3NsZXR0ZXJBbmFseXRpY3MgPSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBldmVudF9uYW1lOiAnbmV3c2xldHRlci1zaWdudXAnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbmV3c2xldHRlcl9zaWdudXBfc3RhdGU6ICd1bnN1Y2Nlc3NmdWwnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdXNlck5hbWU6ICdcIicgKyBpbnB1dERhdGEgKyAnXCInXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH07XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICBhbmFseXRpY3NFdmVudCggJC5leHRlbmQoYW5hbHl0aWNzX2RhdGEsIG5ld3NsZXR0ZXJBbmFseXRpY3MpICk7XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAkKCcuanMtbmV3c2xldHRlci1zaWdudXAtZXJyb3InKS5zaG93KCk7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICBlbHNle1xyXG4gICAgICAgICAgICAgICAgICAgICQoJy5qcy1uZXdzbGV0dGVyLXNpZ251cC0tZXJyb3ItaW52YWxpZGVtYWlsZm9ybWF0Jykuc2hvdygpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9KTtcclxuICAgICAgICB9XHJcbiAgICB9O1xyXG59XHJcblxyXG5leHBvcnQgZGVmYXVsdCBuZXdzbGV0dGVyU2lnbnVwQ29udHJvbGxlcjtcclxuIiwidmFyIFNlYXJjaFNjcmlwdCA9IGZ1bmN0aW9uKCkge1xyXG5cclxuXHRcdFx0LyogVG9nZ2xlIHNlYXJjaCB0aXBzIHZpc2liaWxpdHkgKi9cclxuXHRcdFx0JCgnLmpzLXRvZ2dsZS1zZWFyY2gtdGlwcycpLm9uKCdjbGljaycsIGZ1bmN0aW9uIHRvZ2dsZVRpcHMoKSB7XHJcblx0XHRcdFx0JCgnLnNlYXJjaC1iYXJfX3RpcHMnKS50b2dnbGVDbGFzcygnb3BlbicpO1xyXG5cdFx0XHRcdCQoJy5zZWFyY2gtYmFyJykudG9nZ2xlQ2xhc3MoJ3RpcHMtb3BlbicpO1xyXG5cdFx0XHR9KTtcclxuXHJcbn0oKTtcclxuIiwiLyoqXHJcbiAqIEBsaWNlbnNlXHJcbiAqIFNlbGVjdGl2aXR5LmpzIDIuMS4wIDxodHRwczovL2FyZW5kanIuZ2l0aHViLmlvL3NlbGVjdGl2aXR5Lz5cclxuICogQ29weXJpZ2h0IChjKSAyMDE0LTIwMTYgQXJlbmQgdmFuIEJlZWxlbiBqci5cclxuICogICAgICAgICAgIChjKSAyMDE2IFNwZWFrYXAgQlZcclxuICogQXZhaWxhYmxlIHVuZGVyIE1JVCBsaWNlbnNlIDxodHRwczovL2dpdGh1Yi5jb20vYXJlbmRqci9zZWxlY3Rpdml0eS9ibG9iL21hc3Rlci9MSUNFTlNFPlxyXG4gKi9cclxuIWZ1bmN0aW9uKGUpe2lmKFwib2JqZWN0XCI9PXR5cGVvZiBleHBvcnRzJiZcInVuZGVmaW5lZFwiIT10eXBlb2YgbW9kdWxlKW1vZHVsZS5leHBvcnRzPWUoKTtlbHNlIGlmKFwiZnVuY3Rpb25cIj09dHlwZW9mIGRlZmluZSYmZGVmaW5lLmFtZClkZWZpbmUoW10sZSk7ZWxzZXt2YXIgZjtcInVuZGVmaW5lZFwiIT10eXBlb2Ygd2luZG93P2Y9d2luZG93OlwidW5kZWZpbmVkXCIhPXR5cGVvZiBnbG9iYWw/Zj1nbG9iYWw6XCJ1bmRlZmluZWRcIiE9dHlwZW9mIHNlbGYmJihmPXNlbGYpLGYuc2VsZWN0aXZpdHk9ZSgpfX0oZnVuY3Rpb24oKXt2YXIgZGVmaW5lLG1vZHVsZSxleHBvcnRzO3JldHVybiAoZnVuY3Rpb24gZSh0LG4scil7ZnVuY3Rpb24gcyhvLHUpe2lmKCFuW29dKXtpZighdFtvXSl7dmFyIGE9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtpZighdSYmYSlyZXR1cm4gYShvLCEwKTtpZihpKXJldHVybiBpKG8sITApO3ZhciBmPW5ldyBFcnJvcihcIkNhbm5vdCBmaW5kIG1vZHVsZSAnXCIrbytcIidcIik7dGhyb3cgZi5jb2RlPVwiTU9EVUxFX05PVF9GT1VORFwiLGZ9dmFyIGw9bltvXT17ZXhwb3J0czp7fX07dFtvXVswXS5jYWxsKGwuZXhwb3J0cyxmdW5jdGlvbihlKXt2YXIgbj10W29dWzFdW2VdO3JldHVybiBzKG4/bjplKX0sbCxsLmV4cG9ydHMsZSx0LG4scil9cmV0dXJuIG5bb10uZXhwb3J0c312YXIgaT10eXBlb2YgcmVxdWlyZT09XCJmdW5jdGlvblwiJiZyZXF1aXJlO2Zvcih2YXIgbz0wO288ci5sZW5ndGg7bysrKXMocltvXSk7cmV0dXJuIHN9KSh7MTpbZnVuY3Rpb24oX2RlcmVxXyxtb2R1bGUsZXhwb3J0cyl7XHJcbl9kZXJlcV8oNSk7X2RlcmVxXyg2KTtfZGVyZXFfKDcpO19kZXJlcV8oOSk7X2RlcmVxXygxMCk7X2RlcmVxXygxMSk7X2RlcmVxXygxMik7X2RlcmVxXygxMyk7X2RlcmVxXygxNCk7X2RlcmVxXygxNSk7X2RlcmVxXygxNik7X2RlcmVxXygxNyk7X2RlcmVxXygxOCk7X2RlcmVxXygxOSk7bW9kdWxlLmV4cG9ydHM9X2RlcmVxXyg4KTtcclxufSx7XCIxMFwiOjEwLFwiMTFcIjoxMSxcIjEyXCI6MTIsXCIxM1wiOjEzLFwiMTRcIjoxNCxcIjE1XCI6MTUsXCIxNlwiOjE2LFwiMTdcIjoxNyxcIjE4XCI6MTgsXCIxOVwiOjE5LFwiNVwiOjUsXCI2XCI6NixcIjdcIjo3LFwiOFwiOjgsXCI5XCI6OX1dLDI6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG52YXIgJCA9IHdpbmRvdy5qUXVlcnkgfHwgd2luZG93LlplcHRvO1xyXG5cclxuLyoqXHJcbiAqIEV2ZW50IERlbGVnYXRvciBDb25zdHJ1Y3Rvci5cclxuICovXHJcbmZ1bmN0aW9uIEV2ZW50RGVsZWdhdG9yKCkge1xyXG5cclxuICAgIHRoaXMuX2V2ZW50cyA9IFtdO1xyXG5cclxuICAgIHRoaXMuZGVsZWdhdGVFdmVudHMoKTtcclxufVxyXG5cclxuLyoqXHJcbiAqIE1ldGhvZHMuXHJcbiAqL1xyXG4kLmV4dGVuZChFdmVudERlbGVnYXRvci5wcm90b3R5cGUsIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEF0dGFjaGVzIGFsbCBsaXN0ZW5lcnMgZnJvbSB0aGUgZXZlbnRzIG1hcCB0byB0aGUgaW5zdGFuY2UncyBlbGVtZW50LlxyXG4gICAgICpcclxuICAgICAqIE5vcm1hbGx5LCB5b3Ugc2hvdWxkIG5vdCBoYXZlIHRvIGNhbGwgdGhpcyBtZXRob2QgeW91cnNlbGYgYXMgaXQncyBjYWxsZWQgYXV0b21hdGljYWxseSBpblxyXG4gICAgICogdGhlIGNvbnN0cnVjdG9yLlxyXG4gICAgICovXHJcbiAgICBkZWxlZ2F0ZUV2ZW50czogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHRoaXMudW5kZWxlZ2F0ZUV2ZW50cygpO1xyXG5cclxuICAgICAgICAkLmVhY2godGhpcy5ldmVudHMsIGZ1bmN0aW9uKGV2ZW50LCBsaXN0ZW5lcikge1xyXG4gICAgICAgICAgICB2YXIgc2VsZWN0b3IsIGluZGV4ID0gZXZlbnQuaW5kZXhPZignICcpO1xyXG4gICAgICAgICAgICBpZiAoaW5kZXggPiAtMSkge1xyXG4gICAgICAgICAgICAgICAgc2VsZWN0b3IgPSBldmVudC5zbGljZShpbmRleCArIDEpO1xyXG4gICAgICAgICAgICAgICAgZXZlbnQgPSBldmVudC5zbGljZSgwLCBpbmRleCk7XHJcbiAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgIGlmICgkLnR5cGUobGlzdGVuZXIpID09PSAnc3RyaW5nJykge1xyXG4gICAgICAgICAgICAgICAgbGlzdGVuZXIgPSB0aGlzW2xpc3RlbmVyXTtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgbGlzdGVuZXIgPSBsaXN0ZW5lci5iaW5kKHRoaXMpO1xyXG5cclxuICAgICAgICAgICAgaWYgKHNlbGVjdG9yKSB7XHJcbiAgICAgICAgICAgICAgICB0aGlzLiRlbC5vbihldmVudCwgc2VsZWN0b3IsIGxpc3RlbmVyKTtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuJGVsLm9uKGV2ZW50LCBsaXN0ZW5lcik7XHJcbiAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgIHRoaXMuX2V2ZW50cy5wdXNoKHsgZXZlbnQ6IGV2ZW50LCBzZWxlY3Rvcjogc2VsZWN0b3IsIGxpc3RlbmVyOiBsaXN0ZW5lciB9KTtcclxuICAgICAgICB9LmJpbmQodGhpcykpO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIERldGFjaGVzIGFsbCBsaXN0ZW5lcnMgZnJvbSB0aGUgZXZlbnRzIG1hcCBmcm9tIHRoZSBpbnN0YW5jZSdzIGVsZW1lbnQuXHJcbiAgICAgKi9cclxuICAgIHVuZGVsZWdhdGVFdmVudHM6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB0aGlzLl9ldmVudHMuZm9yRWFjaChmdW5jdGlvbihldmVudCkge1xyXG4gICAgICAgICAgICBpZiAoZXZlbnQuc2VsZWN0b3IpIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuJGVsLm9mZihldmVudC5ldmVudCwgZXZlbnQuc2VsZWN0b3IsIGV2ZW50Lmxpc3RlbmVyKTtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuJGVsLm9mZihldmVudC5ldmVudCwgZXZlbnQubGlzdGVuZXIpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSwgdGhpcyk7XHJcblxyXG4gICAgICAgIHRoaXMuX2V2ZW50cyA9IFtdO1xyXG4gICAgfVxyXG5cclxufSk7XHJcblxyXG5tb2R1bGUuZXhwb3J0cyA9IEV2ZW50RGVsZWdhdG9yO1xyXG5cclxufSx7XCJqcXVlcnlcIjpcImpxdWVyeVwifV0sMzpbZnVuY3Rpb24oX2RlcmVxXyxtb2R1bGUsZXhwb3J0cyl7XHJcbid1c2Ugc3RyaWN0JztcclxuXHJcbi8qKlxyXG4gKiBAbGljZW5zZVxyXG4gKiBsb2Rhc2ggMy4zLjEgKEN1c3RvbSBCdWlsZCkgPGh0dHBzOi8vbG9kYXNoLmNvbS8+XHJcbiAqIENvcHlyaWdodCAyMDEyLTIwMTUgVGhlIERvam8gRm91bmRhdGlvbiA8aHR0cDovL2Rvam9mb3VuZGF0aW9uLm9yZy8+XHJcbiAqIEJhc2VkIG9uIFVuZGVyc2NvcmUuanMgMS44LjIgPGh0dHA6Ly91bmRlcnNjb3JlanMub3JnL0xJQ0VOU0U+XHJcbiAqIENvcHlyaWdodCAyMDA5LTIwMTUgSmVyZW15IEFzaGtlbmFzLCBEb2N1bWVudENsb3VkIGFuZCBJbnZlc3RpZ2F0aXZlIFJlcG9ydGVycyAmIEVkaXRvcnNcclxuICogQXZhaWxhYmxlIHVuZGVyIE1JVCBsaWNlbnNlIDxodHRwczovL2xvZGFzaC5jb20vbGljZW5zZT5cclxuICovXHJcblxyXG4vKipcclxuICogR2V0cyB0aGUgbnVtYmVyIG9mIG1pbGxpc2Vjb25kcyB0aGF0IGhhdmUgZWxhcHNlZCBzaW5jZSB0aGUgVW5peCBlcG9jaFxyXG4gKiAgKDEgSmFudWFyeSAxOTcwIDAwOjAwOjAwIFVUQykuXHJcbiAqXHJcbiAqIEBzdGF0aWNcclxuICogQG1lbWJlck9mIF9cclxuICogQGNhdGVnb3J5IERhdGVcclxuICogQGV4YW1wbGVcclxuICpcclxuICogXy5kZWZlcihmdW5jdGlvbihzdGFtcCkge1xyXG4gKiAgIGNvbnNvbGUubG9nKF8ubm93KCkgLSBzdGFtcCk7XHJcbiAqIH0sIF8ubm93KCkpO1xyXG4gKiAvLyA9PiBsb2dzIHRoZSBudW1iZXIgb2YgbWlsbGlzZWNvbmRzIGl0IHRvb2sgZm9yIHRoZSBkZWZlcnJlZCBmdW5jdGlvbiB0byBiZSBpbnZva2VkXHJcbiAqL1xyXG52YXIgbm93ID0gRGF0ZS5ub3c7XHJcblxyXG4vKipcclxuICogQ3JlYXRlcyBhIGZ1bmN0aW9uIHRoYXQgZGVsYXlzIGludm9raW5nIGBmdW5jYCB1bnRpbCBhZnRlciBgd2FpdGAgbWlsbGlzZWNvbmRzXHJcbiAqIGhhdmUgZWxhcHNlZCBzaW5jZSB0aGUgbGFzdCB0aW1lIGl0IHdhcyBpbnZva2VkLlxyXG4gKlxyXG4gKiBTZWUgW0RhdmlkIENvcmJhY2hvJ3MgYXJ0aWNsZV1cclxuICogICAgICAgICAgICAgICAgICAgICAgICAoaHR0cDovL2RydXBhbG1vdGlvbi5jb20vYXJ0aWNsZS9kZWJvdW5jZS1hbmQtdGhyb3R0bGUtdmlzdWFsLWV4cGxhbmF0aW9uKVxyXG4gKiBmb3IgZGV0YWlscyBvdmVyIHRoZSBkaWZmZXJlbmNlcyBiZXR3ZWVuIGBfLmRlYm91bmNlYCBhbmQgYF8udGhyb3R0bGVgLlxyXG4gKlxyXG4gKiBAc3RhdGljXHJcbiAqIEBtZW1iZXJPZiBfXHJcbiAqIEBjYXRlZ29yeSBGdW5jdGlvblxyXG4gKiBAcGFyYW0ge0Z1bmN0aW9ufSBmdW5jIFRoZSBmdW5jdGlvbiB0byBkZWJvdW5jZS5cclxuICogQHBhcmFtIHtudW1iZXJ9IFt3YWl0PTBdIFRoZSBudW1iZXIgb2YgbWlsbGlzZWNvbmRzIHRvIGRlbGF5LlxyXG4gKiBAcmV0dXJucyB7RnVuY3Rpb259IFJldHVybnMgdGhlIG5ldyBkZWJvdW5jZWQgZnVuY3Rpb24uXHJcbiAqIEBleGFtcGxlXHJcbiAqXHJcbiAqIC8vIGF2b2lkIGNvc3RseSBjYWxjdWxhdGlvbnMgd2hpbGUgdGhlIHdpbmRvdyBzaXplIGlzIGluIGZsdXhcclxuICogalF1ZXJ5KHdpbmRvdykub24oJ3Jlc2l6ZScsIF8uZGVib3VuY2UoY2FsY3VsYXRlTGF5b3V0LCAxNTApKTtcclxuICovXHJcbmZ1bmN0aW9uIGRlYm91bmNlKGZ1bmMsIHdhaXQpIHtcclxuICAgIHZhciBhcmdzLFxyXG4gICAgICAgIHJlc3VsdCxcclxuICAgICAgICBzdGFtcCxcclxuICAgICAgICB0aW1lb3V0SWQsXHJcbiAgICAgICAgdHJhaWxpbmdDYWxsLFxyXG4gICAgICAgIGxhc3RDYWxsZWQgPSAwO1xyXG5cclxuICAgIHdhaXQgPSB3YWl0IDwgMCA/IDAgOiAoK3dhaXQgfHwgMCk7XHJcblxyXG4gICAgZnVuY3Rpb24gZGVsYXllZCgpIHtcclxuICAgICAgICB2YXIgcmVtYWluaW5nID0gd2FpdCAtIChub3coKSAtIHN0YW1wKTtcclxuICAgICAgICBpZiAocmVtYWluaW5nIDw9IDAgfHwgcmVtYWluaW5nID4gd2FpdCkge1xyXG4gICAgICAgICAgICB2YXIgaXNDYWxsZWQgPSB0cmFpbGluZ0NhbGw7XHJcbiAgICAgICAgICAgIHRpbWVvdXRJZCA9IHRyYWlsaW5nQ2FsbCA9IHVuZGVmaW5lZDtcclxuICAgICAgICAgICAgaWYgKGlzQ2FsbGVkKSB7XHJcbiAgICAgICAgICAgICAgICBsYXN0Q2FsbGVkID0gbm93KCk7XHJcbiAgICAgICAgICAgICAgICByZXN1bHQgPSBmdW5jLmFwcGx5KG51bGwsIGFyZ3MpO1xyXG4gICAgICAgICAgICAgICAgaWYgKCF0aW1lb3V0SWQpIHtcclxuICAgICAgICAgICAgICAgICAgICBhcmdzID0gbnVsbDtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRpbWVvdXRJZCA9IHNldFRpbWVvdXQoZGVsYXllZCwgcmVtYWluaW5nKTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gZGVib3VuY2VkKCkge1xyXG4gICAgICAgIGFyZ3MgPSBhcmd1bWVudHM7XHJcbiAgICAgICAgc3RhbXAgPSBub3coKTtcclxuICAgICAgICB0cmFpbGluZ0NhbGwgPSB0cnVlO1xyXG5cclxuICAgICAgICBpZiAoIXRpbWVvdXRJZCkge1xyXG4gICAgICAgICAgICB0aW1lb3V0SWQgPSBzZXRUaW1lb3V0KGRlbGF5ZWQsIHdhaXQpO1xyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gcmVzdWx0O1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIGRlYm91bmNlZDtcclxufVxyXG5cclxubW9kdWxlLmV4cG9ydHMgPSBkZWJvdW5jZTtcclxuXHJcbn0se31dLDQ6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG4vKipcclxuICogQGxpY2Vuc2VcclxuICogTG8tRGFzaCAyLjQuMSAoQ3VzdG9tIEJ1aWxkKSA8aHR0cDovL2xvZGFzaC5jb20vPlxyXG4gKiBDb3B5cmlnaHQgMjAxMi0yMDEzIFRoZSBEb2pvIEZvdW5kYXRpb24gPGh0dHA6Ly9kb2pvZm91bmRhdGlvbi5vcmcvPlxyXG4gKiBCYXNlZCBvbiBVbmRlcnNjb3JlLmpzIDEuNS4yIDxodHRwOi8vdW5kZXJzY29yZWpzLm9yZy9MSUNFTlNFPlxyXG4gKiBDb3B5cmlnaHQgMjAwOS0yMDEzIEplcmVteSBBc2hrZW5hcywgRG9jdW1lbnRDbG91ZCBhbmQgSW52ZXN0aWdhdGl2ZSBSZXBvcnRlcnMgJiBFZGl0b3JzXHJcbiAqIEF2YWlsYWJsZSB1bmRlciBNSVQgbGljZW5zZSA8aHR0cDovL2xvZGFzaC5jb20vbGljZW5zZT5cclxuICovXHJcblxyXG52YXIgaHRtbEVzY2FwZXMgPSB7XHJcbiAgICAnJic6ICcmYW1wOycsXHJcbiAgICAnPCc6ICcmbHQ7JyxcclxuICAgICc+JzogJyZndDsnLFxyXG4gICAgJ1wiJzogJyZxdW90OycsXHJcbiAgICBcIidcIjogJyYjMzk7J1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFVzZWQgYnkgYGVzY2FwZWAgdG8gY29udmVydCBjaGFyYWN0ZXJzIHRvIEhUTUwgZW50aXRpZXMuXHJcbiAqXHJcbiAqIEBwcml2YXRlXHJcbiAqIEBwYXJhbSB7c3RyaW5nfSBtYXRjaCBUaGUgbWF0Y2hlZCBjaGFyYWN0ZXIgdG8gZXNjYXBlLlxyXG4gKiBAcmV0dXJucyB7c3RyaW5nfSBSZXR1cm5zIHRoZSBlc2NhcGVkIGNoYXJhY3Rlci5cclxuICovXHJcbmZ1bmN0aW9uIGVzY2FwZUh0bWxDaGFyKG1hdGNoKSB7XHJcbiAgICByZXR1cm4gaHRtbEVzY2FwZXNbbWF0Y2hdO1xyXG59XHJcblxyXG52YXIgcmVVbmVzY2FwZWRIdG1sID0gbmV3IFJlZ0V4cCgnWycgKyBPYmplY3Qua2V5cyhodG1sRXNjYXBlcykuam9pbignJykgKyAnXScsICdnJyk7XHJcblxyXG4vKipcclxuICogQ29udmVydHMgdGhlIGNoYXJhY3RlcnMgYCZgLCBgPGAsIGA+YCwgYFwiYCwgYW5kIGAnYCBpbiBgc3RyaW5nYCB0byB0aGVpclxyXG4gKiBjb3JyZXNwb25kaW5nIEhUTUwgZW50aXRpZXMuXHJcbiAqXHJcbiAqIEBzdGF0aWNcclxuICogQG1lbWJlck9mIF9cclxuICogQGNhdGVnb3J5IFV0aWxpdGllc1xyXG4gKiBAcGFyYW0ge3N0cmluZ30gc3RyaW5nIFRoZSBzdHJpbmcgdG8gZXNjYXBlLlxyXG4gKiBAcmV0dXJucyB7c3RyaW5nfSBSZXR1cm5zIHRoZSBlc2NhcGVkIHN0cmluZy5cclxuICogQGV4YW1wbGVcclxuICpcclxuICogXy5lc2NhcGUoJ0ZyZWQsIFdpbG1hLCAmIFBlYmJsZXMnKTtcclxuICogLy8gPT4gJ0ZyZWQsIFdpbG1hLCAmYW1wOyBQZWJibGVzJ1xyXG4gKi9cclxuZnVuY3Rpb24gZXNjYXBlKHN0cmluZykge1xyXG4gICAgcmV0dXJuIHN0cmluZyA/IFN0cmluZyhzdHJpbmcpLnJlcGxhY2UocmVVbmVzY2FwZWRIdG1sLCBlc2NhcGVIdG1sQ2hhcikgOiAnJztcclxufVxyXG5cclxubW9kdWxlLmV4cG9ydHMgPSBlc2NhcGU7XHJcblxyXG59LHt9XSw1OltmdW5jdGlvbihfZGVyZXFfLG1vZHVsZSxleHBvcnRzKXtcclxuJ3VzZSBzdHJpY3QnO1xyXG5cclxudmFyICQgPSB3aW5kb3cualF1ZXJ5IHx8IHdpbmRvdy5aZXB0bztcclxuXHJcbnZhciBkZWJvdW5jZSA9IF9kZXJlcV8oMyk7XHJcblxyXG52YXIgU2VsZWN0aXZpdHkgPSBfZGVyZXFfKDgpO1xyXG5cclxuX2RlcmVxXygxMyk7XHJcblxyXG4vKipcclxuICogT3B0aW9uIGxpc3RlbmVyIHRoYXQgaW1wbGVtZW50cyBhIGNvbnZlbmllbmNlIHF1ZXJ5IGZ1bmN0aW9uIGZvciBwZXJmb3JtaW5nIEFKQVggcmVxdWVzdHMuXHJcbiAqL1xyXG5TZWxlY3Rpdml0eS5PcHRpb25MaXN0ZW5lcnMudW5zaGlmdChmdW5jdGlvbihzZWxlY3Rpdml0eSwgb3B0aW9ucykge1xyXG5cclxuICAgIHZhciBhamF4ID0gb3B0aW9ucy5hamF4O1xyXG4gICAgaWYgKGFqYXggJiYgYWpheC51cmwpIHtcclxuICAgICAgICB2YXIgZm9ybWF0RXJyb3IgPSBhamF4LmZvcm1hdEVycm9yIHx8IFNlbGVjdGl2aXR5LkxvY2FsZS5hamF4RXJyb3I7XHJcbiAgICAgICAgdmFyIG1pbmltdW1JbnB1dExlbmd0aCA9IGFqYXgubWluaW11bUlucHV0TGVuZ3RoIHx8IDA7XHJcbiAgICAgICAgdmFyIHBhcmFtcyA9IGFqYXgucGFyYW1zO1xyXG4gICAgICAgIHZhciBwcm9jZXNzSXRlbSA9IGFqYXgucHJvY2Vzc0l0ZW0gfHwgZnVuY3Rpb24oaXRlbSkgeyByZXR1cm4gaXRlbTsgfTtcclxuICAgICAgICB2YXIgcXVpZXRNaWxsaXMgPSBhamF4LnF1aWV0TWlsbGlzIHx8IDA7XHJcbiAgICAgICAgdmFyIHJlc3VsdHNDYiA9IGFqYXgucmVzdWx0cyB8fCBmdW5jdGlvbihkYXRhKSB7IHJldHVybiB7IHJlc3VsdHM6IGRhdGEsIG1vcmU6IGZhbHNlIH07IH07XHJcbiAgICAgICAgdmFyIHRyYW5zcG9ydCA9IGFqYXgudHJhbnNwb3J0IHx8ICQuYWpheDtcclxuXHJcbiAgICAgICAgaWYgKHF1aWV0TWlsbGlzKSB7XHJcbiAgICAgICAgICAgIHRyYW5zcG9ydCA9IGRlYm91bmNlKHRyYW5zcG9ydCwgcXVpZXRNaWxsaXMpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgb3B0aW9ucy5xdWVyeSA9IGZ1bmN0aW9uKHF1ZXJ5T3B0aW9ucykge1xyXG4gICAgICAgICAgICB2YXIgb2Zmc2V0ID0gcXVlcnlPcHRpb25zLm9mZnNldDtcclxuICAgICAgICAgICAgdmFyIHRlcm0gPSBxdWVyeU9wdGlvbnMudGVybTtcclxuICAgICAgICAgICAgaWYgKHRlcm0ubGVuZ3RoIDwgbWluaW11bUlucHV0TGVuZ3RoKSB7XHJcbiAgICAgICAgICAgICAgICBxdWVyeU9wdGlvbnMuZXJyb3IoXHJcbiAgICAgICAgICAgICAgICAgICAgU2VsZWN0aXZpdHkuTG9jYWxlLm5lZWRNb3JlQ2hhcmFjdGVycyhtaW5pbXVtSW5wdXRMZW5ndGggLSB0ZXJtLmxlbmd0aClcclxuICAgICAgICAgICAgICAgICk7XHJcbiAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgdXJsID0gKGFqYXgudXJsIGluc3RhbmNlb2YgRnVuY3Rpb24gPyBhamF4LnVybChxdWVyeU9wdGlvbnMpIDogYWpheC51cmwpO1xyXG4gICAgICAgICAgICAgICAgaWYgKHBhcmFtcykge1xyXG4gICAgICAgICAgICAgICAgICAgIHVybCArPSAodXJsLmluZGV4T2YoJz8nKSA+IC0xID8gJyYnIDogJz8nKSArICQucGFyYW0ocGFyYW1zKHRlcm0sIG9mZnNldCkpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgICAgIHZhciBzdWNjZXNzID0gYWpheC5zdWNjZXNzO1xyXG4gICAgICAgICAgICAgICAgdmFyIGVycm9yID0gYWpheC5lcnJvcjtcclxuXHJcbiAgICAgICAgICAgICAgICB0cmFuc3BvcnQoJC5leHRlbmQoe30sIGFqYXgsIHtcclxuICAgICAgICAgICAgICAgICAgICB1cmw6IHVybCxcclxuICAgICAgICAgICAgICAgICAgICBzdWNjZXNzOiBmdW5jdGlvbihkYXRhLCB0ZXh0U3RhdHVzLCBqcVhIUikge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoc3VjY2Vzcykge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgc3VjY2VzcyhkYXRhLCB0ZXh0U3RhdHVzLCBqcVhIUik7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciByZXN1bHRzID0gcmVzdWx0c0NiKGRhdGEsIG9mZnNldCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJlc3VsdHMucmVzdWx0cyA9IHJlc3VsdHMucmVzdWx0cy5tYXAocHJvY2Vzc0l0ZW0pO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBxdWVyeU9wdGlvbnMuY2FsbGJhY2socmVzdWx0cyk7XHJcbiAgICAgICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgICAgICBlcnJvcjogZnVuY3Rpb24oanFYSFIsIHRleHRTdGF0dXMsIGVycm9yVGhyb3duKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmIChlcnJvcikge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgZXJyb3IoanFYSFIsIHRleHRTdGF0dXMsIGVycm9yVGhyb3duKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgcXVlcnlPcHRpb25zLmVycm9yKFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgZm9ybWF0RXJyb3IodGVybSwganFYSFIsIHRleHRTdGF0dXMsIGVycm9yVGhyb3duKSxcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHsgZXNjYXBlOiBmYWxzZSB9XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICk7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgfSkpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfTtcclxuICAgIH1cclxufSk7XHJcblxyXG59LHtcIjEzXCI6MTMsXCIzXCI6MyxcIjhcIjo4LFwianF1ZXJ5XCI6XCJqcXVlcnlcIn1dLDY6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG52YXIgU2VsZWN0aXZpdHkgPSBfZGVyZXFfKDgpO1xyXG5cclxudmFyIGxhdGVzdFF1ZXJ5TnVtID0gMDtcclxuXHJcbi8qKlxyXG4gKiBPcHRpb24gbGlzdGVuZXIgdGhhdCB3aWxsIGRpc2NhcmQgYW55IGNhbGxiYWNrcyBmcm9tIHRoZSBxdWVyeSBmdW5jdGlvbiBpZiBhbm90aGVyIHF1ZXJ5IGhhc1xyXG4gKiBiZWVuIGNhbGxlZCBhZnRlcndhcmRzLiBUaGlzIHByZXZlbnRzIHJlc3BvbnNlcyBmcm9tIHJlbW90ZSBzb3VyY2VzIGFycml2aW5nIG91dC1vZi1vcmRlci5cclxuICovXHJcblNlbGVjdGl2aXR5Lk9wdGlvbkxpc3RlbmVycy5wdXNoKGZ1bmN0aW9uKHNlbGVjdGl2aXR5LCBvcHRpb25zKSB7XHJcblxyXG4gICAgdmFyIHF1ZXJ5ID0gb3B0aW9ucy5xdWVyeTtcclxuICAgIGlmIChxdWVyeSAmJiAhcXVlcnkuX2FzeW5jKSB7XHJcbiAgICAgICAgb3B0aW9ucy5xdWVyeSA9IGZ1bmN0aW9uKHF1ZXJ5T3B0aW9ucykge1xyXG4gICAgICAgICAgICBsYXRlc3RRdWVyeU51bSsrO1xyXG4gICAgICAgICAgICB2YXIgcXVlcnlOdW0gPSBsYXRlc3RRdWVyeU51bTtcclxuXHJcbiAgICAgICAgICAgIHZhciBjYWxsYmFjayA9IHF1ZXJ5T3B0aW9ucy5jYWxsYmFjaztcclxuICAgICAgICAgICAgdmFyIGVycm9yID0gcXVlcnlPcHRpb25zLmVycm9yO1xyXG4gICAgICAgICAgICBxdWVyeU9wdGlvbnMuY2FsbGJhY2sgPSBmdW5jdGlvbigpIHtcclxuICAgICAgICAgICAgICAgIGlmIChxdWVyeU51bSA9PT0gbGF0ZXN0UXVlcnlOdW0pIHtcclxuICAgICAgICAgICAgICAgICAgICBjYWxsYmFjay5hcHBseShudWxsLCBhcmd1bWVudHMpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9O1xyXG4gICAgICAgICAgICBxdWVyeU9wdGlvbnMuZXJyb3IgPSBmdW5jdGlvbigpIHtcclxuICAgICAgICAgICAgICAgIGlmIChxdWVyeU51bSA9PT0gbGF0ZXN0UXVlcnlOdW0pIHtcclxuICAgICAgICAgICAgICAgICAgICBlcnJvci5hcHBseShudWxsLCBhcmd1bWVudHMpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9O1xyXG4gICAgICAgICAgICBxdWVyeShxdWVyeU9wdGlvbnMpO1xyXG4gICAgICAgIH07XHJcbiAgICAgICAgb3B0aW9ucy5xdWVyeS5fYXN5bmMgPSB0cnVlO1xyXG4gICAgfVxyXG59KTtcclxuXHJcbn0se1wiOFwiOjh9XSw3OltmdW5jdGlvbihfZGVyZXFfLG1vZHVsZSxleHBvcnRzKXtcclxuJ3VzZSBzdHJpY3QnO1xyXG5cclxudmFyICQgPSB3aW5kb3cualF1ZXJ5IHx8IHdpbmRvdy5aZXB0bztcclxuXHJcbnZhciBTZWxlY3Rpdml0eURyb3Bkb3duID0gX2RlcmVxXygxMCk7XHJcblxyXG4vKipcclxuICogTWV0aG9kcy5cclxuICovXHJcbiQuZXh0ZW5kKFNlbGVjdGl2aXR5RHJvcGRvd24ucHJvdG90eXBlLCB7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAaW5oZXJpdFxyXG4gICAgICovXHJcbiAgICByZW1vdmVDbG9zZUhhbmRsZXI6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICBpZiAodGhpcy5fJGJhY2tkcm9wICYmICF0aGlzLnBhcmVudE1lbnUpIHtcclxuICAgICAgICAgICAgdGhpcy5fJGJhY2tkcm9wLnJlbW92ZSgpO1xyXG4gICAgICAgICAgICB0aGlzLl8kYmFja2Ryb3AgPSBudWxsO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAaW5oZXJpdFxyXG4gICAgICovXHJcbiAgICBzZXR1cENsb3NlSGFuZGxlcjogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHZhciAkYmFja2Ryb3A7XHJcbiAgICAgICAgaWYgKHRoaXMucGFyZW50TWVudSkge1xyXG4gICAgICAgICAgICAkYmFja2Ryb3AgPSB0aGlzLnBhcmVudE1lbnUuXyRiYWNrZHJvcDtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAkYmFja2Ryb3AgPSAkKCc8ZGl2PicpLmFkZENsYXNzKCdzZWxlY3Rpdml0eS1iYWNrZHJvcCcpO1xyXG5cclxuICAgICAgICAgICAgJCgnYm9keScpLmFwcGVuZCgkYmFja2Ryb3ApO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgJGJhY2tkcm9wLm9uKCdjbGljaycsIHRoaXMuY2xvc2UuYmluZCh0aGlzKSk7XHJcblxyXG4gICAgICAgIHRoaXMuXyRiYWNrZHJvcCA9ICRiYWNrZHJvcDtcclxuICAgIH1cclxuXHJcbn0pO1xyXG5cclxufSx7XCIxMFwiOjEwLFwianF1ZXJ5XCI6XCJqcXVlcnlcIn1dLDg6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG52YXIgJCA9IHdpbmRvdy5qUXVlcnkgfHwgd2luZG93LlplcHRvO1xyXG5cclxudmFyIEV2ZW50RGVsZWdhdG9yID0gX2RlcmVxXygyKTtcclxuXHJcbi8qKlxyXG4gKiBDcmVhdGUgYSBuZXcgU2VsZWN0aXZpdHkgaW5zdGFuY2Ugb3IgaW52b2tlIGEgbWV0aG9kIG9uIGFuIGluc3RhbmNlLlxyXG4gKlxyXG4gKiBAcGFyYW0gbWV0aG9kTmFtZSBPcHRpb25hbCBuYW1lIG9mIGEgbWV0aG9kIHRvIGNhbGwuIElmIG9taXR0ZWQsIGEgU2VsZWN0aXZpdHkgaW5zdGFuY2UgaXNcclxuICogICAgICAgICAgICAgICAgICAgY3JlYXRlZCBmb3IgZWFjaCBlbGVtZW50IGluIHRoZSBzZXQgb2YgbWF0Y2hlZCBlbGVtZW50cy4gSWYgYW4gZWxlbWVudCBpbiB0aGVcclxuICogICAgICAgICAgICAgICAgICAgc2V0IGFscmVhZHkgaGFzIGEgU2VsZWN0aXZpdHkgaW5zdGFuY2UsIHRoZSByZXN1bHQgaXMgdGhlIHNhbWUgYXMgaWYgdGhlXHJcbiAqICAgICAgICAgICAgICAgICAgIHNldE9wdGlvbnMoKSBtZXRob2QgaXMgY2FsbGVkLiBJZiBhIG1ldGhvZCBuYW1lIGlzIGdpdmVuLCB0aGUgb3B0aW9uc1xyXG4gKiAgICAgICAgICAgICAgICAgICBwYXJhbWV0ZXIgaXMgaWdub3JlZCBhbmQgYW55IGFkZGl0aW9uYWwgcGFyYW1ldGVycyBhcmUgcGFzc2VkIHRvIHRoZSBnaXZlblxyXG4gKiAgICAgICAgICAgICAgICAgICBtZXRob2QuXHJcbiAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0IHRvIHBhc3MgdG8gdGhlIGNvbnN0cnVjdG9yIG9yIHRoZSBzZXRPcHRpb25zKCkgbWV0aG9kLiBJbiBjYXNlXHJcbiAqICAgICAgICAgICAgICAgIGEgbmV3IGluc3RhbmNlIGlzIGJlaW5nIGNyZWF0ZWQsIHRoZSBmb2xsb3dpbmcgcHJvcGVydGllcyBhcmUgdXNlZDpcclxuICogICAgICAgICAgICAgICAgaW5wdXRUeXBlIC0gVGhlIGlucHV0IHR5cGUgdG8gdXNlLiBEZWZhdWx0IGlucHV0IHR5cGVzIGluY2x1ZGUgJ011bHRpcGxlJyBhbmRcclxuICogICAgICAgICAgICAgICAgICAgICAgICAgICAgJ1NpbmdsZScsIGJ1dCB5b3UgY2FuIGFkZCBjdXN0b20gaW5wdXQgdHlwZXMgdG8gdGhlIElucHV0VHlwZXMgbWFwIG9yXHJcbiAqICAgICAgICAgICAgICAgICAgICAgICAgICAgIGp1c3Qgc3BlY2lmeSBvbmUgaGVyZSBhcyBhIGZ1bmN0aW9uLiBUaGUgZGVmYXVsdCB2YWx1ZSBpcyAnU2luZ2xlJyxcclxuICogICAgICAgICAgICAgICAgICAgICAgICAgICAgdW5sZXNzIG11bHRpcGxlIGlzIHRydWUgaW4gd2hpY2ggY2FzZSBpdCBpcyAnTXVsdGlwbGUnLlxyXG4gKiAgICAgICAgICAgICAgICBtdWx0aXBsZSAtIEJvb2xlYW4gZGV0ZXJtaW5pbmcgd2hldGhlciBtdWx0aXBsZSBpdGVtcyBtYXkgYmUgc2VsZWN0ZWRcclxuICogICAgICAgICAgICAgICAgICAgICAgICAgICAoZGVmYXVsdDogZmFsc2UpLiBJZiB0cnVlLCBhIE11bHRpcGxlU2VsZWN0aXZpdHkgaW5zdGFuY2UgaXMgY3JlYXRlZCxcclxuICogICAgICAgICAgICAgICAgICAgICAgICAgICBvdGhlcndpc2UgYSBTaW5nbGVTZWxlY3Rpdml0eSBpbnN0YW5jZSBpcyBjcmVhdGVkLlxyXG4gKlxyXG4gKiBAcmV0dXJuIElmIHRoZSBnaXZlbiBtZXRob2QgcmV0dXJucyBhIHZhbHVlLCB0aGlzIG1ldGhvZCByZXR1cm5zIHRoZSB2YWx1ZSBvZiB0aGF0IG1ldGhvZFxyXG4gKiAgICAgICAgIGV4ZWN1dGVkIG9uIHRoZSBmaXJzdCBlbGVtZW50IGluIHRoZSBzZXQgb2YgbWF0Y2hlZCBlbGVtZW50cy5cclxuICovXHJcbmZ1bmN0aW9uIHNlbGVjdGl2aXR5KG1ldGhvZE5hbWUsIG9wdGlvbnMpIHtcclxuICAgIC8qIGpzaGludCB2YWxpZHRoaXM6IHRydWUgKi9cclxuXHJcbiAgICB2YXIgbWV0aG9kQXJncyA9IEFycmF5LnByb3RvdHlwZS5zbGljZS5jYWxsKGFyZ3VtZW50cywgMSk7XHJcbiAgICB2YXIgcmVzdWx0O1xyXG5cclxuICAgIHRoaXMuZWFjaChmdW5jdGlvbigpIHtcclxuICAgICAgICB2YXIgaW5zdGFuY2UgPSB0aGlzLnNlbGVjdGl2aXR5O1xyXG5cclxuICAgICAgICBpZiAoaW5zdGFuY2UpIHtcclxuICAgICAgICAgICAgaWYgKCQudHlwZShtZXRob2ROYW1lKSAhPT0gJ3N0cmluZycpIHtcclxuICAgICAgICAgICAgICAgIG1ldGhvZEFyZ3MgPSBbbWV0aG9kTmFtZV07XHJcbiAgICAgICAgICAgICAgICBtZXRob2ROYW1lID0gJ3NldE9wdGlvbnMnO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICBpZiAoJC50eXBlKGluc3RhbmNlW21ldGhvZE5hbWVdKSA9PT0gJ2Z1bmN0aW9uJykge1xyXG4gICAgICAgICAgICAgICAgaWYgKHJlc3VsdCA9PT0gdW5kZWZpbmVkKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgcmVzdWx0ID0gaW5zdGFuY2VbbWV0aG9kTmFtZV0uYXBwbHkoaW5zdGFuY2UsIG1ldGhvZEFyZ3MpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKCdVbmtub3duIG1ldGhvZDogJyArIG1ldGhvZE5hbWUpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgaWYgKCQudHlwZShtZXRob2ROYW1lKSA9PT0gJ3N0cmluZycpIHtcclxuICAgICAgICAgICAgICAgIGlmIChtZXRob2ROYW1lICE9PSAnZGVzdHJveScpIHtcclxuICAgICAgICAgICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoJ0Nhbm5vdCBjYWxsIG1ldGhvZCBvbiBlbGVtZW50IHdpdGhvdXQgU2VsZWN0aXZpdHkgaW5zdGFuY2UnKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIG9wdGlvbnMgPSAkLmV4dGVuZCh7fSwgbWV0aG9kTmFtZSwgeyBlbGVtZW50OiB0aGlzIH0pO1xyXG5cclxuICAgICAgICAgICAgICAgIC8vIHRoaXMgaXMgYSBvbmUtdGltZSBoYWNrIHRvIGZhY2lsaXRhdGUgdGhlIHNlbGVjdGl2aXR5LXRyYWRpdGlvbmFsIG1vZHVsZSwgYmVjYXVzZVxyXG4gICAgICAgICAgICAgICAgLy8gdGhlIG1vZHVsZSBpcyBub3QgYWJsZSB0byBob29rIHRoaXMgZWFybHkgaW50byBjcmVhdGlvbiBvZiB0aGUgaW5zdGFuY2VcclxuICAgICAgICAgICAgICAgIHZhciAkdGhpcyA9ICQodGhpcyk7XHJcbiAgICAgICAgICAgICAgICBpZiAoJHRoaXMuaXMoJ3NlbGVjdCcpICYmICR0aGlzLnByb3AoJ211bHRpcGxlJykpIHtcclxuICAgICAgICAgICAgICAgICAgICBvcHRpb25zLm11bHRpcGxlID0gdHJ1ZTtcclxuICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICB2YXIgSW5wdXRUeXBlcyA9IFNlbGVjdGl2aXR5LklucHV0VHlwZXM7XHJcbiAgICAgICAgICAgICAgICB2YXIgSW5wdXRUeXBlID0gKG9wdGlvbnMuaW5wdXRUeXBlIHx8IChvcHRpb25zLm11bHRpcGxlID8gJ011bHRpcGxlJyA6ICdTaW5nbGUnKSk7XHJcbiAgICAgICAgICAgICAgICBpZiAoJC50eXBlKElucHV0VHlwZSkgIT09ICdmdW5jdGlvbicpIHtcclxuICAgICAgICAgICAgICAgICAgICBpZiAoSW5wdXRUeXBlc1tJbnB1dFR5cGVdKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIElucHV0VHlwZSA9IElucHV0VHlwZXNbSW5wdXRUeXBlXTtcclxuICAgICAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoJ1Vua25vd24gU2VsZWN0aXZpdHkgaW5wdXQgdHlwZTogJyArIElucHV0VHlwZSk7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgICAgIHRoaXMuc2VsZWN0aXZpdHkgPSBuZXcgSW5wdXRUeXBlKG9wdGlvbnMpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfSk7XHJcblxyXG4gICAgcmV0dXJuIChyZXN1bHQgPT09IHVuZGVmaW5lZCA/IHRoaXMgOiByZXN1bHQpO1xyXG59XHJcblxyXG4vKipcclxuICogU2VsZWN0aXZpdHkgQmFzZSBDb25zdHJ1Y3Rvci5cclxuICpcclxuICogWW91IHdpbGwgbmV2ZXIgdXNlIHRoaXMgY29uc3RydWN0b3IgZGlyZWN0bHkuIEluc3RlYWQsIHlvdSB1c2UgJChzZWxlY3Rvcikuc2VsZWN0aXZpdHkob3B0aW9ucylcclxuICogdG8gY3JlYXRlIGFuIGluc3RhbmNlIG9mIGVpdGhlciBNdWx0aXBsZVNlbGVjdGl2aXR5IG9yIFNpbmdsZVNlbGVjdGl2aXR5LiBUaGlzIGNsYXNzIGRlZmluZXMgYWxsXHJcbiAqIGZ1bmN0aW9uYWxpdHkgdGhhdCBpcyBjb21tb24gYmV0d2VlbiBib3RoLlxyXG4gKlxyXG4gKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25zIG9iamVjdC4gQWNjZXB0cyB0aGUgc2FtZSBvcHRpb25zIGFzIHRoZSBzZXRPcHRpb25zIG1ldGhvZCgpLCBpbiBhZGRpdGlvblxyXG4gKiAgICAgICAgICAgICAgICB0byB0aGUgZm9sbG93aW5nIG9uZXM6XHJcbiAqICAgICAgICAgICAgICAgIGRhdGEgLSBJbml0aWFsIHNlbGVjdGlvbiBkYXRhIHRvIHNldC4gVGhpcyBzaG91bGQgYmUgYW4gYXJyYXkgb2Ygb2JqZWN0cyB3aXRoICdpZCdcclxuICogICAgICAgICAgICAgICAgICAgICAgIGFuZCAndGV4dCcgcHJvcGVydGllcy4gVGhpcyBvcHRpb24gaXMgbXV0dWFsbHkgZXhjbHVzaXZlIHdpdGggJ3ZhbHVlJy5cclxuICogICAgICAgICAgICAgICAgZWxlbWVudCAtIFRoZSBET00gZWxlbWVudCB0byB3aGljaCB0byBhdHRhY2ggdGhlIFNlbGVjdGl2aXR5IGluc3RhbmNlLiBUaGlzXHJcbiAqICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wZXJ0eSBpcyBzZXQgYXV0b21hdGljYWxseSBieSB0aGUgJC5mbi5zZWxlY3Rpdml0eSgpIGZ1bmN0aW9uLlxyXG4gKiAgICAgICAgICAgICAgICB2YWx1ZSAtIEluaXRpYWwgdmFsdWUgdG8gc2V0LiBUaGlzIHNob3VsZCBiZSBhbiBhcnJheSBvZiBJRHMuIFRoaXMgcHJvcGVydHkgaXNcclxuICogICAgICAgICAgICAgICAgICAgICAgICBtdXR1YWxseSBleGNsdXNpdmUgd2l0aCAnZGF0YScuXHJcbiAqL1xyXG5mdW5jdGlvbiBTZWxlY3Rpdml0eShvcHRpb25zKSB7XHJcblxyXG4gICAgaWYgKCEodGhpcyBpbnN0YW5jZW9mIFNlbGVjdGl2aXR5KSkge1xyXG4gICAgICAgIHJldHVybiBzZWxlY3Rpdml0eS5hcHBseSh0aGlzLCBhcmd1bWVudHMpO1xyXG4gICAgfVxyXG5cclxuICAgIC8qKlxyXG4gICAgICogalF1ZXJ5IGNvbnRhaW5lciBmb3IgdGhlIGVsZW1lbnQgdG8gd2hpY2ggdGhpcyBpbnN0YW5jZSBpcyBhdHRhY2hlZC5cclxuICAgICAqL1xyXG4gICAgdGhpcy4kZWwgPSAkKG9wdGlvbnMuZWxlbWVudCk7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBqUXVlcnkgY29udGFpbmVyIGZvciB0aGUgc2VhcmNoIGlucHV0LlxyXG4gICAgICpcclxuICAgICAqIE1heSBiZSBudWxsIGFzIGxvbmcgYXMgdGhlcmUgaXMgbm8gdmlzaWJsZSBzZWFyY2ggaW5wdXQuIEl0IGlzIHNldCBieSBpbml0U2VhcmNoSW5wdXQoKS5cclxuICAgICAqL1xyXG4gICAgdGhpcy4kc2VhcmNoSW5wdXQgPSBudWxsO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVmZXJlbmNlIHRvIHRoZSBjdXJyZW50bHkgb3BlbiBkcm9wZG93bi5cclxuICAgICAqL1xyXG4gICAgdGhpcy5kcm9wZG93biA9IG51bGw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBXaGV0aGVyIHRoZSBpbnB1dCBpcyBlbmFibGVkLlxyXG4gICAgICpcclxuICAgICAqIFRoaXMgaXMgZmFsc2Ugd2hlbiB0aGUgb3B0aW9uIHJlYWRPbmx5IGlzIGZhbHNlIG9yIHRoZSBvcHRpb24gcmVtb3ZlT25seSBpcyBmYWxzZS5cclxuICAgICAqL1xyXG4gICAgdGhpcy5lbmFibGVkID0gdHJ1ZTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEJvb2xlYW4gd2hldGhlciB0aGUgYnJvd3NlciBoYXMgdG91Y2ggaW5wdXQuXHJcbiAgICAgKi9cclxuICAgIHRoaXMuaGFzVG91Y2ggPSAodHlwZW9mIHdpbmRvdyAhPT0gJ3VuZGVmaW5lZCcgJiYgJ29udG91Y2hzdGFydCcgaW4gd2luZG93KTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEJvb2xlYW4gd2hldGhlciB0aGUgYnJvd3NlciBoYXMgYSBwaHlzaWNhbCBrZXlib2FyZCBhdHRhY2hlZCB0byBpdC5cclxuICAgICAqXHJcbiAgICAgKiBHaXZlbiB0aGF0IHRoZXJlIGlzIG5vIHdheSBmb3IgSmF2YVNjcmlwdCB0byByZWxpYWJseSBkZXRlY3QgdGhpcyB5ZXQsIHdlIGp1c3QgYXNzdW1lIGl0J3NcclxuICAgICAqIHRoZSBvcHBvc2l0ZSBvZiBoYXNUb3VjaCBmb3Igbm93LlxyXG4gICAgICovXHJcbiAgICB0aGlzLmhhc0tleWJvYXJkID0gIXRoaXMuaGFzVG91Y2g7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBBcnJheSBvZiBpdGVtcyBmcm9tIHdoaWNoIHRvIHNlbGVjdC4gSWYgc2V0LCB0aGlzIHdpbGwgYmUgYW4gYXJyYXkgb2Ygb2JqZWN0cyB3aXRoICdpZCcgYW5kXHJcbiAgICAgKiAndGV4dCcgcHJvcGVydGllcy5cclxuICAgICAqXHJcbiAgICAgKiBJZiBnaXZlbiwgYWxsIGl0ZW1zIGFyZSBleHBlY3RlZCB0byBiZSBhdmFpbGFibGUgbG9jYWxseSBhbmQgYWxsIHNlbGVjdGlvbiBvcGVyYXRpb25zIG9wZXJhdGVcclxuICAgICAqIG9uIHRoaXMgbG9jYWwgYXJyYXkgb25seS4gSWYgbnVsbCwgaXRlbXMgYXJlIG5vdCBhdmFpbGFibGUgbG9jYWxseSwgYW5kIGEgcXVlcnkgZnVuY3Rpb25cclxuICAgICAqIHNob3VsZCBiZSBwcm92aWRlZCB0byBmZXRjaCByZW1vdGUgZGF0YS5cclxuICAgICAqL1xyXG4gICAgdGhpcy5pdGVtcyA9IG51bGw7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBUaGUgZnVuY3Rpb24gdG8gYmUgdXNlZCBmb3IgbWF0Y2hpbmcgc2VhcmNoIHJlc3VsdHMuXHJcbiAgICAgKi9cclxuICAgIHRoaXMubWF0Y2hlciA9IFNlbGVjdGl2aXR5Lm1hdGNoZXI7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBPcHRpb25zIHBhc3NlZCB0byB0aGUgU2VsZWN0aXZpdHkgaW5zdGFuY2Ugb3Igc2V0IHRocm91Z2ggc2V0T3B0aW9ucygpLlxyXG4gICAgICovXHJcbiAgICB0aGlzLm9wdGlvbnMgPSB7fTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEFycmF5IG9mIHNlYXJjaCBpbnB1dCBsaXN0ZW5lcnMuXHJcbiAgICAgKlxyXG4gICAgICogQ3VzdG9tIGxpc3RlbmVycyBjYW4gYmUgc3BlY2lmaWVkIGluIHRoZSBvcHRpb25zIG9iamVjdC5cclxuICAgICAqL1xyXG4gICAgdGhpcy5zZWFyY2hJbnB1dExpc3RlbmVycyA9IFNlbGVjdGl2aXR5LlNlYXJjaElucHV0TGlzdGVuZXJzO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogTWFwcGluZyBvZiB0ZW1wbGF0ZXMuXHJcbiAgICAgKlxyXG4gICAgICogQ3VzdG9tIHRlbXBsYXRlcyBjYW4gYmUgc3BlY2lmaWVkIGluIHRoZSBvcHRpb25zIG9iamVjdC5cclxuICAgICAqL1xyXG4gICAgdGhpcy50ZW1wbGF0ZXMgPSAkLmV4dGVuZCh7fSwgU2VsZWN0aXZpdHkuVGVtcGxhdGVzKTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFRoZSBsYXN0IHVzZWQgc2VhcmNoIHRlcm0uXHJcbiAgICAgKi9cclxuICAgIHRoaXMudGVybSA9ICcnO1xyXG5cclxuICAgIHRoaXMuc2V0T3B0aW9ucyhvcHRpb25zKTtcclxuXHJcbiAgICBpZiAob3B0aW9ucy52YWx1ZSkge1xyXG4gICAgICAgIHRoaXMudmFsdWUob3B0aW9ucy52YWx1ZSwgeyB0cmlnZ2VyQ2hhbmdlOiBmYWxzZSB9KTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgICAgdGhpcy5kYXRhKG9wdGlvbnMuZGF0YSB8fCBudWxsLCB7IHRyaWdnZXJDaGFuZ2U6IGZhbHNlIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuJGVsLm9uKCdtb3VzZW92ZXInLCB0aGlzLl9tb3VzZW92ZXIuYmluZCh0aGlzKSk7XHJcbiAgICB0aGlzLiRlbC5vbignbW91c2VsZWF2ZScsIHRoaXMuX21vdXNlb3V0LmJpbmQodGhpcykpO1xyXG4gICAgdGhpcy4kZWwub24oJ3NlbGVjdGl2aXR5LWNsb3NlJywgdGhpcy5fY2xvc2VkLmJpbmQodGhpcykpO1xyXG5cclxuICAgIEV2ZW50RGVsZWdhdG9yLmNhbGwodGhpcyk7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBNZXRob2RzLlxyXG4gKi9cclxuJC5leHRlbmQoU2VsZWN0aXZpdHkucHJvdG90eXBlLCBFdmVudERlbGVnYXRvci5wcm90b3R5cGUsIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIENvbnZlbmllbmNlIHNob3J0Y3V0IGZvciB0aGlzLiRlbC5maW5kKHNlbGVjdG9yKS5cclxuICAgICAqL1xyXG4gICAgJDogZnVuY3Rpb24oc2VsZWN0b3IpIHtcclxuXHJcbiAgICAgICAgcmV0dXJuIHRoaXMuJGVsLmZpbmQoc2VsZWN0b3IpO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIENsb3NlcyB0aGUgZHJvcGRvd24uXHJcbiAgICAgKi9cclxuICAgIGNsb3NlOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMuZHJvcGRvd24pIHtcclxuICAgICAgICAgICAgdGhpcy5kcm9wZG93bi5jbG9zZSgpO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBTZXRzIG9yIGdldHMgdGhlIHNlbGVjdGlvbiBkYXRhLlxyXG4gICAgICpcclxuICAgICAqIFRoZSBzZWxlY3Rpb24gZGF0YSBjb250YWlucyBib3RoIElEcyBhbmQgdGV4dCBsYWJlbHMuIElmIHlvdSBvbmx5IHdhbnQgdG8gc2V0IG9yIGdldCB0aGUgSURzLFxyXG4gICAgICogeW91IHNob3VsZCB1c2UgdGhlIHZhbHVlKCkgbWV0aG9kLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBuZXdEYXRhIE9wdGlvbmFsIG5ldyBkYXRhIHRvIHNldC4gRm9yIGEgTXVsdGlwbGVTZWxlY3Rpdml0eSBpbnN0YW5jZSB0aGUgZGF0YSBtdXN0IGJlXHJcbiAgICAgKiAgICAgICAgICAgICAgICBhbiBhcnJheSBvZiBvYmplY3RzIHdpdGggJ2lkJyBhbmQgJ3RleHQnIHByb3BlcnRpZXMsIGZvciBhIFNpbmdsZVNlbGVjdGl2aXR5XHJcbiAgICAgKiAgICAgICAgICAgICAgICBpbnN0YW5jZSB0aGUgZGF0YSBtdXN0IGJlIGEgc2luZ2xlIHN1Y2ggb2JqZWN0IG9yIG51bGwgdG8gaW5kaWNhdGUgbm8gaXRlbSBpc1xyXG4gICAgICogICAgICAgICAgICAgICAgc2VsZWN0ZWQuXHJcbiAgICAgKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25hbCBvcHRpb25zIG9iamVjdC4gTWF5IGNvbnRhaW4gdGhlIGZvbGxvd2luZyBwcm9wZXJ0eTpcclxuICAgICAqICAgICAgICAgICAgICAgIHRyaWdnZXJDaGFuZ2UgLSBTZXQgdG8gZmFsc2UgdG8gc3VwcHJlc3MgdGhlIFwiY2hhbmdlXCIgZXZlbnQgYmVpbmcgdHJpZ2dlcmVkLlxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIE5vdGUgdGhpcyB3aWxsIGFsc28gY2F1c2UgdGhlIFVJIHRvIG5vdCB1cGRhdGUgYXV0b21hdGljYWxseTtcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBzbyB5b3UgbWF5IHdhbnQgdG8gY2FsbCByZXJlbmRlclNlbGVjdGlvbigpIG1hbnVhbGx5IHdoZW5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB1c2luZyB0aGlzIG9wdGlvbi5cclxuICAgICAqXHJcbiAgICAgKiBAcmV0dXJuIElmIG5ld0RhdGEgaXMgb21pdHRlZCwgdGhpcyBtZXRob2QgcmV0dXJucyB0aGUgY3VycmVudCBkYXRhLlxyXG4gICAgICovXHJcbiAgICBkYXRhOiBmdW5jdGlvbihuZXdEYXRhLCBvcHRpb25zKSB7XHJcblxyXG4gICAgICAgIG9wdGlvbnMgPSBvcHRpb25zIHx8IHt9O1xyXG5cclxuICAgICAgICBpZiAobmV3RGF0YSA9PT0gdW5kZWZpbmVkKSB7XHJcbiAgICAgICAgICAgIHJldHVybiB0aGlzLl9kYXRhO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIG5ld0RhdGEgPSB0aGlzLnZhbGlkYXRlRGF0YShuZXdEYXRhKTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMuX2RhdGEgPSBuZXdEYXRhO1xyXG4gICAgICAgICAgICB0aGlzLl92YWx1ZSA9IHRoaXMuZ2V0VmFsdWVGb3JEYXRhKG5ld0RhdGEpO1xyXG5cclxuICAgICAgICAgICAgaWYgKG9wdGlvbnMudHJpZ2dlckNoYW5nZSAhPT0gZmFsc2UpIHtcclxuICAgICAgICAgICAgICAgIHRoaXMudHJpZ2dlckNoYW5nZSgpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIERlc3Ryb3lzIHRoZSBTZWxlY3Rpdml0eSBpbnN0YW5jZS5cclxuICAgICAqL1xyXG4gICAgZGVzdHJveTogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHRoaXMudW5kZWxlZ2F0ZUV2ZW50cygpO1xyXG5cclxuICAgICAgICB2YXIgJGVsID0gdGhpcy4kZWw7XHJcbiAgICAgICAgJGVsLmNoaWxkcmVuKCkucmVtb3ZlKCk7XHJcbiAgICAgICAgJGVsWzBdLnNlbGVjdGl2aXR5ID0gbnVsbDtcclxuICAgICAgICAkZWwgPSBudWxsO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEZpbHRlcnMgdGhlIHJlc3VsdHMgdG8gYmUgZGlzcGxheWVkIGluIHRoZSBkcm9wZG93bi5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgZGVmYXVsdCBpbXBsZW1lbnRhdGlvbiBzaW1wbHkgcmV0dXJucyB0aGUgcmVzdWx0cyB1bmZpbHRlcmVkLCBidXQgdGhlIE11bHRpcGxlU2VsZWN0aXZpdHlcclxuICAgICAqIGNsYXNzIG92ZXJyaWRlcyB0aGlzIG1ldGhvZCB0byBmaWx0ZXIgb3V0IGFueSBpdGVtcyB0aGF0IGhhdmUgYWxyZWFkeSBiZWVuIHNlbGVjdGVkLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSByZXN1bHRzIEFycmF5IG9mIGl0ZW1zIHdpdGggJ2lkJyBhbmQgJ3RleHQnIHByb3BlcnRpZXMuXHJcbiAgICAgKlxyXG4gICAgICogQHJldHVybiBUaGUgZmlsdGVyZWQgYXJyYXkuXHJcbiAgICAgKi9cclxuICAgIGZpbHRlclJlc3VsdHM6IGZ1bmN0aW9uKHJlc3VsdHMpIHtcclxuXHJcbiAgICAgICAgcmV0dXJuIHJlc3VsdHM7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQXBwbGllcyBmb2N1cyB0byB0aGUgaW5wdXQuXHJcbiAgICAgKi9cclxuICAgIGZvY3VzOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMuJHNlYXJjaElucHV0KSB7XHJcbiAgICAgICAgICAgIHRoaXMuJHNlYXJjaElucHV0LmZvY3VzKCk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJldHVybnMgdGhlIGNvcnJlY3QgaXRlbSBmb3IgYSBnaXZlbiBJRC5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gaWQgVGhlIElEIHRvIGdldCB0aGUgaXRlbSBmb3IuXHJcbiAgICAgKlxyXG4gICAgICogQHJldHVybiBUaGUgY29ycmVzcG9uZGluZyBpdGVtLiBXaWxsIGJlIGFuIG9iamVjdCB3aXRoICdpZCcgYW5kICd0ZXh0JyBwcm9wZXJ0aWVzIG9yIG51bGwgaWZcclxuICAgICAqICAgICAgICAgdGhlIGl0ZW0gY2Fubm90IGJlIGZvdW5kLiBOb3RlIHRoYXQgaWYgbm8gaXRlbXMgYXJlIGRlZmluZWQsIHRoaXMgbWV0aG9kIGFzc3VtZXMgdGhlXHJcbiAgICAgKiAgICAgICAgIHRleHQgbGFiZWxzIHdpbGwgYmUgZXF1YWwgdG8gdGhlIElEcy5cclxuICAgICAqL1xyXG4gICAgZ2V0SXRlbUZvcklkOiBmdW5jdGlvbihpZCkge1xyXG5cclxuICAgICAgICB2YXIgaXRlbXMgPSB0aGlzLml0ZW1zO1xyXG4gICAgICAgIGlmIChpdGVtcykge1xyXG4gICAgICAgICAgICByZXR1cm4gU2VsZWN0aXZpdHkuZmluZE5lc3RlZEJ5SWQoaXRlbXMsIGlkKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICByZXR1cm4geyBpZDogaWQsIHRleHQ6ICcnICsgaWQgfTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogSW5pdGlhbGl6ZXMgdGhlIHNlYXJjaCBpbnB1dCBlbGVtZW50LlxyXG4gICAgICpcclxuICAgICAqIFNldHMgdGhlICRzZWFyY2hJbnB1dCBwcm9wZXJ0eSwgaW52b2tlcyBhbGwgc2VhcmNoIGlucHV0IGxpc3RlbmVycyBhbmQgYXR0YWNoZXMgdGhlIGRlZmF1bHRcclxuICAgICAqIGFjdGlvbiBvZiBzZWFyY2hpbmcgd2hlbiBzb21ldGhpbmcgaXMgdHlwZWQuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtICRpbnB1dCBqUXVlcnkgY29udGFpbmVyIGZvciB0aGUgaW5wdXQgZWxlbWVudC5cclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbmFsIG9wdGlvbnMgb2JqZWN0LiBNYXkgY29udGFpbiB0aGUgZm9sbG93aW5nIHByb3BlcnR5OlxyXG4gICAgICogICAgICAgICAgICAgICAgbm9TZWFyY2ggLSBJZiB0cnVlLCBubyBldmVudCBoYW5kbGVycyBhcmUgc2V0dXAgdG8gaW5pdGlhdGUgc2VhcmNoaW5nIHdoZW5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgdGhlIHVzZXIgdHlwZXMgaW4gdGhlIGlucHV0IGZpZWxkLiBUaGlzIGlzIHVzZWZ1bCBpZiB5b3Ugd2FudCB0b1xyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICB1c2UgdGhlIGlucHV0IG9ubHkgdG8gaGFuZGxlIGtleWJvYXJkIHN1cHBvcnQuXHJcbiAgICAgKi9cclxuICAgIGluaXRTZWFyY2hJbnB1dDogZnVuY3Rpb24oJGlucHV0LCBvcHRpb25zKSB7XHJcblxyXG4gICAgICAgIHRoaXMuJHNlYXJjaElucHV0ID0gJGlucHV0O1xyXG5cclxuICAgICAgICB0aGlzLnNlYXJjaElucHV0TGlzdGVuZXJzLmZvckVhY2goZnVuY3Rpb24obGlzdGVuZXIpIHtcclxuICAgICAgICAgICAgbGlzdGVuZXIodGhpcywgJGlucHV0KTtcclxuICAgICAgICB9LmJpbmQodGhpcykpO1xyXG5cclxuICAgICAgICBpZiAoIW9wdGlvbnMgfHwgIW9wdGlvbnMubm9TZWFyY2gpIHtcclxuICAgICAgICAgICAgJGlucHV0Lm9uKCdrZXl1cCcsIGZ1bmN0aW9uKGV2ZW50KSB7XHJcbiAgICAgICAgICAgICAgICBpZiAoIWV2ZW50LmlzRGVmYXVsdFByZXZlbnRlZCgpKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgdGhpcy5zZWFyY2goKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfS5iaW5kKHRoaXMpKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogT3BlbnMgdGhlIGRyb3Bkb3duLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbmFsIG9wdGlvbnMgb2JqZWN0LiBNYXkgY29udGFpbiB0aGUgZm9sbG93aW5nIHByb3BlcnR5OlxyXG4gICAgICogICAgICAgICAgICAgICAgc2VhcmNoIC0gQm9vbGVhbiB3aGV0aGVyIHRoZSBkcm9wZG93biBzaG91bGQgYmUgaW5pdGlhbGl6ZWQgYnkgcGVyZm9ybWluZyBhXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICBzZWFyY2ggZm9yIHRoZSBlbXB0eSBzdHJpbmcgKGllLiBkaXNwbGF5IGFsbCByZXN1bHRzKS4gRGVmYXVsdCBpc1xyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgdHJ1ZS5cclxuICAgICAqICAgICAgICAgICAgICAgIHNob3dTZWFyY2hJbnB1dCAtIEJvb2xlYW4gd2hldGhlciBhIHNlYXJjaCBpbnB1dCBzaG91bGQgYmUgc2hvd24gaW4gdGhlXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBkcm9wZG93bi4gRGVmYXVsdCBpcyBmYWxzZS5cclxuICAgICAqL1xyXG4gICAgb3BlbjogZnVuY3Rpb24ob3B0aW9ucykge1xyXG5cclxuICAgICAgICBvcHRpb25zID0gb3B0aW9ucyB8fCB7fTtcclxuXHJcbiAgICAgICAgaWYgKCF0aGlzLmRyb3Bkb3duKSB7XHJcbiAgICAgICAgICAgIGlmICh0aGlzLnRyaWdnZXJFdmVudCgnc2VsZWN0aXZpdHktb3BlbmluZycpKSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgRHJvcGRvd24gPSB0aGlzLm9wdGlvbnMuZHJvcGRvd24gfHwgU2VsZWN0aXZpdHkuRHJvcGRvd247XHJcbiAgICAgICAgICAgICAgICBpZiAoRHJvcGRvd24pIHtcclxuICAgICAgICAgICAgICAgICAgICB0aGlzLmRyb3Bkb3duID0gbmV3IERyb3Bkb3duKHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgaXRlbXM6IHRoaXMuaXRlbXMsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHBvc2l0aW9uOiB0aGlzLm9wdGlvbnMucG9zaXRpb25Ecm9wZG93bixcclxuICAgICAgICAgICAgICAgICAgICAgICAgcXVlcnk6IHRoaXMub3B0aW9ucy5xdWVyeSxcclxuICAgICAgICAgICAgICAgICAgICAgICAgc2VsZWN0aXZpdHk6IHRoaXMsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHNob3dTZWFyY2hJbnB1dDogb3B0aW9ucy5zaG93U2VhcmNoSW5wdXRcclxuICAgICAgICAgICAgICAgICAgICB9KTtcclxuICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICBpZiAob3B0aW9ucy5zZWFyY2ggIT09IGZhbHNlKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgdGhpcy5zZWFyY2goJycpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICB0aGlzLiRlbC5jaGlsZHJlbigpLnRvZ2dsZUNsYXNzKCdvcGVuJywgdHJ1ZSk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIChSZS0pcG9zaXRpb25zIHRoZSBkcm9wZG93bi5cclxuICAgICAqL1xyXG4gICAgcG9zaXRpb25Ecm9wZG93bjogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLmRyb3Bkb3duKSB7XHJcbiAgICAgICAgICAgIHRoaXMuZHJvcGRvd24ucG9zaXRpb24oKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogU2VhcmNoZXMgZm9yIHJlc3VsdHMgYmFzZWQgb24gdGhlIHRlcm0gZ2l2ZW4gb3IgdGhlIHRlcm0gZW50ZXJlZCBpbiB0aGUgc2VhcmNoIGlucHV0LlxyXG4gICAgICpcclxuICAgICAqIElmIGFuIGl0ZW1zIGFycmF5IGhhcyBiZWVuIHBhc3NlZCB3aXRoIHRoZSBvcHRpb25zIHRvIHRoZSBTZWxlY3Rpdml0eSBpbnN0YW5jZSwgYSBsb2NhbFxyXG4gICAgICogc2VhcmNoIHdpbGwgYmUgcGVyZm9ybWVkIGFtb25nIHRob3NlIGl0ZW1zLiBPdGhlcndpc2UsIHRoZSBxdWVyeSBmdW5jdGlvbiBzcGVjaWZpZWQgaW4gdGhlXHJcbiAgICAgKiBvcHRpb25zIHdpbGwgYmUgdXNlZCB0byBwZXJmb3JtIHRoZSBzZWFyY2guIElmIG5laXRoZXIgaXMgZGVmaW5lZCwgbm90aGluZyBoYXBwZW5zLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSB0ZXJtIE9wdGlvbmFsIHRlcm0gdG8gc2VhcmNoIGZvci4gSWYgb21taXR0ZWQsIHRoZSB2YWx1ZSBvZiB0aGUgc2VhcmNoIGlucHV0IGVsZW1lbnRcclxuICAgICAqICAgICAgICAgICAgIGlzIHVzZWQgYXMgdGVybS5cclxuICAgICAqL1xyXG4gICAgc2VhcmNoOiBmdW5jdGlvbih0ZXJtKSB7XHJcblxyXG4gICAgICAgIGlmICh0ZXJtID09PSB1bmRlZmluZWQpIHtcclxuICAgICAgICAgICAgdGVybSA9ICh0aGlzLiRzZWFyY2hJbnB1dCA/IHRoaXMuJHNlYXJjaElucHV0LnZhbCgpIDogJycpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdGhpcy5vcGVuKHsgc2VhcmNoOiBmYWxzZSB9KTtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMuZHJvcGRvd24pIHtcclxuICAgICAgICAgICAgdGhpcy5kcm9wZG93bi5zZWFyY2godGVybSk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFNldHMgb25lIG9yIG1vcmUgb3B0aW9ucyBvbiB0aGlzIFNlbGVjdGl2aXR5IGluc3RhbmNlLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0LiBNYXkgY29udGFpbiBvbmUgb3IgbW9yZSBvZiB0aGUgZm9sbG93aW5nIHByb3BlcnRpZXM6XHJcbiAgICAgKiAgICAgICAgICAgICAgICBjbG9zZU9uU2VsZWN0IC0gU2V0IHRvIGZhbHNlIHRvIGtlZXAgdGhlIGRyb3Bkb3duIG9wZW4gYWZ0ZXIgdGhlIHVzZXIgaGFzXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgc2VsZWN0ZWQgYW4gaXRlbS4gVGhpcyBpcyB1c2VmdWwgaWYgeW91IHdhbnQgdG8gYWxsb3cgdGhlIHVzZXJcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB0byBxdWlja2x5IHNlbGVjdCBtdWx0aXBsZSBpdGVtcy4gVGhlIGRlZmF1bHQgdmFsdWUgaXMgdHJ1ZS5cclxuICAgICAqICAgICAgICAgICAgICAgIGRyb3Bkb3duIC0gQ3VzdG9tIGRyb3Bkb3duIGltcGxlbWVudGF0aW9uIHRvIHVzZSBmb3IgdGhpcyBpbnN0YW5jZS5cclxuICAgICAqICAgICAgICAgICAgICAgIGluaXRTZWxlY3Rpb24gLSBGdW5jdGlvbiB0byBtYXAgdmFsdWVzIGJ5IElEIHRvIHNlbGVjdGlvbiBkYXRhLiBUaGlzIGZ1bmN0aW9uXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVjZWl2ZXMgdHdvIGFyZ3VtZW50cywgJ3ZhbHVlJyBhbmQgJ2NhbGxiYWNrJy4gVGhlIHZhbHVlIGlzXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgdGhlIGN1cnJlbnQgdmFsdWUgb2YgdGhlIHNlbGVjdGlvbiwgd2hpY2ggaXMgYW4gSUQgb3IgYW4gYXJyYXlcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBvZiBJRHMgZGVwZW5kaW5nIG9uIHRoZSBpbnB1dCB0eXBlLiBUaGUgY2FsbGJhY2sgc2hvdWxkIGJlXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgaW52b2tlZCB3aXRoIGFuIG9iamVjdCBvciBhcnJheSBvZiBvYmplY3RzLCByZXNwZWN0aXZlbHksXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgY29udGFpbmluZyAnaWQnIGFuZCAndGV4dCcgcHJvcGVydGllcy5cclxuICAgICAqICAgICAgICAgICAgICAgIGl0ZW1zIC0gQXJyYXkgb2YgaXRlbXMgZnJvbSB3aGljaCB0byBzZWxlY3QuIFNob3VsZCBiZSBhbiBhcnJheSBvZiBvYmplY3RzXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgIHdpdGggJ2lkJyBhbmQgJ3RleHQnIHByb3BlcnRpZXMuIEFzIGNvbnZlbmllbmNlLCB5b3UgbWF5IGFsc28gcGFzcyBhblxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICBhcnJheSBvZiBzdHJpbmdzLCBpbiB3aGljaCBjYXNlIHRoZSBzYW1lIHN0cmluZyBpcyB1c2VkIGZvciBib3RoIHRoZVxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAnaWQnIGFuZCAndGV4dCcgcHJvcGVydGllcy4gSWYgaXRlbXMgYXJlIGdpdmVuLCBhbGwgaXRlbXMgYXJlIGV4cGVjdGVkXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgIHRvIGJlIGF2YWlsYWJsZSBsb2NhbGx5IGFuZCBhbGwgc2VsZWN0aW9uIG9wZXJhdGlvbnMgb3BlcmF0ZSBvbiB0aGlzXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgIGxvY2FsIGFycmF5IG9ubHkuIElmIG51bGwsIGl0ZW1zIGFyZSBub3QgYXZhaWxhYmxlIGxvY2FsbHksIGFuZCBhXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgIHF1ZXJ5IGZ1bmN0aW9uIHNob3VsZCBiZSBwcm92aWRlZCB0byBmZXRjaCByZW1vdGUgZGF0YS5cclxuICAgICAqICAgICAgICAgICAgICAgIG1hdGNoZXIgLSBGdW5jdGlvbiB0byBkZXRlcm1pbmUgd2hldGhlciB0ZXh0IG1hdGNoZXMgYSBnaXZlbiBzZWFyY2ggdGVybS4gTm90ZVxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgIHRoaXMgZnVuY3Rpb24gaXMgb25seSB1c2VkIGlmIHlvdSBoYXZlIHNwZWNpZmllZCBhbiBhcnJheSBvZiBpdGVtcy5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICBSZWNlaXZlcyB0d28gYXJndW1lbnRzOlxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgIGl0ZW0gLSBUaGUgaXRlbSB0aGF0IHNob3VsZCBtYXRjaCB0aGUgc2VhcmNoIHRlcm0uXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgdGVybSAtIFRoZSBzZWFyY2ggdGVybS4gTm90ZSB0aGF0IGZvciBwZXJmb3JtYW5jZSByZWFzb25zLCB0aGUgdGVybVxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBoYXMgYWx3YXlzIGJlZW4gYWxyZWFkeSBwcm9jZXNzZWQgdXNpbmdcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgU2VsZWN0aXZpdHkudHJhbnNmb3JtVGV4dCgpLlxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgIFRoZSBtZXRob2Qgc2hvdWxkIHJldHVybiB0aGUgaXRlbSBpZiBpdCBtYXRjaGVzLCBhbmQgbnVsbCBvdGhlcndpc2UuXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgSWYgdGhlIGl0ZW0gaGFzIGEgY2hpbGRyZW4gYXJyYXksIHRoZSBtYXRjaGVyIGlzIGV4cGVjdGVkIHRvIGZpbHRlclxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgIHRob3NlIGl0c2VsZiAoYmUgc3VyZSB0byBvbmx5IHJldHVybiB0aGUgZmlsdGVyZWQgYXJyYXkgb2YgY2hpbGRyZW5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICBpbiB0aGUgcmV0dXJuZWQgaXRlbSBhbmQgbm90IHRvIG1vZGlmeSB0aGUgY2hpbGRyZW4gb2YgdGhlIGl0ZW1cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICBhcmd1bWVudCkuXHJcbiAgICAgKiAgICAgICAgICAgICAgICBwbGFjZWhvbGRlciAtIFBsYWNlaG9sZGVyIHRleHQgdG8gZGlzcGxheSB3aGVuIHRoZSBlbGVtZW50IGhhcyBubyBmb2N1cyBhbmRcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbm8gc2VsZWN0ZWQgaXRlbXMuXHJcbiAgICAgKiAgICAgICAgICAgICAgICBwb3NpdGlvbkRyb3Bkb3duIC0gRnVuY3Rpb24gdG8gcG9zaXRpb24gdGhlIGRyb3Bkb3duLiBSZWNlaXZlcyB0d28gYXJndW1lbnRzOlxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICRkcm9wZG93bkVsIC0gVGhlIGVsZW1lbnQgdG8gYmUgcG9zaXRpb25lZC5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAkc2VsZWN0RWwgLSBUaGUgZWxlbWVudCBvZiB0aGUgU2VsZWN0aXZpdHkgaW5zdGFuY2UsIHRoYXRcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB5b3UgY2FuIHBvc2l0aW9uIHRoZSBkcm9wZG93biB0by5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBUaGUgZGVmYXVsdCBpbXBsZW1lbnRhdGlvbiBwb3NpdGlvbnMgdGhlIGRyb3Bkb3duIGVsZW1lbnRcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB1bmRlciB0aGUgU2VsZWN0aXZpdHkncyBlbGVtZW50IGFuZCBnaXZlcyBpdCB0aGUgc2FtZVxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHdpZHRoLlxyXG4gICAgICogICAgICAgICAgICAgICAgcXVlcnkgLSBGdW5jdGlvbiB0byB1c2UgZm9yIHF1ZXJ5aW5nIGl0ZW1zLiBSZWNlaXZlcyBhIHNpbmdsZSBvYmplY3QgYXNcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgYXJndW1lbnQgd2l0aCB0aGUgZm9sbG93aW5nIHByb3BlcnRpZXM6XHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgIGNhbGxiYWNrIC0gQ2FsbGJhY2sgdG8gaW52b2tlIHdoZW4gdGhlIHJlc3VsdHMgYXJlIGF2YWlsYWJsZS4gVGhpc1xyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGNhbGxiYWNrIHNob3VsZCBiZSBwYXNzZWQgYSBzaW5nbGUgb2JqZWN0IGFzIGFyZ3VtZW50IHdpdGhcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB0aGUgZm9sbG93aW5nIHByb3BlcnRpZXM6XHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbW9yZSAtIEJvb2xlYW4gdGhhdCBjYW4gYmUgc2V0IHRvIHRydWUgdG8gaW5kaWNhdGUgdGhlcmVcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYXJlIG1vcmUgcmVzdWx0cyBhdmFpbGFibGUuIEFkZGl0aW9uYWwgcmVzdWx0cyBtYXlcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYmUgZmV0Y2hlZCBieSB0aGUgdXNlciB0aHJvdWdoIHBhZ2luYXRpb24uXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVzdWx0cyAtIEFycmF5IG9mIHJlc3VsdCBpdGVtcy4gVGhlIGZvcm1hdCBmb3IgdGhlIHJlc3VsdFxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBpdGVtcyBpcyB0aGUgc2FtZSBhcyBmb3IgcGFzc2luZyBsb2NhbCBpdGVtcy5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgb2Zmc2V0IC0gVGhpcyBwcm9wZXJ0eSBpcyBvbmx5IHVzZWQgZm9yIHBhZ2luYXRpb24gYW5kIGluZGljYXRlcyBob3dcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbWFueSByZXN1bHRzIHNob3VsZCBiZSBza2lwcGVkIHdoZW4gcmV0dXJuaW5nIG1vcmUgcmVzdWx0cy5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgc2VsZWN0aXZpdHkgLSBUaGUgU2VsZWN0aXZpdHkgaW5zdGFuY2UgdGhlIHF1ZXJ5IGZ1bmN0aW9uIGlzIHVzZWQgb24uXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgIHRlcm0gLSBUaGUgc2VhcmNoIHRlcm0gdGhlIHVzZXIgaXMgc2VhcmNoaW5nIGZvci4gVW5saWtlIHdpdGggdGhlXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBtYXRjaGVyIGZ1bmN0aW9uLCB0aGUgdGVybSBoYXMgbm90IGJlZW4gcHJvY2Vzc2VkIHVzaW5nXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBTZWxlY3Rpdml0eS50cmFuc2Zvcm1UZXh0KCkuXHJcbiAgICAgKiAgICAgICAgICAgICAgICByZWFkT25seSAtIElmIHRydWUsIGRpc2FibGVzIGFueSBtb2RpZmljYXRpb24gb2YgdGhlIGlucHV0LlxyXG4gICAgICogICAgICAgICAgICAgICAgcmVtb3ZlT25seSAtIElmIHRydWUsIGRpc2FibGVzIGFueSBtb2RpZmljYXRpb24gb2YgdGhlIGlucHV0IGV4Y2VwdCByZW1vdmluZ1xyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgIG9mIHNlbGVjdGVkIGl0ZW1zLlxyXG4gICAgICogICAgICAgICAgICAgICAgc2VhcmNoSW5wdXRMaXN0ZW5lcnMgLSBBcnJheSBvZiBzZWFyY2ggaW5wdXQgbGlzdGVuZXJzLiBCeSBkZWZhdWx0LCB0aGUgZ2xvYmFsXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGFycmF5IFNlbGVjdGl2aXR5LlNlYXJjaElucHV0TGlzdGVuZXJzIGlzIHVzZWQuXHJcbiAgICAgKiAgICAgICAgICAgICAgICBzaG93RHJvcGRvd24gLSBTZXQgdG8gZmFsc2UgaWYgeW91IGRvbid0IHdhbnQgdG8gdXNlIGFueSBkcm9wZG93biAoeW91IGNhblxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgc3RpbGwgb3BlbiBpdCBwcm9ncmFtbWF0aWNhbGx5IHVzaW5nIG9wZW4oKSkuXHJcbiAgICAgKiAgICAgICAgICAgICAgICB0ZW1wbGF0ZXMgLSBPYmplY3Qgd2l0aCBpbnN0YW5jZS1zcGVjaWZpYyB0ZW1wbGF0ZXMgdG8gb3ZlcnJpZGUgdGhlIGdsb2JhbFxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgdGVtcGxhdGVzIGFzc2lnbmVkIHRvIFNlbGVjdGl2aXR5LlRlbXBsYXRlcy5cclxuICAgICAqL1xyXG4gICAgc2V0T3B0aW9uczogZnVuY3Rpb24ob3B0aW9ucykge1xyXG5cclxuICAgICAgICBvcHRpb25zID0gb3B0aW9ucyB8fCB7fTtcclxuXHJcbiAgICAgICAgU2VsZWN0aXZpdHkuT3B0aW9uTGlzdGVuZXJzLmZvckVhY2goZnVuY3Rpb24obGlzdGVuZXIpIHtcclxuICAgICAgICAgICAgbGlzdGVuZXIodGhpcywgb3B0aW9ucyk7XHJcbiAgICAgICAgfS5iaW5kKHRoaXMpKTtcclxuXHJcbiAgICAgICAgJC5leHRlbmQodGhpcy5vcHRpb25zLCBvcHRpb25zKTtcclxuXHJcbiAgICAgICAgdmFyIGFsbG93ZWRUeXBlcyA9ICQuZXh0ZW5kKHtcclxuICAgICAgICAgICAgY2xvc2VPblNlbGVjdDogJ2Jvb2xlYW4nLFxyXG4gICAgICAgICAgICBkcm9wZG93bjogJ2Z1bmN0aW9ufG51bGwnLFxyXG4gICAgICAgICAgICBpbml0U2VsZWN0aW9uOiAnZnVuY3Rpb258bnVsbCcsXHJcbiAgICAgICAgICAgIG1hdGNoZXI6ICdmdW5jdGlvbnxudWxsJyxcclxuICAgICAgICAgICAgcGxhY2Vob2xkZXI6ICdzdHJpbmcnLFxyXG4gICAgICAgICAgICBwb3NpdGlvbkRyb3Bkb3duOiAnZnVuY3Rpb258bnVsbCcsXHJcbiAgICAgICAgICAgIHF1ZXJ5OiAnZnVuY3Rpb258bnVsbCcsXHJcbiAgICAgICAgICAgIHJlYWRPbmx5OiAnYm9vbGVhbicsXHJcbiAgICAgICAgICAgIHJlbW92ZU9ubHk6ICdib29sZWFuJyxcclxuICAgICAgICAgICAgc2VhcmNoSW5wdXRMaXN0ZW5lcnM6ICdhcnJheSdcclxuICAgICAgICB9LCBvcHRpb25zLmFsbG93ZWRUeXBlcyk7XHJcblxyXG4gICAgICAgICQuZWFjaChvcHRpb25zLCBmdW5jdGlvbihrZXksIHZhbHVlKSB7XHJcbiAgICAgICAgICAgIHZhciB0eXBlID0gYWxsb3dlZFR5cGVzW2tleV07XHJcbiAgICAgICAgICAgIGlmICh0eXBlICYmICF0eXBlLnNwbGl0KCd8Jykuc29tZShmdW5jdGlvbih0eXBlKSB7IHJldHVybiAkLnR5cGUodmFsdWUpID09PSB0eXBlOyB9KSkge1xyXG4gICAgICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKGtleSArICcgbXVzdCBiZSBvZiB0eXBlICcgKyB0eXBlKTtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgc3dpdGNoIChrZXkpIHtcclxuICAgICAgICAgICAgY2FzZSAnaXRlbXMnOlxyXG4gICAgICAgICAgICAgICAgdGhpcy5pdGVtcyA9ICh2YWx1ZSA9PT0gbnVsbCA/IHZhbHVlIDogU2VsZWN0aXZpdHkucHJvY2Vzc0l0ZW1zKHZhbHVlKSk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuXHJcbiAgICAgICAgICAgIGNhc2UgJ21hdGNoZXInOlxyXG4gICAgICAgICAgICAgICAgdGhpcy5tYXRjaGVyID0gdmFsdWU7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuXHJcbiAgICAgICAgICAgIGNhc2UgJ3NlYXJjaElucHV0TGlzdGVuZXJzJzpcclxuICAgICAgICAgICAgICAgIHRoaXMuc2VhcmNoSW5wdXRMaXN0ZW5lcnMgPSB2YWx1ZTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgICAgICAgY2FzZSAndGVtcGxhdGVzJzpcclxuICAgICAgICAgICAgICAgICQuZXh0ZW5kKHRoaXMudGVtcGxhdGVzLCB2YWx1ZSk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0uYmluZCh0aGlzKSk7XHJcblxyXG4gICAgICAgIHRoaXMuZW5hYmxlZCA9ICghdGhpcy5vcHRpb25zLnJlYWRPbmx5ICYmICF0aGlzLm9wdGlvbnMucmVtb3ZlT25seSk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmV0dXJucyB0aGUgcmVzdWx0IG9mIHRoZSBnaXZlbiB0ZW1wbGF0ZS5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gdGVtcGxhdGVOYW1lIE5hbWUgb2YgdGhlIHRlbXBsYXRlIHRvIHByb2Nlc3MuXHJcbiAgICAgKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25zIHRvIHBhc3MgdG8gdGhlIHRlbXBsYXRlLlxyXG4gICAgICpcclxuICAgICAqIEByZXR1cm4gU3RyaW5nIGNvbnRhaW5pbmcgSFRNTC5cclxuICAgICAqL1xyXG4gICAgdGVtcGxhdGU6IGZ1bmN0aW9uKHRlbXBsYXRlTmFtZSwgb3B0aW9ucykge1xyXG5cclxuICAgICAgICB2YXIgdGVtcGxhdGUgPSB0aGlzLnRlbXBsYXRlc1t0ZW1wbGF0ZU5hbWVdO1xyXG4gICAgICAgIGlmICh0ZW1wbGF0ZSkge1xyXG4gICAgICAgICAgICBpZiAoJC50eXBlKHRlbXBsYXRlKSA9PT0gJ2Z1bmN0aW9uJykge1xyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHRlbXBsYXRlKG9wdGlvbnMpO1xyXG4gICAgICAgICAgICB9IGVsc2UgaWYgKHRlbXBsYXRlLnJlbmRlcikge1xyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHRlbXBsYXRlLnJlbmRlcihvcHRpb25zKTtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIHJldHVybiB0ZW1wbGF0ZS50b1N0cmluZygpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKCdVbmtub3duIHRlbXBsYXRlOiAnICsgdGVtcGxhdGVOYW1lKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogVHJpZ2dlcnMgdGhlIGNoYW5nZSBldmVudC5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgZXZlbnQgb2JqZWN0IGF0IGxlYXN0IGNvbnRhaW5zIHRoZSBmb2xsb3dpbmcgcHJvcGVydHk6XHJcbiAgICAgKiB2YWx1ZSAtIFRoZSBuZXcgdmFsdWUgb2YgdGhlIFNlbGVjdGl2aXR5IGluc3RhbmNlLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBPcHRpb25hbCBhZGRpdGlvbmFsIG9wdGlvbnMgYWRkZWQgdG8gdGhlIGV2ZW50IG9iamVjdC5cclxuICAgICAqL1xyXG4gICAgdHJpZ2dlckNoYW5nZTogZnVuY3Rpb24ob3B0aW9ucykge1xyXG5cclxuICAgICAgICB0aGlzLnRyaWdnZXJFdmVudCgnY2hhbmdlJywgJC5leHRlbmQoeyB2YWx1ZTogdGhpcy5fdmFsdWUgfSwgb3B0aW9ucykpO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFRyaWdnZXJzIGFuIGV2ZW50IG9uIHRoZSBpbnN0YW5jZSdzIGVsZW1lbnQuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIE9wdGlvbmFsIGV2ZW50IGRhdGEgdG8gYmUgYWRkZWQgdG8gdGhlIGV2ZW50IG9iamVjdC5cclxuICAgICAqXHJcbiAgICAgKiBAcmV0dXJuIFdoZXRoZXIgdGhlIGRlZmF1bHQgYWN0aW9uIG9mIHRoZSBldmVudCBtYXkgYmUgZXhlY3V0ZWQsIGllLiByZXR1cm5zIGZhbHNlIGlmXHJcbiAgICAgKiAgICAgICAgIHByZXZlbnREZWZhdWx0KCkgaGFzIGJlZW4gY2FsbGVkLlxyXG4gICAgICovXHJcbiAgICB0cmlnZ2VyRXZlbnQ6IGZ1bmN0aW9uKGV2ZW50TmFtZSwgZGF0YSkge1xyXG5cclxuICAgICAgICB2YXIgZXZlbnQgPSAkLkV2ZW50KGV2ZW50TmFtZSwgZGF0YSB8fCB7fSk7XHJcbiAgICAgICAgdGhpcy4kZWwudHJpZ2dlcihldmVudCk7XHJcbiAgICAgICAgcmV0dXJuICFldmVudC5pc0RlZmF1bHRQcmV2ZW50ZWQoKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBTaG9ydGhhbmQgZm9yIHZhbHVlKCkuXHJcbiAgICAgKi9cclxuICAgIHZhbDogZnVuY3Rpb24obmV3VmFsdWUpIHtcclxuXHJcbiAgICAgICAgcmV0dXJuIHRoaXMudmFsdWUobmV3VmFsdWUpO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFZhbGlkYXRlcyBhIHNpbmdsZSBpdGVtLiBUaHJvd3MgYW4gZXhjZXB0aW9uIGlmIHRoZSBpdGVtIGlzIGludmFsaWQuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIGl0ZW0gVGhlIGl0ZW0gdG8gdmFsaWRhdGUuXHJcbiAgICAgKlxyXG4gICAgICogQHJldHVybiBUaGUgdmFsaWRhdGVkIGl0ZW0uIE1heSBkaWZmZXIgZnJvbSB0aGUgaW5wdXQgaXRlbS5cclxuICAgICAqL1xyXG4gICAgdmFsaWRhdGVJdGVtOiBmdW5jdGlvbihpdGVtKSB7XHJcblxyXG4gICAgICAgIGlmIChpdGVtICYmIFNlbGVjdGl2aXR5LmlzVmFsaWRJZChpdGVtLmlkKSAmJiAkLnR5cGUoaXRlbS50ZXh0KSA9PT0gJ3N0cmluZycpIHtcclxuICAgICAgICAgICAgcmV0dXJuIGl0ZW07XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgdGhyb3cgbmV3IEVycm9yKCdJdGVtIHNob3VsZCBoYXZlIGlkIChudW1iZXIgb3Igc3RyaW5nKSBhbmQgdGV4dCAoc3RyaW5nKSBwcm9wZXJ0aWVzJyk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFNldHMgb3IgZ2V0cyB0aGUgdmFsdWUgb2YgdGhlIHNlbGVjdGlvbi5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgdmFsdWUgb2YgdGhlIHNlbGVjdGlvbiBvbmx5IGNvbmNlcm5zIHRoZSBJRHMgb2YgdGhlIHNlbGVjdGlvbiBpdGVtcy4gSWYgeW91IGFyZVxyXG4gICAgICogaW50ZXJlc3RlZCBpbiB0aGUgSURzIGFuZCB0aGUgdGV4dCBsYWJlbHMsIHlvdSBzaG91bGQgdXNlIHRoZSBkYXRhKCkgbWV0aG9kLlxyXG4gICAgICpcclxuICAgICAqIE5vdGUgdGhhdCBpZiBuZWl0aGVyIHRoZSBpdGVtcyBvcHRpb24gbm9yIHRoZSBpbml0U2VsZWN0aW9uIG9wdGlvbiBoYXZlIGJlZW4gc2V0LCBTZWxlY3Rpdml0eVxyXG4gICAgICogd2lsbCBoYXZlIG5vIHdheSB0byBkZXRlcm1pbmUgd2hhdCB0ZXh0IGxhYmVscyBzaG91bGQgYmUgdXNlZCB3aXRoIHRoZSBnaXZlbiBJRHMgaW4gd2hpY2hcclxuICAgICAqIGNhc2UgaXQgd2lsbCBhc3N1bWUgdGhlIHRleHQgaXMgZXF1YWwgdG8gdGhlIElELiBUaGlzIGlzIHVzZWZ1bCBpZiB5b3UncmUgd29ya2luZyB3aXRoIHRhZ3MsXHJcbiAgICAgKiBvciBzZWxlY3RpbmcgZS1tYWlsIGFkZHJlc3NlcyBmb3IgaW5zdGFuY2UsIGJ1dCBtYXkgbm90IGFsd2F5cyBiZSB3aGF0IHlvdSB3YW50LlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBuZXdWYWx1ZSBPcHRpb25hbCBuZXcgdmFsdWUgdG8gc2V0LiBGb3IgYSBNdWx0aXBsZVNlbGVjdGl2aXR5IGluc3RhbmNlIHRoZSB2YWx1ZSBtdXN0XHJcbiAgICAgKiAgICAgICAgICAgICAgICAgYmUgYW4gYXJyYXkgb2YgSURzLCBmb3IgYSBTaW5nbGVTZWxlY3Rpdml0eSBpbnN0YW5jZSB0aGUgdmFsdWUgbXVzdCBiZSBhXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgc2luZ2xlIElEIChhIHN0cmluZyBvciBhIG51bWJlcikgb3IgbnVsbCB0byBpbmRpY2F0ZSBubyBpdGVtIGlzIHNlbGVjdGVkLlxyXG4gICAgICogQHBhcmFtIG9wdGlvbnMgT3B0aW9uYWwgb3B0aW9ucyBvYmplY3QuIE1heSBjb250YWluIHRoZSBmb2xsb3dpbmcgcHJvcGVydHk6XHJcbiAgICAgKiAgICAgICAgICAgICAgICB0cmlnZ2VyQ2hhbmdlIC0gU2V0IHRvIGZhbHNlIHRvIHN1cHByZXNzIHRoZSBcImNoYW5nZVwiIGV2ZW50IGJlaW5nIHRyaWdnZXJlZC5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBOb3RlIHRoaXMgd2lsbCBhbHNvIGNhdXNlIHRoZSBVSSB0byBub3QgdXBkYXRlIGF1dG9tYXRpY2FsbHk7XHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgc28geW91IG1heSB3YW50IHRvIGNhbGwgcmVyZW5kZXJTZWxlY3Rpb24oKSBtYW51YWxseSB3aGVuXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgdXNpbmcgdGhpcyBvcHRpb24uXHJcbiAgICAgKlxyXG4gICAgICogQHJldHVybiBJZiBuZXdWYWx1ZSBpcyBvbWl0dGVkLCB0aGlzIG1ldGhvZCByZXR1cm5zIHRoZSBjdXJyZW50IHZhbHVlLlxyXG4gICAgICovXHJcbiAgICB2YWx1ZTogZnVuY3Rpb24obmV3VmFsdWUsIG9wdGlvbnMpIHtcclxuXHJcbiAgICAgICAgb3B0aW9ucyA9IG9wdGlvbnMgfHwge307XHJcblxyXG4gICAgICAgIGlmIChuZXdWYWx1ZSA9PT0gdW5kZWZpbmVkKSB7XHJcbiAgICAgICAgICAgIHJldHVybiB0aGlzLl92YWx1ZTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBuZXdWYWx1ZSA9IHRoaXMudmFsaWRhdGVWYWx1ZShuZXdWYWx1ZSk7XHJcblxyXG4gICAgICAgICAgICB0aGlzLl92YWx1ZSA9IG5ld1ZhbHVlO1xyXG5cclxuICAgICAgICAgICAgaWYgKHRoaXMub3B0aW9ucy5pbml0U2VsZWN0aW9uKSB7XHJcbiAgICAgICAgICAgICAgICB0aGlzLm9wdGlvbnMuaW5pdFNlbGVjdGlvbihuZXdWYWx1ZSwgZnVuY3Rpb24oZGF0YSkge1xyXG4gICAgICAgICAgICAgICAgICAgIGlmICh0aGlzLl92YWx1ZSA9PT0gbmV3VmFsdWUpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdGhpcy5fZGF0YSA9IHRoaXMudmFsaWRhdGVEYXRhKGRhdGEpO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgaWYgKG9wdGlvbnMudHJpZ2dlckNoYW5nZSAhPT0gZmFsc2UpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHRoaXMudHJpZ2dlckNoYW5nZSgpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgfS5iaW5kKHRoaXMpKTtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuX2RhdGEgPSB0aGlzLmdldERhdGFGb3JWYWx1ZShuZXdWYWx1ZSk7XHJcblxyXG4gICAgICAgICAgICAgICAgaWYgKG9wdGlvbnMudHJpZ2dlckNoYW5nZSAhPT0gZmFsc2UpIHtcclxuICAgICAgICAgICAgICAgICAgICB0aGlzLnRyaWdnZXJDaGFuZ2UoKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfY2xvc2VkOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgdGhpcy5kcm9wZG93biA9IG51bGw7XHJcblxyXG4gICAgICAgIHRoaXMuJGVsLmNoaWxkcmVuKCkudG9nZ2xlQ2xhc3MoJ29wZW4nLCBmYWxzZSk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX2dldEl0ZW1JZDogZnVuY3Rpb24oZWxlbWVudE9yRXZlbnQpIHtcclxuXHJcbiAgICAgICAgLy8gcmV0dXJucyB0aGUgaXRlbSBJRCByZWxhdGVkIHRvIGFuIGVsZW1lbnQgb3IgZXZlbnQgdGFyZ2V0LlxyXG4gICAgICAgIC8vIElEcyBjYW4gYmUgZWl0aGVyIG51bWJlcnMgb3Igc3RyaW5ncywgYnV0IGF0dHJpYnV0ZSB2YWx1ZXMgYXJlIGFsd2F5cyBzdHJpbmdzLCBzbyB3ZVxyXG4gICAgICAgIC8vIHdpbGwgaGF2ZSB0byBmaW5kIG91dCB3aGV0aGVyIHRoZSBpdGVtIElEIG91Z2h0IHRvIGJlIGEgbnVtYmVyIG9yIHN0cmluZyBvdXJzZWx2ZXMuXHJcbiAgICAgICAgLy8gJC5mbi5kYXRhKCkgaXMgYSBiaXQgb3ZlcnplYWxvdXMgZm9yIG91ciBjYXNlLCBiZWNhdXNlIGl0IHJldHVybnMgYSBudW1iZXIgd2hlbmV2ZXIgdGhlXHJcbiAgICAgICAgLy8gYXR0cmlidXRlIHZhbHVlIGNhbiBiZSBwYXJzZWQgYXMgYSBudW1iZXIuIGhvd2V2ZXIsIGl0IGlzIHBvc3NpYmxlIGFuIGl0ZW0gaGFkIGFuIElEXHJcbiAgICAgICAgLy8gd2hpY2ggaXMgYSBzdHJpbmcgYnV0IHdoaWNoIGlzIHBhcnNlYWJsZSBhcyBudW1iZXIsIGluIHdoaWNoIGNhc2Ugd2UgdmVyaWZ5IGlmIHRoZSBJRFxyXG4gICAgICAgIC8vIGFzIG51bWJlciBpcyBhY3R1YWxseSBmb3VuZCBhbW9uZyB0aGUgZGF0YSBvciByZXN1bHRzLiBpZiBpdCBpc24ndCwgd2UgYXNzdW1lIGl0IHdhc1xyXG4gICAgICAgIC8vIHN1cHBvc2VkIHRvIGJlIGEgc3RyaW5nIGFmdGVyIGFsbC4uLlxyXG5cclxuICAgICAgICB2YXIgJGVsZW1lbnQ7XHJcbiAgICAgICAgaWYgKGVsZW1lbnRPckV2ZW50LnRhcmdldCkge1xyXG4gICAgICAgICAgICAkZWxlbWVudCA9ICQoZWxlbWVudE9yRXZlbnQudGFyZ2V0KS5jbG9zZXN0KCdbZGF0YS1pdGVtLWlkXScpO1xyXG4gICAgICAgIH0gZWxzZSBpZiAoZWxlbWVudE9yRXZlbnQubGVuZ3RoKSB7XHJcbiAgICAgICAgICAgICRlbGVtZW50ID0gZWxlbWVudE9yRXZlbnQ7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgJGVsZW1lbnQgPSAkKGVsZW1lbnRPckV2ZW50KTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHZhciBpZCA9ICRlbGVtZW50LmRhdGEoJ2l0ZW0taWQnKTtcclxuICAgICAgICBpZiAoJC50eXBlKGlkKSA9PT0gJ3N0cmluZycpIHtcclxuICAgICAgICAgICAgcmV0dXJuIGlkO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIGlmIChTZWxlY3Rpdml0eS5maW5kQnlJZCh0aGlzLl9kYXRhIHx8IFtdLCBpZCkpIHtcclxuICAgICAgICAgICAgICAgIHJldHVybiBpZDtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIHZhciBkcm9wZG93biA9IHRoaXMuZHJvcGRvd247XHJcbiAgICAgICAgICAgICAgICB3aGlsZSAoZHJvcGRvd24pIHtcclxuICAgICAgICAgICAgICAgICAgICBpZiAoU2VsZWN0aXZpdHkuZmluZE5lc3RlZEJ5SWQoZHJvcGRvd24ucmVzdWx0cywgaWQpKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiBpZDtcclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgLy8gRklYTUU6IHJlZmVyZW5jZSB0byBzdWJtZW51IGRvZXNuJ3QgYmVsb25nIGluIGJhc2UgbW9kdWxlXHJcbiAgICAgICAgICAgICAgICAgICAgZHJvcGRvd24gPSBkcm9wZG93bi5zdWJtZW51O1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuICcnICsgaWQ7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX21vdXNlb3V0OiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgdGhpcy4kZWwuY2hpbGRyZW4oKS50b2dnbGVDbGFzcygnaG92ZXInLCBmYWxzZSk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX21vdXNlb3ZlcjogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHRoaXMuJGVsLmNoaWxkcmVuKCkudG9nZ2xlQ2xhc3MoJ2hvdmVyJywgdHJ1ZSk7XHJcbiAgICB9XHJcblxyXG59KTtcclxuXHJcbi8qKlxyXG4gKiBEcm9wZG93biBjbGFzcyB0byB1c2UgZm9yIGRpc3BsYXlpbmcgZHJvcGRvd25zLlxyXG4gKlxyXG4gKiBUaGUgZGVmYXVsdCBpbXBsZW1lbnRhdGlvbiBvZiBhIGRyb3Bkb3duIGlzIGRlZmluZWQgaW4gdGhlIHNlbGVjdGl2aXR5LWRyb3Bkb3duIG1vZHVsZS5cclxuICovXHJcblNlbGVjdGl2aXR5LkRyb3Bkb3duID0gbnVsbDtcclxuXHJcbi8qKlxyXG4gKiBNYXBwaW5nIG9mIGlucHV0IHR5cGVzLlxyXG4gKi9cclxuU2VsZWN0aXZpdHkuSW5wdXRUeXBlcyA9IHt9O1xyXG5cclxuLyoqXHJcbiAqIEFycmF5IG9mIG9wdGlvbiBsaXN0ZW5lcnMuXHJcbiAqXHJcbiAqIE9wdGlvbiBsaXN0ZW5lcnMgYXJlIGludm9rZWQgd2hlbiBzZXRPcHRpb25zKCkgaXMgY2FsbGVkLiBFdmVyeSBsaXN0ZW5lciByZWNlaXZlcyB0d28gYXJndW1lbnRzOlxyXG4gKlxyXG4gKiBzZWxlY3Rpdml0eSAtIFRoZSBTZWxlY3Rpdml0eSBpbnN0YW5jZS5cclxuICogb3B0aW9ucyAtIFRoZSBvcHRpb25zIHRoYXQgYXJlIGFib3V0IHRvIGJlIHNldC4gVGhlIGxpc3RlbmVyIG1heSBtb2RpZnkgdGhpcyBvcHRpb25zIG9iamVjdC5cclxuICpcclxuICogQW4gZXhhbXBsZSBvZiBhbiBvcHRpb24gbGlzdGVuZXIgaXMgdGhlIHNlbGVjdGl2aXR5LXRyYWRpdGlvbmFsIG1vZHVsZS5cclxuICovXHJcblNlbGVjdGl2aXR5Lk9wdGlvbkxpc3RlbmVycyA9IFtdO1xyXG5cclxuLyoqXHJcbiAqIEFycmF5IG9mIHNlYXJjaCBpbnB1dCBsaXN0ZW5lcnMuXHJcbiAqXHJcbiAqIFNlYXJjaCBpbnB1dCBsaXN0ZW5lcnMgYXJlIGludm9rZWQgd2hlbiBpbml0U2VhcmNoSW5wdXQoKSBpcyBjYWxsZWQgKHR5cGljYWxseSByaWdodCBhZnRlciB0aGVcclxuICogc2VhcmNoIGlucHV0IGlzIGNyZWF0ZWQpLiBFdmVyeSBsaXN0ZW5lciByZWNlaXZlcyB0d28gYXJndW1lbnRzOlxyXG4gKlxyXG4gKiBzZWxlY3Rpdml0eSAtIFRoZSBTZWxlY3Rpdml0eSBpbnN0YW5jZS5cclxuICogJGlucHV0IC0galF1ZXJ5IGNvbnRhaW5lciB3aXRoIHRoZSBzZWFyY2ggaW5wdXQuXHJcbiAqXHJcbiAqIEFuIGV4YW1wbGUgb2YgYSBzZWFyY2ggaW5wdXQgbGlzdGVuZXIgaXMgdGhlIHNlbGVjdGl2aXR5LWtleWJvYXJkIG1vZHVsZS5cclxuICovXHJcblNlbGVjdGl2aXR5LlNlYXJjaElucHV0TGlzdGVuZXJzID0gW107XHJcblxyXG4vKipcclxuICogTWFwcGluZyB3aXRoIHRlbXBsYXRlcyB0byB1c2UgZm9yIHJlbmRlcmluZyBzZWxlY3QgYm94ZXMgYW5kIGRyb3Bkb3ducy4gU2VlXHJcbiAqIHNlbGVjdGl2aXR5LXRlbXBsYXRlcy5qcyBmb3IgYSB1c2VmdWwgc2V0IG9mIGRlZmF1bHQgdGVtcGxhdGVzLCBhcyB3ZWxsIGFzIGZvciBkb2N1bWVudGF0aW9uIG9mXHJcbiAqIHRoZSBpbmRpdmlkdWFsIHRlbXBsYXRlcy5cclxuICovXHJcblNlbGVjdGl2aXR5LlRlbXBsYXRlcyA9IHt9O1xyXG5cclxuLyoqXHJcbiAqIEZpbmRzIGFuIGl0ZW0gaW4gdGhlIGdpdmVuIGFycmF5IHdpdGggdGhlIHNwZWNpZmllZCBJRC5cclxuICpcclxuICogQHBhcmFtIGFycmF5IEFycmF5IHRvIHNlYXJjaCBpbi5cclxuICogQHBhcmFtIGlkIElEIHRvIHNlYXJjaCBmb3IuXHJcbiAqXHJcbiAqIEByZXR1cm4gVGhlIGl0ZW0gaW4gdGhlIGFycmF5IHdpdGggdGhlIGdpdmVuIElELCBvciBudWxsIGlmIHRoZSBpdGVtIHdhcyBub3QgZm91bmQuXHJcbiAqL1xyXG5TZWxlY3Rpdml0eS5maW5kQnlJZCA9IGZ1bmN0aW9uKGFycmF5LCBpZCkge1xyXG5cclxuICAgIHZhciBpbmRleCA9IFNlbGVjdGl2aXR5LmZpbmRJbmRleEJ5SWQoYXJyYXksIGlkKTtcclxuICAgIHJldHVybiAoaW5kZXggPiAtMSA/IGFycmF5W2luZGV4XSA6IG51bGwpO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEZpbmRzIHRoZSBpbmRleCBvZiBhbiBpdGVtIGluIHRoZSBnaXZlbiBhcnJheSB3aXRoIHRoZSBzcGVjaWZpZWQgSUQuXHJcbiAqXHJcbiAqIEBwYXJhbSBhcnJheSBBcnJheSB0byBzZWFyY2ggaW4uXHJcbiAqIEBwYXJhbSBpZCBJRCB0byBzZWFyY2ggZm9yLlxyXG4gKlxyXG4gKiBAcmV0dXJuIFRoZSBpbmRleCBvZiB0aGUgaXRlbSBpbiB0aGUgYXJyYXkgd2l0aCB0aGUgZ2l2ZW4gSUQsIG9yIC0xIGlmIHRoZSBpdGVtIHdhcyBub3QgZm91bmQuXHJcbiAqL1xyXG5TZWxlY3Rpdml0eS5maW5kSW5kZXhCeUlkID0gZnVuY3Rpb24oYXJyYXksIGlkKSB7XHJcblxyXG4gICAgZm9yICh2YXIgaSA9IDAsIGxlbmd0aCA9IGFycmF5Lmxlbmd0aDsgaSA8IGxlbmd0aDsgaSsrKSB7XHJcbiAgICAgICAgaWYgKGFycmF5W2ldLmlkID09PSBpZCkge1xyXG4gICAgICAgICAgICByZXR1cm4gaTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICByZXR1cm4gLTE7XHJcbn07XHJcblxyXG4vKipcclxuICogRmluZHMgYW4gaXRlbSBpbiB0aGUgZ2l2ZW4gYXJyYXkgd2l0aCB0aGUgc3BlY2lmaWVkIElELiBJdGVtcyBpbiB0aGUgYXJyYXkgbWF5IGNvbnRhaW4gJ2NoaWxkcmVuJ1xyXG4gKiBwcm9wZXJ0aWVzIHdoaWNoIGluIHR1cm4gd2lsbCBiZSBzZWFyY2hlZCBmb3IgdGhlIGl0ZW0uXHJcbiAqXHJcbiAqIEBwYXJhbSBhcnJheSBBcnJheSB0byBzZWFyY2ggaW4uXHJcbiAqIEBwYXJhbSBpZCBJRCB0byBzZWFyY2ggZm9yLlxyXG4gKlxyXG4gKiBAcmV0dXJuIFRoZSBpdGVtIGluIHRoZSBhcnJheSB3aXRoIHRoZSBnaXZlbiBJRCwgb3IgbnVsbCBpZiB0aGUgaXRlbSB3YXMgbm90IGZvdW5kLlxyXG4gKi9cclxuU2VsZWN0aXZpdHkuZmluZE5lc3RlZEJ5SWQgPSAgbnVsbCAmJiBmdW5jdGlvbihhcnJheSwgaWQpIHtcclxuXHJcbiAgICBmb3IgKHZhciBpID0gMCwgbGVuZ3RoID0gYXJyYXkubGVuZ3RoOyBpIDwgbGVuZ3RoOyBpKyspIHtcclxuICAgICAgICB2YXIgaXRlbSA9IGFycmF5W2ldO1xyXG4gICAgICAgIGlmIChpdGVtLmlkID09PSBpZCkge1xyXG4gICAgICAgICAgICByZXR1cm4gaXRlbTtcclxuICAgICAgICB9IGVsc2UgaWYgKGl0ZW0uY2hpbGRyZW4pIHtcclxuICAgICAgICAgICAgdmFyIHJlc3VsdCA9IFNlbGVjdGl2aXR5LmZpbmROZXN0ZWRCeUlkKGl0ZW0uY2hpbGRyZW4sIGlkKTtcclxuICAgICAgICAgICAgaWYgKHJlc3VsdCkge1xyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHJlc3VsdDtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIHJldHVybiBudWxsO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFV0aWxpdHkgbWV0aG9kIGZvciBpbmhlcml0aW5nIGFub3RoZXIgY2xhc3MuXHJcbiAqXHJcbiAqIEBwYXJhbSBTdWJDbGFzcyBDb25zdHJ1Y3RvciBmdW5jdGlvbiBvZiB0aGUgc3ViY2xhc3MuXHJcbiAqIEBwYXJhbSBTdXBlckNsYXNzIE9wdGlvbmFsIGNvbnN0cnVjdG9yIGZ1bmN0aW9uIG9mIHRoZSBzdXBlcmNsYXNzLiBJZiBvbWl0dGVkLCBTZWxlY3Rpdml0eSBpc1xyXG4gKiAgICAgICAgICAgICAgICAgICB1c2VkIGFzIHN1cGVyY2xhc3MuXHJcbiAqIEBwYXJhbSBwcm90b3R5cGUgT2JqZWN0IHdpdGggbWV0aG9kcyB5b3Ugd2FudCB0byBhZGQgdG8gdGhlIHN1YmNsYXNzIHByb3RvdHlwZS5cclxuICpcclxuICogQHJldHVybiBBIHV0aWxpdHkgZnVuY3Rpb24gZm9yIGNhbGxpbmcgdGhlIG1ldGhvZHMgb2YgdGhlIHN1cGVyY2xhc3MuIFRoaXMgZnVuY3Rpb24gcmVjZWl2ZXMgdHdvXHJcbiAqICAgICAgICAgYXJndW1lbnRzOiBUaGUgdGhpcyBvYmplY3Qgb24gd2hpY2ggeW91IHdhbnQgdG8gZXhlY3V0ZSB0aGUgbWV0aG9kIGFuZCB0aGUgbmFtZSBvZiB0aGVcclxuICogICAgICAgICBtZXRob2QuIEFueSBhcmd1bWVudHMgcGFzdCB0aG9zZSBhcmUgcGFzc2VkIHRvIHRoZSBzdXBlcmNsYXNzIG1ldGhvZC5cclxuICovXHJcblNlbGVjdGl2aXR5LmluaGVyaXRzID0gZnVuY3Rpb24oU3ViQ2xhc3MsIFN1cGVyQ2xhc3MsIHByb3RvdHlwZSkge1xyXG5cclxuICAgIGlmIChhcmd1bWVudHMubGVuZ3RoID09PSAyKSB7XHJcbiAgICAgICAgcHJvdG90eXBlID0gU3VwZXJDbGFzcztcclxuICAgICAgICBTdXBlckNsYXNzID0gU2VsZWN0aXZpdHk7XHJcbiAgICB9XHJcblxyXG4gICAgU3ViQ2xhc3MucHJvdG90eXBlID0gJC5leHRlbmQoXHJcbiAgICAgICAgT2JqZWN0LmNyZWF0ZShTdXBlckNsYXNzLnByb3RvdHlwZSksXHJcbiAgICAgICAgeyBjb25zdHJ1Y3RvcjogU3ViQ2xhc3MgfSxcclxuICAgICAgICBwcm90b3R5cGVcclxuICAgICk7XHJcblxyXG4gICAgcmV0dXJuIGZ1bmN0aW9uKHNlbGYsIG1ldGhvZE5hbWUpIHtcclxuICAgICAgICBTdXBlckNsYXNzLnByb3RvdHlwZVttZXRob2ROYW1lXS5hcHBseShzZWxmLCBBcnJheS5wcm90b3R5cGUuc2xpY2UuY2FsbChhcmd1bWVudHMsIDIpKTtcclxuICAgIH07XHJcbn07XHJcblxyXG4vKipcclxuICogQ2hlY2tzIHdoZXRoZXIgYSB2YWx1ZSBjYW4gYmUgdXNlZCBhcyBhIHZhbGlkIElEIGZvciBzZWxlY3Rpb24gaXRlbXMuIE9ubHkgbnVtYmVycyBhbmQgc3RyaW5nc1xyXG4gKiBhcmUgYWNjZXB0ZWQgdG8gYmUgdXNlZCBhcyBJRHMuXHJcbiAqXHJcbiAqIEBwYXJhbSBpZCBUaGUgdmFsdWUgdG8gY2hlY2sgd2hldGhlciBpdCBpcyBhIHZhbGlkIElELlxyXG4gKlxyXG4gKiBAcmV0dXJuIHRydWUgaWYgdGhlIHZhbHVlIGlzIGEgdmFsaWQgSUQsIGZhbHNlIG90aGVyd2lzZS5cclxuICovXHJcblNlbGVjdGl2aXR5LmlzVmFsaWRJZCA9IGZ1bmN0aW9uKGlkKSB7XHJcblxyXG4gICAgdmFyIHR5cGUgPSAkLnR5cGUoaWQpO1xyXG4gICAgcmV0dXJuIHR5cGUgPT09ICdudW1iZXInIHx8IHR5cGUgPT09ICdzdHJpbmcnO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIERlY2lkZXMgd2hldGhlciBhIGdpdmVuIGl0ZW0gbWF0Y2hlcyBhIHNlYXJjaCB0ZXJtLiBUaGUgZGVmYXVsdCBpbXBsZW1lbnRhdGlvbiBzaW1wbHlcclxuICogY2hlY2tzIHdoZXRoZXIgdGhlIHRlcm0gaXMgY29udGFpbmVkIHdpdGhpbiB0aGUgaXRlbSdzIHRleHQsIGFmdGVyIHRyYW5zZm9ybWluZyB0aGVtIHVzaW5nXHJcbiAqIHRyYW5zZm9ybVRleHQoKS5cclxuICpcclxuICogQHBhcmFtIGl0ZW0gVGhlIGl0ZW0gdGhhdCBzaG91bGQgbWF0Y2ggdGhlIHNlYXJjaCB0ZXJtLlxyXG4gKiBAcGFyYW0gdGVybSBUaGUgc2VhcmNoIHRlcm0uIE5vdGUgdGhhdCBmb3IgcGVyZm9ybWFuY2UgcmVhc29ucywgdGhlIHRlcm0gaGFzIGFsd2F5cyBiZWVuIGFscmVhZHlcclxuICogICAgICAgICAgICAgcHJvY2Vzc2VkIHVzaW5nIHRyYW5zZm9ybVRleHQoKS5cclxuICpcclxuICogQHJldHVybiB0cnVlIGlmIHRoZSB0ZXh0IG1hdGNoZXMgdGhlIHRlcm0sIGZhbHNlIG90aGVyd2lzZS5cclxuICovXHJcblNlbGVjdGl2aXR5Lm1hdGNoZXIgPSBmdW5jdGlvbihpdGVtLCB0ZXJtKSB7XHJcblxyXG4gICAgdmFyIHJlc3VsdCA9IG51bGw7XHJcbiAgICBpZiAoU2VsZWN0aXZpdHkudHJhbnNmb3JtVGV4dChpdGVtLnRleHQpLmluZGV4T2YodGVybSkgPiAtMSkge1xyXG4gICAgICAgIHJlc3VsdCA9IGl0ZW07XHJcbiAgICB9IGVsc2UgaWYgKGl0ZW0uY2hpbGRyZW4pIHtcclxuICAgICAgICB2YXIgbWF0Y2hpbmdDaGlsZHJlbiA9IGl0ZW0uY2hpbGRyZW4ubWFwKGZ1bmN0aW9uKGNoaWxkKSB7XHJcbiAgICAgICAgICAgIHJldHVybiBTZWxlY3Rpdml0eS5tYXRjaGVyKGNoaWxkLCB0ZXJtKTtcclxuICAgICAgICB9KS5maWx0ZXIoZnVuY3Rpb24oY2hpbGQpIHtcclxuICAgICAgICAgICAgcmV0dXJuICEhY2hpbGQ7XHJcbiAgICAgICAgfSk7XHJcbiAgICAgICAgaWYgKG1hdGNoaW5nQ2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgICAgICAgIHJlc3VsdCA9IHsgaWQ6IGl0ZW0uaWQsIHRleHQ6IGl0ZW0udGV4dCwgY2hpbGRyZW46IG1hdGNoaW5nQ2hpbGRyZW4gfTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICByZXR1cm4gcmVzdWx0O1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIEhlbHBlciBmdW5jdGlvbiBmb3IgcHJvY2Vzc2luZyBpdGVtcy5cclxuICpcclxuICogQHBhcmFtIGl0ZW0gVGhlIGl0ZW0gdG8gcHJvY2VzcywgZWl0aGVyIGFzIG9iamVjdCBjb250YWluaW5nICdpZCcgYW5kICd0ZXh0JyBwcm9wZXJ0aWVzIG9yIGp1c3RcclxuICogICAgICAgICAgICAgYXMgSUQuIFRoZSAnaWQnIHByb3BlcnR5IG9mIGFuIGl0ZW0gaXMgb3B0aW9uYWwgaWYgaXQgaGFzIGEgJ2NoaWxkcmVuJyBwcm9wZXJ0eVxyXG4gKiAgICAgICAgICAgICBjb250YWluaW5nIGFuIGFycmF5IG9mIGl0ZW1zLlxyXG4gKlxyXG4gKiBAcmV0dXJuIE9iamVjdCBjb250YWluaW5nICdpZCcgYW5kICd0ZXh0JyBwcm9wZXJ0aWVzLlxyXG4gKi9cclxuU2VsZWN0aXZpdHkucHJvY2Vzc0l0ZW0gPSBmdW5jdGlvbihpdGVtKSB7XHJcblxyXG4gICAgaWYgKFNlbGVjdGl2aXR5LmlzVmFsaWRJZChpdGVtKSkge1xyXG4gICAgICAgIHJldHVybiB7IGlkOiBpdGVtLCB0ZXh0OiAnJyArIGl0ZW0gfTtcclxuICAgIH0gZWxzZSBpZiAoaXRlbSAmJlxyXG4gICAgICAgICAgICAgICAoU2VsZWN0aXZpdHkuaXNWYWxpZElkKGl0ZW0uaWQpIHx8IGl0ZW0uY2hpbGRyZW4pICYmXHJcbiAgICAgICAgICAgICAgICQudHlwZShpdGVtLnRleHQpID09PSAnc3RyaW5nJykge1xyXG4gICAgICAgIGlmIChpdGVtLmNoaWxkcmVuKSB7XHJcbiAgICAgICAgICAgIGl0ZW0uY2hpbGRyZW4gPSBTZWxlY3Rpdml0eS5wcm9jZXNzSXRlbXMoaXRlbS5jaGlsZHJlbik7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICByZXR1cm4gaXRlbTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgICAgdGhyb3cgbmV3IEVycm9yKCdpbnZhbGlkIGl0ZW0nKTtcclxuICAgIH1cclxufTtcclxuXHJcbi8qKlxyXG4gKiBIZWxwZXIgZnVuY3Rpb24gZm9yIHByb2Nlc3NpbmcgYW4gYXJyYXkgb2YgaXRlbXMuXHJcbiAqXHJcbiAqIEBwYXJhbSBpdGVtcyBBcnJheSBvZiBpdGVtcyB0byBwcm9jZXNzLiBTZWUgcHJvY2Vzc0l0ZW0oKSBmb3IgZGV0YWlscyBhYm91dCBhIHNpbmdsZSBpdGVtLlxyXG4gKlxyXG4gKiBAcmV0dXJuIEFycmF5IHdpdGggaXRlbXMuXHJcbiAqL1xyXG5TZWxlY3Rpdml0eS5wcm9jZXNzSXRlbXMgPSBmdW5jdGlvbihpdGVtcykge1xyXG5cclxuICAgIGlmICgkLnR5cGUoaXRlbXMpID09PSAnYXJyYXknKSB7XHJcbiAgICAgICAgcmV0dXJuIGl0ZW1zLm1hcChTZWxlY3Rpdml0eS5wcm9jZXNzSXRlbSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICAgIHRocm93IG5ldyBFcnJvcignaW52YWxpZCBpdGVtcycpO1xyXG4gICAgfVxyXG59O1xyXG5cclxuLyoqXHJcbiAqIFF1b3RlcyBhIHN0cmluZyBzbyBpdCBjYW4gYmUgdXNlZCBpbiBhIENTUyBhdHRyaWJ1dGUgc2VsZWN0b3IuIEl0IGFkZHMgZG91YmxlIHF1b3RlcyB0byB0aGVcclxuICogc3RyaW5nIGFuZCBlc2NhcGVzIGFsbCBvY2N1cnJlbmNlcyBvZiB0aGUgcXVvdGUgY2hhcmFjdGVyIGluc2lkZSB0aGUgc3RyaW5nLlxyXG4gKlxyXG4gKiBAcGFyYW0gc3RyaW5nIFRoZSBzdHJpbmcgdG8gcXVvdGUuXHJcbiAqXHJcbiAqIEByZXR1cm4gVGhlIHF1b3RlZCBzdHJpbmcuXHJcbiAqL1xyXG5TZWxlY3Rpdml0eS5xdW90ZUNzc0F0dHIgPSBmdW5jdGlvbihzdHJpbmcpIHtcclxuXHJcbiAgICByZXR1cm4gJ1wiJyArICgnJyArIHN0cmluZykucmVwbGFjZSgvXFxcXC9nLCAnXFxcXFxcXFwnKS5yZXBsYWNlKC9cIi9nLCAnXFxcXFwiJykgKyAnXCInO1xyXG59O1xyXG5cclxuLyoqXHJcbiAqIFRyYW5zZm9ybXMgdGV4dCBpbiBvcmRlciB0byBmaW5kIG1hdGNoZXMuIFRoZSBkZWZhdWx0IGltcGxlbWVudGF0aW9uIGNhc3RzIGFsbCBzdHJpbmdzIHRvXHJcbiAqIGxvd2VyLWNhc2Ugc28gdGhhdCBhbnkgbWF0Y2hlcyBmb3VuZCB3aWxsIGJlIGNhc2UtaW5zZW5zaXRpdmUuXHJcbiAqXHJcbiAqIEBwYXJhbSBzdHJpbmcgVGhlIHN0cmluZyB0byB0cmFuc2Zvcm0uXHJcbiAqXHJcbiAqIEByZXR1cm4gVGhlIHRyYW5zZm9ybWVkIHN0cmluZy5cclxuICovXHJcblNlbGVjdGl2aXR5LnRyYW5zZm9ybVRleHQgPSBmdW5jdGlvbihzdHJpbmcpIHtcclxuXHJcbiAgICByZXR1cm4gc3RyaW5nLnRvTG93ZXJDYXNlKCk7XHJcbn07XHJcblxyXG5tb2R1bGUuZXhwb3J0cyA9ICQuZm4uc2VsZWN0aXZpdHkgPSBTZWxlY3Rpdml0eTtcclxuXHJcbn0se1wiMlwiOjIsXCJqcXVlcnlcIjpcImpxdWVyeVwifV0sOTpbZnVuY3Rpb24oX2RlcmVxXyxtb2R1bGUsZXhwb3J0cyl7XHJcbid1c2Ugc3RyaWN0JztcclxuXHJcbnZhciBESUFDUklUSUNTID0ge1xyXG4gICAgJ1xcdTI0QjYnOiAnQScsXHJcbiAgICAnXFx1RkYyMSc6ICdBJyxcclxuICAgICdcXHUwMEMwJzogJ0EnLFxyXG4gICAgJ1xcdTAwQzEnOiAnQScsXHJcbiAgICAnXFx1MDBDMic6ICdBJyxcclxuICAgICdcXHUxRUE2JzogJ0EnLFxyXG4gICAgJ1xcdTFFQTQnOiAnQScsXHJcbiAgICAnXFx1MUVBQSc6ICdBJyxcclxuICAgICdcXHUxRUE4JzogJ0EnLFxyXG4gICAgJ1xcdTAwQzMnOiAnQScsXHJcbiAgICAnXFx1MDEwMCc6ICdBJyxcclxuICAgICdcXHUwMTAyJzogJ0EnLFxyXG4gICAgJ1xcdTFFQjAnOiAnQScsXHJcbiAgICAnXFx1MUVBRSc6ICdBJyxcclxuICAgICdcXHUxRUI0JzogJ0EnLFxyXG4gICAgJ1xcdTFFQjInOiAnQScsXHJcbiAgICAnXFx1MDIyNic6ICdBJyxcclxuICAgICdcXHUwMUUwJzogJ0EnLFxyXG4gICAgJ1xcdTAwQzQnOiAnQScsXHJcbiAgICAnXFx1MDFERSc6ICdBJyxcclxuICAgICdcXHUxRUEyJzogJ0EnLFxyXG4gICAgJ1xcdTAwQzUnOiAnQScsXHJcbiAgICAnXFx1MDFGQSc6ICdBJyxcclxuICAgICdcXHUwMUNEJzogJ0EnLFxyXG4gICAgJ1xcdTAyMDAnOiAnQScsXHJcbiAgICAnXFx1MDIwMic6ICdBJyxcclxuICAgICdcXHUxRUEwJzogJ0EnLFxyXG4gICAgJ1xcdTFFQUMnOiAnQScsXHJcbiAgICAnXFx1MUVCNic6ICdBJyxcclxuICAgICdcXHUxRTAwJzogJ0EnLFxyXG4gICAgJ1xcdTAxMDQnOiAnQScsXHJcbiAgICAnXFx1MDIzQSc6ICdBJyxcclxuICAgICdcXHUyQzZGJzogJ0EnLFxyXG4gICAgJ1xcdUE3MzInOiAnQUEnLFxyXG4gICAgJ1xcdTAwQzYnOiAnQUUnLFxyXG4gICAgJ1xcdTAxRkMnOiAnQUUnLFxyXG4gICAgJ1xcdTAxRTInOiAnQUUnLFxyXG4gICAgJ1xcdUE3MzQnOiAnQU8nLFxyXG4gICAgJ1xcdUE3MzYnOiAnQVUnLFxyXG4gICAgJ1xcdUE3MzgnOiAnQVYnLFxyXG4gICAgJ1xcdUE3M0EnOiAnQVYnLFxyXG4gICAgJ1xcdUE3M0MnOiAnQVknLFxyXG4gICAgJ1xcdTI0QjcnOiAnQicsXHJcbiAgICAnXFx1RkYyMic6ICdCJyxcclxuICAgICdcXHUxRTAyJzogJ0InLFxyXG4gICAgJ1xcdTFFMDQnOiAnQicsXHJcbiAgICAnXFx1MUUwNic6ICdCJyxcclxuICAgICdcXHUwMjQzJzogJ0InLFxyXG4gICAgJ1xcdTAxODInOiAnQicsXHJcbiAgICAnXFx1MDE4MSc6ICdCJyxcclxuICAgICdcXHUyNEI4JzogJ0MnLFxyXG4gICAgJ1xcdUZGMjMnOiAnQycsXHJcbiAgICAnXFx1MDEwNic6ICdDJyxcclxuICAgICdcXHUwMTA4JzogJ0MnLFxyXG4gICAgJ1xcdTAxMEEnOiAnQycsXHJcbiAgICAnXFx1MDEwQyc6ICdDJyxcclxuICAgICdcXHUwMEM3JzogJ0MnLFxyXG4gICAgJ1xcdTFFMDgnOiAnQycsXHJcbiAgICAnXFx1MDE4Nyc6ICdDJyxcclxuICAgICdcXHUwMjNCJzogJ0MnLFxyXG4gICAgJ1xcdUE3M0UnOiAnQycsXHJcbiAgICAnXFx1MjRCOSc6ICdEJyxcclxuICAgICdcXHVGRjI0JzogJ0QnLFxyXG4gICAgJ1xcdTFFMEEnOiAnRCcsXHJcbiAgICAnXFx1MDEwRSc6ICdEJyxcclxuICAgICdcXHUxRTBDJzogJ0QnLFxyXG4gICAgJ1xcdTFFMTAnOiAnRCcsXHJcbiAgICAnXFx1MUUxMic6ICdEJyxcclxuICAgICdcXHUxRTBFJzogJ0QnLFxyXG4gICAgJ1xcdTAxMTAnOiAnRCcsXHJcbiAgICAnXFx1MDE4Qic6ICdEJyxcclxuICAgICdcXHUwMThBJzogJ0QnLFxyXG4gICAgJ1xcdTAxODknOiAnRCcsXHJcbiAgICAnXFx1QTc3OSc6ICdEJyxcclxuICAgICdcXHUwMUYxJzogJ0RaJyxcclxuICAgICdcXHUwMUM0JzogJ0RaJyxcclxuICAgICdcXHUwMUYyJzogJ0R6JyxcclxuICAgICdcXHUwMUM1JzogJ0R6JyxcclxuICAgICdcXHUyNEJBJzogJ0UnLFxyXG4gICAgJ1xcdUZGMjUnOiAnRScsXHJcbiAgICAnXFx1MDBDOCc6ICdFJyxcclxuICAgICdcXHUwMEM5JzogJ0UnLFxyXG4gICAgJ1xcdTAwQ0EnOiAnRScsXHJcbiAgICAnXFx1MUVDMCc6ICdFJyxcclxuICAgICdcXHUxRUJFJzogJ0UnLFxyXG4gICAgJ1xcdTFFQzQnOiAnRScsXHJcbiAgICAnXFx1MUVDMic6ICdFJyxcclxuICAgICdcXHUxRUJDJzogJ0UnLFxyXG4gICAgJ1xcdTAxMTInOiAnRScsXHJcbiAgICAnXFx1MUUxNCc6ICdFJyxcclxuICAgICdcXHUxRTE2JzogJ0UnLFxyXG4gICAgJ1xcdTAxMTQnOiAnRScsXHJcbiAgICAnXFx1MDExNic6ICdFJyxcclxuICAgICdcXHUwMENCJzogJ0UnLFxyXG4gICAgJ1xcdTFFQkEnOiAnRScsXHJcbiAgICAnXFx1MDExQSc6ICdFJyxcclxuICAgICdcXHUwMjA0JzogJ0UnLFxyXG4gICAgJ1xcdTAyMDYnOiAnRScsXHJcbiAgICAnXFx1MUVCOCc6ICdFJyxcclxuICAgICdcXHUxRUM2JzogJ0UnLFxyXG4gICAgJ1xcdTAyMjgnOiAnRScsXHJcbiAgICAnXFx1MUUxQyc6ICdFJyxcclxuICAgICdcXHUwMTE4JzogJ0UnLFxyXG4gICAgJ1xcdTFFMTgnOiAnRScsXHJcbiAgICAnXFx1MUUxQSc6ICdFJyxcclxuICAgICdcXHUwMTkwJzogJ0UnLFxyXG4gICAgJ1xcdTAxOEUnOiAnRScsXHJcbiAgICAnXFx1MjRCQic6ICdGJyxcclxuICAgICdcXHVGRjI2JzogJ0YnLFxyXG4gICAgJ1xcdTFFMUUnOiAnRicsXHJcbiAgICAnXFx1MDE5MSc6ICdGJyxcclxuICAgICdcXHVBNzdCJzogJ0YnLFxyXG4gICAgJ1xcdTI0QkMnOiAnRycsXHJcbiAgICAnXFx1RkYyNyc6ICdHJyxcclxuICAgICdcXHUwMUY0JzogJ0cnLFxyXG4gICAgJ1xcdTAxMUMnOiAnRycsXHJcbiAgICAnXFx1MUUyMCc6ICdHJyxcclxuICAgICdcXHUwMTFFJzogJ0cnLFxyXG4gICAgJ1xcdTAxMjAnOiAnRycsXHJcbiAgICAnXFx1MDFFNic6ICdHJyxcclxuICAgICdcXHUwMTIyJzogJ0cnLFxyXG4gICAgJ1xcdTAxRTQnOiAnRycsXHJcbiAgICAnXFx1MDE5Myc6ICdHJyxcclxuICAgICdcXHVBN0EwJzogJ0cnLFxyXG4gICAgJ1xcdUE3N0QnOiAnRycsXHJcbiAgICAnXFx1QTc3RSc6ICdHJyxcclxuICAgICdcXHUyNEJEJzogJ0gnLFxyXG4gICAgJ1xcdUZGMjgnOiAnSCcsXHJcbiAgICAnXFx1MDEyNCc6ICdIJyxcclxuICAgICdcXHUxRTIyJzogJ0gnLFxyXG4gICAgJ1xcdTFFMjYnOiAnSCcsXHJcbiAgICAnXFx1MDIxRSc6ICdIJyxcclxuICAgICdcXHUxRTI0JzogJ0gnLFxyXG4gICAgJ1xcdTFFMjgnOiAnSCcsXHJcbiAgICAnXFx1MUUyQSc6ICdIJyxcclxuICAgICdcXHUwMTI2JzogJ0gnLFxyXG4gICAgJ1xcdTJDNjcnOiAnSCcsXHJcbiAgICAnXFx1MkM3NSc6ICdIJyxcclxuICAgICdcXHVBNzhEJzogJ0gnLFxyXG4gICAgJ1xcdTI0QkUnOiAnSScsXHJcbiAgICAnXFx1RkYyOSc6ICdJJyxcclxuICAgICdcXHUwMENDJzogJ0knLFxyXG4gICAgJ1xcdTAwQ0QnOiAnSScsXHJcbiAgICAnXFx1MDBDRSc6ICdJJyxcclxuICAgICdcXHUwMTI4JzogJ0knLFxyXG4gICAgJ1xcdTAxMkEnOiAnSScsXHJcbiAgICAnXFx1MDEyQyc6ICdJJyxcclxuICAgICdcXHUwMTMwJzogJ0knLFxyXG4gICAgJ1xcdTAwQ0YnOiAnSScsXHJcbiAgICAnXFx1MUUyRSc6ICdJJyxcclxuICAgICdcXHUxRUM4JzogJ0knLFxyXG4gICAgJ1xcdTAxQ0YnOiAnSScsXHJcbiAgICAnXFx1MDIwOCc6ICdJJyxcclxuICAgICdcXHUwMjBBJzogJ0knLFxyXG4gICAgJ1xcdTFFQ0EnOiAnSScsXHJcbiAgICAnXFx1MDEyRSc6ICdJJyxcclxuICAgICdcXHUxRTJDJzogJ0knLFxyXG4gICAgJ1xcdTAxOTcnOiAnSScsXHJcbiAgICAnXFx1MjRCRic6ICdKJyxcclxuICAgICdcXHVGRjJBJzogJ0onLFxyXG4gICAgJ1xcdTAxMzQnOiAnSicsXHJcbiAgICAnXFx1MDI0OCc6ICdKJyxcclxuICAgICdcXHUyNEMwJzogJ0snLFxyXG4gICAgJ1xcdUZGMkInOiAnSycsXHJcbiAgICAnXFx1MUUzMCc6ICdLJyxcclxuICAgICdcXHUwMUU4JzogJ0snLFxyXG4gICAgJ1xcdTFFMzInOiAnSycsXHJcbiAgICAnXFx1MDEzNic6ICdLJyxcclxuICAgICdcXHUxRTM0JzogJ0snLFxyXG4gICAgJ1xcdTAxOTgnOiAnSycsXHJcbiAgICAnXFx1MkM2OSc6ICdLJyxcclxuICAgICdcXHVBNzQwJzogJ0snLFxyXG4gICAgJ1xcdUE3NDInOiAnSycsXHJcbiAgICAnXFx1QTc0NCc6ICdLJyxcclxuICAgICdcXHVBN0EyJzogJ0snLFxyXG4gICAgJ1xcdTI0QzEnOiAnTCcsXHJcbiAgICAnXFx1RkYyQyc6ICdMJyxcclxuICAgICdcXHUwMTNGJzogJ0wnLFxyXG4gICAgJ1xcdTAxMzknOiAnTCcsXHJcbiAgICAnXFx1MDEzRCc6ICdMJyxcclxuICAgICdcXHUxRTM2JzogJ0wnLFxyXG4gICAgJ1xcdTFFMzgnOiAnTCcsXHJcbiAgICAnXFx1MDEzQic6ICdMJyxcclxuICAgICdcXHUxRTNDJzogJ0wnLFxyXG4gICAgJ1xcdTFFM0EnOiAnTCcsXHJcbiAgICAnXFx1MDE0MSc6ICdMJyxcclxuICAgICdcXHUwMjNEJzogJ0wnLFxyXG4gICAgJ1xcdTJDNjInOiAnTCcsXHJcbiAgICAnXFx1MkM2MCc6ICdMJyxcclxuICAgICdcXHVBNzQ4JzogJ0wnLFxyXG4gICAgJ1xcdUE3NDYnOiAnTCcsXHJcbiAgICAnXFx1QTc4MCc6ICdMJyxcclxuICAgICdcXHUwMUM3JzogJ0xKJyxcclxuICAgICdcXHUwMUM4JzogJ0xqJyxcclxuICAgICdcXHUyNEMyJzogJ00nLFxyXG4gICAgJ1xcdUZGMkQnOiAnTScsXHJcbiAgICAnXFx1MUUzRSc6ICdNJyxcclxuICAgICdcXHUxRTQwJzogJ00nLFxyXG4gICAgJ1xcdTFFNDInOiAnTScsXHJcbiAgICAnXFx1MkM2RSc6ICdNJyxcclxuICAgICdcXHUwMTlDJzogJ00nLFxyXG4gICAgJ1xcdTI0QzMnOiAnTicsXHJcbiAgICAnXFx1RkYyRSc6ICdOJyxcclxuICAgICdcXHUwMUY4JzogJ04nLFxyXG4gICAgJ1xcdTAxNDMnOiAnTicsXHJcbiAgICAnXFx1MDBEMSc6ICdOJyxcclxuICAgICdcXHUxRTQ0JzogJ04nLFxyXG4gICAgJ1xcdTAxNDcnOiAnTicsXHJcbiAgICAnXFx1MUU0Nic6ICdOJyxcclxuICAgICdcXHUwMTQ1JzogJ04nLFxyXG4gICAgJ1xcdTFFNEEnOiAnTicsXHJcbiAgICAnXFx1MUU0OCc6ICdOJyxcclxuICAgICdcXHUwMjIwJzogJ04nLFxyXG4gICAgJ1xcdTAxOUQnOiAnTicsXHJcbiAgICAnXFx1QTc5MCc6ICdOJyxcclxuICAgICdcXHVBN0E0JzogJ04nLFxyXG4gICAgJ1xcdTAxQ0EnOiAnTkonLFxyXG4gICAgJ1xcdTAxQ0InOiAnTmonLFxyXG4gICAgJ1xcdTI0QzQnOiAnTycsXHJcbiAgICAnXFx1RkYyRic6ICdPJyxcclxuICAgICdcXHUwMEQyJzogJ08nLFxyXG4gICAgJ1xcdTAwRDMnOiAnTycsXHJcbiAgICAnXFx1MDBENCc6ICdPJyxcclxuICAgICdcXHUxRUQyJzogJ08nLFxyXG4gICAgJ1xcdTFFRDAnOiAnTycsXHJcbiAgICAnXFx1MUVENic6ICdPJyxcclxuICAgICdcXHUxRUQ0JzogJ08nLFxyXG4gICAgJ1xcdTAwRDUnOiAnTycsXHJcbiAgICAnXFx1MUU0Qyc6ICdPJyxcclxuICAgICdcXHUwMjJDJzogJ08nLFxyXG4gICAgJ1xcdTFFNEUnOiAnTycsXHJcbiAgICAnXFx1MDE0Qyc6ICdPJyxcclxuICAgICdcXHUxRTUwJzogJ08nLFxyXG4gICAgJ1xcdTFFNTInOiAnTycsXHJcbiAgICAnXFx1MDE0RSc6ICdPJyxcclxuICAgICdcXHUwMjJFJzogJ08nLFxyXG4gICAgJ1xcdTAyMzAnOiAnTycsXHJcbiAgICAnXFx1MDBENic6ICdPJyxcclxuICAgICdcXHUwMjJBJzogJ08nLFxyXG4gICAgJ1xcdTFFQ0UnOiAnTycsXHJcbiAgICAnXFx1MDE1MCc6ICdPJyxcclxuICAgICdcXHUwMUQxJzogJ08nLFxyXG4gICAgJ1xcdTAyMEMnOiAnTycsXHJcbiAgICAnXFx1MDIwRSc6ICdPJyxcclxuICAgICdcXHUwMUEwJzogJ08nLFxyXG4gICAgJ1xcdTFFREMnOiAnTycsXHJcbiAgICAnXFx1MUVEQSc6ICdPJyxcclxuICAgICdcXHUxRUUwJzogJ08nLFxyXG4gICAgJ1xcdTFFREUnOiAnTycsXHJcbiAgICAnXFx1MUVFMic6ICdPJyxcclxuICAgICdcXHUxRUNDJzogJ08nLFxyXG4gICAgJ1xcdTFFRDgnOiAnTycsXHJcbiAgICAnXFx1MDFFQSc6ICdPJyxcclxuICAgICdcXHUwMUVDJzogJ08nLFxyXG4gICAgJ1xcdTAwRDgnOiAnTycsXHJcbiAgICAnXFx1MDFGRSc6ICdPJyxcclxuICAgICdcXHUwMTg2JzogJ08nLFxyXG4gICAgJ1xcdTAxOUYnOiAnTycsXHJcbiAgICAnXFx1QTc0QSc6ICdPJyxcclxuICAgICdcXHVBNzRDJzogJ08nLFxyXG4gICAgJ1xcdTAxQTInOiAnT0knLFxyXG4gICAgJ1xcdUE3NEUnOiAnT08nLFxyXG4gICAgJ1xcdTAyMjInOiAnT1UnLFxyXG4gICAgJ1xcdTI0QzUnOiAnUCcsXHJcbiAgICAnXFx1RkYzMCc6ICdQJyxcclxuICAgICdcXHUxRTU0JzogJ1AnLFxyXG4gICAgJ1xcdTFFNTYnOiAnUCcsXHJcbiAgICAnXFx1MDFBNCc6ICdQJyxcclxuICAgICdcXHUyQzYzJzogJ1AnLFxyXG4gICAgJ1xcdUE3NTAnOiAnUCcsXHJcbiAgICAnXFx1QTc1Mic6ICdQJyxcclxuICAgICdcXHVBNzU0JzogJ1AnLFxyXG4gICAgJ1xcdTI0QzYnOiAnUScsXHJcbiAgICAnXFx1RkYzMSc6ICdRJyxcclxuICAgICdcXHVBNzU2JzogJ1EnLFxyXG4gICAgJ1xcdUE3NTgnOiAnUScsXHJcbiAgICAnXFx1MDI0QSc6ICdRJyxcclxuICAgICdcXHUyNEM3JzogJ1InLFxyXG4gICAgJ1xcdUZGMzInOiAnUicsXHJcbiAgICAnXFx1MDE1NCc6ICdSJyxcclxuICAgICdcXHUxRTU4JzogJ1InLFxyXG4gICAgJ1xcdTAxNTgnOiAnUicsXHJcbiAgICAnXFx1MDIxMCc6ICdSJyxcclxuICAgICdcXHUwMjEyJzogJ1InLFxyXG4gICAgJ1xcdTFFNUEnOiAnUicsXHJcbiAgICAnXFx1MUU1Qyc6ICdSJyxcclxuICAgICdcXHUwMTU2JzogJ1InLFxyXG4gICAgJ1xcdTFFNUUnOiAnUicsXHJcbiAgICAnXFx1MDI0Qyc6ICdSJyxcclxuICAgICdcXHUyQzY0JzogJ1InLFxyXG4gICAgJ1xcdUE3NUEnOiAnUicsXHJcbiAgICAnXFx1QTdBNic6ICdSJyxcclxuICAgICdcXHVBNzgyJzogJ1InLFxyXG4gICAgJ1xcdTI0QzgnOiAnUycsXHJcbiAgICAnXFx1RkYzMyc6ICdTJyxcclxuICAgICdcXHUxRTlFJzogJ1MnLFxyXG4gICAgJ1xcdTAxNUEnOiAnUycsXHJcbiAgICAnXFx1MUU2NCc6ICdTJyxcclxuICAgICdcXHUwMTVDJzogJ1MnLFxyXG4gICAgJ1xcdTFFNjAnOiAnUycsXHJcbiAgICAnXFx1MDE2MCc6ICdTJyxcclxuICAgICdcXHUxRTY2JzogJ1MnLFxyXG4gICAgJ1xcdTFFNjInOiAnUycsXHJcbiAgICAnXFx1MUU2OCc6ICdTJyxcclxuICAgICdcXHUwMjE4JzogJ1MnLFxyXG4gICAgJ1xcdTAxNUUnOiAnUycsXHJcbiAgICAnXFx1MkM3RSc6ICdTJyxcclxuICAgICdcXHVBN0E4JzogJ1MnLFxyXG4gICAgJ1xcdUE3ODQnOiAnUycsXHJcbiAgICAnXFx1MjRDOSc6ICdUJyxcclxuICAgICdcXHVGRjM0JzogJ1QnLFxyXG4gICAgJ1xcdTFFNkEnOiAnVCcsXHJcbiAgICAnXFx1MDE2NCc6ICdUJyxcclxuICAgICdcXHUxRTZDJzogJ1QnLFxyXG4gICAgJ1xcdTAyMUEnOiAnVCcsXHJcbiAgICAnXFx1MDE2Mic6ICdUJyxcclxuICAgICdcXHUxRTcwJzogJ1QnLFxyXG4gICAgJ1xcdTFFNkUnOiAnVCcsXHJcbiAgICAnXFx1MDE2Nic6ICdUJyxcclxuICAgICdcXHUwMUFDJzogJ1QnLFxyXG4gICAgJ1xcdTAxQUUnOiAnVCcsXHJcbiAgICAnXFx1MDIzRSc6ICdUJyxcclxuICAgICdcXHVBNzg2JzogJ1QnLFxyXG4gICAgJ1xcdUE3MjgnOiAnVFonLFxyXG4gICAgJ1xcdTI0Q0EnOiAnVScsXHJcbiAgICAnXFx1RkYzNSc6ICdVJyxcclxuICAgICdcXHUwMEQ5JzogJ1UnLFxyXG4gICAgJ1xcdTAwREEnOiAnVScsXHJcbiAgICAnXFx1MDBEQic6ICdVJyxcclxuICAgICdcXHUwMTY4JzogJ1UnLFxyXG4gICAgJ1xcdTFFNzgnOiAnVScsXHJcbiAgICAnXFx1MDE2QSc6ICdVJyxcclxuICAgICdcXHUxRTdBJzogJ1UnLFxyXG4gICAgJ1xcdTAxNkMnOiAnVScsXHJcbiAgICAnXFx1MDBEQyc6ICdVJyxcclxuICAgICdcXHUwMURCJzogJ1UnLFxyXG4gICAgJ1xcdTAxRDcnOiAnVScsXHJcbiAgICAnXFx1MDFENSc6ICdVJyxcclxuICAgICdcXHUwMUQ5JzogJ1UnLFxyXG4gICAgJ1xcdTFFRTYnOiAnVScsXHJcbiAgICAnXFx1MDE2RSc6ICdVJyxcclxuICAgICdcXHUwMTcwJzogJ1UnLFxyXG4gICAgJ1xcdTAxRDMnOiAnVScsXHJcbiAgICAnXFx1MDIxNCc6ICdVJyxcclxuICAgICdcXHUwMjE2JzogJ1UnLFxyXG4gICAgJ1xcdTAxQUYnOiAnVScsXHJcbiAgICAnXFx1MUVFQSc6ICdVJyxcclxuICAgICdcXHUxRUU4JzogJ1UnLFxyXG4gICAgJ1xcdTFFRUUnOiAnVScsXHJcbiAgICAnXFx1MUVFQyc6ICdVJyxcclxuICAgICdcXHUxRUYwJzogJ1UnLFxyXG4gICAgJ1xcdTFFRTQnOiAnVScsXHJcbiAgICAnXFx1MUU3Mic6ICdVJyxcclxuICAgICdcXHUwMTcyJzogJ1UnLFxyXG4gICAgJ1xcdTFFNzYnOiAnVScsXHJcbiAgICAnXFx1MUU3NCc6ICdVJyxcclxuICAgICdcXHUwMjQ0JzogJ1UnLFxyXG4gICAgJ1xcdTI0Q0InOiAnVicsXHJcbiAgICAnXFx1RkYzNic6ICdWJyxcclxuICAgICdcXHUxRTdDJzogJ1YnLFxyXG4gICAgJ1xcdTFFN0UnOiAnVicsXHJcbiAgICAnXFx1MDFCMic6ICdWJyxcclxuICAgICdcXHVBNzVFJzogJ1YnLFxyXG4gICAgJ1xcdTAyNDUnOiAnVicsXHJcbiAgICAnXFx1QTc2MCc6ICdWWScsXHJcbiAgICAnXFx1MjRDQyc6ICdXJyxcclxuICAgICdcXHVGRjM3JzogJ1cnLFxyXG4gICAgJ1xcdTFFODAnOiAnVycsXHJcbiAgICAnXFx1MUU4Mic6ICdXJyxcclxuICAgICdcXHUwMTc0JzogJ1cnLFxyXG4gICAgJ1xcdTFFODYnOiAnVycsXHJcbiAgICAnXFx1MUU4NCc6ICdXJyxcclxuICAgICdcXHUxRTg4JzogJ1cnLFxyXG4gICAgJ1xcdTJDNzInOiAnVycsXHJcbiAgICAnXFx1MjRDRCc6ICdYJyxcclxuICAgICdcXHVGRjM4JzogJ1gnLFxyXG4gICAgJ1xcdTFFOEEnOiAnWCcsXHJcbiAgICAnXFx1MUU4Qyc6ICdYJyxcclxuICAgICdcXHUyNENFJzogJ1knLFxyXG4gICAgJ1xcdUZGMzknOiAnWScsXHJcbiAgICAnXFx1MUVGMic6ICdZJyxcclxuICAgICdcXHUwMEREJzogJ1knLFxyXG4gICAgJ1xcdTAxNzYnOiAnWScsXHJcbiAgICAnXFx1MUVGOCc6ICdZJyxcclxuICAgICdcXHUwMjMyJzogJ1knLFxyXG4gICAgJ1xcdTFFOEUnOiAnWScsXHJcbiAgICAnXFx1MDE3OCc6ICdZJyxcclxuICAgICdcXHUxRUY2JzogJ1knLFxyXG4gICAgJ1xcdTFFRjQnOiAnWScsXHJcbiAgICAnXFx1MDFCMyc6ICdZJyxcclxuICAgICdcXHUwMjRFJzogJ1knLFxyXG4gICAgJ1xcdTFFRkUnOiAnWScsXHJcbiAgICAnXFx1MjRDRic6ICdaJyxcclxuICAgICdcXHVGRjNBJzogJ1onLFxyXG4gICAgJ1xcdTAxNzknOiAnWicsXHJcbiAgICAnXFx1MUU5MCc6ICdaJyxcclxuICAgICdcXHUwMTdCJzogJ1onLFxyXG4gICAgJ1xcdTAxN0QnOiAnWicsXHJcbiAgICAnXFx1MUU5Mic6ICdaJyxcclxuICAgICdcXHUxRTk0JzogJ1onLFxyXG4gICAgJ1xcdTAxQjUnOiAnWicsXHJcbiAgICAnXFx1MDIyNCc6ICdaJyxcclxuICAgICdcXHUyQzdGJzogJ1onLFxyXG4gICAgJ1xcdTJDNkInOiAnWicsXHJcbiAgICAnXFx1QTc2Mic6ICdaJyxcclxuICAgICdcXHUyNEQwJzogJ2EnLFxyXG4gICAgJ1xcdUZGNDEnOiAnYScsXHJcbiAgICAnXFx1MUU5QSc6ICdhJyxcclxuICAgICdcXHUwMEUwJzogJ2EnLFxyXG4gICAgJ1xcdTAwRTEnOiAnYScsXHJcbiAgICAnXFx1MDBFMic6ICdhJyxcclxuICAgICdcXHUxRUE3JzogJ2EnLFxyXG4gICAgJ1xcdTFFQTUnOiAnYScsXHJcbiAgICAnXFx1MUVBQic6ICdhJyxcclxuICAgICdcXHUxRUE5JzogJ2EnLFxyXG4gICAgJ1xcdTAwRTMnOiAnYScsXHJcbiAgICAnXFx1MDEwMSc6ICdhJyxcclxuICAgICdcXHUwMTAzJzogJ2EnLFxyXG4gICAgJ1xcdTFFQjEnOiAnYScsXHJcbiAgICAnXFx1MUVBRic6ICdhJyxcclxuICAgICdcXHUxRUI1JzogJ2EnLFxyXG4gICAgJ1xcdTFFQjMnOiAnYScsXHJcbiAgICAnXFx1MDIyNyc6ICdhJyxcclxuICAgICdcXHUwMUUxJzogJ2EnLFxyXG4gICAgJ1xcdTAwRTQnOiAnYScsXHJcbiAgICAnXFx1MDFERic6ICdhJyxcclxuICAgICdcXHUxRUEzJzogJ2EnLFxyXG4gICAgJ1xcdTAwRTUnOiAnYScsXHJcbiAgICAnXFx1MDFGQic6ICdhJyxcclxuICAgICdcXHUwMUNFJzogJ2EnLFxyXG4gICAgJ1xcdTAyMDEnOiAnYScsXHJcbiAgICAnXFx1MDIwMyc6ICdhJyxcclxuICAgICdcXHUxRUExJzogJ2EnLFxyXG4gICAgJ1xcdTFFQUQnOiAnYScsXHJcbiAgICAnXFx1MUVCNyc6ICdhJyxcclxuICAgICdcXHUxRTAxJzogJ2EnLFxyXG4gICAgJ1xcdTAxMDUnOiAnYScsXHJcbiAgICAnXFx1MkM2NSc6ICdhJyxcclxuICAgICdcXHUwMjUwJzogJ2EnLFxyXG4gICAgJ1xcdUE3MzMnOiAnYWEnLFxyXG4gICAgJ1xcdTAwRTYnOiAnYWUnLFxyXG4gICAgJ1xcdTAxRkQnOiAnYWUnLFxyXG4gICAgJ1xcdTAxRTMnOiAnYWUnLFxyXG4gICAgJ1xcdUE3MzUnOiAnYW8nLFxyXG4gICAgJ1xcdUE3MzcnOiAnYXUnLFxyXG4gICAgJ1xcdUE3MzknOiAnYXYnLFxyXG4gICAgJ1xcdUE3M0InOiAnYXYnLFxyXG4gICAgJ1xcdUE3M0QnOiAnYXknLFxyXG4gICAgJ1xcdTI0RDEnOiAnYicsXHJcbiAgICAnXFx1RkY0Mic6ICdiJyxcclxuICAgICdcXHUxRTAzJzogJ2InLFxyXG4gICAgJ1xcdTFFMDUnOiAnYicsXHJcbiAgICAnXFx1MUUwNyc6ICdiJyxcclxuICAgICdcXHUwMTgwJzogJ2InLFxyXG4gICAgJ1xcdTAxODMnOiAnYicsXHJcbiAgICAnXFx1MDI1Myc6ICdiJyxcclxuICAgICdcXHUyNEQyJzogJ2MnLFxyXG4gICAgJ1xcdUZGNDMnOiAnYycsXHJcbiAgICAnXFx1MDEwNyc6ICdjJyxcclxuICAgICdcXHUwMTA5JzogJ2MnLFxyXG4gICAgJ1xcdTAxMEInOiAnYycsXHJcbiAgICAnXFx1MDEwRCc6ICdjJyxcclxuICAgICdcXHUwMEU3JzogJ2MnLFxyXG4gICAgJ1xcdTFFMDknOiAnYycsXHJcbiAgICAnXFx1MDE4OCc6ICdjJyxcclxuICAgICdcXHUwMjNDJzogJ2MnLFxyXG4gICAgJ1xcdUE3M0YnOiAnYycsXHJcbiAgICAnXFx1MjE4NCc6ICdjJyxcclxuICAgICdcXHUyNEQzJzogJ2QnLFxyXG4gICAgJ1xcdUZGNDQnOiAnZCcsXHJcbiAgICAnXFx1MUUwQic6ICdkJyxcclxuICAgICdcXHUwMTBGJzogJ2QnLFxyXG4gICAgJ1xcdTFFMEQnOiAnZCcsXHJcbiAgICAnXFx1MUUxMSc6ICdkJyxcclxuICAgICdcXHUxRTEzJzogJ2QnLFxyXG4gICAgJ1xcdTFFMEYnOiAnZCcsXHJcbiAgICAnXFx1MDExMSc6ICdkJyxcclxuICAgICdcXHUwMThDJzogJ2QnLFxyXG4gICAgJ1xcdTAyNTYnOiAnZCcsXHJcbiAgICAnXFx1MDI1Nyc6ICdkJyxcclxuICAgICdcXHVBNzdBJzogJ2QnLFxyXG4gICAgJ1xcdTAxRjMnOiAnZHonLFxyXG4gICAgJ1xcdTAxQzYnOiAnZHonLFxyXG4gICAgJ1xcdTI0RDQnOiAnZScsXHJcbiAgICAnXFx1RkY0NSc6ICdlJyxcclxuICAgICdcXHUwMEU4JzogJ2UnLFxyXG4gICAgJ1xcdTAwRTknOiAnZScsXHJcbiAgICAnXFx1MDBFQSc6ICdlJyxcclxuICAgICdcXHUxRUMxJzogJ2UnLFxyXG4gICAgJ1xcdTFFQkYnOiAnZScsXHJcbiAgICAnXFx1MUVDNSc6ICdlJyxcclxuICAgICdcXHUxRUMzJzogJ2UnLFxyXG4gICAgJ1xcdTFFQkQnOiAnZScsXHJcbiAgICAnXFx1MDExMyc6ICdlJyxcclxuICAgICdcXHUxRTE1JzogJ2UnLFxyXG4gICAgJ1xcdTFFMTcnOiAnZScsXHJcbiAgICAnXFx1MDExNSc6ICdlJyxcclxuICAgICdcXHUwMTE3JzogJ2UnLFxyXG4gICAgJ1xcdTAwRUInOiAnZScsXHJcbiAgICAnXFx1MUVCQic6ICdlJyxcclxuICAgICdcXHUwMTFCJzogJ2UnLFxyXG4gICAgJ1xcdTAyMDUnOiAnZScsXHJcbiAgICAnXFx1MDIwNyc6ICdlJyxcclxuICAgICdcXHUxRUI5JzogJ2UnLFxyXG4gICAgJ1xcdTFFQzcnOiAnZScsXHJcbiAgICAnXFx1MDIyOSc6ICdlJyxcclxuICAgICdcXHUxRTFEJzogJ2UnLFxyXG4gICAgJ1xcdTAxMTknOiAnZScsXHJcbiAgICAnXFx1MUUxOSc6ICdlJyxcclxuICAgICdcXHUxRTFCJzogJ2UnLFxyXG4gICAgJ1xcdTAyNDcnOiAnZScsXHJcbiAgICAnXFx1MDI1Qic6ICdlJyxcclxuICAgICdcXHUwMUREJzogJ2UnLFxyXG4gICAgJ1xcdTI0RDUnOiAnZicsXHJcbiAgICAnXFx1RkY0Nic6ICdmJyxcclxuICAgICdcXHUxRTFGJzogJ2YnLFxyXG4gICAgJ1xcdTAxOTInOiAnZicsXHJcbiAgICAnXFx1QTc3Qyc6ICdmJyxcclxuICAgICdcXHUyNEQ2JzogJ2cnLFxyXG4gICAgJ1xcdUZGNDcnOiAnZycsXHJcbiAgICAnXFx1MDFGNSc6ICdnJyxcclxuICAgICdcXHUwMTFEJzogJ2cnLFxyXG4gICAgJ1xcdTFFMjEnOiAnZycsXHJcbiAgICAnXFx1MDExRic6ICdnJyxcclxuICAgICdcXHUwMTIxJzogJ2cnLFxyXG4gICAgJ1xcdTAxRTcnOiAnZycsXHJcbiAgICAnXFx1MDEyMyc6ICdnJyxcclxuICAgICdcXHUwMUU1JzogJ2cnLFxyXG4gICAgJ1xcdTAyNjAnOiAnZycsXHJcbiAgICAnXFx1QTdBMSc6ICdnJyxcclxuICAgICdcXHUxRDc5JzogJ2cnLFxyXG4gICAgJ1xcdUE3N0YnOiAnZycsXHJcbiAgICAnXFx1MjRENyc6ICdoJyxcclxuICAgICdcXHVGRjQ4JzogJ2gnLFxyXG4gICAgJ1xcdTAxMjUnOiAnaCcsXHJcbiAgICAnXFx1MUUyMyc6ICdoJyxcclxuICAgICdcXHUxRTI3JzogJ2gnLFxyXG4gICAgJ1xcdTAyMUYnOiAnaCcsXHJcbiAgICAnXFx1MUUyNSc6ICdoJyxcclxuICAgICdcXHUxRTI5JzogJ2gnLFxyXG4gICAgJ1xcdTFFMkInOiAnaCcsXHJcbiAgICAnXFx1MUU5Nic6ICdoJyxcclxuICAgICdcXHUwMTI3JzogJ2gnLFxyXG4gICAgJ1xcdTJDNjgnOiAnaCcsXHJcbiAgICAnXFx1MkM3Nic6ICdoJyxcclxuICAgICdcXHUwMjY1JzogJ2gnLFxyXG4gICAgJ1xcdTAxOTUnOiAnaHYnLFxyXG4gICAgJ1xcdTI0RDgnOiAnaScsXHJcbiAgICAnXFx1RkY0OSc6ICdpJyxcclxuICAgICdcXHUwMEVDJzogJ2knLFxyXG4gICAgJ1xcdTAwRUQnOiAnaScsXHJcbiAgICAnXFx1MDBFRSc6ICdpJyxcclxuICAgICdcXHUwMTI5JzogJ2knLFxyXG4gICAgJ1xcdTAxMkInOiAnaScsXHJcbiAgICAnXFx1MDEyRCc6ICdpJyxcclxuICAgICdcXHUwMEVGJzogJ2knLFxyXG4gICAgJ1xcdTFFMkYnOiAnaScsXHJcbiAgICAnXFx1MUVDOSc6ICdpJyxcclxuICAgICdcXHUwMUQwJzogJ2knLFxyXG4gICAgJ1xcdTAyMDknOiAnaScsXHJcbiAgICAnXFx1MDIwQic6ICdpJyxcclxuICAgICdcXHUxRUNCJzogJ2knLFxyXG4gICAgJ1xcdTAxMkYnOiAnaScsXHJcbiAgICAnXFx1MUUyRCc6ICdpJyxcclxuICAgICdcXHUwMjY4JzogJ2knLFxyXG4gICAgJ1xcdTAxMzEnOiAnaScsXHJcbiAgICAnXFx1MjREOSc6ICdqJyxcclxuICAgICdcXHVGRjRBJzogJ2onLFxyXG4gICAgJ1xcdTAxMzUnOiAnaicsXHJcbiAgICAnXFx1MDFGMCc6ICdqJyxcclxuICAgICdcXHUwMjQ5JzogJ2onLFxyXG4gICAgJ1xcdTI0REEnOiAnaycsXHJcbiAgICAnXFx1RkY0Qic6ICdrJyxcclxuICAgICdcXHUxRTMxJzogJ2snLFxyXG4gICAgJ1xcdTAxRTknOiAnaycsXHJcbiAgICAnXFx1MUUzMyc6ICdrJyxcclxuICAgICdcXHUwMTM3JzogJ2snLFxyXG4gICAgJ1xcdTFFMzUnOiAnaycsXHJcbiAgICAnXFx1MDE5OSc6ICdrJyxcclxuICAgICdcXHUyQzZBJzogJ2snLFxyXG4gICAgJ1xcdUE3NDEnOiAnaycsXHJcbiAgICAnXFx1QTc0Myc6ICdrJyxcclxuICAgICdcXHVBNzQ1JzogJ2snLFxyXG4gICAgJ1xcdUE3QTMnOiAnaycsXHJcbiAgICAnXFx1MjREQic6ICdsJyxcclxuICAgICdcXHVGRjRDJzogJ2wnLFxyXG4gICAgJ1xcdTAxNDAnOiAnbCcsXHJcbiAgICAnXFx1MDEzQSc6ICdsJyxcclxuICAgICdcXHUwMTNFJzogJ2wnLFxyXG4gICAgJ1xcdTFFMzcnOiAnbCcsXHJcbiAgICAnXFx1MUUzOSc6ICdsJyxcclxuICAgICdcXHUwMTNDJzogJ2wnLFxyXG4gICAgJ1xcdTFFM0QnOiAnbCcsXHJcbiAgICAnXFx1MUUzQic6ICdsJyxcclxuICAgICdcXHUwMTdGJzogJ2wnLFxyXG4gICAgJ1xcdTAxNDInOiAnbCcsXHJcbiAgICAnXFx1MDE5QSc6ICdsJyxcclxuICAgICdcXHUwMjZCJzogJ2wnLFxyXG4gICAgJ1xcdTJDNjEnOiAnbCcsXHJcbiAgICAnXFx1QTc0OSc6ICdsJyxcclxuICAgICdcXHVBNzgxJzogJ2wnLFxyXG4gICAgJ1xcdUE3NDcnOiAnbCcsXHJcbiAgICAnXFx1MDFDOSc6ICdsaicsXHJcbiAgICAnXFx1MjREQyc6ICdtJyxcclxuICAgICdcXHVGRjREJzogJ20nLFxyXG4gICAgJ1xcdTFFM0YnOiAnbScsXHJcbiAgICAnXFx1MUU0MSc6ICdtJyxcclxuICAgICdcXHUxRTQzJzogJ20nLFxyXG4gICAgJ1xcdTAyNzEnOiAnbScsXHJcbiAgICAnXFx1MDI2Ric6ICdtJyxcclxuICAgICdcXHUyNEREJzogJ24nLFxyXG4gICAgJ1xcdUZGNEUnOiAnbicsXHJcbiAgICAnXFx1MDFGOSc6ICduJyxcclxuICAgICdcXHUwMTQ0JzogJ24nLFxyXG4gICAgJ1xcdTAwRjEnOiAnbicsXHJcbiAgICAnXFx1MUU0NSc6ICduJyxcclxuICAgICdcXHUwMTQ4JzogJ24nLFxyXG4gICAgJ1xcdTFFNDcnOiAnbicsXHJcbiAgICAnXFx1MDE0Nic6ICduJyxcclxuICAgICdcXHUxRTRCJzogJ24nLFxyXG4gICAgJ1xcdTFFNDknOiAnbicsXHJcbiAgICAnXFx1MDE5RSc6ICduJyxcclxuICAgICdcXHUwMjcyJzogJ24nLFxyXG4gICAgJ1xcdTAxNDknOiAnbicsXHJcbiAgICAnXFx1QTc5MSc6ICduJyxcclxuICAgICdcXHVBN0E1JzogJ24nLFxyXG4gICAgJ1xcdTAxQ0MnOiAnbmonLFxyXG4gICAgJ1xcdTI0REUnOiAnbycsXHJcbiAgICAnXFx1RkY0Ric6ICdvJyxcclxuICAgICdcXHUwMEYyJzogJ28nLFxyXG4gICAgJ1xcdTAwRjMnOiAnbycsXHJcbiAgICAnXFx1MDBGNCc6ICdvJyxcclxuICAgICdcXHUxRUQzJzogJ28nLFxyXG4gICAgJ1xcdTFFRDEnOiAnbycsXHJcbiAgICAnXFx1MUVENyc6ICdvJyxcclxuICAgICdcXHUxRUQ1JzogJ28nLFxyXG4gICAgJ1xcdTAwRjUnOiAnbycsXHJcbiAgICAnXFx1MUU0RCc6ICdvJyxcclxuICAgICdcXHUwMjJEJzogJ28nLFxyXG4gICAgJ1xcdTFFNEYnOiAnbycsXHJcbiAgICAnXFx1MDE0RCc6ICdvJyxcclxuICAgICdcXHUxRTUxJzogJ28nLFxyXG4gICAgJ1xcdTFFNTMnOiAnbycsXHJcbiAgICAnXFx1MDE0Ric6ICdvJyxcclxuICAgICdcXHUwMjJGJzogJ28nLFxyXG4gICAgJ1xcdTAyMzEnOiAnbycsXHJcbiAgICAnXFx1MDBGNic6ICdvJyxcclxuICAgICdcXHUwMjJCJzogJ28nLFxyXG4gICAgJ1xcdTFFQ0YnOiAnbycsXHJcbiAgICAnXFx1MDE1MSc6ICdvJyxcclxuICAgICdcXHUwMUQyJzogJ28nLFxyXG4gICAgJ1xcdTAyMEQnOiAnbycsXHJcbiAgICAnXFx1MDIwRic6ICdvJyxcclxuICAgICdcXHUwMUExJzogJ28nLFxyXG4gICAgJ1xcdTFFREQnOiAnbycsXHJcbiAgICAnXFx1MUVEQic6ICdvJyxcclxuICAgICdcXHUxRUUxJzogJ28nLFxyXG4gICAgJ1xcdTFFREYnOiAnbycsXHJcbiAgICAnXFx1MUVFMyc6ICdvJyxcclxuICAgICdcXHUxRUNEJzogJ28nLFxyXG4gICAgJ1xcdTFFRDknOiAnbycsXHJcbiAgICAnXFx1MDFFQic6ICdvJyxcclxuICAgICdcXHUwMUVEJzogJ28nLFxyXG4gICAgJ1xcdTAwRjgnOiAnbycsXHJcbiAgICAnXFx1MDFGRic6ICdvJyxcclxuICAgICdcXHUwMjU0JzogJ28nLFxyXG4gICAgJ1xcdUE3NEInOiAnbycsXHJcbiAgICAnXFx1QTc0RCc6ICdvJyxcclxuICAgICdcXHUwMjc1JzogJ28nLFxyXG4gICAgJ1xcdTAxQTMnOiAnb2knLFxyXG4gICAgJ1xcdTAyMjMnOiAnb3UnLFxyXG4gICAgJ1xcdUE3NEYnOiAnb28nLFxyXG4gICAgJ1xcdTI0REYnOiAncCcsXHJcbiAgICAnXFx1RkY1MCc6ICdwJyxcclxuICAgICdcXHUxRTU1JzogJ3AnLFxyXG4gICAgJ1xcdTFFNTcnOiAncCcsXHJcbiAgICAnXFx1MDFBNSc6ICdwJyxcclxuICAgICdcXHUxRDdEJzogJ3AnLFxyXG4gICAgJ1xcdUE3NTEnOiAncCcsXHJcbiAgICAnXFx1QTc1Myc6ICdwJyxcclxuICAgICdcXHVBNzU1JzogJ3AnLFxyXG4gICAgJ1xcdTI0RTAnOiAncScsXHJcbiAgICAnXFx1RkY1MSc6ICdxJyxcclxuICAgICdcXHUwMjRCJzogJ3EnLFxyXG4gICAgJ1xcdUE3NTcnOiAncScsXHJcbiAgICAnXFx1QTc1OSc6ICdxJyxcclxuICAgICdcXHUyNEUxJzogJ3InLFxyXG4gICAgJ1xcdUZGNTInOiAncicsXHJcbiAgICAnXFx1MDE1NSc6ICdyJyxcclxuICAgICdcXHUxRTU5JzogJ3InLFxyXG4gICAgJ1xcdTAxNTknOiAncicsXHJcbiAgICAnXFx1MDIxMSc6ICdyJyxcclxuICAgICdcXHUwMjEzJzogJ3InLFxyXG4gICAgJ1xcdTFFNUInOiAncicsXHJcbiAgICAnXFx1MUU1RCc6ICdyJyxcclxuICAgICdcXHUwMTU3JzogJ3InLFxyXG4gICAgJ1xcdTFFNUYnOiAncicsXHJcbiAgICAnXFx1MDI0RCc6ICdyJyxcclxuICAgICdcXHUwMjdEJzogJ3InLFxyXG4gICAgJ1xcdUE3NUInOiAncicsXHJcbiAgICAnXFx1QTdBNyc6ICdyJyxcclxuICAgICdcXHVBNzgzJzogJ3InLFxyXG4gICAgJ1xcdTI0RTInOiAncycsXHJcbiAgICAnXFx1RkY1Myc6ICdzJyxcclxuICAgICdcXHUwMERGJzogJ3MnLFxyXG4gICAgJ1xcdTAxNUInOiAncycsXHJcbiAgICAnXFx1MUU2NSc6ICdzJyxcclxuICAgICdcXHUwMTVEJzogJ3MnLFxyXG4gICAgJ1xcdTFFNjEnOiAncycsXHJcbiAgICAnXFx1MDE2MSc6ICdzJyxcclxuICAgICdcXHUxRTY3JzogJ3MnLFxyXG4gICAgJ1xcdTFFNjMnOiAncycsXHJcbiAgICAnXFx1MUU2OSc6ICdzJyxcclxuICAgICdcXHUwMjE5JzogJ3MnLFxyXG4gICAgJ1xcdTAxNUYnOiAncycsXHJcbiAgICAnXFx1MDIzRic6ICdzJyxcclxuICAgICdcXHVBN0E5JzogJ3MnLFxyXG4gICAgJ1xcdUE3ODUnOiAncycsXHJcbiAgICAnXFx1MUU5Qic6ICdzJyxcclxuICAgICdcXHUyNEUzJzogJ3QnLFxyXG4gICAgJ1xcdUZGNTQnOiAndCcsXHJcbiAgICAnXFx1MUU2Qic6ICd0JyxcclxuICAgICdcXHUxRTk3JzogJ3QnLFxyXG4gICAgJ1xcdTAxNjUnOiAndCcsXHJcbiAgICAnXFx1MUU2RCc6ICd0JyxcclxuICAgICdcXHUwMjFCJzogJ3QnLFxyXG4gICAgJ1xcdTAxNjMnOiAndCcsXHJcbiAgICAnXFx1MUU3MSc6ICd0JyxcclxuICAgICdcXHUxRTZGJzogJ3QnLFxyXG4gICAgJ1xcdTAxNjcnOiAndCcsXHJcbiAgICAnXFx1MDFBRCc6ICd0JyxcclxuICAgICdcXHUwMjg4JzogJ3QnLFxyXG4gICAgJ1xcdTJDNjYnOiAndCcsXHJcbiAgICAnXFx1QTc4Nyc6ICd0JyxcclxuICAgICdcXHVBNzI5JzogJ3R6JyxcclxuICAgICdcXHUyNEU0JzogJ3UnLFxyXG4gICAgJ1xcdUZGNTUnOiAndScsXHJcbiAgICAnXFx1MDBGOSc6ICd1JyxcclxuICAgICdcXHUwMEZBJzogJ3UnLFxyXG4gICAgJ1xcdTAwRkInOiAndScsXHJcbiAgICAnXFx1MDE2OSc6ICd1JyxcclxuICAgICdcXHUxRTc5JzogJ3UnLFxyXG4gICAgJ1xcdTAxNkInOiAndScsXHJcbiAgICAnXFx1MUU3Qic6ICd1JyxcclxuICAgICdcXHUwMTZEJzogJ3UnLFxyXG4gICAgJ1xcdTAwRkMnOiAndScsXHJcbiAgICAnXFx1MDFEQyc6ICd1JyxcclxuICAgICdcXHUwMUQ4JzogJ3UnLFxyXG4gICAgJ1xcdTAxRDYnOiAndScsXHJcbiAgICAnXFx1MDFEQSc6ICd1JyxcclxuICAgICdcXHUxRUU3JzogJ3UnLFxyXG4gICAgJ1xcdTAxNkYnOiAndScsXHJcbiAgICAnXFx1MDE3MSc6ICd1JyxcclxuICAgICdcXHUwMUQ0JzogJ3UnLFxyXG4gICAgJ1xcdTAyMTUnOiAndScsXHJcbiAgICAnXFx1MDIxNyc6ICd1JyxcclxuICAgICdcXHUwMUIwJzogJ3UnLFxyXG4gICAgJ1xcdTFFRUInOiAndScsXHJcbiAgICAnXFx1MUVFOSc6ICd1JyxcclxuICAgICdcXHUxRUVGJzogJ3UnLFxyXG4gICAgJ1xcdTFFRUQnOiAndScsXHJcbiAgICAnXFx1MUVGMSc6ICd1JyxcclxuICAgICdcXHUxRUU1JzogJ3UnLFxyXG4gICAgJ1xcdTFFNzMnOiAndScsXHJcbiAgICAnXFx1MDE3Myc6ICd1JyxcclxuICAgICdcXHUxRTc3JzogJ3UnLFxyXG4gICAgJ1xcdTFFNzUnOiAndScsXHJcbiAgICAnXFx1MDI4OSc6ICd1JyxcclxuICAgICdcXHUyNEU1JzogJ3YnLFxyXG4gICAgJ1xcdUZGNTYnOiAndicsXHJcbiAgICAnXFx1MUU3RCc6ICd2JyxcclxuICAgICdcXHUxRTdGJzogJ3YnLFxyXG4gICAgJ1xcdTAyOEInOiAndicsXHJcbiAgICAnXFx1QTc1Ric6ICd2JyxcclxuICAgICdcXHUwMjhDJzogJ3YnLFxyXG4gICAgJ1xcdUE3NjEnOiAndnknLFxyXG4gICAgJ1xcdTI0RTYnOiAndycsXHJcbiAgICAnXFx1RkY1Nyc6ICd3JyxcclxuICAgICdcXHUxRTgxJzogJ3cnLFxyXG4gICAgJ1xcdTFFODMnOiAndycsXHJcbiAgICAnXFx1MDE3NSc6ICd3JyxcclxuICAgICdcXHUxRTg3JzogJ3cnLFxyXG4gICAgJ1xcdTFFODUnOiAndycsXHJcbiAgICAnXFx1MUU5OCc6ICd3JyxcclxuICAgICdcXHUxRTg5JzogJ3cnLFxyXG4gICAgJ1xcdTJDNzMnOiAndycsXHJcbiAgICAnXFx1MjRFNyc6ICd4JyxcclxuICAgICdcXHVGRjU4JzogJ3gnLFxyXG4gICAgJ1xcdTFFOEInOiAneCcsXHJcbiAgICAnXFx1MUU4RCc6ICd4JyxcclxuICAgICdcXHUyNEU4JzogJ3knLFxyXG4gICAgJ1xcdUZGNTknOiAneScsXHJcbiAgICAnXFx1MUVGMyc6ICd5JyxcclxuICAgICdcXHUwMEZEJzogJ3knLFxyXG4gICAgJ1xcdTAxNzcnOiAneScsXHJcbiAgICAnXFx1MUVGOSc6ICd5JyxcclxuICAgICdcXHUwMjMzJzogJ3knLFxyXG4gICAgJ1xcdTFFOEYnOiAneScsXHJcbiAgICAnXFx1MDBGRic6ICd5JyxcclxuICAgICdcXHUxRUY3JzogJ3knLFxyXG4gICAgJ1xcdTFFOTknOiAneScsXHJcbiAgICAnXFx1MUVGNSc6ICd5JyxcclxuICAgICdcXHUwMUI0JzogJ3knLFxyXG4gICAgJ1xcdTAyNEYnOiAneScsXHJcbiAgICAnXFx1MUVGRic6ICd5JyxcclxuICAgICdcXHUyNEU5JzogJ3onLFxyXG4gICAgJ1xcdUZGNUEnOiAneicsXHJcbiAgICAnXFx1MDE3QSc6ICd6JyxcclxuICAgICdcXHUxRTkxJzogJ3onLFxyXG4gICAgJ1xcdTAxN0MnOiAneicsXHJcbiAgICAnXFx1MDE3RSc6ICd6JyxcclxuICAgICdcXHUxRTkzJzogJ3onLFxyXG4gICAgJ1xcdTFFOTUnOiAneicsXHJcbiAgICAnXFx1MDFCNic6ICd6JyxcclxuICAgICdcXHUwMjI1JzogJ3onLFxyXG4gICAgJ1xcdTAyNDAnOiAneicsXHJcbiAgICAnXFx1MkM2Qyc6ICd6JyxcclxuICAgICdcXHVBNzYzJzogJ3onLFxyXG4gICAgJ1xcdTAzODYnOiAnXFx1MDM5MScsXHJcbiAgICAnXFx1MDM4OCc6ICdcXHUwMzk1JyxcclxuICAgICdcXHUwMzg5JzogJ1xcdTAzOTcnLFxyXG4gICAgJ1xcdTAzOEEnOiAnXFx1MDM5OScsXHJcbiAgICAnXFx1MDNBQSc6ICdcXHUwMzk5JyxcclxuICAgICdcXHUwMzhDJzogJ1xcdTAzOUYnLFxyXG4gICAgJ1xcdTAzOEUnOiAnXFx1MDNBNScsXHJcbiAgICAnXFx1MDNBQic6ICdcXHUwM0E1JyxcclxuICAgICdcXHUwMzhGJzogJ1xcdTAzQTknLFxyXG4gICAgJ1xcdTAzQUMnOiAnXFx1MDNCMScsXHJcbiAgICAnXFx1MDNBRCc6ICdcXHUwM0I1JyxcclxuICAgICdcXHUwM0FFJzogJ1xcdTAzQjcnLFxyXG4gICAgJ1xcdTAzQUYnOiAnXFx1MDNCOScsXHJcbiAgICAnXFx1MDNDQSc6ICdcXHUwM0I5JyxcclxuICAgICdcXHUwMzkwJzogJ1xcdTAzQjknLFxyXG4gICAgJ1xcdTAzQ0MnOiAnXFx1MDNCRicsXHJcbiAgICAnXFx1MDNDRCc6ICdcXHUwM0M1JyxcclxuICAgICdcXHUwM0NCJzogJ1xcdTAzQzUnLFxyXG4gICAgJ1xcdTAzQjAnOiAnXFx1MDNDNScsXHJcbiAgICAnXFx1MDNDOSc6ICdcXHUwM0M5JyxcclxuICAgICdcXHUwM0MyJzogJ1xcdTAzQzMnXHJcbn07XHJcblxyXG52YXIgU2VsZWN0aXZpdHkgPSBfZGVyZXFfKDgpO1xyXG52YXIgcHJldmlvdXNUcmFuc2Zvcm0gPSBTZWxlY3Rpdml0eS50cmFuc2Zvcm1UZXh0O1xyXG5cclxuLyoqXHJcbiAqIEV4dGVuZGVkIHZlcnNpb24gb2YgdGhlIHRyYW5zZm9ybVRleHQoKSBmdW5jdGlvbiB0aGF0IHNpbXBsaWZpZXMgZGlhY3JpdGljcyB0byB0aGVpciBsYXRpbjFcclxuICogY291bnRlcnBhcnRzLlxyXG4gKlxyXG4gKiBOb3RlIHRoYXQgaWYgYWxsIHF1ZXJ5IGZ1bmN0aW9ucyBmZXRjaCB0aGVpciByZXN1bHRzIGZyb20gYSByZW1vdGUgc2VydmVyLCB5b3UgbWF5IG5vdCBuZWVkIHRoaXNcclxuICogZnVuY3Rpb24sIGJlY2F1c2UgaXQgbWFrZXMgc2Vuc2UgdG8gcmVtb3ZlIGRpYWNyaXRpY3Mgc2VydmVyLXNpZGUgaW4gc3VjaCBjYXNlcy5cclxuICovXHJcblNlbGVjdGl2aXR5LnRyYW5zZm9ybVRleHQgPSBmdW5jdGlvbihzdHJpbmcpIHtcclxuICAgIHZhciByZXN1bHQgPSAnJztcclxuICAgIGZvciAodmFyIGkgPSAwLCBsZW5ndGggPSBzdHJpbmcubGVuZ3RoOyBpIDwgbGVuZ3RoOyBpKyspIHtcclxuICAgICAgICB2YXIgY2hhcmFjdGVyID0gc3RyaW5nW2ldO1xyXG4gICAgICAgIHJlc3VsdCArPSBESUFDUklUSUNTW2NoYXJhY3Rlcl0gfHwgY2hhcmFjdGVyO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIHByZXZpb3VzVHJhbnNmb3JtKHJlc3VsdCk7XHJcbn07XHJcblxyXG59LHtcIjhcIjo4fV0sMTA6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG52YXIgJCA9IHdpbmRvdy5qUXVlcnkgfHwgd2luZG93LlplcHRvO1xyXG5cclxudmFyIGRlYm91bmNlID0gX2RlcmVxXygzKTtcclxuXHJcbnZhciBFdmVudERlbGVnYXRvciA9IF9kZXJlcV8oMik7XHJcblxyXG52YXIgU2VsZWN0aXZpdHkgPSBfZGVyZXFfKDgpO1xyXG5cclxuLyoqXHJcbiAqIHNlbGVjdGl2aXR5IERyb3Bkb3duIENvbnN0cnVjdG9yLlxyXG4gKlxyXG4gKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25zIG9iamVjdC4gU2hvdWxkIGhhdmUgdGhlIGZvbGxvd2luZyBwcm9wZXJ0aWVzOlxyXG4gKiAgICAgICAgICAgICAgICBzZWxlY3Rpdml0eSAtIFNlbGVjdGl2aXR5IGluc3RhbmNlIHRvIHNob3cgdGhlIGRyb3Bkb3duIGZvci5cclxuICogICAgICAgICAgICAgICAgc2hvd1NlYXJjaElucHV0IC0gQm9vbGVhbiB3aGV0aGVyIGEgc2VhcmNoIGlucHV0IHNob3VsZCBiZSBzaG93bi5cclxuICovXHJcbmZ1bmN0aW9uIFNlbGVjdGl2aXR5RHJvcGRvd24ob3B0aW9ucykge1xyXG5cclxuICAgIHZhciBzZWxlY3Rpdml0eSA9IG9wdGlvbnMuc2VsZWN0aXZpdHk7XHJcblxyXG4gICAgdGhpcy4kZWwgPSAkKHNlbGVjdGl2aXR5LnRlbXBsYXRlKCdkcm9wZG93bicsIHtcclxuICAgICAgICBkcm9wZG93bkNzc0NsYXNzOiBzZWxlY3Rpdml0eS5vcHRpb25zLmRyb3Bkb3duQ3NzQ2xhc3MsXHJcbiAgICAgICAgc2VhcmNoSW5wdXRQbGFjZWhvbGRlcjogc2VsZWN0aXZpdHkub3B0aW9ucy5zZWFyY2hJbnB1dFBsYWNlaG9sZGVyLFxyXG4gICAgICAgIHNob3dTZWFyY2hJbnB1dDogb3B0aW9ucy5zaG93U2VhcmNoSW5wdXRcclxuICAgIH0pKTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIGpRdWVyeSBjb250YWluZXIgdG8gYWRkIHRoZSByZXN1bHRzIHRvLlxyXG4gICAgICovXHJcbiAgICB0aGlzLiRyZXN1bHRzID0gdGhpcy4kKCcuc2VsZWN0aXZpdHktcmVzdWx0cy1jb250YWluZXInKTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEJvb2xlYW4gaW5kaWNhdGluZyB3aGV0aGVyIG1vcmUgcmVzdWx0cyBhcmUgYXZhaWxhYmxlIHRoYW4gY3VycmVudGx5IGRpc3BsYXllZCBpbiB0aGVcclxuICAgICAqIGRyb3Bkb3duLlxyXG4gICAgICovXHJcbiAgICB0aGlzLmhhc01vcmUgPSBmYWxzZTtcclxuXHJcbiAgICAvKipcclxuICAgICAqIFRoZSBjdXJyZW50bHkgaGlnaGxpZ2h0ZWQgcmVzdWx0IGl0ZW0uXHJcbiAgICAgKi9cclxuICAgIHRoaXMuaGlnaGxpZ2h0ZWRSZXN1bHQgPSBudWxsO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogQm9vbGVhbiB3aGV0aGVyIHRoZSBsb2FkIG1vcmUgbGluayBpcyBjdXJyZW50bHkgaGlnaGxpZ2h0ZWQuXHJcbiAgICAgKi9cclxuICAgIHRoaXMubG9hZE1vcmVIaWdobGlnaHRlZCA9IGZhbHNlO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogT3B0aW9ucyBwYXNzZWQgdG8gdGhlIGRyb3Bkb3duIGNvbnN0cnVjdG9yLlxyXG4gICAgICovXHJcbiAgICB0aGlzLm9wdGlvbnMgPSBvcHRpb25zO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogVGhlIHJlc3VsdHMgZGlzcGxheWVkIGluIHRoZSBkcm9wZG93bi5cclxuICAgICAqL1xyXG4gICAgdGhpcy5yZXN1bHRzID0gW107XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBTZWxlY3Rpdml0eSBpbnN0YW5jZS5cclxuICAgICAqL1xyXG4gICAgdGhpcy5zZWxlY3Rpdml0eSA9IHNlbGVjdGl2aXR5O1xyXG5cclxuICAgIHRoaXMuX2Nsb3NlZCA9IGZhbHNlO1xyXG5cclxuICAgIHRoaXMuX2Nsb3NlUHJveHkgPSB0aGlzLmNsb3NlLmJpbmQodGhpcyk7XHJcbiAgICBpZiAoc2VsZWN0aXZpdHkub3B0aW9ucy5jbG9zZU9uU2VsZWN0ICE9PSBmYWxzZSkge1xyXG4gICAgICAgIHNlbGVjdGl2aXR5LiRlbC5vbignc2VsZWN0aXZpdHktc2VsZWN0aW5nJywgdGhpcy5fY2xvc2VQcm94eSk7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5fbGFzdE1vdXNlUG9zaXRpb24gPSB7fTtcclxuXHJcbiAgICB0aGlzLmFkZFRvRG9tKCk7XHJcbiAgICB0aGlzLnBvc2l0aW9uKCk7XHJcbiAgICB0aGlzLnNldHVwQ2xvc2VIYW5kbGVyKCk7XHJcblxyXG4gICAgdGhpcy5fc3VwcHJlc3NNb3VzZVdoZWVsKCk7XHJcblxyXG4gICAgaWYgKG9wdGlvbnMuc2hvd1NlYXJjaElucHV0KSB7XHJcbiAgICAgICAgc2VsZWN0aXZpdHkuaW5pdFNlYXJjaElucHV0KHRoaXMuJCgnLnNlbGVjdGl2aXR5LXNlYXJjaC1pbnB1dCcpKTtcclxuICAgICAgICBzZWxlY3Rpdml0eS5mb2N1cygpO1xyXG4gICAgfVxyXG5cclxuICAgIEV2ZW50RGVsZWdhdG9yLmNhbGwodGhpcyk7XHJcblxyXG4gICAgdGhpcy4kcmVzdWx0cy5vbignc2Nyb2xsIHRvdWNobW92ZSB0b3VjaGVuZCcsIGRlYm91bmNlKHRoaXMuX3Njcm9sbGVkLmJpbmQodGhpcyksIDUwKSk7XHJcblxyXG4gICAgdGhpcy5zaG93TG9hZGluZygpO1xyXG5cclxuICAgIHNldFRpbWVvdXQodGhpcy50cmlnZ2VyT3Blbi5iaW5kKHRoaXMpLCAxKTtcclxufVxyXG5cclxuLyoqXHJcbiAqIE1ldGhvZHMuXHJcbiAqL1xyXG4kLmV4dGVuZChTZWxlY3Rpdml0eURyb3Bkb3duLnByb3RvdHlwZSwgRXZlbnREZWxlZ2F0b3IucHJvdG90eXBlLCB7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDb252ZW5pZW5jZSBzaG9ydGN1dCBmb3IgdGhpcy4kZWwuZmluZChzZWxlY3RvcikuXHJcbiAgICAgKi9cclxuICAgICQ6IGZ1bmN0aW9uKHNlbGVjdG9yKSB7XHJcblxyXG4gICAgICAgIHJldHVybiB0aGlzLiRlbC5maW5kKHNlbGVjdG9yKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBBZGRzIHRoZSBkcm9wZG93biB0byB0aGUgRE9NLlxyXG4gICAgICovXHJcbiAgICBhZGRUb0RvbTogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHZhciAkbmV4dDtcclxuICAgICAgICB2YXIgJGFuY2hvciA9IHRoaXMuc2VsZWN0aXZpdHkuJGVsO1xyXG4gICAgICAgIHdoaWxlICgoJG5leHQgPSAkYW5jaG9yLm5leHQoJy5zZWxlY3Rpdml0eS1kcm9wZG93bicpKS5sZW5ndGgpIHtcclxuICAgICAgICAgICAgJGFuY2hvciA9ICRuZXh0O1xyXG4gICAgICAgIH1cclxuICAgICAgICB0aGlzLiRlbC5pbnNlcnRBZnRlcigkYW5jaG9yKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDbG9zZXMgdGhlIGRyb3Bkb3duLlxyXG4gICAgICovXHJcbiAgICBjbG9zZTogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIGlmICghdGhpcy5fY2xvc2VkKSB7XHJcbiAgICAgICAgICAgIHRoaXMuX2Nsb3NlZCA9IHRydWU7XHJcblxyXG4gICAgICAgICAgICB0aGlzLiRlbC5yZW1vdmUoKTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMucmVtb3ZlQ2xvc2VIYW5kbGVyKCk7XHJcblxyXG4gICAgICAgICAgICB0aGlzLnNlbGVjdGl2aXR5LiRlbC5vZmYoJ3NlbGVjdGl2aXR5LXNlbGVjdGluZycsIHRoaXMuX2Nsb3NlUHJveHkpO1xyXG5cclxuICAgICAgICAgICAgdGhpcy50cmlnZ2VyQ2xvc2UoKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogRXZlbnRzIG1hcC5cclxuICAgICAqXHJcbiAgICAgKiBGb2xsb3dzIHRoZSBzYW1lIGZvcm1hdCBhcyBCYWNrYm9uZTogaHR0cDovL2JhY2tib25lanMub3JnLyNWaWV3LWRlbGVnYXRlRXZlbnRzXHJcbiAgICAgKi9cclxuICAgIGV2ZW50czoge1xyXG4gICAgICAgICdjbGljayAuc2VsZWN0aXZpdHktbG9hZC1tb3JlJzogJ19sb2FkTW9yZUNsaWNrZWQnLFxyXG4gICAgICAgICdjbGljayAuc2VsZWN0aXZpdHktcmVzdWx0LWl0ZW0nOiAnX3Jlc3VsdENsaWNrZWQnLFxyXG4gICAgICAgICdtb3VzZWVudGVyIC5zZWxlY3Rpdml0eS1sb2FkLW1vcmUnOiAnX2xvYWRNb3JlSG92ZXJlZCcsXHJcbiAgICAgICAgJ21vdXNlZW50ZXIgLnNlbGVjdGl2aXR5LXJlc3VsdC1pdGVtJzogJ19yZXN1bHRIb3ZlcmVkJ1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEhpZ2hsaWdodHMgYSByZXN1bHQgaXRlbS5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gaXRlbSBUaGUgaXRlbSB0byBoaWdobGlnaHQuXHJcbiAgICAgKi9cclxuICAgIGhpZ2hsaWdodDogZnVuY3Rpb24oaXRlbSkge1xyXG5cclxuICAgICAgICBpZiAodGhpcy5sb2FkTW9yZUhpZ2hsaWdodGVkKSB7XHJcbiAgICAgICAgICAgIHRoaXMuJCgnLnNlbGVjdGl2aXR5LWxvYWQtbW9yZScpLnJlbW92ZUNsYXNzKCdoaWdobGlnaHQnKTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHRoaXMuJCgnLnNlbGVjdGl2aXR5LXJlc3VsdC1pdGVtJykucmVtb3ZlQ2xhc3MoJ2hpZ2hsaWdodCcpXHJcbiAgICAgICAgICAgIC5maWx0ZXIoJ1tkYXRhLWl0ZW0taWQ9JyArIFNlbGVjdGl2aXR5LnF1b3RlQ3NzQXR0cihpdGVtLmlkKSArICddJylcclxuICAgICAgICAgICAgLmFkZENsYXNzKCdoaWdobGlnaHQnKTtcclxuXHJcbiAgICAgICAgdGhpcy5oaWdobGlnaHRlZFJlc3VsdCA9IGl0ZW07XHJcbiAgICAgICAgdGhpcy5sb2FkTW9yZUhpZ2hsaWdodGVkID0gZmFsc2U7XHJcblxyXG4gICAgICAgIHRoaXMuc2VsZWN0aXZpdHkudHJpZ2dlckV2ZW50KCdzZWxlY3Rpdml0eS1oaWdobGlnaHQnLCB7IGl0ZW06IGl0ZW0sIGlkOiBpdGVtLmlkIH0pO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEhpZ2hsaWdodHMgdGhlIGxvYWQgbW9yZSBsaW5rLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBpdGVtIFRoZSBpdGVtIHRvIGhpZ2hsaWdodC5cclxuICAgICAqL1xyXG4gICAgaGlnaGxpZ2h0TG9hZE1vcmU6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB0aGlzLiQoJy5zZWxlY3Rpdml0eS1yZXN1bHQtaXRlbScpLnJlbW92ZUNsYXNzKCdoaWdobGlnaHQnKTtcclxuXHJcbiAgICAgICAgdGhpcy4kKCcuc2VsZWN0aXZpdHktbG9hZC1tb3JlJykuYWRkQ2xhc3MoJ2hpZ2hsaWdodCcpO1xyXG5cclxuICAgICAgICB0aGlzLmhpZ2hsaWdodGVkUmVzdWx0ID0gbnVsbDtcclxuICAgICAgICB0aGlzLmxvYWRNb3JlSGlnaGxpZ2h0ZWQgPSB0cnVlO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIExvYWRzIGEgZm9sbG93LXVwIHBhZ2Ugd2l0aCByZXN1bHRzIGFmdGVyIGEgc2VhcmNoLlxyXG4gICAgICpcclxuICAgICAqIFRoaXMgbWV0aG9kIHNob3VsZCBvbmx5IGJlIGNhbGxlZCBhZnRlciBhIGNhbGwgdG8gc2VhcmNoKCkgd2hlbiB0aGUgY2FsbGJhY2sgaGFzIGluZGljYXRlZFxyXG4gICAgICogbW9yZSByZXN1bHRzIGFyZSBhdmFpbGFibGUuXHJcbiAgICAgKi9cclxuICAgIGxvYWRNb3JlOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgdGhpcy5vcHRpb25zLnF1ZXJ5KHtcclxuICAgICAgICAgICAgY2FsbGJhY2s6IGZ1bmN0aW9uKHJlc3BvbnNlKSB7XHJcbiAgICAgICAgICAgICAgICBpZiAocmVzcG9uc2UgJiYgcmVzcG9uc2UucmVzdWx0cykge1xyXG4gICAgICAgICAgICAgICAgICAgIHRoaXMuX3Nob3dSZXN1bHRzKFxyXG4gICAgICAgICAgICAgICAgICAgICAgICBTZWxlY3Rpdml0eS5wcm9jZXNzSXRlbXMocmVzcG9uc2UucmVzdWx0cyksXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHsgYWRkOiB0cnVlLCBoYXNNb3JlOiAhIXJlc3BvbnNlLm1vcmUgfVxyXG4gICAgICAgICAgICAgICAgICAgICk7XHJcbiAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcignY2FsbGJhY2sgbXVzdCBiZSBwYXNzZWQgYSByZXNwb25zZSBvYmplY3QnKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfS5iaW5kKHRoaXMpLFxyXG4gICAgICAgICAgICBlcnJvcjogdGhpcy5fc2hvd1Jlc3VsdHMuYmluZCh0aGlzLCBbXSwgeyBhZGQ6IHRydWUgfSksXHJcbiAgICAgICAgICAgIG9mZnNldDogdGhpcy5yZXN1bHRzLmxlbmd0aCxcclxuICAgICAgICAgICAgc2VsZWN0aXZpdHk6IHRoaXMuc2VsZWN0aXZpdHksXHJcbiAgICAgICAgICAgIHRlcm06IHRoaXMudGVybVxyXG4gICAgICAgIH0pO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFBvc2l0aW9ucyB0aGUgZHJvcGRvd24gaW5zaWRlIHRoZSBET00uXHJcbiAgICAgKi9cclxuICAgIHBvc2l0aW9uOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgdmFyIHBvc2l0aW9uID0gdGhpcy5vcHRpb25zLnBvc2l0aW9uO1xyXG4gICAgICAgIGlmIChwb3NpdGlvbikge1xyXG4gICAgICAgICAgICBwb3NpdGlvbih0aGlzLiRlbCwgdGhpcy5zZWxlY3Rpdml0eS4kZWwpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdGhpcy5fc2Nyb2xsZWQoKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBSZW1vdmVzIHRoZSBldmVudCBoYW5kbGVyIHRvIGNsb3NlIHRoZSBkcm9wZG93bi5cclxuICAgICAqL1xyXG4gICAgcmVtb3ZlQ2xvc2VIYW5kbGVyOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgJCgnYm9keScpLm9mZignY2xpY2snLCB0aGlzLl9jbG9zZVByb3h5KTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBSZW5kZXJzIGFuIGFycmF5IG9mIHJlc3VsdCBpdGVtcy5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gaXRlbXMgQXJyYXkgb2YgcmVzdWx0IGl0ZW1zLlxyXG4gICAgICpcclxuICAgICAqIEByZXR1cm4gSFRNTC1mb3JtYXR0ZWQgc3RyaW5nIHRvIGRpc3BsYXkgdGhlIHJlc3VsdCBpdGVtcy5cclxuICAgICAqL1xyXG4gICAgcmVuZGVySXRlbXM6IGZ1bmN0aW9uKGl0ZW1zKSB7XHJcblxyXG4gICAgICAgIHZhciBzZWxlY3Rpdml0eSA9IHRoaXMuc2VsZWN0aXZpdHk7XHJcbiAgICAgICAgcmV0dXJuIGl0ZW1zLm1hcChmdW5jdGlvbihpdGVtKSB7XHJcbiAgICAgICAgICAgIHZhciByZXN1bHQgPSBzZWxlY3Rpdml0eS50ZW1wbGF0ZShpdGVtLmlkID8gJ3Jlc3VsdEl0ZW0nIDogJ3Jlc3VsdExhYmVsJywgaXRlbSk7XHJcbiAgICAgICAgICAgIGlmIChpdGVtLmNoaWxkcmVuKSB7XHJcbiAgICAgICAgICAgICAgICByZXN1bHQgKz0gc2VsZWN0aXZpdHkudGVtcGxhdGUoJ3Jlc3VsdENoaWxkcmVuJywge1xyXG4gICAgICAgICAgICAgICAgICAgIGNoaWxkcmVuSHRtbDogdGhpcy5yZW5kZXJJdGVtcyhpdGVtLmNoaWxkcmVuKVxyXG4gICAgICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgcmV0dXJuIHJlc3VsdDtcclxuICAgICAgICB9LCB0aGlzKS5qb2luKCcnKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBTZWFyY2hlcyBmb3IgcmVzdWx0cyBiYXNlZCBvbiB0aGUgdGVybSBnaXZlbiBvciB0aGUgdGVybSBlbnRlcmVkIGluIHRoZSBzZWFyY2ggaW5wdXQuXHJcbiAgICAgKlxyXG4gICAgICogSWYgYW4gaXRlbXMgYXJyYXkgaGFzIGJlZW4gcGFzc2VkIHdpdGggdGhlIG9wdGlvbnMgdG8gdGhlIFNlbGVjdGl2aXR5IGluc3RhbmNlLCBhIGxvY2FsXHJcbiAgICAgKiBzZWFyY2ggd2lsbCBiZSBwZXJmb3JtZWQgYW1vbmcgdGhvc2UgaXRlbXMuIE90aGVyd2lzZSwgdGhlIHF1ZXJ5IGZ1bmN0aW9uIHNwZWNpZmllZCBpbiB0aGVcclxuICAgICAqIG9wdGlvbnMgd2lsbCBiZSB1c2VkIHRvIHBlcmZvcm0gdGhlIHNlYXJjaC4gSWYgbmVpdGhlciBpcyBkZWZpbmVkLCBub3RoaW5nIGhhcHBlbnMuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIHRlcm0gVGVybSB0byBzZWFyY2ggZm9yLlxyXG4gICAgICovXHJcbiAgICBzZWFyY2g6IGZ1bmN0aW9uKHRlcm0pIHtcclxuXHJcbiAgICAgICAgdmFyIHNlbGYgPSB0aGlzO1xyXG5cclxuICAgICAgICB0ZXJtID0gdGVybSB8fCAnJztcclxuICAgICAgICBzZWxmLnRlcm0gPSB0ZXJtO1xyXG5cclxuICAgICAgICBpZiAoc2VsZi5vcHRpb25zLml0ZW1zKSB7XHJcbiAgICAgICAgICAgIHRlcm0gPSBTZWxlY3Rpdml0eS50cmFuc2Zvcm1UZXh0KHRlcm0pO1xyXG4gICAgICAgICAgICB2YXIgbWF0Y2hlciA9IHNlbGYuc2VsZWN0aXZpdHkubWF0Y2hlcjtcclxuICAgICAgICAgICAgc2VsZi5fc2hvd1Jlc3VsdHMoc2VsZi5vcHRpb25zLml0ZW1zLm1hcChmdW5jdGlvbihpdGVtKSB7XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gbWF0Y2hlcihpdGVtLCB0ZXJtKTtcclxuICAgICAgICAgICAgfSkuZmlsdGVyKGZ1bmN0aW9uKGl0ZW0pIHtcclxuICAgICAgICAgICAgICAgIHJldHVybiAhIWl0ZW07XHJcbiAgICAgICAgICAgIH0pLCB7IHRlcm06IHRlcm0gfSk7XHJcbiAgICAgICAgfSBlbHNlIGlmIChzZWxmLm9wdGlvbnMucXVlcnkpIHtcclxuICAgICAgICAgICAgc2VsZi5vcHRpb25zLnF1ZXJ5KHtcclxuICAgICAgICAgICAgICAgIGNhbGxiYWNrOiBmdW5jdGlvbihyZXNwb25zZSkge1xyXG4gICAgICAgICAgICAgICAgICAgIGlmIChyZXNwb25zZSAmJiByZXNwb25zZS5yZXN1bHRzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHNlbGYuX3Nob3dSZXN1bHRzKFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgU2VsZWN0aXZpdHkucHJvY2Vzc0l0ZW1zKHJlc3BvbnNlLnJlc3VsdHMpLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgeyBoYXNNb3JlOiAhIXJlc3BvbnNlLm1vcmUsIHRlcm06IHRlcm0gfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICApO1xyXG4gICAgICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcignY2FsbGJhY2sgbXVzdCBiZSBwYXNzZWQgYSByZXNwb25zZSBvYmplY3QnKTtcclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAgZXJyb3I6IHNlbGYuc2hvd0Vycm9yLmJpbmQoc2VsZiksXHJcbiAgICAgICAgICAgICAgICBvZmZzZXQ6IDAsXHJcbiAgICAgICAgICAgICAgICBzZWxlY3Rpdml0eTogc2VsZi5zZWxlY3Rpdml0eSxcclxuICAgICAgICAgICAgICAgIHRlcm06IHRlcm1cclxuICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFNlbGVjdHMgdGhlIGhpZ2hsaWdodGVkIGl0ZW0uXHJcbiAgICAgKi9cclxuICAgIHNlbGVjdEhpZ2hsaWdodDogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLmhpZ2hsaWdodGVkUmVzdWx0KSB7XHJcbiAgICAgICAgICAgIHRoaXMuc2VsZWN0SXRlbSh0aGlzLmhpZ2hsaWdodGVkUmVzdWx0LmlkKTtcclxuICAgICAgICB9IGVsc2UgaWYgKHRoaXMubG9hZE1vcmVIaWdobGlnaHRlZCkge1xyXG4gICAgICAgICAgICB0aGlzLl9sb2FkTW9yZUNsaWNrZWQoKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogU2VsZWN0cyB0aGUgaXRlbSB3aXRoIHRoZSBnaXZlbiBJRC5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gaWQgSUQgb2YgdGhlIGl0ZW0gdG8gc2VsZWN0LlxyXG4gICAgICovXHJcbiAgICBzZWxlY3RJdGVtOiBmdW5jdGlvbihpZCkge1xyXG5cclxuICAgICAgICB2YXIgaXRlbSA9IFNlbGVjdGl2aXR5LmZpbmROZXN0ZWRCeUlkKHRoaXMucmVzdWx0cywgaWQpO1xyXG4gICAgICAgIGlmIChpdGVtICYmICFpdGVtLmRpc2FibGVkKSB7XHJcbiAgICAgICAgICAgIHZhciBvcHRpb25zID0geyBpZDogaWQsIGl0ZW06IGl0ZW0gfTtcclxuICAgICAgICAgICAgaWYgKHRoaXMuc2VsZWN0aXZpdHkudHJpZ2dlckV2ZW50KCdzZWxlY3Rpdml0eS1zZWxlY3RpbmcnLCBvcHRpb25zKSkge1xyXG4gICAgICAgICAgICAgICAgdGhpcy5zZWxlY3Rpdml0eS50cmlnZ2VyRXZlbnQoJ3NlbGVjdGl2aXR5LXNlbGVjdGVkJywgb3B0aW9ucyk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogU2V0cyB1cCBhbiBldmVudCBoYW5kbGVyIHRoYXQgd2lsbCBjbG9zZSB0aGUgZHJvcGRvd24gd2hlbiB0aGUgU2VsZWN0aXZpdHkgY29udHJvbCBsb3Nlc1xyXG4gICAgICogZm9jdXMuXHJcbiAgICAgKi9cclxuICAgIHNldHVwQ2xvc2VIYW5kbGVyOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgJCgnYm9keScpLm9uKCdjbGljaycsIHRoaXMuX2Nsb3NlUHJveHkpO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFNob3dzIGFuIGVycm9yIG1lc3NhZ2UuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIG1lc3NhZ2UgRXJyb3IgbWVzc2FnZSB0byBkaXNwbGF5LlxyXG4gICAgICogQHBhcmFtIG9wdGlvbnMgT3B0aW9ucyBvYmplY3QuIE1heSBjb250YWluIHRoZSBmb2xsb3dpbmcgcHJvcGVydHk6XHJcbiAgICAgKiAgICAgICAgICAgICAgICBlc2NhcGUgLSBTZXQgdG8gZmFsc2UgdG8gZGlzYWJsZSBIVE1MLWVzY2FwaW5nIG9mIHRoZSBtZXNzYWdlLiBVc2VmdWwgaWYgeW91XHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICB3YW50IHRvIHNldCByYXcgSFRNTCBhcyB0aGUgbWVzc2FnZSwgYnV0IG1heSBvcGVuIHlvdSB1cCB0byBYU1NcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgIGF0dGFja3MgaWYgeW91J3JlIG5vdCBjYXJlZnVsIHdpdGggZXNjYXBpbmcgdXNlciBpbnB1dC5cclxuICAgICAqL1xyXG4gICAgc2hvd0Vycm9yOiBmdW5jdGlvbihtZXNzYWdlLCBvcHRpb25zKSB7XHJcblxyXG4gICAgICAgIG9wdGlvbnMgPSBvcHRpb25zIHx8IHt9O1xyXG5cclxuICAgICAgICB0aGlzLiRyZXN1bHRzLmh0bWwodGhpcy5zZWxlY3Rpdml0eS50ZW1wbGF0ZSgnZXJyb3InLCB7XHJcbiAgICAgICAgICAgIGVzY2FwZTogb3B0aW9ucy5lc2NhcGUgIT09IGZhbHNlLFxyXG4gICAgICAgICAgICBtZXNzYWdlOiBtZXNzYWdlXHJcbiAgICAgICAgfSkpO1xyXG5cclxuICAgICAgICB0aGlzLmhhc01vcmUgPSBmYWxzZTtcclxuICAgICAgICB0aGlzLnJlc3VsdHMgPSBbXTtcclxuXHJcbiAgICAgICAgdGhpcy5oaWdobGlnaHRlZFJlc3VsdCA9IG51bGw7XHJcbiAgICAgICAgdGhpcy5sb2FkTW9yZUhpZ2hsaWdodGVkID0gZmFsc2U7XHJcblxyXG4gICAgICAgIHRoaXMucG9zaXRpb24oKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBTaG93cyBhIGxvYWRpbmcgaW5kaWNhdG9yIGluIHRoZSBkcm9wZG93bi5cclxuICAgICAqL1xyXG4gICAgc2hvd0xvYWRpbmc6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB0aGlzLiRyZXN1bHRzLmh0bWwodGhpcy5zZWxlY3Rpdml0eS50ZW1wbGF0ZSgnbG9hZGluZycpKTtcclxuXHJcbiAgICAgICAgdGhpcy5oYXNNb3JlID0gZmFsc2U7XHJcbiAgICAgICAgdGhpcy5yZXN1bHRzID0gW107XHJcblxyXG4gICAgICAgIHRoaXMuaGlnaGxpZ2h0ZWRSZXN1bHQgPSBudWxsO1xyXG4gICAgICAgIHRoaXMubG9hZE1vcmVIaWdobGlnaHRlZCA9IGZhbHNlO1xyXG5cclxuICAgICAgICB0aGlzLnBvc2l0aW9uKCk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogU2hvd3MgdGhlIHJlc3VsdHMgZnJvbSBhIHNlYXJjaCBxdWVyeS5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gcmVzdWx0cyBBcnJheSBvZiByZXN1bHQgaXRlbXMuXHJcbiAgICAgKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25zIG9iamVjdC4gTWF5IGNvbnRhaW4gdGhlIGZvbGxvd2luZyBwcm9wZXJ0aWVzOlxyXG4gICAgICogICAgICAgICAgICAgICAgYWRkIC0gVHJ1ZSBpZiB0aGUgcmVzdWx0cyBzaG91bGQgYmUgYWRkZWQgdG8gYW55IGFscmVhZHkgc2hvd24gcmVzdWx0cy5cclxuICAgICAqICAgICAgICAgICAgICAgIGhhc01vcmUgLSBCb29sZWFuIHdoZXRoZXIgbW9yZSByZXN1bHRzIGNhbiBiZSBmZXRjaGVkIHVzaW5nIHRoZSBxdWVyeSgpXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgZnVuY3Rpb24uXHJcbiAgICAgKiAgICAgICAgICAgICAgICB0ZXJtIC0gVGhlIHNlYXJjaCB0ZXJtIGZvciB3aGljaCB0aGUgcmVzdWx0cyBhcmUgZGlzcGxheWVkLlxyXG4gICAgICovXHJcbiAgICBzaG93UmVzdWx0czogZnVuY3Rpb24ocmVzdWx0cywgb3B0aW9ucykge1xyXG5cclxuICAgICAgICB2YXIgcmVzdWx0c0h0bWwgPSB0aGlzLnJlbmRlckl0ZW1zKHJlc3VsdHMpO1xyXG4gICAgICAgIGlmIChvcHRpb25zLmhhc01vcmUpIHtcclxuICAgICAgICAgICAgcmVzdWx0c0h0bWwgKz0gdGhpcy5zZWxlY3Rpdml0eS50ZW1wbGF0ZSgnbG9hZE1vcmUnKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBpZiAoIXJlc3VsdHNIdG1sICYmICFvcHRpb25zLmFkZCkge1xyXG4gICAgICAgICAgICAgICAgcmVzdWx0c0h0bWwgPSB0aGlzLnNlbGVjdGl2aXR5LnRlbXBsYXRlKCdub1Jlc3VsdHMnLCB7IHRlcm06IG9wdGlvbnMudGVybSB9KTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgaWYgKG9wdGlvbnMuYWRkKSB7XHJcbiAgICAgICAgICAgIHRoaXMuJCgnLnNlbGVjdGl2aXR5LWxvYWRpbmcnKS5yZXBsYWNlV2l0aChyZXN1bHRzSHRtbCk7XHJcblxyXG4gICAgICAgICAgICB0aGlzLnJlc3VsdHMgPSB0aGlzLnJlc3VsdHMuY29uY2F0KHJlc3VsdHMpO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRoaXMuJHJlc3VsdHMuaHRtbChyZXN1bHRzSHRtbCk7XHJcblxyXG4gICAgICAgICAgICB0aGlzLnJlc3VsdHMgPSByZXN1bHRzO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdGhpcy5oYXNNb3JlID0gb3B0aW9ucy5oYXNNb3JlO1xyXG5cclxuICAgICAgICBpZiAoIW9wdGlvbnMuYWRkIHx8IHRoaXMubG9hZE1vcmVIaWdobGlnaHRlZCkge1xyXG4gICAgICAgICAgICB0aGlzLl9oaWdobGlnaHRGaXJzdEl0ZW0ocmVzdWx0cyk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLnBvc2l0aW9uKCk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogVHJpZ2dlcnMgdGhlICdzZWxlY3Rpdml0eS1jbG9zZScgZXZlbnQuXHJcbiAgICAgKi9cclxuICAgIHRyaWdnZXJDbG9zZTogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHRoaXMuc2VsZWN0aXZpdHkuJGVsLnRyaWdnZXIoJ3NlbGVjdGl2aXR5LWNsb3NlJyk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogVHJpZ2dlcnMgdGhlICdzZWxlY3Rpdml0eS1vcGVuJyBldmVudC5cclxuICAgICAqL1xyXG4gICAgdHJpZ2dlck9wZW46IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB0aGlzLnNlbGVjdGl2aXR5LiRlbC50cmlnZ2VyKCdzZWxlY3Rpdml0eS1vcGVuJyk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX2hpZ2hsaWdodEZpcnN0SXRlbTogZnVuY3Rpb24ocmVzdWx0cykge1xyXG5cclxuICAgICAgICBmdW5jdGlvbiBmaW5kRmlyc3RJdGVtKHJlc3VsdHMpIHtcclxuICAgICAgICAgICAgZm9yICh2YXIgaSA9IDAsIGxlbmd0aCA9IHJlc3VsdHMubGVuZ3RoOyBpIDwgbGVuZ3RoOyBpKyspIHtcclxuICAgICAgICAgICAgICAgIHZhciByZXN1bHQgPSByZXN1bHRzW2ldO1xyXG4gICAgICAgICAgICAgICAgaWYgKHJlc3VsdC5pZCkge1xyXG4gICAgICAgICAgICAgICAgICAgIHJldHVybiByZXN1bHQ7XHJcbiAgICAgICAgICAgICAgICB9IGVsc2UgaWYgKHJlc3VsdC5jaGlsZHJlbikge1xyXG4gICAgICAgICAgICAgICAgICAgIHZhciBpdGVtID0gZmluZEZpcnN0SXRlbShyZXN1bHQuY2hpbGRyZW4pO1xyXG4gICAgICAgICAgICAgICAgICAgIGlmIChpdGVtKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiBpdGVtO1xyXG4gICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdmFyIGZpcnN0SXRlbSA9IGZpbmRGaXJzdEl0ZW0ocmVzdWx0cyk7XHJcbiAgICAgICAgaWYgKGZpcnN0SXRlbSkge1xyXG4gICAgICAgICAgICB0aGlzLmhpZ2hsaWdodChmaXJzdEl0ZW0pO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRoaXMuaGlnaGxpZ2h0ZWRSZXN1bHQgPSBudWxsO1xyXG4gICAgICAgICAgICB0aGlzLmxvYWRNb3JlSGlnaGxpZ2h0ZWQgPSBmYWxzZTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX2xvYWRNb3JlQ2xpY2tlZDogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHRoaXMuJCgnLnNlbGVjdGl2aXR5LWxvYWQtbW9yZScpLnJlcGxhY2VXaXRoKHRoaXMuc2VsZWN0aXZpdHkudGVtcGxhdGUoJ2xvYWRpbmcnKSk7XHJcblxyXG4gICAgICAgIHRoaXMubG9hZE1vcmUoKTtcclxuXHJcbiAgICAgICAgdGhpcy5zZWxlY3Rpdml0eS5mb2N1cygpO1xyXG5cclxuICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX2xvYWRNb3JlSG92ZXJlZDogZnVuY3Rpb24oZXZlbnQpIHtcclxuXHJcbiAgICAgICAgaWYgKGV2ZW50LnNjcmVlblggPT09IHVuZGVmaW5lZCB8fCBldmVudC5zY3JlZW5YICE9PSB0aGlzLl9sYXN0TW91c2VQb3NpdGlvbi54IHx8XHJcbiAgICAgICAgICAgIGV2ZW50LnNjcmVlblkgPT09IHVuZGVmaW5lZCB8fCBldmVudC5zY3JlZW5ZICE9PSB0aGlzLl9sYXN0TW91c2VQb3NpdGlvbi55KSB7XHJcbiAgICAgICAgICAgIHRoaXMuaGlnaGxpZ2h0TG9hZE1vcmUoKTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMuX3JlY29yZE1vdXNlUG9zaXRpb24oZXZlbnQpO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfcmVjb3JkTW91c2VQb3NpdGlvbjogZnVuY3Rpb24oZXZlbnQpIHtcclxuXHJcbiAgICAgICAgdGhpcy5fbGFzdE1vdXNlUG9zaXRpb24gPSB7IHg6IGV2ZW50LnNjcmVlblgsIHk6IGV2ZW50LnNjcmVlblkgfTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfcmVzdWx0Q2xpY2tlZDogZnVuY3Rpb24oZXZlbnQpIHtcclxuXHJcbiAgICAgICAgdGhpcy5zZWxlY3RJdGVtKHRoaXMuc2VsZWN0aXZpdHkuX2dldEl0ZW1JZChldmVudCkpO1xyXG5cclxuICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX3Jlc3VsdEhvdmVyZWQ6IGZ1bmN0aW9uKGV2ZW50KSB7XHJcblxyXG4gICAgICAgIGlmIChldmVudC5zY3JlZW5YID09PSB1bmRlZmluZWQgfHwgZXZlbnQuc2NyZWVuWCAhPT0gdGhpcy5fbGFzdE1vdXNlUG9zaXRpb24ueCB8fFxyXG4gICAgICAgICAgICBldmVudC5zY3JlZW5ZID09PSB1bmRlZmluZWQgfHwgZXZlbnQuc2NyZWVuWSAhPT0gdGhpcy5fbGFzdE1vdXNlUG9zaXRpb24ueSkge1xyXG4gICAgICAgICAgICB2YXIgaWQgPSB0aGlzLnNlbGVjdGl2aXR5Ll9nZXRJdGVtSWQoZXZlbnQpO1xyXG4gICAgICAgICAgICB2YXIgaXRlbSA9IFNlbGVjdGl2aXR5LmZpbmROZXN0ZWRCeUlkKHRoaXMucmVzdWx0cywgaWQpO1xyXG4gICAgICAgICAgICBpZiAoaXRlbSAmJiAhaXRlbS5kaXNhYmxlZCkge1xyXG4gICAgICAgICAgICAgICAgdGhpcy5oaWdobGlnaHQoaXRlbSk7XHJcbiAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgIHRoaXMuX3JlY29yZE1vdXNlUG9zaXRpb24oZXZlbnQpO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfc2Nyb2xsZWQ6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB2YXIgJGxvYWRNb3JlID0gdGhpcy4kKCcuc2VsZWN0aXZpdHktbG9hZC1tb3JlJyk7XHJcbiAgICAgICAgaWYgKCRsb2FkTW9yZS5sZW5ndGgpIHtcclxuICAgICAgICAgICAgaWYgKCRsb2FkTW9yZVswXS5vZmZzZXRUb3AgLSB0aGlzLiRyZXN1bHRzWzBdLnNjcm9sbFRvcCA8IHRoaXMuJGVsLmhlaWdodCgpKSB7XHJcbiAgICAgICAgICAgICAgICB0aGlzLl9sb2FkTW9yZUNsaWNrZWQoKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfc2hvd1Jlc3VsdHM6IGZ1bmN0aW9uKHJlc3VsdHMsIG9wdGlvbnMpIHtcclxuXHJcbiAgICAgICAgdGhpcy5zaG93UmVzdWx0cyh0aGlzLnNlbGVjdGl2aXR5LmZpbHRlclJlc3VsdHMocmVzdWx0cyksIG9wdGlvbnMpO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9zdXBwcmVzc01vdXNlV2hlZWw6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB2YXIgc3VwcHJlc3NNb3VzZVdoZWVsU2VsZWN0b3IgPSB0aGlzLnNlbGVjdGl2aXR5Lm9wdGlvbnMuc3VwcHJlc3NNb3VzZVdoZWVsU2VsZWN0b3I7XHJcbiAgICAgICAgaWYgKHN1cHByZXNzTW91c2VXaGVlbFNlbGVjdG9yID09PSBudWxsKSB7XHJcbiAgICAgICAgICAgIHJldHVybjtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHZhciBzZWxlY3RvciA9IHN1cHByZXNzTW91c2VXaGVlbFNlbGVjdG9yIHx8ICcuc2VsZWN0aXZpdHktcmVzdWx0cy1jb250YWluZXInO1xyXG4gICAgICAgIHRoaXMuJGVsLm9uKCdET01Nb3VzZVNjcm9sbCBtb3VzZXdoZWVsJywgc2VsZWN0b3IsIGZ1bmN0aW9uKGV2ZW50KSB7XHJcblxyXG4gICAgICAgICAgICAvLyBUaGFua3MgdG8gVHJveSBBbGZvcmQ6XHJcbiAgICAgICAgICAgIC8vIGh0dHA6Ly9zdGFja292ZXJmbG93LmNvbS9xdWVzdGlvbnMvNTgwMjQ2Ny9wcmV2ZW50LXNjcm9sbGluZy1vZi1wYXJlbnQtZWxlbWVudFxyXG5cclxuICAgICAgICAgICAgdmFyICRlbCA9ICQodGhpcyksXHJcbiAgICAgICAgICAgICAgICBzY3JvbGxUb3AgPSB0aGlzLnNjcm9sbFRvcCxcclxuICAgICAgICAgICAgICAgIHNjcm9sbEhlaWdodCA9IHRoaXMuc2Nyb2xsSGVpZ2h0LFxyXG4gICAgICAgICAgICAgICAgaGVpZ2h0ID0gJGVsLmhlaWdodCgpLFxyXG4gICAgICAgICAgICAgICAgb3JpZ2luYWxFdmVudCA9IGV2ZW50Lm9yaWdpbmFsRXZlbnQsXHJcbiAgICAgICAgICAgICAgICBkZWx0YSA9IChldmVudC50eXBlID09PSAnRE9NTW91c2VTY3JvbGwnID8gb3JpZ2luYWxFdmVudC5kZXRhaWwgKiAtNDBcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgOiBvcmlnaW5hbEV2ZW50LndoZWVsRGVsdGEpLFxyXG4gICAgICAgICAgICAgICAgdXAgPSBkZWx0YSA+IDA7XHJcblxyXG4gICAgICAgICAgICBmdW5jdGlvbiBwcmV2ZW50KCkge1xyXG4gICAgICAgICAgICAgICAgZXZlbnQuc3RvcFByb3BhZ2F0aW9uKCk7XHJcbiAgICAgICAgICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xyXG4gICAgICAgICAgICAgICAgZXZlbnQucmV0dXJuVmFsdWUgPSBmYWxzZTtcclxuICAgICAgICAgICAgICAgIHJldHVybiBmYWxzZTtcclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgaWYgKHNjcm9sbEhlaWdodCA+IGhlaWdodCkge1xyXG4gICAgICAgICAgICAgICAgaWYgKCF1cCAmJiAtZGVsdGEgPiBzY3JvbGxIZWlnaHQgLSBoZWlnaHQgLSBzY3JvbGxUb3ApIHtcclxuICAgICAgICAgICAgICAgICAgICAvLyBTY3JvbGxpbmcgZG93biwgYnV0IHRoaXMgd2lsbCB0YWtlIHVzIHBhc3QgdGhlIGJvdHRvbS5cclxuICAgICAgICAgICAgICAgICAgICAkZWwuc2Nyb2xsVG9wKHNjcm9sbEhlaWdodCk7XHJcbiAgICAgICAgICAgICAgICAgICAgcmV0dXJuIHByZXZlbnQoKTtcclxuICAgICAgICAgICAgICAgIH0gZWxzZSBpZiAodXAgJiYgZGVsdGEgPiBzY3JvbGxUb3ApIHtcclxuICAgICAgICAgICAgICAgICAgICAvLyBTY3JvbGxpbmcgdXAsIGJ1dCB0aGlzIHdpbGwgdGFrZSB1cyBwYXN0IHRoZSB0b3AuXHJcbiAgICAgICAgICAgICAgICAgICAgJGVsLnNjcm9sbFRvcCgwKTtcclxuICAgICAgICAgICAgICAgICAgICByZXR1cm4gcHJldmVudCgpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG59KTtcclxuXHJcbm1vZHVsZS5leHBvcnRzID0gU2VsZWN0aXZpdHkuRHJvcGRvd24gPSBTZWxlY3Rpdml0eURyb3Bkb3duO1xyXG5cclxufSx7XCIyXCI6MixcIjNcIjozLFwiOFwiOjgsXCJqcXVlcnlcIjpcImpxdWVyeVwifV0sMTE6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG52YXIgJCA9IHdpbmRvdy5qUXVlcnkgfHwgd2luZG93LlplcHRvO1xyXG5cclxudmFyIFNlbGVjdGl2aXR5ID0gX2RlcmVxXyg4KTtcclxudmFyIE11bHRpcGxlU2VsZWN0aXZpdHkgPSBfZGVyZXFfKDE0KTtcclxuXHJcbmZ1bmN0aW9uIGlzVmFsaWRFbWFpbChlbWFpbCkge1xyXG5cclxuICAgIHZhciBhdEluZGV4ID0gZW1haWwuaW5kZXhPZignQCcpO1xyXG4gICAgdmFyIGRvdEluZGV4ID0gZW1haWwubGFzdEluZGV4T2YoJy4nKTtcclxuICAgIHZhciBzcGFjZUluZGV4ID0gZW1haWwuaW5kZXhPZignICcpO1xyXG4gICAgcmV0dXJuIChhdEluZGV4ID4gMCAmJiBkb3RJbmRleCA+IGF0SW5kZXggKyAxICYmXHJcbiAgICAgICAgICAgIGRvdEluZGV4IDwgZW1haWwubGVuZ3RoIC0gMiAmJiBzcGFjZUluZGV4ID09PSAtMSk7XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGxhc3RXb3JkKHRva2VuLCBsZW5ndGgpIHtcclxuXHJcbiAgICBsZW5ndGggPSAobGVuZ3RoID09PSB1bmRlZmluZWQgPyB0b2tlbi5sZW5ndGggOiBsZW5ndGgpO1xyXG4gICAgZm9yICh2YXIgaSA9IGxlbmd0aCAtIDE7IGkgPj0gMDsgaS0tKSB7XHJcbiAgICAgICAgaWYgKCgvXFxzLykudGVzdCh0b2tlbltpXSkpIHtcclxuICAgICAgICAgICAgcmV0dXJuIHRva2VuLnNsaWNlKGkgKyAxLCBsZW5ndGgpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIHJldHVybiB0b2tlbi5zbGljZSgwLCBsZW5ndGgpO1xyXG59XHJcblxyXG5mdW5jdGlvbiBzdHJpcEVuY2xvc3VyZSh0b2tlbiwgZW5jbG9zdXJlKSB7XHJcblxyXG4gICAgaWYgKHRva2VuLnNsaWNlKDAsIDEpID09PSBlbmNsb3N1cmVbMF0gJiYgdG9rZW4uc2xpY2UoLTEpID09PSBlbmNsb3N1cmVbMV0pIHtcclxuICAgICAgICByZXR1cm4gdG9rZW4uc2xpY2UoMSwgLTEpLnRyaW0oKTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgICAgcmV0dXJuIHRva2VuLnRyaW0oKTtcclxuICAgIH1cclxufVxyXG5cclxuZnVuY3Rpb24gY3JlYXRlRW1haWxJdGVtKHRva2VuKSB7XHJcblxyXG4gICAgdmFyIGVtYWlsID0gbGFzdFdvcmQodG9rZW4pO1xyXG4gICAgdmFyIG5hbWUgPSB0b2tlbi5zbGljZSgwLCAtZW1haWwubGVuZ3RoKS50cmltKCk7XHJcbiAgICBpZiAoaXNWYWxpZEVtYWlsKGVtYWlsKSkge1xyXG4gICAgICAgIGVtYWlsID0gc3RyaXBFbmNsb3N1cmUoc3RyaXBFbmNsb3N1cmUoZW1haWwsICcoKScpLCAnPD4nKTtcclxuICAgICAgICBuYW1lID0gc3RyaXBFbmNsb3N1cmUobmFtZSwgJ1wiXCInKS50cmltKCkgfHwgZW1haWw7XHJcbiAgICAgICAgcmV0dXJuIHsgaWQ6IGVtYWlsLCB0ZXh0OiBuYW1lIH07XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICAgIHJldHVybiAodG9rZW4udHJpbSgpID8geyBpZDogdG9rZW4sIHRleHQ6IHRva2VuIH0gOiBudWxsKTtcclxuICAgIH1cclxufVxyXG5cclxuZnVuY3Rpb24gZW1haWxUb2tlbml6ZXIoaW5wdXQsIHNlbGVjdGlvbiwgY3JlYXRlVG9rZW4pIHtcclxuXHJcbiAgICBmdW5jdGlvbiBoYXNUb2tlbihpbnB1dCkge1xyXG4gICAgICAgIGlmIChpbnB1dCkge1xyXG4gICAgICAgICAgICBmb3IgKHZhciBpID0gMCwgbGVuZ3RoID0gaW5wdXQubGVuZ3RoOyBpIDwgbGVuZ3RoOyBpKyspIHtcclxuICAgICAgICAgICAgICAgIHN3aXRjaCAoaW5wdXRbaV0pIHtcclxuICAgICAgICAgICAgICAgIGNhc2UgJzsnOlxyXG4gICAgICAgICAgICAgICAgY2FzZSAnLCc6XHJcbiAgICAgICAgICAgICAgICBjYXNlICdcXG4nOlxyXG4gICAgICAgICAgICAgICAgICAgIHJldHVybiB0cnVlO1xyXG4gICAgICAgICAgICAgICAgY2FzZSAnICc6XHJcbiAgICAgICAgICAgICAgICBjYXNlICdcXHQnOlxyXG4gICAgICAgICAgICAgICAgICAgIGlmIChpc1ZhbGlkRW1haWwobGFzdFdvcmQoaW5wdXQsIGkpKSkge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICByZXR1cm4gdHJ1ZTtcclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgICAgICBjYXNlICdcIic6XHJcbiAgICAgICAgICAgICAgICAgICAgZG8geyBpKys7IH0gd2hpbGUoaSA8IGxlbmd0aCAmJiBpbnB1dFtpXSAhPT0gJ1wiJyk7XHJcbiAgICAgICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgICAgICBkZWZhdWx0OlxyXG4gICAgICAgICAgICAgICAgICAgIGNvbnRpbnVlO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiB0YWtlVG9rZW4oaW5wdXQpIHtcclxuICAgICAgICBmb3IgKHZhciBpID0gMCwgbGVuZ3RoID0gaW5wdXQubGVuZ3RoOyBpIDwgbGVuZ3RoOyBpKyspIHtcclxuICAgICAgICAgICAgc3dpdGNoIChpbnB1dFtpXSkge1xyXG4gICAgICAgICAgICBjYXNlICc7JzpcclxuICAgICAgICAgICAgY2FzZSAnLCc6XHJcbiAgICAgICAgICAgIGNhc2UgJ1xcbic6XHJcbiAgICAgICAgICAgICAgICByZXR1cm4geyB0ZXJtOiBpbnB1dC5zbGljZSgwLCBpKSwgaW5wdXQ6IGlucHV0LnNsaWNlKGkgKyAxKSB9O1xyXG4gICAgICAgICAgICBjYXNlICcgJzpcclxuICAgICAgICAgICAgY2FzZSAnXFx0JzpcclxuICAgICAgICAgICAgICAgIGlmIChpc1ZhbGlkRW1haWwobGFzdFdvcmQoaW5wdXQsIGkpKSkge1xyXG4gICAgICAgICAgICAgICAgICAgIHJldHVybiB7IHRlcm06IGlucHV0LnNsaWNlKDAsIGkpLCBpbnB1dDogaW5wdXQuc2xpY2UoaSArIDEpIH07XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgY2FzZSAnXCInOlxyXG4gICAgICAgICAgICAgICAgZG8geyBpKys7IH0gd2hpbGUoaSA8IGxlbmd0aCAmJiBpbnB1dFtpXSAhPT0gJ1wiJyk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgZGVmYXVsdDpcclxuICAgICAgICAgICAgICAgIGNvbnRpbnVlO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHJldHVybiB7fTtcclxuICAgIH1cclxuXHJcbiAgICB3aGlsZSAoaGFzVG9rZW4oaW5wdXQpKSB7XHJcbiAgICAgICAgdmFyIHRva2VuID0gdGFrZVRva2VuKGlucHV0KTtcclxuICAgICAgICBpZiAodG9rZW4udGVybSkge1xyXG4gICAgICAgICAgICB2YXIgaXRlbSA9IGNyZWF0ZUVtYWlsSXRlbSh0b2tlbi50ZXJtKTtcclxuICAgICAgICAgICAgaWYgKGl0ZW0gJiYgIShpdGVtLmlkICYmIFNlbGVjdGl2aXR5LmZpbmRCeUlkKHNlbGVjdGlvbiwgaXRlbS5pZCkpKSB7XHJcbiAgICAgICAgICAgICAgICBjcmVhdGVUb2tlbihpdGVtKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgICAgICBpbnB1dCA9IHRva2VuLmlucHV0O1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBpbnB1dDtcclxufVxyXG5cclxuLyoqXHJcbiAqIEVtYWlsc2VsZWN0aXZpdHkgQ29uc3RydWN0b3IuXHJcbiAqXHJcbiAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0LiBBY2NlcHRzIGFsbCBvcHRpb25zIGZyb20gdGhlIE11bHRpcGxlU2VsZWN0aXZpdHkgQ29uc3RydWN0b3IuXHJcbiAqL1xyXG5mdW5jdGlvbiBFbWFpbHNlbGVjdGl2aXR5KG9wdGlvbnMpIHtcclxuXHJcbiAgICBNdWx0aXBsZVNlbGVjdGl2aXR5LmNhbGwodGhpcywgb3B0aW9ucyk7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBNZXRob2RzLlxyXG4gKi9cclxudmFyIGNhbGxTdXBlciA9IFNlbGVjdGl2aXR5LmluaGVyaXRzKEVtYWlsc2VsZWN0aXZpdHksIE11bHRpcGxlU2VsZWN0aXZpdHksIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBpbmhlcml0XHJcbiAgICAgKi9cclxuICAgIGluaXRTZWFyY2hJbnB1dDogZnVuY3Rpb24oJGlucHV0KSB7XHJcblxyXG4gICAgICAgIGNhbGxTdXBlcih0aGlzLCAnaW5pdFNlYXJjaElucHV0JywgJGlucHV0KTtcclxuXHJcbiAgICAgICAgJGlucHV0Lm9uKCdibHVyJywgZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgICAgIHZhciB0ZXJtID0gJGlucHV0LnZhbCgpO1xyXG4gICAgICAgICAgICBpZiAoaXNWYWxpZEVtYWlsKGxhc3RXb3JkKHRlcm0pKSkge1xyXG4gICAgICAgICAgICAgICAgdGhpcy5hZGQoY3JlYXRlRW1haWxJdGVtKHRlcm0pKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0uYmluZCh0aGlzKSk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQGluaGVyaXRcclxuICAgICAqXHJcbiAgICAgKiBOb3RlIHRoYXQgZm9yIHRoZSBFbWFpbCBpbnB1dCB0eXBlIHRoZSBvcHRpb24gc2hvd0Ryb3Bkb3duIGlzIHNldCB0byBmYWxzZSBhbmQgdGhlIHRva2VuaXplclxyXG4gICAgICogb3B0aW9uIGlzIHNldCB0byBhIHRva2VuaXplciBzcGVjaWFsaXplZCBmb3IgZW1haWwgYWRkcmVzc2VzLlxyXG4gICAgICovXHJcbiAgICBzZXRPcHRpb25zOiBmdW5jdGlvbihvcHRpb25zKSB7XHJcblxyXG4gICAgICAgIG9wdGlvbnMgPSAkLmV4dGVuZCh7XHJcbiAgICAgICAgICAgIGNyZWF0ZVRva2VuSXRlbTogY3JlYXRlRW1haWxJdGVtLFxyXG4gICAgICAgICAgICBzaG93RHJvcGRvd246IGZhbHNlLFxyXG4gICAgICAgICAgICB0b2tlbml6ZXI6IGVtYWlsVG9rZW5pemVyXHJcbiAgICAgICAgfSwgb3B0aW9ucyk7XHJcblxyXG4gICAgICAgIGNhbGxTdXBlcih0aGlzLCAnc2V0T3B0aW9ucycsIG9wdGlvbnMpO1xyXG4gICAgfVxyXG5cclxufSk7XHJcblxyXG5tb2R1bGUuZXhwb3J0cyA9IFNlbGVjdGl2aXR5LklucHV0VHlwZXMuRW1haWwgPSBFbWFpbHNlbGVjdGl2aXR5O1xyXG5cclxufSx7XCIxNFwiOjE0LFwiOFwiOjgsXCJqcXVlcnlcIjpcImpxdWVyeVwifV0sMTI6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG52YXIgU2VsZWN0aXZpdHkgPSBfZGVyZXFfKDgpO1xyXG5cclxudmFyIEtFWV9CQUNLU1BBQ0UgPSA4O1xyXG52YXIgS0VZX0RPV05fQVJST1cgPSA0MDtcclxudmFyIEtFWV9FTlRFUiA9IDEzO1xyXG52YXIgS0VZX0VTQ0FQRSA9IDI3O1xyXG52YXIgS0VZX1RBQiA9IDk7XHJcbnZhciBLRVlfVVBfQVJST1cgPSAzODtcclxuXHJcbi8qKlxyXG4gKiBTZWFyY2ggaW5wdXQgbGlzdGVuZXIgcHJvdmlkaW5nIGtleWJvYXJkIHN1cHBvcnQgZm9yIG5hdmlnYXRpbmcgdGhlIGRyb3Bkb3duLlxyXG4gKi9cclxuZnVuY3Rpb24gbGlzdGVuZXIoc2VsZWN0aXZpdHksICRpbnB1dCkge1xyXG5cclxuICAgIHZhciBrZXlkb3duQ2FuY2VsZWQgPSBmYWxzZTtcclxuICAgIHZhciBjbG9zZVN1Ym1lbnUgPSBudWxsO1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogTW92ZXMgYSBkcm9wZG93bidzIGhpZ2hsaWdodCB0byB0aGUgbmV4dCBvciBwcmV2aW91cyByZXN1bHQgaXRlbS5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gZGVsdGEgRWl0aGVyIDEgdG8gbW92ZSB0byB0aGUgbmV4dCBpdGVtLCBvciAtMSB0byBtb3ZlIHRvIHRoZSBwcmV2aW91cyBpdGVtLlxyXG4gICAgICovXHJcbiAgICBmdW5jdGlvbiBtb3ZlSGlnaGxpZ2h0KGRyb3Bkb3duLCBkZWx0YSkge1xyXG5cclxuICAgICAgICBmdW5jdGlvbiBmaW5kRWxlbWVudEluZGV4KCRlbGVtZW50cywgc2VsZWN0b3IpIHtcclxuICAgICAgICAgICAgZm9yICh2YXIgaSA9IDAsIGxlbmd0aCA9ICRlbGVtZW50cy5sZW5ndGg7IGkgPCBsZW5ndGg7IGkrKykge1xyXG4gICAgICAgICAgICAgICAgaWYgKCRlbGVtZW50cy5lcShpKS5pcyhzZWxlY3RvcikpIHtcclxuICAgICAgICAgICAgICAgICAgICByZXR1cm4gaTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICByZXR1cm4gLTE7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBmdW5jdGlvbiBzY3JvbGxUb0hpZ2hsaWdodCgpIHtcclxuICAgICAgICAgICAgdmFyICRlbDtcclxuICAgICAgICAgICAgaWYgKGRyb3Bkb3duLmhpZ2hsaWdodGVkUmVzdWx0KSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgcXVvdGVkSWQgPSBTZWxlY3Rpdml0eS5xdW90ZUNzc0F0dHIoZHJvcGRvd24uaGlnaGxpZ2h0ZWRSZXN1bHQuaWQpO1xyXG4gICAgICAgICAgICAgICAgJGVsID0gZHJvcGRvd24uJCgnLnNlbGVjdGl2aXR5LXJlc3VsdC1pdGVtW2RhdGEtaXRlbS1pZD0nICsgcXVvdGVkSWQgKyAnXScpO1xyXG4gICAgICAgICAgICB9IGVsc2UgaWYgKGRyb3Bkb3duLmxvYWRNb3JlSGlnaGxpZ2h0ZWQpIHtcclxuICAgICAgICAgICAgICAgICRlbCA9IGRyb3Bkb3duLiQoJy5zZWxlY3Rpdml0eS1sb2FkLW1vcmUnKTtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIHJldHVybjsgLy8gbm8gaGlnaGxpZ2h0IHRvIHNjcm9sbCB0b1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICB2YXIgcG9zaXRpb24gPSAkZWwucG9zaXRpb24oKTtcclxuICAgICAgICAgICAgaWYgKCFwb3NpdGlvbikge1xyXG4gICAgICAgICAgICAgICAgcmV0dXJuO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICB2YXIgdG9wID0gcG9zaXRpb24udG9wO1xyXG4gICAgICAgICAgICB2YXIgcmVzdWx0c0hlaWdodCA9IGRyb3Bkb3duLiRyZXN1bHRzLmhlaWdodCgpO1xyXG4gICAgICAgICAgICB2YXIgZWxIZWlnaHQgPSAoJGVsLm91dGVySGVpZ2h0ID8gJGVsLm91dGVySGVpZ2h0KCkgOiAkZWwuaGVpZ2h0KCkpO1xyXG4gICAgICAgICAgICBpZiAodG9wIDwgMCB8fCB0b3AgPiByZXN1bHRzSGVpZ2h0IC0gZWxIZWlnaHQpIHtcclxuICAgICAgICAgICAgICAgIHRvcCArPSBkcm9wZG93bi4kcmVzdWx0cy5zY3JvbGxUb3AoKTtcclxuICAgICAgICAgICAgICAgIGRyb3Bkb3duLiRyZXN1bHRzLnNjcm9sbFRvcChkZWx0YSA8IDAgPyB0b3AgOiB0b3AgLSByZXN1bHRzSGVpZ2h0ICsgZWxIZWlnaHQpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBpZiAoZHJvcGRvd24uc3VibWVudSkge1xyXG4gICAgICAgICAgICBtb3ZlSGlnaGxpZ2h0KGRyb3Bkb3duLnN1Ym1lbnUsIGRlbHRhKTtcclxuICAgICAgICAgICAgcmV0dXJuO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdmFyIHJlc3VsdHMgPSBkcm9wZG93bi5yZXN1bHRzO1xyXG4gICAgICAgIGlmIChyZXN1bHRzLmxlbmd0aCkge1xyXG4gICAgICAgICAgICB2YXIgJHJlc3VsdHMgPSBkcm9wZG93bi4kKCcuc2VsZWN0aXZpdHktcmVzdWx0LWl0ZW0nKTtcclxuICAgICAgICAgICAgdmFyIGRlZmF1bHRJbmRleCA9IChkZWx0YSA+IDAgPyAwIDogJHJlc3VsdHMubGVuZ3RoIC0gMSk7XHJcbiAgICAgICAgICAgIHZhciBpbmRleCA9IGRlZmF1bHRJbmRleDtcclxuICAgICAgICAgICAgdmFyIGhpZ2hsaWdodGVkUmVzdWx0ID0gZHJvcGRvd24uaGlnaGxpZ2h0ZWRSZXN1bHQ7XHJcbiAgICAgICAgICAgIGlmIChoaWdobGlnaHRlZFJlc3VsdCkge1xyXG4gICAgICAgICAgICAgICAgdmFyIHF1b3RlZElkID0gU2VsZWN0aXZpdHkucXVvdGVDc3NBdHRyKGhpZ2hsaWdodGVkUmVzdWx0LmlkKTtcclxuICAgICAgICAgICAgICAgIGluZGV4ID0gZmluZEVsZW1lbnRJbmRleCgkcmVzdWx0cywgJ1tkYXRhLWl0ZW0taWQ9JyArIHF1b3RlZElkICsgJ10nKSArIGRlbHRhO1xyXG4gICAgICAgICAgICAgICAgaWYgKGRlbHRhID4gMCA/IGluZGV4ID49ICRyZXN1bHRzLmxlbmd0aCA6IGluZGV4IDwgMCkge1xyXG4gICAgICAgICAgICAgICAgICAgIGlmIChkcm9wZG93bi5oYXNNb3JlKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGRyb3Bkb3duLmhpZ2hsaWdodExvYWRNb3JlKCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHNjcm9sbFRvSGlnaGxpZ2h0KCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybjtcclxuICAgICAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBpbmRleCA9IGRlZmF1bHRJbmRleDtcclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgIHZhciByZXN1bHQgPSBTZWxlY3Rpdml0eS5maW5kTmVzdGVkQnlJZChyZXN1bHRzLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgc2VsZWN0aXZpdHkuX2dldEl0ZW1JZCgkcmVzdWx0c1tpbmRleF0pKTtcclxuICAgICAgICAgICAgaWYgKHJlc3VsdCkge1xyXG4gICAgICAgICAgICAgICAgZHJvcGRvd24uaGlnaGxpZ2h0KHJlc3VsdCwgeyBkZWxheTogISFyZXN1bHQuc3VibWVudSB9KTtcclxuICAgICAgICAgICAgICAgIHNjcm9sbFRvSGlnaGxpZ2h0KCk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24ga2V5SGVsZChldmVudCkge1xyXG5cclxuICAgICAgICB2YXIgZHJvcGRvd24gPSBzZWxlY3Rpdml0eS5kcm9wZG93bjtcclxuICAgICAgICBpZiAoZHJvcGRvd24pIHtcclxuICAgICAgICAgICAgaWYgKGV2ZW50LmtleUNvZGUgPT09IEtFWV9CQUNLU1BBQ0UpIHtcclxuICAgICAgICAgICAgICAgIGlmICghJGlucHV0LnZhbCgpKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKGRyb3Bkb3duLnN1Ym1lbnUpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdmFyIHN1Ym1lbnUgPSBkcm9wZG93bi5zdWJtZW51O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB3aGlsZSAoc3VibWVudS5zdWJtZW51KSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBzdWJtZW51ID0gc3VibWVudS5zdWJtZW51O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGNsb3NlU3VibWVudSA9IHN1Ym1lbnU7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xyXG4gICAgICAgICAgICAgICAgICAgIGtleWRvd25DYW5jZWxlZCA9IHRydWU7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH0gZWxzZSBpZiAoZXZlbnQua2V5Q29kZSA9PT0gS0VZX0RPV05fQVJST1cpIHtcclxuICAgICAgICAgICAgICAgIG1vdmVIaWdobGlnaHQoZHJvcGRvd24sIDEpO1xyXG4gICAgICAgICAgICB9IGVsc2UgaWYgKGV2ZW50LmtleUNvZGUgPT09IEtFWV9VUF9BUlJPVykge1xyXG4gICAgICAgICAgICAgICAgbW92ZUhpZ2hsaWdodChkcm9wZG93biwgLTEpO1xyXG4gICAgICAgICAgICB9IGVsc2UgaWYgKGV2ZW50LmtleUNvZGUgPT09IEtFWV9UQUIpIHtcclxuICAgICAgICAgICAgICAgIHNldFRpbWVvdXQoZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgc2VsZWN0aXZpdHkuY2xvc2UoeyBrZWVwRm9jdXM6IGZhbHNlIH0pO1xyXG4gICAgICAgICAgICAgICAgfSwgMSk7XHJcbiAgICAgICAgICAgIH0gZWxzZSBpZiAoZXZlbnQua2V5Q29kZSA9PT0gS0VZX0VOVEVSKSB7XHJcbiAgICAgICAgICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpOyAvLyBkb24ndCBzdWJtaXQgZm9ybXMgb24ga2V5ZG93blxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIGtleVJlbGVhc2VkKGV2ZW50KSB7XHJcblxyXG4gICAgICAgIGZ1bmN0aW9uIG9wZW4oKSB7XHJcbiAgICAgICAgICAgIGlmIChzZWxlY3Rpdml0eS5vcHRpb25zLnNob3dEcm9wZG93biAhPT0gZmFsc2UpIHtcclxuICAgICAgICAgICAgICAgIHNlbGVjdGl2aXR5Lm9wZW4oKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdmFyIGRyb3Bkb3duID0gc2VsZWN0aXZpdHkuZHJvcGRvd247XHJcbiAgICAgICAgaWYgKGtleWRvd25DYW5jZWxlZCkge1xyXG4gICAgICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xyXG4gICAgICAgICAgICBrZXlkb3duQ2FuY2VsZWQgPSBmYWxzZTtcclxuXHJcbiAgICAgICAgICAgIGlmIChjbG9zZVN1Ym1lbnUpIHtcclxuICAgICAgICAgICAgICAgIGNsb3NlU3VibWVudS5jbG9zZSgpO1xyXG4gICAgICAgICAgICAgICAgc2VsZWN0aXZpdHkuZm9jdXMoKTtcclxuICAgICAgICAgICAgICAgIGNsb3NlU3VibWVudSA9IG51bGw7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9IGVsc2UgaWYgKGV2ZW50LmtleUNvZGUgPT09IEtFWV9CQUNLU1BBQ0UpIHtcclxuICAgICAgICAgICAgaWYgKCFkcm9wZG93biAmJiBzZWxlY3Rpdml0eS5vcHRpb25zLmFsbG93Q2xlYXIpIHtcclxuICAgICAgICAgICAgICAgIHNlbGVjdGl2aXR5LmNsZWFyKCk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9IGVsc2UgaWYgKGV2ZW50LmtleUNvZGUgPT09IEtFWV9FTlRFUiAmJiAhZXZlbnQuY3RybEtleSkge1xyXG4gICAgICAgICAgICBpZiAoZHJvcGRvd24pIHtcclxuICAgICAgICAgICAgICAgIGRyb3Bkb3duLnNlbGVjdEhpZ2hsaWdodCgpO1xyXG4gICAgICAgICAgICB9IGVsc2UgaWYgKHNlbGVjdGl2aXR5Lm9wdGlvbnMuc2hvd0Ryb3Bkb3duICE9PSBmYWxzZSkge1xyXG4gICAgICAgICAgICAgICAgb3BlbigpO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xyXG4gICAgICAgIH0gZWxzZSBpZiAoZXZlbnQua2V5Q29kZSA9PT0gS0VZX0VTQ0FQRSkge1xyXG4gICAgICAgICAgICBzZWxlY3Rpdml0eS5jbG9zZSgpO1xyXG5cclxuICAgICAgICAgICAgZXZlbnQucHJldmVudERlZmF1bHQoKTtcclxuICAgICAgICB9IGVsc2UgaWYgKGV2ZW50LmtleUNvZGUgPT09IEtFWV9ET1dOX0FSUk9XIHx8IGV2ZW50LmtleUNvZGUgPT09IEtFWV9VUF9BUlJPVykge1xyXG4gICAgICAgICAgICAvLyBoYW5kbGVkIGluIGtleUhlbGQoKSBiZWNhdXNlIHRoZSByZXNwb25zZSBmZWVscyBmYXN0ZXIgYW5kIGl0IHdvcmtzIHdpdGggcmVwZWF0ZWRcclxuICAgICAgICAgICAgLy8gZXZlbnRzIGlmIHRoZSB1c2VyIGhvbGRzIHRoZSBrZXkgZm9yIGEgbG9uZ2VyIHBlcmlvZFxyXG4gICAgICAgICAgICAvLyBzdGlsbCwgd2UgaXNzdWUgYW4gb3BlbigpIGNhbGwgaGVyZSBpbiBjYXNlIHRoZSBkcm9wZG93biB3YXMgbm90IHlldCBvcGVuLi4uXHJcbiAgICAgICAgICAgIG9wZW4oKTtcclxuXHJcbiAgICAgICAgICAgIGV2ZW50LnByZXZlbnREZWZhdWx0KCk7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgb3BlbigpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuXHJcbiAgICAkaW5wdXQub24oJ2tleWRvd24nLCBrZXlIZWxkKS5vbigna2V5dXAnLCBrZXlSZWxlYXNlZCk7XHJcbn1cclxuXHJcblNlbGVjdGl2aXR5LlNlYXJjaElucHV0TGlzdGVuZXJzLnB1c2gobGlzdGVuZXIpO1xyXG5cclxufSx7XCI4XCI6OH1dLDEzOltmdW5jdGlvbihfZGVyZXFfLG1vZHVsZSxleHBvcnRzKXtcclxuJ3VzZSBzdHJpY3QnO1xyXG5cclxudmFyIGVzY2FwZSA9IF9kZXJlcV8oNCk7XHJcbnZhciBTZWxlY3Rpdml0eSA9IF9kZXJlcV8oOCk7XHJcblxyXG4vKipcclxuICogTG9jYWxpemFibGUgZWxlbWVudHMgb2YgdGhlIFNlbGVjdGl2aXR5IFRlbXBsYXRlcy5cclxuICpcclxuICogQmUgYXdhcmUgdGhhdCB0aGVzZSBzdHJpbmdzIGFyZSBhZGRlZCBzdHJhaWdodCB0byB0aGUgSFRNTCBvdXRwdXQgb2YgdGhlIHRlbXBsYXRlcywgc28gYW55XHJcbiAqIG5vbi1zYWZlIHN0cmluZ3Mgc2hvdWxkIGJlIGVzY2FwZWQuXHJcbiAqL1xyXG5TZWxlY3Rpdml0eS5Mb2NhbGUgPSB7XHJcblxyXG4gICAgYWpheEVycm9yOiBmdW5jdGlvbih0ZXJtKSB7IHJldHVybiAnRmFpbGVkIHRvIGZldGNoIHJlc3VsdHMgZm9yIDxiPicgKyBlc2NhcGUodGVybSkgKyAnPC9iPic7IH0sXHJcbiAgICBsb2FkaW5nOiAnTG9hZGluZy4uLicsXHJcbiAgICBsb2FkTW9yZTogJ0xvYWQgbW9yZS4uLicsXHJcbiAgICBuZWVkTW9yZUNoYXJhY3RlcnM6IGZ1bmN0aW9uKG51bUNoYXJhY3RlcnMpIHtcclxuICAgICAgICByZXR1cm4gJ0VudGVyICcgKyBudW1DaGFyYWN0ZXJzICsgJyBtb3JlIGNoYXJhY3RlcnMgdG8gc2VhcmNoJztcclxuICAgIH0sXHJcbiAgICBub1Jlc3VsdHM6ICdObyByZXN1bHRzIGZvdW5kJyxcclxuICAgIG5vUmVzdWx0c0ZvclRlcm06IGZ1bmN0aW9uKHRlcm0pIHsgcmV0dXJuICdObyByZXN1bHRzIGZvciA8Yj4nICsgZXNjYXBlKHRlcm0pICsgJzwvYj4nOyB9XHJcblxyXG59O1xyXG5cclxufSx7XCI0XCI6NCxcIjhcIjo4fV0sMTQ6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG52YXIgJCA9IHdpbmRvdy5qUXVlcnkgfHwgd2luZG93LlplcHRvO1xyXG5cclxudmFyIFNlbGVjdGl2aXR5ID0gX2RlcmVxXyg4KTtcclxuXHJcbnZhciBLRVlfQkFDS1NQQUNFID0gODtcclxudmFyIEtFWV9ERUxFVEUgPSA0NjtcclxudmFyIEtFWV9FTlRFUiA9IDEzO1xyXG5cclxuLyoqXHJcbiAqIE11bHRpcGxlU2VsZWN0aXZpdHkgQ29uc3RydWN0b3IuXHJcbiAqXHJcbiAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0LiBBY2NlcHRzIGFsbCBvcHRpb25zIGZyb20gdGhlIFNlbGVjdGl2aXR5IEJhc2UgQ29uc3RydWN0b3IgaW5cclxuICogICAgICAgICAgICAgICAgYWRkaXRpb24gdG8gdGhvc2UgYWNjZXB0ZWQgYnkgTXVsdGlwbGVTZWxlY3Rpdml0eS5zZXRPcHRpb25zKCkuXHJcbiAqL1xyXG5mdW5jdGlvbiBNdWx0aXBsZVNlbGVjdGl2aXR5KG9wdGlvbnMpIHtcclxuXHJcbiAgICBTZWxlY3Rpdml0eS5jYWxsKHRoaXMsIG9wdGlvbnMpO1xyXG5cclxuICAgIHRoaXMuJGVsLmh0bWwodGhpcy50ZW1wbGF0ZSgnbXVsdGlwbGVTZWxlY3RJbnB1dCcsIHsgZW5hYmxlZDogdGhpcy5lbmFibGVkIH0pKVxyXG4gICAgICAgICAgICAudHJpZ2dlcignc2VsZWN0aXZpdHktaW5pdCcsICdtdWx0aXBsZScpO1xyXG5cclxuICAgIHRoaXMuX2hpZ2hsaWdodGVkSXRlbUlkID0gbnVsbDtcclxuXHJcbiAgICB0aGlzLmluaXRTZWFyY2hJbnB1dCh0aGlzLiQoJy5zZWxlY3Rpdml0eS1tdWx0aXBsZS1pbnB1dDpub3QoLnNlbGVjdGl2aXR5LXdpZHRoLWRldGVjdG9yKScpKTtcclxuXHJcbiAgICB0aGlzLnJlcmVuZGVyU2VsZWN0aW9uKCk7XHJcblxyXG4gICAgaWYgKCFvcHRpb25zLnBvc2l0aW9uRHJvcGRvd24pIHtcclxuICAgICAgICAvLyBkcm9wZG93bnMgZm9yIG11bHRpcGxlLXZhbHVlIGlucHV0cyBzaG91bGQgb3BlbiBiZWxvdyB0aGUgc2VsZWN0IGJveCxcclxuICAgICAgICAvLyB1bmxlc3MgdGhlcmUgaXMgbm90IGVub3VnaCBzcGFjZSBiZWxvdywgYnV0IHRoZXJlIGlzIHNwYWNlIGVub3VnaCBhYm92ZSwgdGhlbiBpdCBzaG91bGRcclxuICAgICAgICAvLyBvcGVuIHVwd2FyZHNcclxuICAgICAgICB0aGlzLm9wdGlvbnMucG9zaXRpb25Ecm9wZG93biA9IGZ1bmN0aW9uKCRlbCwgJHNlbGVjdEVsKSB7XHJcbiAgICAgICAgICAgIHZhciBwb3NpdGlvbiA9ICRzZWxlY3RFbC5wb3NpdGlvbigpLFxyXG4gICAgICAgICAgICAgICAgZHJvcGRvd25IZWlnaHQgPSAkZWwuaGVpZ2h0KCksXHJcbiAgICAgICAgICAgICAgICBzZWxlY3RIZWlnaHQgPSAkc2VsZWN0RWwuaGVpZ2h0KCksXHJcbiAgICAgICAgICAgICAgICB0b3AgPSAkc2VsZWN0RWxbMF0uZ2V0Qm91bmRpbmdDbGllbnRSZWN0KCkudG9wLFxyXG4gICAgICAgICAgICAgICAgYm90dG9tID0gdG9wICsgc2VsZWN0SGVpZ2h0ICsgZHJvcGRvd25IZWlnaHQsXHJcbiAgICAgICAgICAgICAgICBvcGVuVXB3YXJkcyA9ICh0eXBlb2Ygd2luZG93ICE9PSAndW5kZWZpbmVkJyAmJiBib3R0b20gPiAkKHdpbmRvdykuaGVpZ2h0KCkgJiZcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHRvcCAtIGRyb3Bkb3duSGVpZ2h0ID4gMCk7XHJcblxyXG4gICAgICAgICAgICB2YXIgd2lkdGggPSAkc2VsZWN0RWwub3V0ZXJXaWR0aCA/ICRzZWxlY3RFbC5vdXRlcldpZHRoKCkgOiAkc2VsZWN0RWwud2lkdGgoKTtcclxuICAgICAgICAgICAgJGVsLmNzcyh7XHJcbiAgICAgICAgICAgICAgICBsZWZ0OiBwb3NpdGlvbi5sZWZ0ICsgJ3B4JyxcclxuICAgICAgICAgICAgICAgIHRvcDogcG9zaXRpb24udG9wICsgKG9wZW5VcHdhcmRzID8gLWRyb3Bkb3duSGVpZ2h0IDogc2VsZWN0SGVpZ2h0KSArICdweCdcclxuICAgICAgICAgICAgfSkud2lkdGgod2lkdGgpO1xyXG4gICAgICAgIH07XHJcbiAgICB9XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBNZXRob2RzLlxyXG4gKi9cclxudmFyIGNhbGxTdXBlciA9IFNlbGVjdGl2aXR5LmluaGVyaXRzKE11bHRpcGxlU2VsZWN0aXZpdHksIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEFkZHMgYW4gaXRlbSB0byB0aGUgc2VsZWN0aW9uLCBpZiBpdCdzIG5vdCBzZWxlY3RlZCB5ZXQuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIGl0ZW0gVGhlIGl0ZW0gdG8gYWRkLiBNYXkgYmUgYW4gaXRlbSB3aXRoICdpZCcgYW5kICd0ZXh0JyBwcm9wZXJ0aWVzIG9yIGp1c3QgYW4gSUQuXHJcbiAgICAgKi9cclxuICAgIGFkZDogZnVuY3Rpb24oaXRlbSkge1xyXG5cclxuICAgICAgICB2YXIgaXRlbUlzSWQgPSBTZWxlY3Rpdml0eS5pc1ZhbGlkSWQoaXRlbSk7XHJcbiAgICAgICAgdmFyIGlkID0gKGl0ZW1Jc0lkID8gaXRlbSA6IHRoaXMudmFsaWRhdGVJdGVtKGl0ZW0pICYmIGl0ZW0uaWQpO1xyXG5cclxuICAgICAgICBpZiAodGhpcy5fdmFsdWUuaW5kZXhPZihpZCkgPT09IC0xKSB7XHJcbiAgICAgICAgICAgIHRoaXMuX3ZhbHVlLnB1c2goaWQpO1xyXG5cclxuICAgICAgICAgICAgaWYgKGl0ZW1Jc0lkICYmIHRoaXMub3B0aW9ucy5pbml0U2VsZWN0aW9uKSB7XHJcbiAgICAgICAgICAgICAgICB0aGlzLm9wdGlvbnMuaW5pdFNlbGVjdGlvbihbaWRdLCBmdW5jdGlvbihkYXRhKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKHRoaXMuX3ZhbHVlLmluZGV4T2YoaWQpID4gLTEpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgaXRlbSA9IHRoaXMudmFsaWRhdGVJdGVtKGRhdGFbMF0pO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB0aGlzLl9kYXRhLnB1c2goaXRlbSk7XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICB0aGlzLnRyaWdnZXJDaGFuZ2UoeyBhZGRlZDogaXRlbSB9KTtcclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICB9LmJpbmQodGhpcykpO1xyXG4gICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgaWYgKGl0ZW1Jc0lkKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgaXRlbSA9IHRoaXMuZ2V0SXRlbUZvcklkKGlkKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIHRoaXMuX2RhdGEucHVzaChpdGVtKTtcclxuXHJcbiAgICAgICAgICAgICAgICB0aGlzLnRyaWdnZXJDaGFuZ2UoeyBhZGRlZDogaXRlbSB9KTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdGhpcy4kc2VhcmNoSW5wdXQudmFsKCcnKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDbGVhcnMgdGhlIGRhdGEgYW5kIHZhbHVlLlxyXG4gICAgICovXHJcbiAgICBjbGVhcjogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHRoaXMuZGF0YShbXSk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogRXZlbnRzIG1hcC5cclxuICAgICAqXHJcbiAgICAgKiBGb2xsb3dzIHRoZSBzYW1lIGZvcm1hdCBhcyBCYWNrYm9uZTogaHR0cDovL2JhY2tib25lanMub3JnLyNWaWV3LWRlbGVnYXRlRXZlbnRzXHJcbiAgICAgKi9cclxuICAgIGV2ZW50czoge1xyXG4gICAgICAgICdjaGFuZ2UnOiAncmVyZW5kZXJTZWxlY3Rpb24nLFxyXG4gICAgICAgICdjaGFuZ2UgLnNlbGVjdGl2aXR5LW11bHRpcGxlLWlucHV0JzogZnVuY3Rpb24oKSB7IHJldHVybiBmYWxzZTsgfSxcclxuICAgICAgICAnY2xpY2snOiAnX2NsaWNrZWQnLFxyXG4gICAgICAgICdjbGljayAuc2VsZWN0aXZpdHktbXVsdGlwbGUtc2VsZWN0ZWQtaXRlbSc6ICdfaXRlbUNsaWNrZWQnLFxyXG4gICAgICAgICdrZXlkb3duIC5zZWxlY3Rpdml0eS1tdWx0aXBsZS1pbnB1dCc6ICdfa2V5SGVsZCcsXHJcbiAgICAgICAgJ2tleXVwIC5zZWxlY3Rpdml0eS1tdWx0aXBsZS1pbnB1dCc6ICdfa2V5UmVsZWFzZWQnLFxyXG4gICAgICAgICdwYXN0ZSAuc2VsZWN0aXZpdHktbXVsdGlwbGUtaW5wdXQnOiAnX29uUGFzdGUnLFxyXG4gICAgICAgICdzZWxlY3Rpdml0eS1zZWxlY3RlZCc6ICdfcmVzdWx0U2VsZWN0ZWQnXHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQGluaGVyaXRcclxuICAgICAqL1xyXG4gICAgZmlsdGVyUmVzdWx0czogZnVuY3Rpb24ocmVzdWx0cykge1xyXG5cclxuICAgICAgICByZXR1cm4gcmVzdWx0cy5maWx0ZXIoZnVuY3Rpb24oaXRlbSkge1xyXG4gICAgICAgICAgICByZXR1cm4gIVNlbGVjdGl2aXR5LmZpbmRCeUlkKHRoaXMuX2RhdGEsIGl0ZW0uaWQpO1xyXG4gICAgICAgIH0sIHRoaXMpO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJldHVybnMgdGhlIGNvcnJlY3QgZGF0YSBmb3IgYSBnaXZlbiB2YWx1ZS5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gdmFsdWUgVGhlIHZhbHVlIHRvIGdldCB0aGUgZGF0YSBmb3IuIFNob3VsZCBiZSBhbiBhcnJheSBvZiBJRHMuXHJcbiAgICAgKlxyXG4gICAgICogQHJldHVybiBUaGUgY29ycmVzcG9uZGluZyBkYXRhLiBXaWxsIGJlIGFuIGFycmF5IG9mIG9iamVjdHMgd2l0aCAnaWQnIGFuZCAndGV4dCcgcHJvcGVydGllcy5cclxuICAgICAqICAgICAgICAgTm90ZSB0aGF0IGlmIG5vIGl0ZW1zIGFyZSBkZWZpbmVkLCB0aGlzIG1ldGhvZCBhc3N1bWVzIHRoZSB0ZXh0IGxhYmVscyB3aWxsIGJlIGVxdWFsXHJcbiAgICAgKiAgICAgICAgIHRvIHRoZSBJRHMuXHJcbiAgICAgKi9cclxuICAgIGdldERhdGFGb3JWYWx1ZTogZnVuY3Rpb24odmFsdWUpIHtcclxuXHJcbiAgICAgICAgcmV0dXJuIHZhbHVlLm1hcCh0aGlzLmdldEl0ZW1Gb3JJZCwgdGhpcykuZmlsdGVyKGZ1bmN0aW9uKGl0ZW0pIHsgcmV0dXJuICEhaXRlbTsgfSk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmV0dXJucyB0aGUgY29ycmVjdCB2YWx1ZSBmb3IgdGhlIGdpdmVuIGRhdGEuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIGRhdGEgVGhlIGRhdGEgdG8gZ2V0IHRoZSB2YWx1ZSBmb3IuIFNob3VsZCBiZSBhbiBhcnJheSBvZiBvYmplY3RzIHdpdGggJ2lkJyBhbmQgJ3RleHQnXHJcbiAgICAgKiAgICAgICAgICAgICBwcm9wZXJ0aWVzLlxyXG4gICAgICpcclxuICAgICAqIEByZXR1cm4gVGhlIGNvcnJlc3BvbmRpbmcgdmFsdWUuIFdpbGwgYmUgYW4gYXJyYXkgb2YgSURzLlxyXG4gICAgICovXHJcbiAgICBnZXRWYWx1ZUZvckRhdGE6IGZ1bmN0aW9uKGRhdGEpIHtcclxuXHJcbiAgICAgICAgcmV0dXJuIGRhdGEubWFwKGZ1bmN0aW9uKGl0ZW0pIHsgcmV0dXJuIGl0ZW0uaWQ7IH0pO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlbW92ZXMgYW4gaXRlbSBmcm9tIHRoZSBzZWxlY3Rpb24sIGlmIGl0IGlzIHNlbGVjdGVkLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBpdGVtIFRoZSBpdGVtIHRvIHJlbW92ZS4gTWF5IGJlIGFuIGl0ZW0gd2l0aCAnaWQnIGFuZCAndGV4dCcgcHJvcGVydGllcyBvciBqdXN0IGFuIElELlxyXG4gICAgICovXHJcbiAgICByZW1vdmU6IGZ1bmN0aW9uKGl0ZW0pIHtcclxuXHJcbiAgICAgICAgdmFyIGlkID0gKCQudHlwZShpdGVtKSA9PT0gJ29iamVjdCcgPyBpdGVtLmlkIDogaXRlbSk7XHJcblxyXG4gICAgICAgIHZhciByZW1vdmVkSXRlbTtcclxuICAgICAgICB2YXIgaW5kZXggPSBTZWxlY3Rpdml0eS5maW5kSW5kZXhCeUlkKHRoaXMuX2RhdGEsIGlkKTtcclxuICAgICAgICBpZiAoaW5kZXggPiAtMSkge1xyXG4gICAgICAgICAgICByZW1vdmVkSXRlbSA9IHRoaXMuX2RhdGFbaW5kZXhdO1xyXG4gICAgICAgICAgICB0aGlzLl9kYXRhLnNwbGljZShpbmRleCwgMSk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBpZiAodGhpcy5fdmFsdWVbaW5kZXhdICE9PSBpZCkge1xyXG4gICAgICAgICAgICBpbmRleCA9IHRoaXMuX3ZhbHVlLmluZGV4T2YoaWQpO1xyXG4gICAgICAgIH1cclxuICAgICAgICBpZiAoaW5kZXggPiAtMSkge1xyXG4gICAgICAgICAgICB0aGlzLl92YWx1ZS5zcGxpY2UoaW5kZXgsIDEpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgaWYgKHJlbW92ZWRJdGVtKSB7XHJcbiAgICAgICAgICAgIHRoaXMudHJpZ2dlckNoYW5nZSh7IHJlbW92ZWQ6IHJlbW92ZWRJdGVtIH0pO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgaWYgKGlkID09PSB0aGlzLl9oaWdobGlnaHRlZEl0ZW1JZCkge1xyXG4gICAgICAgICAgICB0aGlzLl9oaWdobGlnaHRlZEl0ZW1JZCA9IG51bGw7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlLXJlbmRlcnMgdGhlIHNlbGVjdGlvbi5cclxuICAgICAqXHJcbiAgICAgKiBOb3JtYWxseSB0aGUgVUkgaXMgYXV0b21hdGljYWxseSB1cGRhdGVkIHdoZW5ldmVyIHRoZSBzZWxlY3Rpb24gY2hhbmdlcywgYnV0IHlvdSBtYXkgd2FudCB0b1xyXG4gICAgICogY2FsbCB0aGlzIG1ldGhvZCBleHBsaWNpdGx5IGlmIHlvdSd2ZSB1cGRhdGVkIHRoZSBzZWxlY3Rpb24gd2l0aCB0aGUgdHJpZ2dlckNoYW5nZSBvcHRpb24gc2V0XHJcbiAgICAgKiB0byBmYWxzZS5cclxuICAgICAqL1xyXG4gICAgcmVyZW5kZXJTZWxlY3Rpb246IGZ1bmN0aW9uKGV2ZW50KSB7XHJcblxyXG4gICAgICAgIGV2ZW50ID0gZXZlbnQgfHwge307XHJcblxyXG4gICAgICAgIGlmIChldmVudC5hZGRlZCkge1xyXG4gICAgICAgICAgICB0aGlzLl9yZW5kZXJTZWxlY3RlZEl0ZW0oZXZlbnQuYWRkZWQpO1xyXG5cclxuICAgICAgICAgICAgdGhpcy5fc2Nyb2xsVG9Cb3R0b20oKTtcclxuICAgICAgICB9IGVsc2UgaWYgKGV2ZW50LnJlbW92ZWQpIHtcclxuICAgICAgICAgICAgdmFyIHF1b3RlZElkID0gU2VsZWN0aXZpdHkucXVvdGVDc3NBdHRyKGV2ZW50LnJlbW92ZWQuaWQpO1xyXG4gICAgICAgICAgICB0aGlzLiQoJy5zZWxlY3Rpdml0eS1tdWx0aXBsZS1zZWxlY3RlZC1pdGVtW2RhdGEtaXRlbS1pZD0nICsgcXVvdGVkSWQgKyAnXScpLnJlbW92ZSgpO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRoaXMuJCgnLnNlbGVjdGl2aXR5LW11bHRpcGxlLXNlbGVjdGVkLWl0ZW0nKS5yZW1vdmUoKTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMuX2RhdGEuZm9yRWFjaCh0aGlzLl9yZW5kZXJTZWxlY3RlZEl0ZW0sIHRoaXMpO1xyXG5cclxuICAgICAgICAgICAgdGhpcy5fdXBkYXRlSW5wdXRXaWR0aCgpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgaWYgKGV2ZW50LmFkZGVkIHx8IGV2ZW50LnJlbW92ZWQpIHtcclxuICAgICAgICAgICAgaWYgKHRoaXMuZHJvcGRvd24pIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuZHJvcGRvd24uc2hvd1Jlc3VsdHModGhpcy5maWx0ZXJSZXN1bHRzKHRoaXMuZHJvcGRvd24ucmVzdWx0cyksIHtcclxuICAgICAgICAgICAgICAgICAgICBoYXNNb3JlOiB0aGlzLmRyb3Bkb3duLmhhc01vcmVcclxuICAgICAgICAgICAgICAgIH0pO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICBpZiAodGhpcy5oYXNLZXlib2FyZCkge1xyXG4gICAgICAgICAgICAgICAgdGhpcy5mb2N1cygpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLnBvc2l0aW9uRHJvcGRvd24oKTtcclxuXHJcbiAgICAgICAgdGhpcy5fdXBkYXRlUGxhY2Vob2xkZXIoKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAaW5oZXJpdFxyXG4gICAgICovXHJcbiAgICBzZWFyY2g6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB2YXIgdGVybSA9IHRoaXMuJHNlYXJjaElucHV0LnZhbCgpO1xyXG5cclxuICAgICAgICBpZiAodGhpcy5vcHRpb25zLnRva2VuaXplcikge1xyXG4gICAgICAgICAgICB0ZXJtID0gdGhpcy5vcHRpb25zLnRva2VuaXplcih0ZXJtLCB0aGlzLl9kYXRhLCB0aGlzLmFkZC5iaW5kKHRoaXMpLCB0aGlzLm9wdGlvbnMpO1xyXG5cclxuICAgICAgICAgICAgaWYgKCQudHlwZSh0ZXJtKSA9PT0gJ3N0cmluZycgJiYgdGVybSAhPT0gdGhpcy4kc2VhcmNoSW5wdXQudmFsKCkpIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuJHNlYXJjaElucHV0LnZhbCh0ZXJtKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgaWYgKHRoaXMuZHJvcGRvd24pIHtcclxuICAgICAgICAgICAgY2FsbFN1cGVyKHRoaXMsICdzZWFyY2gnKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQGluaGVyaXRcclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25zIG9iamVjdC4gSW4gYWRkaXRpb24gdG8gdGhlIG9wdGlvbnMgc3VwcG9ydGVkIGluIHRoZSBiYXNlXHJcbiAgICAgKiAgICAgICAgICAgICAgICBpbXBsZW1lbnRhdGlvbiwgdGhpcyBtYXkgY29udGFpbiB0aGUgZm9sbG93aW5nIHByb3BlcnRpZXM6XHJcbiAgICAgKiAgICAgICAgICAgICAgICBiYWNrc3BhY2VIaWdobGlnaHRzQmVmb3JlRGVsZXRlIC0gSWYgc2V0IHRvIHRydWUsIHdoZW4gdGhlIHVzZXIgZW50ZXJzIGFcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBiYWNrc3BhY2Ugd2hpbGUgdGhlcmUgaXMgbm8gdGV4dCBpbiB0aGVcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBzZWFyY2ggZmllbGQgYnV0IHRoZXJlIGFyZSBzZWxlY3RlZCBpdGVtcyxcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB0aGUgbGFzdCBzZWxlY3RlZCBpdGVtIHdpbGwgYmUgaGlnaGxpZ2h0ZWRcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBhbmQgd2hlbiBhIHNlY29uZCBiYWNrc3BhY2UgaXMgZW50ZXJlZCB0aGVcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBpdGVtIGlzIGRlbGV0ZWQuIElmIGZhbHNlLCB0aGUgaXRlbSBnZXRzXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgZGVsZXRlZCBvbiB0aGUgZmlyc3QgYmFja3NwYWNlLiBUaGUgZGVmYXVsdFxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHZhbHVlIGlzIHRydWUgb24gZGV2aWNlcyB0aGF0IGhhdmUgdG91Y2hcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBpbnB1dCBhbmQgZmFsc2Ugb24gZGV2aWNlcyB0aGF0IGRvbid0LlxyXG4gICAgICogICAgICAgICAgICAgICAgY3JlYXRlVG9rZW5JdGVtIC0gRnVuY3Rpb24gdG8gY3JlYXRlIGEgbmV3IGl0ZW0gZnJvbSBhIHVzZXIncyBzZWFyY2ggdGVybS5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFRoaXMgaXMgdXNlZCB0byB0dXJuIHRoZSB0ZXJtIGludG8gYW4gaXRlbSB3aGVuIGRyb3Bkb3duc1xyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYXJlIGRpc2FibGVkIGFuZCB0aGUgdXNlciBwcmVzc2VzIEVudGVyLiBJdCBpcyBhbHNvIHVzZWQgYnlcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHRoZSBkZWZhdWx0IHRva2VuaXplciB0byBjcmVhdGUgaXRlbXMgZm9yIGluZGl2aWR1YWwgdG9rZW5zLlxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgVGhlIGZ1bmN0aW9uIHJlY2VpdmVzIGEgJ3Rva2VuJyBwYXJhbWV0ZXIgd2hpY2ggaXMgdGhlXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBzZWFyY2ggdGVybSAob3IgcGFydCBvZiBhIHNlYXJjaCB0ZXJtKSB0byBjcmVhdGUgYW4gaXRlbSBmb3JcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGFuZCBtdXN0IHJldHVybiBhbiBpdGVtIG9iamVjdCB3aXRoICdpZCcgYW5kICd0ZXh0J1xyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcGVydGllcyBvciBudWxsIGlmIG5vIHRva2VuIGNhbiBiZSBjcmVhdGVkIGZyb20gdGhlIHRlcm0uXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBUaGUgZGVmYXVsdCBpcyBhIGZ1bmN0aW9uIHRoYXQgcmV0dXJucyBhbiBpdGVtIHdoZXJlIHRoZSBpZFxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYW5kIHRleHQgYm90aCBtYXRjaCB0aGUgdG9rZW4gZm9yIGFueSBub24tZW1wdHkgc3RyaW5nIGFuZFxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgd2hpY2ggcmV0dXJucyBudWxsIG90aGVyd2lzZS5cclxuICAgICAqICAgICAgICAgICAgICAgIHRva2VuaXplciAtIEZ1bmN0aW9uIGZvciB0b2tlbml6aW5nIHNlYXJjaCB0ZXJtcy4gV2lsbCByZWNlaXZlIHRoZSBmb2xsb3dpbmdcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgIHBhcmFtZXRlcnM6XHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpbnB1dCAtIFRoZSBpbnB1dCBzdHJpbmcgdG8gdG9rZW5pemUuXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICBzZWxlY3Rpb24gLSBUaGUgY3VycmVudCBzZWxlY3Rpb24gZGF0YS5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgIGNyZWF0ZVRva2VuIC0gQ2FsbGJhY2sgdG8gY3JlYXRlIGEgdG9rZW4gZnJvbSB0aGUgc2VhcmNoIHRlcm1zLlxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBTaG91bGQgYmUgcGFzc2VkIGFuIGl0ZW0gb2JqZWN0IHdpdGggJ2lkJyBhbmQgJ3RleHQnXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BlcnRpZXMuXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICBvcHRpb25zIC0gVGhlIG9wdGlvbnMgc2V0IG9uIHRoZSBTZWxlY3Rpdml0eSBpbnN0YW5jZS5cclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgIEFueSBzdHJpbmcgcmV0dXJuZWQgYnkgdGhlIHRva2VuaXplciBmdW5jdGlvbiBpcyB0cmVhdGVkIGFzIHRoZVxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVtYWluZGVyIG9mIHVudG9rZW5pemVkIGlucHV0LlxyXG4gICAgICovXHJcbiAgICBzZXRPcHRpb25zOiBmdW5jdGlvbihvcHRpb25zKSB7XHJcblxyXG4gICAgICAgIG9wdGlvbnMgPSBvcHRpb25zIHx8IHt9O1xyXG5cclxuICAgICAgICB2YXIgYmFja3NwYWNlSGlnaGxpZ2h0c0JlZm9yZURlbGV0ZSA9ICdiYWNrc3BhY2VIaWdobGlnaHRzQmVmb3JlRGVsZXRlJztcclxuICAgICAgICBpZiAob3B0aW9uc1tiYWNrc3BhY2VIaWdobGlnaHRzQmVmb3JlRGVsZXRlXSA9PT0gdW5kZWZpbmVkKSB7XHJcbiAgICAgICAgICAgIG9wdGlvbnNbYmFja3NwYWNlSGlnaGxpZ2h0c0JlZm9yZURlbGV0ZV0gPSB0aGlzLmhhc1RvdWNoO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgb3B0aW9ucy5hbGxvd2VkVHlwZXMgPSBvcHRpb25zLmFsbG93ZWRUeXBlcyB8fCB7fTtcclxuICAgICAgICBvcHRpb25zLmFsbG93ZWRUeXBlc1tiYWNrc3BhY2VIaWdobGlnaHRzQmVmb3JlRGVsZXRlXSA9ICdib29sZWFuJztcclxuXHJcbiAgICAgICAgdmFyIHdhc0VuYWJsZWQgPSB0aGlzLmVuYWJsZWQ7XHJcblxyXG4gICAgICAgIGNhbGxTdXBlcih0aGlzLCAnc2V0T3B0aW9ucycsIG9wdGlvbnMpO1xyXG5cclxuICAgICAgICBpZiAod2FzRW5hYmxlZCAhPT0gdGhpcy5lbmFibGVkKSB7XHJcbiAgICAgICAgICAgIHRoaXMuJGVsLmh0bWwodGhpcy50ZW1wbGF0ZSgnbXVsdGlwbGVTZWxlY3RJbnB1dCcsIHsgZW5hYmxlZDogdGhpcy5lbmFibGVkIH0pKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogVmFsaWRhdGVzIGRhdGEgdG8gc2V0LiBUaHJvd3MgYW4gZXhjZXB0aW9uIGlmIHRoZSBkYXRhIGlzIGludmFsaWQuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIGRhdGEgVGhlIGRhdGEgdG8gdmFsaWRhdGUuIFNob3VsZCBiZSBhbiBhcnJheSBvZiBvYmplY3RzIHdpdGggJ2lkJyBhbmQgJ3RleHQnXHJcbiAgICAgKiAgICAgICAgICAgICBwcm9wZXJ0aWVzLlxyXG4gICAgICpcclxuICAgICAqIEByZXR1cm4gVGhlIHZhbGlkYXRlZCBkYXRhLiBUaGlzIG1heSBkaWZmZXIgZnJvbSB0aGUgaW5wdXQgZGF0YS5cclxuICAgICAqL1xyXG4gICAgdmFsaWRhdGVEYXRhOiBmdW5jdGlvbihkYXRhKSB7XHJcblxyXG4gICAgICAgIGlmIChkYXRhID09PSBudWxsKSB7XHJcbiAgICAgICAgICAgIHJldHVybiBbXTtcclxuICAgICAgICB9IGVsc2UgaWYgKCQudHlwZShkYXRhKSA9PT0gJ2FycmF5Jykge1xyXG4gICAgICAgICAgICByZXR1cm4gZGF0YS5tYXAodGhpcy52YWxpZGF0ZUl0ZW0sIHRoaXMpO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcignRGF0YSBmb3IgTXVsdGlTZWxlY3Rpdml0eSBpbnN0YW5jZSBzaG91bGQgYmUgYXJyYXknKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogVmFsaWRhdGVzIGEgdmFsdWUgdG8gc2V0LiBUaHJvd3MgYW4gZXhjZXB0aW9uIGlmIHRoZSB2YWx1ZSBpcyBpbnZhbGlkLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSB2YWx1ZSBUaGUgdmFsdWUgdG8gdmFsaWRhdGUuIFNob3VsZCBiZSBhbiBhcnJheSBvZiBJRHMuXHJcbiAgICAgKlxyXG4gICAgICogQHJldHVybiBUaGUgdmFsaWRhdGVkIHZhbHVlLiBUaGlzIG1heSBkaWZmZXIgZnJvbSB0aGUgaW5wdXQgdmFsdWUuXHJcbiAgICAgKi9cclxuICAgIHZhbGlkYXRlVmFsdWU6IGZ1bmN0aW9uKHZhbHVlKSB7XHJcblxyXG4gICAgICAgIGlmICh2YWx1ZSA9PT0gbnVsbCkge1xyXG4gICAgICAgICAgICByZXR1cm4gW107XHJcbiAgICAgICAgfSBlbHNlIGlmICgkLnR5cGUodmFsdWUpID09PSAnYXJyYXknKSB7XHJcbiAgICAgICAgICAgIGlmICh2YWx1ZS5ldmVyeShTZWxlY3Rpdml0eS5pc1ZhbGlkSWQpKSB7XHJcbiAgICAgICAgICAgICAgICByZXR1cm4gdmFsdWU7XHJcbiAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoJ1ZhbHVlIGNvbnRhaW5zIGludmFsaWQgSURzJyk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoJ1ZhbHVlIGZvciBNdWx0aVNlbGVjdGl2aXR5IGluc3RhbmNlIHNob3VsZCBiZSBhbiBhcnJheScpO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfYmFja3NwYWNlUHJlc3NlZDogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLm9wdGlvbnMuYmFja3NwYWNlSGlnaGxpZ2h0c0JlZm9yZURlbGV0ZSkge1xyXG4gICAgICAgICAgICBpZiAodGhpcy5faGlnaGxpZ2h0ZWRJdGVtSWQpIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuX2RlbGV0ZVByZXNzZWQoKTtcclxuICAgICAgICAgICAgfSBlbHNlIGlmICh0aGlzLl92YWx1ZS5sZW5ndGgpIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuX2hpZ2hsaWdodEl0ZW0odGhpcy5fdmFsdWUuc2xpY2UoLTEpWzBdKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5fdmFsdWUubGVuZ3RoKSB7XHJcbiAgICAgICAgICAgIHRoaXMucmVtb3ZlKHRoaXMuX3ZhbHVlLnNsaWNlKC0xKVswXSk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9jbGlja2VkOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMuZW5hYmxlZCkge1xyXG4gICAgICAgICAgICB0aGlzLmZvY3VzKCk7XHJcblxyXG4gICAgICAgICAgICB0aGlzLl9vcGVuKCk7XHJcblxyXG4gICAgICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9jcmVhdGVUb2tlbjogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHZhciB0ZXJtID0gdGhpcy4kc2VhcmNoSW5wdXQudmFsKCk7XHJcbiAgICAgICAgdmFyIGNyZWF0ZVRva2VuSXRlbSA9IHRoaXMub3B0aW9ucy5jcmVhdGVUb2tlbkl0ZW07XHJcblxyXG4gICAgICAgIGlmICh0ZXJtICYmIGNyZWF0ZVRva2VuSXRlbSkge1xyXG4gICAgICAgICAgICB2YXIgaXRlbSA9IGNyZWF0ZVRva2VuSXRlbSh0ZXJtKTtcclxuICAgICAgICAgICAgaWYgKGl0ZW0pIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuYWRkKGl0ZW0pO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9kZWxldGVQcmVzc2VkOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMuX2hpZ2hsaWdodGVkSXRlbUlkKSB7XHJcbiAgICAgICAgICAgIHRoaXMucmVtb3ZlKHRoaXMuX2hpZ2hsaWdodGVkSXRlbUlkKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX2hpZ2hsaWdodEl0ZW06IGZ1bmN0aW9uKGlkKSB7XHJcblxyXG4gICAgICAgIHRoaXMuX2hpZ2hsaWdodGVkSXRlbUlkID0gaWQ7XHJcbiAgICAgICAgdGhpcy4kKCcuc2VsZWN0aXZpdHktbXVsdGlwbGUtc2VsZWN0ZWQtaXRlbScpLnJlbW92ZUNsYXNzKCdoaWdobGlnaHRlZCcpXHJcbiAgICAgICAgICAgIC5maWx0ZXIoJ1tkYXRhLWl0ZW0taWQ9JyArIFNlbGVjdGl2aXR5LnF1b3RlQ3NzQXR0cihpZCkgKyAnXScpLmFkZENsYXNzKCdoaWdobGlnaHRlZCcpO1xyXG5cclxuICAgICAgICBpZiAodGhpcy5oYXNLZXlib2FyZCkge1xyXG4gICAgICAgICAgICB0aGlzLmZvY3VzKCk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9pdGVtQ2xpY2tlZDogZnVuY3Rpb24oZXZlbnQpIHtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMuZW5hYmxlZCkge1xyXG4gICAgICAgICAgICB0aGlzLl9oaWdobGlnaHRJdGVtKHRoaXMuX2dldEl0ZW1JZChldmVudCkpO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfaXRlbVJlbW92ZUNsaWNrZWQ6IGZ1bmN0aW9uKGV2ZW50KSB7XHJcblxyXG4gICAgICAgIHRoaXMucmVtb3ZlKHRoaXMuX2dldEl0ZW1JZChldmVudCkpO1xyXG5cclxuICAgICAgICB0aGlzLl91cGRhdGVJbnB1dFdpZHRoKCk7XHJcblxyXG4gICAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfa2V5SGVsZDogZnVuY3Rpb24oZXZlbnQpIHtcclxuXHJcbiAgICAgICAgdGhpcy5fb3JpZ2luYWxWYWx1ZSA9IHRoaXMuJHNlYXJjaElucHV0LnZhbCgpO1xyXG5cclxuICAgICAgICBpZiAoZXZlbnQua2V5Q29kZSA9PT0gS0VZX0VOVEVSICYmICFldmVudC5jdHJsS2V5KSB7XHJcbiAgICAgICAgICAgIGV2ZW50LnByZXZlbnREZWZhdWx0KCk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9rZXlSZWxlYXNlZDogZnVuY3Rpb24oZXZlbnQpIHtcclxuXHJcbiAgICAgICAgdmFyIGlucHV0SGFkVGV4dCA9ICEhdGhpcy5fb3JpZ2luYWxWYWx1ZTtcclxuXHJcbiAgICAgICAgaWYgKGV2ZW50LmtleUNvZGUgPT09IEtFWV9FTlRFUiAmJiAhZXZlbnQuY3RybEtleSkge1xyXG4gICAgICAgICAgICBpZiAodGhpcy5vcHRpb25zLmNyZWF0ZVRva2VuSXRlbSkge1xyXG4gICAgICAgICAgICAgICAgdGhpcy5fY3JlYXRlVG9rZW4oKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0gZWxzZSBpZiAoZXZlbnQua2V5Q29kZSA9PT0gS0VZX0JBQ0tTUEFDRSAmJiAhaW5wdXRIYWRUZXh0KSB7XHJcbiAgICAgICAgICAgIHRoaXMuX2JhY2tzcGFjZVByZXNzZWQoKTtcclxuICAgICAgICB9IGVsc2UgaWYgKGV2ZW50LmtleUNvZGUgPT09IEtFWV9ERUxFVEUgJiYgIWlucHV0SGFkVGV4dCkge1xyXG4gICAgICAgICAgICB0aGlzLl9kZWxldGVQcmVzc2VkKCk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLl91cGRhdGVJbnB1dFdpZHRoKCk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX29uUGFzdGU6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICBzZXRUaW1lb3V0KGZ1bmN0aW9uKCkge1xyXG4gICAgICAgICAgICB0aGlzLnNlYXJjaCgpO1xyXG5cclxuICAgICAgICAgICAgaWYgKHRoaXMub3B0aW9ucy5jcmVhdGVUb2tlbkl0ZW0pIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuX2NyZWF0ZVRva2VuKCk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9LmJpbmQodGhpcyksIDEwKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfb3BlbjogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLm9wdGlvbnMuc2hvd0Ryb3Bkb3duICE9PSBmYWxzZSkge1xyXG4gICAgICAgICAgICB0aGlzLm9wZW4oKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIF9yZW5kZXJTZWxlY3RlZEl0ZW06IGZ1bmN0aW9uKGl0ZW0pIHtcclxuXHJcbiAgICAgICAgdGhpcy4kc2VhcmNoSW5wdXQuYmVmb3JlKHRoaXMudGVtcGxhdGUoJ211bHRpcGxlU2VsZWN0ZWRJdGVtJywgJC5leHRlbmQoe1xyXG4gICAgICAgICAgICBoaWdobGlnaHRlZDogKGl0ZW0uaWQgPT09IHRoaXMuX2hpZ2hsaWdodGVkSXRlbUlkKSxcclxuICAgICAgICAgICAgcmVtb3ZhYmxlOiAhdGhpcy5vcHRpb25zLnJlYWRPbmx5XHJcbiAgICAgICAgfSwgaXRlbSkpKTtcclxuXHJcbiAgICAgICAgdmFyIHF1b3RlZElkID0gU2VsZWN0aXZpdHkucXVvdGVDc3NBdHRyKGl0ZW0uaWQpO1xyXG4gICAgICAgIHRoaXMuJCgnLnNlbGVjdGl2aXR5LW11bHRpcGxlLXNlbGVjdGVkLWl0ZW1bZGF0YS1pdGVtLWlkPScgKyBxdW90ZWRJZCArICddJylcclxuICAgICAgICAgICAgLmZpbmQoJy5zZWxlY3Rpdml0eS1tdWx0aXBsZS1zZWxlY3RlZC1pdGVtLXJlbW92ZScpXHJcbiAgICAgICAgICAgIC5vbignY2xpY2snLCB0aGlzLl9pdGVtUmVtb3ZlQ2xpY2tlZC5iaW5kKHRoaXMpKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAcHJpdmF0ZVxyXG4gICAgICovXHJcbiAgICBfcmVzdWx0U2VsZWN0ZWQ6IGZ1bmN0aW9uKGV2ZW50KSB7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLl92YWx1ZS5pbmRleE9mKGV2ZW50LmlkKSA9PT0gLTEpIHtcclxuICAgICAgICAgICAgdGhpcy5hZGQoZXZlbnQuaXRlbSk7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgdGhpcy5yZW1vdmUoZXZlbnQuaXRlbSk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9zY3JvbGxUb0JvdHRvbTogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHZhciAkaW5wdXRDb250YWluZXIgPSB0aGlzLiQoJy5zZWxlY3Rpdml0eS1tdWx0aXBsZS1pbnB1dC1jb250YWluZXInKTtcclxuICAgICAgICAkaW5wdXRDb250YWluZXIuc2Nyb2xsVG9wKCRpbnB1dENvbnRhaW5lci5oZWlnaHQoKSk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX3VwZGF0ZUlucHV0V2lkdGg6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICBpZiAodGhpcy5lbmFibGVkKSB7XHJcbiAgICAgICAgICAgIHZhciAkaW5wdXQgPSB0aGlzLiRzZWFyY2hJbnB1dCwgJHdpZHRoRGV0ZWN0b3IgPSB0aGlzLiQoJy5zZWxlY3Rpdml0eS13aWR0aC1kZXRlY3RvcicpO1xyXG4gICAgICAgICAgICAkd2lkdGhEZXRlY3Rvci50ZXh0KCRpbnB1dC52YWwoKSB8fFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICF0aGlzLl9kYXRhLmxlbmd0aCAmJiB0aGlzLm9wdGlvbnMucGxhY2Vob2xkZXIgfHxcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAnJyk7XHJcbiAgICAgICAgICAgICRpbnB1dC53aWR0aCgkd2lkdGhEZXRlY3Rvci53aWR0aCgpICsgMjApO1xyXG5cclxuICAgICAgICAgICAgdGhpcy5wb3NpdGlvbkRyb3Bkb3duKCk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF91cGRhdGVQbGFjZWhvbGRlcjogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHZhciBwbGFjZWhvbGRlciA9IHRoaXMuX2RhdGEubGVuZ3RoID8gJycgOiB0aGlzLm9wdGlvbnMucGxhY2Vob2xkZXI7XHJcbiAgICAgICAgaWYgKHRoaXMuZW5hYmxlZCkge1xyXG4gICAgICAgICAgICB0aGlzLiRzZWFyY2hJbnB1dC5hdHRyKCdwbGFjZWhvbGRlcicsIHBsYWNlaG9sZGVyKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICB0aGlzLiQoJy5zZWxlY3Rpdml0eS1wbGFjZWhvbGRlcicpLnRleHQocGxhY2Vob2xkZXIpO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuXHJcbn0pO1xyXG5cclxubW9kdWxlLmV4cG9ydHMgPSBTZWxlY3Rpdml0eS5JbnB1dFR5cGVzLk11bHRpcGxlID0gTXVsdGlwbGVTZWxlY3Rpdml0eTtcclxuXHJcbn0se1wiOFwiOjgsXCJqcXVlcnlcIjpcImpxdWVyeVwifV0sMTU6W2Z1bmN0aW9uKF9kZXJlcV8sbW9kdWxlLGV4cG9ydHMpe1xyXG4ndXNlIHN0cmljdCc7XHJcblxyXG52YXIgJCA9IHdpbmRvdy5qUXVlcnkgfHwgd2luZG93LlplcHRvO1xyXG5cclxudmFyIFNlbGVjdGl2aXR5ID0gX2RlcmVxXyg4KTtcclxuXHJcbi8qKlxyXG4gKiBTaW5nbGVTZWxlY3Rpdml0eSBDb25zdHJ1Y3Rvci5cclxuICpcclxuICogQHBhcmFtIG9wdGlvbnMgT3B0aW9ucyBvYmplY3QuIEFjY2VwdHMgYWxsIG9wdGlvbnMgZnJvbSB0aGUgU2VsZWN0aXZpdHkgQmFzZSBDb25zdHJ1Y3RvciBpblxyXG4gKiAgICAgICAgICAgICAgICBhZGRpdGlvbiB0byB0aG9zZSBhY2NlcHRlZCBieSBTaW5nbGVTZWxlY3Rpdml0eS5zZXRPcHRpb25zKCkuXHJcbiAqL1xyXG5mdW5jdGlvbiBTaW5nbGVTZWxlY3Rpdml0eShvcHRpb25zKSB7XHJcblxyXG4gICAgU2VsZWN0aXZpdHkuY2FsbCh0aGlzLCBvcHRpb25zKTtcclxuXHJcbiAgICB0aGlzLiRlbC5odG1sKHRoaXMudGVtcGxhdGUoJ3NpbmdsZVNlbGVjdElucHV0JywgdGhpcy5vcHRpb25zKSlcclxuICAgICAgICAgICAgLnRyaWdnZXIoJ3NlbGVjdGl2aXR5LWluaXQnLCAnc2luZ2xlJyk7XHJcblxyXG4gICAgdGhpcy5yZXJlbmRlclNlbGVjdGlvbigpO1xyXG5cclxuICAgIGlmICghb3B0aW9ucy5wb3NpdGlvbkRyb3Bkb3duKSB7XHJcbiAgICAgICAgLy8gZHJvcGRvd25zIGZvciBzaW5nbGUtdmFsdWUgaW5wdXRzIHNob3VsZCBvcGVuIGJlbG93IHRoZSBzZWxlY3QgYm94LFxyXG4gICAgICAgIC8vIHVubGVzcyB0aGVyZSBpcyBub3QgZW5vdWdoIHNwYWNlIGJlbG93LCBpbiB3aGljaCBjYXNlIHRoZSBkcm9wZG93biBzaG91bGQgYmUgbW92ZWQgdXBcclxuICAgICAgICAvLyBqdXN0IGVub3VnaCBzbyBpdCBmaXRzIGluIHRoZSB3aW5kb3csIGJ1dCBuZXZlciBzbyBtdWNoIHRoYXQgaXQgcmVhY2hlcyBhYm92ZSB0aGUgdG9wXHJcbiAgICAgICAgdGhpcy5vcHRpb25zLnBvc2l0aW9uRHJvcGRvd24gPSBmdW5jdGlvbigkZWwsICRzZWxlY3RFbCkge1xyXG4gICAgICAgICAgICB2YXIgcG9zaXRpb24gPSAkc2VsZWN0RWwucG9zaXRpb24oKSxcclxuICAgICAgICAgICAgICAgIGRyb3Bkb3duSGVpZ2h0ID0gJGVsLmhlaWdodCgpLFxyXG4gICAgICAgICAgICAgICAgc2VsZWN0SGVpZ2h0ID0gJHNlbGVjdEVsLmhlaWdodCgpLFxyXG4gICAgICAgICAgICAgICAgdG9wID0gJHNlbGVjdEVsWzBdLmdldEJvdW5kaW5nQ2xpZW50UmVjdCgpLnRvcCxcclxuICAgICAgICAgICAgICAgIGJvdHRvbSA9IHRvcCArIHNlbGVjdEhlaWdodCArIGRyb3Bkb3duSGVpZ2h0LFxyXG4gICAgICAgICAgICAgICAgZGVsdGFVcCA9IDA7XHJcblxyXG4gICAgICAgICAgICBpZiAodHlwZW9mIHdpbmRvdyAhPT0gJ3VuZGVmaW5lZCcpIHtcclxuICAgICAgICAgICAgICAgIGRlbHRhVXAgPSBNYXRoLm1pbihNYXRoLm1heChib3R0b20gLSAkKHdpbmRvdykuaGVpZ2h0KCksIDApLCB0b3AgKyBzZWxlY3RIZWlnaHQpO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICB2YXIgd2lkdGggPSAkc2VsZWN0RWwub3V0ZXJXaWR0aCA/ICRzZWxlY3RFbC5vdXRlcldpZHRoKCkgOiAkc2VsZWN0RWwud2lkdGgoKTtcclxuICAgICAgICAgICAgJGVsLmNzcyh7XHJcbiAgICAgICAgICAgICAgICBsZWZ0OiBwb3NpdGlvbi5sZWZ0ICsgJ3B4JyxcclxuICAgICAgICAgICAgICAgIHRvcDogKHBvc2l0aW9uLnRvcCArIHNlbGVjdEhlaWdodCAtIGRlbHRhVXApICsgJ3B4J1xyXG4gICAgICAgICAgICB9KS53aWR0aCh3aWR0aCk7XHJcbiAgICAgICAgfTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAob3B0aW9ucy5zaG93U2VhcmNoSW5wdXRJbkRyb3Bkb3duID09PSBmYWxzZSkge1xyXG4gICAgICAgIHRoaXMuaW5pdFNlYXJjaElucHV0KHRoaXMuJCgnLnNlbGVjdGl2aXR5LXNpbmdsZS1zZWxlY3QtaW5wdXQnKSwgeyBub1NlYXJjaDogdHJ1ZSB9KTtcclxuICAgIH1cclxufVxyXG5cclxuLyoqXHJcbiAqIE1ldGhvZHMuXHJcbiAqL1xyXG52YXIgY2FsbFN1cGVyID0gU2VsZWN0aXZpdHkuaW5oZXJpdHMoU2luZ2xlU2VsZWN0aXZpdHksIHtcclxuXHJcbiAgICAvKipcclxuICAgICAqIEV2ZW50cyBtYXAuXHJcbiAgICAgKlxyXG4gICAgICogRm9sbG93cyB0aGUgc2FtZSBmb3JtYXQgYXMgQmFja2JvbmU6IGh0dHA6Ly9iYWNrYm9uZWpzLm9yZy8jVmlldy1kZWxlZ2F0ZUV2ZW50c1xyXG4gICAgICovXHJcbiAgICBldmVudHM6IHtcclxuICAgICAgICAnY2hhbmdlJzogJ3JlcmVuZGVyU2VsZWN0aW9uJyxcclxuICAgICAgICAnY2xpY2snOiAnX2NsaWNrZWQnLFxyXG4gICAgICAgICdmb2N1cyAuc2VsZWN0aXZpdHktc2luZ2xlLXNlbGVjdC1pbnB1dCc6ICdfZm9jdXNlZCcsXHJcbiAgICAgICAgJ3NlbGVjdGl2aXR5LXNlbGVjdGVkJzogJ19yZXN1bHRTZWxlY3RlZCdcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBDbGVhcnMgdGhlIGRhdGEgYW5kIHZhbHVlLlxyXG4gICAgICovXHJcbiAgICBjbGVhcjogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIHRoaXMuZGF0YShudWxsKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAaW5oZXJpdFxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbmFsIG9wdGlvbnMgb2JqZWN0LiBNYXkgY29udGFpbiB0aGUgZm9sbG93aW5nIHByb3BlcnR5OlxyXG4gICAgICogICAgICAgICAgICAgICAga2VlcEZvY3VzIC0gSWYgZmFsc2UsIHRoZSBmb2N1cyB3b24ndCByZW1haW4gb24gdGhlIGlucHV0LlxyXG4gICAgICovXHJcbiAgICBjbG9zZTogZnVuY3Rpb24ob3B0aW9ucykge1xyXG5cclxuICAgICAgICB0aGlzLl9jbG9zaW5nID0gdHJ1ZTtcclxuXHJcbiAgICAgICAgY2FsbFN1cGVyKHRoaXMsICdjbG9zZScpO1xyXG5cclxuICAgICAgICBpZiAoIW9wdGlvbnMgfHwgb3B0aW9ucy5rZWVwRm9jdXMgIT09IGZhbHNlKSB7XHJcbiAgICAgICAgICAgIHRoaXMuJHNlYXJjaElucHV0LmZvY3VzKCk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLl9jbG9zaW5nID0gZmFsc2U7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmV0dXJucyB0aGUgY29ycmVjdCBkYXRhIGZvciBhIGdpdmVuIHZhbHVlLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSB2YWx1ZSBUaGUgdmFsdWUgdG8gZ2V0IHRoZSBkYXRhIGZvci4gU2hvdWxkIGJlIGFuIElELlxyXG4gICAgICpcclxuICAgICAqIEByZXR1cm4gVGhlIGNvcnJlc3BvbmRpbmcgZGF0YS4gV2lsbCBiZSBhbiBvYmplY3Qgd2l0aCAnaWQnIGFuZCAndGV4dCcgcHJvcGVydGllcy4gTm90ZSB0aGF0XHJcbiAgICAgKiAgICAgICAgIGlmIG5vIGl0ZW1zIGFyZSBkZWZpbmVkLCB0aGlzIG1ldGhvZCBhc3N1bWVzIHRoZSB0ZXh0IGxhYmVsIHdpbGwgYmUgZXF1YWwgdG8gdGhlIElELlxyXG4gICAgICovXHJcbiAgICBnZXREYXRhRm9yVmFsdWU6IGZ1bmN0aW9uKHZhbHVlKSB7XHJcblxyXG4gICAgICAgIHJldHVybiB0aGlzLmdldEl0ZW1Gb3JJZCh2YWx1ZSk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmV0dXJucyB0aGUgY29ycmVjdCB2YWx1ZSBmb3IgdGhlIGdpdmVuIGRhdGEuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIGRhdGEgVGhlIGRhdGEgdG8gZ2V0IHRoZSB2YWx1ZSBmb3IuIFNob3VsZCBiZSBhbiBvYmplY3Qgd2l0aCAnaWQnIGFuZCAndGV4dCdcclxuICAgICAqICAgICAgICAgICAgIHByb3BlcnRpZXMgb3IgbnVsbC5cclxuICAgICAqXHJcbiAgICAgKiBAcmV0dXJuIFRoZSBjb3JyZXNwb25kaW5nIHZhbHVlLiBXaWxsIGJlIGFuIElEIG9yIG51bGwuXHJcbiAgICAgKi9cclxuICAgIGdldFZhbHVlRm9yRGF0YTogZnVuY3Rpb24oZGF0YSkge1xyXG5cclxuICAgICAgICByZXR1cm4gKGRhdGEgPyBkYXRhLmlkIDogbnVsbCk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQGluaGVyaXRcclxuICAgICAqL1xyXG4gICAgb3BlbjogZnVuY3Rpb24ob3B0aW9ucykge1xyXG5cclxuICAgICAgICB0aGlzLl9vcGVuaW5nID0gdHJ1ZTtcclxuXHJcbiAgICAgICAgdmFyIHNob3dTZWFyY2hJbnB1dCA9ICh0aGlzLm9wdGlvbnMuc2hvd1NlYXJjaElucHV0SW5Ecm9wZG93biAhPT0gZmFsc2UpO1xyXG5cclxuICAgICAgICBjYWxsU3VwZXIodGhpcywgJ29wZW4nLCAkLmV4dGVuZCh7IHNob3dTZWFyY2hJbnB1dDogc2hvd1NlYXJjaElucHV0IH0sIG9wdGlvbnMpKTtcclxuXHJcbiAgICAgICAgaWYgKCFzaG93U2VhcmNoSW5wdXQpIHtcclxuICAgICAgICAgICAgdGhpcy5mb2N1cygpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdGhpcy5fb3BlbmluZyA9IGZhbHNlO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlLXJlbmRlcnMgdGhlIHNlbGVjdGlvbi5cclxuICAgICAqXHJcbiAgICAgKiBOb3JtYWxseSB0aGUgVUkgaXMgYXV0b21hdGljYWxseSB1cGRhdGVkIHdoZW5ldmVyIHRoZSBzZWxlY3Rpb24gY2hhbmdlcywgYnV0IHlvdSBtYXkgd2FudCB0b1xyXG4gICAgICogY2FsbCB0aGlzIG1ldGhvZCBleHBsaWNpdGx5IGlmIHlvdSd2ZSB1cGRhdGVkIHRoZSBzZWxlY3Rpb24gd2l0aCB0aGUgdHJpZ2dlckNoYW5nZSBvcHRpb24gc2V0XHJcbiAgICAgKiB0byBmYWxzZS5cclxuICAgICAqL1xyXG4gICAgcmVyZW5kZXJTZWxlY3Rpb246IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICB2YXIgJGNvbnRhaW5lciA9IHRoaXMuJCgnLnNlbGVjdGl2aXR5LXNpbmdsZS1yZXN1bHQtY29udGFpbmVyJyk7XHJcbiAgICAgICAgaWYgKHRoaXMuX2RhdGEpIHtcclxuICAgICAgICAgICAgJGNvbnRhaW5lci5odG1sKFxyXG4gICAgICAgICAgICAgICAgdGhpcy50ZW1wbGF0ZSgnc2luZ2xlU2VsZWN0ZWRJdGVtJywgJC5leHRlbmQoe1xyXG4gICAgICAgICAgICAgICAgICAgIHJlbW92YWJsZTogdGhpcy5vcHRpb25zLmFsbG93Q2xlYXIgJiYgIXRoaXMub3B0aW9ucy5yZWFkT25seVxyXG4gICAgICAgICAgICAgICAgfSwgdGhpcy5fZGF0YSkpXHJcbiAgICAgICAgICAgICk7XHJcblxyXG4gICAgICAgICAgICAkY29udGFpbmVyLmZpbmQoJy5zZWxlY3Rpdml0eS1zaW5nbGUtc2VsZWN0ZWQtaXRlbS1yZW1vdmUnKVxyXG4gICAgICAgICAgICAgICAgICAgICAgLm9uKCdjbGljaycsIHRoaXMuX2l0ZW1SZW1vdmVDbGlja2VkLmJpbmQodGhpcykpO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICRjb250YWluZXIuaHRtbChcclxuICAgICAgICAgICAgICAgIHRoaXMudGVtcGxhdGUoJ3NpbmdsZVNlbGVjdFBsYWNlaG9sZGVyJywgeyBwbGFjZWhvbGRlcjogdGhpcy5vcHRpb25zLnBsYWNlaG9sZGVyIH0pXHJcbiAgICAgICAgICAgICk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBpbmhlcml0XHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIG9wdGlvbnMgT3B0aW9ucyBvYmplY3QuIEluIGFkZGl0aW9uIHRvIHRoZSBvcHRpb25zIHN1cHBvcnRlZCBpbiB0aGUgYmFzZVxyXG4gICAgICogICAgICAgICAgICAgICAgaW1wbGVtZW50YXRpb24sIHRoaXMgbWF5IGNvbnRhaW4gdGhlIGZvbGxvd2luZyBwcm9wZXJ0aWVzOlxyXG4gICAgICogICAgICAgICAgICAgICAgYWxsb3dDbGVhciAtIEJvb2xlYW4gd2hldGhlciB0aGUgc2VsZWN0ZWQgaXRlbSBtYXkgYmUgcmVtb3ZlZC5cclxuICAgICAqICAgICAgICAgICAgICAgIHNob3dTZWFyY2hJbnB1dEluRHJvcGRvd24gLSBTZXQgdG8gZmFsc2UgdG8gcmVtb3ZlIHRoZSBzZWFyY2ggaW5wdXQgdXNlZCBpblxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGRyb3Bkb3ducy4gVGhlIGRlZmF1bHQgaXMgdHJ1ZS5cclxuICAgICAqL1xyXG4gICAgc2V0T3B0aW9uczogZnVuY3Rpb24ob3B0aW9ucykge1xyXG5cclxuICAgICAgICBvcHRpb25zID0gb3B0aW9ucyB8fCB7fTtcclxuXHJcbiAgICAgICAgb3B0aW9ucy5hbGxvd2VkVHlwZXMgPSAkLmV4dGVuZChvcHRpb25zLmFsbG93ZWRUeXBlcyB8fCB7fSwge1xyXG4gICAgICAgICAgICBhbGxvd0NsZWFyOiAnYm9vbGVhbicsXHJcbiAgICAgICAgICAgIHNob3dTZWFyY2hJbnB1dEluRHJvcGRvd246ICdib29sZWFuJ1xyXG4gICAgICAgIH0pO1xyXG5cclxuICAgICAgICBjYWxsU3VwZXIodGhpcywgJ3NldE9wdGlvbnMnLCBvcHRpb25zKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBWYWxpZGF0ZXMgZGF0YSB0byBzZXQuIFRocm93cyBhbiBleGNlcHRpb24gaWYgdGhlIGRhdGEgaXMgaW52YWxpZC5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gZGF0YSBUaGUgZGF0YSB0byB2YWxpZGF0ZS4gU2hvdWxkIGJlIGFuIG9iamVjdCB3aXRoICdpZCcgYW5kICd0ZXh0JyBwcm9wZXJ0aWVzIG9yIG51bGxcclxuICAgICAqICAgICAgICAgICAgIHRvIGluZGljYXRlIG5vIGl0ZW0gaXMgc2VsZWN0ZWQuXHJcbiAgICAgKlxyXG4gICAgICogQHJldHVybiBUaGUgdmFsaWRhdGVkIGRhdGEuIFRoaXMgbWF5IGRpZmZlciBmcm9tIHRoZSBpbnB1dCBkYXRhLlxyXG4gICAgICovXHJcbiAgICB2YWxpZGF0ZURhdGE6IGZ1bmN0aW9uKGRhdGEpIHtcclxuXHJcbiAgICAgICAgcmV0dXJuIChkYXRhID09PSBudWxsID8gZGF0YSA6IHRoaXMudmFsaWRhdGVJdGVtKGRhdGEpKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBWYWxpZGF0ZXMgYSB2YWx1ZSB0byBzZXQuIFRocm93cyBhbiBleGNlcHRpb24gaWYgdGhlIHZhbHVlIGlzIGludmFsaWQuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIHZhbHVlIFRoZSB2YWx1ZSB0byB2YWxpZGF0ZS4gU2hvdWxkIGJlIG51bGwgb3IgYSB2YWxpZCBJRC5cclxuICAgICAqXHJcbiAgICAgKiBAcmV0dXJuIFRoZSB2YWxpZGF0ZWQgdmFsdWUuIFRoaXMgbWF5IGRpZmZlciBmcm9tIHRoZSBpbnB1dCB2YWx1ZS5cclxuICAgICAqL1xyXG4gICAgdmFsaWRhdGVWYWx1ZTogZnVuY3Rpb24odmFsdWUpIHtcclxuXHJcbiAgICAgICAgaWYgKHZhbHVlID09PSBudWxsIHx8IFNlbGVjdGl2aXR5LmlzVmFsaWRJZCh2YWx1ZSkpIHtcclxuICAgICAgICAgICAgcmV0dXJuIHZhbHVlO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRocm93IG5ldyBFcnJvcignVmFsdWUgZm9yIFNpbmdsZVNlbGVjdGl2aXR5IGluc3RhbmNlIHNob3VsZCBiZSBhIHZhbGlkIElEIG9yIG51bGwnKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX2NsaWNrZWQ6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICBpZiAodGhpcy5lbmFibGVkKSB7XHJcbiAgICAgICAgICAgIGlmICh0aGlzLmRyb3Bkb3duKSB7XHJcbiAgICAgICAgICAgICAgICB0aGlzLmNsb3NlKCk7XHJcbiAgICAgICAgICAgIH0gZWxzZSBpZiAodGhpcy5vcHRpb25zLnNob3dEcm9wZG93biAhPT0gZmFsc2UpIHtcclxuICAgICAgICAgICAgICAgIHRoaXMub3BlbigpO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9mb2N1c2VkOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMuZW5hYmxlZCAmJiAhdGhpcy5fY2xvc2luZyAmJiAhdGhpcy5fb3BlbmluZyAmJlxyXG4gICAgICAgICAgICB0aGlzLm9wdGlvbnMuc2hvd0Ryb3Bkb3duICE9PSBmYWxzZSkge1xyXG4gICAgICAgICAgICB0aGlzLm9wZW4oKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX2l0ZW1SZW1vdmVDbGlja2VkOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgdGhpcy5kYXRhKG51bGwpO1xyXG5cclxuICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQHByaXZhdGVcclxuICAgICAqL1xyXG4gICAgX3Jlc3VsdFNlbGVjdGVkOiBmdW5jdGlvbihldmVudCkge1xyXG5cclxuICAgICAgICB0aGlzLmRhdGEoZXZlbnQuaXRlbSk7XHJcblxyXG4gICAgICAgIHRoaXMuY2xvc2UoKTtcclxuICAgIH1cclxuXHJcbn0pO1xyXG5cclxubW9kdWxlLmV4cG9ydHMgPSBTZWxlY3Rpdml0eS5JbnB1dFR5cGVzLlNpbmdsZSA9IFNpbmdsZVNlbGVjdGl2aXR5O1xyXG5cclxufSx7XCI4XCI6OCxcImpxdWVyeVwiOlwianF1ZXJ5XCJ9XSwxNjpbZnVuY3Rpb24oX2RlcmVxXyxtb2R1bGUsZXhwb3J0cyl7XHJcbid1c2Ugc3RyaWN0JztcclxuXHJcbnZhciBTZWxlY3Rpdml0eSA9IF9kZXJlcV8oOCk7XHJcbnZhciBTZWxlY3Rpdml0eURyb3Bkb3duID0gX2RlcmVxXygxMCk7XHJcblxyXG4vKipcclxuICogRXh0ZW5kZWQgZHJvcGRvd24gdGhhdCBzdXBwb3J0cyBzdWJtZW51cy5cclxuICovXHJcbmZ1bmN0aW9uIFNlbGVjdGl2aXR5U3VibWVudShvcHRpb25zKSB7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBPcHRpb25hbCBwYXJlbnQgZHJvcGRvd24gbWVudSBmcm9tIHdoaWNoIHRoaXMgZHJvcGRvd24gd2FzIG9wZW5lZC5cclxuICAgICAqL1xyXG4gICAgdGhpcy5wYXJlbnRNZW51ID0gb3B0aW9ucy5wYXJlbnRNZW51O1xyXG5cclxuICAgIFNlbGVjdGl2aXR5RHJvcGRvd24uY2FsbCh0aGlzLCBvcHRpb25zKTtcclxuXHJcbiAgICB0aGlzLl9jbG9zZVN1Ym1lbnVUaW1lb3V0ID0gMDtcclxuXHJcbiAgICB0aGlzLl9vcGVuU3VibWVudVRpbWVvdXQgPSAwO1xyXG59XHJcblxyXG52YXIgY2FsbFN1cGVyID0gU2VsZWN0aXZpdHkuaW5oZXJpdHMoU2VsZWN0aXZpdHlTdWJtZW51LCBTZWxlY3Rpdml0eURyb3Bkb3duLCB7XHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAaW5oZXJpdFxyXG4gICAgICovXHJcbiAgICBjbG9zZTogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLnN1Ym1lbnUpIHtcclxuICAgICAgICAgICAgdGhpcy5zdWJtZW51LmNsb3NlKCk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBjYWxsU3VwZXIodGhpcywgJ2Nsb3NlJyk7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLnBhcmVudE1lbnUpIHtcclxuICAgICAgICAgICAgdGhpcy5wYXJlbnRNZW51LnN1Ym1lbnUgPSBudWxsO1xyXG4gICAgICAgICAgICB0aGlzLnBhcmVudE1lbnUgPSBudWxsO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgY2xlYXJUaW1lb3V0KHRoaXMuX2Nsb3NlU3VibWVudVRpbWVvdXQpO1xyXG4gICAgICAgIGNsZWFyVGltZW91dCh0aGlzLl9vcGVuU3VibWVudVRpbWVvdXQpO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBpbmhlcml0XHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIG9wdGlvbnMgT3B0aW9uYWwgb3B0aW9ucyBvYmplY3QuIE1heSBjb250YWluIHRoZSBmb2xsb3dpbmcgcHJvcGVydHk6XHJcbiAgICAgKiAgICAgICAgICAgICAgICBkZWxheSAtIElmIHRydWUsIGluZGljYXRlcyBhbnkgc3VibWVudSBzaG91bGQgbm90IGJlIG9wZW5lZCB1bnRpbCBhZnRlciBzb21lXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgIGRlbGF5LlxyXG4gICAgICovXHJcbiAgICBoaWdobGlnaHQ6IGZ1bmN0aW9uKGl0ZW0sIG9wdGlvbnMpIHtcclxuXHJcbiAgICAgICAgaWYgKG9wdGlvbnMgJiYgb3B0aW9ucy5kZWxheSkge1xyXG4gICAgICAgICAgICBjYWxsU3VwZXIodGhpcywgJ2hpZ2hsaWdodCcsIGl0ZW0pO1xyXG5cclxuICAgICAgICAgICAgY2xlYXJUaW1lb3V0KHRoaXMuX29wZW5TdWJtZW51VGltZW91dCk7XHJcbiAgICAgICAgICAgIHRoaXMuX29wZW5TdWJtZW51VGltZW91dCA9IHNldFRpbWVvdXQodGhpcy5fZG9IaWdobGlnaHQuYmluZCh0aGlzLCBpdGVtKSwgMzAwKTtcclxuICAgICAgICB9IGVsc2UgaWYgKHRoaXMuc3VibWVudSkge1xyXG4gICAgICAgICAgICBpZiAodGhpcy5oaWdobGlnaHRlZFJlc3VsdCAmJiB0aGlzLmhpZ2hsaWdodGVkUmVzdWx0LmlkID09PSBpdGVtLmlkKSB7XHJcbiAgICAgICAgICAgICAgICB0aGlzLl9kb0hpZ2hsaWdodChpdGVtKTtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIGNsZWFyVGltZW91dCh0aGlzLl9jbG9zZVN1Ym1lbnVUaW1lb3V0KTtcclxuICAgICAgICAgICAgICAgIHRoaXMuX2Nsb3NlU3VibWVudVRpbWVvdXQgPSBzZXRUaW1lb3V0KFxyXG4gICAgICAgICAgICAgICAgICAgIHRoaXMuX2Nsb3NlU3VibWVudUFuZEhpZ2hsaWdodC5iaW5kKHRoaXMsIGl0ZW0pLCAxMDBcclxuICAgICAgICAgICAgICAgICk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBpZiAodGhpcy5wYXJlbnRNZW51ICYmIHRoaXMucGFyZW50TWVudS5fY2xvc2VTdWJtZW51VGltZW91dCkge1xyXG4gICAgICAgICAgICAgICAgY2xlYXJUaW1lb3V0KHRoaXMucGFyZW50TWVudS5fY2xvc2VTdWJtZW51VGltZW91dCk7XHJcbiAgICAgICAgICAgICAgICB0aGlzLnBhcmVudE1lbnUuX2Nsb3NlU3VibWVudVRpbWVvdXQgPSAwO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICB0aGlzLl9kb0hpZ2hsaWdodChpdGVtKTtcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogQGluaGVyaXRcclxuICAgICAqL1xyXG4gICAgc2VhcmNoOiBmdW5jdGlvbih0ZXJtKSB7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLnN1Ym1lbnUpIHtcclxuICAgICAgICAgICAgdGhpcy5zdWJtZW51LnNlYXJjaCh0ZXJtKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBjYWxsU3VwZXIodGhpcywgJ3NlYXJjaCcsIHRlcm0pO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAaW5oZXJpdFxyXG4gICAgICovXHJcbiAgICBzZWxlY3RIaWdobGlnaHQ6IGZ1bmN0aW9uKCkge1xyXG5cclxuICAgICAgICBpZiAodGhpcy5zdWJtZW51KSB7XHJcbiAgICAgICAgICAgIHRoaXMuc3VibWVudS5zZWxlY3RIaWdobGlnaHQoKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBjYWxsU3VwZXIodGhpcywgJ3NlbGVjdEhpZ2hsaWdodCcpO1xyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAaW5oZXJpdFxyXG4gICAgICovXHJcbiAgICBzZWxlY3RJdGVtOiBmdW5jdGlvbihpZCkge1xyXG5cclxuICAgICAgICB2YXIgaXRlbSA9IFNlbGVjdGl2aXR5LmZpbmROZXN0ZWRCeUlkKHRoaXMucmVzdWx0cywgaWQpO1xyXG4gICAgICAgIGlmIChpdGVtICYmICFpdGVtLmRpc2FibGVkICYmICFpdGVtLnN1Ym1lbnUpIHtcclxuICAgICAgICAgICAgdmFyIG9wdGlvbnMgPSB7IGlkOiBpZCwgaXRlbTogaXRlbSB9O1xyXG4gICAgICAgICAgICBpZiAodGhpcy5zZWxlY3Rpdml0eS50cmlnZ2VyRXZlbnQoJ3NlbGVjdGl2aXR5LXNlbGVjdGluZycsIG9wdGlvbnMpKSB7XHJcbiAgICAgICAgICAgICAgICB0aGlzLnNlbGVjdGl2aXR5LnRyaWdnZXJFdmVudCgnc2VsZWN0aXZpdHktc2VsZWN0ZWQnLCBvcHRpb25zKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBAaW5oZXJpdFxyXG4gICAgICovXHJcbiAgICBzaG93UmVzdWx0czogZnVuY3Rpb24ocmVzdWx0cywgb3B0aW9ucykge1xyXG5cclxuICAgICAgICBpZiAodGhpcy5zdWJtZW51KSB7XHJcbiAgICAgICAgICAgIHRoaXMuc3VibWVudS5zaG93UmVzdWx0cyhyZXN1bHRzLCBvcHRpb25zKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBjYWxsU3VwZXIodGhpcywgJ3Nob3dSZXN1bHRzJywgcmVzdWx0cywgb3B0aW9ucyk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBpbmhlcml0XHJcbiAgICAgKi9cclxuICAgIHRyaWdnZXJDbG9zZTogZnVuY3Rpb24oKSB7XHJcblxyXG4gICAgICAgIGlmICh0aGlzLnBhcmVudE1lbnUpIHtcclxuICAgICAgICAgICAgdGhpcy5zZWxlY3Rpdml0eS4kZWwudHJpZ2dlcignc2VsZWN0aXZpdHktY2xvc2Utc3VibWVudScpO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIGNhbGxTdXBlcih0aGlzLCAndHJpZ2dlckNsb3NlJyk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBpbmhlcml0XHJcbiAgICAgKi9cclxuICAgIHRyaWdnZXJPcGVuOiBmdW5jdGlvbigpIHtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMucGFyZW50TWVudSkge1xyXG4gICAgICAgICAgICB0aGlzLnNlbGVjdGl2aXR5LiRlbC50cmlnZ2VyKCdzZWxlY3Rpdml0eS1vcGVuLXN1Ym1lbnUnKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBjYWxsU3VwZXIodGhpcywgJ3RyaWdnZXJPcGVuJyk7XHJcbiAgICAgICAgfVxyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9jbG9zZVN1Ym1lbnVBbmRIaWdobGlnaHQ6IGZ1bmN0aW9uKGl0ZW0pIHtcclxuXHJcbiAgICAgICAgaWYgKHRoaXMuc3VibWVudSkge1xyXG4gICAgICAgICAgICB0aGlzLnN1Ym1lbnUuY2xvc2UoKTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHRoaXMuX2RvSGlnaGxpZ2h0KGl0ZW0pO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIEBwcml2YXRlXHJcbiAgICAgKi9cclxuICAgIF9kb0hpZ2hsaWdodDogZnVuY3Rpb24oaXRlbSkge1xyXG5cclxuICAgICAgICBjYWxsU3VwZXIodGhpcywgJ2hpZ2hsaWdodCcsIGl0ZW0pO1xyXG5cclxuICAgICAgICBpZiAoaXRlbS5zdWJtZW51ICYmICF0aGlzLnN1Ym1lbnUpIHtcclxuICAgICAgICAgICAgdmFyIHNlbGVjdGl2aXR5ID0gdGhpcy5zZWxlY3Rpdml0eTtcclxuICAgICAgICAgICAgdmFyIERyb3Bkb3duID0gc2VsZWN0aXZpdHkub3B0aW9ucy5kcm9wZG93biB8fCBTZWxlY3Rpdml0eS5Ecm9wZG93bjtcclxuICAgICAgICAgICAgaWYgKERyb3Bkb3duKSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgcXVvdGVkSWQgPSBTZWxlY3Rpdml0eS5xdW90ZUNzc0F0dHIoaXRlbS5pZCk7XHJcbiAgICAgICAgICAgICAgICB2YXIgJGl0ZW0gPSB0aGlzLiQoJy5zZWxlY3Rpdml0eS1yZXN1bHQtaXRlbVtkYXRhLWl0ZW0taWQ9JyArIHF1b3RlZElkICsgJ10nKTtcclxuICAgICAgICAgICAgICAgIHZhciAkZHJvcGRvd25FbCA9IHRoaXMuJGVsO1xyXG5cclxuICAgICAgICAgICAgICAgIHRoaXMuc3VibWVudSA9IG5ldyBEcm9wZG93bih7XHJcbiAgICAgICAgICAgICAgICAgICAgaXRlbXM6IGl0ZW0uc3VibWVudS5pdGVtcyB8fCBudWxsLFxyXG4gICAgICAgICAgICAgICAgICAgIHBhcmVudE1lbnU6IHRoaXMsXHJcbiAgICAgICAgICAgICAgICAgICAgcG9zaXRpb246IGl0ZW0uc3VibWVudS5wb3NpdGlvbkRyb3Bkb3duIHx8IGZ1bmN0aW9uKCRlbCkge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB2YXIgZHJvcGRvd25Qb3NpdGlvbiA9ICRkcm9wZG93bkVsLnBvc2l0aW9uKCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciB3aWR0aCA9ICRkcm9wZG93bkVsLndpZHRoKCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICRlbC5jc3Moe1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbGVmdDogZHJvcGRvd25Qb3NpdGlvbi5sZWZ0ICsgd2lkdGggKyAncHgnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdG9wOiAkaXRlbS5wb3NpdGlvbigpLnRvcCArIGRyb3Bkb3duUG9zaXRpb24udG9wICsgJ3B4J1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9KS53aWR0aCh3aWR0aCk7XHJcbiAgICAgICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgICAgICBxdWVyeTogaXRlbS5zdWJtZW51LnF1ZXJ5IHx8IG51bGwsXHJcbiAgICAgICAgICAgICAgICAgICAgc2VsZWN0aXZpdHk6IHNlbGVjdGl2aXR5LFxyXG4gICAgICAgICAgICAgICAgICAgIHNob3dTZWFyY2hJbnB1dDogaXRlbS5zdWJtZW51LnNob3dTZWFyY2hJbnB1dFxyXG4gICAgICAgICAgICAgICAgfSk7XHJcblxyXG4gICAgICAgICAgICAgICAgdGhpcy5zdWJtZW51LnNlYXJjaCgnJyk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG59KTtcclxuXHJcblNlbGVjdGl2aXR5LkRyb3Bkb3duID0gU2VsZWN0aXZpdHlTdWJtZW51O1xyXG5cclxuU2VsZWN0aXZpdHkuZmluZE5lc3RlZEJ5SWQgPSBmdW5jdGlvbihhcnJheSwgaWQpIHtcclxuXHJcbiAgICBmb3IgKHZhciBpID0gMCwgbGVuZ3RoID0gYXJyYXkubGVuZ3RoOyBpIDwgbGVuZ3RoOyBpKyspIHtcclxuICAgICAgICB2YXIgaXRlbSA9IGFycmF5W2ldLCByZXN1bHQ7XHJcbiAgICAgICAgaWYgKGl0ZW0uaWQgPT09IGlkKSB7XHJcbiAgICAgICAgICAgIHJlc3VsdCA9IGl0ZW07XHJcbiAgICAgICAgfSBlbHNlIGlmIChpdGVtLmNoaWxkcmVuKSB7XHJcbiAgICAgICAgICAgIHJlc3VsdCA9IFNlbGVjdGl2aXR5LmZpbmROZXN0ZWRCeUlkKGl0ZW0uY2hpbGRyZW4sIGlkKTtcclxuICAgICAgICB9IGVsc2UgaWYgKGl0ZW0uc3VibWVudSAmJiBpdGVtLnN1Ym1lbnUuaXRlbXMpIHtcclxuICAgICAgICAgICAgcmVzdWx0ID0gU2VsZWN0aXZpdHkuZmluZE5lc3RlZEJ5SWQoaXRlbS5zdWJtZW51Lml0ZW1zLCBpZCk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGlmIChyZXN1bHQpIHtcclxuICAgICAgICAgICAgcmV0dXJuIHJlc3VsdDtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICByZXR1cm4gbnVsbDtcclxufTtcclxuXHJcbm1vZHVsZS5leHBvcnRzID0gU2VsZWN0aXZpdHlTdWJtZW51O1xyXG5cclxufSx7XCIxMFwiOjEwLFwiOFwiOjh9XSwxNzpbZnVuY3Rpb24oX2RlcmVxXyxtb2R1bGUsZXhwb3J0cyl7XHJcbid1c2Ugc3RyaWN0JztcclxuXHJcbnZhciBlc2NhcGUgPSBfZGVyZXFfKDQpO1xyXG5cclxudmFyIFNlbGVjdGl2aXR5ID0gX2RlcmVxXyg4KTtcclxuXHJcbl9kZXJlcV8oMTMpO1xyXG5cclxuLyoqXHJcbiAqIERlZmF1bHQgc2V0IG9mIHRlbXBsYXRlcyB0byB1c2Ugd2l0aCBTZWxlY3Rpdml0eS5qcy5cclxuICpcclxuICogTm90ZSB0aGF0IGV2ZXJ5IHRlbXBsYXRlIGNhbiBiZSBkZWZpbmVkIGFzIGVpdGhlciBhIHN0cmluZywgYSBmdW5jdGlvbiByZXR1cm5pbmcgYSBzdHJpbmcgKGxpa2VcclxuICogSGFuZGxlYmFycyB0ZW1wbGF0ZXMsIGZvciBpbnN0YW5jZSkgb3IgYXMgYW4gb2JqZWN0IGNvbnRhaW5pbmcgYSByZW5kZXIgZnVuY3Rpb24gKGxpa2UgSG9nYW4uanNcclxuICogdGVtcGxhdGVzLCBmb3IgaW5zdGFuY2UpLlxyXG4gKi9cclxuU2VsZWN0aXZpdHkuVGVtcGxhdGVzID0ge1xyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVuZGVycyB0aGUgZHJvcGRvd24uXHJcbiAgICAgKlxyXG4gICAgICogVGhlIHRlbXBsYXRlIGlzIGV4cGVjdGVkIHRvIGhhdmUgYXQgbGVhc3Qgb25lIGVsZW1lbnQgd2l0aCB0aGUgY2xhc3NcclxuICAgICAqICdzZWxlY3Rpdml0eS1yZXN1bHRzLWNvbnRhaW5lcicsIHdoaWNoIGlzIHdoZXJlIGFsbCByZXN1bHRzIHdpbGwgYmUgYWRkZWQgdG8uXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIG9wdGlvbnMgT3B0aW9ucyBvYmplY3QgY29udGFpbmluZyB0aGUgZm9sbG93aW5nIHByb3BlcnRpZXM6XHJcbiAgICAgKiAgICAgICAgICAgICAgICBkcm9wZG93bkNzc0NsYXNzIC0gT3B0aW9uYWwgQ1NTIGNsYXNzIHRvIGFkZCB0byB0aGUgdG9wLWxldmVsIGVsZW1lbnQuXHJcbiAgICAgKiAgICAgICAgICAgICAgICBzZWFyY2hJbnB1dFBsYWNlaG9sZGVyIC0gT3B0aW9uYWwgcGxhY2Vob2xkZXIgdGV4dCB0byBkaXNwbGF5IGluIHRoZSBzZWFyY2hcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBpbnB1dCBpbiB0aGUgZHJvcGRvd24uXHJcbiAgICAgKiAgICAgICAgICAgICAgICBzaG93U2VhcmNoSW5wdXQgLSBCb29sZWFuIHdoZXRoZXIgYSBzZWFyY2ggaW5wdXQgc2hvdWxkIGJlIHNob3duLiBJZiB0cnVlLFxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYW4gaW5wdXQgZWxlbWVudCB3aXRoIHRoZSAnc2VsZWN0aXZpdHktc2VhcmNoLWlucHV0JyBpc1xyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgZXhwZWN0ZWQuXHJcbiAgICAgKi9cclxuICAgIGRyb3Bkb3duOiBmdW5jdGlvbihvcHRpb25zKSB7XHJcbiAgICAgICAgdmFyIGV4dHJhQ2xhc3MgPSAob3B0aW9ucy5kcm9wZG93bkNzc0NsYXNzID8gJyAnICsgb3B0aW9ucy5kcm9wZG93bkNzc0NsYXNzIDogJycpLFxyXG4gICAgICAgICAgICBzZWFyY2hJbnB1dCA9ICcnO1xyXG4gICAgICAgIGlmIChvcHRpb25zLnNob3dTZWFyY2hJbnB1dCkge1xyXG4gICAgICAgICAgICBleHRyYUNsYXNzICs9ICcgaGFzLXNlYXJjaC1pbnB1dCc7XHJcblxyXG4gICAgICAgICAgICB2YXIgcGxhY2Vob2xkZXIgPSBvcHRpb25zLnNlYXJjaElucHV0UGxhY2Vob2xkZXI7XHJcbiAgICAgICAgICAgIHNlYXJjaElucHV0ID0gKFxyXG4gICAgICAgICAgICAgICAgJzxkaXYgY2xhc3M9XCJzZWxlY3Rpdml0eS1zZWFyY2gtaW5wdXQtY29udGFpbmVyXCI+JyArXHJcbiAgICAgICAgICAgICAgICAgICAgJzxpbnB1dCB0eXBlPVwidGV4dFwiIGNsYXNzPVwic2VsZWN0aXZpdHktc2VhcmNoLWlucHV0XCInICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIChwbGFjZWhvbGRlciA/ICcgcGxhY2Vob2xkZXI9XCInICsgZXNjYXBlKHBsYWNlaG9sZGVyKSArICdcIidcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA6ICcnKSArICc+JyArXHJcbiAgICAgICAgICAgICAgICAnPC9kaXY+J1xyXG4gICAgICAgICAgICApO1xyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICAnPGRpdiBjbGFzcz1cInNlbGVjdGl2aXR5LWRyb3Bkb3duJyArIGV4dHJhQ2xhc3MgKyAnXCI+JyArXHJcbiAgICAgICAgICAgICAgICBzZWFyY2hJbnB1dCArXHJcbiAgICAgICAgICAgICAgICAnPGRpdiBjbGFzcz1cInNlbGVjdGl2aXR5LXJlc3VsdHMtY29udGFpbmVyXCI+PC9kaXY+JyArXHJcbiAgICAgICAgICAgICc8L2Rpdj4nXHJcbiAgICAgICAgKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBSZW5kZXJzIGFuIGVycm9yIG1lc3NhZ2UgaW4gdGhlIGRyb3Bkb3duLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0IGNvbnRhaW5pbmcgdGhlIGZvbGxvd2luZyBwcm9wZXJ0aWVzOlxyXG4gICAgICogICAgICAgICAgICAgICAgZXNjYXBlIC0gQm9vbGVhbiB3aGV0aGVyIHRoZSBtZXNzYWdlIHNob3VsZCBiZSBIVE1MLWVzY2FwZWQuXHJcbiAgICAgKiAgICAgICAgICAgICAgICBtZXNzYWdlIC0gVGhlIG1lc3NhZ2UgdG8gZGlzcGxheS5cclxuICAgICAqL1xyXG4gICAgZXJyb3I6IGZ1bmN0aW9uKG9wdGlvbnMpIHtcclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICAnPGRpdiBjbGFzcz1cInNlbGVjdGl2aXR5LWVycm9yXCI+JyArXHJcbiAgICAgICAgICAgICAgICAob3B0aW9ucy5lc2NhcGUgPyBlc2NhcGUob3B0aW9ucy5tZXNzYWdlKSA6IG9wdGlvbnMubWVzc2FnZSkgK1xyXG4gICAgICAgICAgICAnPC9kaXY+J1xyXG4gICAgICAgICk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVuZGVycyBhIGxvYWRpbmcgaW5kaWNhdG9yIGluIHRoZSBkcm9wZG93bi5cclxuICAgICAqXHJcbiAgICAgKiBUaGlzIHRlbXBsYXRlIGlzIGV4cGVjdGVkIHRvIGhhdmUgYW4gZWxlbWVudCB3aXRoIGEgJ3NlbGVjdGl2aXR5LWxvYWRpbmcnIGNsYXNzIHdoaWNoIG1heSBiZVxyXG4gICAgICogcmVwbGFjZWQgd2l0aCBhY3R1YWwgcmVzdWx0cy5cclxuICAgICAqL1xyXG4gICAgbG9hZGluZzogZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgcmV0dXJuICc8ZGl2IGNsYXNzPVwic2VsZWN0aXZpdHktbG9hZGluZ1wiPicgKyBTZWxlY3Rpdml0eS5Mb2NhbGUubG9hZGluZyArICc8L2Rpdj4nO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIExvYWQgbW9yZSBpbmRpY2F0b3IuXHJcbiAgICAgKlxyXG4gICAgICogVGhpcyB0ZW1wbGF0ZSBpcyBleHBlY3RlZCB0byBoYXZlIGFuIGVsZW1lbnQgd2l0aCBhICdzZWxlY3Rpdml0eS1sb2FkLW1vcmUnIGNsYXNzIHdoaWNoLCB3aGVuXHJcbiAgICAgKiBjbGlja2VkLCB3aWxsIGxvYWQgbW9yZSByZXN1bHRzLlxyXG4gICAgICovXHJcbiAgICBsb2FkTW9yZTogZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgcmV0dXJuICc8ZGl2IGNsYXNzPVwic2VsZWN0aXZpdHktbG9hZC1tb3JlXCI+JyArIFNlbGVjdGl2aXR5LkxvY2FsZS5sb2FkTW9yZSArICc8L2Rpdj4nO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlbmRlcnMgbXVsdGktc2VsZWN0aW9uIGlucHV0IGJveGVzLlxyXG4gICAgICpcclxuICAgICAqIFRoZSB0ZW1wbGF0ZSBpcyBleHBlY3RlZCB0byBoYXZlIGF0IGxlYXN0IGhhdmUgZWxlbWVudHMgd2l0aCB0aGUgZm9sbG93aW5nIGNsYXNzZXM6XHJcbiAgICAgKiAnc2VsZWN0aXZpdHktbXVsdGlwbGUtaW5wdXQtY29udGFpbmVyJyAtIFRoZSBlbGVtZW50IGNvbnRhaW5pbmcgYWxsIHRoZSBzZWxlY3RlZCBpdGVtcyBhbmRcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgdGhlIGlucHV0IGZvciBzZWxlY3RpbmcgYWRkaXRpb25hbCBpdGVtcy5cclxuICAgICAqICdzZWxlY3Rpdml0eS1tdWx0aXBsZS1pbnB1dCcgLSBUaGUgYWN0dWFsIGlucHV0IGVsZW1lbnQgdGhhdCBhbGxvd3MgdGhlIHVzZXIgdG8gdHlwZSB0b1xyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHNlYXJjaCBmb3IgbW9yZSBpdGVtcy4gV2hlbiBzZWxlY3RlZCBpdGVtcyBhcmUgYWRkZWQsIHRoZXkgYXJlXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgaW5zZXJ0ZWQgcmlnaHQgYmVmb3JlIHRoaXMgZWxlbWVudC5cclxuICAgICAqICdzZWxlY3Rpdml0eS13aWR0aC1kZXRlY3RvcicgLSBUaGlzIGVsZW1lbnQgaXMgb3B0aW9uYWwsIGJ1dCBpbXBvcnRhbnQgdG8gbWFrZSBzdXJlIHRoZVxyXG4gICAgICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICcuc2VsZWN0aXZpdHktbXVsdGlwbGUtaW5wdXQnIGVsZW1lbnQgd2lsbCBmaXQgaW4gdGhlXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgY29udGFpbmVyLiBUaGUgd2lkdGggZGV0ZWN0b3IgYWxzbyBoYXMgdGhlXHJcbiAgICAgKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgJ3NlbGVjdDItbXVsdGlwbGUtaW5wdXQnIGNsYXNzIG9uIHB1cnBvc2UgdG8gYmUgYWJsZSB0byBkZXRlY3RcclxuICAgICAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB0aGUgd2lkdGggb2YgdGV4dCBlbnRlcmVkIGluIHRoZSBpbnB1dCBlbGVtZW50LlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0IGNvbnRhaW5pbmcgdGhlIGZvbGxvd2luZyBwcm9wZXJ0eTpcclxuICAgICAqICAgICAgICAgICAgICAgIGVuYWJsZWQgLSBCb29sZWFuIHdoZXRoZXIgdGhlIGlucHV0IGlzIGVuYWJsZWQuXHJcbiAgICAgKi9cclxuICAgIG11bHRpcGxlU2VsZWN0SW5wdXQ6IGZ1bmN0aW9uKG9wdGlvbnMpIHtcclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICAnPGRpdiBjbGFzcz1cInNlbGVjdGl2aXR5LW11bHRpcGxlLWlucHV0LWNvbnRhaW5lclwiPicgK1xyXG4gICAgICAgICAgICAgICAgKG9wdGlvbnMuZW5hYmxlZCA/ICc8aW5wdXQgdHlwZT1cInRleHRcIiBhdXRvY29tcGxldGU9XCJvZmZcIiBhdXRvY29ycmVjdD1cIm9mZlwiICcgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAnYXV0b2NhcGl0YWxpemU9XCJvZmZcIiAnICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgJ2NsYXNzPVwic2VsZWN0aXZpdHktbXVsdGlwbGUtaW5wdXRcIj4nICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAnPHNwYW4gY2xhc3M9XCJzZWxlY3Rpdml0eS1tdWx0aXBsZS1pbnB1dCAnICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgJ3NlbGVjdGl2aXR5LXdpZHRoLWRldGVjdG9yXCI+PC9zcGFuPidcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgOiAnPGRpdiBjbGFzcz1cInNlbGVjdGl2aXR5LW11bHRpcGxlLWlucHV0ICcgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICdzZWxlY3Rpdml0eS1wbGFjZWhvbGRlclwiPjwvZGl2PicpICtcclxuICAgICAgICAgICAgICAgICc8ZGl2IGNsYXNzPVwic2VsZWN0aXZpdHktY2xlYXJmaXhcIj48L2Rpdj4nICtcclxuICAgICAgICAgICAgJzwvZGl2PidcclxuICAgICAgICApO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlbmRlcnMgYSBzZWxlY3RlZCBpdGVtIGluIG11bHRpLXNlbGVjdGlvbiBpbnB1dCBib3hlcy5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgdGVtcGxhdGUgaXMgZXhwZWN0ZWQgdG8gaGF2ZSBhIHRvcC1sZXZlbCBlbGVtZW50IHdpdGggdGhlIGNsYXNzXHJcbiAgICAgKiAnc2VsZWN0aXZpdHktbXVsdGlwbGUtc2VsZWN0ZWQtaXRlbScuIFRoaXMgZWxlbWVudCBpcyBhbHNvIHJlcXVpcmVkIHRvIGhhdmUgYSAnZGF0YS1pdGVtLWlkJ1xyXG4gICAgICogYXR0cmlidXRlIHdpdGggdGhlIElEIHNldCB0byB0aGF0IHBhc3NlZCB0aHJvdWdoIHRoZSBvcHRpb25zIG9iamVjdC5cclxuICAgICAqXHJcbiAgICAgKiBBbiBlbGVtZW50IHdpdGggdGhlIGNsYXNzICdzZWxlY3Rpdml0eS1tdWx0aXBsZS1zZWxlY3RlZC1pdGVtLXJlbW92ZScgc2hvdWxkIGJlIHByZXNlbnRcclxuICAgICAqIHdoaWNoLCB3aGVuIGNsaWNrZWQsIHdpbGwgY2F1c2UgdGhlIGVsZW1lbnQgdG8gYmUgcmVtb3ZlZC5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25zIG9iamVjdCBjb250YWluaW5nIHRoZSBmb2xsb3dpbmcgcHJvcGVydGllczpcclxuICAgICAqICAgICAgICAgICAgICAgIGhpZ2hsaWdodGVkIC0gQm9vbGVhbiB3aGV0aGVyIHRoaXMgaXRlbSBpcyBjdXJyZW50bHkgaGlnaGxpZ2h0ZWQuXHJcbiAgICAgKiAgICAgICAgICAgICAgICBpZCAtIElkZW50aWZpZXIgZm9yIHRoZSBpdGVtLlxyXG4gICAgICogICAgICAgICAgICAgICAgcmVtb3ZhYmxlIC0gQm9vbGVhbiB3aGV0aGVyIGEgcmVtb3ZlIGljb24gc2hvdWxkIGJlIGRpc3BsYXllZC5cclxuICAgICAqICAgICAgICAgICAgICAgIHRleHQgLSBUZXh0IGxhYmVsIHdoaWNoIHRoZSB1c2VyIHNlZXMuXHJcbiAgICAgKi9cclxuICAgIG11bHRpcGxlU2VsZWN0ZWRJdGVtOiBmdW5jdGlvbihvcHRpb25zKSB7XHJcbiAgICAgICAgdmFyIGV4dHJhQ2xhc3MgPSAob3B0aW9ucy5oaWdobGlnaHRlZCA/ICcgaGlnaGxpZ2h0ZWQnIDogJycpO1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgICc8c3BhbiBjbGFzcz1cInNlbGVjdGl2aXR5LW11bHRpcGxlLXNlbGVjdGVkLWl0ZW0nICsgZXh0cmFDbGFzcyArICdcIiAnICtcclxuICAgICAgICAgICAgICAgICAgJ2RhdGEtaXRlbS1pZD1cIicgKyBlc2NhcGUob3B0aW9ucy5pZCkgKyAnXCI+JyArXHJcbiAgICAgICAgICAgICAgICAob3B0aW9ucy5yZW1vdmFibGUgPyAnPGEgY2xhc3M9XCJzZWxlY3Rpdml0eS1tdWx0aXBsZS1zZWxlY3RlZC1pdGVtLXJlbW92ZVwiPicgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICc8aSBjbGFzcz1cImZhIGZhLXJlbW92ZVwiPjwvaT4nICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICc8L2E+J1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDogJycpICtcclxuICAgICAgICAgICAgICAgIGVzY2FwZShvcHRpb25zLnRleHQpICtcclxuICAgICAgICAgICAgJzwvc3Bhbj4nXHJcbiAgICAgICAgKTtcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBSZW5kZXJzIGEgbWVzc2FnZSB0aGVyZSBhcmUgbm8gcmVzdWx0cyBmb3IgdGhlIGdpdmVuIHF1ZXJ5LlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0IGNvbnRhaW5pbmcgdGhlIGZvbGxvd2luZyBwcm9wZXJ0eTpcclxuICAgICAqICAgICAgICAgICAgICAgIHRlcm0gLSBTZWFyY2ggdGVybSB0aGUgdXNlciBpcyBzZWFyY2hpbmcgZm9yLlxyXG4gICAgICovXHJcbiAgICBub1Jlc3VsdHM6IGZ1bmN0aW9uKG9wdGlvbnMpIHtcclxuICAgICAgICB2YXIgTG9jYWxlID0gU2VsZWN0aXZpdHkuTG9jYWxlO1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgICc8ZGl2IGNsYXNzPVwic2VsZWN0aXZpdHktZXJyb3JcIj4nICtcclxuICAgICAgICAgICAgICAgIChvcHRpb25zLnRlcm0gPyBMb2NhbGUubm9SZXN1bHRzRm9yVGVybShvcHRpb25zLnRlcm0pIDogTG9jYWxlLm5vUmVzdWx0cykgK1xyXG4gICAgICAgICAgICAnPC9kaXY+J1xyXG4gICAgICAgICk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVuZGVycyBhIGNvbnRhaW5lciBmb3IgaXRlbSBjaGlsZHJlbi5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgdGVtcGxhdGUgaXMgZXhwZWN0ZWQgdG8gaGF2ZSBhbiBlbGVtZW50IHdpdGggdGhlIGNsYXNzICdzZWxlY3Rpdml0eS1yZXN1bHQtY2hpbGRyZW4nLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0IGNvbnRhaW5pbmcgdGhlIGZvbGxvd2luZyBwcm9wZXJ0eTpcclxuICAgICAqICAgICAgICAgICAgICAgIGNoaWxkcmVuSHRtbCAtIFJlbmRlcmVkIEhUTUwgZm9yIHRoZSBjaGlsZHJlbi5cclxuICAgICAqL1xyXG4gICAgcmVzdWx0Q2hpbGRyZW46IGZ1bmN0aW9uKG9wdGlvbnMpIHtcclxuICAgICAgICByZXR1cm4gJzxkaXYgY2xhc3M9XCJzZWxlY3Rpdml0eS1yZXN1bHQtY2hpbGRyZW5cIj4nICsgb3B0aW9ucy5jaGlsZHJlbkh0bWwgKyAnPC9kaXY+JztcclxuICAgIH0sXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBSZW5kZXIgYSByZXN1bHQgaXRlbSBpbiB0aGUgZHJvcGRvd24uXHJcbiAgICAgKlxyXG4gICAgICogVGhlIHRlbXBsYXRlIGlzIGV4cGVjdGVkIHRvIGhhdmUgYSB0b3AtbGV2ZWwgZWxlbWVudCB3aXRoIHRoZSBjbGFzc1xyXG4gICAgICogJ3NlbGVjdGl2aXR5LXJlc3VsdC1pdGVtJy4gVGhpcyBlbGVtZW50IGlzIGFsc28gcmVxdWlyZWQgdG8gaGF2ZSBhICdkYXRhLWl0ZW0taWQnIGF0dHJpYnV0ZVxyXG4gICAgICogd2l0aCB0aGUgSUQgc2V0IHRvIHRoYXQgcGFzc2VkIHRocm91Z2ggdGhlIG9wdGlvbnMgb2JqZWN0LlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0IGNvbnRhaW5pbmcgdGhlIGZvbGxvd2luZyBwcm9wZXJ0aWVzOlxyXG4gICAgICogICAgICAgICAgICAgICAgaWQgLSBJZGVudGlmaWVyIGZvciB0aGUgaXRlbS5cclxuICAgICAqICAgICAgICAgICAgICAgIHRleHQgLSBUZXh0IGxhYmVsIHdoaWNoIHRoZSB1c2VyIHNlZXMuXHJcbiAgICAgKiAgICAgICAgICAgICAgICBkaXNhYmxlZCAtIFRydXRoeSBpZiB0aGUgaXRlbSBzaG91bGQgYmUgZGlzYWJsZWQuXHJcbiAgICAgKiAgICAgICAgICAgICAgICBzdWJtZW51IC0gVHJ1dGh5IGlmIHRoZSByZXN1bHQgaXRlbSBoYXMgYSBtZW51IHdpdGggc3VicmVzdWx0cy5cclxuICAgICAqL1xyXG4gICAgcmVzdWx0SXRlbTogZnVuY3Rpb24ob3B0aW9ucykge1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgICc8ZGl2IGNsYXNzPVwic2VsZWN0aXZpdHktcmVzdWx0LWl0ZW0nICsgKG9wdGlvbnMuZGlzYWJsZWQgPyAnIGRpc2FibGVkJyA6ICcnKSArICdcIicgK1xyXG4gICAgICAgICAgICAgICAgJyBkYXRhLWl0ZW0taWQ9XCInICsgZXNjYXBlKG9wdGlvbnMuaWQpICsgJ1wiPicgK1xyXG4gICAgICAgICAgICAgICAgZXNjYXBlKG9wdGlvbnMudGV4dCkgK1xyXG4gICAgICAgICAgICAgICAgKG9wdGlvbnMuc3VibWVudSA/ICc8aSBjbGFzcz1cInNlbGVjdGl2aXR5LXN1Ym1lbnUtaWNvbiBmYSBmYS1jaGV2cm9uLXJpZ2h0XCI+PC9pPidcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgOiAnJykgK1xyXG4gICAgICAgICAgICAnPC9kaXY+J1xyXG4gICAgICAgICk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVuZGVyIGEgcmVzdWx0IGxhYmVsIGluIHRoZSBkcm9wZG93bi5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgdGVtcGxhdGUgaXMgZXhwZWN0ZWQgdG8gaGF2ZSBhIHRvcC1sZXZlbCBlbGVtZW50IHdpdGggdGhlIGNsYXNzXHJcbiAgICAgKiAnc2VsZWN0aXZpdHktcmVzdWx0LWxhYmVsJy5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25zIG9iamVjdCBjb250YWluaW5nIHRoZSBmb2xsb3dpbmcgcHJvcGVydGllczpcclxuICAgICAqICAgICAgICAgICAgICAgIHRleHQgLSBUZXh0IGxhYmVsLlxyXG4gICAgICovXHJcbiAgICByZXN1bHRMYWJlbDogZnVuY3Rpb24ob3B0aW9ucykge1xyXG4gICAgICAgIHJldHVybiAnPGRpdiBjbGFzcz1cInNlbGVjdGl2aXR5LXJlc3VsdC1sYWJlbFwiPicgKyBlc2NhcGUob3B0aW9ucy50ZXh0KSArICc8L2Rpdj4nO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlbmRlcnMgc2luZ2xlLXNlbGVjdCBpbnB1dCBib3hlcy5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgdGVtcGxhdGUgaXMgZXhwZWN0ZWQgdG8gaGF2ZSBhdCBsZWFzdCBvbmUgZWxlbWVudCB3aXRoIHRoZSBjbGFzc1xyXG4gICAgICogJ3NlbGVjdGl2aXR5LXNpbmdsZS1yZXN1bHQtY29udGFpbmVyJyB3aGljaCBpcyB0aGUgZWxlbWVudCBjb250YWluaW5nIHRoZSBzZWxlY3RlZCBpdGVtIG9yXHJcbiAgICAgKiB0aGUgcGxhY2Vob2xkZXIuXHJcbiAgICAgKi9cclxuICAgIHNpbmdsZVNlbGVjdElucHV0OiAoXHJcbiAgICAgICAgJzxkaXYgY2xhc3M9XCJzZWxlY3Rpdml0eS1zaW5nbGUtc2VsZWN0XCI+JyArXHJcbiAgICAgICAgICAgICc8aW5wdXQgdHlwZT1cInRleHRcIiBjbGFzcz1cInNlbGVjdGl2aXR5LXNpbmdsZS1zZWxlY3QtaW5wdXRcIj4nICtcclxuICAgICAgICAgICAgJzxkaXYgY2xhc3M9XCJzZWxlY3Rpdml0eS1zaW5nbGUtcmVzdWx0LWNvbnRhaW5lclwiPjwvZGl2PicgK1xyXG4gICAgICAgICAgICAnPGkgY2xhc3M9XCJmYSBmYS1zb3J0LWRlc2Mgc2VsZWN0aXZpdHktY2FyZXRcIj48L2k+JyArXHJcbiAgICAgICAgJzwvZGl2PidcclxuICAgICksXHJcblxyXG4gICAgLyoqXHJcbiAgICAgKiBSZW5kZXJzIHRoZSBwbGFjZWhvbGRlciBmb3Igc2luZ2xlLXNlbGVjdCBpbnB1dCBib3hlcy5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgdGVtcGxhdGUgaXMgZXhwZWN0ZWQgdG8gaGF2ZSBhIHRvcC1sZXZlbCBlbGVtZW50IHdpdGggdGhlIGNsYXNzXHJcbiAgICAgKiAnc2VsZWN0aXZpdHktcGxhY2Vob2xkZXInLlxyXG4gICAgICpcclxuICAgICAqIEBwYXJhbSBvcHRpb25zIE9wdGlvbnMgb2JqZWN0IGNvbnRhaW5pbmcgdGhlIGZvbGxvd2luZyBwcm9wZXJ0eTpcclxuICAgICAqICAgICAgICAgICAgICAgIHBsYWNlaG9sZGVyIC0gVGhlIHBsYWNlaG9sZGVyIHRleHQuXHJcbiAgICAgKi9cclxuICAgIHNpbmdsZVNlbGVjdFBsYWNlaG9sZGVyOiBmdW5jdGlvbihvcHRpb25zKSB7XHJcbiAgICAgICAgcmV0dXJuIChcclxuICAgICAgICAgICAgJzxkaXYgY2xhc3M9XCJzZWxlY3Rpdml0eS1wbGFjZWhvbGRlclwiPicgK1xyXG4gICAgICAgICAgICAgICAgZXNjYXBlKG9wdGlvbnMucGxhY2Vob2xkZXIpICtcclxuICAgICAgICAgICAgJzwvZGl2PidcclxuICAgICAgICApO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlbmRlcnMgdGhlIHNlbGVjdGVkIGl0ZW0gaW4gc2luZ2xlLXNlbGVjdCBpbnB1dCBib3hlcy5cclxuICAgICAqXHJcbiAgICAgKiBUaGUgdGVtcGxhdGUgaXMgZXhwZWN0ZWQgdG8gaGF2ZSBhIHRvcC1sZXZlbCBlbGVtZW50IHdpdGggdGhlIGNsYXNzXHJcbiAgICAgKiAnc2VsZWN0aXZpdHktc2luZ2xlLXNlbGVjdGVkLWl0ZW0nLiBUaGlzIGVsZW1lbnQgaXMgYWxzbyByZXF1aXJlZCB0byBoYXZlIGEgJ2RhdGEtaXRlbS1pZCdcclxuICAgICAqIGF0dHJpYnV0ZSB3aXRoIHRoZSBJRCBzZXQgdG8gdGhhdCBwYXNzZWQgdGhyb3VnaCB0aGUgb3B0aW9ucyBvYmplY3QuXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIG9wdGlvbnMgT3B0aW9ucyBvYmplY3QgY29udGFpbmluZyB0aGUgZm9sbG93aW5nIHByb3BlcnRpZXM6XHJcbiAgICAgKiAgICAgICAgICAgICAgICBpZCAtIElkZW50aWZpZXIgZm9yIHRoZSBpdGVtLlxyXG4gICAgICogICAgICAgICAgICAgICAgcmVtb3ZhYmxlIC0gQm9vbGVhbiB3aGV0aGVyIGEgcmVtb3ZlIGljb24gc2hvdWxkIGJlIGRpc3BsYXllZC5cclxuICAgICAqICAgICAgICAgICAgICAgIHRleHQgLSBUZXh0IGxhYmVsIHdoaWNoIHRoZSB1c2VyIHNlZXMuXHJcbiAgICAgKi9cclxuICAgIHNpbmdsZVNlbGVjdGVkSXRlbTogZnVuY3Rpb24ob3B0aW9ucykge1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgICc8c3BhbiBjbGFzcz1cInNlbGVjdGl2aXR5LXNpbmdsZS1zZWxlY3RlZC1pdGVtXCIgJyArXHJcbiAgICAgICAgICAgICAgICAgICdkYXRhLWl0ZW0taWQ9XCInICsgZXNjYXBlKG9wdGlvbnMuaWQpICsgJ1wiPicgK1xyXG4gICAgICAgICAgICAgICAgKG9wdGlvbnMucmVtb3ZhYmxlID8gJzxhIGNsYXNzPVwic2VsZWN0aXZpdHktc2luZ2xlLXNlbGVjdGVkLWl0ZW0tcmVtb3ZlXCI+JyArXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgJzxpIGNsYXNzPVwiZmEgZmEtcmVtb3ZlXCI+PC9pPicgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgJzwvYT4nXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgOiAnJykgK1xyXG4gICAgICAgICAgICAgICAgZXNjYXBlKG9wdGlvbnMudGV4dCkgK1xyXG4gICAgICAgICAgICAnPC9zcGFuPidcclxuICAgICAgICApO1xyXG4gICAgfSxcclxuXHJcbiAgICAvKipcclxuICAgICAqIFJlbmRlcnMgc2VsZWN0LWJveCBpbnNpZGUgc2luZ2xlLXNlbGVjdCBpbnB1dCB0aGF0IHdhcyBpbml0aWFsaXplZCBvblxyXG4gICAgICogdHJhZGl0aW9uYWwgPHNlbGVjdD4gZWxlbWVudC5cclxuICAgICAqXHJcbiAgICAgKiBAcGFyYW0gb3B0aW9ucyBPcHRpb25zIG9iamVjdCBjb250YWluaW5nIHRoZSBmb2xsb3dpbmcgcHJvcGVydGllczpcclxuICAgICAqICAgICAgICAgICAgICAgIG5hbWUgLSBOYW1lIG9mIHRoZSA8c2VsZWN0PiBlbGVtZW50LlxyXG4gICAgICogICAgICAgICAgICAgICAgbW9kZSAtIE1vZGUgaW4gd2hpY2ggc2VsZWN0IGV4aXN0cywgc2luZ2xlIG9yIG11bHRpcGxlLlxyXG4gICAgICovXHJcbiAgICBzZWxlY3RDb21wbGlhbmNlOiBmdW5jdGlvbihvcHRpb25zKSB7XHJcbiAgICAgICAgdmFyIG1vZGUgPSBvcHRpb25zLm1vZGU7XHJcbiAgICAgICAgdmFyIG5hbWUgPSBvcHRpb25zLm5hbWU7XHJcbiAgICAgICAgaWYgKG1vZGUgPT09ICdtdWx0aXBsZScgJiYgbmFtZS5zbGljZSgtMikgIT09ICdbXScpIHtcclxuICAgICAgICAgICAgbmFtZSArPSAnW10nO1xyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICAnPHNlbGVjdCBuYW1lPVwiJyArIG5hbWUgKyAnXCInICsgKG1vZGUgPT09ICdtdWx0aXBsZScgPyAnIG11bHRpcGxlJyA6ICcnKSArICc+PC9zZWxlY3Q+J1xyXG4gICAgICAgICk7XHJcbiAgICB9LFxyXG5cclxuICAgIC8qKlxyXG4gICAgICogUmVuZGVycyB0aGUgc2VsZWN0ZWQgaXRlbSBpbiBjb21wbGlhbmNlIDxzZWxlY3Q+IGVsZW1lbnQgYXMgPG9wdGlvbj4uXHJcbiAgICAgKlxyXG4gICAgICogQHBhcmFtIG9wdGlvbnMgT3B0aW9ucyBvYmplY3QgY29udGFpbmluZyB0aGUgZm9sbG93aW5nIHByb3BlcnRpZXNcclxuICAgICAqICAgICAgICAgICAgICAgIGlkIC0gSWRlbnRpZmllciBmb3IgdGhlIGl0ZW0uXHJcbiAgICAgKiAgICAgICAgICAgICAgICB0ZXh0IC0gVGV4dCBsYWJlbCB3aGljaCB0aGUgdXNlciBzZWVzLlxyXG4gICAgICovXHJcbiAgICBzZWxlY3RPcHRpb25Db21wbGlhbmNlOiBmdW5jdGlvbihvcHRpb25zKSB7XHJcbiAgICAgICAgcmV0dXJuIChcclxuICAgICAgICAgICAgJzxvcHRpb24gdmFsdWU9XCInICsgZXNjYXBlKG9wdGlvbnMuaWQpICsgJ1wiIHNlbGVjdGVkPicgK1xyXG4gICAgICAgICAgICAgICAgZXNjYXBlKG9wdGlvbnMudGV4dCkgK1xyXG4gICAgICAgICAgICAnPC9vcHRpb24+J1xyXG4gICAgICAgICk7XHJcbiAgICB9XHJcblxyXG59O1xyXG5cclxufSx7XCIxM1wiOjEzLFwiNFwiOjQsXCI4XCI6OH1dLDE4OltmdW5jdGlvbihfZGVyZXFfLG1vZHVsZSxleHBvcnRzKXtcclxuJ3VzZSBzdHJpY3QnO1xyXG5cclxudmFyICQgPSB3aW5kb3cualF1ZXJ5IHx8IHdpbmRvdy5aZXB0bztcclxuXHJcbnZhciBTZWxlY3Rpdml0eSA9IF9kZXJlcV8oOCk7XHJcblxyXG5mdW5jdGlvbiBkZWZhdWx0VG9rZW5pemVyKGlucHV0LCBzZWxlY3Rpb24sIGNyZWF0ZVRva2VuLCBvcHRpb25zKSB7XHJcblxyXG4gICAgdmFyIGNyZWF0ZVRva2VuSXRlbSA9IG9wdGlvbnMuY3JlYXRlVG9rZW5JdGVtIHx8IGZ1bmN0aW9uKHRva2VuKSB7XHJcbiAgICAgICAgcmV0dXJuIHRva2VuID8geyBpZDogdG9rZW4sIHRleHQ6IHRva2VuIH0gOiBudWxsO1xyXG4gICAgfTtcclxuXHJcbiAgICB2YXIgc2VwYXJhdG9ycyA9IG9wdGlvbnMudG9rZW5TZXBhcmF0b3JzO1xyXG5cclxuICAgIGZ1bmN0aW9uIGhhc1Rva2VuKGlucHV0KSB7XHJcbiAgICAgICAgcmV0dXJuIGlucHV0ID8gc2VwYXJhdG9ycy5zb21lKGZ1bmN0aW9uKHNlcGFyYXRvcikge1xyXG4gICAgICAgICAgICByZXR1cm4gaW5wdXQuaW5kZXhPZihzZXBhcmF0b3IpID4gLTE7XHJcbiAgICAgICAgfSkgOiBmYWxzZTtcclxuICAgIH1cclxuXHJcbiAgICBmdW5jdGlvbiB0YWtlVG9rZW4oaW5wdXQpIHtcclxuICAgICAgICBmb3IgKHZhciBpID0gMCwgbGVuZ3RoID0gaW5wdXQubGVuZ3RoOyBpIDwgbGVuZ3RoOyBpKyspIHtcclxuICAgICAgICAgICAgaWYgKHNlcGFyYXRvcnMuaW5kZXhPZihpbnB1dFtpXSkgPiAtMSkge1xyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHsgdGVybTogaW5wdXQuc2xpY2UoMCwgaSksIGlucHV0OiBpbnB1dC5zbGljZShpICsgMSkgfTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4ge307XHJcbiAgICB9XHJcblxyXG4gICAgd2hpbGUgKGhhc1Rva2VuKGlucHV0KSkge1xyXG4gICAgICAgIHZhciB0b2tlbiA9IHRha2VUb2tlbihpbnB1dCk7XHJcbiAgICAgICAgaWYgKHRva2VuLnRlcm0pIHtcclxuICAgICAgICAgICAgdmFyIGl0ZW0gPSBjcmVhdGVUb2tlbkl0ZW0odG9rZW4udGVybSk7XHJcbiAgICAgICAgICAgIGlmIChpdGVtICYmICFTZWxlY3Rpdml0eS5maW5kQnlJZChzZWxlY3Rpb24sIGl0ZW0uaWQpKSB7XHJcbiAgICAgICAgICAgICAgICBjcmVhdGVUb2tlbihpdGVtKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgICAgICBpbnB1dCA9IHRva2VuLmlucHV0O1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBpbnB1dDtcclxufVxyXG5cclxuLyoqXHJcbiAqIE9wdGlvbiBsaXN0ZW5lciB0aGF0IHByb3ZpZGVzIGEgZGVmYXVsdCB0b2tlbml6ZXIgd2hpY2ggaXMgdXNlZCB3aGVuIHRoZSB0b2tlblNlcGFyYXRvcnMgb3B0aW9uXHJcbiAqIGlzIHNwZWNpZmllZC5cclxuICpcclxuICogQHBhcmFtIG9wdGlvbnMgT3B0aW9ucyBvYmplY3QuIEluIGFkZGl0aW9uIHRvIHRoZSBvcHRpb25zIHN1cHBvcnRlZCBpbiB0aGUgbXVsdGktaW5wdXRcclxuICogICAgICAgICAgICAgICAgaW1wbGVtZW50YXRpb24sIHRoaXMgbWF5IGNvbnRhaW4gdGhlIGZvbGxvd2luZyBwcm9wZXJ0eTpcclxuICogICAgICAgICAgICAgICAgdG9rZW5TZXBhcmF0b3JzIC0gQXJyYXkgb2Ygc3RyaW5nIHNlcGFyYXRvcnMgd2hpY2ggYXJlIHVzZWQgdG8gc2VwYXJhdGUgdGhlIHNlYXJjaFxyXG4gKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBzdHJpbmcgaW50byB0b2tlbnMuIElmIHNwZWNpZmllZCBhbmQgdGhlIHRva2VuaXplciBwcm9wZXJ0eSBpc1xyXG4gKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBub3Qgc2V0LCB0aGUgdG9rZW5pemVyIHByb3BlcnR5IHdpbGwgYmUgc2V0IHRvIGEgZnVuY3Rpb24gd2hpY2hcclxuICogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgc3BsaXRzIHRoZSBzZWFyY2ggdGVybSBpbnRvIHRva2VucyBzZXBhcmF0ZWQgYnkgYW55IG9mIHRoZSBnaXZlblxyXG4gKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBzZXBhcmF0b3JzLiBUaGUgdG9rZW5zIHdpbGwgYmUgY29udmVydGVkIGludG8gc2VsZWN0YWJsZSBpdGVtc1xyXG4gKiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB1c2luZyB0aGUgJ2NyZWF0ZVRva2VuSXRlbScgZnVuY3Rpb24uIFRoZSBkZWZhdWx0IHRva2VuaXplciBhbHNvXHJcbiAqICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGZpbHRlcnMgb3V0IGFscmVhZHkgc2VsZWN0ZWQgaXRlbXMuXHJcbiAqL1xyXG5TZWxlY3Rpdml0eS5PcHRpb25MaXN0ZW5lcnMucHVzaChmdW5jdGlvbihzZWxlY3Rpdml0eSwgb3B0aW9ucykge1xyXG5cclxuICAgIGlmIChvcHRpb25zLnRva2VuU2VwYXJhdG9ycykge1xyXG4gICAgICAgIG9wdGlvbnMuYWxsb3dlZFR5cGVzID0gJC5leHRlbmQoeyB0b2tlblNlcGFyYXRvcnM6ICdhcnJheScgfSwgb3B0aW9ucy5hbGxvd2VkVHlwZXMpO1xyXG5cclxuICAgICAgICBvcHRpb25zLnRva2VuaXplciA9IG9wdGlvbnMudG9rZW5pemVyIHx8IGRlZmF1bHRUb2tlbml6ZXI7XHJcbiAgICB9XHJcbn0pO1xyXG5cclxufSx7XCI4XCI6OCxcImpxdWVyeVwiOlwianF1ZXJ5XCJ9XSwxOTpbZnVuY3Rpb24oX2RlcmVxXyxtb2R1bGUsZXhwb3J0cyl7XHJcbid1c2Ugc3RyaWN0JztcclxuXHJcbnZhciAkID0gd2luZG93LmpRdWVyeSB8fCB3aW5kb3cuWmVwdG87XHJcblxyXG52YXIgU2VsZWN0aXZpdHkgPSBfZGVyZXFfKDgpO1xyXG5cclxuZnVuY3Rpb24gcmVwbGFjZVNlbGVjdEVsZW1lbnQoJGVsLCBvcHRpb25zKSB7XHJcblxyXG4gICAgdmFyIGRhdGEgPSAob3B0aW9ucy5tdWx0aXBsZSA/IFtdIDogbnVsbCk7XHJcblxyXG4gICAgdmFyIG1hcE9wdGlvbnMgPSBmdW5jdGlvbigpIHtcclxuICAgICAgICB2YXIgJHRoaXMgPSAkKHRoaXMpO1xyXG4gICAgICAgIGlmICgkdGhpcy5pcygnb3B0aW9uJykpIHtcclxuICAgICAgICAgICAgdmFyIHRleHQgPSAkdGhpcy50ZXh0KCk7XHJcbiAgICAgICAgICAgIHZhciBpZCA9ICR0aGlzLmF0dHIoJ3ZhbHVlJykgfHwgdGV4dDtcclxuICAgICAgICAgICAgaWYgKCR0aGlzLnByb3AoJ3NlbGVjdGVkJykpIHtcclxuICAgICAgICAgICAgICAgIHZhciBpdGVtID0geyBpZDogaWQsIHRleHQ6IHRleHQgfTtcclxuICAgICAgICAgICAgICAgIGlmIChvcHRpb25zLm11bHRpcGxlKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgZGF0YS5wdXNoKGl0ZW0pO1xyXG4gICAgICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgICAgICBkYXRhID0gaXRlbTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgcmV0dXJuIHtcclxuICAgICAgICAgICAgICAgIGlkOiBpZCxcclxuICAgICAgICAgICAgICAgIHRleHQ6ICR0aGlzLmF0dHIoJ2xhYmVsJykgfHwgdGV4dFxyXG4gICAgICAgICAgICB9O1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHJldHVybiB7XHJcbiAgICAgICAgICAgICAgICB0ZXh0OiAkdGhpcy5hdHRyKCdsYWJlbCcpLFxyXG4gICAgICAgICAgICAgICAgY2hpbGRyZW46ICR0aGlzLmNoaWxkcmVuKCdvcHRpb24sb3B0Z3JvdXAnKS5tYXAobWFwT3B0aW9ucykuZ2V0KClcclxuICAgICAgICAgICAgfTtcclxuICAgICAgICB9XHJcbiAgICB9O1xyXG5cclxuICAgIG9wdGlvbnMuYWxsb3dDbGVhciA9ICgnYWxsb3dDbGVhcicgaW4gb3B0aW9ucyA/IG9wdGlvbnMuYWxsb3dDbGVhciA6ICEkZWwucHJvcCgncmVxdWlyZWQnKSk7XHJcblxyXG4gICAgdmFyIGl0ZW1zID0gJGVsLmNoaWxkcmVuKCdvcHRpb24sb3B0Z3JvdXAnKS5tYXAobWFwT3B0aW9ucykuZ2V0KCk7XHJcbiAgICBvcHRpb25zLml0ZW1zID0gKG9wdGlvbnMucXVlcnkgPyBudWxsIDogaXRlbXMpO1xyXG5cclxuICAgIG9wdGlvbnMucGxhY2Vob2xkZXIgPSBvcHRpb25zLnBsYWNlaG9sZGVyIHx8ICRlbC5kYXRhKCdwbGFjZWhvbGRlcicpIHx8ICcnO1xyXG5cclxuICAgIG9wdGlvbnMuZGF0YSA9IGRhdGE7XHJcblxyXG4gICAgdmFyIGNsYXNzZXMgPSAoJGVsLmF0dHIoJ2NsYXNzJykgfHwgJ3NlbGVjdGl2aXR5LWlucHV0Jykuc3BsaXQoJyAnKTtcclxuICAgIGlmIChjbGFzc2VzLmluZGV4T2YoJ3NlbGVjdGl2aXR5LWlucHV0JykgPT09IC0xKSB7XHJcbiAgICAgICAgY2xhc3Nlcy5wdXNoKCdzZWxlY3Rpdml0eS1pbnB1dCcpO1xyXG4gICAgfVxyXG5cclxuICAgIHZhciAkZGl2ID0gJCgnPGRpdj4nKS5hdHRyKHtcclxuICAgICAgICAnaWQnOiAkZWwuYXR0cignaWQnKSxcclxuICAgICAgICAnY2xhc3MnOiBjbGFzc2VzLmpvaW4oJyAnKSxcclxuICAgICAgICAnc3R5bGUnOiAkZWwuYXR0cignc3R5bGUnKSxcclxuICAgICAgICAnZGF0YS1uYW1lJzogJGVsLmF0dHIoJ25hbWUnKVxyXG4gICAgfSk7XHJcbiAgICAkZWwucmVwbGFjZVdpdGgoJGRpdik7XHJcbiAgICByZXR1cm4gJGRpdjtcclxufVxyXG5cclxuZnVuY3Rpb24gYmluZFRyYWRpdGlvbmFsU2VsZWN0RXZlbnRzKHNlbGVjdGl2aXR5KSB7XHJcblxyXG4gICAgdmFyICRlbCA9IHNlbGVjdGl2aXR5LiRlbDtcclxuXHJcbiAgICAkZWwub24oJ3NlbGVjdGl2aXR5LWluaXQnLCBmdW5jdGlvbihldmVudCwgbW9kZSkge1xyXG4gICAgICAgICRlbC5hcHBlbmQoc2VsZWN0aXZpdHkudGVtcGxhdGUoJ3NlbGVjdENvbXBsaWFuY2UnLCB7XHJcbiAgICAgICAgICAgIG1vZGU6IG1vZGUsXHJcbiAgICAgICAgICAgIG5hbWU6ICRlbC5hdHRyKCdkYXRhLW5hbWUnKVxyXG4gICAgICAgIH0pKS5yZW1vdmVBdHRyKCdkYXRhLW5hbWUnKTtcclxuICAgIH0pLm9uKCdzZWxlY3Rpdml0eS1pbml0IGNoYW5nZScsIGZ1bmN0aW9uKCkge1xyXG4gICAgICAgIHZhciBkYXRhID0gc2VsZWN0aXZpdHkuX2RhdGE7XHJcbiAgICAgICAgdmFyICRzZWxlY3QgPSAkZWwuZmluZCgnc2VsZWN0Jyk7XHJcblxyXG4gICAgICAgIGlmIChkYXRhIGluc3RhbmNlb2YgQXJyYXkpIHtcclxuICAgICAgICAgICAgJHNlbGVjdC5lbXB0eSgpO1xyXG5cclxuICAgICAgICAgICAgZGF0YS5mb3JFYWNoKGZ1bmN0aW9uKGl0ZW0pIHtcclxuICAgICAgICAgICAgICAgICRzZWxlY3QuYXBwZW5kKHNlbGVjdGl2aXR5LnRlbXBsYXRlKCdzZWxlY3RPcHRpb25Db21wbGlhbmNlJywgaXRlbSkpO1xyXG4gICAgICAgICAgICB9KTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICBpZiAoZGF0YSkge1xyXG4gICAgICAgICAgICAgICAgJHNlbGVjdC5odG1sKHNlbGVjdGl2aXR5LnRlbXBsYXRlKCdzZWxlY3RPcHRpb25Db21wbGlhbmNlJywgZGF0YSkpO1xyXG4gICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgJHNlbGVjdC5lbXB0eSgpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfSk7XHJcbn1cclxuXHJcbi8qKlxyXG4gKiBPcHRpb24gbGlzdGVuZXIgcHJvdmlkaW5nIHN1cHBvcnQgZm9yIGNvbnZlcnRpbmcgdHJhZGl0aW9uYWwgPHNlbGVjdD4gYm94ZXMgaW50byBTZWxlY3Rpdml0eVxyXG4gKiBpbnN0YW5jZXMuXHJcbiAqL1xyXG5TZWxlY3Rpdml0eS5PcHRpb25MaXN0ZW5lcnMucHVzaChmdW5jdGlvbihzZWxlY3Rpdml0eSwgb3B0aW9ucykge1xyXG5cclxuICAgIHZhciAkZWwgPSBzZWxlY3Rpdml0eS4kZWw7XHJcbiAgICBpZiAoJGVsLmlzKCdzZWxlY3QnKSkge1xyXG4gICAgICAgIGlmICgkZWwuYXR0cignYXV0b2ZvY3VzJykpIHtcclxuICAgICAgICAgICAgc2V0VGltZW91dChmdW5jdGlvbigpIHtcclxuICAgICAgICAgICAgICAgIHNlbGVjdGl2aXR5LmZvY3VzKCk7XHJcbiAgICAgICAgICAgIH0sIDEpO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgc2VsZWN0aXZpdHkuJGVsID0gcmVwbGFjZVNlbGVjdEVsZW1lbnQoJGVsLCBvcHRpb25zKTtcclxuICAgICAgICBzZWxlY3Rpdml0eS4kZWxbMF0uc2VsZWN0aXZpdHkgPSBzZWxlY3Rpdml0eTtcclxuXHJcbiAgICAgICAgYmluZFRyYWRpdGlvbmFsU2VsZWN0RXZlbnRzKHNlbGVjdGl2aXR5KTtcclxuICAgIH1cclxufSk7XHJcblxyXG59LHtcIjhcIjo4LFwianF1ZXJ5XCI6XCJqcXVlcnlcIn1dfSx7fSxbMV0pKDEpXHJcbn0pOyIsIiFmdW5jdGlvbihyb290LCBmYWN0b3J5KSB7XHJcbiAgICBcImZ1bmN0aW9uXCIgPT0gdHlwZW9mIGRlZmluZSAmJiBkZWZpbmUuYW1kID8gLy8gQU1ELiBSZWdpc3RlciBhcyBhbiBhbm9ueW1vdXMgbW9kdWxlIHVubGVzcyBhbWRNb2R1bGVJZCBpcyBzZXRcclxuICAgIGRlZmluZShbXSwgZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgcmV0dXJuIHJvb3Quc3ZnNGV2ZXJ5Ym9keSA9IGZhY3RvcnkoKTtcclxuICAgIH0pIDogXCJvYmplY3RcIiA9PSB0eXBlb2YgZXhwb3J0cyA/IG1vZHVsZS5leHBvcnRzID0gZmFjdG9yeSgpIDogcm9vdC5zdmc0ZXZlcnlib2R5ID0gZmFjdG9yeSgpO1xyXG59KHRoaXMsIGZ1bmN0aW9uKCkge1xyXG4gICAgLyohIHN2ZzRldmVyeWJvZHkgdjIuMC4zIHwgZ2l0aHViLmNvbS9qb25hdGhhbnRuZWFsL3N2ZzRldmVyeWJvZHkgKi9cclxuICAgIGZ1bmN0aW9uIGVtYmVkKHN2ZywgdGFyZ2V0KSB7XHJcbiAgICAgICAgLy8gaWYgdGhlIHRhcmdldCBleGlzdHNcclxuICAgICAgICBpZiAodGFyZ2V0KSB7XHJcbiAgICAgICAgICAgIC8vIGNyZWF0ZSBhIGRvY3VtZW50IGZyYWdtZW50IHRvIGhvbGQgdGhlIGNvbnRlbnRzIG9mIHRoZSB0YXJnZXRcclxuICAgICAgICAgICAgdmFyIGZyYWdtZW50ID0gZG9jdW1lbnQuY3JlYXRlRG9jdW1lbnRGcmFnbWVudCgpLCB2aWV3Qm94ID0gIXN2Zy5nZXRBdHRyaWJ1dGUoXCJ2aWV3Qm94XCIpICYmIHRhcmdldC5nZXRBdHRyaWJ1dGUoXCJ2aWV3Qm94XCIpO1xyXG4gICAgICAgICAgICAvLyBjb25kaXRpb25hbGx5IHNldCB0aGUgdmlld0JveCBvbiB0aGUgc3ZnXHJcbiAgICAgICAgICAgIHZpZXdCb3ggJiYgc3ZnLnNldEF0dHJpYnV0ZShcInZpZXdCb3hcIiwgdmlld0JveCk7XHJcbiAgICAgICAgICAgIC8vIGNvcHkgdGhlIGNvbnRlbnRzIG9mIHRoZSBjbG9uZSBpbnRvIHRoZSBmcmFnbWVudFxyXG4gICAgICAgICAgICBmb3IgKC8vIGNsb25lIHRoZSB0YXJnZXRcclxuICAgICAgICAgICAgdmFyIGNsb25lID0gdGFyZ2V0LmNsb25lTm9kZSghMCk7IGNsb25lLmNoaWxkTm9kZXMubGVuZ3RoOyApIHtcclxuICAgICAgICAgICAgICAgIGZyYWdtZW50LmFwcGVuZENoaWxkKGNsb25lLmZpcnN0Q2hpbGQpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIC8vIGFwcGVuZCB0aGUgZnJhZ21lbnQgaW50byB0aGUgc3ZnXHJcbiAgICAgICAgICAgIHN2Zy5hcHBlbmRDaGlsZChmcmFnbWVudCk7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG4gICAgZnVuY3Rpb24gbG9hZHJlYWR5c3RhdGVjaGFuZ2UoeGhyKSB7XHJcbiAgICAgICAgLy8gbGlzdGVuIHRvIGNoYW5nZXMgaW4gdGhlIHJlcXVlc3RcclxuICAgICAgICB4aHIub25yZWFkeXN0YXRlY2hhbmdlID0gZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgICAgIC8vIGlmIHRoZSByZXF1ZXN0IGlzIHJlYWR5XHJcbiAgICAgICAgICAgIGlmICg0ID09PSB4aHIucmVhZHlTdGF0ZSkge1xyXG4gICAgICAgICAgICAgICAgLy8gZ2V0IHRoZSBjYWNoZWQgaHRtbCBkb2N1bWVudFxyXG4gICAgICAgICAgICAgICAgdmFyIGNhY2hlZERvY3VtZW50ID0geGhyLl9jYWNoZWREb2N1bWVudDtcclxuICAgICAgICAgICAgICAgIC8vIGVuc3VyZSB0aGUgY2FjaGVkIGh0bWwgZG9jdW1lbnQgYmFzZWQgb24gdGhlIHhociByZXNwb25zZVxyXG4gICAgICAgICAgICAgICAgY2FjaGVkRG9jdW1lbnQgfHwgKGNhY2hlZERvY3VtZW50ID0geGhyLl9jYWNoZWREb2N1bWVudCA9IGRvY3VtZW50LmltcGxlbWVudGF0aW9uLmNyZWF0ZUhUTUxEb2N1bWVudChcIlwiKSwgXHJcbiAgICAgICAgICAgICAgICBjYWNoZWREb2N1bWVudC5ib2R5LmlubmVySFRNTCA9IHhoci5yZXNwb25zZVRleHQsIHhoci5fY2FjaGVkVGFyZ2V0ID0ge30pLCAvLyBjbGVhciB0aGUgeGhyIGVtYmVkcyBsaXN0IGFuZCBlbWJlZCBlYWNoIGl0ZW1cclxuICAgICAgICAgICAgICAgIHhoci5fZW1iZWRzLnNwbGljZSgwKS5tYXAoZnVuY3Rpb24oaXRlbSkge1xyXG4gICAgICAgICAgICAgICAgICAgIC8vIGdldCB0aGUgY2FjaGVkIHRhcmdldFxyXG4gICAgICAgICAgICAgICAgICAgIHZhciB0YXJnZXQgPSB4aHIuX2NhY2hlZFRhcmdldFtpdGVtLmlkXTtcclxuICAgICAgICAgICAgICAgICAgICAvLyBlbnN1cmUgdGhlIGNhY2hlZCB0YXJnZXRcclxuICAgICAgICAgICAgICAgICAgICB0YXJnZXQgfHwgKHRhcmdldCA9IHhoci5fY2FjaGVkVGFyZ2V0W2l0ZW0uaWRdID0gY2FjaGVkRG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoaXRlbS5pZCkpLCBcclxuICAgICAgICAgICAgICAgICAgICAvLyBlbWJlZCB0aGUgdGFyZ2V0IGludG8gdGhlIHN2Z1xyXG4gICAgICAgICAgICAgICAgICAgIGVtYmVkKGl0ZW0uc3ZnLCB0YXJnZXQpO1xyXG4gICAgICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9LCAvLyB0ZXN0IHRoZSByZWFkeSBzdGF0ZSBjaGFuZ2UgaW1tZWRpYXRlbHlcclxuICAgICAgICB4aHIub25yZWFkeXN0YXRlY2hhbmdlKCk7XHJcbiAgICB9XHJcbiAgICBmdW5jdGlvbiBzdmc0ZXZlcnlib2R5KHJhd29wdHMpIHtcclxuICAgICAgICBmdW5jdGlvbiBvbmludGVydmFsKCkge1xyXG4gICAgICAgICAgICAvLyB3aGlsZSB0aGUgaW5kZXggZXhpc3RzIGluIHRoZSBsaXZlIDx1c2U+IGNvbGxlY3Rpb25cclxuICAgICAgICAgICAgZm9yICgvLyBnZXQgdGhlIGNhY2hlZCA8dXNlPiBpbmRleFxyXG4gICAgICAgICAgICB2YXIgaW5kZXggPSAwOyBpbmRleCA8IHVzZXMubGVuZ3RoOyApIHtcclxuICAgICAgICAgICAgICAgIC8vIGdldCB0aGUgY3VycmVudCA8dXNlPlxyXG4gICAgICAgICAgICAgICAgdmFyIHVzZSA9IHVzZXNbaW5kZXhdLCBzdmcgPSB1c2UucGFyZW50Tm9kZTtcclxuICAgICAgICAgICAgICAgIGlmIChzdmcgJiYgL3N2Zy9pLnRlc3Qoc3ZnLm5vZGVOYW1lKSkge1xyXG4gICAgICAgICAgICAgICAgICAgIHZhciBzcmMgPSB1c2UuZ2V0QXR0cmlidXRlKFwieGxpbms6aHJlZlwiKTtcclxuICAgICAgICAgICAgICAgICAgICBpZiAocG9seWZpbGwgJiYgKCFvcHRzLnZhbGlkYXRlIHx8IG9wdHMudmFsaWRhdGUoc3JjLCBzdmcsIHVzZSkpKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIHJlbW92ZSB0aGUgPHVzZT4gZWxlbWVudFxyXG4gICAgICAgICAgICAgICAgICAgICAgICBzdmcucmVtb3ZlQ2hpbGQodXNlKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gcGFyc2UgdGhlIHNyYyBhbmQgZ2V0IHRoZSB1cmwgYW5kIGlkXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciBzcmNTcGxpdCA9IHNyYy5zcGxpdChcIiNcIiksIHVybCA9IHNyY1NwbGl0LnNoaWZ0KCksIGlkID0gc3JjU3BsaXQuam9pbihcIiNcIik7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIGlmIHRoZSBsaW5rIGlzIGV4dGVybmFsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmICh1cmwubGVuZ3RoKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAvLyBnZXQgdGhlIGNhY2hlZCB4aHIgcmVxdWVzdFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdmFyIHhociA9IHJlcXVlc3RzW3VybF07XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAvLyBlbnN1cmUgdGhlIHhociByZXF1ZXN0IGV4aXN0c1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgeGhyIHx8ICh4aHIgPSByZXF1ZXN0c1t1cmxdID0gbmV3IFhNTEh0dHBSZXF1ZXN0KCksIHhoci5vcGVuKFwiR0VUXCIsIHVybCksIHhoci5zZW5kKCksIFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgeGhyLl9lbWJlZHMgPSBbXSksIC8vIGFkZCB0aGUgc3ZnIGFuZCBpZCBhcyBhbiBpdGVtIHRvIHRoZSB4aHIgZW1iZWRzIGxpc3RcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHhoci5fZW1iZWRzLnB1c2goe1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHN2Zzogc3ZnLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlkOiBpZFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfSksIC8vIHByZXBhcmUgdGhlIHhociByZWFkeSBzdGF0ZSBjaGFuZ2UgZXZlbnRcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxvYWRyZWFkeXN0YXRlY2hhbmdlKHhocik7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAvLyBlbWJlZCB0aGUgbG9jYWwgaWQgaW50byB0aGUgc3ZnXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBlbWJlZChzdmcsIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKGlkKSk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgIC8vIGluY3JlYXNlIHRoZSBpbmRleCB3aGVuIHRoZSBwcmV2aW91cyB2YWx1ZSB3YXMgbm90IFwidmFsaWRcIlxyXG4gICAgICAgICAgICAgICAgICAgICsraW5kZXg7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgLy8gY29udGludWUgdGhlIGludGVydmFsXHJcbiAgICAgICAgICAgIHJlcXVlc3RBbmltYXRpb25GcmFtZShvbmludGVydmFsLCA2Nyk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHZhciBwb2x5ZmlsbCwgb3B0cyA9IE9iamVjdChyYXdvcHRzKSwgbmV3ZXJJRVVBID0gL1xcYlRyaWRlbnRcXC9bNTY3XVxcYnxcXGJNU0lFICg/Ojl8MTApXFwuMFxcYi8sIHdlYmtpdFVBID0gL1xcYkFwcGxlV2ViS2l0XFwvKFxcZCspXFxiLywgb2xkZXJFZGdlVUEgPSAvXFxiRWRnZVxcLzEyXFwuKFxcZCspXFxiLztcclxuICAgICAgICBwb2x5ZmlsbCA9IFwicG9seWZpbGxcIiBpbiBvcHRzID8gb3B0cy5wb2x5ZmlsbCA6IG5ld2VySUVVQS50ZXN0KG5hdmlnYXRvci51c2VyQWdlbnQpIHx8IChuYXZpZ2F0b3IudXNlckFnZW50Lm1hdGNoKG9sZGVyRWRnZVVBKSB8fCBbXSlbMV0gPCAxMDU0NyB8fCAobmF2aWdhdG9yLnVzZXJBZ2VudC5tYXRjaCh3ZWJraXRVQSkgfHwgW10pWzFdIDwgNTM3O1xyXG4gICAgICAgIC8vIGNyZWF0ZSB4aHIgcmVxdWVzdHMgb2JqZWN0XHJcbiAgICAgICAgdmFyIHJlcXVlc3RzID0ge30sIHJlcXVlc3RBbmltYXRpb25GcmFtZSA9IHdpbmRvdy5yZXF1ZXN0QW5pbWF0aW9uRnJhbWUgfHwgc2V0VGltZW91dCwgdXNlcyA9IGRvY3VtZW50LmdldEVsZW1lbnRzQnlUYWdOYW1lKFwidXNlXCIpO1xyXG4gICAgICAgIC8vIGNvbmRpdGlvbmFsbHkgc3RhcnQgdGhlIGludGVydmFsIGlmIHRoZSBwb2x5ZmlsbCBpcyBhY3RpdmVcclxuICAgICAgICBwb2x5ZmlsbCAmJiBvbmludGVydmFsKCk7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gc3ZnNGV2ZXJ5Ym9keTtcclxufSk7IiwidmFyIHRvZ2dsZUljb25zID0gZnVuY3Rpb24oY29udGFpbmVyKSB7XHJcblxyXG4gICAgaWYoIWNvbnRhaW5lcikgY29uc29sZS53YXJuKCd0b2dnbGVJY29ucyBtaXNzaW5nIGNvbnRhaW5lciBlbGVtZW50Jyk7XHJcblxyXG4gICAgJChjb250YWluZXIpLmZpbmQoJy50b2dnbGVhYmxlLWljb24nKS5lYWNoKGZ1bmN0aW9uKGluZHgsIGl0ZW0pIHtcclxuICAgICAgICAkKGl0ZW0pLmhhc0NsYXNzKCdpcy12aXNpYmxlJykgPyAkKGl0ZW0pLnJlbW92ZUNsYXNzKCdpcy12aXNpYmxlJykgOiAkKGl0ZW0pLmFkZENsYXNzKCdpcy12aXNpYmxlJyk7XHJcbiAgICB9KTtcclxuXHJcbn07XHJcblxyXG5leHBvcnQgeyB0b2dnbGVJY29ucyB9O1xyXG4iLCIvKiBaZXB0byB2MS4xLjYgLSB6ZXB0byBldmVudCBhamF4IGZvcm0gaWUgLSB6ZXB0b2pzLmNvbS9saWNlbnNlICovXHJcbnZhciBaZXB0bz1mdW5jdGlvbigpe2Z1bmN0aW9uIEwodCl7cmV0dXJuIG51bGw9PXQ/U3RyaW5nKHQpOmpbUy5jYWxsKHQpXXx8XCJvYmplY3RcIn1mdW5jdGlvbiBaKHQpe3JldHVyblwiZnVuY3Rpb25cIj09TCh0KX1mdW5jdGlvbiBfKHQpe3JldHVybiBudWxsIT10JiZ0PT10LndpbmRvd31mdW5jdGlvbiAkKHQpe3JldHVybiBudWxsIT10JiZ0Lm5vZGVUeXBlPT10LkRPQ1VNRU5UX05PREV9ZnVuY3Rpb24gRCh0KXtyZXR1cm5cIm9iamVjdFwiPT1MKHQpfWZ1bmN0aW9uIE0odCl7cmV0dXJuIEQodCkmJiFfKHQpJiZPYmplY3QuZ2V0UHJvdG90eXBlT2YodCk9PU9iamVjdC5wcm90b3R5cGV9ZnVuY3Rpb24gUih0KXtyZXR1cm5cIm51bWJlclwiPT10eXBlb2YgdC5sZW5ndGh9ZnVuY3Rpb24gayh0KXtyZXR1cm4gcy5jYWxsKHQsZnVuY3Rpb24odCl7cmV0dXJuIG51bGwhPXR9KX1mdW5jdGlvbiB6KHQpe3JldHVybiB0Lmxlbmd0aD4wP24uZm4uY29uY2F0LmFwcGx5KFtdLHQpOnR9ZnVuY3Rpb24gRih0KXtyZXR1cm4gdC5yZXBsYWNlKC86Oi9nLFwiL1wiKS5yZXBsYWNlKC8oW0EtWl0rKShbQS1aXVthLXpdKS9nLFwiJDFfJDJcIikucmVwbGFjZSgvKFthLXpcXGRdKShbQS1aXSkvZyxcIiQxXyQyXCIpLnJlcGxhY2UoL18vZyxcIi1cIikudG9Mb3dlckNhc2UoKX1mdW5jdGlvbiBxKHQpe3JldHVybiB0IGluIGY/Zlt0XTpmW3RdPW5ldyBSZWdFeHAoXCIoXnxcXFxccylcIit0K1wiKFxcXFxzfCQpXCIpfWZ1bmN0aW9uIEgodCxlKXtyZXR1cm5cIm51bWJlclwiIT10eXBlb2YgZXx8Y1tGKHQpXT9lOmUrXCJweFwifWZ1bmN0aW9uIEkodCl7dmFyIGUsbjtyZXR1cm4gdVt0XXx8KGU9YS5jcmVhdGVFbGVtZW50KHQpLGEuYm9keS5hcHBlbmRDaGlsZChlKSxuPWdldENvbXB1dGVkU3R5bGUoZSxcIlwiKS5nZXRQcm9wZXJ0eVZhbHVlKFwiZGlzcGxheVwiKSxlLnBhcmVudE5vZGUucmVtb3ZlQ2hpbGQoZSksXCJub25lXCI9PW4mJihuPVwiYmxvY2tcIiksdVt0XT1uKSx1W3RdfWZ1bmN0aW9uIFYodCl7cmV0dXJuXCJjaGlsZHJlblwiaW4gdD9vLmNhbGwodC5jaGlsZHJlbik6bi5tYXAodC5jaGlsZE5vZGVzLGZ1bmN0aW9uKHQpe3JldHVybiAxPT10Lm5vZGVUeXBlP3Q6dm9pZCAwfSl9ZnVuY3Rpb24gQihuLGkscil7Zm9yKGUgaW4gaSlyJiYoTShpW2VdKXx8QShpW2VdKSk/KE0oaVtlXSkmJiFNKG5bZV0pJiYobltlXT17fSksQShpW2VdKSYmIUEobltlXSkmJihuW2VdPVtdKSxCKG5bZV0saVtlXSxyKSk6aVtlXSE9PXQmJihuW2VdPWlbZV0pfWZ1bmN0aW9uIFUodCxlKXtyZXR1cm4gbnVsbD09ZT9uKHQpOm4odCkuZmlsdGVyKGUpfWZ1bmN0aW9uIEoodCxlLG4saSl7cmV0dXJuIFooZSk/ZS5jYWxsKHQsbixpKTplfWZ1bmN0aW9uIFgodCxlLG4pe251bGw9PW4/dC5yZW1vdmVBdHRyaWJ1dGUoZSk6dC5zZXRBdHRyaWJ1dGUoZSxuKX1mdW5jdGlvbiBXKGUsbil7dmFyIGk9ZS5jbGFzc05hbWV8fFwiXCIscj1pJiZpLmJhc2VWYWwhPT10O3JldHVybiBuPT09dD9yP2kuYmFzZVZhbDppOnZvaWQocj9pLmJhc2VWYWw9bjplLmNsYXNzTmFtZT1uKX1mdW5jdGlvbiBZKHQpe3RyeXtyZXR1cm4gdD9cInRydWVcIj09dHx8KFwiZmFsc2VcIj09dD8hMTpcIm51bGxcIj09dD9udWxsOit0K1wiXCI9PXQ/K3Q6L15bXFxbXFx7XS8udGVzdCh0KT9uLnBhcnNlSlNPTih0KTp0KTp0fWNhdGNoKGUpe3JldHVybiB0fX1mdW5jdGlvbiBHKHQsZSl7ZSh0KTtmb3IodmFyIG49MCxpPXQuY2hpbGROb2Rlcy5sZW5ndGg7aT5uO24rKylHKHQuY2hpbGROb2Rlc1tuXSxlKX12YXIgdCxlLG4saSxDLE4scj1bXSxvPXIuc2xpY2Uscz1yLmZpbHRlcixhPXdpbmRvdy5kb2N1bWVudCx1PXt9LGY9e30sYz17XCJjb2x1bW4tY291bnRcIjoxLGNvbHVtbnM6MSxcImZvbnQtd2VpZ2h0XCI6MSxcImxpbmUtaGVpZ2h0XCI6MSxvcGFjaXR5OjEsXCJ6LWluZGV4XCI6MSx6b29tOjF9LGw9L15cXHMqPChcXHcrfCEpW14+XSo+LyxoPS9ePChcXHcrKVxccypcXC8/Pig/OjxcXC9cXDE+fCkkLyxwPS88KD8hYXJlYXxicnxjb2x8ZW1iZWR8aHJ8aW1nfGlucHV0fGxpbmt8bWV0YXxwYXJhbSkoKFtcXHc6XSspW14+XSopXFwvPi9naSxkPS9eKD86Ym9keXxodG1sKSQvaSxtPS8oW0EtWl0pL2csZz1bXCJ2YWxcIixcImNzc1wiLFwiaHRtbFwiLFwidGV4dFwiLFwiZGF0YVwiLFwid2lkdGhcIixcImhlaWdodFwiLFwib2Zmc2V0XCJdLHY9W1wiYWZ0ZXJcIixcInByZXBlbmRcIixcImJlZm9yZVwiLFwiYXBwZW5kXCJdLHk9YS5jcmVhdGVFbGVtZW50KFwidGFibGVcIikseD1hLmNyZWF0ZUVsZW1lbnQoXCJ0clwiKSxiPXt0cjphLmNyZWF0ZUVsZW1lbnQoXCJ0Ym9keVwiKSx0Ym9keTp5LHRoZWFkOnksdGZvb3Q6eSx0ZDp4LHRoOngsXCIqXCI6YS5jcmVhdGVFbGVtZW50KFwiZGl2XCIpfSx3PS9jb21wbGV0ZXxsb2FkZWR8aW50ZXJhY3RpdmUvLEU9L15bXFx3LV0qJC8saj17fSxTPWoudG9TdHJpbmcsVD17fSxPPWEuY3JlYXRlRWxlbWVudChcImRpdlwiKSxQPXt0YWJpbmRleDpcInRhYkluZGV4XCIscmVhZG9ubHk6XCJyZWFkT25seVwiLFwiZm9yXCI6XCJodG1sRm9yXCIsXCJjbGFzc1wiOlwiY2xhc3NOYW1lXCIsbWF4bGVuZ3RoOlwibWF4TGVuZ3RoXCIsY2VsbHNwYWNpbmc6XCJjZWxsU3BhY2luZ1wiLGNlbGxwYWRkaW5nOlwiY2VsbFBhZGRpbmdcIixyb3dzcGFuOlwicm93U3BhblwiLGNvbHNwYW46XCJjb2xTcGFuXCIsdXNlbWFwOlwidXNlTWFwXCIsZnJhbWVib3JkZXI6XCJmcmFtZUJvcmRlclwiLGNvbnRlbnRlZGl0YWJsZTpcImNvbnRlbnRFZGl0YWJsZVwifSxBPUFycmF5LmlzQXJyYXl8fGZ1bmN0aW9uKHQpe3JldHVybiB0IGluc3RhbmNlb2YgQXJyYXl9O3JldHVybiBULm1hdGNoZXM9ZnVuY3Rpb24odCxlKXtpZighZXx8IXR8fDEhPT10Lm5vZGVUeXBlKXJldHVybiExO3ZhciBuPXQud2Via2l0TWF0Y2hlc1NlbGVjdG9yfHx0Lm1vek1hdGNoZXNTZWxlY3Rvcnx8dC5vTWF0Y2hlc1NlbGVjdG9yfHx0Lm1hdGNoZXNTZWxlY3RvcjtpZihuKXJldHVybiBuLmNhbGwodCxlKTt2YXIgaSxyPXQucGFyZW50Tm9kZSxvPSFyO3JldHVybiBvJiYocj1PKS5hcHBlbmRDaGlsZCh0KSxpPX5ULnFzYShyLGUpLmluZGV4T2YodCksbyYmTy5yZW1vdmVDaGlsZCh0KSxpfSxDPWZ1bmN0aW9uKHQpe3JldHVybiB0LnJlcGxhY2UoLy0rKC4pPy9nLGZ1bmN0aW9uKHQsZSl7cmV0dXJuIGU/ZS50b1VwcGVyQ2FzZSgpOlwiXCJ9KX0sTj1mdW5jdGlvbih0KXtyZXR1cm4gcy5jYWxsKHQsZnVuY3Rpb24oZSxuKXtyZXR1cm4gdC5pbmRleE9mKGUpPT1ufSl9LFQuZnJhZ21lbnQ9ZnVuY3Rpb24oZSxpLHIpe3ZhciBzLHUsZjtyZXR1cm4gaC50ZXN0KGUpJiYocz1uKGEuY3JlYXRlRWxlbWVudChSZWdFeHAuJDEpKSksc3x8KGUucmVwbGFjZSYmKGU9ZS5yZXBsYWNlKHAsXCI8JDE+PC8kMj5cIikpLGk9PT10JiYoaT1sLnRlc3QoZSkmJlJlZ0V4cC4kMSksaSBpbiBifHwoaT1cIipcIiksZj1iW2ldLGYuaW5uZXJIVE1MPVwiXCIrZSxzPW4uZWFjaChvLmNhbGwoZi5jaGlsZE5vZGVzKSxmdW5jdGlvbigpe2YucmVtb3ZlQ2hpbGQodGhpcyl9KSksTShyKSYmKHU9bihzKSxuLmVhY2gocixmdW5jdGlvbih0LGUpe2cuaW5kZXhPZih0KT4tMT91W3RdKGUpOnUuYXR0cih0LGUpfSkpLHN9LFQuWj1mdW5jdGlvbih0LGUpe3JldHVybiB0PXR8fFtdLHQuX19wcm90b19fPW4uZm4sdC5zZWxlY3Rvcj1lfHxcIlwiLHR9LFQuaXNaPWZ1bmN0aW9uKHQpe3JldHVybiB0IGluc3RhbmNlb2YgVC5afSxULmluaXQ9ZnVuY3Rpb24oZSxpKXt2YXIgcjtpZighZSlyZXR1cm4gVC5aKCk7aWYoXCJzdHJpbmdcIj09dHlwZW9mIGUpaWYoZT1lLnRyaW0oKSxcIjxcIj09ZVswXSYmbC50ZXN0KGUpKXI9VC5mcmFnbWVudChlLFJlZ0V4cC4kMSxpKSxlPW51bGw7ZWxzZXtpZihpIT09dClyZXR1cm4gbihpKS5maW5kKGUpO3I9VC5xc2EoYSxlKX1lbHNle2lmKFooZSkpcmV0dXJuIG4oYSkucmVhZHkoZSk7aWYoVC5pc1ooZSkpcmV0dXJuIGU7aWYoQShlKSlyPWsoZSk7ZWxzZSBpZihEKGUpKXI9W2VdLGU9bnVsbDtlbHNlIGlmKGwudGVzdChlKSlyPVQuZnJhZ21lbnQoZS50cmltKCksUmVnRXhwLiQxLGkpLGU9bnVsbDtlbHNle2lmKGkhPT10KXJldHVybiBuKGkpLmZpbmQoZSk7cj1ULnFzYShhLGUpfX1yZXR1cm4gVC5aKHIsZSl9LG49ZnVuY3Rpb24odCxlKXtyZXR1cm4gVC5pbml0KHQsZSl9LG4uZXh0ZW5kPWZ1bmN0aW9uKHQpe3ZhciBlLG49by5jYWxsKGFyZ3VtZW50cywxKTtyZXR1cm5cImJvb2xlYW5cIj09dHlwZW9mIHQmJihlPXQsdD1uLnNoaWZ0KCkpLG4uZm9yRWFjaChmdW5jdGlvbihuKXtCKHQsbixlKX0pLHR9LFQucXNhPWZ1bmN0aW9uKHQsZSl7dmFyIG4saT1cIiNcIj09ZVswXSxyPSFpJiZcIi5cIj09ZVswXSxzPWl8fHI/ZS5zbGljZSgxKTplLGE9RS50ZXN0KHMpO3JldHVybiAkKHQpJiZhJiZpPyhuPXQuZ2V0RWxlbWVudEJ5SWQocykpP1tuXTpbXToxIT09dC5ub2RlVHlwZSYmOSE9PXQubm9kZVR5cGU/W106by5jYWxsKGEmJiFpP3I/dC5nZXRFbGVtZW50c0J5Q2xhc3NOYW1lKHMpOnQuZ2V0RWxlbWVudHNCeVRhZ05hbWUoZSk6dC5xdWVyeVNlbGVjdG9yQWxsKGUpKX0sbi5jb250YWlucz1hLmRvY3VtZW50RWxlbWVudC5jb250YWlucz9mdW5jdGlvbih0LGUpe3JldHVybiB0IT09ZSYmdC5jb250YWlucyhlKX06ZnVuY3Rpb24odCxlKXtmb3IoO2UmJihlPWUucGFyZW50Tm9kZSk7KWlmKGU9PT10KXJldHVybiEwO3JldHVybiExfSxuLnR5cGU9TCxuLmlzRnVuY3Rpb249WixuLmlzV2luZG93PV8sbi5pc0FycmF5PUEsbi5pc1BsYWluT2JqZWN0PU0sbi5pc0VtcHR5T2JqZWN0PWZ1bmN0aW9uKHQpe3ZhciBlO2ZvcihlIGluIHQpcmV0dXJuITE7cmV0dXJuITB9LG4uaW5BcnJheT1mdW5jdGlvbih0LGUsbil7cmV0dXJuIHIuaW5kZXhPZi5jYWxsKGUsdCxuKX0sbi5jYW1lbENhc2U9QyxuLnRyaW09ZnVuY3Rpb24odCl7cmV0dXJuIG51bGw9PXQ/XCJcIjpTdHJpbmcucHJvdG90eXBlLnRyaW0uY2FsbCh0KX0sbi51dWlkPTAsbi5zdXBwb3J0PXt9LG4uZXhwcj17fSxuLm1hcD1mdW5jdGlvbih0LGUpe3ZhciBuLHIsbyxpPVtdO2lmKFIodCkpZm9yKHI9MDtyPHQubGVuZ3RoO3IrKyluPWUodFtyXSxyKSxudWxsIT1uJiZpLnB1c2gobik7ZWxzZSBmb3IobyBpbiB0KW49ZSh0W29dLG8pLG51bGwhPW4mJmkucHVzaChuKTtyZXR1cm4geihpKX0sbi5lYWNoPWZ1bmN0aW9uKHQsZSl7dmFyIG4saTtpZihSKHQpKXtmb3Iobj0wO248dC5sZW5ndGg7bisrKWlmKGUuY2FsbCh0W25dLG4sdFtuXSk9PT0hMSlyZXR1cm4gdH1lbHNlIGZvcihpIGluIHQpaWYoZS5jYWxsKHRbaV0saSx0W2ldKT09PSExKXJldHVybiB0O3JldHVybiB0fSxuLmdyZXA9ZnVuY3Rpb24odCxlKXtyZXR1cm4gcy5jYWxsKHQsZSl9LHdpbmRvdy5KU09OJiYobi5wYXJzZUpTT049SlNPTi5wYXJzZSksbi5lYWNoKFwiQm9vbGVhbiBOdW1iZXIgU3RyaW5nIEZ1bmN0aW9uIEFycmF5IERhdGUgUmVnRXhwIE9iamVjdCBFcnJvclwiLnNwbGl0KFwiIFwiKSxmdW5jdGlvbih0LGUpe2pbXCJbb2JqZWN0IFwiK2UrXCJdXCJdPWUudG9Mb3dlckNhc2UoKX0pLG4uZm49e2ZvckVhY2g6ci5mb3JFYWNoLHJlZHVjZTpyLnJlZHVjZSxwdXNoOnIucHVzaCxzb3J0OnIuc29ydCxpbmRleE9mOnIuaW5kZXhPZixjb25jYXQ6ci5jb25jYXQsbWFwOmZ1bmN0aW9uKHQpe3JldHVybiBuKG4ubWFwKHRoaXMsZnVuY3Rpb24oZSxuKXtyZXR1cm4gdC5jYWxsKGUsbixlKX0pKX0sc2xpY2U6ZnVuY3Rpb24oKXtyZXR1cm4gbihvLmFwcGx5KHRoaXMsYXJndW1lbnRzKSl9LHJlYWR5OmZ1bmN0aW9uKHQpe3JldHVybiB3LnRlc3QoYS5yZWFkeVN0YXRlKSYmYS5ib2R5P3Qobik6YS5hZGRFdmVudExpc3RlbmVyKFwiRE9NQ29udGVudExvYWRlZFwiLGZ1bmN0aW9uKCl7dChuKX0sITEpLHRoaXN9LGdldDpmdW5jdGlvbihlKXtyZXR1cm4gZT09PXQ/by5jYWxsKHRoaXMpOnRoaXNbZT49MD9lOmUrdGhpcy5sZW5ndGhdfSx0b0FycmF5OmZ1bmN0aW9uKCl7cmV0dXJuIHRoaXMuZ2V0KCl9LHNpemU6ZnVuY3Rpb24oKXtyZXR1cm4gdGhpcy5sZW5ndGh9LHJlbW92ZTpmdW5jdGlvbigpe3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24oKXtudWxsIT10aGlzLnBhcmVudE5vZGUmJnRoaXMucGFyZW50Tm9kZS5yZW1vdmVDaGlsZCh0aGlzKX0pfSxlYWNoOmZ1bmN0aW9uKHQpe3JldHVybiByLmV2ZXJ5LmNhbGwodGhpcyxmdW5jdGlvbihlLG4pe3JldHVybiB0LmNhbGwoZSxuLGUpIT09ITF9KSx0aGlzfSxmaWx0ZXI6ZnVuY3Rpb24odCl7cmV0dXJuIFoodCk/dGhpcy5ub3QodGhpcy5ub3QodCkpOm4ocy5jYWxsKHRoaXMsZnVuY3Rpb24oZSl7cmV0dXJuIFQubWF0Y2hlcyhlLHQpfSkpfSxhZGQ6ZnVuY3Rpb24odCxlKXtyZXR1cm4gbihOKHRoaXMuY29uY2F0KG4odCxlKSkpKX0saXM6ZnVuY3Rpb24odCl7cmV0dXJuIHRoaXMubGVuZ3RoPjAmJlQubWF0Y2hlcyh0aGlzWzBdLHQpfSxub3Q6ZnVuY3Rpb24oZSl7dmFyIGk9W107aWYoWihlKSYmZS5jYWxsIT09dCl0aGlzLmVhY2goZnVuY3Rpb24odCl7ZS5jYWxsKHRoaXMsdCl8fGkucHVzaCh0aGlzKX0pO2Vsc2V7dmFyIHI9XCJzdHJpbmdcIj09dHlwZW9mIGU/dGhpcy5maWx0ZXIoZSk6UihlKSYmWihlLml0ZW0pP28uY2FsbChlKTpuKGUpO3RoaXMuZm9yRWFjaChmdW5jdGlvbih0KXtyLmluZGV4T2YodCk8MCYmaS5wdXNoKHQpfSl9cmV0dXJuIG4oaSl9LGhhczpmdW5jdGlvbih0KXtyZXR1cm4gdGhpcy5maWx0ZXIoZnVuY3Rpb24oKXtyZXR1cm4gRCh0KT9uLmNvbnRhaW5zKHRoaXMsdCk6bih0aGlzKS5maW5kKHQpLnNpemUoKX0pfSxlcTpmdW5jdGlvbih0KXtyZXR1cm4tMT09PXQ/dGhpcy5zbGljZSh0KTp0aGlzLnNsaWNlKHQsK3QrMSl9LGZpcnN0OmZ1bmN0aW9uKCl7dmFyIHQ9dGhpc1swXTtyZXR1cm4gdCYmIUQodCk/dDpuKHQpfSxsYXN0OmZ1bmN0aW9uKCl7dmFyIHQ9dGhpc1t0aGlzLmxlbmd0aC0xXTtyZXR1cm4gdCYmIUQodCk/dDpuKHQpfSxmaW5kOmZ1bmN0aW9uKHQpe3ZhciBlLGk9dGhpcztyZXR1cm4gZT10P1wib2JqZWN0XCI9PXR5cGVvZiB0P24odCkuZmlsdGVyKGZ1bmN0aW9uKCl7dmFyIHQ9dGhpcztyZXR1cm4gci5zb21lLmNhbGwoaSxmdW5jdGlvbihlKXtyZXR1cm4gbi5jb250YWlucyhlLHQpfSl9KToxPT10aGlzLmxlbmd0aD9uKFQucXNhKHRoaXNbMF0sdCkpOnRoaXMubWFwKGZ1bmN0aW9uKCl7cmV0dXJuIFQucXNhKHRoaXMsdCl9KTpuKCl9LGNsb3Nlc3Q6ZnVuY3Rpb24odCxlKXt2YXIgaT10aGlzWzBdLHI9ITE7Zm9yKFwib2JqZWN0XCI9PXR5cGVvZiB0JiYocj1uKHQpKTtpJiYhKHI/ci5pbmRleE9mKGkpPj0wOlQubWF0Y2hlcyhpLHQpKTspaT1pIT09ZSYmISQoaSkmJmkucGFyZW50Tm9kZTtyZXR1cm4gbihpKX0scGFyZW50czpmdW5jdGlvbih0KXtmb3IodmFyIGU9W10saT10aGlzO2kubGVuZ3RoPjA7KWk9bi5tYXAoaSxmdW5jdGlvbih0KXtyZXR1cm4odD10LnBhcmVudE5vZGUpJiYhJCh0KSYmZS5pbmRleE9mKHQpPDA/KGUucHVzaCh0KSx0KTp2b2lkIDB9KTtyZXR1cm4gVShlLHQpfSxwYXJlbnQ6ZnVuY3Rpb24odCl7cmV0dXJuIFUoTih0aGlzLnBsdWNrKFwicGFyZW50Tm9kZVwiKSksdCl9LGNoaWxkcmVuOmZ1bmN0aW9uKHQpe3JldHVybiBVKHRoaXMubWFwKGZ1bmN0aW9uKCl7cmV0dXJuIFYodGhpcyl9KSx0KX0sY29udGVudHM6ZnVuY3Rpb24oKXtyZXR1cm4gdGhpcy5tYXAoZnVuY3Rpb24oKXtyZXR1cm4gby5jYWxsKHRoaXMuY2hpbGROb2Rlcyl9KX0sc2libGluZ3M6ZnVuY3Rpb24odCl7cmV0dXJuIFUodGhpcy5tYXAoZnVuY3Rpb24odCxlKXtyZXR1cm4gcy5jYWxsKFYoZS5wYXJlbnROb2RlKSxmdW5jdGlvbih0KXtyZXR1cm4gdCE9PWV9KX0pLHQpfSxlbXB0eTpmdW5jdGlvbigpe3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24oKXt0aGlzLmlubmVySFRNTD1cIlwifSl9LHBsdWNrOmZ1bmN0aW9uKHQpe3JldHVybiBuLm1hcCh0aGlzLGZ1bmN0aW9uKGUpe3JldHVybiBlW3RdfSl9LHNob3c6ZnVuY3Rpb24oKXtyZXR1cm4gdGhpcy5lYWNoKGZ1bmN0aW9uKCl7XCJub25lXCI9PXRoaXMuc3R5bGUuZGlzcGxheSYmKHRoaXMuc3R5bGUuZGlzcGxheT1cIlwiKSxcIm5vbmVcIj09Z2V0Q29tcHV0ZWRTdHlsZSh0aGlzLFwiXCIpLmdldFByb3BlcnR5VmFsdWUoXCJkaXNwbGF5XCIpJiYodGhpcy5zdHlsZS5kaXNwbGF5PUkodGhpcy5ub2RlTmFtZSkpfSl9LHJlcGxhY2VXaXRoOmZ1bmN0aW9uKHQpe3JldHVybiB0aGlzLmJlZm9yZSh0KS5yZW1vdmUoKX0sd3JhcDpmdW5jdGlvbih0KXt2YXIgZT1aKHQpO2lmKHRoaXNbMF0mJiFlKXZhciBpPW4odCkuZ2V0KDApLHI9aS5wYXJlbnROb2RlfHx0aGlzLmxlbmd0aD4xO3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24obyl7bih0aGlzKS53cmFwQWxsKGU/dC5jYWxsKHRoaXMsbyk6cj9pLmNsb25lTm9kZSghMCk6aSl9KX0sd3JhcEFsbDpmdW5jdGlvbih0KXtpZih0aGlzWzBdKXtuKHRoaXNbMF0pLmJlZm9yZSh0PW4odCkpO2Zvcih2YXIgZTsoZT10LmNoaWxkcmVuKCkpLmxlbmd0aDspdD1lLmZpcnN0KCk7bih0KS5hcHBlbmQodGhpcyl9cmV0dXJuIHRoaXN9LHdyYXBJbm5lcjpmdW5jdGlvbih0KXt2YXIgZT1aKHQpO3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24oaSl7dmFyIHI9bih0aGlzKSxvPXIuY29udGVudHMoKSxzPWU/dC5jYWxsKHRoaXMsaSk6dDtvLmxlbmd0aD9vLndyYXBBbGwocyk6ci5hcHBlbmQocyl9KX0sdW53cmFwOmZ1bmN0aW9uKCl7cmV0dXJuIHRoaXMucGFyZW50KCkuZWFjaChmdW5jdGlvbigpe24odGhpcykucmVwbGFjZVdpdGgobih0aGlzKS5jaGlsZHJlbigpKX0pLHRoaXN9LGNsb25lOmZ1bmN0aW9uKCl7cmV0dXJuIHRoaXMubWFwKGZ1bmN0aW9uKCl7cmV0dXJuIHRoaXMuY2xvbmVOb2RlKCEwKX0pfSxoaWRlOmZ1bmN0aW9uKCl7cmV0dXJuIHRoaXMuY3NzKFwiZGlzcGxheVwiLFwibm9uZVwiKX0sdG9nZ2xlOmZ1bmN0aW9uKGUpe3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24oKXt2YXIgaT1uKHRoaXMpOyhlPT09dD9cIm5vbmVcIj09aS5jc3MoXCJkaXNwbGF5XCIpOmUpP2kuc2hvdygpOmkuaGlkZSgpfSl9LHByZXY6ZnVuY3Rpb24odCl7cmV0dXJuIG4odGhpcy5wbHVjayhcInByZXZpb3VzRWxlbWVudFNpYmxpbmdcIikpLmZpbHRlcih0fHxcIipcIil9LG5leHQ6ZnVuY3Rpb24odCl7cmV0dXJuIG4odGhpcy5wbHVjayhcIm5leHRFbGVtZW50U2libGluZ1wiKSkuZmlsdGVyKHR8fFwiKlwiKX0saHRtbDpmdW5jdGlvbih0KXtyZXR1cm4gMCBpbiBhcmd1bWVudHM/dGhpcy5lYWNoKGZ1bmN0aW9uKGUpe3ZhciBpPXRoaXMuaW5uZXJIVE1MO24odGhpcykuZW1wdHkoKS5hcHBlbmQoSih0aGlzLHQsZSxpKSl9KTowIGluIHRoaXM/dGhpc1swXS5pbm5lckhUTUw6bnVsbH0sdGV4dDpmdW5jdGlvbih0KXtyZXR1cm4gMCBpbiBhcmd1bWVudHM/dGhpcy5lYWNoKGZ1bmN0aW9uKGUpe3ZhciBuPUoodGhpcyx0LGUsdGhpcy50ZXh0Q29udGVudCk7dGhpcy50ZXh0Q29udGVudD1udWxsPT1uP1wiXCI6XCJcIitufSk6MCBpbiB0aGlzP3RoaXNbMF0udGV4dENvbnRlbnQ6bnVsbH0sYXR0cjpmdW5jdGlvbihuLGkpe3ZhciByO3JldHVyblwic3RyaW5nXCIhPXR5cGVvZiBufHwxIGluIGFyZ3VtZW50cz90aGlzLmVhY2goZnVuY3Rpb24odCl7aWYoMT09PXRoaXMubm9kZVR5cGUpaWYoRChuKSlmb3IoZSBpbiBuKVgodGhpcyxlLG5bZV0pO2Vsc2UgWCh0aGlzLG4sSih0aGlzLGksdCx0aGlzLmdldEF0dHJpYnV0ZShuKSkpfSk6dGhpcy5sZW5ndGgmJjE9PT10aGlzWzBdLm5vZGVUeXBlPyEocj10aGlzWzBdLmdldEF0dHJpYnV0ZShuKSkmJm4gaW4gdGhpc1swXT90aGlzWzBdW25dOnI6dH0scmVtb3ZlQXR0cjpmdW5jdGlvbih0KXtyZXR1cm4gdGhpcy5lYWNoKGZ1bmN0aW9uKCl7MT09PXRoaXMubm9kZVR5cGUmJnQuc3BsaXQoXCIgXCIpLmZvckVhY2goZnVuY3Rpb24odCl7WCh0aGlzLHQpfSx0aGlzKX0pfSxwcm9wOmZ1bmN0aW9uKHQsZSl7cmV0dXJuIHQ9UFt0XXx8dCwxIGluIGFyZ3VtZW50cz90aGlzLmVhY2goZnVuY3Rpb24obil7dGhpc1t0XT1KKHRoaXMsZSxuLHRoaXNbdF0pfSk6dGhpc1swXSYmdGhpc1swXVt0XX0sZGF0YTpmdW5jdGlvbihlLG4pe3ZhciBpPVwiZGF0YS1cIitlLnJlcGxhY2UobSxcIi0kMVwiKS50b0xvd2VyQ2FzZSgpLHI9MSBpbiBhcmd1bWVudHM/dGhpcy5hdHRyKGksbik6dGhpcy5hdHRyKGkpO3JldHVybiBudWxsIT09cj9ZKHIpOnR9LHZhbDpmdW5jdGlvbih0KXtyZXR1cm4gMCBpbiBhcmd1bWVudHM/dGhpcy5lYWNoKGZ1bmN0aW9uKGUpe3RoaXMudmFsdWU9Sih0aGlzLHQsZSx0aGlzLnZhbHVlKX0pOnRoaXNbMF0mJih0aGlzWzBdLm11bHRpcGxlP24odGhpc1swXSkuZmluZChcIm9wdGlvblwiKS5maWx0ZXIoZnVuY3Rpb24oKXtyZXR1cm4gdGhpcy5zZWxlY3RlZH0pLnBsdWNrKFwidmFsdWVcIik6dGhpc1swXS52YWx1ZSl9LG9mZnNldDpmdW5jdGlvbih0KXtpZih0KXJldHVybiB0aGlzLmVhY2goZnVuY3Rpb24oZSl7dmFyIGk9bih0aGlzKSxyPUoodGhpcyx0LGUsaS5vZmZzZXQoKSksbz1pLm9mZnNldFBhcmVudCgpLm9mZnNldCgpLHM9e3RvcDpyLnRvcC1vLnRvcCxsZWZ0OnIubGVmdC1vLmxlZnR9O1wic3RhdGljXCI9PWkuY3NzKFwicG9zaXRpb25cIikmJihzLnBvc2l0aW9uPVwicmVsYXRpdmVcIiksaS5jc3Mocyl9KTtpZighdGhpcy5sZW5ndGgpcmV0dXJuIG51bGw7dmFyIGU9dGhpc1swXS5nZXRCb3VuZGluZ0NsaWVudFJlY3QoKTtyZXR1cm57bGVmdDplLmxlZnQrd2luZG93LnBhZ2VYT2Zmc2V0LHRvcDplLnRvcCt3aW5kb3cucGFnZVlPZmZzZXQsd2lkdGg6TWF0aC5yb3VuZChlLndpZHRoKSxoZWlnaHQ6TWF0aC5yb3VuZChlLmhlaWdodCl9fSxjc3M6ZnVuY3Rpb24odCxpKXtpZihhcmd1bWVudHMubGVuZ3RoPDIpe3ZhciByLG89dGhpc1swXTtpZighbylyZXR1cm47aWYocj1nZXRDb21wdXRlZFN0eWxlKG8sXCJcIiksXCJzdHJpbmdcIj09dHlwZW9mIHQpcmV0dXJuIG8uc3R5bGVbQyh0KV18fHIuZ2V0UHJvcGVydHlWYWx1ZSh0KTtpZihBKHQpKXt2YXIgcz17fTtyZXR1cm4gbi5lYWNoKHQsZnVuY3Rpb24odCxlKXtzW2VdPW8uc3R5bGVbQyhlKV18fHIuZ2V0UHJvcGVydHlWYWx1ZShlKX0pLHN9fXZhciBhPVwiXCI7aWYoXCJzdHJpbmdcIj09TCh0KSlpfHwwPT09aT9hPUYodCkrXCI6XCIrSCh0LGkpOnRoaXMuZWFjaChmdW5jdGlvbigpe3RoaXMuc3R5bGUucmVtb3ZlUHJvcGVydHkoRih0KSl9KTtlbHNlIGZvcihlIGluIHQpdFtlXXx8MD09PXRbZV0/YSs9RihlKStcIjpcIitIKGUsdFtlXSkrXCI7XCI6dGhpcy5lYWNoKGZ1bmN0aW9uKCl7dGhpcy5zdHlsZS5yZW1vdmVQcm9wZXJ0eShGKGUpKX0pO3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24oKXt0aGlzLnN0eWxlLmNzc1RleHQrPVwiO1wiK2F9KX0saW5kZXg6ZnVuY3Rpb24odCl7cmV0dXJuIHQ/dGhpcy5pbmRleE9mKG4odClbMF0pOnRoaXMucGFyZW50KCkuY2hpbGRyZW4oKS5pbmRleE9mKHRoaXNbMF0pfSxoYXNDbGFzczpmdW5jdGlvbih0KXtyZXR1cm4gdD9yLnNvbWUuY2FsbCh0aGlzLGZ1bmN0aW9uKHQpe3JldHVybiB0aGlzLnRlc3QoVyh0KSl9LHEodCkpOiExfSxhZGRDbGFzczpmdW5jdGlvbih0KXtyZXR1cm4gdD90aGlzLmVhY2goZnVuY3Rpb24oZSl7aWYoXCJjbGFzc05hbWVcImluIHRoaXMpe2k9W107dmFyIHI9Vyh0aGlzKSxvPUoodGhpcyx0LGUscik7by5zcGxpdCgvXFxzKy9nKS5mb3JFYWNoKGZ1bmN0aW9uKHQpe24odGhpcykuaGFzQ2xhc3ModCl8fGkucHVzaCh0KX0sdGhpcyksaS5sZW5ndGgmJlcodGhpcyxyKyhyP1wiIFwiOlwiXCIpK2kuam9pbihcIiBcIikpfX0pOnRoaXN9LHJlbW92ZUNsYXNzOmZ1bmN0aW9uKGUpe3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24obil7aWYoXCJjbGFzc05hbWVcImluIHRoaXMpe2lmKGU9PT10KXJldHVybiBXKHRoaXMsXCJcIik7aT1XKHRoaXMpLEoodGhpcyxlLG4saSkuc3BsaXQoL1xccysvZykuZm9yRWFjaChmdW5jdGlvbih0KXtpPWkucmVwbGFjZShxKHQpLFwiIFwiKX0pLFcodGhpcyxpLnRyaW0oKSl9fSl9LHRvZ2dsZUNsYXNzOmZ1bmN0aW9uKGUsaSl7cmV0dXJuIGU/dGhpcy5lYWNoKGZ1bmN0aW9uKHIpe3ZhciBvPW4odGhpcykscz1KKHRoaXMsZSxyLFcodGhpcykpO3Muc3BsaXQoL1xccysvZykuZm9yRWFjaChmdW5jdGlvbihlKXsoaT09PXQ/IW8uaGFzQ2xhc3MoZSk6aSk/by5hZGRDbGFzcyhlKTpvLnJlbW92ZUNsYXNzKGUpfSl9KTp0aGlzfSxzY3JvbGxUb3A6ZnVuY3Rpb24oZSl7aWYodGhpcy5sZW5ndGgpe3ZhciBuPVwic2Nyb2xsVG9wXCJpbiB0aGlzWzBdO3JldHVybiBlPT09dD9uP3RoaXNbMF0uc2Nyb2xsVG9wOnRoaXNbMF0ucGFnZVlPZmZzZXQ6dGhpcy5lYWNoKG4/ZnVuY3Rpb24oKXt0aGlzLnNjcm9sbFRvcD1lfTpmdW5jdGlvbigpe3RoaXMuc2Nyb2xsVG8odGhpcy5zY3JvbGxYLGUpfSl9fSxzY3JvbGxMZWZ0OmZ1bmN0aW9uKGUpe2lmKHRoaXMubGVuZ3RoKXt2YXIgbj1cInNjcm9sbExlZnRcImluIHRoaXNbMF07cmV0dXJuIGU9PT10P24/dGhpc1swXS5zY3JvbGxMZWZ0OnRoaXNbMF0ucGFnZVhPZmZzZXQ6dGhpcy5lYWNoKG4/ZnVuY3Rpb24oKXt0aGlzLnNjcm9sbExlZnQ9ZX06ZnVuY3Rpb24oKXt0aGlzLnNjcm9sbFRvKGUsdGhpcy5zY3JvbGxZKX0pfX0scG9zaXRpb246ZnVuY3Rpb24oKXtpZih0aGlzLmxlbmd0aCl7dmFyIHQ9dGhpc1swXSxlPXRoaXMub2Zmc2V0UGFyZW50KCksaT10aGlzLm9mZnNldCgpLHI9ZC50ZXN0KGVbMF0ubm9kZU5hbWUpP3t0b3A6MCxsZWZ0OjB9OmUub2Zmc2V0KCk7cmV0dXJuIGkudG9wLT1wYXJzZUZsb2F0KG4odCkuY3NzKFwibWFyZ2luLXRvcFwiKSl8fDAsaS5sZWZ0LT1wYXJzZUZsb2F0KG4odCkuY3NzKFwibWFyZ2luLWxlZnRcIikpfHwwLHIudG9wKz1wYXJzZUZsb2F0KG4oZVswXSkuY3NzKFwiYm9yZGVyLXRvcC13aWR0aFwiKSl8fDAsci5sZWZ0Kz1wYXJzZUZsb2F0KG4oZVswXSkuY3NzKFwiYm9yZGVyLWxlZnQtd2lkdGhcIikpfHwwLHt0b3A6aS50b3Atci50b3AsbGVmdDppLmxlZnQtci5sZWZ0fX19LG9mZnNldFBhcmVudDpmdW5jdGlvbigpe3JldHVybiB0aGlzLm1hcChmdW5jdGlvbigpe2Zvcih2YXIgdD10aGlzLm9mZnNldFBhcmVudHx8YS5ib2R5O3QmJiFkLnRlc3QodC5ub2RlTmFtZSkmJlwic3RhdGljXCI9PW4odCkuY3NzKFwicG9zaXRpb25cIik7KXQ9dC5vZmZzZXRQYXJlbnQ7cmV0dXJuIHR9KX19LG4uZm4uZGV0YWNoPW4uZm4ucmVtb3ZlLFtcIndpZHRoXCIsXCJoZWlnaHRcIl0uZm9yRWFjaChmdW5jdGlvbihlKXt2YXIgaT1lLnJlcGxhY2UoLy4vLGZ1bmN0aW9uKHQpe3JldHVybiB0WzBdLnRvVXBwZXJDYXNlKCl9KTtuLmZuW2VdPWZ1bmN0aW9uKHIpe3ZhciBvLHM9dGhpc1swXTtyZXR1cm4gcj09PXQ/XyhzKT9zW1wiaW5uZXJcIitpXTokKHMpP3MuZG9jdW1lbnRFbGVtZW50W1wic2Nyb2xsXCIraV06KG89dGhpcy5vZmZzZXQoKSkmJm9bZV06dGhpcy5lYWNoKGZ1bmN0aW9uKHQpe3M9bih0aGlzKSxzLmNzcyhlLEoodGhpcyxyLHQsc1tlXSgpKSl9KX19KSx2LmZvckVhY2goZnVuY3Rpb24odCxlKXt2YXIgaT1lJTI7bi5mblt0XT1mdW5jdGlvbigpe3ZhciB0LG8scj1uLm1hcChhcmd1bWVudHMsZnVuY3Rpb24oZSl7cmV0dXJuIHQ9TChlKSxcIm9iamVjdFwiPT10fHxcImFycmF5XCI9PXR8fG51bGw9PWU/ZTpULmZyYWdtZW50KGUpfSkscz10aGlzLmxlbmd0aD4xO3JldHVybiByLmxlbmd0aDwxP3RoaXM6dGhpcy5lYWNoKGZ1bmN0aW9uKHQsdSl7bz1pP3U6dS5wYXJlbnROb2RlLHU9MD09ZT91Lm5leHRTaWJsaW5nOjE9PWU/dS5maXJzdENoaWxkOjI9PWU/dTpudWxsO3ZhciBmPW4uY29udGFpbnMoYS5kb2N1bWVudEVsZW1lbnQsbyk7ci5mb3JFYWNoKGZ1bmN0aW9uKHQpe2lmKHMpdD10LmNsb25lTm9kZSghMCk7ZWxzZSBpZighbylyZXR1cm4gbih0KS5yZW1vdmUoKTtvLmluc2VydEJlZm9yZSh0LHUpLGYmJkcodCxmdW5jdGlvbih0KXtudWxsPT10Lm5vZGVOYW1lfHxcIlNDUklQVFwiIT09dC5ub2RlTmFtZS50b1VwcGVyQ2FzZSgpfHx0LnR5cGUmJlwidGV4dC9qYXZhc2NyaXB0XCIhPT10LnR5cGV8fHQuc3JjfHx3aW5kb3cuZXZhbC5jYWxsKHdpbmRvdyx0LmlubmVySFRNTCl9KX0pfSl9LG4uZm5baT90K1wiVG9cIjpcImluc2VydFwiKyhlP1wiQmVmb3JlXCI6XCJBZnRlclwiKV09ZnVuY3Rpb24oZSl7cmV0dXJuIG4oZSlbdF0odGhpcyksdGhpc319KSxULloucHJvdG90eXBlPW4uZm4sVC51bmlxPU4sVC5kZXNlcmlhbGl6ZVZhbHVlPVksbi56ZXB0bz1ULG59KCk7d2luZG93LlplcHRvPVplcHRvLHZvaWQgMD09PXdpbmRvdy4kJiYod2luZG93LiQ9WmVwdG8pLGZ1bmN0aW9uKHQpe2Z1bmN0aW9uIGwodCl7cmV0dXJuIHQuX3ppZHx8KHQuX3ppZD1lKyspfWZ1bmN0aW9uIGgodCxlLG4saSl7aWYoZT1wKGUpLGUubnMpdmFyIHI9ZChlLm5zKTtyZXR1cm4oc1tsKHQpXXx8W10pLmZpbHRlcihmdW5jdGlvbih0KXtyZXR1cm4hKCF0fHxlLmUmJnQuZSE9ZS5lfHxlLm5zJiYhci50ZXN0KHQubnMpfHxuJiZsKHQuZm4pIT09bChuKXx8aSYmdC5zZWwhPWkpfSl9ZnVuY3Rpb24gcCh0KXt2YXIgZT0oXCJcIit0KS5zcGxpdChcIi5cIik7cmV0dXJue2U6ZVswXSxuczplLnNsaWNlKDEpLnNvcnQoKS5qb2luKFwiIFwiKX19ZnVuY3Rpb24gZCh0KXtyZXR1cm4gbmV3IFJlZ0V4cChcIig/Ol58IClcIit0LnJlcGxhY2UoXCIgXCIsXCIgLiogP1wiKStcIig/OiB8JClcIil9ZnVuY3Rpb24gbSh0LGUpe3JldHVybiB0LmRlbCYmIXUmJnQuZSBpbiBmfHwhIWV9ZnVuY3Rpb24gZyh0KXtyZXR1cm4gY1t0XXx8dSYmZlt0XXx8dH1mdW5jdGlvbiB2KGUsaSxyLG8sYSx1LGYpe3ZhciBoPWwoZSksZD1zW2hdfHwoc1toXT1bXSk7aS5zcGxpdCgvXFxzLykuZm9yRWFjaChmdW5jdGlvbihpKXtpZihcInJlYWR5XCI9PWkpcmV0dXJuIHQoZG9jdW1lbnQpLnJlYWR5KHIpO3ZhciBzPXAoaSk7cy5mbj1yLHMuc2VsPWEscy5lIGluIGMmJihyPWZ1bmN0aW9uKGUpe3ZhciBuPWUucmVsYXRlZFRhcmdldDtyZXR1cm4hbnx8biE9PXRoaXMmJiF0LmNvbnRhaW5zKHRoaXMsbik/cy5mbi5hcHBseSh0aGlzLGFyZ3VtZW50cyk6dm9pZCAwfSkscy5kZWw9dTt2YXIgbD11fHxyO3MucHJveHk9ZnVuY3Rpb24odCl7aWYodD1qKHQpLCF0LmlzSW1tZWRpYXRlUHJvcGFnYXRpb25TdG9wcGVkKCkpe3QuZGF0YT1vO3ZhciBpPWwuYXBwbHkoZSx0Ll9hcmdzPT1uP1t0XTpbdF0uY29uY2F0KHQuX2FyZ3MpKTtyZXR1cm4gaT09PSExJiYodC5wcmV2ZW50RGVmYXVsdCgpLHQuc3RvcFByb3BhZ2F0aW9uKCkpLGl9fSxzLmk9ZC5sZW5ndGgsZC5wdXNoKHMpLFwiYWRkRXZlbnRMaXN0ZW5lclwiaW4gZSYmZS5hZGRFdmVudExpc3RlbmVyKGcocy5lKSxzLnByb3h5LG0ocyxmKSl9KX1mdW5jdGlvbiB5KHQsZSxuLGkscil7dmFyIG89bCh0KTsoZXx8XCJcIikuc3BsaXQoL1xccy8pLmZvckVhY2goZnVuY3Rpb24oZSl7aCh0LGUsbixpKS5mb3JFYWNoKGZ1bmN0aW9uKGUpe2RlbGV0ZSBzW29dW2UuaV0sXCJyZW1vdmVFdmVudExpc3RlbmVyXCJpbiB0JiZ0LnJlbW92ZUV2ZW50TGlzdGVuZXIoZyhlLmUpLGUucHJveHksbShlLHIpKX0pfSl9ZnVuY3Rpb24gaihlLGkpe3JldHVybihpfHwhZS5pc0RlZmF1bHRQcmV2ZW50ZWQpJiYoaXx8KGk9ZSksdC5lYWNoKEUsZnVuY3Rpb24odCxuKXt2YXIgcj1pW3RdO2VbdF09ZnVuY3Rpb24oKXtyZXR1cm4gdGhpc1tuXT14LHImJnIuYXBwbHkoaSxhcmd1bWVudHMpfSxlW25dPWJ9KSwoaS5kZWZhdWx0UHJldmVudGVkIT09bj9pLmRlZmF1bHRQcmV2ZW50ZWQ6XCJyZXR1cm5WYWx1ZVwiaW4gaT9pLnJldHVyblZhbHVlPT09ITE6aS5nZXRQcmV2ZW50RGVmYXVsdCYmaS5nZXRQcmV2ZW50RGVmYXVsdCgpKSYmKGUuaXNEZWZhdWx0UHJldmVudGVkPXgpKSxlfWZ1bmN0aW9uIFModCl7dmFyIGUsaT17b3JpZ2luYWxFdmVudDp0fTtmb3IoZSBpbiB0KXcudGVzdChlKXx8dFtlXT09PW58fChpW2VdPXRbZV0pO3JldHVybiBqKGksdCl9dmFyIG4sZT0xLGk9QXJyYXkucHJvdG90eXBlLnNsaWNlLHI9dC5pc0Z1bmN0aW9uLG89ZnVuY3Rpb24odCl7cmV0dXJuXCJzdHJpbmdcIj09dHlwZW9mIHR9LHM9e30sYT17fSx1PVwib25mb2N1c2luXCJpbiB3aW5kb3csZj17Zm9jdXM6XCJmb2N1c2luXCIsYmx1cjpcImZvY3Vzb3V0XCJ9LGM9e21vdXNlZW50ZXI6XCJtb3VzZW92ZXJcIixtb3VzZWxlYXZlOlwibW91c2VvdXRcIn07YS5jbGljaz1hLm1vdXNlZG93bj1hLm1vdXNldXA9YS5tb3VzZW1vdmU9XCJNb3VzZUV2ZW50c1wiLHQuZXZlbnQ9e2FkZDp2LHJlbW92ZTp5fSx0LnByb3h5PWZ1bmN0aW9uKGUsbil7dmFyIHM9MiBpbiBhcmd1bWVudHMmJmkuY2FsbChhcmd1bWVudHMsMik7aWYocihlKSl7dmFyIGE9ZnVuY3Rpb24oKXtyZXR1cm4gZS5hcHBseShuLHM/cy5jb25jYXQoaS5jYWxsKGFyZ3VtZW50cykpOmFyZ3VtZW50cyl9O3JldHVybiBhLl96aWQ9bChlKSxhfWlmKG8obikpcmV0dXJuIHM/KHMudW5zaGlmdChlW25dLGUpLHQucHJveHkuYXBwbHkobnVsbCxzKSk6dC5wcm94eShlW25dLGUpO3Rocm93IG5ldyBUeXBlRXJyb3IoXCJleHBlY3RlZCBmdW5jdGlvblwiKX0sdC5mbi5iaW5kPWZ1bmN0aW9uKHQsZSxuKXtyZXR1cm4gdGhpcy5vbih0LGUsbil9LHQuZm4udW5iaW5kPWZ1bmN0aW9uKHQsZSl7cmV0dXJuIHRoaXMub2ZmKHQsZSl9LHQuZm4ub25lPWZ1bmN0aW9uKHQsZSxuLGkpe3JldHVybiB0aGlzLm9uKHQsZSxuLGksMSl9O3ZhciB4PWZ1bmN0aW9uKCl7cmV0dXJuITB9LGI9ZnVuY3Rpb24oKXtyZXR1cm4hMX0sdz0vXihbQS1aXXxyZXR1cm5WYWx1ZSR8bGF5ZXJbWFldJCkvLEU9e3ByZXZlbnREZWZhdWx0OlwiaXNEZWZhdWx0UHJldmVudGVkXCIsc3RvcEltbWVkaWF0ZVByb3BhZ2F0aW9uOlwiaXNJbW1lZGlhdGVQcm9wYWdhdGlvblN0b3BwZWRcIixzdG9wUHJvcGFnYXRpb246XCJpc1Byb3BhZ2F0aW9uU3RvcHBlZFwifTt0LmZuLmRlbGVnYXRlPWZ1bmN0aW9uKHQsZSxuKXtyZXR1cm4gdGhpcy5vbihlLHQsbil9LHQuZm4udW5kZWxlZ2F0ZT1mdW5jdGlvbih0LGUsbil7cmV0dXJuIHRoaXMub2ZmKGUsdCxuKX0sdC5mbi5saXZlPWZ1bmN0aW9uKGUsbil7cmV0dXJuIHQoZG9jdW1lbnQuYm9keSkuZGVsZWdhdGUodGhpcy5zZWxlY3RvcixlLG4pLHRoaXN9LHQuZm4uZGllPWZ1bmN0aW9uKGUsbil7cmV0dXJuIHQoZG9jdW1lbnQuYm9keSkudW5kZWxlZ2F0ZSh0aGlzLnNlbGVjdG9yLGUsbiksdGhpc30sdC5mbi5vbj1mdW5jdGlvbihlLHMsYSx1LGYpe3ZhciBjLGwsaD10aGlzO3JldHVybiBlJiYhbyhlKT8odC5lYWNoKGUsZnVuY3Rpb24odCxlKXtoLm9uKHQscyxhLGUsZil9KSxoKToobyhzKXx8cih1KXx8dT09PSExfHwodT1hLGE9cyxzPW4pLChyKGEpfHxhPT09ITEpJiYodT1hLGE9biksdT09PSExJiYodT1iKSxoLmVhY2goZnVuY3Rpb24obixyKXtmJiYoYz1mdW5jdGlvbih0KXtyZXR1cm4geShyLHQudHlwZSx1KSx1LmFwcGx5KHRoaXMsYXJndW1lbnRzKX0pLHMmJihsPWZ1bmN0aW9uKGUpe3ZhciBuLG89dChlLnRhcmdldCkuY2xvc2VzdChzLHIpLmdldCgwKTtyZXR1cm4gbyYmbyE9PXI/KG49dC5leHRlbmQoUyhlKSx7Y3VycmVudFRhcmdldDpvLGxpdmVGaXJlZDpyfSksKGN8fHUpLmFwcGx5KG8sW25dLmNvbmNhdChpLmNhbGwoYXJndW1lbnRzLDEpKSkpOnZvaWQgMH0pLHYocixlLHUsYSxzLGx8fGMpfSkpfSx0LmZuLm9mZj1mdW5jdGlvbihlLGkscyl7dmFyIGE9dGhpcztyZXR1cm4gZSYmIW8oZSk/KHQuZWFjaChlLGZ1bmN0aW9uKHQsZSl7YS5vZmYodCxpLGUpfSksYSk6KG8oaSl8fHIocyl8fHM9PT0hMXx8KHM9aSxpPW4pLHM9PT0hMSYmKHM9YiksYS5lYWNoKGZ1bmN0aW9uKCl7eSh0aGlzLGUscyxpKX0pKX0sdC5mbi50cmlnZ2VyPWZ1bmN0aW9uKGUsbil7cmV0dXJuIGU9byhlKXx8dC5pc1BsYWluT2JqZWN0KGUpP3QuRXZlbnQoZSk6aihlKSxlLl9hcmdzPW4sdGhpcy5lYWNoKGZ1bmN0aW9uKCl7ZS50eXBlIGluIGYmJlwiZnVuY3Rpb25cIj09dHlwZW9mIHRoaXNbZS50eXBlXT90aGlzW2UudHlwZV0oKTpcImRpc3BhdGNoRXZlbnRcImluIHRoaXM/dGhpcy5kaXNwYXRjaEV2ZW50KGUpOnQodGhpcykudHJpZ2dlckhhbmRsZXIoZSxuKX0pfSx0LmZuLnRyaWdnZXJIYW5kbGVyPWZ1bmN0aW9uKGUsbil7dmFyIGkscjtyZXR1cm4gdGhpcy5lYWNoKGZ1bmN0aW9uKHMsYSl7aT1TKG8oZSk/dC5FdmVudChlKTplKSxpLl9hcmdzPW4saS50YXJnZXQ9YSx0LmVhY2goaChhLGUudHlwZXx8ZSksZnVuY3Rpb24odCxlKXtyZXR1cm4gcj1lLnByb3h5KGkpLGkuaXNJbW1lZGlhdGVQcm9wYWdhdGlvblN0b3BwZWQoKT8hMTp2b2lkIDB9KX0pLHJ9LFwiZm9jdXNpbiBmb2N1c291dCBmb2N1cyBibHVyIGxvYWQgcmVzaXplIHNjcm9sbCB1bmxvYWQgY2xpY2sgZGJsY2xpY2sgbW91c2Vkb3duIG1vdXNldXAgbW91c2Vtb3ZlIG1vdXNlb3ZlciBtb3VzZW91dCBtb3VzZWVudGVyIG1vdXNlbGVhdmUgY2hhbmdlIHNlbGVjdCBrZXlkb3duIGtleXByZXNzIGtleXVwIGVycm9yXCIuc3BsaXQoXCIgXCIpLmZvckVhY2goZnVuY3Rpb24oZSl7dC5mbltlXT1mdW5jdGlvbih0KXtyZXR1cm4gMCBpbiBhcmd1bWVudHM/dGhpcy5iaW5kKGUsdCk6dGhpcy50cmlnZ2VyKGUpfX0pLHQuRXZlbnQ9ZnVuY3Rpb24odCxlKXtvKHQpfHwoZT10LHQ9ZS50eXBlKTt2YXIgbj1kb2N1bWVudC5jcmVhdGVFdmVudChhW3RdfHxcIkV2ZW50c1wiKSxpPSEwO2lmKGUpZm9yKHZhciByIGluIGUpXCJidWJibGVzXCI9PXI/aT0hIWVbcl06bltyXT1lW3JdO3JldHVybiBuLmluaXRFdmVudCh0LGksITApLGoobil9fShaZXB0byksZnVuY3Rpb24odCl7ZnVuY3Rpb24gaChlLG4saSl7dmFyIHI9dC5FdmVudChuKTtyZXR1cm4gdChlKS50cmlnZ2VyKHIsaSksIXIuaXNEZWZhdWx0UHJldmVudGVkKCl9ZnVuY3Rpb24gcCh0LGUsaSxyKXtyZXR1cm4gdC5nbG9iYWw/aChlfHxuLGkscik6dm9pZCAwfWZ1bmN0aW9uIGQoZSl7ZS5nbG9iYWwmJjA9PT10LmFjdGl2ZSsrJiZwKGUsbnVsbCxcImFqYXhTdGFydFwiKX1mdW5jdGlvbiBtKGUpe2UuZ2xvYmFsJiYhLS10LmFjdGl2ZSYmcChlLG51bGwsXCJhamF4U3RvcFwiKX1mdW5jdGlvbiBnKHQsZSl7dmFyIG49ZS5jb250ZXh0O3JldHVybiBlLmJlZm9yZVNlbmQuY2FsbChuLHQsZSk9PT0hMXx8cChlLG4sXCJhamF4QmVmb3JlU2VuZFwiLFt0LGVdKT09PSExPyExOnZvaWQgcChlLG4sXCJhamF4U2VuZFwiLFt0LGVdKX1mdW5jdGlvbiB2KHQsZSxuLGkpe3ZhciByPW4uY29udGV4dCxvPVwic3VjY2Vzc1wiO24uc3VjY2Vzcy5jYWxsKHIsdCxvLGUpLGkmJmkucmVzb2x2ZVdpdGgocixbdCxvLGVdKSxwKG4scixcImFqYXhTdWNjZXNzXCIsW2Usbix0XSkseChvLGUsbil9ZnVuY3Rpb24geSh0LGUsbixpLHIpe3ZhciBvPWkuY29udGV4dDtpLmVycm9yLmNhbGwobyxuLGUsdCksciYmci5yZWplY3RXaXRoKG8sW24sZSx0XSkscChpLG8sXCJhamF4RXJyb3JcIixbbixpLHR8fGVdKSx4KGUsbixpKX1mdW5jdGlvbiB4KHQsZSxuKXt2YXIgaT1uLmNvbnRleHQ7bi5jb21wbGV0ZS5jYWxsKGksZSx0KSxwKG4saSxcImFqYXhDb21wbGV0ZVwiLFtlLG5dKSxtKG4pfWZ1bmN0aW9uIGIoKXt9ZnVuY3Rpb24gdyh0KXtyZXR1cm4gdCYmKHQ9dC5zcGxpdChcIjtcIiwyKVswXSksdCYmKHQ9PWY/XCJodG1sXCI6dD09dT9cImpzb25cIjpzLnRlc3QodCk/XCJzY3JpcHRcIjphLnRlc3QodCkmJlwieG1sXCIpfHxcInRleHRcIn1mdW5jdGlvbiBFKHQsZSl7cmV0dXJuXCJcIj09ZT90Oih0K1wiJlwiK2UpLnJlcGxhY2UoL1smP117MSwyfS8sXCI/XCIpfWZ1bmN0aW9uIGooZSl7ZS5wcm9jZXNzRGF0YSYmZS5kYXRhJiZcInN0cmluZ1wiIT10LnR5cGUoZS5kYXRhKSYmKGUuZGF0YT10LnBhcmFtKGUuZGF0YSxlLnRyYWRpdGlvbmFsKSksIWUuZGF0YXx8ZS50eXBlJiZcIkdFVFwiIT1lLnR5cGUudG9VcHBlckNhc2UoKXx8KGUudXJsPUUoZS51cmwsZS5kYXRhKSxlLmRhdGE9dm9pZCAwKX1mdW5jdGlvbiBTKGUsbixpLHIpe3JldHVybiB0LmlzRnVuY3Rpb24obikmJihyPWksaT1uLG49dm9pZCAwKSx0LmlzRnVuY3Rpb24oaSl8fChyPWksaT12b2lkIDApLHt1cmw6ZSxkYXRhOm4sc3VjY2VzczppLGRhdGFUeXBlOnJ9fWZ1bmN0aW9uIEMoZSxuLGkscil7dmFyIG8scz10LmlzQXJyYXkobiksYT10LmlzUGxhaW5PYmplY3Qobik7dC5lYWNoKG4sZnVuY3Rpb24obix1KXtvPXQudHlwZSh1KSxyJiYobj1pP3I6citcIltcIisoYXx8XCJvYmplY3RcIj09b3x8XCJhcnJheVwiPT1vP246XCJcIikrXCJdXCIpLCFyJiZzP2UuYWRkKHUubmFtZSx1LnZhbHVlKTpcImFycmF5XCI9PW98fCFpJiZcIm9iamVjdFwiPT1vP0MoZSx1LGksbik6ZS5hZGQobix1KX0pfXZhciBpLHIsZT0wLG49d2luZG93LmRvY3VtZW50LG89LzxzY3JpcHRcXGJbXjxdKig/Oig/ITxcXC9zY3JpcHQ+KTxbXjxdKikqPFxcL3NjcmlwdD4vZ2kscz0vXig/OnRleHR8YXBwbGljYXRpb24pXFwvamF2YXNjcmlwdC9pLGE9L14oPzp0ZXh0fGFwcGxpY2F0aW9uKVxcL3htbC9pLHU9XCJhcHBsaWNhdGlvbi9qc29uXCIsZj1cInRleHQvaHRtbFwiLGM9L15cXHMqJC8sbD1uLmNyZWF0ZUVsZW1lbnQoXCJhXCIpO2wuaHJlZj13aW5kb3cubG9jYXRpb24uaHJlZix0LmFjdGl2ZT0wLHQuYWpheEpTT05QPWZ1bmN0aW9uKGkscil7aWYoIShcInR5cGVcImluIGkpKXJldHVybiB0LmFqYXgoaSk7dmFyIGYsaCxvPWkuanNvbnBDYWxsYmFjayxzPSh0LmlzRnVuY3Rpb24obyk/bygpOm8pfHxcImpzb25wXCIrICsrZSxhPW4uY3JlYXRlRWxlbWVudChcInNjcmlwdFwiKSx1PXdpbmRvd1tzXSxjPWZ1bmN0aW9uKGUpe3QoYSkudHJpZ2dlckhhbmRsZXIoXCJlcnJvclwiLGV8fFwiYWJvcnRcIil9LGw9e2Fib3J0OmN9O3JldHVybiByJiZyLnByb21pc2UobCksdChhKS5vbihcImxvYWQgZXJyb3JcIixmdW5jdGlvbihlLG4pe2NsZWFyVGltZW91dChoKSx0KGEpLm9mZigpLnJlbW92ZSgpLFwiZXJyb3JcIiE9ZS50eXBlJiZmP3YoZlswXSxsLGkscik6eShudWxsLG58fFwiZXJyb3JcIixsLGksciksd2luZG93W3NdPXUsZiYmdC5pc0Z1bmN0aW9uKHUpJiZ1KGZbMF0pLHU9Zj12b2lkIDB9KSxnKGwsaSk9PT0hMT8oYyhcImFib3J0XCIpLGwpOih3aW5kb3dbc109ZnVuY3Rpb24oKXtmPWFyZ3VtZW50c30sYS5zcmM9aS51cmwucmVwbGFjZSgvXFw/KC4rKT1cXD8vLFwiPyQxPVwiK3MpLG4uaGVhZC5hcHBlbmRDaGlsZChhKSxpLnRpbWVvdXQ+MCYmKGg9c2V0VGltZW91dChmdW5jdGlvbigpe2MoXCJ0aW1lb3V0XCIpfSxpLnRpbWVvdXQpKSxsKX0sdC5hamF4U2V0dGluZ3M9e3R5cGU6XCJHRVRcIixiZWZvcmVTZW5kOmIsc3VjY2VzczpiLGVycm9yOmIsY29tcGxldGU6Yixjb250ZXh0Om51bGwsZ2xvYmFsOiEwLHhocjpmdW5jdGlvbigpe3JldHVybiBuZXcgd2luZG93LlhNTEh0dHBSZXF1ZXN0fSxhY2NlcHRzOntzY3JpcHQ6XCJ0ZXh0L2phdmFzY3JpcHQsIGFwcGxpY2F0aW9uL2phdmFzY3JpcHQsIGFwcGxpY2F0aW9uL3gtamF2YXNjcmlwdFwiLGpzb246dSx4bWw6XCJhcHBsaWNhdGlvbi94bWwsIHRleHQveG1sXCIsaHRtbDpmLHRleHQ6XCJ0ZXh0L3BsYWluXCJ9LGNyb3NzRG9tYWluOiExLHRpbWVvdXQ6MCxwcm9jZXNzRGF0YTohMCxjYWNoZTohMH0sdC5hamF4PWZ1bmN0aW9uKGUpe3ZhciBhLG89dC5leHRlbmQoe30sZXx8e30pLHM9dC5EZWZlcnJlZCYmdC5EZWZlcnJlZCgpO2ZvcihpIGluIHQuYWpheFNldHRpbmdzKXZvaWQgMD09PW9baV0mJihvW2ldPXQuYWpheFNldHRpbmdzW2ldKTtkKG8pLG8uY3Jvc3NEb21haW58fChhPW4uY3JlYXRlRWxlbWVudChcImFcIiksYS5ocmVmPW8udXJsLGEuaHJlZj1hLmhyZWYsby5jcm9zc0RvbWFpbj1sLnByb3RvY29sK1wiLy9cIitsLmhvc3QhPWEucHJvdG9jb2wrXCIvL1wiK2EuaG9zdCksby51cmx8fChvLnVybD13aW5kb3cubG9jYXRpb24udG9TdHJpbmcoKSksaihvKTt2YXIgdT1vLmRhdGFUeXBlLGY9L1xcPy4rPVxcPy8udGVzdChvLnVybCk7aWYoZiYmKHU9XCJqc29ucFwiKSxvLmNhY2hlIT09ITEmJihlJiZlLmNhY2hlPT09ITB8fFwic2NyaXB0XCIhPXUmJlwianNvbnBcIiE9dSl8fChvLnVybD1FKG8udXJsLFwiXz1cIitEYXRlLm5vdygpKSksXCJqc29ucFwiPT11KXJldHVybiBmfHwoby51cmw9RShvLnVybCxvLmpzb25wP28uanNvbnArXCI9P1wiOm8uanNvbnA9PT0hMT9cIlwiOlwiY2FsbGJhY2s9P1wiKSksdC5hamF4SlNPTlAobyxzKTt2YXIgQyxoPW8uYWNjZXB0c1t1XSxwPXt9LG09ZnVuY3Rpb24odCxlKXtwW3QudG9Mb3dlckNhc2UoKV09W3QsZV19LHg9L14oW1xcdy1dKzopXFwvXFwvLy50ZXN0KG8udXJsKT9SZWdFeHAuJDE6d2luZG93LmxvY2F0aW9uLnByb3RvY29sLFM9by54aHIoKSxUPVMuc2V0UmVxdWVzdEhlYWRlcjtpZihzJiZzLnByb21pc2UoUyksby5jcm9zc0RvbWFpbnx8bShcIlgtUmVxdWVzdGVkLVdpdGhcIixcIlhNTEh0dHBSZXF1ZXN0XCIpLG0oXCJBY2NlcHRcIixofHxcIiovKlwiKSwoaD1vLm1pbWVUeXBlfHxoKSYmKGguaW5kZXhPZihcIixcIik+LTEmJihoPWguc3BsaXQoXCIsXCIsMilbMF0pLFMub3ZlcnJpZGVNaW1lVHlwZSYmUy5vdmVycmlkZU1pbWVUeXBlKGgpKSwoby5jb250ZW50VHlwZXx8by5jb250ZW50VHlwZSE9PSExJiZvLmRhdGEmJlwiR0VUXCIhPW8udHlwZS50b1VwcGVyQ2FzZSgpKSYmbShcIkNvbnRlbnQtVHlwZVwiLG8uY29udGVudFR5cGV8fFwiYXBwbGljYXRpb24veC13d3ctZm9ybS11cmxlbmNvZGVkXCIpLG8uaGVhZGVycylmb3IociBpbiBvLmhlYWRlcnMpbShyLG8uaGVhZGVyc1tyXSk7aWYoUy5zZXRSZXF1ZXN0SGVhZGVyPW0sUy5vbnJlYWR5c3RhdGVjaGFuZ2U9ZnVuY3Rpb24oKXtpZig0PT1TLnJlYWR5U3RhdGUpe1Mub25yZWFkeXN0YXRlY2hhbmdlPWIsY2xlYXJUaW1lb3V0KEMpO3ZhciBlLG49ITE7aWYoUy5zdGF0dXM+PTIwMCYmUy5zdGF0dXM8MzAwfHwzMDQ9PVMuc3RhdHVzfHwwPT1TLnN0YXR1cyYmXCJmaWxlOlwiPT14KXt1PXV8fHcoby5taW1lVHlwZXx8Uy5nZXRSZXNwb25zZUhlYWRlcihcImNvbnRlbnQtdHlwZVwiKSksZT1TLnJlc3BvbnNlVGV4dDt0cnl7XCJzY3JpcHRcIj09dT8oMSxldmFsKShlKTpcInhtbFwiPT11P2U9Uy5yZXNwb25zZVhNTDpcImpzb25cIj09dSYmKGU9Yy50ZXN0KGUpP251bGw6dC5wYXJzZUpTT04oZSkpfWNhdGNoKGkpe249aX1uP3kobixcInBhcnNlcmVycm9yXCIsUyxvLHMpOnYoZSxTLG8scyl9ZWxzZSB5KFMuc3RhdHVzVGV4dHx8bnVsbCxTLnN0YXR1cz9cImVycm9yXCI6XCJhYm9ydFwiLFMsbyxzKX19LGcoUyxvKT09PSExKXJldHVybiBTLmFib3J0KCkseShudWxsLFwiYWJvcnRcIixTLG8scyksUztpZihvLnhockZpZWxkcylmb3IociBpbiBvLnhockZpZWxkcylTW3JdPW8ueGhyRmllbGRzW3JdO3ZhciBOPVwiYXN5bmNcImluIG8/by5hc3luYzohMDtTLm9wZW4oby50eXBlLG8udXJsLE4sby51c2VybmFtZSxvLnBhc3N3b3JkKTtmb3IociBpbiBwKVQuYXBwbHkoUyxwW3JdKTtyZXR1cm4gby50aW1lb3V0PjAmJihDPXNldFRpbWVvdXQoZnVuY3Rpb24oKXtTLm9ucmVhZHlzdGF0ZWNoYW5nZT1iLFMuYWJvcnQoKSx5KG51bGwsXCJ0aW1lb3V0XCIsUyxvLHMpfSxvLnRpbWVvdXQpKSxTLnNlbmQoby5kYXRhP28uZGF0YTpudWxsKSxTfSx0LmdldD1mdW5jdGlvbigpe3JldHVybiB0LmFqYXgoUy5hcHBseShudWxsLGFyZ3VtZW50cykpfSx0LnBvc3Q9ZnVuY3Rpb24oKXt2YXIgZT1TLmFwcGx5KG51bGwsYXJndW1lbnRzKTtyZXR1cm4gZS50eXBlPVwiUE9TVFwiLHQuYWpheChlKX0sdC5nZXRKU09OPWZ1bmN0aW9uKCl7dmFyIGU9Uy5hcHBseShudWxsLGFyZ3VtZW50cyk7cmV0dXJuIGUuZGF0YVR5cGU9XCJqc29uXCIsdC5hamF4KGUpfSx0LmZuLmxvYWQ9ZnVuY3Rpb24oZSxuLGkpe2lmKCF0aGlzLmxlbmd0aClyZXR1cm4gdGhpczt2YXIgYSxyPXRoaXMscz1lLnNwbGl0KC9cXHMvKSx1PVMoZSxuLGkpLGY9dS5zdWNjZXNzO3JldHVybiBzLmxlbmd0aD4xJiYodS51cmw9c1swXSxhPXNbMV0pLHUuc3VjY2Vzcz1mdW5jdGlvbihlKXtyLmh0bWwoYT90KFwiPGRpdj5cIikuaHRtbChlLnJlcGxhY2UobyxcIlwiKSkuZmluZChhKTplKSxmJiZmLmFwcGx5KHIsYXJndW1lbnRzKX0sdC5hamF4KHUpLHRoaXN9O3ZhciBUPWVuY29kZVVSSUNvbXBvbmVudDt0LnBhcmFtPWZ1bmN0aW9uKGUsbil7dmFyIGk9W107cmV0dXJuIGkuYWRkPWZ1bmN0aW9uKGUsbil7dC5pc0Z1bmN0aW9uKG4pJiYobj1uKCkpLG51bGw9PW4mJihuPVwiXCIpLHRoaXMucHVzaChUKGUpK1wiPVwiK1QobikpfSxDKGksZSxuKSxpLmpvaW4oXCImXCIpLnJlcGxhY2UoLyUyMC9nLFwiK1wiKX19KFplcHRvKSxmdW5jdGlvbih0KXt0LmZuLnNlcmlhbGl6ZUFycmF5PWZ1bmN0aW9uKCl7dmFyIGUsbixpPVtdLHI9ZnVuY3Rpb24odCl7cmV0dXJuIHQuZm9yRWFjaD90LmZvckVhY2gocik6dm9pZCBpLnB1c2goe25hbWU6ZSx2YWx1ZTp0fSl9O3JldHVybiB0aGlzWzBdJiZ0LmVhY2godGhpc1swXS5lbGVtZW50cyxmdW5jdGlvbihpLG8pe249by50eXBlLGU9by5uYW1lLGUmJlwiZmllbGRzZXRcIiE9by5ub2RlTmFtZS50b0xvd2VyQ2FzZSgpJiYhby5kaXNhYmxlZCYmXCJzdWJtaXRcIiE9biYmXCJyZXNldFwiIT1uJiZcImJ1dHRvblwiIT1uJiZcImZpbGVcIiE9biYmKFwicmFkaW9cIiE9biYmXCJjaGVja2JveFwiIT1ufHxvLmNoZWNrZWQpJiZyKHQobykudmFsKCkpfSksaX0sdC5mbi5zZXJpYWxpemU9ZnVuY3Rpb24oKXt2YXIgdD1bXTtyZXR1cm4gdGhpcy5zZXJpYWxpemVBcnJheSgpLmZvckVhY2goZnVuY3Rpb24oZSl7dC5wdXNoKGVuY29kZVVSSUNvbXBvbmVudChlLm5hbWUpK1wiPVwiK2VuY29kZVVSSUNvbXBvbmVudChlLnZhbHVlKSl9KSx0LmpvaW4oXCImXCIpfSx0LmZuLnN1Ym1pdD1mdW5jdGlvbihlKXtpZigwIGluIGFyZ3VtZW50cyl0aGlzLmJpbmQoXCJzdWJtaXRcIixlKTtlbHNlIGlmKHRoaXMubGVuZ3RoKXt2YXIgbj10LkV2ZW50KFwic3VibWl0XCIpO3RoaXMuZXEoMCkudHJpZ2dlcihuKSxuLmlzRGVmYXVsdFByZXZlbnRlZCgpfHx0aGlzLmdldCgwKS5zdWJtaXQoKX1yZXR1cm4gdGhpc319KFplcHRvKSxmdW5jdGlvbih0KXtcIl9fcHJvdG9fX1wiaW57fXx8dC5leHRlbmQodC56ZXB0byx7WjpmdW5jdGlvbihlLG4pe3JldHVybiBlPWV8fFtdLHQuZXh0ZW5kKGUsdC5mbiksZS5zZWxlY3Rvcj1ufHxcIlwiLGUuX19aPSEwLGV9LGlzWjpmdW5jdGlvbihlKXtyZXR1cm5cImFycmF5XCI9PT10LnR5cGUoZSkmJlwiX19aXCJpbiBlfX0pO3RyeXtnZXRDb21wdXRlZFN0eWxlKHZvaWQgMCl9Y2F0Y2goZSl7dmFyIG49Z2V0Q29tcHV0ZWRTdHlsZTt3aW5kb3cuZ2V0Q29tcHV0ZWRTdHlsZT1mdW5jdGlvbih0KXt0cnl7cmV0dXJuIG4odCl9Y2F0Y2goZSl7cmV0dXJuIG51bGx9fX19KFplcHRvKTtcclxuXHJcbi8vICAgICBaZXB0by5qc1xyXG4vLyAgICAgKGMpIDIwMTAtMjAxNiBUaG9tYXMgRnVjaHNcclxuLy8gICAgIFplcHRvLmpzIG1heSBiZSBmcmVlbHkgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLlxyXG4vL1xyXG4vLyAgICAgaHR0cHM6Ly9naXRodWIuY29tL21hZHJvYmJ5L3plcHRvL2Jsb2IvbWFzdGVyL3NyYy9zZWxlY3Rvci5qc1xyXG5cclxuIWZ1bmN0aW9uKHQpe2Z1bmN0aW9uIG4obil7cmV0dXJuIG49dChuKSwhKCFuLndpZHRoKCkmJiFuLmhlaWdodCgpKSYmXCJub25lXCIhPT1uLmNzcyhcImRpc3BsYXlcIil9ZnVuY3Rpb24gZSh0LG4pe3Q9dC5yZXBsYWNlKC89I1xcXS9nLCc9XCIjXCJdJyk7dmFyIGUsaSxyPXUuZXhlYyh0KTtpZihyJiZyWzJdaW4gcyYmKGU9c1tyWzJdXSxpPXJbM10sdD1yWzFdLGkpKXt2YXIgbz1OdW1iZXIoaSk7aT1pc05hTihvKT9pLnJlcGxhY2UoL15bXCInXXxbXCInXSQvZyxcIlwiKTpvfXJldHVybiBuKHQsZSxpKX12YXIgaT10LnplcHRvLHI9aS5xc2Esbz1pLm1hdGNoZXMscz10LmV4cHJbXCI6XCJdPXt2aXNpYmxlOmZ1bmN0aW9uKCl7cmV0dXJuIG4odGhpcyk/dGhpczp2b2lkIDB9LGhpZGRlbjpmdW5jdGlvbigpe3JldHVybiBuKHRoaXMpP3ZvaWQgMDp0aGlzfSxzZWxlY3RlZDpmdW5jdGlvbigpe3JldHVybiB0aGlzLnNlbGVjdGVkP3RoaXM6dm9pZCAwfSxjaGVja2VkOmZ1bmN0aW9uKCl7cmV0dXJuIHRoaXMuY2hlY2tlZD90aGlzOnZvaWQgMH0scGFyZW50OmZ1bmN0aW9uKCl7cmV0dXJuIHRoaXMucGFyZW50Tm9kZX0sZmlyc3Q6ZnVuY3Rpb24odCl7cmV0dXJuIDA9PT10P3RoaXM6dm9pZCAwfSxsYXN0OmZ1bmN0aW9uKHQsbil7cmV0dXJuIHQ9PT1uLmxlbmd0aC0xP3RoaXM6dm9pZCAwfSxlcTpmdW5jdGlvbih0LG4sZSl7cmV0dXJuIHQ9PT1lP3RoaXM6dm9pZCAwfSxjb250YWluczpmdW5jdGlvbihuLGUsaSl7cmV0dXJuIHQodGhpcykudGV4dCgpLmluZGV4T2YoaSk+LTE/dGhpczp2b2lkIDB9LGhhczpmdW5jdGlvbih0LG4sZSl7cmV0dXJuIGkucXNhKHRoaXMsZSkubGVuZ3RoP3RoaXM6dm9pZCAwfX0sdT1uZXcgUmVnRXhwKFwiKC4qKTooXFxcXHcrKSg/OlxcXFwoKFteKV0rKVxcXFwpKT8kXFxcXHMqXCIpLGM9L15cXHMqPi8saD1cIlplcHRvXCIrICtuZXcgRGF0ZTtpLnFzYT1mdW5jdGlvbihuLG8pe3JldHVybiBlKG8sZnVuY3Rpb24oZSxzLHUpe3RyeXt2YXIgYTshZSYmcz9lPVwiKlwiOmMudGVzdChlKSYmKGE9dChuKS5hZGRDbGFzcyhoKSxlPVwiLlwiK2grXCIgXCIrZSk7dmFyIGY9cihuLGUpfWNhdGNoKGQpe3Rocm93IGNvbnNvbGUuZXJyb3IoXCJlcnJvciBwZXJmb3JtaW5nIHNlbGVjdG9yOiAlb1wiLG8pLGR9ZmluYWxseXthJiZhLnJlbW92ZUNsYXNzKGgpfXJldHVybiBzP2kudW5pcSh0Lm1hcChmLGZ1bmN0aW9uKHQsbil7cmV0dXJuIHMuY2FsbCh0LG4sZix1KX0pKTpmfSl9LGkubWF0Y2hlcz1mdW5jdGlvbih0LG4pe3JldHVybiBlKG4sZnVuY3Rpb24obixlLGkpe3JldHVybighbnx8byh0LG4pKSYmKCFlfHxlLmNhbGwodCxudWxsLGkpPT09dCl9KX19KFplcHRvKTtcclxuIl19