function formController(requestVerificationToken) {
	this.watchForm = function(form, successCallback, failureCallback, presubmit) {

        if (!form) return false;

        var formSubmit = $(form).find('button[type=submit]');

		$(formSubmit).on('click', (event) => {

			var currentForm;
			if(event.target.form) {
				currentForm = event.target.form;
			} else {
				currentForm = $(event.target).closest('form');
			}

			if(presubmit) {
				presubmit();
			}

			event.preventDefault(); // Prevent form submitting

            this.hideErrors(currentForm); // Reset any visible errors

            // Prevent user from re-submitting form
			$(event.target).attr('disabled', 'disabled');

			var inputData = {};

			$(currentForm).find('input, select, textarea').each(function() {

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

			if(!$(currentForm).data('on-submit')) {
				console.warn('No submit link for form');
			}

			$.ajax({
				url: $(currentForm).data('on-submit'),
				type: 'POST',
				data: inputData,
				context: this,
				success: function (response) {
					if (response.success) {

                        this.showSuccessMessage(currentForm);

						if (successCallback) {
							successCallback(currentForm, this, event);
						}

                        if($(form).data('on-success')) {
                            window.location.href = $(currentForm).data('on-success');
                        }
					}
					else {
						if (response.reasons && response.reasons.length > 0) {
							for (var reason in response.reasons) {
								this.showError(form, '.js-form-error-' + response.reasons[reason]);
							}
						} else {
                            this.showError(currentForm, '.js-form-error-general');
						}

						if (failureCallback) {
							failureCallback(currentForm);
						}
					}
				},
				error: function(response) {

					this.showError(currentForm, '.js-form-error-general');

					if (failureCallback) {
						failureCallback(currentForm);
					}
				},
                complete: function() {
                    $(formSubmit).removeAttr('disabled');
                }

			});

			return false;
		});
	};

	this.showSuccessMessage = function(form) {
		$(form).find('.js-form-success').show();
	};

	this.showError = function(form, error) {
		if($(form).find(error)) {
			$(form).find(error).show();
		}
	};

	this.hideErrors = function(form) {
		$(form).find('.js-form-error').hide();
	};
}

export default formController;
