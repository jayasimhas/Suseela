import { analyticsEvent } from './analytics-controller';

function loginController(requestVerificationToken) {

	this.addControl = function(triggerForm, successCallback, failureCallback) {

		if (triggerForm) {
			$(triggerForm).on('submit', (event) => {

				var triggerElement = $(event.target).find('button[type=submit]');

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

				        var loginAnalytics =  {
							event_name: 'login',
							login_state: 'successful',
							userName: '"' + inputData.username + '"'
						};

				        analyticsEvent(	$.extend(analytics_data, loginAnalytics) );

						if (successCallback) {
							successCallback(triggerElement);
						}

						if($(triggerElement).data('login-redirect-url')) {
							window.location.href = $(triggerElement).data('login-redirect-url');
							// If Angular, need location.reload to force page refresh
							if(angular) {
								angular.element($('.search-results')[0]).controller().forceRefresh();
							}
						} else {
							window.location.reload(false);
						}
					}
				    else {

				        var loginAnalytics = {
							event_name: "login",
							login_state: "unsuccessful",
							userName: '"' + inputData.username + '"'
						};

				        analyticsEvent( $.extend(analytics_data, loginAnalytics) );


						if (response.redirectUrl) {
							window.location.href = response.redirectUrl;
						}
						else {
							if (failureCallback) {
								failureCallback(event.target);
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
