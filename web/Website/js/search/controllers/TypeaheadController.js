/* global angular */

// set up controller and pass data source
// note: a controller is usually destroyed & recreated when the route changes
var InformaTypeaheadController = function($scope, getCompaniesService) {

    $scope.$watch('pageId', function() {
        getCompaniesService.fetchCompanies($scope.pageId).then(function(response) {

            var companies = [];
            companies = $.map(response.data, function(value, index) {
                return value.companyName;
            });

            $scope.companies = companies;

        }).catch(function(reason) {
            console.log("error");
            console.log(reason);
        });
    });
};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaTypeaheadController", ['$scope', 'getCompaniesService', InformaTypeaheadController]);