var fs     = require('fs');
var fsp    = require('./lib/fs-promise');
var marked = require('marked');
var path   = require('path');

var findFrontMatter = /^\s*-{3,}\r?\n((?:[ \t]*[A-z][\w-]*[ \t]*:[ \t]*[\w-][^\n]*\n*)*)(?:[ \t]*-{3,})?/;
var splitFrontMatter = /([A-z][\w-]*)[ \t]*:[ \t]*([\w-][^\n]*)/g;

module.exports = require('postcss').plugin('mdcss', function (opts) {
	// set options object
	opts = Object(opts);

	// set theme
	opts.theme = opts.theme || require('runway-theme-velir');

	// set index
	opts.index = opts.index || 'index.html';

	// throw if theme is not a function
	if (typeof opts.theme !== 'function') throw Error('The theme failed to load');

	// conditionally set theme as executed theme
	if (opts.theme.type === 'mdcss-theme') opts.theme = opts.theme(opts);

	// set destination path
	opts.destination = path.join(process.cwd(), opts.destination || 'styleguide');

	// set additional assets path
	opts.assets = (opts.assets || []).map(function (src) {
		return path.join(process.cwd(), src);
	});

	// return plugin
	return function (css, result) {
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

				// If there's no inline content/examples,
				// conditionally import external content
				if (!doc.content) {
					// get comment source path
					var src = comment.source.input.file;

					// if the comment source path exists
					if (src) {
						// get the closest matching directory for this comment
						var localdir = src ? path.dirname(src) : dir;

						var mdbase = doc.import;
						var mdspec;

						// conditionally use a sibling md files if no import exists
						if (!mdbase) {
							mdbase = mdspec = path.basename(src, path.extname(src));

							if (doc.name) {
								mdspec += '.' + doc.name;
							}

							mdbase += '.md';
							mdspec += '.md';
						}

						// try to read the closest matching documentation
						try {
							if (mdspec) {
								doc.content = fs.readFileSync(path.join(localdir, mdspec), 'utf8');
							} else throw new Error();
						} catch (error1) {
							try {
								doc.content = fs.readFileSync(path.join(localdir, mdbase), 'utf8');
							} catch (error2) {
								doc.content = '';

								comment.warn(result, 'Documentation import "' + mdbase + '" could not be read.');
							}
						}
					}
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
			// if documentation has a parent section
			if ('section' in doc) {
				// get parent section
				var title  = doc.section;
				var slug  = slugify(title);
				var parent = store[slug];

				// if parent section does not exist
				if (!parent) {
					// create parent section
					parent = store[slug] = {
						title: title,
						name:  slug
					};

					// add parent section to list
					list.push(parent);
				}

				// If no children, set as empty array
				parent.children = parent.children || [];

				// make documentation a child of the parent section
				parent.children.push(doc);

				doc.parent = parent;
			} else {
				// otherwise make documentation a child of list
				list.push(doc);
			}
		});

		// return theme executed with parsed list, destination
		return opts.theme({
			list: list,
			opts: opts
		}).then(function (docs) {
			// empty the destination directory
			return fsp.emptyDir(opts.destination)
			// then copy the theme assets into the destination
			.then(function () {
				return fsp.copy(docs.assets, opts.destination);
			})
			// then copy the compiled template into the destination
			.then(function () {
				return fsp.outputFile(path.join(opts.destination, opts.index), docs.template);
			})
			// then copy any of the additional assets into the destination
			.then(function () {
				return Promise.all(opts.assets.map(function (src) {
					return fsp.copy(src, path.join(opts.destination, path.basename(src)));
				}));
			});
		});
	};
});

// Converts string to slug
// Second Side Menu! => second-side-menu
function slugify(title) {
	return title.replace(/\s+/g, '-').replace(/[^A-z0-9_-]/g, '').toLowerCase();
}
