﻿
var InformaFacetController = function ($scope, $location, $http, $anchorScroll, searchService, searchBootstrapper) {
    "use strict";

    // Bind `this` to vm - a represetation of the view model
    var vm = this;

    // var init = function() {

    // General Facet stuff
    vm.facetGroups = searchService.getFacetGroups();
    vm.searchService = searchService;
    vm.location = $location;
    vm.anchorScroll = $anchorScroll;
    vm.searchBootstrapper = searchBootstrapper;
    vm.MaxFacetShow = 5;

    // Date Facet stuff
    vm.CurrentDateSelection = '';
    vm.ValidDates = true;
    vm.DateFilters = [
        { label: 'Last 24 hours', key: 'day', selected: false },
        { label: 'Last 3 days', key: 'threedays', selected: false },
        { label: 'Last week', key: 'week', selected: false },
        { label: 'Last month', key: 'month', selected: false },
        { label: 'Last year', key: 'year', selected: false },
        { label: 'Select date range', key: 'custom', selected: false }
    ];

    //Select a radio button if the search was using a custom search
    var dateArrayLength = vm.DateFilters.length;
    for (var i = 0; i < dateArrayLength; i++) {

        if ($location.search().dateFilterLabel == vm.DateFilters[i].key) {
            vm.CurrentDateSelection = vm.DateFilters[i].key;
            vm.DateFilters[i].selected = true;
        }
    }

    // };

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
        vm.searchService.getFacet(facet.id).selected = facet.selected;
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
        this.update();
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
        vm.currentDateRange = "";
        vm.update();
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

        if (vm.CurrentDateSelection == 'custom') {
            var filter = vm.getFilter(filterKey);
            var filterDateLabel = vm.getFilter('dateFilterLabel');
            filterDateLabel.setValue('custom');

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

        vm.CurrentDateSelection = dateFilter;

        if (dateFilter == 'custom') {
            return;
        }

        var filter = vm.getFilter(filterKey);
        var filterDateLabel = vm.getFilter('dateFilterLabel');

        var startDate = datesObject[dateFilter];
        var endDate = datesObject.day;
        vm.currentDateRange = dateFilter;

        filterDateLabel.setValue(dateFilter);
        filter.setValue(startDate + ";" + endDate);

        vm.updateSelectedDate(dateFilter);
        vm.update();
    };

    vm.updateSelectedDate = function (dateFilter) {
        var dateArrayLength = vm.DateFilters.length;
        for (var i = 0; i < dateArrayLength; i++) {
            if (dateFilter == vm.DateFilters[i].key) {
                vm.CurrentDateSelection = vm.DateFilters[i].key;
                vm.DateFilters[i].selected = true;
            } else {
                vm.DateFilters[i].selected = false;
            }
        }
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

    vm.currentDateRange = vm.getDateFilterLabel();

    // init();

};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaFacetController", ['$scope', '$location', '$http', '$anchorScroll', 'searchService', 'searchBootstrapper', InformaFacetController]);
