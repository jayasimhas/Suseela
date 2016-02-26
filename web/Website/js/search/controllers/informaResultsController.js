﻿var InformaResultsController = function InformaResultsController($scope, searchService, viewHeadlinesStateService) {
    var _this = this;

    this.service = searchService;
    this.docs = [];

    $scope.headlinesOnly = viewHeadlinesStateService;

    $scope.$watchCollection(function () {
        return searchService.getResults();
    }, function () {
        _this.docs = searchService.getResults();
    });

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaResultsController", ['$scope', 'searchService', 'viewHeadlinesStateService', InformaResultsController]);