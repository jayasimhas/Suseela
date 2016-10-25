/* global analytics_data */

import { analyticsEvent } from './controllers/analytics-controller';

function newsletterSignupController() {

    this.checkForUserSignedUp = function(){
        $.get('/Account/api/PreferencesApi/IsUserSignedUp', function(response) {
            var res = response;
            if(response) {
                $(".newsletter-signup").hide();
            }

        });
    };

    this.addControl = function(triggerElement, successCallback, failureCallback) {
        if (triggerElement) {
            $(triggerElement).on('click', (event) => {

                // Prevent form submit
                event.preventDefault();

                // Hide any errors
                $('.js-newsletter-signup-error').hide();

                var inputData = "";
                var url = $(triggerElement).data('signup-url');

                $(triggerElement).parents('.newsletter-signup').find('input').each(function() {
                    inputData = $(this).val();
                });

                url = url + '?userName=' + inputData;

                $.get(url, function(response) {
                    var newsletterAnalytics;

                    if (response == 'true') {

                        newsletterAnalytics = {
                            event_name: 'newsletter-signup',
                            newsletter_signup_state: 'successful',
                            userName: '"' + inputData + '"'
                        };

                        analyticsEvent( $.extend( analytics_data, newsletterAnalytics) );

                        $(".newsletter-signup-before-submit").hide();
                        $(".newsletter-signup-after-submit").show();

                    } else if (response == 'mustregister'){

                        newsletterAnalytics = {
                            event_name: 'newsletter-signup',
                            newsletter_signup_state: 'unsuccessful',
                            userName: '"' + inputData + '"'
                        };

                        analyticsEvent( $.extend(analytics_data, newsletterAnalytics) );

                        $('.newsletter-signup-before-submit').hide();
                        $('.newsletter-signup-needs-registration').show();
                    }
                    else
                    {
                        newsletterAnalytics = {
                            event_name: 'newsletter-signup',
                            newsletter_signup_state: 'unsuccessful',
                            userName: '"' + inputData + '"'
                        };

                        analyticsEvent( $.extend(analytics_data, newsletterAnalytics) );

                        $('.js-newsletter-signup-error').show();
                    }
                });
            });
        }
    };
}

export default newsletterSignupController;
