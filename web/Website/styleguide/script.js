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

		wrap.appendChild(pre);

		// conditionally syntax highlight code
		if (conf.lang in Prism.languages) code.innerHTML = Prism.highlight(text, Prism.languages[conf.lang]);

		// conditionally create code examples
		if (conf.example in examples.lang) {
			examples.lang[conf.example](pre, text, conf);

			if (!(conf.lang in Prism.languages)) wrap.removeChild(pre);
		}
	});

	var sections = document.getElementsByTagName('section');

	for(var i = 0; i < sections.length; i++) {
		sections[i].classList.add('hidden');
		sections[i].addEventListener('click', toggleSection(sections[i].id));
	}

	var sectionTriggers = document.querySelectorAll('[data-toggle-section]');
	for(var j = 0; j < sectionTriggers.length; j++) {
		sectionTriggers[j].addEventListener('click', toggleSection(sectionTriggers[j].dataset.toggleSection));
	}

});

var resizeIframes = function() {
	$('.iframe-wrapper').each(function(e, i, a) {
		console.log(e);
		var elmScrollHeight = e.offsetHeight;
		var style  = e.style;
		var scrollHeight;
		// get iframe dom
		var iwin = e.contentWindow;
		var idoc = e.document;

		if (scrollHeight !== elmScrollHeight) {
			scrollHeight = elmScrollHeight;
			style.height = 0;
			style.height = parseInt(documentElement.scrollHeight) + (iframe.offsetHeight - iwin.innerHeight) + 'px';
		}
	});
};

window.addEventListener('message', function(e) {
	console.log(e.data);
  // var $iframe = jQuery("#myIframe");
  // var eventName = e.data[0];
  // var data = e.data[1];
  // switch(eventName) {
  //   case 'setHeight':
  //     $iframe.height(data);
  //     break;
  // }
}, false);

var toggleSection = function(section) {
	return function() {

		var sections = document.getElementsByTagName('section');

		for(var i = 0; i < sections.length; i++) {
			sections[i].classList.add('hidden');
		}

		var sectionElm = document.getElementById(section);
		sectionElm.classList.remove('hidden');

		if(sectionElm.parentNode.parentNode.tagName === 'SECTION') {
			sectionElm.parentNode.parentNode.classList.remove('hidden');
		} else {
			var subsections = sectionElm.getElementsByTagName('section');
			for(var k = 0; k < subsections.length; k++) {
				subsections[k].classList.remove('hidden');
			}
		}

		resizeIframes();
	};
};
