/* global angular, analytics_data, utag */

var informaSearchApp = angular.module('informaSearchApp');

var InformaResultsController = function InformaResultsController($scope, $sanitize, searchService, viewHeadlinesStateService, $timeout, $window, facetAvailabilityService) {

    var vm = this;

    vm.service = searchService;
    vm.docs = [];

    $scope.headlinesOnly = viewHeadlinesStateService;
    var count = 0;

    $scope.utagAnalytics = function () {
        if (count > 0) {
             var eventDetails = {
                Number_of_Results: '"' + $(".js-searchTotalResults").text() + '"',
                search_Keyword: '"' + $(".js-searchKeyword").text() + '"'
            };
            var dataObj = $.extend(analytics_data, eventDetails);
            if (typeof utag !== 'undefined') {
                utag.link(dataObj);
            }
         }
        count = count + 1;
    };

    $scope.$watchCollection(function () {
        return searchService.getResults();
    }, function () {
        vm.docs = searchService.getResults();
        $scope.utagAnalytics();
    });

    $scope.filterResult = function (url) {
        window.location = url;
        window.location.reload();
    };

    $scope.fireBookmark = function (article, event, key) {
        $timeout(function () {
			vm.docs[key].isArticleBookmarked = vm.docs[key].isArticleBookmarked ? false : true;
		}, 500);
    };

    $scope.$on('refreshPopOuts', function (ngRepeatFinishedEvent) {

		// Enable all facet options when search results land
		// $('.facets__section input').attr('disabled', null);
		facetAvailabilityService.enableFacets();

        window.indexPopOuts();
        window.indexBookmarks();
        window.autoBookmark();
    });

    this.forceRefresh = function () {
        $window.location.reload(false);
    };

};

informaSearchApp.directive('onFinishRender', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            if (scope.$last === true) {
                $timeout(function () {
                    scope.$emit('refreshPopOuts');
                });
            }
        }
    };
});

informaSearchApp.controller("InformaResultsController", ['$scope', '$sanitize', 'searchService', 'viewHeadlinesStateService', '$timeout', '$window', 'facetAvailabilityService', InformaResultsController]);
