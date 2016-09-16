var gulp           = require("gulp"),
    utils          = require("./utils"),
    config         = utils.loadConfig(),
    gulpif         = require("gulp-if"),
    fs             = require("fs"),
    uglify         = require("gulp-uglify"),
    sourcemaps     = require("gulp-sourcemaps"),
    browserify     = require("browserify"),
    shim           = require("browserify-shim"),
    through2       = require("through2"),
    babelify       = require("babelify");


// dev/default settings
utils.setTaskConfig("js", {

    default: {

        // Pass array instead of single file!
        src: [
            config.root + "/js/index.js",
			config.root + "/js/owlCarousel.js",
            config.root + "/js/search/search.js",

            // Angular 1.x doesn't play well with CommonJS modules :(
            config.root + "/js/search/angular-1.5.0.min.js",
            config.root + "/js/search/angular-animate-1.5.0.min.js",
            config.root + "/js/search/angular-sanitize-1.5.0.min.js"
        ],

        dest: config.dest + "/js",

        // js uglify options, to skip, set value to false or omit entirely
        // otherwise, pass options object (can be empty {})
        uglify: false,

        // browserify options
        browserify: {
            debug: true // enable sourcemaps
        }
    },

    prod: {

        browserify: {},

        // uglify javascript for production
        uglify: {}
    }
});


// register the watch
utils.registerWatcher("js", [
    config.root + "/js/**/*.js",
    config.root + "/js/**/*.jsx"
]);



/* compile application javascript */
gulp.task("js", function(){

    var js = utils.loadTaskConfig("js");

    // for browserify usage, see https://medium.com/@sogko/gulp-browserify-the-gulp-y-way-bb359b3f9623
    // ^^ we can't use vinyl-transform anymore because it breaks when trying to use b.transform()
    // https://github.com/sogko/gulp-recipes/tree/master/browserify-vanilla
    var browserifyIt = through2.obj(function (file, enc, callback){

        // https://github.com/substack/node-browserify/issues/1044#issuecomment-72384131
        var b = browserify(js.browserify || {}) // pass options
            .add(file.path) // this file
            .transform(babelify)
            .transform(shim);

        b.bundle(function(err, res){
            if (err){
                callback(err, null); // emit error so drano can do it's thang
            }
            else {
                file.contents = res; // assumes file.contents is a Buffer
                callback(null, file); // pass file along
            }
        });

    });

    return gulp.src(js.src)
        .pipe(utils.drano())
        .pipe(browserifyIt)
        .pipe(sourcemaps.init({ loadMaps: true })) // loads map from browserify file
        .pipe(gulpif((js.uglify), uglify(js.uglify)))
        .pipe(sourcemaps.write("./"))
        .pipe(gulp.dest(js.dest));
});
