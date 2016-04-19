﻿/* global _, datesObject, angular */
var InformaFacetController = function ($scope, $location, $http, $anchorScroll, searchService, searchBootstrapper) {
    "use strict";

    // Bind `this` to vm - a representation of the view model
    var vm = this;

    // General Facet stuff
    vm.facetGroups = searchService.getFacetGroups();
    vm.searchService = searchService;
    vm.location = $location;
    vm.anchorScroll = $anchorScroll;
    vm.searchBootstrapper = searchBootstrapper;
    vm.MaxFacetShow = 5;

    // Date Facet stuff
    vm.DateFilters = [
        { label: 'Last 24 hours', key: 'day', selected: false },
        { label: 'Last 3 days', key: 'threedays', selected: false },
        { label: 'Last week', key: 'week', selected: false },
        { label: 'Last month', key: 'month', selected: false },
        { label: 'Last year', key: 'year', selected: false },
        { label: 'Select date range', key: 'custom', selected: false }
    ];

    /* Real talk: the Javascript Date() method is a trash fire. */
    var dToday = function () {
        return new Date().clearTime();
    };

    var jsDates = {
        minus1Year: function() {
            var jsDateToday = new Date();
            return new Date(jsDateToday.setFullYear(jsDateToday.getFullYear() - 1));
        },
        minus1Month: function() {
            var jsDateToday = new Date();
            var m = jsDateToday.getMonth();
            jsDateToday.setMonth(jsDateToday.getMonth() - 1);

            // If still in same month, set date to last day of previous month
            if (jsDateToday.getMonth() == m) {
                jsDateToday.setDate(0);
            }
            return new Date(jsDateToday.setHours(0,0,0));
        },
        minusXdays: function(days) {
            var jsDateToday = new Date();
            return new Date(jsDateToday.setDate(jsDateToday.getDate() - days));
        }
    };

    var formatDateObject = function(d) {
        return (d.getMonth() + 1) + '/' + d.getDate() + '/' + d.getFullYear();
    };

    vm.datesObject = {
        year: formatDateObject(jsDates.minus1Year()),
        day: formatDateObject(jsDates.minusXdays(1)),
        threedays: formatDateObject(jsDates.minusXdays(3)),
        month: formatDateObject(jsDates.minus1Month()),
        week: formatDateObject(jsDates.minusXdays(7))
    };

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

    // On first run, check all date filters against date filter set in URL
    for (var i = 0; i < vm.DateFilters.length; i++) {
        if ($location.search().dateFilterLabel == vm.DateFilters[i].key) {

            // Mark date filter as selected to match URL
            vm.DateFilters[i].selected = true;

            // If date filter is a date range filter...
            if(vm.DateFilters[i].key === 'custom') {
                // ...convert the date data in the URL to `Date`s...
                // example: date=3/29/2015;4/5/2016
                var splitDates = $location.search().date.split(';');
                // ...and update the model so the UI shows the right data.
                $scope.dateValues.dtFrom = new Date(splitDates[0]);
                $scope.dateValues.dtTo = new Date(splitDates[1]);
            }
        }
    }

    $scope.$watch(function() {
        return searchService.getPager();
    }, function() {
        vm.facetGroups = searchService.getFacetGroups();
    }, true);


    //** This collects the user's saved companies **//
    vm.savedCompanies = {};

    vm.saveCompany = function($item, model, label) {
        vm.savedCompanies[$item] = {
            selected: true,
            label: $item
        };
    };

    $scope.removeCompany = function($item, model, label) {
        delete $scope.savedCompanies[$item.label];
    };


    //** This updates the router/url with the latest search parameters **//
    vm.update = function() {
        vm.searchService.getFilter('page').setValue('1');
        var routeBuilder = this.searchService.getRouteBuilder();
        vm.location.search(routeBuilder.getRoute());
        vm.searchService.query();

        //Scroll to the top of the results when a new page is chosen
        vm.location.hash("searchTop");
        vm.anchorScroll();
    };

    vm.facetChange = function(facet) {
        vm.searchService.getFacetGroup(facet.parentId).getFacet(facet.id).selected = facet.selected;
        vm.update();
    };

    // TODO: this comes from a diff search app, and needs jquery to work.
    //       either hook up jq to this controller or move this elsewhere
    vm.scrollTop = function() {
        // var location = jq(".search-facets__header").offset().top;
        //window.scrollTo(0, location - 80);
    };

    vm.hasSelected = function(values) {
        return _.find(values, { selected: true }) ? true : false;
    };

    vm.getFilter = function(filterKey) {
        var filter = vm.searchService.getFilter(filterKey);
        if (!filter) {
            vm.searchBootstrapper.createFilter(filterKey, "");
            filter = vm.searchService.getFilter(filterKey);
        }
        return filter;
    };

    /* This deselects any selected facet checkboxes, clears all facet parameters
        from the search query, and runs the clearDateRange function */
    vm.clearAllFacets = function() {
        var facetClear = this;
        var facetGroups = facetClear.facetGroups;
        _.each(facetGroups, function(group) {
            // vm.clearGroup(group.id)
            var facets = vm.searchService.getFacetGroup(group.id).getSelectedFacets();
            _.each(facets, function(facet) {
                facet.selected = false;
            });
        });
        vm.clearDateRange();
        vm.update();
    };

    vm.clearFilter = function(filterKey) {
        var filter = vm.getFilter(filterKey);
        filter.setValue("");
    };

    /* This clears the date parameters from the search, deselcts any date radio
    buttons, and clears both custom date input fields **/
    vm.clearDateRange = function() {
        var filter = vm.getFilter('date');
        filter.setValue("");
        filter.selected = false;
        var filterDateLabel = vm.getFilter('dateFilterLabel');
        filterDateLabel.setValue("");
        var dates = vm.DateFilters;
        _.each(dates, function(date) {
            date.selected = false;
        });

    };

    vm.getDateFilterLabel = function() {
        var filterDateLabel = vm.getFilter('dateFilterLabel');
        return filterDateLabel._value;
    };

    vm.searchForCompany = function (selectedCompany) {

        //This is not correct right now, should be using facet groups instead
        //will fix later
        var facets = vm.searchService.getFacetGroup('companies').getSelectedFacets();

        var filter = vm.getFilter('companies');

        var companyFilter = selectedCompany;
        var sep = ';';

        for (var i = 0; i < facets.length; i++) {
            companyFilter += sep + facets[i].id;
        }

        filter.setValue(companyFilter);

        vm.update();
    };

    vm.customDateRangeSearch = function(filterKey, startDate, endDate) {

        var filter = vm.getFilter(filterKey);
        var filterDateLabel = vm.getFilter('dateFilterLabel');
        filterDateLabel.setValue('custom');
        if(startDate > 0 && endDate > 0 && startDate < endDate) {
            var date1Unparsed = new Date(startDate);
            var date1 = (date1Unparsed.getMonth() + 1) + '/' +date1Unparsed.getDate() + '/'  + date1Unparsed.getFullYear();

            var date2Unparsed = new Date(endDate);
            var date2 = (date2Unparsed.getMonth() + 1) + '/' + date2Unparsed.getDate() + '/' + date2Unparsed.getFullYear();

            filter.setValue(date1 + ";" + date2);

            vm.update();
        }

    };

    //** This builds date parameters for the search query **//
    vm.dateRangeSearch = function (filterKey, dateFilter) {

        if (dateFilter == 'custom') {
            return;
        }

        var filter = vm.getFilter(filterKey);
        var filterDateLabel = vm.getFilter('dateFilterLabel');

        var startDate = vm.datesObject[dateFilter];
        var endDate = formatDateObject(new Date());

        filterDateLabel.setValue(dateFilter);
        filter.setValue(startDate + ";" + endDate);

        vm.updateSelectedDate(dateFilter);
        vm.update();
    };

    vm.updateSelectedDate = function (dateFilter) {

        for (var i = 0; i < vm.DateFilters.length; i++) {
            if (dateFilter == vm.DateFilters[i].key) {

                vm.DateFilters[i].selected = true;

            } else {

                vm.DateFilters[i].selected = false;

            }
        }
    };

};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaFacetController", ['$scope', '$location', '$http', '$anchorScroll', 'searchService', 'searchBootstrapper', InformaFacetController]);
