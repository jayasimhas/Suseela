import { analyticsEvent } from './analytics-controller';

function loginController(requestVerificationToken) {
	this.addRegisterUserControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				this.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('register-user-url');

				$(triggerElement).parents('.js-register-user-container').find('input').each(function() {
					var value = '';

					if ($(this).data('checkbox-type') === 'boolean') {
						value = this.checked;

						if ($(this).data('checkbox-boolean-type') === 'reverse') {
							value = !value;
						}
					}
					else {
						value = $(this).val();
					}

					inputData[$(this).attr('name')] = value;
				});

				$.ajax({
					url: url,
					type: 'POST',
					data: inputData,
					context: this,
					success: function (response) {
						if (response.success) {

							var registerAnalytics = {
								event_name: 'register-step-1',
								registration_state: 'successful',
								userName: '"' + inputData.username + '"'
							};

							analyticsEvent( $.extend(analytics_data, registerAnalytics) );

							if (successCallback) {
								successCallback(triggerElement);
							}

							var nextStepUrl = $(triggerElement).data('next-step-url');

							if (nextStepUrl) {
								window.location.href = nextStepUrl;
							}

							this.showSuccessMessage(triggerElement);
						}
						else {
							$(triggerElement).removeAttr('disabled');



							var specificErrorDisplayed = false;

							if (response.reasons && response.reasons.length > 0) {
								for (var reason in response.reasons) {
									this.showError(triggerElement, '.js-register-user-error-' + response.reasons[reason]);
								}

								specificErrorDisplayed = true;
							}

							if (!specificErrorDisplayed)
							{
								this.showError(triggerElement, '.js-register-user-error-general');
							}

							var registerAnalytics = {
								event_name: "registration failure",
								registration_errors: response.reasons
							};

							analyticsEvent( $.extend(analytics_data, registerAnalytics) );

							if (failureCallback) {
								failureCallback(triggerElement);
							}
						}
					},
					error: function(response) {
						$(triggerElement).removeAttr('disabled');

						this.showError(triggerElement, '.js-register-user-error-general');

						if (failureCallback) {
							failureCallback(triggerElement);
						}
					}
				});
			});
		}
	};

	this.showSuccessMessage = function(triggerElement) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-success').show();
	}

	this.showError = function(triggerElement, error) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error-container').show();
		$(triggerElement).parents('.js-register-user-container').find(error).show();
	}

	this.hideErrors = function(triggerElement) {
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error-container').hide();
		$(triggerElement).parents('.js-register-user-container').find('.js-register-user-error').hide();
	}
};

export default loginController;
