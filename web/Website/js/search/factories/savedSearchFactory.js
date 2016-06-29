var informaSearchApp = angular.module('informaSearchApp');

// factory to handle call to companies service
// note: a factory lives through the entire application lifecycle
informaSearchApp.factory('savedSearchService', ['$http', '$location', function ($http, $location) {

    var isSaved = function () {
        var fullUrl = '/api/SavedSearches?url=' + escape($location.url());

        return $http({
            method: 'GET',
            url: fullUrl
        });

    };

    return {
        isSaved: isSaved
    };

}]);
