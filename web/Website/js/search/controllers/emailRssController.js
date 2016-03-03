var EmailRssController = function ($scope, $location) {
    "use strict";

    $scope.currentLocation = $location.url();
    $scope.$watch(function () { return $location.search() }, function (params) {
        console.log(params);
    });

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("EmailRssController", ['$scope', '$location' , EmailRssController]);
