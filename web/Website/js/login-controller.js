function loginController(requestVerificationToken) {
	this.addControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				var inputData = {};
				var url = $(triggerElement).data('login-url');
				var redirectUrl = $(triggerElement).data('login-redirect-url');

				$(triggerElement).parents('.js-login-container').find('input').each(function() {

					var value = '';
					var field = $(this);

					if (field.data('checkbox-type') === 'boolean') {
						value = field.attr('checked') || field.attr('checked') === 'checked';

						if (field.data('checkbox-boolean-type') === 'reverse') {
							value = !value;
						}
					}
					else {
						value = field.val();
					}

					inputData[field.attr('name')] = value;
				});

				console.log(requestVerificationToken);

				$.post(url, inputData, function (response) {
					if (response.success) {
						if (successCallback) {
							successCallback(triggerElement);
						}

						window.location.href = redirectUrl;
					}
					else {
						if (response.redirectUrl) {
							window.location.href = response.redirectUrl;
						}
						else {
							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					}
				});
			});
		}
	}
}

export default loginController;