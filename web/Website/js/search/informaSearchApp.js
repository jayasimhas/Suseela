(function () {
    'use strict';

    //** Including this ngAnimate mock instead of loading ngAnimate because this is smaller **//
    //** (Angular-Strap requires either ngAnimate or this mock **//
    //** source: https://github.com/mgcrea/angular-strap/wiki/ngAnimate-mock **//
    // angular.module('ngAnimate', [])

    //   .factory('$$animateReflow', ['$window', '$timeout', function($window, $timeout) {
    //     var requestAnimationFrame = $window.requestAnimationFrame       ||
    //                                 $window.webkitRequestAnimationFrame ||
    //                                 function(fn) {
    //                                   return $timeout(fn, 10, false);
    //                                 };

    //     var cancelAnimationFrame = $window.cancelAnimationFrame       ||
    //                                $window.webkitCancelAnimationFrame ||
    //                                function(timer) {
    //                                  return $timeout.cancel(timer);
    //                                };
    //     return function(fn) {
    //       var id = requestAnimationFrame(fn);
    //       return function() {
    //         cancelAnimationFrame(id);
    //       };
    //     };
    //   }]);

    var informaSearchApp = angular.module('informaSearchApp', [
        'velir.search',
        'ui.bootstrap'
    ])
    // RH removed 'ngAnimate' from above dependencies b/c of error "realRunner.done is not a function"
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
    
    // ANGULAR UI EXAMPLE
    // angular.module('ui.bootstrap.demo', ['ngAnimate', 'ui.bootstrap']);
    /*angular.module('informaSearchApp').controller('DatepickerDemoCtrl', function ($scope) {
      $scope.today = function() {
        $scope.dt = new Date();
      };
      $scope.today();

      $scope.clear = function() {
        $scope.dt = null;
      };

      $scope.inlineOptions = {
        customClass: getDayClass,
        minDate: new Date(),
        showWeeks: true
      };

      $scope.dateOptions = {
        dateDisabled: disabled,
        formatYear: 'yy',
        maxDate: new Date(2020, 5, 22),
        minDate: new Date(),
        startingDay: 1
      };

      // Disable weekend selection
      function disabled(data) {
        var date = data.date,
          mode = data.mode;
        return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
      }

      $scope.toggleMin = function() {
        $scope.inlineOptions.minDate = $scope.inlineOptions.minDate ? null : new Date();
        $scope.dateOptions.minDate = $scope.inlineOptions.minDate;
      };

      $scope.toggleMin();

      $scope.open1 = function() {
        $scope.popup1.opened = true;
      };

      $scope.open2 = function() {
        $scope.popup2.opened = true;
      };

      $scope.setDate = function(year, month, day) {
        $scope.dt = new Date(year, month, day);
      };

      $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
      $scope.format = $scope.formats[0];
      $scope.altInputFormats = ['M!/d!/yyyy'];

      $scope.popup1 = {
        opened: false
      };

      $scope.popup2 = {
        opened: false
      };

      var tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      var afterTomorrow = new Date();
      afterTomorrow.setDate(tomorrow.getDate() + 1);
      $scope.events = [
        {
          date: tomorrow,
          status: 'full'
        },
        {
          date: afterTomorrow,
          status: 'partially'
        }
      ];

      function getDayClass(data) {
        var date = data.date,
          mode = data.mode;
        if (mode === 'day') {
          var dayToCheck = new Date(date).setHours(0,0,0,0);

          for (var i = 0; i < $scope.events.length; i++) {
            var currentDay = new Date($scope.events[i].date).setHours(0,0,0,0);

            if (dayToCheck === currentDay) {
              return $scope.events[i].status;
            }
          }
        }

        return '';
      }
    });
    */

    

})(); 