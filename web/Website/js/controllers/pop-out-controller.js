function popOutController(triggerElm) {

	// Toggle pop-out when trigger is clicked
    if(triggerElm) {
        $(triggerElm).off();
		$(triggerElm).on('click', (event) => {
			event.preventDefault();
			this.togglePopOut($(event.target));
		});
	}

	// Reposition pop-out when browser window resizes
	$(window).on('resize', (event) => {
		this.updatePopOut();
	});

	// Simulate CSS `rem` (16px)
	// TODO: change this from `rem` to tab padding value for clarity
	var rem = 16;

	// Keep track of the active pop-out element
	// This is an object instead of a var because there might be more "global"
	// state attributes to track in the future.
	var state = {
		activeElm: null,
		customized: {

		}
	};

	// PUBLIC
	// Get the current pop-out element, if there is one.
	// Lets other JS know what's up with the pop-out.
	this.getPopOutElement = function() {
		return state.activeElm;
	};

	// PUBLIC
	// Closes the pop-out.
	this.closePopOut = function(elm) {
		// Reset all z-indexes so new pop-outs are stacked on top properly
		$('.pop-out').removeClass('is-active').css("z-index", "");
		$('.js-pop-out-trigger').css("z-index", "");
	};

	// PUBLIC
	// Toggles the pop-out
	this.togglePopOut = function(e) {
		// Check if clicked element is the toggle itself
		// Otherwise, climb up DOM tree and find it
		var poParent = e.hasClass('js-pop-out-trigger') ? e : e.closest('.js-pop-out-trigger');

		/*  This is a little hacky, but if a user is trying to bookmark an article
			but needs to sign in first, we need to capture and pass the article
			ID as a URL param after a successful sign in attempt. That allows
			us to automatically bookmark the article on page refresh. */

		if(poParent.data('pop-out-type') === 'sign-in' && poParent.data('bookmark-id')) {
			$('.sign-in__submit').data('pass-article-id', poParent.data('bookmark-id'));
		} else {
			poParent.data('bookmark-id', null);
		}

		// Close all pop-outs
		this.closePopOut();

		if(poParent[0] !== state.activeElm) {
			// Update the controller state and open it
			state.activeElm = poParent[0];
			updatePosition();
		} else {
			state.activeElm = null;
		}

	};


	// PUBLIC
	// Stores pop-out customization details for reference when rendering
	this.customize = function(obj) {
		state.customized[obj.id] = obj;
	};


	// PRIVATE
	// Update the visibility and position of the pop-out box and tab.
	var updatePosition = function() {

		var trgr = { // The pop-out trigger
			e: $(state.activeElm)
		};
		// Get trigger height, width, offsetTop, offsetWidth
		trgr.offset = trgr.e.offset();

		trgr.hasStyles = state.customized[trgr.e.data('pop-out-id')];

		// Determine which pop-out template to use
		// TODO: Make this user-configurable
		// Let users assign a name to a template class
		var popOut;
		switch (trgr.e.data('pop-out-type')) {
			// SIGN IN
			// (Global sign-in, bookmarking when not signed in)
			case 'sign-in':
				popOut = $('.js-pop-out__sign-in');
				break;
			// Main Sign In button on top right
		    case 'sign-in-header':
		        popOut = $('.js-pop-out__sign-in-header');
		        break;
			// EMAIL ARTICLE
			case 'email-article':
				popOut = $('.js-pop-out__email-article');
				break;
			// EMAIL ARTICLE
			case 'email-search':
				popOut = $('.js-pop-out__email-search');
				break;
			// EMAIL AUTHOR
			case 'email-author':
				popOut = $('.js-pop-out__email-author');
				break;
		    // EMAIL COMPANy
		    case 'email-company':
		        popOut = $('.js-pop-out__email-company');
		        break;
		    // EMAIL DEAL
		    case 'email-deal':
		        popOut = $('.js-pop-out__email-deal');
		        break;
			// GLOBAL HEADER REGISTRATION
			case 'register':
				popOut = $('.js-pop-out__register');
				break;
			// GLOBAL HEADER REGISTRATION
			case 'myViewregister':
				popOut = $('.js-pop-out__myViewregister');
				break;	
			// SEARCH PAGE - SAVE SEARCH
			case 'save-search':
				popOut = $('.js-pop-out__save-search');
				break;
			default:
				console.warn('Attempting to fire unidentified pop-out.');
				return;
		}


		// Make pop-out visible so we can query for its width
		popOut.addClass('is-active');

		// Check if browser is less than or equal to `small` CSS breakpoint

		var isNarrow = $(window).width() <= 480;
		var isTablet = $(window).width() <= 800;

		// Set separate vertical/horizontal padding on mobile vs. desktop
		var vPad = isNarrow ? 10 : rem;
		var hPad = isNarrow ? 14 : rem;

		// Store output values after calculations, etc.
		var res = {
			offset: {
				box: {},
				tab: {}
			},
			css: {
				box: {},
				tab: {}
			}
		};

		// Box offset top is offsetTop of trigger, plus trigger height,
		// plus padding, minus 1px for border positioning
		res.offset.box.top = Math.floor(trgr.offset.top + trgr.offset.height + (vPad - 1));

		// Check if pop-out will bleed off-screen, causing horizontal scroll bar
		// If it will, force right-align to keep it on-screen
		if(popOut.width() + trgr.offset.left > $(window).width()) {
			trgr.e.data('pop-out-align', 'right');
		}

		// Check for pop-out alignment
		if(trgr.e.data('pop-out-align') === 'right' && !isNarrow) {
			// Pop-out box is flush right with trigger element
			// To flush right, first add trigger offset plus trigger width
			// This positions left edge of pop-out with right edge of trigger
			// Then subtract pop-out width and padding to align both right edges
			// (Flush-left automatically if narrow window)
			res.offset.box.left = isNarrow ? 0 : Math.floor(trgr.offset.left + trgr.offset.width - popOut.offset().width + (hPad - 1));
			// Tab left margin can be ignored, right margin 0 does what we need
			res.offset.tab.left = 'auto';
		} else {
			// Pop-out box is centered with trigger element
			// Box offset left is determined by subtracting the trigger width
			// from the pop-out width, dividing by 2 to find the halfway point,
			// then subtracting that from the trigger left offset.
			// (Flush-left automatically if narrow window)
			res.offset.box.left = isNarrow ? 0 : Math.floor(trgr.offset.left - ((popOut.offset().width - trgr.offset.width) / 2));
			// Pop-out tab is aligned with trigger left edge, adjusted for padding
			// Tab width is set to trigger width below, so this centers the tab
			res.offset.tab.left = isNarrow ? Math.floor(trgr.offset.left - hPad) : 0;
		}

		// Blow up z-index to appear above other triggers
		trgr.e.css('z-index', '9999');

		// Box z-index set to 2 lower than trigger element
		// Box should render below trigger, under tab, above everything else
		res.css.box.zIndex = trgr.e.css('z-index') - 2;

		// Tab height equals trigger height plus padding (1rem top and bottom)

		// Check for custom tab styles
		var tS = trgr.hasStyles ? trgr.hasStyles.tabStyles : undefined;

		// If there are custom styles, and browser is desktop-width...
		if(tS && !isNarrow && !isTablet) {

			res.css.tab.height = tS.deskHeight || trgr.offset.height + (vPad * 2) + "px";

			tS.deskHeight
				? res.offset.box.top += tS.deskHeight - trgr.offset.height - (vPad * 2)
				: null;

			res.css.tab.top = tS.deskHeight
				? '-' + (tS.deskHeight - 1) + 'px'
				: '-' + (trgr.offset.height + (vPad * 2) - 1) + 'px';

		// If there are custom styles, and browser is tablet-width...
		} else if(tS && !isNarrow && isTablet) {

			res.css.tab.height = tS.tabletHeight || trgr.offset.height + (vPad * 2) + "px";

			tS.tabletHeight
				? res.offset.box.top += tS.tabletHeight - trgr.offset.height - (vPad * 2)
				: null;

			res.css.tab.top = tS.tabletHeight
				? '-' + (tS.tabletHeight - 1) + 'px'
				: '-' + (trgr.offset.height + (vPad * 2) - 1) + 'px';

		// If there are custom styles, and browser is phone-width...
		} else if(tS && isNarrow) {

			res.css.tab.height = tS.phoneHeight || trgr.offset.height + (vPad * 2) + "px";

			tS.phoneHeight
				? res.offset.box.top += tS.phoneHeight - trgr.offset.height - (vPad * 2)
				: null;

			res.css.tab.top = tS.phoneHeight
				? '-' + (tS.phoneHeight - 1) + 'px'
				: '-' + (trgr.offset.height + (vPad * 2) - 1) + 'px';

		// Default padding/positioning
		} else {

			res.css.tab.height = trgr.offset.height + (vPad * 2) + "px";

			// Move the tab upwards, equal to the trigger height plus padding
			// minus 1px to account for border and visually overlapping box
			res.css.tab.top = '-' + (trgr.offset.height + (vPad * 2) - 1) + "px";
		}

		// Tab width equals trigger width plus padding (1rem left and right)
		res.css.tab.width = trgr.offset.width + (hPad * 2) + "px";

		// Tab z-index is 1 less than trigger; above box, below trigger
		res.css.tab.zIndex = trgr.e.css('z-index') - 1;

		// `transform` to quickly position box, relative to top left corner
		res.css.box.transform = 'translate3d(' + res.offset.box.left +'px, ' + res.offset.box.top + 'px, 0)';

		// Apply that giant blob of CSS
		popOut.css({
			zIndex: res.css.box.zIndex,
			transform: res.css.box.transform
		}).find('.pop-out__tab').css({ // find this pop-out's child tab
			height: res.css.tab.height,
			width: res.css.tab.width,
			left: res.offset.tab.left,
			right: 0, // This is always 0
			top: res.css.tab.top,
			zIndex: res.css.tab.zIndex
		});
		// Ugly hack for Safari 8, booo
		popOut.css('-webkit-transform', res.css.box.transform);

	};

	// If there is an active pop-out, update its position
	// Mostly useful for when the browser window resizes
	this.updatePopOut = function() {
		if(state.activeElm) {
			updatePosition();
		}
	};
}

export default popOutController;
