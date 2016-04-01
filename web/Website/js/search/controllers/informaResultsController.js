var InformaResultsController = function InformaResultsController($scope, $sanitize, searchService, viewHeadlinesStateService, $timeout, $window) {
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

    $scope.fireBookmark = function(article, event, key) {
        /*  Global bookmark controller fires a second click if clicked element
            isn't the element with the actual click event. (i.e. a child element)
            To make sure we only catch the second click, make sure the element
            has the appropriate class name. */
        if(event.target.nodeName === 'DIV' && event.target.className.indexOf('result__bookmark') >= 0) {
            /*  Angular is faster than the generic bookmark controller; it has to
                wait for an AJAX response before updating the bookmark UI. Use
                $timeout to prevent a race condition.
                TODO - better way to pass bookmark state to generic controller instead */
            $timeout(function() {
                _this.docs[key].isArticleBookmarked = _this.docs[key].isArticleBookmarked ? false : true;
            }, 250);
        }
    };

    $scope.$on('ngRepeatBroadcast1', function(ngRepeatFinishedEvent) {
        window.indexPopOuts();
        window.indexBookmarks();
        window.autoBookmark();
    });

    this.forceRefresh = function() {
        $window.location.reload(false);
    };

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
informaSearchApp.controller("InformaResultsController", ['$scope', '$sanitize','searchService', 'viewHeadlinesStateService',  '$timeout', '$window', InformaResultsController]);
