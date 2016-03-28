
var InformaFacetController = function($scope, $location, $http, searchService, searchBootstrapper, getCompaniesService) {
    "use strict";

    var _this = this;

    var init = function() {

        // General Facet stuff
        _this.facetGroups = searchService.getFacetGroups();
        _this.searchService = searchService;
        _this.location = $location;
        _this.searchBootstrapper = searchBootstrapper;
        _this.MaxFacetShow = 5;
        _this.CompanyList = getCompaniesService.fetchCompanies();

        // Date Facet stuff
        _this.CurrentDateSelection = '';
        _this.currentDateRange = _this.getDateFilterLabel();
        _this.ValidDates = true;
        _this.DateFilters = [
            { label: 'Last 24 hours', key: 'day', selected: false },
            { label: 'Last 3 days', key: 'threedays', selected: false },
            { label: 'Last week', key: 'week', selected: false },
            { label: 'Last month', key: 'month', selected: false },
            { label: 'Last year', key: 'year', selected: false },
            { label: 'Select date range', key: 'custom', selected: false }
        ];

        //Select a radio button if the search was using a custom search
        var dateArrayLength = _this.DateFilters.length;
        for (var i = 0; i < dateArrayLength; i++) {

            if ($location.search().dateFilterLabel == _this.DateFilters[i].key) {
                _this.CurrentDateSelection = _this.DateFilters[i].key;
                _this.DateFilters[i].selected = true;
            }
        }

    };

    $scope.$watch(function() {
        return searchService.getPager();
    }, function() {
        _this.facetGroups = searchService.getFacetGroups();
    }, true);


    //** This collects the user's saved companies **//
    $scope.savedCompanies = {};

    $scope.saveCompany = function($item, model, label) {
        // console.log("selected: ", $item);

        $scope.savedCompanies[$item] = {
            selected: true,
            label: $item
        };

        // console.log($scope.savedCompanies);
    };

    $scope.removeCompany = function($item, model, label) {
        // console.log("selected: ", $item);

        delete $scope.savedCompanies[$item.label];

        // console.log($scope.savedCompanies);
    };


    //** This updates the router/url with the latest search parameters **//
    _this.update = function() {
        _this.searchService.getFilter('page').setValue('1');
        var routeBuilder = this.searchService.getRouteBuilder();
        _this.location.search(routeBuilder.getRoute());
        _this.searchService.query();
        // _this.scrollTop();
    };

    _this.facetChange = function(facet) {
        _this.searchService.getFacet(facet.id).selected = facet.selected;
        _this.update();
    };

    // TODO: this comes from a diff search app, and needs jquery to work.
    //       either hook up jq to this controller or move this elsewhere
    _this.scrollTop = function() {
        // var location = jq(".search-facets__header").offset().top;
        //window.scrollTo(0, location - 80);
    };

    _this.hasSelected = function(values) {
        return _.find(values, { selected: true }) ? true : false;
    };

    _this.getFilter = function(filterKey) {
        var filter = _this.searchService.getFilter(filterKey);
        if (!filter) {
            _this.searchBootstrapper.createFilter(filterKey, "");
            filter = _this.searchService.getFilter(filterKey);
        }
        return filter;
    };

    //** This deselects any selected facet checkboxes, clears all facet parameters from the search query, and runs the clearDateRange function **//
    _this.clearAllFacets = function() {
        var facetClear = this;
        var facetGroups = facetClear.facetGroups;
        _.each(facetGroups, function(group) {
            // _this.clearGroup(group.id)
            var facets = _this.searchService.getFacetGroup(group.id).getSelectedFacets();
            _.each(facets, function(facet) {
                facet.selected = false;
            });
        });
        _this.clearDateRange();
        this.update();
    };

    _this.clearFilter = function(filterKey) {
        var filter = _this.getFilter(filterKey);
        filter.setValue("");
    };

    //** This clears the date parameters from the search, deselcts any date radio buttons, and clears both custom date input fields **//
    _this.clearDateRange = function() {
        var filter = _this.getFilter('date');
        filter.setValue("");
        filter.selected = false;
        var filterDateLabel = _this.getFilter('dateFilterLabel');
        filterDateLabel.setValue("");
        var dates = _this.DateFilters;
        _.each(dates, function(date) {
            date.selected = false;
        });
        _this.currentDateRange = "";
        _this.update();
    };

    _this.getDateFilterLabel = function() {
        var filterDateLabel = _this.getFilter('dateFilterLabel');
        return filterDateLabel._value;
    };

    _this.searchForCompany = function (selectedCompany) {

        //This is not correct right now, should be using facet groups instead
        //will fix later
        var facets = _this.searchService.getFacetGroup('companies').getSelectedFacets();

        var filter = _this.getFilter('companies');

        var companyFilter = selectedCompany;
        var sep = ';';

        for (var i = 0;i < facets.length; i++) {
            companyFilter += sep + facets[i].id;
        }

        filter.setValue(companyFilter);

        _this.update();
    };

    _this.customDateRangeSearch = function(filterKey, startDate, endDate) {

        if (_this.CurrentDateSelection == 'custom') {
            var filter = _this.getFilter(filterKey);
            var filterDateLabel = _this.getFilter('dateFilterLabel');
            filterDateLabel.setValue('custom');

            var date1Unparsed = new Date(startDate);
            var date1 = (date1Unparsed.getMonth() + 1) + '/' +date1Unparsed.getDate() + '/'  + date1Unparsed.getFullYear();

            var date2Unparsed = new Date(endDate);
            var date2 = (date2Unparsed.getMonth() + 1) + '/' + date2Unparsed.getDate() + '/' + date2Unparsed.getFullYear();

            filter.setValue(date1 + ";" + date2);

            _this.update();
        }

    };

    //** This builds date parameters for the search query **//
    _this.dateRangeSearch = function (filterKey, dateFilter) {

        _this.CurrentDateSelection = dateFilter;

        if (dateFilter == 'custom') {
            return;
        }

        var filter = _this.getFilter(filterKey);
        var filterDateLabel = _this.getFilter('dateFilterLabel');

        var startDate = datesObject[dateFilter];
        var endDate = datesObject.day;
        _this.currentDateRange = dateFilter;

        filterDateLabel.setValue(dateFilter);
        filter.setValue(startDate + ";" + endDate);

        _this.updateSelectedDate(dateFilter);
        _this.update();
    };

    _this.updateSelectedDate = function (dateFilter) {
        var dateArrayLength = _this.DateFilters.length;
        for (var i = 0; i < dateArrayLength; i++) {
            if (dateFilter == _this.DateFilters[i].key) {
                _this.CurrentDateSelection = _this.DateFilters[i].key;
                _this.DateFilters[i].selected = true;
            } else {
                _this.DateFilters[i].selected = false;
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

    init();

};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaFacetController", ['$scope', '$location', '$http', 'searchService', 'searchBootstrapper', 'getCompaniesService', InformaFacetController]);
