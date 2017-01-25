(function (argument) {
	// body...

	$(document).on('click', '.header_salesforce_sign-in-out', function(e) {
		e.preventDefault();
		debugger;
		var IframeUrl = $(this).attr('data-logout-url');
		var RelocateUrl = $(this).attr('data-redirect-url');
		 popup = window.open(IframeUrl, "popup", "status=1,width=0,height=0");
	     popup.close();
	     window.location.href=RelocateUrl;
	});
})();