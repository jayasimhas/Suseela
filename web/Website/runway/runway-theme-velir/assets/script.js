/* global examples, Prism */

$(document).ready(function () {

	var currentTheme = location.search.split('theme=')[1];

	$('.js-theme-select').on('change', function() {

		var siteRoot = 'http://' + location.host + location.pathname;

		if(this.selectedIndex === 0 && currentTheme) {
			window.location.href = siteRoot;
		}
		if(this.selectedIndex === 1 && currentTheme !== 'pink-sheet') {
			window.location.href = siteRoot + '?theme=pink-sheet';
		}
		if(this.selectedIndex === 2 && currentTheme !== 'rose-sheet') {
			window.location.href = siteRoot + '?theme=rose-sheet';
		}
		if(this.selectedIndex === 3 && currentTheme !== 'medtech') {
			window.location.href = siteRoot + '?theme=medtech';
		}
		if(this.selectedIndex === 4 && currentTheme !== 'invivo') {
			window.location.href = siteRoot + '?theme=invivo';
		}
	});

	$('pre code[class^="lang"]').each(function(code, index, array) {
		// set pre, wrap, opts, and get meta data from code
		var pre  = code.parentNode;
		var wrap = pre.parentNode.insertBefore(document.createElement('figure'), pre);
		var conf = {};
		var text = String(code.textContent || code.innerText || '');

		// get meta data from code class
		code.className.replace(/^lang-(\w+)(?::(\w+))?/, function ($0, $1, $2) {
			if ($2) return 'example:' + $2 + ',lang:' + $2;

			if ($1 === 'example') return 'example:html';

			return 'lang:' + $1;
		}).split(/\s*,\s*/).forEach(function (opt) {
			opt = opt.split(':');

			conf[opt.shift().trim()] = opt.join(':').trim();
		});

		code.removeAttribute('class');

		wrap.appendChild(pre);

		// conditionally syntax highlight code
		if (conf.lang in Prism.languages) code.innerHTML = Prism.highlight(text, Prism.languages[conf.lang]);

		// conditionally create code examples
		if (conf.example in examples.lang) {
			examples.lang[conf.example](pre, text, conf);

			if (!(conf.lang in Prism.languages)) wrap.removeChild(pre);
		}
	});


	/**
	 * Drag to resize component
	 */

	$('.iframe-external-wrapper').each(function(elm, ind, arr) {

		var handle = $(elm).find('.iframe-resizer-handle');
		var thisHandle = null,
			iframe = null,
			parentFigure = null;

		var mouseMoveHandler = function (event) {
			event.preventDefault();
			var offset = event.pageX - iframe._startX;
			var newWidth = iframe._startWidth + offset;
			if(newWidth <= parentFigure.width()) {
				var readjustmentOffset = parentFigure.width() - newWidth;
				thisHandle.style.right = readjustmentOffset + 'px';
				iframe.elm[0].style.width = newWidth + 'px';
			}
		};

		var mouseUpHandler = function (event) {
			document.removeEventListener('mousemove', mouseMoveHandler);
			document.removeEventListener('mouseup', mouseUpHandler);
			iframe = thisHandle = parentFigure = null;
		};

		$(handle).on('mousedown', function (event) {
			event.preventDefault();

			thisHandle = event.target;

			iframe = {
				elm: $(event.target).parent().find('iframe'),
				_startX: event.pageX
			};

			parentFigure = $(thisHandle).parent();

			iframe._startWidth = iframe.elm.width();

			// `mousemove`, not `onmousemove`, for more accurate updates
			document.addEventListener('mousemove', mouseMoveHandler, false);
			document.addEventListener('mouseup', mouseUpHandler, false);
		});
	});

});
