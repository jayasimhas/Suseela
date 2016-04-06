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

var css = utils.loadTaskConfig("css");

utils.setTaskConfig("styleguide", {
    default: {
        src: css.dest + "/index-generated.css",
    }
});


// register the watch
utils.registerWatcher("styleguide", [
    config.root + "/styles/**/*.scss"
]);


/* css task */
gulp.task("styleguide", function() {

    var styleguide = utils.loadTaskConfig("styleguide");

    var gulpStyle = gulp.src(styleguide.src)
        .pipe(utils.drano())
        .pipe(postcss([
            require('../runway')({
                title: "Informa Style Guide",
                logo: "https://pbs.twimg.com/profile_images/537287811724881921/JrtNWIt5.png",
                css: ['style.css', 'https://fonts.googleapis.com/css?family=PT+Serif:400,400italic|Roboto:400,300,500,700,400italic|Roboto+Condensed:400,700,700italic'],
                assets: ['dist/img/svg-sprite.svg', 'dist/index-generated.css'],
                examples: {
                    css: ["https://fonts.googleapis.com/css?family=PT+Serif:400,400italic|Roboto:400,300,500,700,400italic|Roboto+Condensed:400,700,700italic", "index-generated.css"]
                }
            })
        ]));

    return gulpStyle;
});
