/* global angular */

var SavedSearchController = function ($scope, $location, $timeout, $http, searchService, savedSearchService) {
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
            window.indexPopOuts();
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
        console.log(vm.isSaved);
        if(vm.isSaved) {
            window.lightboxController.showLightbox($(e.target).closest('.js-lightbox-modal-trigger'));
        }
    };

    vm.whichIcon = function(isSaved) {
        if(isSaved) {
            return vm.isSaved ? 'is-visible' : null;
        } else {
            return vm.isSaved ? null : 'is-visible';
        }
    };

    vm.whichTrigger = function() {
        return vm.isSaved ? 'js-lightbox-modal-trigger' : 'js-pop-out-trigger';
    };

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SavedSearchController", ['$scope', '$location', '$timeout', '$http', 'searchService', 'savedSearchService', SavedSearchController]);
