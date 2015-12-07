var gulp           = require("gulp"),
    utils          = require("./utils"),
    config         = utils.loadConfig(),
    tsd         = require("gulp-tsd");


// copy files settings
utils.setTaskConfig("typings", {

    default: {
        command: 'reinstall',
        config: "./tsd.json"
    },
    prod: {

    }
    
});

// register the watch
//utils.registerWatcher("typings", [
//    config.root + "/html/**/*.html", 
//    config.root + "/html/**/*.htm",
//    config.root + "/index.html"
//]);


/* copy files */
gulp.task("typings", function(next) {

    var settings = utils.loadTaskConfig("typings");

    return tsd(settings, next);

});




