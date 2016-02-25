function loginController(requestVerificationToken) {
	this.addControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				var inputData = $(triggerElement).parent('.js-login-container').serialize();
				var url = $(triggerElement).data('login-url');
				var redirectUrl = $(triggerElement).data('login-redirect-url');

				console.log(requestVerificationToken);

				// Login
				$.post(url, inputData, function (response) {
					if (response.success) {
						if (successCallback) {
							successCallback();
						}

						console.log(response.message);
						console.log(response.username);

						// Redirect
						//window.location.href = redirectUrl;
					}
					else {
						if (failureCallback) {
							failureCallback();
						}

						console.log(response.message);
						console.log(response.username);
					}
				});
			});
		}
	}
}

export default loginController;