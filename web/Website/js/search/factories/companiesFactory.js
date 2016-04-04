var informaSearchApp = angular.module('informaSearchApp');

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

    };

    return {fetchCompanies : fetchCompanies};

}]);
