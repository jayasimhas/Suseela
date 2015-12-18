import './vendor';

/* Toggle menu visibility */
$('.main-menu__toggler').on('click', function toggleMenu() {
	$('.main-menu').toggleClass('is-active');
	$('.main-menu__toggler').toggleClass('is-active');
});

/* Toggle menu categories */
$('.main-menu__section-title').on('click', function toggleMenuItems(e) {
 	console.log(e);
	$(e.target).toggleClass('is-active');
});

/* Attach / detach sticky menu */
$(window).on('scroll', function windowScrolled() {
	if ($(this).scrollTop() > 100) {
		$('.header__wrapper .main-menu__toggler').addClass('is-sticky');
	} else {
		$('.header__wrapper .main-menu__toggler').removeClass('is-sticky');
	}  
});
