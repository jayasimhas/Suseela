function lightboxModalController() {

    $('.js-lightbox-modal-trigger').on('click', function(e) {

        // Freeze the page and add the dark overlay
        $('body')
            .addClass('lightboxed')
            .append('<div class="lightbox-modal__backdrop"></div>');

        // Find the specific modal for this trigger, and the associated form
        var targetModal = $(e.target).data('lightbox-modal');
        var successForm = $(e.target).closest('.' + $(e.target).data('lightbox-modal-success-target'));

        // Show the modal, add an on-click listener for the "success" button
        $('.' + targetModal)
            .show()
            .find('.js-lightbox-modal-submit')
            .on('click', function(f) {
                successForm.find('button[type=submit]').click();
                closeLightboxModal();
            });

        // Don't submit any forms for real.
        return false;
    });

    // When the Dismiss button is clicked...
    $('.js-close-lightbox-modal').on('click', function(e) {
        closeLightboxModal();
    });


    var closeLightboxModal = function() {
        $('body').removeClass('lightboxed');
        $('.lightbox-modal__backdrop').remove();
        $('.lightbox-modal').hide();
    };
    
}

export default lightboxModalController;
