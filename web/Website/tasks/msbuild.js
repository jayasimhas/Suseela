var gulp = require("gulp"),
    utils = require("./utils"),
    config = utils.loadConfig(),
    preprocess = require("gulp-preprocess"),
    rename = require("gulp-rename"),
    argv = require('yargs').argv,    
    msbuild = require("gulp-msbuild");


var path = require("path"),
    fs = require("fs");
//init settings
utils.setTaskConfig("msbuild", {
    default: {
        envRoot: config.root + "../../..",
        src: "Informa.sln",
        toolsVersion: 14.0,
        configuration: "Debug-NoTDS"
    },
    prod: {
        
    }
});        

gulp.task("msbuild", function () {
    var msbuildTask = utils.loadTaskConfig("msbuild");

    var filePath = path.resolve(msbuildTask.envRoot + "/" + msbuildTask.src);

    return gulp.src(filePath)
        .pipe(msbuild({
            toolsVersion: msbuildTask.toolsVersion,
            properties: { configuration: msbuildTask.configuration }
        }));
});

