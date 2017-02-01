(function (argument) {
	// body...

	$(document).on('click', '.header_salesforce_sign-in-out', function(e) {
		e.preventDefault();
		var IframeUrl = $(this).attr('data-logout-url');
		var RelocateUrl = $(this).attr('data-redirect-url');
		$(document.body).append('<iframe width="1000" height="1000" src="' + IframeUrl + '" frameborder="0"></iframe>');
		window.location.href = RelocateUrl;
	});
})();