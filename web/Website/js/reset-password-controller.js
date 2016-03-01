function loginController(requestVerificationToken) {
	this.addControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-message').hide();
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('reset-url');

				$(triggerElement).parents('.js-reset-password-container').find('input').each(function() {
					inputData[$(this).attr('name')] = $(this).val();
				})

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					success: function (response) {
						if (response.success) {
							$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-success-message').show();
							
							if (successCallback) {
								successCallback(triggerElement);
							}
						}
						else {
							$(triggerElement).removeAttr('disabled');
							$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-message').show();

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function(response) {
						$(triggerElement).removeAttr('disabled');
						
						$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-message').show();

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};
};

export default loginController;