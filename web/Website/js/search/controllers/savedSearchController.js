/* global angular */

var SavedSearchController = function ($scope, $location, searchService, savedSearchService) {
    "use strict";

    var vm = this;

    vm.searchService = searchService;
    $scope.isSaved = false;

    $scope.$watch(function () {
        return searchService.getPager();
    }, function () {
        if ($scope.isAuthenticated) {
            savedSearchService.isSaved().then(function (response) {
                $scope.isSaved = response.data;
            });
        }
    }, true);

    $scope.toggleSavedSearch = function() {
        if($scope.isSaved) {
            $scope.isSaved = false;
        }
    };

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SavedSearchController", ['$scope', '$location', 'searchService', 'savedSearchService', SavedSearchController]);
