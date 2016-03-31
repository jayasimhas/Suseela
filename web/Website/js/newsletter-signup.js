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
                        var newsletterAnalytics = {"event_name":"newsletter-signup","newsletter_signup_state":"successful","userName":'"'+inputData+'"'};
                        var result ={};
                        $.extend(result,analytics_data,newsletterAnalytics);
                        //  utag.link({
                        //    result
                        //});

                        $(".newsletter-signup-before-submit").hide();
                        $(".newsletter-signup-after-submit").show();                    
                    }
                    else
                    {
                        var newsletterAnalytics = {"event_name":"newsletter-signup","newsletter_signup_state":"unsuccessful","userName":'"'+inputData+'"'};
                        var result ={};
                        $.extend(result,analytics_data,newsletterAnalytics);
                        //  utag.link({
                        //    result
                        //});

                    }
                  
                });
            });
        }
    }


}

export default newsletterSignupController;
