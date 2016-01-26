import Zepto from './zepto.min';
import svg4everybody from './svg4everybody';
import Cookies from './jscookie';

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



/* OLD POP-OUT CONTROLLER
$('.pop-out__trigger-content').on('click', function togglePopOut(e) {
	$(e.target).parents('.pop-out').css('position', 'absolute');
	$(e.target).parents('.pop-out__trigger').toggleClass('is-active');
});*/

function popOutController() {
	var rem = 16; // Simulate CSS `rem` (16px)
	var state = {
		activeElm: null
	};

	this.getPopOutElement = function() {
		return state.activeElm;
	};

	this.closePopOut = function() {
		$('.pop-out').removeClass('is-active');
		state.activeElm = null;
	}

	this.togglePopOut = function(e) {
		// Check if clicked element is the toggle itself
		// Otherwise, climb up DOM tree and find it
		var poParent = e.hasClass('js-pop-out-trigger') ? e : e.closest('.js-pop-out-trigger');

		// If the current pop-out is the same as the active pop-out...
		if(poParent[0] === state.activeElm) {
			// ...close it
			this.closePopOut();
		} else {
			// ...update the controller state and open it
			state.activeElm = poParent[0];
			updatePosition();
		}

	};

	var updatePosition = function() {
		var t = { // The pop-out toggle element
			elm: $(state.activeElm)
		};
		t.pos = t.elm.offset();

		// Close all open pop-outs
		$('.pop-out').removeClass('is-active');

		// Determine which pop-out template to use
		var targetPopOut;
		switch (t.elm.data('pop-out-type')) {
			case 'sign-in':
				targetPopOut = $('.js-pop-out__sign-in');
				break;
			case 'email-article':
				targetPopOut = $('.js-pop-out__email-article');
				break;
			default:
				console.warn('Attempting to fire unidentified pop-out.');
				return;
				break;
		}
		if(t.elm.data('pop-out-align') === 'right') {
			var boxOffsetTop = Math.floor(t.pos.top + t.pos.height + (rem - 1));
			var boxOffsetLeft = Math.floor(t.pos.left + t.pos.width - 380 + (rem - 1));
			var tabOffsetLeft = 'auto';
			var tabOffsetRight = '0px';
		} else {
			var boxOffsetTop = Math.floor(t.pos.top + t.pos.height + (rem - 1));
			var boxOffsetLeft = Math.floor(t.pos.left - ((380 - t.pos.width) / 2));
			var tabOffsetLeft = 0;
			var tabOffsetRight = 0;
		}
		targetPopOut.css({
			zIndex: t.elm.css('z-index') - 2,
			transform: 'translate3d(' + boxOffsetLeft +'px, ' + boxOffsetTop + 'px, 0)',
		}).toggleClass('is-active');

		$('.pop-out__tab').css({
			height: t.pos.height + (rem * 2) + "px",
			width: t.pos.width + (rem * 2) + "px",
			left: tabOffsetLeft,
			right: tabOffsetRight,
			top: '-' + (t.pos.height + (rem * 2) - 1) + "px",
			zIndex: t.elm.css('z-index') - 1
		});
	};

	// If there is an active pop-out, update its position
	// Mostly useful for when the browser window resizes
	this.updatePopOut = function() {
		if(state.activeElm) {
			updatePosition();
		}
	}
};


// Pre-registration username validation
$('.header__hover--register .js-register-submit').on('click', function validateUsername(e) {
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


$(document).ready(function() {

	var poc = new popOutController();

	// Toggle pop-out when clicked
	$('.js-pop-out-trigger').on('click', function togglePopOut2(event) {
		poc.togglePopOut($(event.target));
	});

	// Reposition pop-out when browser window resizes
	$(window).on('resize', function(event) {
		poc.updatePopOut();
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
});
