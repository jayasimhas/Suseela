/* global angular */

var EmailRssController = function ($scope, $location) {
    "use strict";


    $scope.$watch(function () {
        return $location.search();
    }, function (params) {
        var params = $location.search();
       
        var nUrl = "";
        for (var idxkey in params) {
            if (idxkey == "sortBy") {
                nUrl = nUrl + "&" + "sortBy=date";
            }

            if (idxkey == "sortOrder") {
                nUrl = nUrl + "&" + "sortOrder=desc";
            }

            if (idxkey != "sortBy" && idxkey != "sortOrder") {
                nUrl = nUrl + "&" + idxkey + "=" + params[idxkey];
            }
        }
				// strip leading &
        if (nUrl.charAt(0) === '&') {
	        nUrl = nUrl.substr(1);
        }
        $scope.currentLocation = "?" + nUrl;
    });

    $scope.testvar = 'hello';

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("EmailRssController", ['$scope', '$location', EmailRssController]);
