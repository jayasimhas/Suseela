(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
// * * *
//  ANALYTICS CONTROLLER
//  For ease-of-use, better DRY, better prevention of JS errors when ads are blocked
// * * *

'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
function analyticsEvent(dataObj) {
    if (typeof utag !== 'undefined') {
        utag.link(dataObj);
    }
};

exports.analyticsEvent = analyticsEvent;

},{}],2:[function(require,module,exports){
/* global angular */

// set up controller and pass data source
// note: a controller is usually destroyed & recreated when the route changes
'use strict';

var InformaTypeaheadController = function InformaTypeaheadController($scope, getCompaniesService) {

    $scope.$watch('pageId', function () {
        getCompaniesService.fetchCompanies($scope.pageId).then(function (response) {

            var companies = [];
            companies = $.map(response.data, function (value, index) {
                return value.companyName;
            });

            $scope.companies = companies;
        })['catch'](function (reason) {
            console.log("error");
            console.log(reason);
        });
    });
};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("InformaTypeaheadController", ['$scope', 'getCompaniesService', InformaTypeaheadController]);

},{}],3:[function(require,module,exports){
/* global angular */

"use strict";

var EmailRssController = function EmailRssController($scope, $location) {
    "use strict";

    $scope.$watch(function () {
        return $location.search();
    }, function (params) {
        var params = $location.search();

        var nUrl = "";
        for (var idxkey in params) {
            if (idxkey == "sortBy") {
                nUrl = nUrl + "&" + "sortBy=date";
            }

            if (idxkey == "sortOrder") {
                nUrl = nUrl + "&" + "sortOrder=desc";
            }

            if (idxkey != "sortBy" && idxkey != "sortOrder") {
                nUrl = nUrl + "&" + idxkey + "=" + params[idxkey];
            }
        }
        // strip leading &
        if (nUrl.startsWith('&')) {
            nUrl = nUrl.substr(1);
        }
        $scope.currentLocation = "?" + nUrl;
    });

    $scope.testvar = 'hello';
};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("EmailRssController", ['$scope', '$location', EmailRssController]);

},{}],4:[function(require,module,exports){
/* global analytics_data */
'use strict';

var _controllersAnalyticsController = require('../../controllers/analytics-controller');

var HeadlineSearchController = function HeadlineSearchController($scope, searchService, searchBootstrapper) {
    "use strict";

    var vm = this;

    vm.searchService = searchService;
    vm.searchBootstrapper = searchBootstrapper;

    vm.update = function () {
        var filter = vm.searchService.getFilter('headlinesOnly');

        if (!filter) {
            vm.searchBootstrapper.createFilter('headlinesOnly', '1');
        } else {
            if (filter._value === '1') {

                filter.setValue('');
            } else {

                filter.setValue('1');
                (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
                    event_name: 'search_utility',
                    search_utility: 'search_headlines_only'
                }));
            }
        }
    };

    vm.init = function () {
        var filter = vm.searchService.getFilter('headlinesOnly');
        return filter !== undefined;
    };
};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("HeadlineSearchController", ['$scope', 'searchService', 'searchBootstrapper', HeadlineSearchController]);

},{"../../controllers/analytics-controller":1}],5:[function(require,module,exports){
"use strict";

var HeadlineToggleController = function HeadlineToggleController($scope, viewHeadlinesStateService) {
    "use strict";

    $scope.headlinesOnly = viewHeadlinesStateService;
};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("HeadlineToggleController", ['$scope', 'viewHeadlinesStateService', HeadlineToggleController]);

},{}],6:[function(require,module,exports){
'use strict';

var informaDatesController = function informaDatesController($scope, $location) {
  'use strict';

  /*
    $scope.dateValues and $scope.datepickers are in InformaFacetsController.js
    Using ng-if creates a new scope for the data in this controller, which
    causes the date values to reset on change. By storing in the parent,
    those values survive any scope resets here.
  */

  // grab today and inject into field
  $scope.today = function () {
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
    return date.getTime() > tomorrow.getTime() || date.getTime() > toTimestamp;
  };

  // Prevent user from selecting date if date is in the future, OR if date
  // is before selected 'from' date. 'to' must be later than 'from'
  function disabledTo(data) {
    var date = data.date;
    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);

    // If no start-time exists, use a very large number so all timestamps are valid
    var fromTimestamp = $scope.dateValues.dtFrom ? $scope.dateValues.dtFrom.getTime() : 0;
    return date.getTime() > tomorrow.getTime() || date.getTime() < fromTimestamp;
  };

  // open min-cal
  $scope.open = function ($event, which) {
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

},{}],7:[function(require,module,exports){
/* global _, datesObject, angular, analytics_data */
"use strict";

var _controllersAnalyticsController = require('../../controllers/analytics-controller');

var InformaFacetController = function InformaFacetController($scope, $rootScope, $location, $http, $anchorScroll, $timeout, searchService, searchBootstrapper, facetAvailabilityService) {
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
    vm.showingOnlySubscriptions = false;

    vm.companies = { "companies": "", "isCompanySelected": false };
    $rootScope.$watch('facetAvailability', function () {
        vm.areFacetsDisabled = facetAvailabilityService.facetsAreEnabled();
    });

    // Date Facet stuff
    vm.DateFilters = [{ label: 'Last 24 hours', key: 'day', selected: false }, { label: 'Last 3 days', key: 'threedays', selected: false }, { label: 'Last week', key: 'week', selected: false }, { label: 'Last month', key: 'month', selected: false }, { label: 'Last year', key: 'year', selected: false }, { label: 'Select date range', key: 'custom', selected: false }];

    vm.originalGroup = [];

    /* Real talk: the Javascript Date() method is a trash fire. */
    var dToday = function dToday() {
        return new Date().clearTime();
    };

    var jsDates = {
        minus1Year: function minus1Year() {
            var jsDateToday = new Date();
            return new Date(jsDateToday.setFullYear(jsDateToday.getFullYear() - 1));
        },
        minus1Month: function minus1Month() {
            var jsDateToday = new Date();
            var m = jsDateToday.getMonth();
            jsDateToday.setMonth(jsDateToday.getMonth() - 1);

            // If still in same month, set date to last day of previous month
            if (jsDateToday.getMonth() == m) {
                jsDateToday.setDate(0);
            }
            return new Date(jsDateToday.setHours(0, 0, 0));
        },
        minusXdays: function minusXdays(days) {
            var jsDateToday = new Date();
            return new Date(jsDateToday.setDate(jsDateToday.getDate() - days));
        }
    };

    var formatDateObject = function formatDateObject(d) {
        return d.getMonth() + 1 + '/' + d.getDate() + '/' + d.getFullYear();
    };

    vm.datesObject = {
        year: formatDateObject(jsDates.minus1Year()),
        day: formatDateObject(jsDates.minusXdays(1)),
        threedays: formatDateObject(jsDates.minusXdays(3)),
        month: formatDateObject(jsDates.minus1Month()),
        week: formatDateObject(jsDates.minusXdays(7))
    };

    vm.timesObject = {
        year: { id: "year", value: "1" },
        day: { id: "hour", value: "24" },
        threedays: { id: "day", value: "3" },
        month: { id: "month", value: "1" },
        week: { id: "week", value: "1" }
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
            if (vm.DateFilters[i].key === 'custom') {
                // ...convert the date data in the URL to `Date`s...
                // example: date=3/29/2015;4/5/2016
                var splitDates = $location.search().date.split(';');
                // ...and update the model so the UI shows the right data.
                $scope.dateValues.dtFrom = new Date(splitDates[0]);
                $scope.dateValues.dtTo = new Date(splitDates[1]);
            }
        }
    }

    $scope.$watch(function () {
        return searchService.getPager();
    }, function () {
        vm.facetGroups = searchService.getFacetGroups();
        vm.originalGroup = searchService.getFacetGroups();

        if (searchService.getNewSearch()) {

            vm.facetGroups = vm.originalGroup;
            vm.clearAllFacets();

            searchService._isNewSearch = false;
        }
    }, true);

    //** This collects the user's saved companies **//
    vm.savedCompanies = {};

    vm.saveCompany = function ($item, model, label) {
        vm.savedCompanies[$item] = {
            selected: true,
            label: $item
        };
    };

    $scope.removeCompany = function ($item, model, label) {
        delete $scope.savedCompanies[$item.label];
    };

    var facetsForAnalytics = false;
    //** This updates the router/url with the latest search parameters **//
    vm.update = function (facetGroupId) {

        if (facetGroupId) {

            var facetGroup;
            facetsForAnalytics = false;

            _.each(vm.facetGroups, function (group) {
                if (group.id === facetGroupId) {
                    facetGroup = group;
                }
            });

            _.each(facetGroup.getSelectedFacets(), function (facet) {
                if (facet) {
                    if (!facetsForAnalytics) {
                        facetsForAnalytics = facet.label;
                    } else {
                        facetsForAnalytics += '|' + facet.label;
                    }
                }
            });

            var event_data = {
                event_name: 'search_facets',
                search_facet_category: facetGroup.label
            };

            if (facetsForAnalytics) {
                event_data.search_facet = facetGroup.label + ": " + facetsForAnalytics;
            }

            (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, event_data));
        }

        // Disable all facet options while updating search results
        facetAvailabilityService.disableFacets();

        var params = this.searchService.getRouteBuilder().getRoute().split('&');
        if (this.searchService.getRouteBuilder().getRoute().indexOf("companies") >= 0) {
            for (var idx_param in params) {
                if (params[idx_param].indexOf("companies") >= 0) {
                    var compValue = params[idx_param].split('=')[1];
                    vm.companies["isCompanySelected"] = true;
                    vm.companies["companies"] = compValue;
                }
            }
        } else {
            vm.companies["isCompanySelected"] = false;
            vm.companies["companies"] = "";
        }

        vm.searchService.getFilter('page').setValue('1');
        var routeBuilder = this.searchService.getRouteBuilder();
        vm.location.search(routeBuilder.getRoute());
        vm.searchService.query();
        //Scroll to the top of the results when a new page is chosen
        vm.location.hash("searchTop");
        vm.anchorScroll();
    };

    vm.updateTime = function (filter) {
        vm.searchService.getFilter('page').setValue('1');
        var routeBuilder = this.searchService.getRouteBuilder();

        var hash = {};
        var urlQuery = "&";
        var h = decodeURIComponent(routeBuilder.getRoute()).split("&");
        for (var idx in h) {
            if (h[idx] != "") {
                var currentParameter = h[idx].split("=");

                if (currentParameter[0] == "dateFilterLabel") {
                    hash[currentParameter[0]] = vm.timesObject[filter].id;
                    urlQuery = urlQuery + "&" + currentParameter[0] + "=" + vm.timesObject[filter].id;
                } else {
                    if (currentParameter[0] != "date") {
                        hash[currentParameter[0]] = currentParameter[1];
                        urlQuery = urlQuery + "&" + currentParameter[0] + "=" + currentParameter[1];
                    }
                }
            }
        }
        hash["time"] = vm.timesObject[filter].value;
        urlQuery = urlQuery + "&time=" + vm.timesObject[filter].value;

        if (vm.companies["isCompanySelected"]) {
            hash["companies"] = vm.companies["companies"];
        }

        if ("companies" in hash) {
            vm.isCompanySelected = true;
            hash["companies"] = hash["companies"].replace(/%20/g, " ").replace(/%3B/g, ';');
            vm.companies["companies"] = hash["companies"].replace(/%20/g, " ").replace(/%3B/g, ';');
        }

        vm.location.search(urlQuery);

        vm.searchService.queryTimePeriod(hash);
        //Scroll to the top of the results when a new page is chosen
        vm.location.hash("searchTop");
        vm.anchorScroll();
    };

    vm.facetChange = function (facet) {

        vm.searchService.getFacetGroup(facet.parentId).getFacet(facet.id).selected = facet.selected;
        vm.update(facet.parentId);
    };

    // facetGroupId: 'publication'
    // facetIds: ['In Vivo', 'Rose Sheet']
    vm.facetChangeMultiple = function (facetGroupId, facetIds) {

        var facets;

        _.each(vm.facetGroups, function (group) {
            if (group.id === facetGroupId) {
                facets = group;
            }
        });

        _.each(facets.getSelectedFacets(), function (facet) {
            if (facet) {
                facet.selected = false;
            }
        });

        _.each(facetIds, function (id) {
            var facet = facets.getFacet(id);
            if (facet) {
                facet.selected = true;
            }
        });

        vm.update(facetGroupId);
    };

    // TODO: this comes from a diff search app, and needs jquery to work.
    //       either hook up jq to this controller or move this elsewhere
    vm.scrollTop = function () {
        // var location = jq(".search-facets__header").offset().top;
        //window.scrollTo(0, location - 80);
    };

    vm.hasSelected = function (values) {
        return _.find(values, { selected: true }) ? true : false;
    };

    vm.getFilter = function (filterKey) {
        var filter = vm.searchService.getFilter(filterKey);
        if (!filter) {
            vm.searchBootstrapper.createFilter(filterKey, "");
            filter = vm.searchService.getFilter(filterKey);
        }
        return filter;
    };

    /* This deselects any selected facet checkboxes, clears all facet parameters
        from the search query, and runs the clearDateRange function */
    vm.clearAllFacets = function () {

        _.each(vm.facetGroups, function (group) {
            // vm.clearGroup(group.id)
            var facets = group.facets;
            _.each(facets, function (facet) {
                facet.selected = false;
            });
        });
        vm.clearDateRange();
        vm.update();
    };

    vm.clearFilter = function (filterKey) {
        var filter = vm.getFilter(filterKey);
        filter.setValue("");
    };

    /* This clears the date parameters from the search, deselcts any date radio
    buttons, and clears both custom date input fields **/
    vm.clearDateRange = function () {
        var filter = vm.getFilter('date');
        filter.setValue("");
        filter.selected = false;
        var filterDateLabel = vm.getFilter('dateFilterLabel');
        filterDateLabel.setValue("");
        var dates = vm.DateFilters;
        _.each(dates, function (date) {
            date.selected = false;
        });
    };

    vm.getDateFilterLabel = function () {
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

    vm.customDateRangeSearch = function (filterKey, startDate, endDate) {

        var filter = vm.getFilter(filterKey);
        var filterDateLabel = vm.getFilter('dateFilterLabel');
        filterDateLabel.setValue('custom');

        if (startDate > new Date()) {
            alert("you can't select date bigger than today");
            $scope.dateValues.dtFrom = "";
        }

        if (endDate > new Date()) {
            alert("you can't select date bigger than today");
            $scope.dateValues.dtTo = "";
        }

        if (startDate > 0 && endDate > 0 && startDate < endDate) {
            var date1Unparsed = new Date(startDate);
            var date1 = date1Unparsed.getMonth() + 1 + '/' + date1Unparsed.getDate() + '/' + date1Unparsed.getFullYear();

            var date2Unparsed = new Date(endDate);
            var date2 = date2Unparsed.getMonth() + 1 + '/' + date2Unparsed.getDate() + '/' + date2Unparsed.getFullYear();

            filter.setValue(date1 + ";" + date2);

            vm.update();
        } else {
            if (startDate != "" && startDate != undefined && endDate != "" && endDate != undefined) {
                if (startDate > endDate) {
                    alert("You cant put 'from' date bigger than 'to' date");
                    $scope.dateValues.dtFrom = "";
                    $scope.dateValues.dtTo = "";
                }
            }
        }
    };

    $scope.options = {
        showWeeks: false
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
        vm.updateTime(dateFilter);
        //vm.update();
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
informaSearchApp.controller("InformaFacetController", ['$scope', '$rootScope', '$location', '$http', '$anchorScroll', '$timeout', 'searchService', 'searchBootstrapper', 'facetAvailabilityService', InformaFacetController]);

},{"../../controllers/analytics-controller":1}],8:[function(require,module,exports){
/* global angular, analytics_data, utag */

'use strict';

var informaSearchApp = angular.module('informaSearchApp');

var InformaResultsController = function InformaResultsController($scope, $sanitize, searchService, viewHeadlinesStateService, $timeout, $window, facetAvailabilityService) {

    var vm = this;

    vm.service = searchService;
    vm.docs = [];

    $scope.headlinesOnly = viewHeadlinesStateService;
    var count = 0;

    $scope.utagAnalytics = function () {
        if (count > 0) {
            var eventDetails = {
                Number_of_Results: '"' + $(".js-searchTotalResults").text() + '"',
                search_Keyword: '"' + $(".js-searchKeyword").text() + '"'
            };
            var dataObj = $.extend(analytics_data, eventDetails);
            if (typeof utag !== 'undefined') {
                utag.link(dataObj);
            }
        }
        count = count + 1;
    };

    $scope.$watchCollection(function () {
        return searchService.getResults();
    }, function () {
        vm.docs = searchService.getResults();
        $scope.utagAnalytics();
    });

    $scope.filterResult = function (url) {
        window.location = url;
        window.location.reload();
    };

    $scope.fireBookmark = function (article, event, key) {
        $timeout(function () {
            vm.docs[key].isArticleBookmarked = vm.docs[key].isArticleBookmarked ? false : true;
        }, 500);
    };

    $scope.$on('refreshPopOuts', function (ngRepeatFinishedEvent) {

        // Enable all facet options when search results land
        // $('.facets__section input').attr('disabled', null);
        facetAvailabilityService.enableFacets();

        window.indexPopOuts();
        window.indexBookmarks();
        window.autoBookmark();
        window.findTooltips();
    });

    this.forceRefresh = function () {
        $window.location.reload(false);
    };
};

informaSearchApp.directive('onFinishRender', function ($timeout) {
    return {
        restrict: 'A',
        link: function link(scope, element, attr) {
            if (scope.$last === true) {
                $timeout(function () {
                    scope.$emit('refreshPopOuts');
                });
            }
        }
    };
});

informaSearchApp.controller("InformaResultsController", ['$scope', '$sanitize', 'searchService', 'viewHeadlinesStateService', '$timeout', '$window', 'facetAvailabilityService', InformaResultsController]);

},{}],9:[function(require,module,exports){
/* global angular, analytics_data */
'use strict';

var _controllersAnalyticsController = require('../../controllers/analytics-controller');

var PageSizeController = function PageSizeController($scope, $location, $anchorScroll, searchService, searchBootstrapper) {
    "use strict";

    var perPageKey = 'perPage';

    var vm = this;

    vm.location = $location;
    vm.anchorScroll = $anchorScroll;
    vm.searchService = searchService;
    vm.searchBootstrapper = searchBootstrapper;

    vm.update = function (pageSize) {
        var filter = vm.searchService.getFilter(perPageKey);

        if (!filter) {
            vm.searchBootstrapper.createFilter(perPageKey, pageSize);
        } else {
            filter.setValue(pageSize);
        }

        vm.searchService.getFilter('page').setValue('1');
        var routeBuilder = this.searchService.getRouteBuilder();
        vm.location.search(routeBuilder.getRoute());
        vm.searchService.query();

        //Scroll to the top of the results when a new page is chosen
        vm.location.hash("searchTop");
        vm.anchorScroll();

        (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
            event_name: 'search_utility',
            search_utility: 'results_per_page_' + pageSize
        }));
    };

    vm.init = function () {
        var filter = vm.searchService.getFilter(perPageKey);

        $scope.pageSize = filter ? filter._value : '10';
    };
};
var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("PageSizeController", ['$scope', '$location', '$anchorScroll', 'searchService', 'searchBootstrapper', PageSizeController]);

},{"../../controllers/analytics-controller":1}],10:[function(require,module,exports){
/* global angular */

"use strict";

var SavedSearchController = function SavedSearchController($scope, $location, $timeout, $http, searchService, savedSearchService) {
    "use strict";

    var vm = this;

    vm.searchService = searchService;
    $scope.searchIsSaved = false;

    $scope.oneClickSaveFocus = false;

    // A user can land on the search page from a "1-click Subscribe" link in an email
    // This handles detection of that query parameter, triggers appropriate UI changes
    function openSaveSearchIf1Click() {

        var urlQuery = $location.search();
        var clickKey = Object.keys(urlQuery).filter(function (cur) {
            return cur.toLowerCase() === "1click";
        });

        if (clickKey.length > 0 && urlQuery[clickKey[0]] === "1") {
            jQuery(".js-save-search").click();
            $location.search(clickKey[0], null);
            $scope.oneClickSaveFocus = true;
        }
    }

    $timeout(function () {
        openSaveSearchIf1Click();
    }, 1000);

    $scope.$watch(function () {
        return searchService.getPager();
    }, function () {
        $scope.title = searchService.getFilter('q').getValue();
        $scope.currentLocation = $location.url();
        if ($scope.isAuthenticated) {
            savedSearchService.isSaved().then(function (response) {
                $scope.searchIsSaved = response.data;
            });
        }
    }, true);

    // Depending on how quickly Angular bootstraps, some lightbox trigger classes
    // might not be added before the lightbox event listners are bound. This
    // manually fires the Saved Search lightbox, in case the normal listener wasn't
    // bound in time.
    vm.showLightbox = function (e) {
        if ($scope.searchIsSaved) {
            window.lightboxController.showLightbox($(e.target).closest('.angular-lightbox-modal-trigger'));
        } else {
            $(e.target).closest('.angular-pop-out-trigger').addClass('js-pop-out-trigger');
            window.controlPopOuts.togglePopOut($(e.target).closest('.angular-pop-out-trigger'));
        }
    };

    vm.searchIsSaved = function () {
        $scope.searchIsSaved = true;
        $scope.$apply();
    };
};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SavedSearchController", ['$scope', '$location', '$timeout', '$http', 'searchService', 'savedSearchService', SavedSearchController]);

},{}],11:[function(require,module,exports){
/* global angular, analytics_data */
'use strict';

var _controllersAnalyticsController = require('../../controllers/analytics-controller');

var SortByDateController = function SortByDateController($scope, $location, $timeout, $http, searchService, savedSearchService) {
	"use strict";

	var vm = this;

	vm.resultsSorted = function (sortingAsc) {
		if (sortingAsc) {
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
				event_name: 'search_utility',
				search_utility: 'sort_by_date:asc'
			}));
		} else {
			(0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
				event_name: 'search_utility',
				search_utility: 'sort_by_date:desc'
			}));
		}
	};
};

