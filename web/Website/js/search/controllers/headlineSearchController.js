/* global analytics_data */
import { analyticsEvent } from '../../controllers/analytics-controller';

var HeadlineSearchController = function ($scope, searchService, searchBootstrapper) {
    "use strict";

    var vm = this;

    vm.searchService = searchService;
    vm.searchBootstrapper = searchBootstrapper;

    vm.update = function () {
        var filter = vm.searchService.getFilter('headlinesOnly');

        if (!filter) {
            vm.searchBootstrapper.createFilter('headlinesOnly', '1');
        } else {
            if (filter._value === '1') {

				filter.setValue('');

            } else {

                filter.setValue('1');
				analyticsEvent( $.extend(analytics_data, {
					event_name: 'search_utility',
					search_utility: 'search_headlines_only'
				}) );

            }
        }
    };

    vm.init = function () {
        var filter = vm.searchService.getFilter('headlinesOnly');
        return filter !== undefined;
    }
};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("HeadlineSearchController", ['$scope', 'searchService', 'searchBootstrapper', HeadlineSearchController]);
