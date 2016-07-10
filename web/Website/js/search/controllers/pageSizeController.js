/* global angular, analytics_data */
import { analyticsEvent } from '../../controllers/analytics-controller';

var PageSizeController = function ($scope, $location, $anchorScroll, searchService, searchBootstrapper) {
    "use strict";

    var perPageKey = 'perPage';

    var vm = this;

    vm.location = $location;
    vm.anchorScroll = $anchorScroll;
    vm.searchService = searchService;
    vm.searchBootstrapper = searchBootstrapper;

    vm.update = function (pageSize) {
        var filter = vm.searchService.getFilter(perPageKey);

        if (!filter) {
            vm.searchBootstrapper.createFilter(perPageKey, pageSize);
        } else {
            filter.setValue(pageSize);
        }

        vm.searchService.getFilter('page').setValue('1');
        var routeBuilder = this.searchService.getRouteBuilder();
        vm.location.search(routeBuilder.getRoute());
        vm.searchService.query();

        //Scroll to the top of the results when a new page is chosen
        vm.location.hash("searchTop");
        vm.anchorScroll();

		analyticsEvent( $.extend(analytics_data, {
			event_name: 'search_utility',
			search_utility: 'results_per_page_' + pageSize
		}));

    };

    vm.init = function() {
        var filter = vm.searchService.getFilter(perPageKey);

        $scope.pageSize = filter ? filter._value : '10';
    };
};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("PageSizeController", ['$scope', '$location', '$anchorScroll', 'searchService', 'searchBootstrapper', PageSizeController]);
