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
gulp.task("init", function() {
    console.log(path.resolve("../../config/env"));          
    console.log(config.env);
    var properties = utils.loadTaskConfig("init");    
    var file = fs.readFileSync(path.resolve(properties.envRoot + "/" + config.local.env), { encoding: "utf8" });

    var jsonProperties = JSON.parse(file);

    console.log(properties.src[1]);

    console.log(jsonProperties);
    return gulp.src(properties.src, {base: config.root})
        .pipe(debug())
        .pipe(preprocess({ context: jsonProperties }))
        .pipe(rename({
            extname: ""
        }))
    .pipe(gulp.dest("."));
});

