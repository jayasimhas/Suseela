function loginController(requestVerificationToken) {
	this.addControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				this.hideErrorMessage(triggerElement);
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
							//this.showSuccessMessage(triggerElement);
							
							if (successCallback) {
								successCallback(triggerElement);
							}
						}
						else {
							$(triggerElement).removeAttr('disabled');
							
							//this.showErrorMessage(triggerElement);

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function(response) {
						$(triggerElement).removeAttr('disabled');
						
						//this.showErrorMessage(triggerElement);

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	}

	this.showErrorMessage = function(triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-message').show();
	}

	this.hideErrorMessage = function(triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-message').hide();
	}

	this.showSuccessMessage = function(triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-success-message').show();
	}
}

export default loginController;