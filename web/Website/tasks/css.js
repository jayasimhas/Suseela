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
    sourcemaps    = require("gulp-sourcemaps");

// css settings
utils.setTaskConfig("css", {
    default: {

        src: config.root + "/styles/**/*.scss",
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

    var gulpCss = gulp.src(css.src)
        .pipe(utils.drano())
        .pipe(sourcemaps.init())
        .pipe(sass(css.sass))
        .pipe(autoprefixer(css.autoprefixer))
        .pipe(pixrem("16px",{atrules: true, html: true}))
        .pipe(concat(css.filename, {newLine: ""}))
        .pipe(rename({
            suffix: "-generated"
        }))
        .pipe(postcss([
            require('mdcss')({
                title: "Informa Style Guide",
                logo: "https://pbs.twimg.com/profile_images/537287811724881921/JrtNWIt5.png",
                css: ['style.css', 'https://fonts.googleapis.com/css?family=PT+Serif:400,400italic|Roboto:400,300,500,700,400italic|Roboto+Condensed:400,700,700italic'],
                assets: ['dist/img/svg-sprite.svg', 'dist/index-generated.css'],
                examples: {
                    css: ["https://fonts.googleapis.com/css?family=PT+Serif:400,400italic|Roboto:400,300,500,700,400italic|Roboto+Condensed:400,700,700italic", "index-generated.css"]
                }
            })
        ]));

    // only add the header text if this css isn't compressed
    if (css.sass && css.sass.outputStyle !== "compressed"){
        gulpCss.pipe(header("/* This file is generated.  DO NOT EDIT. */ \n"));
    }

    return gulpCss
        .pipe(sourcemaps.write("./"))
        .pipe(gulp.dest(css.dest));
});
