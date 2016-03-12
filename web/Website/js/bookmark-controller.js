function bookmarkController() {

    // * * *
    //  Article bookmarking logic goes here
    // * * *
    this.toggle = function(e) {

        var flipIcon = function() {
            var svg = $(e).find('use')[0];
            var currentIcon = svg.getAttributeNS('http://www.w3.org/1999/xlink', 'href').split('#')[1];

            if(currentIcon === 'bookmark') {
                svg.setAttributeNS('http://www.w3.org/1999/xlink', 'href', '/dist/img/svg-sprite.svg#bookmarked');
            } else {
                svg.setAttributeNS('http://www.w3.org/1999/xlink', 'href', '/dist/img/svg-sprite.svg#bookmark');
            }
        };

        var apiData = {
            DocumentID: $(e).closest('.js-bookmark-article').data('bookmark-id')
        };

        if(/* IS BOOKMARKED */) {
            var endpoint = '/Account/api/SavedDocumentApi/RemoveItem/';
        } else {
            var endpoint = '/Account/api/SavedDocumentApi/SaveItem/';
        }

        if(apiData.DocumentID) {

            $.ajax({
                url: endpoint,
                type: 'POST',
                data: apiData,
                context: this,
                success: function (response) {
                    if (response.success) {
                        flipIcon();
                    }
                    else {
                        
                        /* if (response.reasons && response.reasons.length > 0) {
                            for (var reason in response.reasons) {
                                this.showError(form, '.js-form-error-' + response.reasons[reason]);
                            }
                        } else {
                            this.showError(form, '.js-form-error-general');
                        }

                        if (failureCallback) {
                            failureCallback(form);
                        } */
                    }
                },
                error: function(response) {

                    /* this.showError(form, '.js-form-error-general');
                    
                    if (failureCallback) {
                        failureCallback(form);
                    } */
                },
                complete: function() {
                    //$(formSubmit).removeAttr('disabled');
                }

            });

        }
    };
}

export default bookmarkController;
