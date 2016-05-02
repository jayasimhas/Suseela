var gulp          = require("gulp"),
    utils         = require("./utils"),
    config        = utils.loadConfig(),
    sass          = require("gulp-sass"),
    autoprefixer  = require("gulp-autoprefixer"),
    pixrem        = require("gulp-pixrem"),
    rename        = require("gulp-rename"),
    header        = require("gulp-header"),
    concat        = require("gulp-concat"),
    postcss       = require('gulp-postcss'),
    debug         = require('gulp-debug'),
    sourcemaps    = require("gulp-sourcemaps");

// css settings
utils.setTaskConfig("css", {
    default: {

        src: config.root + "/styles/index.scss",
        dest: config.dest,

        filename: "index.css",

        sass: {
            outputStyle: "nested"
            // includePaths: require("node-neat").includePaths
        },

        autoprefixer: {
            browsers: ["> 1%", "last 2 versions", "Firefox ESR", "Opera 12.1", "ie >= 10"]
        }
    },

    prod: {
        sass: {
            outputStyle: "compressed"
        }
    }
});


// register the watch
utils.registerWatcher("css", [
    config.root + "/styles/**/*.scss"
]);


/* css task */
gulp.task("css", function() {

    var css = utils.loadTaskConfig("css");

    console.log(css);

    var gulpCss = gulp.src(css.src)
        .pipe(utils.drano())
        .pipe(sourcemaps.init())
        .pipe(sass(css.sass))
        .pipe(autoprefixer(css.autoprefixer))
        .pipe(pixrem("16px",{atrules: true, html: true}))
        .pipe(concat(css.filename, {newLine: ""}))
        .pipe(rename({
            suffix: "-generated"
        }));

    // only add the header text if this css isn't compressed
    if (css.sass && css.sass.outputStyle !== "compressed"){
        gulpCss.pipe(header("/* This file is generated.  DO NOT EDIT. */ \n"));
    }

    return gulpCss
        .pipe(debug({title: "css-1: ", minimal: false}))
        .pipe(sourcemaps.write("./"))
        .pipe(debug({title: "css-2: ", minimal: false}))
        .pipe(gulp.dest(css.dest))
        .pipe(debug({title: "css-3: ", minimal: false}));
});
