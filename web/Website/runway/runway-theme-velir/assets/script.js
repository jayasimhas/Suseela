/* global examples, Prism */

$(document).ready(function () {
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

		$(wrap).addClass('classhere');

		wrap.appendChild(pre);

		// conditionally syntax highlight code
		if (conf.lang in Prism.languages) code.innerHTML = Prism.highlight(text, Prism.languages[conf.lang]);

		// conditionally create code examples
		if (conf.example in examples.lang) {
			examples.lang[conf.example](pre, text, conf);

			if (!(conf.lang in Prism.languages)) wrap.removeChild(pre);
		}
	});

	$('.iframe-resizer-handle').each(function(elm, ind, arr) {

		var currentElement = null;

		var mouseMoveHandler = function (event) {
			event.preventDefault();
			var newHeight = currentElement._startHeight + (event.pageY - currentElement._startY);
			Element.height(Element.getPrevious(currentElement), newHeight);
		};

		var mouseUpHandler = function (event) {
			currentElement = null;
			Event.removeEvent(document, 'mousemove', mouseMoveHandler);
			Event.removeEvent(document, 'mouseup', mouseUpHandler);
		};

		for (var i in panel.content.childNodes) {
			var element = panel.content.childNodes[i],
				tag = element.nodeName ? element.nodeName.toUpperCase() : false;
			if (tag === 'DIV' && Element.hasClass(element, 'panel-resize-handle')) {

				Event.addEvent(element, 'mousedown', function (event) {
					event.preventDefault();
					currentElement = this;
					this._startY = event.pageY;
					this._startHeight = parseInt(Element.height(Element.getPrevious(currentElement)));

					Event.addEvent(document, 'mousemove', mouseMoveHandler);
					Event.addEvent(document, 'mouseup', mouseUpHandler);
				});
			}
		}
	});

});
