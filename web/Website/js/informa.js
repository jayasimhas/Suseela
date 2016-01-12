// TODO include this file individually to prevent one giant file
// TODO note that this is Zepto, not jQuery

/* Toggle menu visibility */
$('.js-toggle-menu').on('click', function toggleMenu() {
	$('.main-menu').toggleClass('is-active');
	$('.menu-toggler').toggleClass('is-active');
});

/* Toggle menu categories */
$('.js-toggle-menu-section').on('click', function toggleMenuItems(e) {
	$(e.target).toggleClass('is-active');
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
	thisBanner.addClass('is-hidden');

	var dismissedBanners = Cookies.get('dismissedBanners') || {};
	dismissedBanners[thisBanner.data('banner-id')] = true;
	Cookies.set('dismissedBanners', dismissedBanners);
});

/* Generic toggle pop-out */
$('.js-toggle-pop-out').on('click', function togglePopOut(e) {
	$(e.target).parents('.pop-out__trigger').toggleClass('is-active');
});

$(document).ready(function() {

	var dismissedBanners = Cookies.getJSON('dismissedBanners') || {};
	$('.banner').each(function() {
		if($(this).data('banner-id') in dismissedBanners === false) {
			$(this).css('display', 'block');
		}
	});

});
