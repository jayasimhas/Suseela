var HeadlineToggleController = function ($scope, viewHeadlinesStateService) {
    "use strict";

    $scope.headlinesOnly = viewHeadlinesStateService;

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("HeadlineToggleController", ['$scope', 'viewHeadlinesStateService', HeadlineToggleController]);
