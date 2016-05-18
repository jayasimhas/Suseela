/* global angular */

var SavedSearchController = function ($scope, $location, $timeout, $http, searchService, savedSearchService) {
    "use strict";

    var vm = this;

    vm.searchService = searchService;
    $scope.searchIsSaved = false;

    $scope.$watch(function () {
        return searchService.getPager();
    }, function () {
        $scope.title = searchService.getFilter('q').getValue();
        $scope.currentLocation = $location.url();
        if ($scope.isAuthenticated) {
            savedSearchService.isSaved().then(function (response) {
                $scope.searchIsSaved = response.data;
            });
        }
    }, true);

    // Depending on how quickly Angular bootstraps, some lightbox trigger classes
    // might not be added before the lightbox event listners are bound. This
    // manually fires the Saved Search lightbox, in case the normal listener wasn't
    // bound in time.
    vm.showLightbox = function(e) {
        if($scope.searchIsSaved) {
            window.lightboxController.showLightbox($(e.target).closest('.js-lightbox-modal-trigger'));
        }
    };

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SavedSearchController", ['$scope', '$location', '$timeout', '$http', 'searchService', 'savedSearchService', SavedSearchController]);
