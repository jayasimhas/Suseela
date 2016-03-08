(function () {
    'use strict';

    var informaSearchApp = angular.module('informaSearchApp', [
        'velir.search',
        'ui.bootstrap'
    ])
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
            showOnlyHeadlines: function () { return headlines; }
            ,
            updateValue: function () {
                
                headlines = !headlines;
            }
        }
    });


// TODO: This should be split off into a new file inside the controllers directory
    informaSearchApp.controller("InformaDatesController",
      ["$scope", function($scope){

      // need to differentiate the 2 datepickers
      $scope.datepickers = {
        dtFrom: false,
        dtTo: false
      }
 
      // grab today and inject into field
      $scope.today = function() {
        $scope.dtFrom = new Date();
        $scope.dtTo = new Date();
      };
      
      // run today() function
      $scope.today();

      // setup clear
      $scope.clear = function () {
        $scope.dt = null;
        // $scope.dtFrom = null;
        // $scope.dtTo = null;
      };

      $scope.dateOptions = {
        // dateDisabled: disabled,
        showWeeks: false, 
        formatDayHeader: 'EEE', 
        formatDay: 'd',
        startingDay: 1
      };

      // open min-cal
      $scope.open = function($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers[which] = true;
      };

      
    }]);



})(); 