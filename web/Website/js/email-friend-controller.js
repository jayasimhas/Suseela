function emailFriendController() {
	this.addControl = function(triggerElement, successCallback, failureCallback) {
		if (triggerElement) {
			$(triggerElement).on('click', (event) => {
				var inputData = {};
				var url = $(triggerElement).data('email-friend');
				//var redirectUrl = $(triggerElement).data('login-redirect-url');

				$(triggerElement).parents('.js-pop-out__email-article').find('input').each(function() {
					inputData[$(this).attr('name')] = $(this).val();
				});

				//console.log(requestVerificationToken);

				$.post(url, inputData, function (response) {
					if (response.success) {
						alert("Email Sent");
						if (successCallback) {
							successCallback(triggerElement);
						}

						//window.location.href = redirectUrl;
					}
					else {
						alert("Email Not Sent");
						//if (response.redirectUrl) {
						//	window.location.href = response.redirectUrl;
						//}
						//else {
						//	if (failureCallback) {
						//		failureCallback(triggerElement);
						//	}
						//}
					}
				});
			});
		}
	}
}

export default emailFriendController;