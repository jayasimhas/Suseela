(function () {
    'use strict';

    //'ngTouch',
    //       'achqAppServices',
    //       'achqAppControllers',
    //       'velirPagerFactory',
    //       'velirSearchMapper',
    //       'velirSkip'

    var informaSearchApp = angular.module('informaSearchApp', [
        'velir.search'
    ])
        .constant('apiEndpoints', {
            API_BASE: '/api',
            SEARCH_ENDPOINT: '/search'
        })
        .config(['$logProvider', function ($logProvider) {
            // All debugging should be done via $log instead of directly to console
            // This flag disables $log.debug() output
            $logProvider.debugEnabled(false);
        }])
        .config(['$compileProvider', function ($compileProvider) {
            // Disabled to increase performance
            // https://docs.angularjs.org/api/ng/provider/$compileProvider#debugInfoEnabled
            //$compileProvider.debugInfoEnabled(false);
        }])
    ;

})();