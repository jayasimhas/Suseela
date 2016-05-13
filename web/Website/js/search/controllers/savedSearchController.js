/* global angular */

var SavedSearchController = function ($scope, $location, $http, searchService, savedSearchService) {
    "use strict";

    var vm = this;

    vm.searchService = searchService;
    vm.isSaved = false;
    
    $scope.$watch(function () {
        return searchService.getPager();
    }, function () {
        $scope.title = searchService.getFilter('q').getValue();
        $scope.currentLocation = $location.url();
        if ($scope.isAuthenticated) {
            savedSearchService.isSaved().then(function (response) {
                vm.isSaved = response.data;
            });
        }
    }, true);

    vm.unsaveSearch = function() {
        // If the search isn't saved, don't do anything.
        // Let the form controller handle it
        if(vm.isSaved) {
            console.log($scope.currentLocation);
            $http({
                method: 'DELETE',
                url: '/api/SavedSearches',
                data: {
                    'url': $scope.currentLocation
                },
                headers: {
                    "Content-Type": "application/json"
                }
            }).then(function successCallback(response) {
                vm.isSaved = false;
            }, function errorCallback(response) {
                console.log(response);
            });
        }
    };

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SavedSearchController", ['$scope', '$location', '$http', 'searchService', 'savedSearchService', SavedSearchController]);
