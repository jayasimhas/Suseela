/* global Promise */

var ejs  = require('ejs');
var fs   = require('fs');
var path = require('path');

module.exports = function (params) {

	var themeOptions = Object(params.opts);
	var docs = params;

	// set theme css
	themeOptions.js = themeOptions.js || [];

	// set example conf
	themeOptions.examples = Object.assign({
		base:    '',
		target:  '_self',
		css:     [],
		js:      [],
		bodyjs:  [],
		iframeReset: true
	}, themeOptions.examples);


	docs.opts = themeOptions;

	// set assets directory and template
	docs.assets   = path.join(__dirname, 'assets');
	docs.template = path.join(__dirname, 'template.ejs');

	// set theme options
	docs.themeopts = themeOptions;

	// return promise
	return new Promise(function (resolve, reject) {
		// read template
		fs.readFile(docs.template, 'utf8', function (error, contents) {
			// throw if template could not be read
			if (error) reject(error);
			else {
				// set examples options
				docs.opts = Object.assign({}, docs.opts, docs.themeopts);

				// bake compiled template
				docs.template = ejs.compile(contents)(docs);

				// resolve docs
				resolve(docs);
			}
		});
	});
};
