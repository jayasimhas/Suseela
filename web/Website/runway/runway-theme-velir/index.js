var ejs  = require('ejs');
var fs   = require('fs');
var path = require('path');

module.exports = function (themeopts) {
	// set theme options object
	themeopts = Object(themeopts);

	// set theme css
	themeopts.js = themeopts.js || [];

	// set example conf
	themeopts.examples = Object.assign({
		base:    '',
		target:  '_self',
		css:     [],
		js:      [],
		bodyjs:  [],
		iframeReset: true
	}, themeopts.examples);

	// return theme
	return function (docs) {
		// set assets directory and template
		docs.assets   = path.join(__dirname, 'assets');
		docs.template = path.join(__dirname, 'template.ejs');

		// set theme options
		docs.themeopts = themeopts;

		// return promise
		return new Promise(function (resolve, reject) {
			// read template
			fs.readFile(docs.template, 'utf8', function (error, contents) {
				// throw if template could not be read
				if (error) reject(error);
				else {
					// set examples options
					docs.opts = Object.assign({}, docs.opts, docs.themeopts);

					// set compiled template
					docs.template = ejs.compile(contents)(docs);

					// resolve docs
					resolve(docs);
				}
			});
		});
	};
};

module.exports.type = 'mdcss-theme';
