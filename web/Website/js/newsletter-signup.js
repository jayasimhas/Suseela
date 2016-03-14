function newsletterSignupController() {

    this.checkForUserSignedUp = function(){
        $.post('/api/PreferencesApi/IsUserSignedUp', function(response)
        {
            var res = response;
            if(response)
            {
                $("#newsletter-signup").hide();
            }

        });
    }
    this.addControl = function(triggerElement, successCallback, failureCallback) {
        if (triggerElement) {
            $(triggerElement).on('click', (event) => {
                var inputData = {};
                var url = $(triggerElement).data('signup-url');
                $(triggerElement).parents('.newsletter-signup').find('input').each(function() {
                    inputData[$(this).attr('name')] = $(this).val();
                })

                console.log();

                $.post(url, inputData, function (response) {
                    if (response.success) {
                        $("#newsletter-signup").hide();
                        $("#newsletter-signup-after-submit").show();
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

export default newsletterSignupController;
