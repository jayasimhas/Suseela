import Zepto from './zepto.min';
import svg4everybody from './svg4everybody';
import Cookies from './jscookie';
import PopOutController from './pop-out-controller';

/* Toggle menu visibility */
$('.js-toggle-menu').on('click', function toggleMenu() {
	$('.main-menu').toggleClass('is-active');
	$('.menu-toggler').toggleClass('is-active');
});

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
	$('.header-search__wrapper').toggleClass('is-active').focus();
});

/* Attach / detach sticky menu */
$(window).on('scroll', function windowScrolled() {
	if ($(this).scrollTop() > 100) {
		$('.header__wrapper .menu-toggler').addClass('is-sticky');
	} else {
		$('.header__wrapper .menu-toggler').removeClass('is-sticky');
	}
});

/* Generic banner dismiss */
$('.js-dismiss-banner').on('click', function dismissBanner(e) {
	var thisBanner = $(e.srcElement).parents('.banner');
	thisBanner.removeClass('is-visible');
	console.log(thisBanner);

	var dismissedBanners = Cookies.get('dismissedBanners') || {};
	dismissedBanners[thisBanner.data('banner-id')] = true;
	Cookies.set('dismissedBanners', dismissedBanners);
});



// Pre-registration username validation
$('.js-register-submit').on('click', function validateUsername(e) {
	var submitButton = $(e.target);
	var username = submitButton.siblings('.js-register-username').val();
	$.post('account/api/accountvalidation/username/', { username: username }, function (response) {
		if (response.valid) {
			var redirectUrl = submitButton.attr('data-register-redirect');

			window.location.href = redirectUrl + '?username=' + username;
		}
		else {
			submitButton.siblings('.js-register-invalid').show();
		}
	});
});


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
	$('.pop-out__form-error').toggleClass('is-active');
};

$(document).ready(function() {

	var poc = new PopOutController('.js-pop-out-trigger');

	poc.customize({
		id: 'header-register',
		tabStyles: {
			deskHeight: 87,
			tabletHeight: 72,
			phoneHeight: '' // Default
		}
	});

	poc.customize({
		id: 'header-signin',
		tabStyles: {
			deskHeight: 87,
			tabletHeight: 72,
			phoneHeight: '' // Default
		}
	});

    svg4everybody();

    var dismissedBanners = Cookies.getJSON('dismissedBanners') || {};
	$('.banner').each(function() {
		if($(this).data('banner-id') in dismissedBanners === false) {
			$(this).addClass('is-visible');
		}
	});

	// For each article table, clone and append "view full table" markup
	$('.article-body-content table').forEach(function(e) {
		$(e).after($('.js-mobile-table-template .article-table').clone());
	});

	// Topic links
	var topicContainers = $('.topic-subtopic');

	$('.sub-topic-links').forEach(function(e) {
		var linkList = $(e).find('.bar-separated-link-list');

		topicContainers.forEach(function(tc) {
			var id = tc.id;
			var text = $(tc).data('topic-link-text');

			linkList.append('<a href="#' + id + '">' + text + '</a>');
		});
	});

	// Display the Forgot Password block when "forgot your password" is clicked
	$('.js-show-forgot-password').on('click', function toggleForgotPass() {
		$('.pop-out__sign-in-forgot-password').toggleClass('is-active');
	});


	// Twitter sharing JS
	window.twttr=function(t,e,r){var n,i=t.getElementsByTagName(e)[0],w=window.twttr||{};return t.getElementById(r)?w:(n=t.createElement(e),n.id=r,n.src="https://platform.twitter.com/widgets.js",i.parentNode.insertBefore(n,i),w._e=[],w.ready=function(t){w._e.push(t)},w)}(document,"script","twitter-wjs");

});
