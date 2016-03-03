
var InformaFacetController = function ($scope, $location, $http, searchService, searchBootstrapper) {
    "use strict";

    var _this = this;

    var init = function () {
        _this.currentDateRange = _this.getDateFilterLabel();
        _this.ValidDates = true;

        _this.DateFilters = [
             { label: 'Last 24 hours', key: 'day' },
             { label: 'Last 3 days', key: 'threedays' },
             { label: 'Last week', key: 'week' },
             { label: 'Last month', key: 'month' },
             { label: 'Last year', key: 'year' }
        ];

        _this.MaxFacetShow = 5;


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

        _this.facetGroups = searchService.getFacetGroups();
        _this.searchService = searchService;
        _this.location = $location;
        _this.searchBootstrapper = searchBootstrapper;


        $scope.$watch(function () {
            return searchService.getPager();
        }, function () {
            _this.facetGroups = searchService.getFacetGroups();

        }, true);

        _this.update = function () {
            _this.searchService.getFilter('page').setValue('1');
            var routeBuilder = this.searchService.getRouteBuilder();
            _this.location.search(routeBuilder.getRoute());
            _this.searchService.query();
            _this.scrollTop();
        }

        _this.facetChange = function (facet) {
            _this.searchService.getFacet(facet.id).selected = facet.selected;
            _this.update();
        }

        // _this.scrollTop = function () {
        //     //var location = jq(".search-facets__header").offset().top;
        //     //window.scrollTo(0, location - 80);
        // }

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

        _this.clearAllFacets = function () {
            // _this.clearPublicationDate();
            var facetClear = this;
            var facetGroups = facetClear.facetGroups;
            _.each(facetGroups, function (group) {
                //_this.clearGroup(group.id)
                var facets = _this.searchService.getFacetGroup(group.id).getSelectedFacets();
                _.each(facets, function (facet) {
                    facet.selected = false;
                });
            });
            this.update();
            _this.currentDateRange = "";
            _this.checkSelectedDateRange();
        };

        _this.getFilter = function (filterKey) {
            var filter = _this.searchService.getFilter(filterKey);
            if (!filter) {
                _this.searchBootstrapper.createFilter(filterKey, "");
                filter = _this.searchService.getFilter(filterKey);
            }
            return filter;
        }

        _this.clearFilter = function (filterKey) {
            var filter = _this.getFilter(filterKey);
            filter.setValue("");
        }

        _this.getDateFilterLabel = function () {
            var filterDateLabel = _this.getFilter('dateFilterLabel');
            return filterDateLabel._value;
        }
        
        _this.dateRangeSearch = function (filterKey, dateFilter) {
            var filter = _this.getFilter(filterKey);
            var filterDateLabel = _this.getFilter('dateFilterLabel');

            if (dateFilter == _this.currentDateRange) {

                _this.currentDateRange = "";
                filterDateLabel.setValue("");
                filter.setValue("");
            } else {
                var startDate = datesObject[dateFilter];
                var endDate = datesObject['day'];
                _this.currentDateRange = dateFilter;

                filterDateLabel.setValue(dateFilter);
                filter.setValue(startDate + ";" + endDate);
            }
            //_this.CustomStartDate.val('');
           // _this.CustomEndDate.val('');
           // _this.checkSelectedDateRange();
            _this.update();
        }

        // _this.addCompany = function () {
            
        // }




    init();

};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaFacetController", ['$scope', '$location', '$http', 'searchService', 'searchBootstrapper', InformaFacetController]);