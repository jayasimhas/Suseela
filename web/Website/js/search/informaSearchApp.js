(function () {
    'use strict';

    var informaSearchApp = angular.module('informaSearchApp', [
        'velir.search',
        'ui.bootstrap'   ])
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
      ["$scope", "$location", function ($scope, $location) {

          $scope.dateValues = {
              dtFromValue: '',
              dtToValue: ''
          };

      // need to differentiate the 2 datepickers
          $scope.datepickers = {
              dtFrom: false,
              dtTo: false
      }
 
      // grab today and inject into field
          $scope.today = function () {

              if ($location.search().dateFilterLabel == 'custom') {
                 
                  if ($location.search().date.indexOf(";") > -1) {
                    
                      var dates = $location.search().date.split(";");
                
                      if (dates.length == 2) {

                          var date1 = new Date(dates[0]);
                          $scope.dateValues.dtFromValue = (date1.getMonth() + 1) + "/" + date1.getDate() + "/" + date1.getFullYear().toString().substring(2, 4);

                          var date2 = new Date(dates[1]);
                          $scope.dateValues.dtToValue = (date2.getMonth() + 1) + "/" + date2.getDate() + "/" + date2.getFullYear().toString().substring(2, 4);

                          return;
                      }
                  }
              } 

              $scope.dateValues.dtFromValue = "mm/dd/yy";
              $scope.dateValues.dtToValue = "mm/dd/yy";          
      };
      
      // run today() function
      $scope.today();

      // setup clear
      $scope.clear = function () {
        $scope.dt = null;
        //$scope.dateValues.dtFromValue = "mm/dd/yyyy";
        //$scope.dateValues.dtToValue = "mm/dd/yyyy";
      };

      $scope.fromDateOptions = {
        // dateDisabled: disabled,
        showWeeks: false, 
        formatDayHeader: 'EEE', 
        formatDay: 'd',
        startingDay: 1
      };

      $scope.toDateOptions = {
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

      $scope.$watch('dateValues.dtFromValue', function () {
          $scope.toDateOptions.minDate = $scope.dateValues.dtFromValue;
      });

      $scope.$watch('dateValues.dtToValue', function () {
          $scope.fromDateOptions.maxDate = $scope.dateValues.dtToValue;
      });

    }]);


  
  // factory to handle call to companies service
  // note: a factory lives through the entire application lifecycle
  informaSearchApp.factory('getCompaniesService', ['$http', function($http) {
    var fetchCompanies = function() {
      return $http({
          method: 'GET',
          url: '/velir/services/TypeAhead.asmx/TypeAheadCompanies'
      });
    }
    return {fetchCompanies : fetchCompanies};
  }]);


  // set up controller and pass data source
  // note: a controller is usually destroyed & recreated when the route changes
  informaSearchApp.controller("InformaTypeaheadController", 
    function($scope, getCompaniesService){

      getCompaniesService.fetchCompanies()

      .then(function(response) {

        var companies = [];
        companies = $.map( response.data, function(value, index) {

          return value.CompanyName;

        });

        $scope.companies = companies;
      })

      .catch( function(reason) {

        console.log("error");
        console.log(reason);

      })

  });


})(); 