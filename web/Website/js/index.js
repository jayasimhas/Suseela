/* global angular, analytics_data */

import Zepto from './zepto.min';
import svg4everybody from './svg4everybody';
import Cookies from './jscookie';
import PopOutController from './pop-out-controller';
import NewsletterSignupController  from './newsletter-signup';
import BookmarkController from './bookmark-controller';
import SearchScript from './search-page.js';
import ResetPasswordController from './reset-password-controller';
import RegisterController from './register-controller';
import FormController from './form-controller';
import SortableTableController from './sortable-table-controller';
import { analyticsEvent } from './analytics-controller';


// Make sure proper elm gets the click event
// When a user submits a Forgot Password request, this will display the proper
// success message and hide the form to prevent re-sending.
var showForgotPassSuccess = function() {
    $('.pop-out__sign-in-forgot-password-nested').toggleClass('is-hidden');
    $('.pop-out__sign-in-forgot-password')
		.find('.alert-success')
		.toggleClass('is-active');
};

var renderIframeComponents = function() {
    $('.iframe-component').each(function(index, elm) {
        var desktopEmbed = $(elm).find('.iframe-component__desktop iframe');
        var mobileEmbed = $(elm).find('.iframe-component__mobile iframe');
        var mobileEmbedLink = mobileEmbed.data('embed-link');

        // Check if the user is viewing inside the page editor
        // Don't hide/show desktop and/or mobile, just keep both visible
        // so users can add, edit, or delete either.
        if(desktopEmbed.hasClass('is-page-editor')) {
            return;
        }

        if($(window).width() <= 480) {
            mobileEmbed.show();
            desktopEmbed.hide();
            if(mobileEmbed.html() == '') {
                mobileEmbed.html(mobileEmbed.data('embed-link'));
            }
        } else {
            desktopEmbed.show();
            mobileEmbed.hide();
            if(desktopEmbed.html() == '') {
                desktopEmbed.html(desktopEmbed.data('embed-link'));
            }
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

$(document).ready(function() {

    // Anti Forgery Token
    var requestVerificationToken = $('.main__wrapper').data('request-verification-token');

    /* * *
        Traverses the DOM and registers event listeners for any pop-out triggers.
        Bound explicitly to `window` for easier access by Angular.
    * * */
    window.indexPopOuts = function() {
        window.controlPopOuts = new PopOutController('.js-pop-out-trigger');

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


    window.bookmark = new BookmarkController();

    /* * *
        Traverses the DOM and registers event listeners for any bookmarkable
        articles. Bound explicitly to `window` for easier access by Angular.
    * * */
    window.indexBookmarks = function() { // Toggle bookmark icon
        $('.js-bookmark-article').on('click', function bookmarkArticle(e) {
            // Make sure proper elm gets the click event
            if (e.target !== this) {
                this.click();
                return;
            }

            window.bookmark.toggle(e.target);

        });
    };

    window.indexBookmarks();


    /* * *
        If a user tries bookmarking an article while logged out, they'll be
        prompted to sign in first. This checks for any articles that have been
        passed along for post-sign-in bookmarking. Bound explicitly to `window`
        for easier access by Angular.
    * * */
    window.autoBookmark = function() {

        var bookmarkTheArticle = function(article) {
            $('.js-bookmark-article').each(function(indx, item) {
                if($(item).data('bookmark-id') === article
                    && !$(item).data('is-bookmarked')) {

                    $(item).click();

                } else {
                    // already bookmarked or not a match
                }
            });
        };

        var urlVars = window.location.href.split("?");
        var varsToParse = urlVars[1] ? urlVars[1].split("&") : null;
        if(varsToParse) {
            for (var i=0; i<varsToParse.length; i++) {
                var pair = varsToParse[i].split("=");
                if(pair[0] === 'immb') {
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

        var eventDetails = {
            event_name: "search",
            search_keyword: '"' + $('.header-search__field').val() + '"'
        };

        analyticsEvent( $.extend(analytics_data, eventDetails) );


        if($(window).width() <= 800) {
            $('.header-search__wrapper').toggleClass('is-active').focus();
        } else {
            $(e.target).closest('form').submit();
        }
        e.preventDefault();
        return false;
    });

    var newsletterSignup = new NewsletterSignupController();
    newsletterSignup.checkForUserSignedUp();
    newsletterSignup.addControl('.js-newsletter-signup-submit', null,function(triggerElement) {
    });


    /* * *
        Handle user sign-in attempts.
    * * */
    var userSignIn = new FormController({
        observe: '.js-sign-in-submit',
        successCallback: function(form, context, event) {

            var loginAnalytics =  {
                event_name: 'login',
                login_state: 'successful',
                userName: '"' + $(form).find('input[name=username]').val() + '"'
            };

            analyticsEvent(	$.extend(analytics_data, loginAnalytics) );

            /* * *
                If `pass-article-id` is set, user is probably trying to sign in
                after attempting to bookmark an article. Add the article ID to
                the URL so `autoBookmark()` catches it.
            * * */
            var passArticleId = $(form).find('.sign-in__submit').data('pass-article-id');
            if(passArticleId) {
                var sep = (window.location.href.indexOf('?') > -1) ? '&' : '?';

                window.location.href = window.location.href + sep + 'immb=' + passArticleId;

                // If Angular, need location.reload to force page refresh
                if(typeof angular !== 'undefined') {
                    angular.element($('.search-results')[0]).controller().forceRefresh();
                }

            } else {
                window.location.reload(false);
            }
        },
        failureCallback: function(form, context, event) {

            var loginAnalytics = {
                event_name: "login",
                login_state: "unsuccessful",
                userName: '"' + $(form).find('input[name=username]').val() + '"'
            };

            analyticsEvent( $.extend(analytics_data, loginAnalytics) );

		}
    });

    var resetPassword = new FormController({
        observe: '.form-reset-password',
        successCallback: function() {
            $('.form-reset-password').find('.alert-success').show();
        }
    });

    var newResetPassToken = new FormController({
        observe: '.form-new-reset-pass-token',
        successCallback: function() {
            $('.form-new-reset-pass-token').find('.alert-success').show();
        }
    });

    var userRegistrationController = new FormController({
        observe: '.form-registration'
    });

    var userRegistrationFinalController = new FormController({
        observe: '.form-registration-optins'
    });

    var userPreRegistrationController = new FormController({
		observe: '.form-pre-registration',
		successCallback: function(form) {
            var usernameInput = $(form).find('.js-register-username');
            var nextStepUrl = $(form).data('forwarding-url') + '?' + usernameInput.attr('name') + '=' + encodeURIComponent(usernameInput.val());

            window.location.href = nextStepUrl;
		}
	});

    $('.click-logout').on('click', function(e) {
        analyticsEvent( $.extend(analytics_data, { event_name: "logout" }) );
    });


    var emailArticleController = new FormController({
        observe: '.form-email-article',
        successCallback: function(form) {
            $('.js-email-article-form-wrapper').hide();
            $('.js-email-article-recip-success').html($('.js-email-article-recip-addr').val());
            $('.js-email-article-success').show();

            // Reset the Email Article pop-out to its default state when closed
            $('.js-dismiss-email-article').one('click', function() {
                $('.js-email-article-form-wrapper').show();
                $('.js-email-article-success').hide();
            });
        }
    });


    var emailSearchController = new FormController({
        observe: '.form-email-search',
        successCallback: function(form) {

            $('.js-email-search-form-wrapper').hide();
            $('.js-email-search-recip-success').html($('.js-email-search-recip-addr').val());
            $('.js-email-search-success').show();
            $('.js-email-search-form-wrapper input, .js-email-search-form-wrapper textarea').val('');

            // Reset the Email Article pop-out to its default state when closed
            $('.js-dismiss-email-search').one('click', function() {
                $('.js-email-search-form-wrapper').show();
                $('.js-email-search-success').hide();
            });

        },
        beforeRequest: function() {

            var resultIDs = null;

            $('.js-angular-bookmark').each(function(indx, item) {
                resultIDs = resultIDs ? resultIDs + ',' + $(item).data('bookmark-id') : $(item).data('bookmark-id');
            });

            $('.js-email-search-results-ids').val(resultIDs);
            $('.js-email-search-query').val($('.search-bar__field').val());
            $('.js-email-search-query-url').val(document.location.href);

        }
    });


    var accountEmailPreferencesController = new FormController({
        observe: '.form-email-preferences'
    });


    var accountUpdatePassController = new FormController({
        observe: '.form-update-account-pass',
		successCallback: function(form, context, evt) {
			$(form).find('input, select, textarea').each(function() {
				$(this).val('');
			});
		}
	});

    var accountUpdateContactController = new FormController({
        observe: '.form-update-account-contact',
        successCallback: function(form, context, evt) {
            $(window).scrollTop(($(evt.target).closest('form').find('.js-form-error-general').offset().top - 32));
        }
    });

    var savedDocumentsController = new FormController({
        observe: '.form-remove-saved-document',
        successCallback: function(form, context, evt) {
            $(evt.target).closest('tr').remove();
        }
    });


    svg4everybody();

    var getHeaderEdge = function() {
        return $('.header__wrapper').offset().top + $('.header__wrapper').height();
    };

    /* * *
        MAIN SITE MENU
    * * */
    (function MenuController() {

        var showMenu = function() {
            $('.main-menu').addClass('is-active');
            $('.menu-toggler').addClass('is-active');
            $('.header__wrapper .menu-toggler').addClass('is-sticky');
            $('body').addClass('is-frozen');
        };

        var hideMenu = function() {
            $('.main-menu').removeClass('is-active');
            $('.menu-toggler').removeClass('is-active');
            $('body').removeClass('is-frozen');
            if($(window).scrollTop() <= getHeaderEdge()) {
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
            if ($(this).scrollTop() > getHeaderEdge() || $('.main-menu').hasClass('is-active')) {
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
    var dismissedBanners = Cookies.getJSON('dismissedBanners') || {};
    $('.banner').each(function() {
        if($(this).data('banner-id') in dismissedBanners === false) {
            $(this).addClass('is-visible');
        }
    });

    /* * *
        Generic banner dismiss
    * * */
    $('.js-dismiss-banner').on('click', function dismissBanner(e) {
        var thisBanner = $(e.target).parents('.banner');
        thisBanner.removeClass('is-visible');

        var dismissedBanners = Cookies.getJSON('dismissedBanners') || {};
        dismissedBanners[thisBanner.data('banner-id')] = true;
        Cookies.set('dismissedBanners', dismissedBanners);
    });

    // For each article table, clone and append "view full table" markup
    $('.article-body-content table').not('.article-table--mobile-link').forEach(function(e) {
        var mediaId = $(e).data("mediaid");
        var tableLink = $('.js-mobile-table-template .article-table').clone();

        var url = window.location.href;
        url.replace("#", "");
        if (url.indexOf("?") < 0)
            url += "?";
        else
            url += "&";

        url+= "mobilemedia=true&selectedid=" + mediaId;

        // $(tableLink).find('a').attr("href", url);
        $(tableLink).find('a').data("table-url", url).attr('href', null);
        $(e).after(tableLink);
    });

    // When DOM loads, render the appropriate iFrame components
    // Also add a listener for winder resize, render appropriate containers
    renderIframeComponents();
    $(window).on('resize', (event) => {
        renderIframeComponents();
    });

    // Topic links
    var topicAnchors = $('.js-topic-anchor');

    $('.sub-topic-links').forEach(function(e) {
        var linkList = $(e).find('.bar-separated-link-list');

        topicAnchors.forEach(function(tc) {
            var id = tc.id;
            var text = $(tc).data('topic-link-text');
            var utagInfo = '{"event_name"="topic-jump-to-link-click","topic-name"="'+text+'"}';
            linkList.append('<a href="#' + id + '" class="click-utag" data-info='+text+'>' + text + '</a>');
        });
    });

    // Display the Forgot Password block when "forgot your password" is clicked
    $('.js-show-forgot-password').on('click', function toggleForgotPass() {
        $('.pop-out__sign-in-forgot-password').toggleClass('is-active');
    });

    // Global dismiss button for pop-outs
    $('.dismiss-button').on('click', function(e) {
        if (e.target !== this) {
            this.click();
            return;
        }

        $($(e.target).data('target-element')).removeClass('is-active');
        window.controlPopOuts.closePopOut(e.target);
    });

    // Make sure all external links open in a new window/tab
    $("a[href^=http]").each(function(){
        if(this.href.indexOf(location.hostname) == -1) {
            $(this).attr({
                target: "_blank"
            });
        }
    });

    $('.general-header__navigation').each(function() {

        $(this).on('scroll', function() {
            var scrollLeft = $(this).scrollLeft();
            var scrollWidth = $(this)[0].scrollWidth;
            var winWidth = $(window).width();

            if(scrollLeft > 32) {
                $('.general-header__navigation-scroller--left').addClass('is-visible');
            } else {
                $('.general-header__navigation-scroller--left').removeClass('is-visible');
            }

            if(scrollLeft + winWidth < scrollWidth - 32) {
                $('.general-header__navigation-scroller--right').addClass('is-visible');
            } else {
                $('.general-header__navigation-scroller--right').removeClass('is-visible');
            }

        });

        var scrollLeft = $(this).scrollLeft();
        var scrollWidth = $(this)[0].scrollWidth;
        var winWidth = $(window).width();

        if(scrollLeft + winWidth < scrollWidth - 32) {
            $('.general-header__navigation-scroller--right').addClass('is-visible');
        } else {
            $('.general-header__navigation-scroller--right').removeClass('is-visible');
        }
    });


    // Smooth, clickable scrolling for General page headers
    var smoothScrollingNav = function() {

        // Cache for less DOM checking
        var Scrollable = $('.general-header__navigation');
        var Container = $('.general-header');

        // Find current scroll distance is from left and right edges
        var scrollDistance = function() {
            return {
                left: Scrollable.scrollLeft(),
                right: Scrollable[0].scrollWidth - (Container.width() + Scrollable.scrollLeft())
            };
        };

        var init = function() {

            $('.general-header__navigation-scroller--right').on('click', function() {
                if(scrollDistance().right > 0) { // Not on right edge
                    smoothScroll(200, 'right');
                }
            });

            $('.general-header__navigation-scroller--left').on('click', function() {
                if(scrollDistance().left > 0) {
                    smoothScroll(200, 'left');
                }
            });

        };

        var scrollToTimerCache;
        var totalTravel = null;
        var durationStart = null;

        // Quadratic ease-out algorithm
        var easing = function(time, distance) {
            return distance * (time * (2 - time));
        };

        var smoothScroll = function(duration, direction) {
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
            var travelPcent = 1 - (duration / durationStart);

            // Finds travel change on this loop, adjusted for ease-out
            var travel = easing(travelPcent, ((totalTravel / durationStart) * 10));

            scrollToTimerCache = setTimeout(function() {
                if (!isNaN(parseInt(travel, 10))) {
                    if(direction === 'right') {
                        Scrollable.scrollLeft(Scrollable.scrollLeft() + travel);
                        smoothScroll(duration - 10, direction);
                    } else if(direction === 'left') {
                        Scrollable.scrollLeft(Scrollable.scrollLeft() - travel);
                        smoothScroll(duration - 10, direction);
                    }

                }
            }.bind(this), 10);
        };

        // Bind event listeners
        init();
    };

    $('#newsletters').on('click',function(e){
        newsletterOptins();
    });

    var newsletterOptins = function(){
        if ($('#newsletters').is(':checked')) {
            $('.registration-final').data("info",{"event_name":"registration-complete-thank-you","newsletter_optin":"true"});
        } else {
            $('.registration-final').data("info",{"event_name":"registration-complete-thank-you","newsletter_optin":"false"});
        }
    };

    // TODO - Refactor this code, update class name to a `js-` name
    $('.manage-preferences').click(function(e) {
        var preferencesData = {
            event_name: "manage-preferences"
        };
        if($("#NewsletterOptIn").is(':checked') && $("#DoNotSendOffersOptIn").is(':checked')) {
            preferencesData = {
                newsletter_optin: "true",
                donot_send_offers_optin: "true"
            };
        }
        if(!$("#NewsletterOptIn").is(':checked') && $("#DoNotSendOffersOptIn").is(':checked')) {
            preferencesData = {
                newsletter_optin: "false",
                donot_send_offers_optin: "true"
            };
        }
        if($("#NewsletterOptIn").is(':checked') && !$("#DoNotSendOffersOptIn").is(':checked')) {
            preferencesData = {
                newsletter_optin: "true",
                donot_send_offers_optin: "false"
            };
        }
        if(!$("#NewsletterOptIn").is(':checked') && !$("#DoNotSendOffersOptIn").is(':checked')) {
            preferencesData = {
                newsletter_optin: "false",
                donot_send_offers_optin: "false"
            };
        }

        analyticsEvent( $.extend(analytics_data, preferencesData) );


    });

    newsletterOptins();
    // Execute!
    smoothScrollingNav();


	// Toggle global Informa bar
    $('.informa-ribbon__title').on('click', function (e) {
        $('.informa-ribbon').toggleClass('show');
    });


    var sortTheTables = new SortableTableController();


    $('.click-utag').click(function (e) {
        analyticsEvent( $.extend(analytics_data, $(this).data('info')) );
    });

    $('#chkASBilling').click(function(e){
        if($(this).is(':checked'))
        {
            $('#ddlShippingCountry').val($('#ddlBillingCountry').val());
            $('#txtShippingAddress1').val($('#txtBillingAddress1').val());
            $('#txtShippingAddress2').val($('#txtBillingAddress2').val());
            $('#txtShippingCity').val($('#txtBillingCity').val());
            $('#txtShippingState').val($('#txtBillingState').val());
            $('#txtShippingPostalCode').val($('#txtBillingPostalCode').val());
        }
    });
    // Twitter sharing JS
    window.twttr = function(t,e,r){var n,i=t.getElementsByTagName(e)[0],
        w=window.twttr||{};
    return t.getElementById(r) ? w : (n=t.createElement(e),
    n.id=r,n.src="https://platform.twitter.com/widgets.js",
    i.parentNode.insertBefore(n,i),w._e=[],
    w.ready=function(t) { w._e.push(t); },
    w); } (document,"script","twitter-wjs");

});
