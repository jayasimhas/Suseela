function loginController(requestVerificationToken) {
	this.addControl = function(triggerForm, successCallback, failureCallback) {

		var triggerElement = $(triggerForm).find('button[type=submit]');

		if (triggerForm) {
			$(triggerForm).on('submit', (event) => {

				event.preventDefault();

				var inputData = {};
				var url = $(triggerElement).data('login-url');
				

				$(triggerElement).parents('.js-login-container').find('input').each(function() {

					var value = '';
					var field = $(this);

					if (field.data('checkbox-type') === 'boolean') {
						value = this.checked;

						if (field.data('checkbox-boolean-type') === 'reverse') {
							value = !value;
						}
					}
					else {
						value = field.val();
					}

					inputData[field.attr('name')] = value;
				});

				$.post(url, inputData, function (response) {

					if (response.success) {
						if (successCallback) {
							successCallback(triggerElement);
						}

						if($(triggerElement).data('login-redirect-url')) {
							window.location.href = $(triggerElement).data('login-redirect-url');
						} else {
							window.location.reload(false);
						}
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

				return false;
			});
		}
	}
}

export default loginController;