var informaSearchApp = angular.module('informaSearchApp');
informaSearchApp.controller("SortByDateController", ['$scope', '$location', '$timeout', '$http', 'searchService', 'savedSearchService', SortByDateController]);

},{"../../controllers/analytics-controller":1}],12:[function(require,module,exports){
'use strict';

var informaSearchApp = angular.module('informaSearchApp');

// factory to handle call to companies service
// note: a factory lives through the entire application lifecycle
informaSearchApp.factory('getCompaniesService', ['$http', '$location', function ($http, $location) {

    var fetchCompanies = function fetchCompanies(pageId) {

        var fullUrl = '/api/typeahead/getcompanies?pId=' + pageId;

        if ($location.url()) {
            fullUrl += '&' + $location.url().replace('?', '');
        }

        return $http({
            method: 'GET',
            url: fullUrl
        });
    };

    return { fetchCompanies: fetchCompanies };
}]);

},{}],13:[function(require,module,exports){
'use strict';

var informaSearchApp = angular.module('informaSearchApp');

// factory to handle call to companies service
// note: a factory lives through the entire application lifecycle
informaSearchApp.factory('savedSearchService', ['$http', '$location', function ($http, $location) {

    var isSaved = function isSaved() {
        var fullUrl = '/api/SavedSearches?url=' + escape($location.url());

        return $http({
            method: 'GET',
            url: fullUrl
        });
    };

    return {
        isSaved: isSaved
    };
}]);

},{}],14:[function(require,module,exports){
/* global angular, analytics_data */
'use strict';

var _controllersAnalyticsController = require('../controllers/analytics-controller');

(function () {
    'use strict';

    var informaSearchApp = angular.module('informaSearchApp', ['velir.search', 'ui.bootstrap', 'ngSanitize', 'ngAnimate']).constant('apiEndpoints', {
        API_BASE: '/api',
        SEARCH_ENDPOINT: '/search'
    }).config(['$logProvider', function ($logProvider) {
        // All debugging should be done via $log instead of directly to console
        // This flag disables $log.debug() output
        //$logProvider.debugEnabled(false);
    }]).config(['$compileProvider', function ($compileProvider) {

        // https://docs.angularjs.org/api/ng/provider/$compileProvider#debugInfoEnabled
        // UNCOMMENT THE LINE BELOW IN PRODUCTION FOR PERFORMANCE GAINS

        // $compileProvider.debugInfoEnabled(false);

    }]);

    informaSearchApp.factory('viewHeadlinesStateService', function () {
        var headlines = false;

        return {
            showOnlyHeadlines: function showOnlyHeadlines() {
                return headlines;
            },
            updateValue: function updateValue() {

                if (!headlines) {
                    (0, _controllersAnalyticsController.analyticsEvent)($.extend(analytics_data, {
                        event_name: 'search_utility',
                        search_utility: 'view_headlines_only'
                    }));
                }

                headlines = !headlines;
            }
        };
    });

    informaSearchApp.factory('facetAvailabilityService', function ($rootScope) {

        var facetsState = false;

        return {
            facetsAreEnabled: function facetsAreEnabled() {
                return facetsState;
            },
            enableFacets: function enableFacets() {
                facetsState = false;
                $rootScope.facetAvailability = facetsState;
            },
            disableFacets: function disableFacets() {
                facetsState = true;
                $rootScope.facetAvailability = facetsState;
            },
            toggleFacets: function toggleFacets() {
                facetsState = !facetsState;
                $rootScope.facetAvailability = facetsState;
            }
        };
    });
})();

},{"../controllers/analytics-controller":1}],15:[function(require,module,exports){
'use strict';

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var _informaSearchAppJs = require('./informaSearchApp.js');

var _informaSearchAppJs2 = _interopRequireDefault(_informaSearchAppJs);

var _factoriesCompaniesFactoryJs = require('./factories/companiesFactory.js');

var _factoriesCompaniesFactoryJs2 = _interopRequireDefault(_factoriesCompaniesFactoryJs);

var _factoriesSavedSearchFactoryJs = require('./factories/savedSearchFactory.js');

var _factoriesSavedSearchFactoryJs2 = _interopRequireDefault(_factoriesSavedSearchFactoryJs);

var _controllersHeadlineToggleControllerJs = require('./controllers/headlineToggleController.js');

var _controllersHeadlineToggleControllerJs2 = _interopRequireDefault(_controllersHeadlineToggleControllerJs);

var _controllersInformaFacetsControllerJs = require('./controllers/informaFacetsController.js');

var _controllersInformaFacetsControllerJs2 = _interopRequireDefault(_controllersInformaFacetsControllerJs);

var _controllersInformaDatesControllerJs = require('./controllers/informaDatesController.js');

var _controllersInformaDatesControllerJs2 = _interopRequireDefault(_controllersInformaDatesControllerJs);

var _controllersInformaResultsControllerJs = require('./controllers/informaResultsController.js');

var _controllersInformaResultsControllerJs2 = _interopRequireDefault(_controllersInformaResultsControllerJs);

var _controllersEmailRssControllerJs = require('./controllers/emailRssController.js');

var _controllersEmailRssControllerJs2 = _interopRequireDefault(_controllersEmailRssControllerJs);

var _controllersTypeaheadControllerJs = require('./controllers/TypeaheadController.js');

var _controllersTypeaheadControllerJs2 = _interopRequireDefault(_controllersTypeaheadControllerJs);

var _controllersHeadlineSearchControllerJs = require('./controllers/headlineSearchController.js');

var _controllersHeadlineSearchControllerJs2 = _interopRequireDefault(_controllersHeadlineSearchControllerJs);

var _controllersPageSizeControllerJs = require('./controllers/pageSizeController.js');

var _controllersPageSizeControllerJs2 = _interopRequireDefault(_controllersPageSizeControllerJs);

var _controllersSavedSearchControllerJs = require('./controllers/savedSearchController.js');

var _controllersSavedSearchControllerJs2 = _interopRequireDefault(_controllersSavedSearchControllerJs);

var _controllersSortByDateControllerJs = require('./controllers/sortByDateController.js');

var _controllersSortByDateControllerJs2 = _interopRequireDefault(_controllersSortByDateControllerJs);

var _uiBootstrapCustomTpls124MinJs = require('./ui-bootstrap-custom-tpls-1.2.4.min.js');

var _uiBootstrapCustomTpls124MinJs2 = _interopRequireDefault(_uiBootstrapCustomTpls124MinJs);

},{"./controllers/TypeaheadController.js":2,"./controllers/emailRssController.js":3,"./controllers/headlineSearchController.js":4,"./controllers/headlineToggleController.js":5,"./controllers/informaDatesController.js":6,"./controllers/informaFacetsController.js":7,"./controllers/informaResultsController.js":8,"./controllers/pageSizeController.js":9,"./controllers/savedSearchController.js":10,"./controllers/sortByDateController.js":11,"./factories/companiesFactory.js":12,"./factories/savedSearchFactory.js":13,"./informaSearchApp.js":14,"./ui-bootstrap-custom-tpls-1.2.4.min.js":16}],16:[function(require,module,exports){
/*
 * angular-ui-bootstrap
 * http://angular-ui.github.io/bootstrap/

 * Version: 1.2.4 - 2016-03-06
 * License: MIT
 */"use strict";

angular.module("ui.bootstrap", ["ui.bootstrap.tpls", "ui.bootstrap.datepicker", "ui.bootstrap.dateparser", "ui.bootstrap.isClass", "ui.bootstrap.position", "ui.bootstrap.typeahead", "ui.bootstrap.debounce"]), angular.module("ui.bootstrap.tpls", ["uib/template/datepicker/datepicker.html", "uib/template/datepicker/day.html", "uib/template/datepicker/month.html", "uib/template/datepicker/popup.html", "uib/template/datepicker/year.html", "uib/template/typeahead/typeahead-match.html", "uib/template/typeahead/typeahead-popup.html"]), angular.module("ui.bootstrap.datepicker", ["ui.bootstrap.dateparser", "ui.bootstrap.isClass", "ui.bootstrap.position"]).value("$datepickerSuppressError", !1).value("uibDatepickerAttributeWarning", !0).constant("uibDatepickerConfig", { datepickerMode: "day", formatDay: "dd", formatMonth: "MMMM", formatYear: "yyyy", formatDayHeader: "EEE", formatDayTitle: "MMMM yyyy", formatMonthTitle: "yyyy", maxDate: null, maxMode: "year", minDate: null, minMode: "day", ngModelOptions: {}, shortcutPropagation: !1, showWeeks: !0, yearColumns: 5, yearRows: 4 }).controller("UibDatepickerController", ["$scope", "$attrs", "$parse", "$interpolate", "$locale", "$log", "dateFilter", "uibDatepickerConfig", "$datepickerSuppressError", "uibDatepickerAttributeWarning", "uibDateParser", function (e, t, a, i, n, r, o, s, l, u, p) {
  function c(t) {
    e.datepickerMode = t, g && (e.datepickerOptions.datepickerMode = t);
  }var d = this,
      h = { $setViewValue: angular.noop },
      f = {},
      m = [],
      g = !!t.datepickerOptions;if ((this.modes = ["day", "month", "year"], g)) ["customClass", "dateDisabled", "datepickerMode", "formatDay", "formatDayHeader", "formatDayTitle", "formatMonth", "formatMonthTitle", "formatYear", "initDate", "maxDate", "maxMode", "minDate", "minMode", "showWeeks", "shortcutPropagation", "startingDay", "yearColumns", "yearRows"].forEach(function (t) {
    switch (t) {case "customClass":case "dateDisabled":
        e[t] = e.datepickerOptions[t] || angular.noop;break;case "datepickerMode":
        e.datepickerMode = angular.isDefined(e.datepickerOptions.datepickerMode) ? e.datepickerOptions.datepickerMode : s.datepickerMode;break;case "formatDay":case "formatDayHeader":case "formatDayTitle":case "formatMonth":case "formatMonthTitle":case "formatYear":
        d[t] = angular.isDefined(e.datepickerOptions[t]) ? i(e.datepickerOptions[t])(e.$parent) : s[t];break;case "showWeeks":case "shortcutPropagation":case "yearColumns":case "yearRows":
        d[t] = angular.isDefined(e.datepickerOptions[t]) ? e.datepickerOptions[t] : s[t];break;case "startingDay":
        d.startingDay = angular.isDefined(e.datepickerOptions.startingDay) ? e.datepickerOptions.startingDay : angular.isNumber(s.startingDay) ? s.startingDay : (n.DATETIME_FORMATS.FIRSTDAYOFWEEK + 8) % 7;break;case "maxDate":case "minDate":
        e.datepickerOptions[t] ? e.$watch(function () {
          return e.datepickerOptions[t];
        }, function (e) {
          d[t] = e ? angular.isDate(e) ? p.fromTimezone(new Date(e), f.timezone) : new Date(o(e, "medium")) : null, d.refreshView();
        }) : d[t] = s[t] ? p.fromTimezone(new Date(s[t]), f.timezone) : null;break;case "maxMode":case "minMode":
        e.datepickerOptions[t] ? e.$watch(function () {
          return e.datepickerOptions[t];
        }, function (a) {
          d[t] = e[t] = angular.isDefined(a) ? a : datepickerOptions[t], ("minMode" === t && d.modes.indexOf(e.datepickerOptions.datepickerMode) < d.modes.indexOf(d[t]) || "maxMode" === t && d.modes.indexOf(e.datepickerOptions.datepickerMode) > d.modes.indexOf(d[t])) && (e.datepickerMode = d[t], e.datepickerOptions.datepickerMode = d[t]);
        }) : d[t] = e[t] = s[t] || null;break;case "initDate":
        e.datepickerOptions.initDate ? (d.activeDate = p.fromTimezone(e.datepickerOptions.initDate, f.timezone) || new Date(), e.$watch(function () {
          return e.datepickerOptions.initDate;
        }, function (e) {
          e && (h.$isEmpty(h.$modelValue) || h.$invalid) && (d.activeDate = p.fromTimezone(e, f.timezone), d.refreshView());
        })) : d.activeDate = new Date();}
  });else {
    if ((angular.forEach(["formatDay", "formatMonth", "formatYear", "formatDayHeader", "formatDayTitle", "formatMonthTitle"], function (a) {
      d[a] = angular.isDefined(t[a]) ? i(t[a])(e.$parent) : s[a], angular.isDefined(t[a]) && u && r.warn("uib-datepicker " + a + " attribute usage is deprecated, use datepicker-options attribute instead");
    }), angular.forEach(["showWeeks", "yearRows", "yearColumns", "shortcutPropagation"], function (a) {
      d[a] = angular.isDefined(t[a]) ? e.$parent.$eval(t[a]) : s[a], angular.isDefined(t[a]) && u && r.warn("uib-datepicker " + a + " attribute usage is deprecated, use datepicker-options attribute instead");
    }), angular.forEach(["dateDisabled", "customClass"], function (e) {
      angular.isDefined(t[e]) && u && r.warn("uib-datepicker " + e + " attribute usage is deprecated, use datepicker-options attribute instead");
    }), angular.isDefined(t.startingDay) ? (u && r.warn("uib-datepicker startingDay attribute usage is deprecated, use datepicker-options attribute instead"), d.startingDay = e.$parent.$eval(t.startingDay)) : d.startingDay = angular.isNumber(s.startingDay) ? s.startingDay : (n.DATETIME_FORMATS.FIRSTDAYOFWEEK + 8) % 7, angular.forEach(["minDate", "maxDate"], function (a) {
      t[a] ? (u && r.warn("uib-datepicker " + a + " attribute usage is deprecated, use datepicker-options attribute instead"), m.push(e.$parent.$watch(t[a], function (e) {
        d[a] = e ? angular.isDate(e) ? p.fromTimezone(new Date(e), f.timezone) : new Date(o(e, "medium")) : null, d.refreshView();
      }))) : d[a] = s[a] ? p.fromTimezone(new Date(s[a]), f.timezone) : null;
    }), angular.forEach(["minMode", "maxMode"], function (a) {
      t[a] ? (u && r.warn("uib-datepicker " + a + " attribute usage is deprecated, use datepicker-options attribute instead"), m.push(e.$parent.$watch(t[a], function (i) {
        d[a] = e[a] = angular.isDefined(i) ? i : t[a], ("minMode" === a && d.modes.indexOf(e.datepickerMode) < d.modes.indexOf(d[a]) || "maxMode" === a && d.modes.indexOf(e.datepickerMode) > d.modes.indexOf(d[a])) && (e.datepickerMode = d[a]);
      }))) : d[a] = e[a] = s[a] || null;
    }), angular.isDefined(t.initDate))) {
      u && r.warn("uib-datepicker initDate attribute usage is deprecated, use datepicker-options attribute instead");var b = p.fromTimezone(e.$parent.$eval(t.initDate), f.timezone);this.activeDate = isNaN(b) ? new Date() : b, m.push(e.$parent.$watch(t.initDate, function (e) {
        e && (h.$isEmpty(h.$modelValue) || h.$invalid) && (e = p.fromTimezone(e, f.timezone), d.activeDate = isNaN(e) ? new Date() : e, d.refreshView());
      }));
    } else this.activeDate = new Date();t.datepickerMode && u && r.warn("uib-datepicker datepickerMode attribute usage is deprecated, use datepicker-options attribute instead"), e.datepickerMode = e.datepickerMode || s.datepickerMode;
  }e.uniqueId = "datepicker-" + e.$id + "-" + Math.floor(1e4 * Math.random()), e.disabled = angular.isDefined(t.disabled) || !1, angular.isDefined(t.ngDisabled) && m.push(e.$parent.$watch(t.ngDisabled, function (t) {
    e.disabled = t, d.refreshView();
  })), e.isActive = function (t) {
    return 0 === d.compare(t.date, d.activeDate) ? (e.activeDateId = t.uid, !0) : !1;
  }, this.init = function (e) {
    h = e, f = e.$options || s.ngModelOptions, this.activeDate = h.$modelValue || new Date(), h.$render = function () {
      d.render();
    };
  }, this.render = function () {
    if (h.$viewValue) {
      var e = new Date(h.$viewValue),
          t = !isNaN(e);t ? this.activeDate = p.fromTimezone(e, f.timezone) : l || r.error('Datepicker directive: "ng-model" value must be a Date object');
    }this.refreshView();
  }, this.refreshView = function () {
    if (this.element) {
      e.selectedDt = null, this._refreshView(), e.activeDt && (e.activeDateId = e.activeDt.uid);var t = h.$viewValue ? new Date(h.$viewValue) : null;t = p.fromTimezone(t, f.timezone), h.$setValidity("dateDisabled", !t || this.element && !this.isDisabled(t));
    }
  }, this.createDateObject = function (t, a) {
    var i = h.$viewValue ? new Date(h.$viewValue) : null;i = p.fromTimezone(i, f.timezone);var n = new Date();n = p.fromTimezone(n, f.timezone);var r = this.compare(t, n),
        o = { date: t, label: p.filter(t, a), selected: i && 0 === this.compare(t, i), disabled: this.isDisabled(t), past: 0 > r, current: 0 === r, future: r > 0, customClass: this.customClass(t) || null };return i && 0 === this.compare(t, i) && (e.selectedDt = o), d.activeDate && 0 === this.compare(o.date, d.activeDate) && (e.activeDt = o), o;
  }, this.isDisabled = function (t) {
    return e.disabled || this.minDate && this.compare(t, this.minDate) < 0 || this.maxDate && this.compare(t, this.maxDate) > 0 || e.dateDisabled && e.dateDisabled({ date: t, mode: e.datepickerMode });
  }, this.customClass = function (t) {
    return e.customClass({ date: t, mode: e.datepickerMode });
  }, this.split = function (e, t) {
    for (var a = []; e.length > 0;) a.push(e.splice(0, t));return a;
  }, e.select = function (t) {
    if (e.datepickerMode === d.minMode) {
      var a = h.$viewValue ? p.fromTimezone(new Date(h.$viewValue), f.timezone) : new Date(0, 0, 0, 0, 0, 0, 0);a.setFullYear(t.getFullYear(), t.getMonth(), t.getDate()), a = p.toTimezone(a, f.timezone), h.$setViewValue(a), h.$render();
    } else d.activeDate = t, c(d.modes[d.modes.indexOf(e.datepickerMode) - 1]), e.$emit("uib:datepicker.mode");
  }, e.move = function (e) {
    var t = d.activeDate.getFullYear() + e * (d.step.years || 0),
        a = d.activeDate.getMonth() + e * (d.step.months || 0);d.activeDate.setFullYear(t, a, 1), d.refreshView();
  }, e.toggleMode = function (t) {
    t = t || 1, e.datepickerMode === d.maxMode && 1 === t || e.datepickerMode === d.minMode && -1 === t || (c(d.modes[d.modes.indexOf(e.datepickerMode) + t]), e.$emit("uib:datepicker.mode"));
  }, e.keys = { 13: "enter", 32: "space", 33: "pageup", 34: "pagedown", 35: "end", 36: "home", 37: "left", 38: "up", 39: "right", 40: "down" };var y = function y() {
    d.element[0].focus();
  };e.$on("uib:datepicker.focus", y), e.keydown = function (t) {
    var a = e.keys[t.which];if (a && !t.shiftKey && !t.altKey && !e.disabled) if ((t.preventDefault(), d.shortcutPropagation || t.stopPropagation(), "enter" === a || "space" === a)) {
      if (d.isDisabled(d.activeDate)) return;e.select(d.activeDate);
    } else !t.ctrlKey || "up" !== a && "down" !== a ? (d.handleKeyDown(a, t), d.refreshView()) : e.toggleMode("up" === a ? 1 : -1);
  }, e.$on("$destroy", function () {
    for (; m.length;) m.shift()();
  });
}]).controller("UibDaypickerController", ["$scope", "$element", "dateFilter", function (e, t, a) {
  function i(e, t) {
    return 1 !== t || e % 4 !== 0 || e % 100 === 0 && e % 400 !== 0 ? r[t] : 29;
  }function n(e) {
    var t = new Date(e);t.setDate(t.getDate() + 4 - (t.getDay() || 7));var a = t.getTime();return t.setMonth(0), t.setDate(1), Math.floor(Math.round((a - t) / 864e5) / 7) + 1;
  }var r = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];this.step = { months: 1 }, this.element = t, this.init = function (t) {
    angular.extend(t, this), e.showWeeks = t.showWeeks, t.refreshView();
  }, this.getDates = function (e, t) {
    for (var a, i = new Array(t), n = new Date(e), r = 0; t > r;) a = new Date(n), i[r++] = a, n.setDate(n.getDate() + 1);return i;
  }, this._refreshView = function () {
    var t = this.activeDate.getFullYear(),
        i = this.activeDate.getMonth(),
        r = new Date(this.activeDate);r.setFullYear(t, i, 1);var o = this.startingDay - r.getDay(),
        s = o > 0 ? 7 - o : -o,
        l = new Date(r);s > 0 && l.setDate(-s + 1);for (var u = this.getDates(l, 42), p = 0; 42 > p; p++) u[p] = angular.extend(this.createDateObject(u[p], this.formatDay), { secondary: u[p].getMonth() !== i, uid: e.uniqueId + "-" + p });e.labels = new Array(7);for (var c = 0; 7 > c; c++) e.labels[c] = { abbr: a(u[c].date, this.formatDayHeader), full: a(u[c].date, "EEEE") };if ((e.title = a(this.activeDate, this.formatDayTitle), e.rows = this.split(u, 7), e.showWeeks)) {
      e.weekNumbers = [];for (var d = (11 - this.startingDay) % 7, h = e.rows.length, f = 0; h > f; f++) e.weekNumbers.push(n(e.rows[f][d].date));
    }
  }, this.compare = function (e, t) {
    var a = new Date(e.getFullYear(), e.getMonth(), e.getDate()),
        i = new Date(t.getFullYear(), t.getMonth(), t.getDate());return a.setFullYear(e.getFullYear()), i.setFullYear(t.getFullYear()), a - i;
  }, this.handleKeyDown = function (e) {
    var t = this.activeDate.getDate();if ("left" === e) t -= 1;else if ("up" === e) t -= 7;else if ("right" === e) t += 1;else if ("down" === e) t += 7;else if ("pageup" === e || "pagedown" === e) {
      var a = this.activeDate.getMonth() + ("pageup" === e ? -1 : 1);this.activeDate.setMonth(a, 1), t = Math.min(i(this.activeDate.getFullYear(), this.activeDate.getMonth()), t);
    } else "home" === e ? t = 1 : "end" === e && (t = i(this.activeDate.getFullYear(), this.activeDate.getMonth()));this.activeDate.setDate(t);
  };
}]).controller("UibMonthpickerController", ["$scope", "$element", "dateFilter", function (e, t, a) {
  this.step = { years: 1 }, this.element = t, this.init = function (e) {
    angular.extend(e, this), e.refreshView();
  }, this._refreshView = function () {
    for (var t, i = new Array(12), n = this.activeDate.getFullYear(), r = 0; 12 > r; r++) t = new Date(this.activeDate), t.setFullYear(n, r, 1), i[r] = angular.extend(this.createDateObject(t, this.formatMonth), { uid: e.uniqueId + "-" + r });e.title = a(this.activeDate, this.formatMonthTitle), e.rows = this.split(i, 3);
  }, this.compare = function (e, t) {
    var a = new Date(e.getFullYear(), e.getMonth()),
        i = new Date(t.getFullYear(), t.getMonth());return a.setFullYear(e.getFullYear()), i.setFullYear(t.getFullYear()), a - i;
  }, this.handleKeyDown = function (e) {
    var t = this.activeDate.getMonth();if ("left" === e) t -= 1;else if ("up" === e) t -= 3;else if ("right" === e) t += 1;else if ("down" === e) t += 3;else if ("pageup" === e || "pagedown" === e) {
      var a = this.activeDate.getFullYear() + ("pageup" === e ? -1 : 1);this.activeDate.setFullYear(a);
    } else "home" === e ? t = 0 : "end" === e && (t = 11);this.activeDate.setMonth(t);
  };
}]).controller("UibYearpickerController", ["$scope", "$element", "dateFilter", function (e, t) {
  function a(e) {
    return parseInt((e - 1) / n, 10) * n + 1;
  }var i, n;this.element = t, this.yearpickerInit = function () {
    i = this.yearColumns, n = this.yearRows * i, this.step = { years: n };
  }, this._refreshView = function () {
    for (var t, r = new Array(n), o = 0, s = a(this.activeDate.getFullYear()); n > o; o++) t = new Date(this.activeDate), t.setFullYear(s + o, 0, 1), r[o] = angular.extend(this.createDateObject(t, this.formatYear), { uid: e.uniqueId + "-" + o });e.title = [r[0].label, r[n - 1].label].join(" - "), e.rows = this.split(r, i), e.columns = i;
  }, this.compare = function (e, t) {
    return e.getFullYear() - t.getFullYear();
  }, this.handleKeyDown = function (e) {
    var t = this.activeDate.getFullYear();"left" === e ? t -= 1 : "up" === e ? t -= i : "right" === e ? t += 1 : "down" === e ? t += i : "pageup" === e || "pagedown" === e ? t += ("pageup" === e ? -1 : 1) * n : "home" === e ? t = a(this.activeDate.getFullYear()) : "end" === e && (t = a(this.activeDate.getFullYear()) + n - 1), this.activeDate.setFullYear(t);
  };
}]).directive("uibDatepicker", function () {
  return { replace: !0, templateUrl: function templateUrl(e, t) {
      return t.templateUrl || "uib/template/datepicker/datepicker.html";
    }, scope: { datepickerMode: "=?", datepickerOptions: "=?", dateDisabled: "&", customClass: "&", shortcutPropagation: "&?" }, require: ["uibDatepicker", "^ngModel"], controller: "UibDatepickerController", controllerAs: "datepicker", link: function link(e, t, a, i) {
      var n = i[0],
          r = i[1];n.init(r);
    } };
}).directive("uibDaypicker", function () {
  return { replace: !0, templateUrl: function templateUrl(e, t) {
      return t.templateUrl || "uib/template/datepicker/day.html";
    }, require: ["^uibDatepicker", "uibDaypicker"], controller: "UibDaypickerController", link: function link(e, t, a, i) {
      var n = i[0],
          r = i[1];r.init(n);
    } };
}).directive("uibMonthpicker", function () {
  return { replace: !0, templateUrl: function templateUrl(e, t) {
      return t.templateUrl || "uib/template/datepicker/month.html";
    }, require: ["^uibDatepicker", "uibMonthpicker"], controller: "UibMonthpickerController", link: function link(e, t, a, i) {
      var n = i[0],
          r = i[1];r.init(n);
    } };
}).directive("uibYearpicker", function () {
  return { replace: !0, templateUrl: function templateUrl(e, t) {
      return t.templateUrl || "uib/template/datepicker/year.html";
    }, require: ["^uibDatepicker", "uibYearpicker"], controller: "UibYearpickerController", link: function link(e, t, a, i) {
      var n = i[0];angular.extend(n, i[1]), n.yearpickerInit(), n.refreshView();
    } };
}).value("uibDatepickerPopupAttributeWarning", !0).constant("uibDatepickerPopupConfig", { altInputFormats: [], appendToBody: !1, clearText: "Clear", closeOnDateSelection: !0, closeText: "Done", currentText: "Today", datepickerPopup: "yyyy-MM-dd", datepickerPopupTemplateUrl: "uib/template/datepicker/popup.html", datepickerTemplateUrl: "uib/template/datepicker/datepicker.html", html5Types: { date: "yyyy-MM-dd", "datetime-local": "yyyy-MM-ddTHH:mm:ss.sss", month: "yyyy-MM" }, onOpenFocus: !0, showButtonBar: !0, placement: "auto bottom-left" }).controller("UibDatepickerPopupController", ["$scope", "$element", "$attrs", "$compile", "$log", "$parse", "$window", "$document", "$rootScope", "$uibPosition", "dateFilter", "uibDateParser", "uibDatepickerPopupConfig", "$timeout", "uibDatepickerConfig", "uibDatepickerPopupAttributeWarning", function (e, t, a, i, n, r, o, s, l, u, p, c, d, h, f, m) {
  function g(e) {
    return e.replace(/([A-Z])/g, function (e) {
      return "-" + e.toLowerCase();
    });
  }function b(t) {
    var a = c.parse(t, $, e.date);if (isNaN(a)) for (var i = 0; i < N.length; i++) if ((a = c.parse(t, N[i], e.date), !isNaN(a))) return a;return a;
  }function y(e) {
    if ((angular.isNumber(e) && (e = new Date(e)), !e)) return null;if (angular.isDate(e) && !isNaN(e)) return e;if (angular.isString(e)) {
      var t = b(e);if (!isNaN(t)) return c.toTimezone(t, P.timezone);
    }return C.$options && C.$options.allowInvalid ? e : void 0;
  }function v(e, t) {
    var i = e || t;return a.ngRequired || i ? (angular.isNumber(i) && (i = new Date(i)), i ? angular.isDate(i) && !isNaN(i) ? !0 : angular.isString(i) ? !isNaN(b(t)) : !1 : !0) : !0;
  }function D(a) {
    if (e.isOpen || !e.disabled) {
      var i = A[0],
          n = t[0].contains(a.target),
          r = void 0 !== i.contains && i.contains(a.target);!e.isOpen || n || r || e.$apply(function () {
        e.isOpen = !1;
      });
    }
  }function k(a) {
    27 === a.which && e.isOpen ? (a.preventDefault(), a.stopPropagation(), e.$apply(function () {
      e.isOpen = !1;
    }), t[0].focus()) : 40 !== a.which || e.isOpen || (a.preventDefault(), a.stopPropagation(), e.$apply(function () {
      e.isOpen = !0;
    }));
  }function w() {
    if (e.isOpen) {
      var i = angular.element(A[0].querySelector(".uib-datepicker-popup")),
          n = a.popupPlacement ? a.popupPlacement : d.placement,
          r = u.positionElements(t, i, n, x);i.css({ top: r.top + "px", left: r.left + "px" }), i.hasClass("uib-position-measure") && i.removeClass("uib-position-measure");
    }
  }var $,
      M,
      x,
      T,
      O,
      E,
      F,
      S,
      I,
      C,
      P,
      A,
      N,
      U = {},
      Y = !1,
      z = [];e.watchData = {}, this.init = function (u) {
    if ((C = u, P = u.$options || f.ngModelOptions, M = angular.isDefined(a.closeOnDateSelection) ? e.$parent.$eval(a.closeOnDateSelection) : d.closeOnDateSelection, x = angular.isDefined(a.datepickerAppendToBody) ? e.$parent.$eval(a.datepickerAppendToBody) : d.appendToBody, T = angular.isDefined(a.onOpenFocus) ? e.$parent.$eval(a.onOpenFocus) : d.onOpenFocus, O = angular.isDefined(a.datepickerPopupTemplateUrl) ? a.datepickerPopupTemplateUrl : d.datepickerPopupTemplateUrl, E = angular.isDefined(a.datepickerTemplateUrl) ? a.datepickerTemplateUrl : d.datepickerTemplateUrl, N = angular.isDefined(a.altInputFormats) ? e.$parent.$eval(a.altInputFormats) : d.altInputFormats, e.showButtonBar = angular.isDefined(a.showButtonBar) ? e.$parent.$eval(a.showButtonBar) : d.showButtonBar, d.html5Types[a.type] ? ($ = d.html5Types[a.type], Y = !0) : ($ = a.uibDatepickerPopup || d.datepickerPopup, a.$observe("uibDatepickerPopup", function (e) {
      var t = e || d.datepickerPopup;if (t !== $ && ($ = t, C.$modelValue = null, !$)) throw new Error("uibDatepickerPopup must have a date format specified.");
    })), !$)) throw new Error("uibDatepickerPopup must have a date format specified.");if (Y && a.uibDatepickerPopup) throw new Error("HTML5 date input types do not support custom formats.");F = angular.element("<div uib-datepicker-popup-wrap><div uib-datepicker></div></div>"), e.ngModelOptions = angular.copy(P), e.ngModelOptions.timezone = null, e.ngModelOptions.updateOnDefault === !0 && (e.ngModelOptions.updateOn = e.ngModelOptions.updateOn ? e.ngModelOptions.updateOn + " default" : "default"), F.attr({ "ng-model": "date", "ng-model-options": "ngModelOptions", "ng-change": "dateSelection(date)", "template-url": O }), S = angular.element(F.children()[0]), S.attr("template-url", E), Y && "month" === a.type && (S.attr("datepicker-mode", '"month"'), S.attr("min-mode", "month")), e.datepickerOptions && S.attr("datepicker-options", "datepickerOptions"), angular.forEach(["minMode", "maxMode", "datepickerMode", "shortcutPropagation"], function (t) {
      if (a[t]) {
        m && n.warn("uib-datepicker settings via uib-datepicker-popup attributes are deprecated and will be removed in UI Bootstrap 1.3, use datepicker-options attribute instead");var i = r(a[t]),
            o = { get: function get() {
            return i(e.$parent);
          } };if ((S.attr(g(t), "watchData." + t), "datepickerMode" === t)) {
          var s = i.assign;o.set = function (t) {
            s(e.$parent, t);
          };
        }Object.defineProperty(e.watchData, t, o);
      }
    }), angular.forEach(["minDate", "maxDate", "initDate"], function (t) {
      if (a[t]) {
        m && n.warn("uib-datepicker settings via uib-datepicker-popup attributes are deprecated and will be removed in UI Bootstrap 1.3, use datepicker-options attribute instead");var i = r(a[t]);z.push(e.$parent.$watch(i, function (a) {
          if ("minDate" === t || "maxDate" === t) U[t] = null === a ? null : angular.isDate(a) ? c.fromTimezone(new Date(a), P.timezone) : new Date(p(a, "medium")), e.watchData[t] = null === a ? null : U[t];else {
            var i = a ? new Date(a) : new Date();e.watchData[t] = c.fromTimezone(i, P.timezone);
          }
        })), S.attr(g(t), "watchData." + t);
      }
    }), a.dateDisabled && (m && n.warn("uib-datepicker settings via uib-datepicker-popup attributes are deprecated and will be removed in UI Bootstrap 1.3, use datepicker-options attribute instead"), S.attr("date-disabled", "dateDisabled({ date: date, mode: mode })")), angular.forEach(["formatDay", "formatMonth", "formatYear", "formatDayHeader", "formatDayTitle", "formatMonthTitle", "showWeeks", "startingDay", "yearRows", "yearColumns"], function (e) {
      angular.isDefined(a[e]) && (m && n.warn("uib-datepicker settings via uib-datepicker-popup attributes are deprecated and will be removed in UI Bootstrap 1.3, use datepicker-options attribute instead"), S.attr(g(e), a[e]));
    }), a.customClass && (m && n.warn("uib-datepicker settings via uib-datepicker-popup attributes are deprecated and will be removed in UI Bootstrap 1.3, use datepicker-options attribute instead"), S.attr("custom-class", "customClass({ date: date, mode: mode })")), Y ? C.$formatters.push(function (t) {
      return e.date = c.fromTimezone(t, P.timezone), t;
    }) : (C.$$parserName = "date", C.$validators.date = v, C.$parsers.unshift(y), C.$formatters.push(function (t) {
      return C.$isEmpty(t) ? (e.date = t, t) : (e.date = c.fromTimezone(t, P.timezone), angular.isNumber(e.date) && (e.date = new Date(e.date)), c.filter(e.date, $));
    })), C.$viewChangeListeners.push(function () {
      e.date = b(C.$viewValue);
    }), t.on("keydown", k), A = i(F)(e), F.remove(), x ? s.find("body").append(A) : t.after(A), e.$on("$destroy", function () {
      for (e.isOpen === !0 && (l.$$phase || e.$apply(function () {
        e.isOpen = !1;
      })), A.remove(), t.off("keydown", k), s.off("click", D), I && I.off("scroll", w), angular.element(o).off("resize", w); z.length;) z.shift()();
    });
  }, e.getText = function (t) {
    return e[t + "Text"] || d[t + "Text"];
  }, e.isDisabled = function (t) {
    return "today" === t && (t = new Date()), e.watchData.minDate && e.compare(t, U.minDate) < 0 || e.watchData.maxDate && e.compare(t, U.maxDate) > 0;
  }, e.compare = function (e, t) {
    return new Date(e.getFullYear(), e.getMonth(), e.getDate()) - new Date(t.getFullYear(), t.getMonth(), t.getDate());
  }, e.dateSelection = function (a) {
    angular.isDefined(a) && (e.date = a);var i = e.date ? c.filter(e.date, $) : null;t.val(i), C.$setViewValue(i), M && (e.isOpen = !1, t[0].focus());
  }, e.keydown = function (a) {
    27 === a.which && (a.stopPropagation(), e.isOpen = !1, t[0].focus());
  }, e.select = function (t, a) {
    if ((a.stopPropagation(), "today" === t)) {
      var i = new Date();angular.isDate(e.date) ? (t = new Date(e.date), t.setFullYear(i.getFullYear(), i.getMonth(), i.getDate())) : t = new Date(i.setHours(0, 0, 0, 0));
    }e.dateSelection(t);
  }, e.close = function (a) {
    a.stopPropagation(), e.isOpen = !1, t[0].focus();
  }, e.disabled = angular.isDefined(a.disabled) || !1, a.ngDisabled && z.push(e.$parent.$watch(r(a.ngDisabled), function (t) {
    e.disabled = t;
  })), e.$watch("isOpen", function (i) {
    i ? e.disabled ? e.isOpen = !1 : h(function () {
      w(), T && e.$broadcast("uib:datepicker.focus"), s.on("click", D);var i = a.popupPlacement ? a.popupPlacement : d.placement;x || u.parsePlacement(i)[2] ? (I = I || angular.element(u.scrollParent(t)), I && I.on("scroll", w)) : I = null, angular.element(o).on("resize", w);
    }, 0, !1) : (s.off("click", D), I && I.off("scroll", w), angular.element(o).off("resize", w));
  }), e.$on("uib:datepicker.mode", function () {
    h(w, 0, !1);
  });
}]).directive("uibDatepickerPopup", function () {
  return { require: ["ngModel", "uibDatepickerPopup"], controller: "UibDatepickerPopupController", scope: { datepickerOptions: "=?", isOpen: "=?", currentText: "@", clearText: "@", closeText: "@", dateDisabled: "&", customClass: "&" }, link: function link(e, t, a, i) {
      var n = i[0],
          r = i[1];r.init(n);
    } };
}).directive("uibDatepickerPopupWrap", function () {
  return { replace: !0, transclude: !0, templateUrl: function templateUrl(e, t) {
      return t.templateUrl || "uib/template/datepicker/popup.html";
    } };
}), angular.module("ui.bootstrap.dateparser", []).service("uibDateParser", ["$log", "$locale", "dateFilter", "orderByFilter", function (e, t, a, i) {
  function n(e, t) {
    var a = [],
        n = e.split(""),
        r = e.indexOf("'");if (r > -1) {
      var o = !1;e = e.split("");for (var s = r; s < e.length; s++) o ? ("'" === e[s] && (s + 1 < e.length && "'" === e[s + 1] ? (e[s + 1] = "$", n[s + 1] = "") : (n[s] = "", o = !1)), e[s] = "$") : "'" === e[s] && (e[s] = "$", n[s] = "", o = !0);e = e.join("");
    }return angular.forEach(h, function (i) {
      var r = e.indexOf(i.key);if (r > -1) {
        e = e.split(""), n[r] = "(" + i.regex + ")", e[r] = "$";for (var o = r + 1, s = r + i.key.length; s > o; o++) n[o] = "", e[o] = "$";e = e.join(""), a.push({ index: r, key: i.key, apply: i[t], matcher: i.regex });
      }
    }), { regex: new RegExp("^" + n.join("") + "$"), map: i(a, "index") };
  }function r(e, t, a) {
    return 1 > a ? !1 : 1 === t && a > 28 ? 29 === a && (e % 4 === 0 && e % 100 !== 0 || e % 400 === 0) : 3 === t || 5 === t || 8 === t || 10 === t ? 31 > a : !0;
  }function o(e) {
    return parseInt(e, 10);
  }function s(e, t) {
    return e && t ? c(e, t) : e;
  }function l(e, t) {
    return e && t ? c(e, t, !0) : e;
  }function u(e, t) {
    var a = Date.parse("Jan 01, 1970 00:00:00 " + e) / 6e4;return isNaN(a) ? t : a;
  }function p(e, t) {
    return e = new Date(e.getTime()), e.setMinutes(e.getMinutes() + t), e;
  }function c(e, t, a) {
    a = a ? -1 : 1;var i = u(t, e.getTimezoneOffset());return p(e, a * (i - e.getTimezoneOffset()));
  }var d,
      h,
      f = /[\\\^\$\*\+\?\|\[\]\(\)\.\{\}]/g;this.init = function () {
    d = t.id, this.parsers = {}, this.formatters = {}, h = [{ key: "yyyy", regex: "\\d{4}", apply: function apply(e) {
        this.year = +e;
      }, formatter: function formatter(e) {
        var t = new Date();return t.setFullYear(Math.abs(e.getFullYear())), a(t, "yyyy");
      } }, { key: "yy", regex: "\\d{2}", apply: function apply(e) {
        this.year = +e + 2e3;
      }, formatter: function formatter(e) {
        var t = new Date();return t.setFullYear(Math.abs(e.getFullYear())), a(t, "yy");
      } }, { key: "y", regex: "\\d{1,4}", apply: function apply(e) {
        this.year = +e;
      }, formatter: function formatter(e) {
        var t = new Date();return t.setFullYear(Math.abs(e.getFullYear())), a(t, "y");
      } }, { key: "M!", regex: "0?[1-9]|1[0-2]", apply: function apply(e) {
        this.month = e - 1;
      }, formatter: function formatter(e) {
        var t = e.getMonth();return (/^[0-9]$/.test(t) ? a(e, "MM") : a(e, "M")
        );
      } }, { key: "MMMM", regex: t.DATETIME_FORMATS.MONTH.join("|"), apply: function apply(e) {
        this.month = t.DATETIME_FORMATS.MONTH.indexOf(e);
      }, formatter: function formatter(e) {
        return a(e, "MMMM");
      } }, { key: "MMM", regex: t.DATETIME_FORMATS.SHORTMONTH.join("|"), apply: function apply(e) {
        this.month = t.DATETIME_FORMATS.SHORTMONTH.indexOf(e);
      }, formatter: function formatter(e) {
        return a(e, "MMM");
      } }, { key: "MM", regex: "0[1-9]|1[0-2]", apply: function apply(e) {
        this.month = e - 1;
      }, formatter: function formatter(e) {
        return a(e, "MM");
      } }, { key: "M", regex: "[1-9]|1[0-2]", apply: function apply(e) {
        this.month = e - 1;
      }, formatter: function formatter(e) {
        return a(e, "M");
      } }, { key: "d!", regex: "[0-2]?[0-9]{1}|3[0-1]{1}", apply: function apply(e) {
        this.date = +e;
      }, formatter: function formatter(e) {
        var t = e.getDate();return (/^[1-9]$/.test(t) ? a(e, "dd") : a(e, "d")
        );
      } }, { key: "dd", regex: "[0-2][0-9]{1}|3[0-1]{1}", apply: function apply(e) {
        this.date = +e;
      }, formatter: function formatter(e) {
        return a(e, "dd");
      } }, { key: "d", regex: "[1-2]?[0-9]{1}|3[0-1]{1}", apply: function apply(e) {
        this.date = +e;
      }, formatter: function formatter(e) {
        return a(e, "d");
      } }, { key: "EEEE", regex: t.DATETIME_FORMATS.DAY.join("|"), formatter: function formatter(e) {
        return a(e, "EEEE");
      } }, { key: "EEE", regex: t.DATETIME_FORMATS.SHORTDAY.join("|"), formatter: function formatter(e) {
        return a(e, "EEE");
      } }, { key: "HH", regex: "(?:0|1)[0-9]|2[0-3]", apply: function apply(e) {
        this.hours = +e;
      }, formatter: function formatter(e) {
        return a(e, "HH");
      } }, { key: "hh", regex: "0[0-9]|1[0-2]", apply: function apply(e) {
        this.hours = +e;
      }, formatter: function formatter(e) {
        return a(e, "hh");
      } }, { key: "H", regex: "1?[0-9]|2[0-3]", apply: function apply(e) {
        this.hours = +e;
      }, formatter: function formatter(e) {
        return a(e, "H");
      } }, { key: "h", regex: "[0-9]|1[0-2]", apply: function apply(e) {
        this.hours = +e;
      }, formatter: function formatter(e) {
        return a(e, "h");
      } }, { key: "mm", regex: "[0-5][0-9]", apply: function apply(e) {
        this.minutes = +e;
      }, formatter: function formatter(e) {
        return a(e, "mm");
      } }, { key: "m", regex: "[0-9]|[1-5][0-9]", apply: function apply(e) {
        this.minutes = +e;
      }, formatter: function formatter(e) {
        return a(e, "m");
      } }, { key: "sss", regex: "[0-9][0-9][0-9]", apply: function apply(e) {
        this.milliseconds = +e;
      }, formatter: function formatter(e) {
        return a(e, "sss");
      } }, { key: "ss", regex: "[0-5][0-9]", apply: function apply(e) {
        this.seconds = +e;
      }, formatter: function formatter(e) {
        return a(e, "ss");
      } }, { key: "s", regex: "[0-9]|[1-5][0-9]", apply: function apply(e) {
        this.seconds = +e;
      }, formatter: function formatter(e) {
        return a(e, "s");
      } }, { key: "a", regex: t.DATETIME_FORMATS.AMPMS.join("|"), apply: function apply(e) {
        12 === this.hours && (this.hours = 0), "PM" === e && (this.hours += 12);
      }, formatter: function formatter(e) {
        return a(e, "a");
      } }, { key: "Z", regex: "[+-]\\d{4}", apply: function apply(e) {
        var t = e.match(/([+-])(\d{2})(\d{2})/),
            a = t[1],
            i = t[2],
            n = t[3];this.hours += o(a + i), this.minutes += o(a + n);
      }, formatter: function formatter(e) {
        return a(e, "Z");
      } }, { key: "ww", regex: "[0-4][0-9]|5[0-3]", formatter: function formatter(e) {
        return a(e, "ww");
      } }, { key: "w", regex: "[0-9]|[1-4][0-9]|5[0-3]", formatter: function formatter(e) {
        return a(e, "w");
      } }, { key: "GGGG", regex: t.DATETIME_FORMATS.ERANAMES.join("|").replace(/\s/g, "\\s"), formatter: function formatter(e) {
        return a(e, "GGGG");
      } }, { key: "GGG", regex: t.DATETIME_FORMATS.ERAS.join("|"), formatter: function formatter(e) {
        return a(e, "GGG");
      } }, { key: "GG", regex: t.DATETIME_FORMATS.ERAS.join("|"), formatter: function formatter(e) {
        return a(e, "GG");
      } }, { key: "G", regex: t.DATETIME_FORMATS.ERAS.join("|"), formatter: function formatter(e) {
        return a(e, "G");
      } }];
  }, this.init(), this.filter = function (e, a) {
    if (!angular.isDate(e) || isNaN(e) || !a) return "";a = t.DATETIME_FORMATS[a] || a, t.id !== d && this.init(), this.formatters[a] || (this.formatters[a] = n(a, "formatter"));var i = this.formatters[a],
        r = i.map,
        o = a;return r.reduce(function (t, a, i) {
      var n = o.match(new RegExp("(.*)" + a.key));n && angular.isString(n[1]) && (t += n[1], o = o.replace(n[1] + a.key, ""));var s = i === r.length - 1 ? o : "";return a.apply ? t + a.apply.call(null, e) + s : t + s;
    }, "");
  }, this.parse = function (a, i, o) {
    if (!angular.isString(a) || !i) return a;i = t.DATETIME_FORMATS[i] || i, i = i.replace(f, "\\$&"), t.id !== d && this.init(), this.parsers[i] || (this.parsers[i] = n(i, "apply"));var s = this.parsers[i],
        l = s.regex,
        u = s.map,
        p = a.match(l),
        c = !1;if (p && p.length) {
      var h, m;angular.isDate(o) && !isNaN(o.getTime()) ? h = { year: o.getFullYear(), month: o.getMonth(), date: o.getDate(), hours: o.getHours(), minutes: o.getMinutes(), seconds: o.getSeconds(), milliseconds: o.getMilliseconds() } : (o && e.warn("dateparser:", "baseDate is not a valid date"), h = { year: 1900, month: 0, date: 1, hours: 0, minutes: 0, seconds: 0, milliseconds: 0 });for (var g = 1, b = p.length; b > g; g++) {
        var y = u[g - 1];"Z" === y.matcher && (c = !0), y.apply && y.apply.call(h, p[g]);
      }var v = c ? Date.prototype.setUTCFullYear : Date.prototype.setFullYear,
          D = c ? Date.prototype.setUTCHours : Date.prototype.setHours;return r(h.year, h.month, h.date) && (!angular.isDate(o) || isNaN(o.getTime()) || c ? (m = new Date(0), v.call(m, h.year, h.month, h.date), D.call(m, h.hours || 0, h.minutes || 0, h.seconds || 0, h.milliseconds || 0)) : (m = new Date(o), v.call(m, h.year, h.month, h.date), D.call(m, h.hours, h.minutes, h.seconds, h.milliseconds))), m;
    }
  }, this.toTimezone = s, this.fromTimezone = l, this.timezoneToOffset = u, this.addDateMinutes = p, this.convertTimezoneToLocal = c;
}]), angular.module("ui.bootstrap.isClass", []).directive("uibIsClass", ["$animate", function (e) {
  var t = /^\s*([\s\S]+?)\s+on\s+([\s\S]+?)\s*$/,
      a = /^\s*([\s\S]+?)\s+for\s+([\s\S]+?)\s*$/;return { restrict: "A", compile: function compile(i, n) {
      function r(e, t) {
        l.push(e), u.push({ scope: e, element: t }), f.forEach(function (t) {
          o(t, e);
        }), e.$on("$destroy", s);
      }function o(t, i) {
        var n = t.match(a),
            r = i.$eval(n[1]),
            o = n[2],
            s = p[t];if (!s) {
          var l = function l(t) {
            var a = null;u.some(function (e) {
              var i = e.scope.$eval(d);return i === t ? (a = e, !0) : void 0;
            }), s.lastActivated !== a && (s.lastActivated && e.removeClass(s.lastActivated.element, r), a && e.addClass(a.element, r), s.lastActivated = a);
          };p[t] = s = { lastActivated: null, scope: i, watchFn: l, compareWithExp: o, watcher: i.$watch(o, l) };
        }s.watchFn(i.$eval(o));
      }function s(e) {
        var t = e.targetScope,
            a = l.indexOf(t);if ((l.splice(a, 1), u.splice(a, 1), l.length)) {
          var i = l[0];angular.forEach(p, function (e) {
            e.scope === t && (e.watcher = i.$watch(e.compareWithExp, e.watchFn), e.scope = i);
          });
        } else p = {};
      }var l = [],
          u = [],
          p = {},
          c = n.uibIsClass.match(t),
          d = c[2],
          h = c[1],
          f = h.split(",");return r;
    } };
}]), angular.module("ui.bootstrap.position", []).factory("$uibPosition", ["$document", "$window", function (e, t) {
  var a,
      i = { normal: /(auto|scroll)/, hidden: /(auto|scroll|hidden)/ },
      n = { auto: /\s?auto?\s?/i, primary: /^(top|bottom|left|right)$/, secondary: /^(top|bottom|left|right|center)$/, vertical: /^(top|bottom)$/ };return { getRawNode: function getRawNode(e) {
      return e.nodeName ? e : e[0] || e;
    }, parseStyle: function parseStyle(e) {
      return e = parseFloat(e), isFinite(e) ? e : 0;
    }, offsetParent: function offsetParent(a) {
      function i(e) {
        return "static" === (t.getComputedStyle(e).position || "static");
      }a = this.getRawNode(a);for (var n = a.offsetParent || e[0].documentElement; n && n !== e[0].documentElement && i(n);) n = n.offsetParent;return n || e[0].documentElement;
    }, scrollbarWidth: function scrollbarWidth() {
      if (angular.isUndefined(a)) {
        var t = angular.element('<div class="uib-position-scrollbar-measure"></div>');e.find("body").append(t), a = t[0].offsetWidth - t[0].clientWidth, a = isFinite(a) ? a : 0, t.remove();
      }return a;
    }, isScrollable: function isScrollable(e, a) {
      e = this.getRawNode(e);var n = a ? i.hidden : i.normal,
          r = t.getComputedStyle(e);return n.test(r.overflow + r.overflowY + r.overflowX);
    }, scrollParent: function scrollParent(a, n) {
      a = this.getRawNode(a);var r = n ? i.hidden : i.normal,
          o = e[0].documentElement,
          s = t.getComputedStyle(a),
          l = "absolute" === s.position,
          u = a.parentElement || o;if (u === o || "fixed" === s.position) return o;for (; u.parentElement && u !== o;) {
        var p = t.getComputedStyle(u);if ((l && "static" !== p.position && (l = !1), !l && r.test(p.overflow + p.overflowY + p.overflowX))) break;
        u = u.parentElement;
      }return u;
    }, position: function position(a, i) {
      a = this.getRawNode(a);var n = this.offset(a);if (i) {
        var r = t.getComputedStyle(a);n.top -= this.parseStyle(r.marginTop), n.left -= this.parseStyle(r.marginLeft);
      }var o = this.offsetParent(a),
          s = { top: 0, left: 0 };return o !== e[0].documentElement && (s = this.offset(o), s.top += o.clientTop - o.scrollTop, s.left += o.clientLeft - o.scrollLeft), { width: Math.round(angular.isNumber(n.width) ? n.width : a.offsetWidth), height: Math.round(angular.isNumber(n.height) ? n.height : a.offsetHeight), top: Math.round(n.top - s.top), left: Math.round(n.left - s.left) };
    }, offset: function offset(a) {
      a = this.getRawNode(a);var i = a.getBoundingClientRect();return { width: Math.round(angular.isNumber(i.width) ? i.width : a.offsetWidth), height: Math.round(angular.isNumber(i.height) ? i.height : a.offsetHeight), top: Math.round(i.top + (t.pageYOffset || e[0].documentElement.scrollTop)), left: Math.round(i.left + (t.pageXOffset || e[0].documentElement.scrollLeft)) };
    }, viewportOffset: function viewportOffset(a, i, n) {
      a = this.getRawNode(a), n = n !== !1 ? !0 : !1;var r = a.getBoundingClientRect(),
          o = { top: 0, left: 0, bottom: 0, right: 0 },
          s = i ? e[0].documentElement : this.scrollParent(a),
          l = s.getBoundingClientRect();if ((o.top = l.top + s.clientTop, o.left = l.left + s.clientLeft, s === e[0].documentElement && (o.top += t.pageYOffset, o.left += t.pageXOffset), o.bottom = o.top + s.clientHeight, o.right = o.left + s.clientWidth, n)) {
        var u = t.getComputedStyle(s);o.top += this.parseStyle(u.paddingTop), o.bottom -= this.parseStyle(u.paddingBottom), o.left += this.parseStyle(u.paddingLeft), o.right -= this.parseStyle(u.paddingRight);
      }return { top: Math.round(r.top - o.top), bottom: Math.round(o.bottom - r.bottom), left: Math.round(r.left - o.left), right: Math.round(o.right - r.right) };
    }, parsePlacement: function parsePlacement(e) {
      var t = n.auto.test(e);return t && (e = e.replace(n.auto, "")), e = e.split("-"), e[0] = e[0] || "top", n.primary.test(e[0]) || (e[0] = "top"), e[1] = e[1] || "center", n.secondary.test(e[1]) || (e[1] = "center"), e[2] = t ? !0 : !1, e;
    }, positionElements: function positionElements(e, a, i, r) {
      e = this.getRawNode(e), a = this.getRawNode(a);var o = angular.isDefined(a.offsetWidth) ? a.offsetWidth : a.prop("offsetWidth"),
          s = angular.isDefined(a.offsetHeight) ? a.offsetHeight : a.prop("offsetHeight");i = this.parsePlacement(i);var l = r ? this.offset(e) : this.position(e),
          u = { top: 0, left: 0, placement: "" };if (i[2]) {
        var p = this.viewportOffset(e, r),
            c = t.getComputedStyle(a),
            d = { width: o + Math.round(Math.abs(this.parseStyle(c.marginLeft) + this.parseStyle(c.marginRight))), height: s + Math.round(Math.abs(this.parseStyle(c.marginTop) + this.parseStyle(c.marginBottom))) };if ((i[0] = "top" === i[0] && d.height > p.top && d.height <= p.bottom ? "bottom" : "bottom" === i[0] && d.height > p.bottom && d.height <= p.top ? "top" : "left" === i[0] && d.width > p.left && d.width <= p.right ? "right" : "right" === i[0] && d.width > p.right && d.width <= p.left ? "left" : i[0], i[1] = "top" === i[1] && d.height - l.height > p.bottom && d.height - l.height <= p.top ? "bottom" : "bottom" === i[1] && d.height - l.height > p.top && d.height - l.height <= p.bottom ? "top" : "left" === i[1] && d.width - l.width > p.right && d.width - l.width <= p.left ? "right" : "right" === i[1] && d.width - l.width > p.left && d.width - l.width <= p.right ? "left" : i[1], "center" === i[1])) if (n.vertical.test(i[0])) {
          var h = l.width / 2 - o / 2;p.left + h < 0 && d.width - l.width <= p.right ? i[1] = "left" : p.right + h < 0 && d.width - l.width <= p.left && (i[1] = "right");
        } else {
          var f = l.height / 2 - d.height / 2;p.top + f < 0 && d.height - l.height <= p.bottom ? i[1] = "top" : p.bottom + f < 0 && d.height - l.height <= p.top && (i[1] = "bottom");
        }
      }switch (i[0]) {case "top":
          u.top = l.top - s;break;case "bottom":
          u.top = l.top + l.height;break;case "left":
          u.left = l.left - o;break;case "right":
          u.left = l.left + l.width;}switch (i[1]) {case "top":
          u.top = l.top;break;case "bottom":
          u.top = l.top + l.height - s;break;case "left":
          u.left = l.left;break;case "right":
          u.left = l.left + l.width - o;break;case "center":
          n.vertical.test(i[0]) ? u.left = l.left + l.width / 2 - o / 2 : u.top = l.top + l.height / 2 - s / 2;}return u.top = Math.round(u.top), u.left = Math.round(u.left), u.placement = "center" === i[1] ? i[0] : i[0] + "-" + i[1], u;
    }, positionArrow: function positionArrow(e, a) {
      e = this.getRawNode(e);var i = e.querySelector(".tooltip-inner, .popover-inner");if (i) {
        var r = angular.element(i).hasClass("tooltip-inner"),
            o = e.querySelector(r ? ".tooltip-arrow" : ".arrow");if (o) {
          var s = { top: "", bottom: "", left: "", right: "" };if ((a = this.parsePlacement(a), "center" === a[1])) return void angular.element(o).css(s);var l = "border-" + a[0] + "-width",
              u = t.getComputedStyle(o)[l],
              p = "border-";p += n.vertical.test(a[0]) ? a[0] + "-" + a[1] : a[1] + "-" + a[0], p += "-radius";var c = t.getComputedStyle(r ? i : e)[p];switch (a[0]) {case "top":
              s.bottom = r ? "0" : "-" + u;break;case "bottom":
              s.top = r ? "0" : "-" + u;break;case "left":
              s.right = r ? "0" : "-" + u;break;case "right":
              s.left = r ? "0" : "-" + u;}s[a[1]] = c, angular.element(o).css(s);
        }
      }
    } };
}]), angular.module("ui.bootstrap.typeahead", ["ui.bootstrap.debounce", "ui.bootstrap.position"]).factory("uibTypeaheadParser", ["$parse", function (e) {
  var t = /^\s*([\s\S]+?)(?:\s+as\s+([\s\S]+?))?\s+for\s+(?:([\$\w][\$\w\d]*))\s+in\s+([\s\S]+?)$/;return { parse: function parse(a) {
      var i = a.match(t);if (!i) throw new Error('Expected typeahead specification in form of "_modelValue_ (as _label_)? for _item_ in _collection_" but got "' + a + '".');return { itemName: i[3], source: e(i[4]), viewMapper: e(i[2] || i[1]), modelMapper: e(i[1]) };
    } };
}]).controller("UibTypeaheadController", ["$scope", "$element", "$attrs", "$compile", "$parse", "$q", "$timeout", "$document", "$window", "$rootScope", "$$debounce", "$uibPosition", "uibTypeaheadParser", function (e, t, a, i, n, r, o, s, l, u, p, c, d) {
  function h() {
    R.moveInProgress || (R.moveInProgress = !0, R.$digest()), X();
  }function f() {
    R.position = F ? c.offset(t) : c.position(t), R.position.top += t.prop("offsetHeight");
  }var m,
      g,
      b = [9, 13, 27, 38, 40],
      y = 200,
      v = e.$eval(a.typeaheadMinLength);v || 0 === v || (v = 1), e.$watch(a.typeaheadMinLength, function (e) {
    v = e || 0 === e ? e : 1;
  });var D = e.$eval(a.typeaheadWaitMs) || 0,
      k = e.$eval(a.typeaheadEditable) !== !1;e.$watch(a.typeaheadEditable, function (e) {
    k = e !== !1;
  });var w,
      $,
      M = n(a.typeaheadLoading).assign || angular.noop,
      x = n(a.typeaheadOnSelect),
      T = angular.isDefined(a.typeaheadSelectOnBlur) ? e.$eval(a.typeaheadSelectOnBlur) : !1,
      O = n(a.typeaheadNoResults).assign || angular.noop,
      E = a.typeaheadInputFormatter ? n(a.typeaheadInputFormatter) : void 0,
      F = a.typeaheadAppendToBody ? e.$eval(a.typeaheadAppendToBody) : !1,
      S = a.typeaheadAppendTo ? e.$eval(a.typeaheadAppendTo) : null,
      I = e.$eval(a.typeaheadFocusFirst) !== !1,
      C = a.typeaheadSelectOnExact ? e.$eval(a.typeaheadSelectOnExact) : !1,
      P = n(a.typeaheadIsOpen).assign || angular.noop,
      A = e.$eval(a.typeaheadShowHint) || !1,
      N = n(a.ngModel),
      U = n(a.ngModel + "($$$p)"),
      Y = function Y(t, a) {
    return angular.isFunction(N(e)) && g && g.$options && g.$options.getterSetter ? U(t, { $$$p: a }) : N.assign(t, a);
  },
      z = d.parse(a.uibTypeahead),
      R = e.$new(),
      V = e.$on("$destroy", function () {
    R.$destroy();
  });R.$on("$destroy", V);var H = "typeahead-" + R.$id + "-" + Math.floor(1e4 * Math.random());t.attr({ "aria-autocomplete": "list", "aria-expanded": !1, "aria-owns": H });var B, W;A && (B = angular.element("<div></div>"), B.css("position", "relative"), t.after(B), W = t.clone(), W.attr("placeholder", ""), W.attr("tabindex", "-1"), W.val(""), W.css({ position: "absolute", top: "0px", left: "0px", "border-color": "transparent", "box-shadow": "none", opacity: 1, background: "none 0% 0% / auto repeat scroll padding-box border-box rgb(255, 255, 255)", color: "#999" }), t.css({ position: "relative", "vertical-align": "top", "background-color": "transparent" }), B.append(W), W.after(t));var q = angular.element("<div uib-typeahead-popup></div>");q.attr({ id: H, matches: "matches", active: "activeIdx", select: "select(activeIdx, evt)", "move-in-progress": "moveInProgress", query: "query", position: "position", "assign-is-open": "assignIsOpen(isOpen)", debounce: "debounceUpdate" }), angular.isDefined(a.typeaheadTemplateUrl) && q.attr("template-url", a.typeaheadTemplateUrl), angular.isDefined(a.typeaheadPopupTemplateUrl) && q.attr("popup-template-url", a.typeaheadPopupTemplateUrl);var _ = function _() {
    A && W.val("");
  },
      j = function j() {
    R.matches = [], R.activeIdx = -1, t.attr("aria-expanded", !1), _();
  },
      G = function G(e) {
    return H + "-option-" + e;
  };R.$watch("activeIdx", function (e) {
    0 > e ? t.removeAttr("aria-activedescendant") : t.attr("aria-activedescendant", G(e));
  });var L = function L(e, t) {
    return R.matches.length > t && e ? e.toUpperCase() === R.matches[t].label.toUpperCase() : !1;
  },
      K = function K(a, i) {
    var n = { $viewValue: a };M(e, !0), O(e, !1), r.when(z.source(e, n)).then(function (r) {
      var o = a === m.$viewValue;if (o && w) if (r && r.length > 0) {
        R.activeIdx = I ? 0 : -1, O(e, !1), R.matches.length = 0;for (var s = 0; s < r.length; s++) n[z.itemName] = r[s], R.matches.push({ id: G(s), label: z.viewMapper(R, n), model: r[s] });if ((R.query = a, f(), t.attr("aria-expanded", !0), C && 1 === R.matches.length && L(a, 0) && (angular.isNumber(R.debounceUpdate) || angular.isObject(R.debounceUpdate) ? p(function () {
          R.select(0, i);
        }, angular.isNumber(R.debounceUpdate) ? R.debounceUpdate : R.debounceUpdate["default"]) : R.select(0, i)), A)) {
          var l = R.matches[0].label;W.val(angular.isString(a) && a.length > 0 && l.slice(0, a.length).toUpperCase() === a.toUpperCase() ? a + l.slice(a.length) : "");
        }
      } else j(), O(e, !0);o && M(e, !1);
    }, function () {
      j(), M(e, !1), O(e, !0);
    });
  };F && (angular.element(l).on("resize", h), s.find("body").on("scroll", h));var X = p(function () {
    R.matches.length && f(), R.moveInProgress = !1;
  }, y);R.moveInProgress = !1, R.query = void 0;var Z,
      J = function J(e) {
    Z = o(function () {
      K(e);
    }, D);
  },
      Q = function Q() {
    Z && o.cancel(Z);
  };j(), R.assignIsOpen = function (t) {
    P(e, t);
  }, R.select = function (i, n) {
    var r,
        s,
        l = {};$ = !0, l[z.itemName] = s = R.matches[i].model, r = z.modelMapper(e, l), Y(e, r), m.$setValidity("editable", !0), m.$setValidity("parse", !0), x(e, { $item: s, $model: r, $label: z.viewMapper(e, l), $event: n }), j(), R.$eval(a.typeaheadFocusOnSelect) !== !1 && o(function () {
      t[0].focus();
    }, 0, !1);
  }, t.on("keydown", function (t) {
    if (0 !== R.matches.length && -1 !== b.indexOf(t.which)) {
      if (-1 === R.activeIdx && (9 === t.which || 13 === t.which) || 9 === t.which && t.shiftKey) return j(), void R.$digest();t.preventDefault();var a;switch (t.which) {case 9:case 13:
          R.$apply(function () {
            angular.isNumber(R.debounceUpdate) || angular.isObject(R.debounceUpdate) ? p(function () {
              R.select(R.activeIdx, t);
            }, angular.isNumber(R.debounceUpdate) ? R.debounceUpdate : R.debounceUpdate["default"]) : R.select(R.activeIdx, t);
          });break;case 27:
          t.stopPropagation(), j(), e.$digest();break;case 38:
          R.activeIdx = (R.activeIdx > 0 ? R.activeIdx : R.matches.length) - 1, R.$digest(), a = q.find("li")[R.activeIdx], a.parentNode.scrollTop = a.offsetTop;break;case 40:
          R.activeIdx = (R.activeIdx + 1) % R.matches.length, R.$digest(), a = q.find("li")[R.activeIdx], a.parentNode.scrollTop = a.offsetTop;}
    }
  }), t.bind("focus", function (e) {
    w = !0, 0 !== v || m.$viewValue || o(function () {
      K(m.$viewValue, e);
    }, 0);
  }), t.bind("blur", function (e) {
    T && R.matches.length && -1 !== R.activeIdx && !$ && ($ = !0, R.$apply(function () {
      angular.isObject(R.debounceUpdate) && angular.isNumber(R.debounceUpdate.blur) ? p(function () {
        R.select(R.activeIdx, e);
      }, R.debounceUpdate.blur) : R.select(R.activeIdx, e);
    })), !k && m.$error.editable && (m.$viewValue = "", t.val("")), w = !1, $ = !1;
  });var et = function et(a) {
    t[0] !== a.target && 3 !== a.which && 0 !== R.matches.length && (j(), u.$$phase || e.$digest());
  };s.on("click", et), e.$on("$destroy", function () {
    s.off("click", et), (F || S) && tt.remove(), F && (angular.element(l).off("resize", h), s.find("body").off("scroll", h)), q.remove(), A && B.remove();
  });var tt = i(q)(R);F ? s.find("body").append(tt) : S ? angular.element(S).eq(0).append(tt) : t.after(tt), this.init = function (t, a) {
    m = t, g = a, R.debounceUpdate = m.$options && n(m.$options.debounce)(e), m.$parsers.unshift(function (t) {
      return w = !0, 0 === v || t && t.length >= v ? D > 0 ? (Q(), J(t)) : K(t) : (M(e, !1), Q(), j()), k ? t : t ? void m.$setValidity("editable", !1) : (m.$setValidity("editable", !0), null);
    }), m.$formatters.push(function (t) {
      var a,
          i,
          n = {};return k || m.$setValidity("editable", !0), E ? (n.$model = t, E(e, n)) : (n[z.itemName] = t, a = z.viewMapper(e, n), n[z.itemName] = void 0, i = z.viewMapper(e, n), a !== i ? a : t);
    });
  };
}]).directive("uibTypeahead", function () {
  return { controller: "UibTypeaheadController", require: ["ngModel", "^?ngModelOptions", "uibTypeahead"], link: function link(e, t, a, i) {
      i[2].init(i[0], i[1]);
    } };
}).directive("uibTypeaheadPopup", ["$$debounce", function (e) {
  return { scope: { matches: "=", query: "=", active: "=", position: "&", moveInProgress: "=", select: "&", assignIsOpen: "&", debounce: "&" }, replace: !0, templateUrl: function templateUrl(e, t) {
      return t.popupTemplateUrl || "uib/template/typeahead/typeahead-popup.html";
    }, link: function link(t, a, i) {
      t.templateUrl = i.templateUrl, t.isOpen = function () {
        var e = t.matches.length > 0;return t.assignIsOpen({ isOpen: e }), e;
      }, t.isActive = function (e) {
        return t.active === e;
      }, t.selectActive = function (e) {
        t.active = e;
      }, t.selectMatch = function (a, i) {
        var n = t.debounce();angular.isNumber(n) || angular.isObject(n) ? e(function () {
          t.select({ activeIdx: a, evt: i });
        }, angular.isNumber(n) ? n : n["default"]) : t.select({ activeIdx: a, evt: i });
      };
    } };
}]).directive("uibTypeaheadMatch", ["$templateRequest", "$compile", "$parse", function (e, t, a) {
  return { scope: { index: "=", match: "=", query: "=" }, link: function link(i, n, r) {
      var o = a(r.templateUrl)(i.$parent) || "uib/template/typeahead/typeahead-match.html";e(o).then(function (e) {
        var a = angular.element(e.trim());n.replaceWith(a), t(a)(i);
      });
    } };
}]).filter("uibTypeaheadHighlight", ["$sce", "$injector", "$log", function (e, t, a) {
  function i(e) {
    return e.replace(/([.?*+^$[\]\\(){}|-])/g, "\\$1");
  }function n(e) {
    return (/<.*>/g.test(e)
    );
  }var r;return r = t.has("$sanitize"), function (t, o) {
    return !r && n(t) && a.warn("Unsafe use of typeahead please use ngSanitize"), t = o ? ("" + t).replace(new RegExp(i(o), "gi"), "<strong>$&</strong>") : t, r || (t = e.trustAsHtml(t)), t;
  };
}]), angular.module("ui.bootstrap.debounce", []).factory("$$debounce", ["$timeout", function (e) {
  return function (t, a) {
    var i;return function () {
      var n = this,
          r = Array.prototype.slice.call(arguments);i && e.cancel(i), i = e(function () {
        t.apply(n, r);
      }, a);
    };
  };
}]), angular.module("uib/template/datepicker/datepicker.html", []).run(["$templateCache", function (e) {
  e.put("uib/template/datepicker/datepicker.html", '<div class="uib-datepicker" ng-switch="datepickerMode" role="application" ng-keydown="keydown($event)">\n  <uib-daypicker ng-switch-when="day" tabindex="0"></uib-daypicker>\n  <uib-monthpicker ng-switch-when="month" tabindex="0"></uib-monthpicker>\n  <uib-yearpicker ng-switch-when="year" tabindex="0"></uib-yearpicker>\n</div>\n');
}]), angular.module("uib/template/datepicker/day.html", []).run(["$templateCache", function (e) {
  e.put("uib/template/datepicker/day.html", '<table class="uib-daypicker" role="grid" aria-labelledby="{{::uniqueId}}-title" aria-activedescendant="{{activeDateId}}">\n  <thead>\n    <tr>\n      <th><button type="button" class="btn btn-default btn-sm pull-left uib-left" ng-click="move(-1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-left"></i></button></th>\n      <th colspan="{{::5 + showWeeks}}"><button id="{{::uniqueId}}-title" role="heading" aria-live="assertive" aria-atomic="true" type="button" class="btn btn-default btn-sm uib-title" ng-click="toggleMode()" ng-disabled="datepickerMode === maxMode" tabindex="-1"><strong>{{title}}</strong></button></th>\n      <th><button type="button" class="btn btn-default btn-sm pull-right uib-right" ng-click="move(1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-right"></i></button></th>\n    </tr>\n    <tr>\n      <th ng-if="showWeeks" class="text-center"></th>\n      <th ng-repeat="label in ::labels track by $index" class="text-center"><small aria-label="{{::label.full}}">{{::label.abbr}}</small></th>\n    </tr>\n  </thead>\n  <tbody>\n    <tr class="uib-weeks" ng-repeat="row in rows track by $index">\n      <td ng-if="showWeeks" class="text-center h6"><em>{{ weekNumbers[$index] }}</em></td>\n      <td ng-repeat="dt in row" class="uib-day text-center" role="gridcell"\n        id="{{::dt.uid}}"\n        ng-class="::dt.customClass">\n        <button type="button" class="btn btn-default btn-sm"\n          uib-is-class="\n            \'btn-info\' for selectedDt,\n            \'active\' for activeDt\n            on dt"\n          ng-click="select(dt.date)"\n          ng-disabled="::dt.disabled"\n          tabindex="-1"><span ng-class="::{\'text-muted\': dt.secondary, \'text-info\': dt.current}">{{::dt.label}}</span></button>\n      </td>\n    </tr>\n  </tbody>\n</table>\n');
}]), angular.module("uib/template/datepicker/month.html", []).run(["$templateCache", function (e) {
  e.put("uib/template/datepicker/month.html", '<table class="uib-monthpicker" role="grid" aria-labelledby="{{::uniqueId}}-title" aria-activedescendant="{{activeDateId}}">\n  <thead>\n    <tr>\n      <th><button type="button" class="btn btn-default btn-sm pull-left uib-left" ng-click="move(-1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-left"></i></button></th>\n      <th><button id="{{::uniqueId}}-title" role="heading" aria-live="assertive" aria-atomic="true" type="button" class="btn btn-default btn-sm uib-title" ng-click="toggleMode()" ng-disabled="datepickerMode === maxMode" tabindex="-1"><strong>{{title}}</strong></button></th>\n      <th><button type="button" class="btn btn-default btn-sm pull-right uib-right" ng-click="move(1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-right"></i></button></th>\n    </tr>\n  </thead>\n  <tbody>\n    <tr class="uib-months" ng-repeat="row in rows track by $index">\n      <td ng-repeat="dt in row" class="uib-month text-center" role="gridcell"\n        id="{{::dt.uid}}"\n        ng-class="::dt.customClass">\n        <button type="button" class="btn btn-default"\n          uib-is-class="\n            \'btn-info\' for selectedDt,\n            \'active\' for activeDt\n            on dt"\n          ng-click="select(dt.date)"\n          ng-disabled="::dt.disabled"\n          tabindex="-1"><span ng-class="::{\'text-info\': dt.current}">{{::dt.label}}</span></button>\n      </td>\n    </tr>\n  </tbody>\n</table>\n');
}]), angular.module("uib/template/datepicker/popup.html", []).run(["$templateCache", function (e) {
  e.put("uib/template/datepicker/popup.html", '<div>\n  <ul class="uib-datepicker-popup dropdown-menu uib-position-measure" dropdown-nested ng-if="isOpen" ng-keydown="keydown($event)" ng-click="$event.stopPropagation()">\n    <li ng-transclude></li>\n    <li ng-if="showButtonBar" class="uib-button-bar">\n      <span class="btn-group pull-left">\n        <button type="button" class="btn btn-sm btn-info uib-datepicker-current" ng-click="select(\'today\', $event)" ng-disabled="isDisabled(\'today\')">{{ getText(\'current\') }}</button>\n        <button type="button" class="btn btn-sm btn-danger uib-clear" ng-click="select(null, $event)">{{ getText(\'clear\') }}</button>\n      </span>\n      <button type="button" class="btn btn-sm btn-success pull-right uib-close" ng-click="close($event)">{{ getText(\'close\') }}</button>\n    </li>\n  </ul>\n</div>\n');
}]), angular.module("uib/template/datepicker/year.html", []).run(["$templateCache", function (e) {
  e.put("uib/template/datepicker/year.html", '<table class="uib-yearpicker" role="grid" aria-labelledby="{{::uniqueId}}-title" aria-activedescendant="{{activeDateId}}">\n  <thead>\n    <tr>\n      <th><button type="button" class="btn btn-default btn-sm pull-left uib-left" ng-click="move(-1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-left"></i></button></th>\n      <th colspan="{{::columns - 2}}"><button id="{{::uniqueId}}-title" role="heading" aria-live="assertive" aria-atomic="true" type="button" class="btn btn-default btn-sm uib-title" ng-click="toggleMode()" ng-disabled="datepickerMode === maxMode" tabindex="-1"><strong>{{title}}</strong></button></th>\n      <th><button type="button" class="btn btn-default btn-sm pull-right uib-right" ng-click="move(1)" tabindex="-1"><i class="glyphicon glyphicon-chevron-right"></i></button></th>\n    </tr>\n  </thead>\n  <tbody>\n    <tr class="uib-years" ng-repeat="row in rows track by $index">\n      <td ng-repeat="dt in row" class="uib-year text-center" role="gridcell"\n        id="{{::dt.uid}}"\n        ng-class="::dt.customClass">\n        <button type="button" class="btn btn-default"\n          uib-is-class="\n            \'btn-info\' for selectedDt,\n            \'active\' for activeDt\n            on dt"\n          ng-click="select(dt.date)"\n          ng-disabled="::dt.disabled"\n          tabindex="-1"><span ng-class="::{\'text-info\': dt.current}">{{::dt.label}}</span></button>\n      </td>\n    </tr>\n  </tbody>\n</table>\n');
}]), angular.module("uib/template/typeahead/typeahead-match.html", []).run(["$templateCache", function (e) {
  e.put("uib/template/typeahead/typeahead-match.html", '<a href\n   tabindex="-1"\n   ng-bind-html="match.label | uibTypeaheadHighlight:query"\n   ng-attr-title="{{match.label}}"></a>\n');
}]), angular.module("uib/template/typeahead/typeahead-popup.html", []).run(["$templateCache", function (e) {
  e.put("uib/template/typeahead/typeahead-popup.html", '<ul class="dropdown-menu" ng-show="isOpen() && !moveInProgress" ng-style="{top: position().top+\'px\', left: position().left+\'px\'}" role="listbox" aria-hidden="{{!isOpen()}}">\n    <li ng-repeat="match in matches track by $index" ng-class="{active: isActive($index) }" ng-mouseenter="selectActive($index)" ng-click="selectMatch($index, $event)" role="option" id="{{::match.id}}">\n        <div uib-typeahead-match index="$index" match="match" query="query" template-url="templateUrl"></div>\n    </li>\n</ul>\n');
}]), angular.module("ui.bootstrap.datepicker").run(function () {
  !angular.$$csp().noInlineStyle && !angular.$$uibDatepickerCss && angular.element(document).find("head").prepend('<style type="text/css">.uib-datepicker .uib-title{width:100%;}.uib-day button,.uib-month button,.uib-year button{min-width:100%;}.uib-datepicker-popup.dropdown-menu{display:block;float:none;margin:0;}.uib-button-bar{padding:10px 9px 2px;}.uib-left,.uib-right{width:100%}</style>'), angular.$$uibDatepickerCss = !0;
}), angular.module("ui.bootstrap.position").run(function () {
  !angular.$$csp().noInlineStyle && !angular.$$uibPositionCss && angular.element(document).find("head").prepend('<style type="text/css">.uib-position-measure{display:block !important;visibility:hidden !important;position:absolute !important;top:-9999px !important;left:-9999px !important;}.uib-position-scrollbar-measure{position:absolute;top:-9999px;width:50px;height:50px;overflow:scroll;}</style>'), angular.$$uibPositionCss = !0;
}), angular.module("ui.bootstrap.typeahead").run(function () {
  !angular.$$csp().noInlineStyle && !angular.$$uibTypeaheadCss && angular.element(document).find("head").prepend('<style type="text/css">[uib-typeahead-popup].dropdown-menu{display:block;}</style>'), angular.$$uibTypeaheadCss = !0;
});

},{}]},{},[15])


//# sourceMappingURL=search.js.map
