var gulp           = require("gulp"),
    utils          = require("./utils"),
    config         = utils.loadConfig(),
    rename         = require("gulp-rename");


// copy files settings
utils.setTaskConfig("copy", {

    default: {
        src: [ 
            config.root + "/html/**/*.html", 
            config.root + "/html/**/*.htm",
            config.root + "/index.html"
        ],
        dest: config.dest
    },

    prod: {

    }
    
});

// register the watch
utils.registerWatcher("copy", [ 
    config.root + "/html/**/*.html", 
    config.root + "/html/**/*.htm",
    config.root + "/index.html"
]);


/* copy files */
gulp.task("copy", function(next) {

    var copy = utils.loadTaskConfig("copy");

    return gulp.src(copy.src)
            .pipe(utils.drano())
            .pipe(gulp.dest(copy.dest));

});




