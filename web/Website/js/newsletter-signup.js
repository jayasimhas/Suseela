function newsletterSignupController() {

    this.checkForUserSignedUp = function(){
        //$post('/api/PreferencesApi/IsUserSignedUp', function(response)
        //{
        //    if(response.success)
        //    {
              
        //    }
        //}
    
        //};

    this.addControl = function(triggerElement, successCallback, failureCallback) {
        if (triggerElement) {
            $(triggerElement).on('click', (event) => {
                var inputData = {};
                var url = $(triggerElement).data('login-url');
                var redirectUrl = $(triggerElement).data('login-redirect-url');

                $(triggerElement).parents('.js-login-container').find('input').each(function() {
                    inputData[$(this).attr('name')] = $(this).val();
                })

                console.log();

                $.post(url, inputData, function (response) {
                    if (response.success) {
                        if (successCallback) {
                            successCallback(triggerElement);
                        }

                        window.location.href = redirectUrl;
                    }
                    else {
                        if (response.redirectUrl) {
                            window.location.href = response.redirectUrl;
                        }
                        else {
                            if (failureCallback) {
                                failureCallback(triggerElement);
                            }
                        }
                    }
                });
            });
        }
    }
}

export default loginController;