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
	    
		$(formSubmit).on('click', (event) => {

			var currentForm;
			if(event.target.form) {
				currentForm = event.target.form;
			} else {
				currentForm = $(event.target).closest('form');
			}

			if(opts.beforeRequest) {
				opts.beforeRequest();
			}

			event.preventDefault(); // Prevent form submitting

            hideErrors(currentForm); // Reset any visible errors

            // Prevent user from re-submitting form
			$(formSubmit).attr('disabled', 'disabled');

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
				type: $(currentForm).data('submit-type') || 'POST',
				data: inputData,
				context: this,
				success: function (response) {
					if (response.success) {

                        showSuccessMessage(currentForm);

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
                }

			});

			return false;
		});
	})();
}

export default formController;
