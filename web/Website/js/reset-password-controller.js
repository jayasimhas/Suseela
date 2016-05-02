import { analyticsEvent } from './analytics-controller';

function loginController(requestVerificationToken) {
	this.addRequestControl = function(triggerElement, successCallback, failureCallback) {
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
							var resetPasswordAnalytics = {
							    event_name: "password reset unsuccessful"						       
							};

							var specificErrorDisplayed = false;

							if ($.inArray('EmailRequirement', response.reasons) !== -1)
							{
								this.showError(triggerElement, '.js-reset-password-error-email');
								specificErrorDisplayed = true;
                            }

							if (!specificErrorDisplayed)
							{
								this.showError(triggerElement, '.js-reset-password-error-general');
							}
							
							analyticsEvent( $.extend(analytics_data, resetPasswordAnalytics) );

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

	this.addChangeControl = function(triggerElement, successCallback, failureCallback) {
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

	this.addRetryControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				this.hideErrors(triggerElement);
				$(triggerElement).attr('disabled', 'disabled');

				var inputData = {};
				var url = $(triggerElement).data('retry-url');

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

							this.showError(triggerElement, '.js-reset-password-error-general');

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
	    $(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-success').show();
	    var resetPasswordAnalytics = {
	        event_name: "password reset success"						       
	    };

	    analyticsEvent( $.extend(analytics_data, resetPasswordAnalytics) );
	}

	this.showError = function(triggerElement, error) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-container').show();
		$(triggerElement).parents('.js-reset-password-container').find(error).show();
		var resetPasswordAnalytics = {
		    event_name: "password reset unsuccessful"	
         };

		analyticsEvent( $.extend(analytics_data, resetPasswordAnalytics) );
	}
	
	this.hideErrors = function(triggerElement) {
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error-container').hide();
		$(triggerElement).parents('.js-reset-password-container').find('.js-reset-password-error').hide();
	}
};

export default loginController;