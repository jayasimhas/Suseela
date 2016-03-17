var SearchScript = function() {

			/* Toggle search tips visibility */
			$('.js-toggle-search-tips').on('click', function toggleTips() {
				$('.search-bar__tips').toggleClass('open');
				$('.search-bar').toggleClass('tips-open');
			});

}();
