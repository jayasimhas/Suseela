var gulp           = require("gulp"),
    utils          = require("./utils"),
    config         = utils.loadConfig(),
    rename         = require("gulp-rename");


// copy files settings
utils.setTaskConfig("copy", {

    default: {
        src: [ 
            config.root + '/assets/img/**/*.*',           
        ],
        dest: config.dest + '/img'
    },

    prod: {

    }
    
});

// register the watch
utils.registerWatcher("copy", [ 
    config.root + '/assets/img/**/*.*',            
]);

/* copy files */
gulp.task("copy", function(next) {

    var copy = utils.loadTaskConfig("copy");

    // register the watch
    utils.registerWatcher("copy", copy.src);

    return gulp.src(copy.src)
            .pipe(utils.drano())
            .pipe(gulp.dest(copy.dest));

});




