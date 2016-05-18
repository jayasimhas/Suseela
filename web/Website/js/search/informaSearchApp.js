(function () {
    'use strict';

    var informaSearchApp = angular.module('informaSearchApp', [
        'velir.search',
        'ui.bootstrap',
        'ngSanitize',
        'ngAnimate'
        ]).constant('apiEndpoints', {
            API_BASE: '/api',
            SEARCH_ENDPOINT: '/search'
        }).config(['$logProvider', function ($logProvider) {
            // All debugging should be done via $log instead of directly to console
            // This flag disables $log.debug() output
            //$logProvider.debugEnabled(false);
        }]).config(['$compileProvider', function ($compileProvider) {
            
            // https://docs.angularjs.org/api/ng/provider/$compileProvider#debugInfoEnabled
            // UNCOMMENT THE LINE BELOW IN PRODUCTION FOR PERFORMANCE GAINS

            $compileProvider.debugInfoEnabled(false);

        }]);

    informaSearchApp.factory('viewHeadlinesStateService', function () {
        var headlines = false;

        return {
            showOnlyHeadlines: function () { return headlines; },
            updateValue: function () {
                headlines = !headlines;
            }
        };
    });

})();
