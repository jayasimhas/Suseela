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
	$('.header__search-field').toggleClass('is-active').focus();
});

/* Attach / detach sticky menu */
$(window).on('scroll', function windowScrolled() {
	if ($(this).scrollTop() > 100) {
		$('.header__wrapper .menu-toggler').addClass('is-sticky');
	} else {
		$('.header__wrapper .menu-toggler').removeClass('is-sticky');
	}
});
