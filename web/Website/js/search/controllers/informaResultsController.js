var InformaResultsController = function InformaResultsController($scope, searchService, viewHeadlinesStateService) {
    var _this = this;

    this.service = searchService;
    this.docs = [];

    $scope.headlinesOnly = viewHeadlinesStateService;

    $scope.$watchCollection(function () {
        return searchService.getResults();
    }, function () {
        _this.docs = searchService.getResults();
    });

    $scope.filterResult = function (url) {
        window.location = url;
        window.location.reload();
    };

    $scope.fireBookmark = function() {
        console.log(window.globaltest);
    };

    $scope.$on('ngRepeatBroadcast1', function(ngRepeatFinishedEvent) {
        window.indexPopOuts();
        window.indexBookmarks();
    });

};
var informaSearchApp = angular.module('informaSearchApp').directive('onFinishRender', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            if (scope.$last === true) {
            $timeout(function () {
                scope.$emit(attr.broadcasteventname ? attr.broadcasteventname : 'ngRepeatFinished');
            });
            }
        }
    };
});
informaSearchApp.controller("InformaResultsController", ['$scope', 'searchService', 'viewHeadlinesStateService', InformaResultsController]);
