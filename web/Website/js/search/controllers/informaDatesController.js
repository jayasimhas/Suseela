var informaDatesController = function ($scope, $location) {
  'use strict';

  /*
    $scope.dateValues and $scope.datepickers are in InformaFacetsController.js
    Using ng-if creates a new scope for the data in this controller, which
    causes the date values to reset on change. By storing in the parent,
    those values survive any scope resets here.
  */
  
  // grab today and inject into field
  $scope.today = function() {
    $scope.dateValues.dtFrom = new Date();
    $scope.dateValues.dtTo = new Date();
  };

  // run today() function
  // $scope.today();

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
    dateDisabled: disabledFrom,
    formatYear: 'yy',
    maxDate: new Date(),
    minDate: new Date(1900, 1, 1)
  };

  $scope.toDateOptions = {
    showWeeks: false,
    formatDayHeader: 'EEE',
    formatDay: 'd',
    startingDay: 0, // Sunday
    dateDisabled: disabledTo,
    formatYear: 'yy',
    maxDate: new Date(),
    minDate: new Date(1900, 1, 1)
  };

  // Prevent user from selecting date if date is in the future, OR if the date
  // is later than the selected 'to' date. 'from' can't be later than 'to'
  function disabledFrom(data) {
      var date = data.date;
      var tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);

      // If no end-time exists, use a very large number so all timestamps are valid
      var toTimestamp = $scope.dateValues.dtTo ? $scope.dateValues.dtTo.getTime() : Math.pow(99, 9);
      return (date.getTime() > tomorrow.getTime()
        || date.getTime() > toTimestamp);
  };

  // Prevent user from selecting date if date is in the future, OR if date
  // is before selected 'from' date. 'to' must be later than 'from'
  function disabledTo(data) {
      var date = data.date;
      var tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);

      // If no start-time exists, use a very large number so all timestamps are valid
      var fromTimestamp = $scope.dateValues.dtFrom ? $scope.dateValues.dtFrom.getTime() : 0;
      return (date.getTime() > tomorrow.getTime()
        || date.getTime() < fromTimestamp);
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

};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaDatesController", ["$scope", "$location", informaDatesController]);
