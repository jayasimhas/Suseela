/// <binding />
/**
 *  Usage:
 *      Once per computer:
 *         $ npm install -g gulp
 *
 *      Once per project, in gulp folder:
 *         $ npm install
 *
 *
 *      Running clumped tasks (defined in this file) --
 *      see tasks/utils.js config
 *         $ gulp dev
 *
 *      Running single task (task defined in /tasks.  eg. /tasks/css.js)
 *         $ gulp css            // will use the default config
 *         $ gulp css --env prod // will use the prod config
 *
 *      For details on setConfig, see "user supplied keys" in /tasks/utils.js
**/

// Include gulp and plugins
var gulp    = require("gulp"),
    utils   = require("./tasks/utils"),
    notify  = require("gulp-notify"),
    path    = require("path"),
    msbuild = require("gulp-msbuild"),
    config  = utils.loadConfig(); // initialize the config


// set some defaults
utils.setConfig({
//    root  : path.resolve("../../web/Website"),
//    dest  : path.resolve("../../web/Website/dist"),
    root  : path.resolve("../Website"),
    dest: path.resolve("../Website/dist"),
    env   : ""
});


// load the tasks
utils.loadTasks(["init", "js", "css", "styleguide", "copy", "bower", "svg-sprite", "msbuild"]);

/**
 * dev task
 */
gulp.task("dev", function () {

    // set the dev config (cache in utils.js)
    utils.setConfig({
        env: "dev",
        watch: true,
        notify: true,
        tasks: ["js", "css", "copy", "bower", "svg-sprite"]
    });

    // build with this config
    utils.build();

});


/**
 * dev task, including styleguide build
 */
gulp.task("dev-styleguide", function () {

    // set the dev config (cache in utils.js)
    utils.setConfig({
        env: "dev",
        watch: true,
        notify: true,
        tasks: ["js", "css", "styleguide", "copy", "bower", "svg-sprite"]
    });

    // build with this config
    utils.build();

});

/**
 * dev special snowflake task
 */
gulp.task("dev-rsync", function () {

    // set the dev config (cache in utils.js)
    utils.setConfig({
        env: "dev",
        watch: true,
        notify: true,
        tasks: ["js", "css", "copy", "bower", "svg-sprite", "rsync"]
    });

    // build with this config
    utils.build();

});

/**
 * dev task
 */
gulp.task("dev-nowatch", function(){

    // set the dev config (cache in utils.js)
    utils.setConfig({
        env   : "dev",
        watch : false,
        notify: false,
        tasks: ["js", "css", "copy", "bower", "svg-sprite"]
    });

    // build with this config
    utils.build();

});

/**
 * dev task
 */
gulp.task("clean-build", function () {

    // set the dev config (cache in utils.js)
    utils.setConfig({
        env: "dev",
        watch: false,
        notify: true,
        tasks: ["init", "js", "css", "copy", "bower", "svg-sprite"]
    });

    // build with this config
    utils.build();

});

/**
 * prod task
 */
gulp.task("prod", function(){

    // set the prod config (cache in utils.js)
    utils.setConfig({
        env   : "prod",
        watch : false,
        tasks: ["js", "css", "copy", "bower", "svg-sprite"]
    });

    // build with this config
    utils.build();

});




// Default Task (run when you run 'gulp'). dev envirnoment
gulp.task("default", [config.local.defaultTask || "dev"]);
