var informaDatesController = function ($scope, $location) {
  'use strict';

  // Create placeholder values for From: and To: date values
  $scope.dateValues = {
      dtFrom: '',
      dtTo: ''
  };

  // need to differentiate the 2 datepickers
  $scope.datepickers = {
      dtFrom: false,
      dtTo: false
  };

  // grab today and inject into field
  $scope.today = function() {
    $scope.dateValues.dtFrom = new Date();
    $scope.dateValues.dtTo = new Date();
  };
  
  // run today() function
  $scope.today();

  // setup clear
  $scope.clear = function () {
    $scope.dateValues.dtFrom = '';
    $scope.dateValues.dtTo = '';
  };

  $scope.fromDateOptions = {
    showWeeks: false, 
    formatDayHeader: 'EEE', 
    formatDay: 'd',
    startingDay: 0, // Sunday
    // dateDisabled: disabled,
    formatYear: 'yy',
    maxDate: new Date(),
    minDate: new Date(1900, 1, 1)
  };

  $scope.toDateOptions = {
    showWeeks: false, 
    formatDayHeader: 'EEE', 
    formatDay: 'd',
    startingDay: 0, // Sunday
    // dateDisabled: disabled,
    formatYear: 'yy',
    maxDate: new Date(),
    minDate: new Date(1900, 1, 1)
  };

  // open min-cal
  $scope.open = function($event, which) {
    $event.preventDefault();
    $event.stopPropagation();

    // Datepicker popups will close when clicked outside, but it's possible to
    // open both at the same time. This prevents that.
    switch (which) {
      case 'dtFrom':
        $scope.datepickers.dtFrom = true;
        $scope.datepickers.dtTo = false;
        break;
      case 'dtTo':
        $scope.datepickers.dtFrom = false;
        $scope.datepickers.dtTo = true;
        break;
      default:
        $scope.datepickers.dtFrom = false;
        $scope.datepickers.dtTo = false;
        break;
    }
  };

  // $scope.$watch('dateValues.dtFromValue', function () {
  //     $scope.toDateOptions.minDate = $scope.dateValues.dtFromValue;
  // });

  // $scope.$watch('dateValues.dtToValue', function () {
  //     $scope.fromDateOptions.maxDate = $scope.dateValues.dtToValue;
  // });

};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaDatesController", ["$scope", "$location", informaDatesController]);