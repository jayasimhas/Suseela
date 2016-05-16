/* global angular */

var SavedSearchController = function ($scope, $location, $timeout, $http, searchService, savedSearchService) {
    "use strict";

    var vm = this;

    vm.searchService = searchService;
    vm.isSaved = false;

    vm.testValue = savedSearchService.testVal;

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

    vm.saveSearch = function() {
        // Helps ng-class "know" the correct state of vm.isSaved
        // When click events are triggered by vanilla JS, expressions don't
        // always update as they should.
        $timeout(function() {
            vm.isSaved = true;
        }, 500);
    };

    vm.unsaveSearch = function() {
        vm.isSaved = false;
    };

    vm.lightboxCheck = function(e) {
        if(vm.isSaved) {
            window.lightboxController.showLightbox($(e.target).closest('.js-lightbox-modal-trigger'));
        }
    };

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SavedSearchController", ['$scope', '$location', '$timeout', '$http', 'searchService', 'savedSearchService', SavedSearchController]);
