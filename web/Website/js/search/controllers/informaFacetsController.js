
var InformaFacetController = function ($scope, $location, $http, searchService, searchBootstrapper) {
    "use strict";

    var _this = this;

    var init = function () {

        // General Facet stuff
        _this.facetGroups = searchService.getFacetGroups();
        _this.searchService = searchService;
        _this.location = $location;
        _this.searchBootstrapper = searchBootstrapper;
        _this.MaxFacetShow = 5;

        // Date Facet stuff
        _this.currentDateRange = _this.getDateFilterLabel();
        _this.ValidDates = true;
        _this.DateFilters = [
             { label: 'Last 24 hours', key: 'day', selected: false },
             { label: 'Last 3 days', key: 'threedays', selected: false },
             { label: 'Last week', key: 'week', selected: false },
             { label: 'Last month', key: 'month', selected: false },
             { label: 'Last year', key: 'year', selected: false }
        ];

        //_this.CustomStartDate = jq("#facet-by-start-date");
        //_this.CustomEndDate = jq("#facet-by-end-date");

        //_this.CustomStartDate = '';
        //_this.CustomEndDate = '';

        //var filter = _this.getFilter('publicationdate');
        //if (filter._value != '') {

        //    var split = filter._value.split(';');
        //    _this.CustomStartDate.val(split[0]);
        //    _this.CustomEndDate.val(split[1]);
        //    //hack to get dates to render correctly
        //    _this.CustomEndDate.focus();
        //    _this.CustomStartDate.focus();
        //    _this.setCustomDateIsActive();
        //}
    };

    $scope.$watch(function () {
        return searchService.getPager();
    }, function () {
        _this.facetGroups = searchService.getFacetGroups();
    }, true);

    //** This updates the router/url with the latest search parameters **//
    _this.update = function () {
        _this.searchService.getFilter('page').setValue('1');
        var routeBuilder = this.searchService.getRouteBuilder();
        _this.location.search(routeBuilder.getRoute());
        _this.searchService.query();
        // _this.scrollTop();
    }

    _this.facetChange = function (facet) {
        _this.searchService.getFacet(facet.id).selected = facet.selected;
        _this.update();
    }

    _this.scrollTop = function () {
        // var location = jq(".search-facets__header").offset().top;
        //window.scrollTo(0, location - 80);
    }

    // _this.clearGroup = function (groupId) {
    //     var facets = _this.searchService.getFacetGroup(groupId).getSelectedFacets();
    //     _.each(facets, function (facet) {
    //         facet.selected = false;
    //     });
    //     this.update();
    // }

    _this.hasSelected = function (values) {
        return _.find(values, { selected: true }) ? true : false;
    };

    _this.getFilter = function (filterKey) {
        var filter = _this.searchService.getFilter(filterKey);
        if (!filter) {
            _this.searchBootstrapper.createFilter(filterKey, "");
            filter = _this.searchService.getFilter(filterKey);
        }
        return filter;
    }

    //** This deselects any selected facet checkboxes, clears all facet parameters from the search query, and runs the clearDateRange function **//
    _this.clearAllFacets = function () {
        var facetClear = this;
        var facetGroups = facetClear.facetGroups;
        _.each(facetGroups, function (group) {
            // _this.clearGroup(group.id)
            var facets = _this.searchService.getFacetGroup(group.id).getSelectedFacets();
            _.each(facets, function (facet) {
                facet.selected = false;
            });
        });
        _this.clearDateRange();
        this.update();
    };

    _this.clearFilter = function (filterKey) {
        var filter = _this.getFilter(filterKey);
        filter.setValue("");
    }

    //** This clears the date parameters from the search, deselcts any date radio buttons, and clears both custom date input fields **//
    _this.clearDateRange = function () {
        var filter = _this.getFilter('date');
        console.log("removing: ", filter);
        filter.setValue("");
        filter.selected = false;
        var filterDateLabel = _this.getFilter('dateFilterLabel');
        console.log("removing: ", filterDateLabel);
        filterDateLabel.setValue("");
        var dates = _this.DateFilters;
         _.each(dates, function(date) {
            date.selected = false;
        });
        _this.currentDateRange = "";
        _this.update();
    }    

    _this.getDateFilterLabel = function () {
        var filterDateLabel = _this.getFilter('dateFilterLabel');
        return filterDateLabel._value;
    }

    _this.customDateRangeSearch = function(filterKey, startDate,endDate) {
        //alert(new Date(endDate).getDay());
        //alert(filterKey + startDate + endDate );

    }

    //** This builds date parameters for the search query **//
    _this.dateRangeSearch = function (filterKey, dateFilter) {
       
        var filter = _this.getFilter(filterKey);
        var filterDateLabel = _this.getFilter('dateFilterLabel');
        console.log("date range: ", dateFilter);

        // if (dateFilter == _this.currentDateRange) {
            // _this.clearDateRange(filterKey);
            // _this.currentDateRange = "";
            // filterDateLabel.setValue("");
            // filter.setValue("");
        // } else {
        var startDate = datesObject[dateFilter];
        var endDate = datesObject['day'];
        _this.currentDateRange = dateFilter;

        filterDateLabel.setValue(dateFilter);
        filter.setValue(startDate + ";" + endDate);
        // }
        //_this.CustomStartDate.val('');
       // _this.CustomEndDate.val('');
       // _this.checkSelectedDateRange();
        _this.update();
    }

    



    init();

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaFacetController", ['$scope', '$location', '$http', 'searchService', 'searchBootstrapper', InformaFacetController]);
