/*

opts.observe — Form element(s) to observe
opts.beforeRequest — Function to execute before making Ajax request
opts.successCallback — If Ajax request is successful, callback
opts.failureCallback — If Ajax request fails / returns false, callback

*/

function formController(opts) {

	var showSuccessMessage = function(form) {
		$(form).find('.js-form-success').show();
	};

	var showError = function(form, error) {
		if($(form).find(error)) {
			$(form).find(error).show();
		}
	};

	var hideErrors = function(form) {
		$(form).find('.js-form-error').hide();
	};

	(function init() {

		var form = opts.observe;

		if (!form) return false;

		var formSubmit = $(form).find('button[type=submit]');

		$(formSubmit).on('click', function(event) {

			// Some forms will require user confirmation before action is taken
			// Default to true (confirmed), set to false later if confirmation is
			// required and user cancels action
			var actionConfirmed = true;

			var currentForm;
			if(event.target.form) {
				currentForm = event.target.form;
			} else {
				currentForm = $(event.target).closest('form');
			}

			if($(currentForm).data('force-confirm')) {
				actionConfirmed = window.confirm($(currentForm).data('force-confirm'));
			}

			if(actionConfirmed) {

				event.preventDefault(); // Prevent form submitting

				hideErrors(currentForm); // Reset any visible errors

				if(opts.beforeRequest) {
					opts.beforeRequest(currentForm);
				}

				// Prevent user from re-submitting form, unless explicitly allowed
				if(!$(currentForm).data('prevent-disabling')) {
					$(formSubmit).attr('disabled', 'disabled');
				}

				var inputData = {};
				var IsValid = true;//Skip Validation if the form is not Update Contact Informatin Form
				if($(currentForm).hasClass('form-update-account-contact'))
				{
					IsValid = ValidateContactInforForm();
				}
				if(IsValid){
					$(currentForm).find('input, select, textarea').each(function() {

						var value = '';
						var field = $(this);

						if (field.data('checkbox-type') === 'boolean') {
							value = this.checked;

							if (field.data('checkbox-boolean-type') === 'reverse') {
								value = !value;
							}
						} else if (field.data('checkbox-type') === 'value') {
							value = this.checked ? field.val() : undefined;
						} else {
							value = field.val();
						}

						if (value !== undefined) {
							if (inputData[field.attr('name')] === undefined) {
								inputData[field.attr('name')] = value;
							}
							else if ($.isArray(inputData[field.attr('name')])) {
								inputData[field.attr('name')].push(value);
							}
							else {
								inputData[field.attr('name')] = [ inputData[field.attr('name')] ];
								inputData[field.attr('name')].push(value);
							}
						}
					});

					// add recaptcha if it exists in the form
					var captchaResponse = grecaptcha.getResponse();
					if (captchaResponse !== undefined)
						inputData['RecaptchaResponse'] = captchaResponse;

					if(!$(currentForm).data('on-submit')) {
						console.warn('No submit link for form');
					}
				    try{
				        for(var index in inputData)
				        {
				            if(inputData[index] == "- Select One -")
				            {
				                inputData[index] = "";
				            }
				        }
				    }catch(ex){console.log(ex);}

					$.ajax({
						url: $(currentForm).data('on-submit'),
						type: $(currentForm).data('submit-type') || 'POST',
						data: inputData,
						context: this,
						success: function (response) {
							if (response.success) {

								showSuccessMessage(currentForm);

								// Passes the form response through with the "context"
								// successCallback is ripe for refactoring, improving parameters
								this.response = response;

								if (opts.successCallback) {
									opts.successCallback(currentForm, this, event);
								}

								if($(form).data('on-success')) {
									window.location.href = $(currentForm).data('on-success');
								}
							}
							else {
								if (response.reasons && response.reasons.length > 0) {
									for (var reason in response.reasons) {
										showError(form, '.js-form-error-' + response.reasons[reason]);
									}
								} else {
									showError(currentForm, '.js-form-error-general');
								}

								if (opts.failureCallback) {
									opts.failureCallback(currentForm,response);
								}
							}
						},
						error: function(response) {

							showError(currentForm, '.js-form-error-general');

							if (opts.failureCallback) {
								opts.failureCallback(currentForm,response);
							}
						},
						complete: function() {
							setTimeout((function() {
								$(formSubmit).removeAttr('disabled');
							}), 250);							

							// reset captcha if available
							grecaptcha.reset();
						}

					});
			
				} // if actionConfirmed
			}
			return false;

		});
	})();
}
function ValidateContactInforForm() {
	var errorHtml = $('#errorMessage').html();
	var errors = 0;
	var result = false;
	var scrollTo = '';
	$('.required').each(function () {
            if ($(this).val() == '' || $(this).text().indexOf("- Select One -") >= 0) {
			$(this).parent().append(errorHtml);
			errors++;
			if(errors==1)
			{
				scrollTo = $(this);
			}
		}
		else {
			$(this).parent().find('.js-form-error').remove();
		}

	});
	if (errors > 0) {
		window.scrollTo(0,scrollTo.offset().top-30);
		result = false;
	}
	else {
		result = true;
	}
	return result;
}

export default formController;
