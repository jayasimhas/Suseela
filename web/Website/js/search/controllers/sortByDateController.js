/* global angular, analytics_data */
import { analyticsEvent } from '../../controllers/analytics-controller';

var SortByDateController = function ($scope, $location, $timeout, $http, searchService, savedSearchService) {
    "use strict";

    var vm = this;

	vm.resultsSorted = function(sortingAsc) {
		if(sortingAsc) {
			analyticsEvent( $.extend(analytics_data, {
				event_name: 'search_utility',
				search_utility: 'sort_by_date:asc'
			}));
		} else {
			analyticsEvent( $.extend(analytics_data, {
				event_name: 'search_utility',
				search_utility: 'sort_by_date:desc'
			}));
		}
	};

};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SortByDateController", ['$scope', '$location', '$timeout', '$http', 'searchService', 'savedSearchService', SortByDateController]);
