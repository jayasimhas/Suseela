var BookmarkCatche = {
	init: function () {
		var Items = $('.article-preview__bookmarker');

		$.each(Items, function (index, item) {
			var $this = $(this);
			var documentId = $(this).attr('data-bookmark-id');
			
			$.ajax({
                url: '/Account/api/SavedDocumentApi/IsSaved/',
                type: 'POST',
                data: {
                    DocumentID: documentId
                },
                context: this,
                success: function (response) {
                    if (response.success) {
                        $this.attr('data-is-bookmarked', true);
                        $this.find('.article-bookmark').removeClass('is-visible');
                        $this.find('.article-bookmark__bookmarked').addClass('is-visible');
                        $this.find('.js-bookmark-label').html('Bookmarked');
                    }
                },
                error: function(response) {
                    return false;
                }
            });

        });
    }
}

$(document).ready(function () {
  if(window.location.pathname == "/") {
  	BookmarkCatche.init();
  }
})