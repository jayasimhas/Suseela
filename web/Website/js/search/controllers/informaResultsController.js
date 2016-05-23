/* global angular */
var informaSearchApp = angular.module('informaSearchApp');

var InformaResultsController = function InformaResultsController($scope, $sanitize, searchService, viewHeadlinesStateService, $timeout, $window) {

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
    }

    $scope.$watchCollection(function () {
        return searchService.getResults();
    }, function () {
        vm.docs = searchService.getResults();
    });

    $scope.filterResult = function (url) {
        window.location = url;
        window.location.reload();
    };

    $scope.fireBookmark = function (article, event, key) {
        /*  Global bookmark controller fires a second click if clicked element
            isn't the element with the actual click event. (i.e. a child element)
            To make sure we only catch the second click, make sure the element
            has the appropriate class name. */
        if (event.target.nodeName === 'DIV' && event.target.className.indexOf('result__bookmark') >= 0) {
            /*  Angular is faster than the generic bookmark controller; it has to
                wait for an AJAX response before updating the bookmark UI. Use
                $timeout to prevent a race condition.
                TODO - better way to pass bookmark state to generic controller instead */
            $timeout(function () {
                vm.docs[key].isArticleBookmarked = vm.docs[key].isArticleBookmarked ? false : true;
            }, 250);
        }
    };

    $scope.$on('refreshPopOuts', function (ngRepeatFinishedEvent) {
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

informaSearchApp.controller("InformaResultsController", ['$scope', '$sanitize', 'searchService', 'viewHeadlinesStateService', '$timeout', '$window', InformaResultsController]);
