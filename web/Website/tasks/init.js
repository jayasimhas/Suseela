var gulp = require("gulp"),
    utils = require("./utils"),
    config = utils.loadConfig(),
    preprocess = require("gulp-preprocess"),
    debug = require("gulp-debug"),
    rename = require("gulp-rename");


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
            ]
        
    },
    prod: {
        
    }
});

/* css task */
gulp.task("init", function () {
    var init = utils.loadTaskConfig("init");
    //console.log(config);
    //console.log(config.local);
    var file = fs.readFileSync(path.resolve(init.envRoot + "/" + config.local.initEnv), { encoding: "utf8" });

    var jsonProperties = JSON.parse(file);

    return gulp.src(init.src, {base: config.root})
        .pipe(debug())
        .pipe(preprocess({ context: jsonProperties }))
        .pipe(rename({
            extname: ""
        }))
        .pipe(gulp.dest("."));
});

