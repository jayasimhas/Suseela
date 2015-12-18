// this file is optional and is for settings that should not be checked into git.
// to use, copy the contents into local.js

module.exports = {

    // hostname to be used with browserSync for proxy.
    // http://www.browsersync.io/docs/options/#option-proxy
    // omit or set to false for static sites
    "hostname": "informa.sfine.velir.com",

    // port that browsersync will run on
    // http://www.browsersync.io/docs/options/#option-port
    "browserSyncPort": 8080,

    // what gulp task to run when you type "gulp"
    "defaultTask": "dev",

};
