function loginController(requestVerificationToken) {
	this.addControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				this.hideErrors(triggerElement);
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
					context: this,
					success: function (response) {
						if (response.success) {
							this.showSuccessMessage(triggerElement);
							
							if (successCallback) {
								successCallback(triggerElement);
							}
						}
						else {
							$(triggerElement).removeAttr('disabled');
							
							var specificErrorDisplayed = false;

							if ($.inArray('PasswordMismatch', response.reasons) !== -1)
							{
								this.showError(triggerElement, '.js-reset-password-error-mismatch');
								specificErrorDisplayed = true;
							}
							if ($.inArray('PasswordRequirements', response.reasons) !== -1)
							{
								this.showError(triggerElement, '.js-reset-password-error-requirements');
								specificErrorDisplayed = true;
							}

							if (!specificErrorDisplayed || ($.inArray('MissingToken', response.reasons) !== -1))
							{
								this.showError(triggerElement, '.js-reset-password-error-general');
							}

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function(response) {
						$(triggerElement).removeAttr('disabled');
						
						this.showError(triggerElement, '.js-reset-password-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.showSuccessMessage = function(triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-success-message').show();
	}

	this.showError = function(triggerElement, error) {
		$(triggerElement).parents('.js-reset-password-container').find(error).show();
	}
	
	this.hideErrors = function(triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error').hide();
	}
};

export default loginController;