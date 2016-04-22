/* global Promise */

var fs		= require('fs');
var fsp   	= require('./lib/fs-promise');
var marked	= require('marked');
var path	= require('path');
var postcss	= require('postcss');

var findFrontMatter = /^\s*-{3,}\r?\n((?:[ \t]*[A-z][\w-]*[ \t]*:[ \t]*[\w-][^\n]*\n*)*)(?:[ \t]*-{3,})?/;
var splitFrontMatter = /([A-z][\w-]*)[ \t]*:[ \t]*([\w-][^\n]*)/g;

// Converts string to slug
// Example: Second Side Menu! => second-side-menu

var slugify = function(title) {
	return title.replace(/\s+/g, '-').replace(/[^A-z0-9_-]/g, '').toLowerCase();
};


module.exports = postcss.plugin('mdcss', function (opts) {

	// Build out `opts` options object with defaults if not set by user
	opts = Object(opts);

	opts = {

		/**
		 * opts.theme
		 * {npm repository/module?}
		 * Currently, must be set as an NPM-style repository/module.
		 * TODO - Simplify this. Point to a single .js file with folder of assets.
		 */
		theme: opts.theme || require('./runway-theme-velir'),

		/**
		 * opts.templatesDir
		 * {string}
		 * Path to default directory where Runway should check for component
		 * templates.
		 * TODO - Let users set `template` as a filename (current behavior)
		 * _or_ a path and filename. If file check fails, try as directory.
		 */
		templatesDir: opts.templatesDir || path.join(process.cwd(), 'runway', 'templates'),

		/**
		 * opts.outputFile
		 * {string}
		 * The filename of the generated styleguide .html document.
		 */
		outputFile: opts.outputFile || 'index.html',

		/**
		 * opts.outputDir
		 * {string}
		 * The directory/path where the generated styleguide, and any required
		 * assets, will be copied to.
		 */
		outputDir: path.join(process.cwd(), opts.outputDir || 'styleguide'),

		/**
		 * opts.assets
		 * {array}
		 * A list of files or directories to copy into the styleguide directory.
		 * These are included inside generated <iframe> elements for styling
		 * each example.
		 * Note: These assets are *not* used for styling the styleguide theme.
		 * Include any assets required for the theme itself in the `asset`
		 * directory in the theme folder.
		 */
		assets: (opts.assets || []).map(function (src) {
			return path.join(process.cwd(), src);
		}),

		examples: opts.examples || null
	};

	// throw if theme is not a function
	if (typeof opts.theme !== 'function') throw Error('The theme failed to load');

	var buildDocs = function (css, result) {

		// set current css directory or current directory
		var dir = css.source.input.file ? path.dirname(css.source.input.file) : process.cwd();

		// set documentation list, store, and unique identifier
		var list = [];
		var store = {};
		var uniq = 0;

		// walk comments
		css.walkComments(function (comment) {

			// Check if comment is Runway documentation
			if (findFrontMatter.test(comment.text)) {

				var doc = {};

				// Filter and parse documentation front-matter
				doc.content = marked(comment.text.replace(findFrontMatter, function (docBlock, frontMatter) {
					if (frontMatter) { // If there is any front-matter, parse it
						frontMatter.replace(splitFrontMatter, function (isMeta0, name, value) {
							doc[name] = value.trim();
						});
					}

					// Now that we've captured it,
					// remove any front-matter and documentation
					return '';

				}, opts.marked).trim()); // Trim whitespace, for good measure.

				// conditionally set the closest documentation name
				if (doc.title && !doc.name) doc.name = slugify(doc.title);


				// If there's a `template` key set in the comment documentation,
				// determine whether the value is a URL or a filepath, then try
				// to load the appropriate template markup.

				if(doc.template) {

					var templateMarkup = fs.readFileSync(path.join(opts.templatesDir, doc.template), 'utf8');

					// Appending template markup to `doc.content`, instead of
					// just setting the value, prevents any notes inside the
					// CSS documentation from being clobbered.
					doc.content += marked(templateMarkup);

				}

				// set documentation context
				doc.context = comment;

				// If section has to name, give it a name
				var sectionName = doc.name || 'section' + --uniq; // section-1, etc
				var finalName = sectionName;

				// Make sure name is unique
				while (finalName in store) {
					finalName = sectionName + --uniq; // main-menu becomes main-menu-2
				}

				// push documentation to store
				store[finalName] = doc;
			}
		});

		// walk stores
		Object.keys(store).forEach(function (name) {
			// set documentation
			var doc = store[name];
			if ('section' in doc) { // if documentation has a parent section, get parent section
				var title = doc.section;
				var slug = slugify(title);
				var parent = store[slug];

				if (!parent) { // if parent section does not exist
					parent = store[slug] = { // create parent section
						title: title,
						name:  slug
					};

					list.push(parent); // add parent section to list
				}

				// If no children, set as empty array
				parent.children = parent.children || [];

				// make documentation a child of the parent section
				parent.children.push(doc);

				doc.parent = parent;
			} else { // make documentation a child of list
				list.push(doc);
			}
		});

		var buildTheme = function(opts) {
			return opts.theme({
				list: list,
				opts: opts
			});
		};

		var copyThemeFiles = function (docs) {

			var emptyStyleguideDir = function() {
				console.log('emptying directory');
				return fsp.emptyDir(opts.outputDir);
			};

			var copyThemeAssets = function() {
				// then copy the theme assets into the outputDir
				return fsp.copy(docs.assets, opts.outputDir);
			};

			var copyStyleguideTemplate = function() {
				return fsp.outputFile(path.join(opts.outputDir, opts.outputFile), docs.template);
			};

			var copyUserAssets = function () {
				// then copy any of the additional assets into the outputDir
				return Promise.all(opts.assets.map(function (src) {
					return fsp.copy(src, path.join(opts.outputDir, path.basename(src)));
				}));
			};

			return emptyStyleguideDir()
				.then( copyThemeAssets )
				.then( copyStyleguideTemplate )
				.then( copyUserAssets );

		};

		// return theme executed with parsed list, outputDir
		return buildTheme(opts).then( copyThemeFiles );

	};

	return buildDocs;
});
