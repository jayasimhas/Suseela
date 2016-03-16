import Zepto from './zepto.min';
import svg4everybody from './svg4everybody';
import Cookies from './jscookie';
import PopOutController from './pop-out-controller';
import NewsletterSignupController  from './newsletter-signup';
import BookmarkController from './bookmark-controller';
import SearchScript from './search-page.js';
import LoginController from './login-controller';
import ResetPasswordController from './reset-password-controller';
import RegisterController from './register-controller';
import FormController from './form-controller';



/* Toggle menu categories */
$('.js-toggle-menu-section').on('click', function toggleMenuItems(e) {
	$(e.target).toggleClass('is-active');
});

/* 	Elements with `position:absolute` don't bubble click events
	`pointer-events: none` would fix, but isn't supported by IE 10.
	Need to hoist the click event to the parent element to toggle menu items. */
$('.js-hoist-menu-click').on('click', function hoistMenuClick(e) {
	$(e.target).parents('.js-toggle-menu-section').trigger('click');
});

/* Toggle header search box (tablets/smartphones) */
$('.js-header-search-trigger').on('click', function toggleMenuItems(e) {
	if($(window).width() <= 800) {
		$('.header-search__wrapper').toggleClass('is-active').focus();
	} else {
		$(e.target).closest('form').submit();
	}
	e.preventDefault();
	return false;
});

/* Generic banner dismiss */
$('.js-dismiss-banner').on('click', function dismissBanner(e) {
	var thisBanner = $(e.srcElement).parents('.banner');
	thisBanner.removeClass('is-visible');

	var dismissedBanners = Cookies.getJSON('dismissedBanners') || {};
	dismissedBanners[thisBanner.data('banner-id')] = true;
	Cookies.set('dismissedBanners', dismissedBanners);
});

	// Make sure proper elm gets the click event
// When a user submits a Forgot Password request, this will display the proper
// success message and hide the form to prevent re-sending.
var showForgotPassSuccess = function() {
	$('.pop-out__sign-in-forgot-password-nested').toggleClass('is-hidden');
	$('.pop-out__sign-in-forgot-password')
		.find('.alert-success')
		.toggleClass('is-active');
};

// Toggle the sign-in error message displayed to a user
var toggleSignInError = function() {
	$('.pop-out__form-error').show();
	//$('.pop-out__form-error').toggleClass('is-active'); - bugged due to styling issues
};

var renderIframeComponents = function() {
	$('.iframe-component').each(function(index, elm) {
		var desktopEmbed = $(elm).find('.iframe-component__desktop');
		var mobileEmbed = $(elm).find('.iframe-component__mobile')
		var mobileEmbedLink = mobileEmbed.data('embed-link');

		// Check if the user is viewing inside the page editor
		// Don't hide/show desktop and/or mobile, just keep both visible
		// so users can add, edit, or delete either.
		if(desktopEmbed.hasClass('is-page-editor')) {
			return;
		}

		if($(window).width() <= 480 && mobileEmbedLink) {
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
	});
};

$(document).ready(function() {

	// Anti Forgery Token
	var requestVerificationToken = $('.main__wrapper').data('request-verification-token');

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

	indexBookmarks();

	var newsletterSignup = new NewsletterSignupController();
	$(".newsletter-signup-after-submit").hide();
	newsletterSignup.checkForUserSignedUp();
	newsletterSignup.addControl('.js-newsletter-signup-submit', null,function(triggerElement) {
		  //  toggleSignInError();
		});


	var login = new LoginController(requestVerificationToken);

	login.addControl(
		'.js-sign-in-submit',
		null,
		function(triggerElement) {
			toggleSignInError();
		}
	);

	var resetPassword = new FormController();
	resetPassword.watchForm('.form-reset-password', function() {
		$('.form-reset-password').find('.alert-success').show();
	});

	var newResetPassToken = new FormController();
	newResetPassToken.watchForm('.form-new-reset-pass-token', function() {
		$('.form-new-reset-pass-token').find('.alert-success').show();
	});
	/*
	resetPassword.addRequestControl(
		'.js-reset-password-request-submit',
		function(triggerElement) {
			$(triggerElement).parents('.js-reset-password-request-form').hide();
		}
	);
	resetPassword.addChangeControl('.js-reset-password-change-submit');
	resetPassword.addRetryControl('.js-reset-password-retry-submit');
*/

	var userRegistrationController = new FormController();
	userRegistrationController.watchForm('.form-registration');

	userRegistrationController.watchForm('.form-registration-optins');

	userRegistrationController.watchForm(
		'.form-pre-registration',
		function(form) {
			var usernameInput = $(form).find('.js-register-username');
			var nextStepUrl = $(form).data('on-success') + '?' + usernameInput.attr('name') + '=' + encodeURIComponent(usernameInput.val());

			window.location.href = nextStepUrl;
		}
	);

	var registerController = new RegisterController();

	registerController.addRegisterUserControl('.js-register-user-optins-submit');

	var emailArticleController = new FormController();
	emailArticleController.watchForm('.form-email-article');

	var accountEmailPreferencesController = new FormController();
	accountEmailPreferencesController.watchForm('.form-email-preferences');

    svg4everybody();

	var getHeaderEdge = function() {
		return $('.header__wrapper').offset().top + $('.header__wrapper').height();
	};

	/* Toggle menu visibility */
	$('.js-toggle-menu').on('click', function toggleMenu() {
		if($('.main-menu').hasClass('is-active')) {
			$('.main-menu').removeClass('is-active');
			$('.menu-toggler').removeClass('is-active');
			$('body').removeClass('is-frozen');
			if($(window).scrollTop() <= getHeaderEdge()) {
				$('.header__wrapper .menu-toggler').removeClass('is-sticky');
			}
		} else {
			$('.main-menu').addClass('is-active');
			$('.menu-toggler').addClass('is-active');
			$('.header__wrapper .menu-toggler').addClass('is-sticky');
			$('body').addClass('is-frozen');
		}
	});

	/* Attach / detach sticky menu */
	$(window).on('scroll', function windowScrolled() {
		if ($(this).scrollTop() > getHeaderEdge() || $('.main-menu').hasClass('is-active')) {
			$('.header__wrapper .menu-toggler').addClass('is-sticky');
		} else {
			$('.header__wrapper .menu-toggler').removeClass('is-sticky');
		}
	});


    var dismissedBanners = Cookies.getJSON('dismissedBanners') || {};
	$('.banner').each(function() {
		if($(this).data('banner-id') in dismissedBanners === false) {
			$(this).addClass('is-visible');
		}
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

 	    $(tableLink).find('a').attr("href", url);
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

			linkList.append('<a href="#' + id + '">' + text + '</a>');
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
		poc.closePopOut();
	});

	// Make sure all external links open in a new window/tab
	$("a[href^=http]").each(function(){
		if(this.href.indexOf(location.hostname) == -1) {
           $(this).attr({
               target: "_blank",
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


	$('.informa-ribbon__title').on('click', function (e) {

		$('.informa-ribbon').toggleClass('show')

	});

	// Twitter sharing JS
	window.twttr=function(t,e,r){var n,i=t.getElementsByTagName(e)[0],w=window.twttr||{};return t.getElementById(r)?w:(n=t.createElement(e),n.id=r,n.src="https://platform.twitter.com/widgets.js",i.parentNode.insertBefore(n,i),w._e=[],w.ready=function(t){w._e.push(t)},w)}(document,"script","twitter-wjs");

});
