var gulp           = require("gulp"),
    utils          = require("./utils"),
    config         = utils.loadConfig(),
    svgmin         = require("gulp-svgmin");


// dev/default settings
var images =  {
    src: [
        config.root + "/assets/images/**/*",
        "!" + config.root + "/assets/images/**/*.svg"
    ],
    watch: [
        config.root + "/assets/images/**/*",
        config.root + "/assets/images/**/*.svg" 
    ],
    dest: config.dest + "/assets/images/",
};

var svg = { 
    src: config.root + "/assets/images/**/*.svg" 
};


// production settings
if (config.env === "prod") {
    // defaults
}



/* copy images and svgs */
gulp.task("images", function(next){

    // images
    gulp.src(images.src)
        .pipe(gulp.dest(images.dest));

    // svg
    gulp.src(svg.src)
        .pipe(svgmin())
        .pipe(gulp.dest(images.dest));

    next();

});

// watch css
if (config.watch){
    utils.logYellow("watching", "images:", images.watch);
    gulp.watch(images.watch, ["images"]);
}
