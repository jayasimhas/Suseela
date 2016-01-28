var gulp = require("gulp"),
    utils = require("./utils"),
    config = utils.loadConfig(),
    preprocess = require("gulp-preprocess"),
    debug = require("gulp-debug"),
    rename = require("gulp-rename"),
    argv = require('yargs').argv,    
    msbuild = require("gulp-msbuild");


var path = require("path"),
    fs = require("fs");
//init settings
utils.setTaskConfig("init", {
    default: {
        envRoot: config.root + "../../../config/env",
        src: [
            config.root + "/../../**/*.velirTemplate",
            "!node_modules/**",
            "!bower_components/**"
        ],
        initFile: (argv.initEnv || config.local.initEnv) + ".json"
    },
    prod: {
        
    }
});

/* css task */
gulp.task("init", function () {
    var init = utils.loadTaskConfig("init");
    
    var filePath = path.resolve(init.envRoot + "/" + init.initFile);
    console.warn("\nProperties file " + filePath + " targeted.\n");
    
    try {
        //fs.accessSync(filePath, fs.F_OK);
        // Do something

        var file = fs.readFileSync(filePath, { encoding: "utf8" });

    var jsonProperties = JSON.parse(file);

    return gulp.src(init.src, {base: config.root})
        .pipe(debug())
        .pipe(preprocess({ context: jsonProperties }))
        .pipe(rename({
            extname: ""
        }))
        .pipe(gulp.dest("."));
    } catch (e) {
        console.warn("\nProperties file " + path.resolve(init.envRoot + "/" + init.initFile) + " not found.\n");
        return gulp;
    }
});
