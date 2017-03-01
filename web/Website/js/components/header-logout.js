(function (argument) {
	// body...

	$('.header_salesforce_sign-in-out').on('click', function (e) {
		e.preventDefault();
		var IframeUrl = $(this).attr('data-logout-url');
		var RelocateUrl = $(this).attr('data-redirect-url');

		$('#hiddenforms_logout form').each(function() {
            $(this)[0].submit();
        });
        
		$(document.body).append('<iframe width="1000" height="1000" src="' + IframeUrl + '" frameborder="0"></iframe>');
		// var openedWindow = window.open('https://ideqa-informabi.cs82.force.com/agribusiness/secur/logout.jsp', 'popup', 'width=0,height=0,scrollbars=no');
		// openedWindow.close();
		// localStorage.setItem('RelocateUrl', RelocateUrl);

		window.location.href = RelocateUrl;
		// window.location.href = RelocateUrl;
	});

	// $(document).ready()
})();