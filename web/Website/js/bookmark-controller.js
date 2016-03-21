function bookmarkController() {

    // * * *
    //  Article bookmarking logic goes here
    // * * *
    this.toggle = function(e) {

        var bookmark = {
            elm: $(e)
        };

        // ID of the article we're bookmarking or un-bookmarking
        bookmark.id = bookmark.elm.closest('.js-bookmark-article').data('bookmark-id');

        // Stash the bookmark icon now, update with the right icon later
        bookmark.icon = bookmark.elm.find('use')[0];

        // Stash the bookmark label data now, swap label text later
        bookmark.label = {
            elm: bookmark.elm.find('.js-bookmark-label')
        };
        bookmark.label.bookmark = bookmark.label.elm.data('label-bookmark');
        bookmark.label.bookmarked = bookmark.label.elm.data('label-bookmarked');


        // Are we bookmarking an article, or un-bookmarking?
        // Used later to know what API endpoint to hit, and what DOM changes are required
        bookmark.isBookmarking = bookmark.elm.data('is-bookmarked') ? false : true;

        var apiEndpoint = bookmark.isBookmarking ?
            '/Account/api/SavedDocumentApi/SaveItem/' :
            '/Account/api/SavedDocumentApi/RemoveItem/';

        if(bookmark.id) {
            $.ajax({
                url: apiEndpoint,
                type: 'POST',
                data: {
                    DocumentID: bookmark.id
                },
                context: this,
                success: function (response) {
                    if (response.success) {

                        if(bookmark.isBookmarking) {

                            bookmark.icon.setAttributeNS(
                                'http://www.w3.org/1999/xlink',
                                'href',
                                '/dist/img/svg-sprite.svg#bookmarked');
                            bookmark.elm.data('is-bookmarked', true);
                            bookmark.label.elm.html(bookmark.label.bookmarked);

                        } else {

                            bookmark.icon.setAttributeNS(
                                'http://www.w3.org/1999/xlink',
                                'href',
                                '/dist/img/svg-sprite.svg#bookmark');
                            bookmark.elm.data('is-bookmarked', null);
                            bookmark.label.elm.html(bookmark.label.bookmark);

                        }

                    }
                    else {

                    }
                },
                error: function(response) {
                },
                complete: function() {
                }
            });

        }
    };
}

export default bookmarkController;
