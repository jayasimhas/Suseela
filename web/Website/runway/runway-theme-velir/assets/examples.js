/* global examples */

examples.lang = {
	color: function (pre, value) {

		var colors = pre.parentNode.insertBefore(document.createElement('div'), pre);
		var allLines = value.trim().split(/\n+/);

		$(colors).addClass('colors');

		var parseLine = function(line) {
			line = line.trim();

			var color = {};
			var match = /@([^:]+):\s*(.+?)(?=\s+@|$)/g;
			var prop;

			// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RegExp/exec
			// `do` instead of `while` prevents ESLint complaints
			do {
				prop = match.exec(line);
				if (prop) {
					color[prop[1]] = prop[2];
				}
			} while (prop);

			return color;
		};

		var parseColor = function(color) {

			var colorNode = document.createElement('div');
			$(colorNode).addClass('color');

			var swatchNode = colorNode.appendChild(document.createElement('div'));
			$(swatchNode).addClass('color-swatch').css({
				backgroundColor: color.color
			});

			Object.keys(color).filter(function (key) {
				return key !== 'color';
			}).forEach(function (key) {
				var propertyNode = colorNode.appendChild(document.createElement('div'));

				$(propertyNode).addClass('color-property')
					.append('<p class="lang-color__color-name">' + color[key] + '</p>')
					.append('<p class="lang-color__color-hex">' + color.color + '</p>')
					.append('<p class="lang-color__color-scss">$color-' + $.fn.slugify(color[key]) + '</p>');
			});

			return colorNode;
		};

		allLines.map(parseLine).map(parseColor).forEach(colors.appendChild, colors);

		function hex2rgb(hex) {
			var bigint = parseInt(hex.slice(1).replace(/^([0-9a-f])([0-9a-f])([0-9a-f])$/i, '$1$1$2$2$3$3'), 16);
			return [(bigint >> 16) & 255, (bigint >> 8) & 255, bigint & 255];
		}

		function contrast(color) {
			var getRGB = /^#/.test(color) ?
				hex2rgb(color) :
				color.replace(/[^\d,]+/g, '').split(/,/).map(function (part) { return part * 1; });
			var rgb = getRGB(color);
			var o   = Math.round(((parseInt(rgb[0]) * 299) + (parseInt(rgb[1]) * 587) + (parseInt(rgb[2]) * 114)) / 1000);

			return o <= 180 ? '#ffffff' : '#000000';
		}
	},
	html: function (pre, value, conf) {
		// get wrap
		var wrap = pre.parentNode;

		var iframe = wrap.insertBefore(document.createElement('iframe'), pre);

		$(wrap).prepend('<div class="iframe-resizer-handle">|||</div>');
		var style  = iframe.style;

		// get iframe dom
		var iwin = iframe.contentWindow;
		var idoc = iwin.document;

		// write example content to iframe
		idoc.open();

		var html = '<base' + (
			examples.base && ' href="' + examples.base + '"'
		) + (
			examples.target && ' target="' + examples.target + '"'
		) + '>';

		html += examples.css.map(function (css) {
			return '<link href="' + css + '" rel="stylesheet">';
		}).join('');

		html += examples.js.map(function (js) {
			return '<script src="' + js + '"></script>';
		}).join('');

		html += value;

		html += examples.bodyjs.map(function (js) {
			return '<script src="' + js + '"></script>';
		}).join('');

		idoc.write(html);

		idoc.close();


		// add default block styles to iframe dom
		if(examples.iframeReset) {
			var resetStyles = 'background:none; border:0; clip:auto; display:block;  height:auto; margin:0; padding:0; position:static; width:auto';
			idoc.documentElement.setAttribute('style', resetStyles);
			idoc.body.setAttribute('style', resetStyles);
		}

		var siteTheme = location.search.split('theme=')[1];
		if(siteTheme) {
			idoc.body.className += 'theme-' + siteTheme;
		}

		idoc.body.innerHTML = '<div class="iframe-wrapper" contenteditable spellcheck="false">' + idoc.body.innerHTML + '<div style="clear: both;"></div></div>';

		if (conf.width) style.width = String(conf.width);

		// set iframe height based on content
		var documentElement = idoc.documentElement;
		var scrollHeight;
		var resizeTimeout = 0;

		var resize = function() {
			var currentScrollHeight = documentElement.getElementsByClassName('iframe-wrapper')[0].offsetHeight;
			if (scrollHeight !== currentScrollHeight) {
				scrollHeight = currentScrollHeight;
				style.height = 0;
				style.height = parseInt(documentElement.scrollHeight) + (iframe.offsetHeight - iwin.innerHeight) + 'px';
				parent.window.dispatchEvent(new Event('resize'));
			}
		};

		iwin.addEventListener('resize', resize);

		setTimeout(resize, 100);

	}
};
