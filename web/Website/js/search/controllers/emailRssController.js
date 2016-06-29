var EmailRssController = function ($scope, $location) {
    "use strict";


    $scope.$watch(function () { return $location.search() }, function (params) {
       

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
        $scope.currentLocation = "?" + nUrl;
    });

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("EmailRssController", ['$scope', '$location', EmailRssController]);
