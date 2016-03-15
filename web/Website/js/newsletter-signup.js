function newsletterSignupController() {

    this.checkForUserSignedUp = function(){
        $.get('/api/PreferencesApi/IsUserSignedUp', function(response)
        {
            var res = response;
            if(response)
            {
                $(".newsletter-signup").hide();
            }

        });
    }
    this.addControl = function(triggerElement, successCallback, failureCallback) {
        if (triggerElement) {
            $(triggerElement).on('click', (event) => {
                event.preventDefault();
                var inputData ="";
                var url = $(triggerElement).data('signup-url');
                $(triggerElement).parents('.newsletter-signup').find('input').each(function() {
                    inputData = $(this).val();
                })
                url = url + "?userName="+inputData;
                $.get(url, function(response) {
                    if (response) {
                        $(".newsletter-signup-before-submit").hide();
                        $(".newsletter-signup-after-submit").show();
                    }
                  
                });
            });
        }
    }


}

export default newsletterSignupController;
