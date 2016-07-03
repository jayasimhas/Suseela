/* global angular */
function lightboxModalController() {

    this.closeLightboxModal = function() {
        $('body').removeClass('lightboxed');
        $('.lightbox-modal__backdrop').remove();
        $('.lightbox-modal').hide();
    };

    var closeLightboxModal = this.closeLightboxModal;

    this.showLightbox = function(lightbox) {
        // Freeze the page and add the dark overlay
        $('body')
            .addClass('lightboxed')
            .append('<div class="lightbox-modal__backdrop"></div>');

        // Find the specific modal for this trigger, and the associated form
        var targetModal = $(lightbox).data('lightbox-modal');
        var successForm = $(lightbox).closest('.' + $(lightbox).data('lightbox-modal-success-target'));

        // Show the modal, add an on-click listener for the "success" button
        $('.' + targetModal)
            .show()
            .find('.js-lightbox-modal-submit')
            // .one, not .on, to prevent stacking event listners
            .one('click', function(e) {
                successForm.find('button[type=submit]').click();
                closeLightboxModal();
            });

        return false;
    };

    var showLightbox = this.showLightbox;

    this.buildLightboxes = function() {
        $('.js-lightbox-modal-trigger').on('click', function(e) {

            if (e.target !== this) {
                this.click();
                return;
            }

            showLightbox(e.target);

            // Don't submit any forms for real.
            return false;
        });
    };

    // When the Dismiss button is clicked...
    $('.js-close-lightbox-modal').on('click', function(e) {
        closeLightboxModal();
    });

    this.buildLightboxes();

	this.clearLightboxes = function() {
		$('.js-lightbox-modal-trigger').off();
	};

}

export default lightboxModalController;
