(function () {
    'use strict';

    var informaSearchApp = angular.module('informaSearchApp', [
        'velir.search',
        'ui.bootstrap',
        'ngSanitize',
        'ngAnimate'])
        .constant('apiEndpoints', {
            API_BASE: '/api',
            SEARCH_ENDPOINT: '/search'
        })
        .config(['$logProvider', function ($logProvider) {
            // All debugging should be done via $log instead of directly to console
            // This flag disables $log.debug() output
            //$logProvider.debugEnabled(false);
        }])
        .config(['$compileProvider', function ($compileProvider) {
            // Disabled to increase performance
            // https://docs.angularjs.org/api/ng/provider/$compileProvider#debugInfoEnabled
            //$compileProvider.debugInfoEnabled(false);
        }])
    ;

    informaSearchApp.factory('viewHeadlinesStateService', function () {
        var headlines = false;

        return {
            showOnlyHeadlines: function () { return headlines; },
            updateValue: function () {
                headlines = !headlines;
            }
        };
    });

  // factory to handle call to companies service
  // note: a factory lives through the entire application lifecycle
    informaSearchApp.factory('getCompaniesService', ['$http', '$location', function ($http, $location) {
    var fetchCompanies = function() {

        var fullUrl = '/velir/services/TypeAhead.asmx/TypeAheadCompaniesFromSearch';
        if ($location.url()) {
            fullUrl += $location.url();
        }

        return $http({
            method: 'GET',
            url: fullUrl
        });
    }
    return {fetchCompanies : fetchCompanies};
  }]);


  // set up controller and pass data source
  // note: a controller is usually destroyed & recreated when the route changes
  informaSearchApp.controller("InformaTypeaheadController", function($scope, getCompaniesService) {

        getCompaniesService.fetchCompanies().then(function(response) {

                var companies = [];
                companies = $.map(response.data, function(value, index) {

                    return value.CompanyName;

                });

                $scope.companies = companies;

            }).catch(function(reason) {

                console.log("error");
                console.log(reason);

            });
        });


})();
