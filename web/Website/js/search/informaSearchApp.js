/* global angular, analytics_data */
import { analyticsEvent } from '../controllers/analytics-controller';

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

            // $compileProvider.debugInfoEnabled(false);

        }]);

    informaSearchApp.factory('viewHeadlinesStateService', function () {
        var headlines = false;

        return {
            showOnlyHeadlines: function () { return headlines; },
            updateValue: function () {

				if(!headlines) {
					analyticsEvent( $.extend(analytics_data, {
						event_name: 'search_utility',
						search_utility: 'view_headlines_only'
					}) );
				}

				headlines = !headlines;

            }
        };
    });

	informaSearchApp.factory('facetAvailabilityService', function ($rootScope) {

		var facetsState = false;

        return {
			facetsAreEnabled: function() {
				return facetsState;
			},
            enableFacets: function () {
				facetsState = false;
				$rootScope.facetAvailability = facetsState;
			},
			disableFacets: function () {
				facetsState = true;
				$rootScope.facetAvailability = facetsState;
            },
			toggleFacets: function () {
				facetsState = !facetsState;
				$rootScope.facetAvailability = facetsState;
            }
        };

    });

})();
