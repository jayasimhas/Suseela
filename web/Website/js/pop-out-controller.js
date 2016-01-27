function popOutController(triggerElm) {

	// Toggle pop-out when trigger is clicked
	if(triggerElm) {
		$(triggerElm).on('click', (event) => {
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
		activeElm: null
	};

	// PUBLIC
	// Get the current pop-out element, if there is one.
	// Lets other JS know what's up with the pop-out.
	this.getPopOutElement = function() {
		return state.activeElm;
	};

	// PUBLIC
	// Closes the pop-out.
	this.closePopOut = function() {
		$('.pop-out').removeClass('is-active');
		state.activeElm = null;
	}

	// PUBLIC
	// Toggles the pop-out
	this.togglePopOut = function(e) {
		// Check if clicked element is the toggle itself
		// Otherwise, climb up DOM tree and find it
		var poParent = e.hasClass('js-pop-out-trigger') ? e : e.closest('.js-pop-out-trigger');

		// If the current pop-out is the same as the active pop-out...
		if(poParent[0] === state.activeElm) {
			// ...close it
			this.closePopOut();
		} else {
			// ...update the controller state and open it
			state.activeElm = poParent[0];
			updatePosition();
		}

	};

	// PRIVATE
	// Update the visibility and position of the pop-out box and tab.
	var updatePosition = function() {

		var trgr = { // The pop-out trigger
			e: $(state.activeElm)
		};
		// Get trigger height, width, offsetTop, offsetWidth
		trgr.offset = trgr.e.offset();

		// Close all open pop-outs
		$('.pop-out').removeClass('is-active');

		// Determine which pop-out template to use
		// TODO: Make this user-configurable
		//			Let users assign a name to a template class
		switch (trgr.e.data('pop-out-type')) {
			// SIGN IN
			// (Global sign-in, bookmarking when not signed in)
			case 'sign-in':
				var popOut = $('.js-pop-out__sign-in');
				break;
			// EMAIL ARTICLE
			case 'email-article':
				var popOut = $('.js-pop-out__email-article');
				break;
			// GLOBAL HEADER REGISTRATION
			case 'register':
				var popOut = $('.js-pop-out__register');
				break;
			default:
				console.warn('Attempting to fire unidentified pop-out.');
				return;
				break;
		}

		// Make pop-out visible so we can query for its width
		popOut.addClass('is-active');

		// Check if browser is less than or equal to `small` CSS breakpoint
		var isNarrow = $(window).width() <= 480;

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
		res.offset.box.top = Math.floor(trgr.offset.top + trgr.offset.height + (rem - 1));

		// Check for pop-out alignment
		if(trgr.e.data('pop-out-align') === 'right') {
			// Pop-out box is flush right with trigger element
			// To flush right, first add trigger offset plus trigger width
			// This positions left edge of pop-out with right edge of trigger
			// Then subtract pop-out width and padding to align both right edges
			// (Flush-left automatically if narrow window)
			res.offset.box.left = isNarrow ? 0 : Math.floor(trgr.offset.left + trgr.offset.width - popOut.offset().width + (rem - 1));
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
			res.offset.tab.left = isNarrow ? Math.floor(trgr.offset.left - rem) : 0;
		}

		// Box z-index set to 2 lower than trigger element
		// Box should render below trigger, under tab, above everything else
		res.css.box.zIndex = trgr.e.css('z-index') - 2;

		// `transform` to quickly position box, relative to top left corner
		res.css.box.transform = 'translate3d(' + res.offset.box.left +'px, ' + res.offset.box.top + 'px, 0)'

		// Tab height equals trigger height plus padding (1rem top and bottom)
		res.css.tab.height = trgr.offset.height + (rem * 2) + "px";

		// Tab width equals trigger width plus padding (1rem left and right)
		res.css.tab.width = trgr.offset.width + (rem * 2) + "px";

		// Move the tab upwards, equal to the trigger height plus padding
		// minus 1px to account for border and visually overlapping box
		res.css.tab.top = '-' + (trgr.offset.height + (rem * 2) - 1) + "px";

		// Tab z-index is 1 less than trigger; above box, below trigger
		res.css.tab.zIndex = trgr.e.css('z-index') - 1;

		// Apply that giant blob of CSS
		popOut.css({
			zIndex: res.css.box.zIndex,
			transform: res.css.box.transform,
		}).find('.pop-out__tab').css({ // find this pop-out's child tab
			height: res.css.tab.height,
			width: res.css.tab.width,
			left: res.offset.tab.left,
			right: 0, // This is always 0
			top: res.css.tab.top,
			zIndex: res.css.tab.zIndex
		});

	};

	// If there is an active pop-out, update its position
	// Mostly useful for when the browser window resizes
	this.updatePopOut = function() {
		if(state.activeElm) {
			updatePosition();
		}
	}
};

export default popOutController;
