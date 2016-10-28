/* global angular */

var SavedSearchController = function ($scope, $location, $timeout, $http, searchService, savedSearchService) {
    "use strict";

    var vm = this;

    vm.searchService = searchService;
    $scope.searchIsSaved = false;

    $scope.oneClickSaveFocus = false;

    // A user can land on the search page from a "1-click Subscribe" link in an email
    // This handles detection of that query parameter, triggers appropriate UI changes
    function openSaveSearchIf1Click() {

        var urlQuery = $location.search();
        var clickKey = Object.keys(urlQuery).filter(function (cur) {
            return cur.toLowerCase() === "1click";
        });

        if (clickKey.length > 0 && urlQuery[clickKey[0]] === "1") {
            jQuery(".js-save-search").click();
            $location.search(clickKey[0], null);
            $scope.oneClickSaveFocus = true;
        }
    }

    $timeout(function() {
        openSaveSearchIf1Click();
    }, 1000);

    $scope.$watch(function () {
        return searchService.getPager();
    }, function () {
        $('.js-save-search-url').val(window.location.hash.substring(1));
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
            window.lightboxController.showLightbox($(e.target).closest('.angular-lightbox-modal-trigger'));
        }
    };

	vm.searchIsSaved = function() {
		$scope.searchIsSaved = true;
		$scope.$apply();
	};
};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SavedSearchController", ['$scope', '$location', '$timeout', '$http', 'searchService', 'savedSearchService', SavedSearchController]);
