/* global angular */

var informaSearchApp = angular.module('informaSearchApp');

// set up controller and pass data source
// note: a controller is usually destroyed & recreated when the route changes
informaSearchApp.controller("InformaTypeaheadController", function($scope, getCompaniesService) {

    $scope.$watch('pageId', function () {
        getCompaniesService.fetchCompanies($scope.pageId).then(function (response) {

            var companies = [];
            companies = $.map(response.data, function (value, index) {
                return value.CompanyName;
            });

            $scope.companies = companies;

        }).catch(function (reason) {
            console.log("error");
            console.log(reason);
        });
    });
});
