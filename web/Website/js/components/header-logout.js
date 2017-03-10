(function (argument) {
	// body...

	$('.header_salesforce_sign-in-out').on('click', function (e) {
		e.preventDefault();
		var IframeUrl = $(this).attr('data-logout-url');
		var RelocateUrl = $(this).attr('data-redirect-url');

		window.location.href = RelocateUrl;
		// window.location.href = RelocateUrl;
	});

	// $(document).ready()
})();