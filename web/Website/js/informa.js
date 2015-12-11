import './vendor';

$('.main-menu__toggler').on('click', function toggleMenu() {
	$('.main-menu').toggleClass('is-active');
	$('.main-menu__toggler').toggleClass('is-active');
});