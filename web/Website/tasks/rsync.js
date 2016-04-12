var gulp           = require("gulp"),
    utils          = require("./utils"),
    config         = utils.loadConfig(),
    path           = require("path"),
    rsync          = require("gulp-rsync"),
    debug          = require("gulp-debug"),
    exec           = require("child_process").exec;

var vmDest = config.local.vmDest;

var locSrc = config.local.locSrc;

// copy files settings
utils.setTaskConfig("rsync", {

    default: {
        src: locSrc,
        dest: vmDest
    },

    prod: {

    }

});

// register the watch
utils.registerWatcher("rsync", locSrc);

/* copy files */
gulp.task("rsync", function() {

    console.log(locSrc);

    var rsyncConfig = utils.loadTaskConfig("rsync");

    return gulp.src(rsyncConfig.src)
        .pipe(utils.drano())
        .pipe(rsync({
            destination: vmDest,
            progress: true,
            incremental: true,
            update: true,
            relative: true
        }))
        .pipe(debug({title: "rsync: "}))

});

gulp.task('rsync-all', function() {
    // Provides access to a to a remote server
    var rsyncCmd = exec('rsync -ruth --exclude=".*" --progress ' + config.root + '/ ' + vmDest);

    rsyncCmd.stdout.on('data', function(data) {
        console.log(data);
    })
});