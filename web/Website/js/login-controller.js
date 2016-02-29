function loginController(requestVerificationToken) {
	this.addControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				var inputData = {};
				var url = $(triggerElement).data('login-url');
				var redirectUrl = $(triggerElement).data('login-redirect-url');

				$(triggerElement).parents('.js-login-container').find('input').each(function() {
					inputData[$(this).attr('name')] = $(this).val();
				})

				console.log(requestVerificationToken);

				$.post(url, inputData, function (response) {
					if (response.success) {
						if (successCallback) {
							successCallback(triggerElement);
						}

						window.location.href = redirectUrl;
					}
					else {
						if (failureCallback) {
							failureCallback(triggerElement);
						}

						console.log(response.message);
					}
				});
			});
		}
	}
}

export default loginController;