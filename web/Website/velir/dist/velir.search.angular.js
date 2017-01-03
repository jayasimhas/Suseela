(function (f) { if (typeof exports === "object" && typeof module !== "undefined") { module.exports = f() } else if (typeof define === "function" && define.amd) { define([], f) } else { var g; if (typeof window !== "undefined") { g = window } else if (typeof global !== "undefined") { g = global } else if (typeof self !== "undefined") { g = self } else { g = this } (g.velir || (g.velir = {})).search = f() } })(function () {
    var define, module, exports; return (function e(t, n, r) { function s(o, u) { if (!n[o]) { if (!t[o]) { var a = typeof require == "function" && require; if (!u && a) return a(o, !0); if (i) return i(o, !0); var f = new Error("Cannot find module '" + o + "'"); throw f.code = "MODULE_NOT_FOUND", f } var l = n[o] = { exports: {} }; t[o][0].call(l.exports, function (e) { var n = t[o][1][e]; return s(n ? n : e) }, l, l.exports, e, t, n, r) } return n[o].exports } var i = typeof require == "function" && require; for (var o = 0; o < r.length; o++) s(r[o]); return s })({
        1: [function (require, module, exports) {
            'use strict';

            Object.defineProperty(exports, '__esModule', {
                value: true
            });

            function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

            function _defaults(obj, defaults) { var keys = Object.getOwnPropertyNames(defaults); for (var i = 0; i < keys.length; i++) { var key = keys[i]; var value = Object.getOwnPropertyDescriptor(defaults, key); if (value && value.configurable && obj[key] === undefined) { Object.defineProperty(obj, key, value); } } return obj; }

            var _AngularSearchModule = require('./Angular/SearchModule');

            var _Core = require('./Core');

            _defaults(exports, _interopRequireWildcard(_Core));

            var Angular = {
                SearchModule: _AngularSearchModule.SearchModule
            };

            exports.Angular = Angular;

        }, { "./Angular/SearchModule": 10, "./Core": 11 }], 2: [function (require, module, exports) {
            "use strict";

            Object.defineProperty(exports, "__esModule", {
                value: true
            });
            var BaseUiConfig = {
                typingTimeout: 500
            };

            exports.BaseUiConfig = BaseUiConfig;

        }, {}], 3: [function (require, module, exports) {
            (function (global) {
                'use strict';

                Object.defineProperty(exports, '__esModule', {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var FacetsController = (function () {
                    function FacetsController($scope, $location, searchService) {
                        var _this = this;

                        _classCallCheck(this, FacetsController);

                        this.facetGroups = searchService.getFacetGroups();
                        this.searchService = searchService;
                        this.location = $location;

                        $scope.$watch(function () {
                           
                            return searchService.getFacetGroups();
                        }, function () {
                            _this.facetGroups = searchService.getFacetGroups();
                        }, true);
                    }

                    _createClass(FacetsController, [{
                        key: 'update',
                        value: function update() {
                            var routeBuilder = this.searchService.getRouteBuilder();
                            this.location.search(routeBuilder.getRoute());
                            this.searchService.query();
                        }
                    }, {
                        key: 'clearGroup',
                        value: function clearGroup(groupId) {
                            var facets = this.searchService.getFacetGroup(groupId).getSelectedFacets();
                            _lodash2['default'].each(facets, function (facet) {
                                facet.selected = false;
                            });
                            this.update();
                        }
                    }]);

                    return FacetsController;
                })();

                exports.FacetsController = FacetsController;

                FacetsController.$inject = ['$scope', '$location', 'searchService'];

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, {}], 4: [function (require, module, exports) {
            (function (global) {
                'use strict';

                Object.defineProperty(exports, '__esModule', {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var MetaController = (function () {
                    function MetaController($scope, $location, searchService) {
                        var _this = this;

                        _classCallCheck(this, MetaController);

                        this.searchService = searchService;
                        this.location = $location;
                        this._initializeData();

                        $scope.$watchCollection(function () {
                            return searchService.getResults();
                        }, function () {
                            _this._initializeData();

                        });
                    }

                    _createClass(MetaController, [{
                        key: 'update',
                        value: function update() {
                            var routerBuilder = this.searchService.getRouteBuilder();
                            this.location.search(routerBuilder.getRoute());
                            this.searchService.query();
                        }
                    }, {
                        key: 'deselectFacet',
                        value: function deselectFacet(facetId) {
                            this.searchService.getFacet(facetId).selected = false;
                            this.update();
                        }
                    }, {
                        key: '_initializeData',
                        value: function _initializeData() {
                            var pager = this.searchService.getPager();
                            this.currentResults = pager.currentResults();
                            this.totalResults = pager.totalResults;
                            this.keywords = this.searchService.getFilter('q').getValue();
                            this.selectedFacetGroups = this._getActiveFacetGroups();
                           
                        }
                    }, {
                        key: '_getActiveFacetGroups',
                        value: function _getActiveFacetGroups() {
                            var groupsWithActive = [];
                            _lodash2['default'].each(this.searchService.getFacetGroups(), function (group) {
                                if (group.getSelectedFacets().length) {
                                    groupsWithActive.push(group);
                                }
                            });

                            return groupsWithActive;
                        }
                    }
                    ]);

                    return MetaController;
                })();

                exports.MetaController = MetaController;

                MetaController.$inject = ['$scope', '$location', 'searchService'];

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, {}], 5: [function (require, module, exports) {
            (function (global) {
                'use strict';

                Object.defineProperty(exports, '__esModule', {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var PaginationController = (function () {
                    function PaginationController($scope, $location, $anchorScroll, searchService) {
                        var _this = this;

                        _classCallCheck(this, PaginationController);

                        this.searchService = searchService;
                        this.location = $location;
                        this.anchorScroll = $anchorScroll;

                        this.pager = this.searchService.getPager();
                        this.initializeData();

                        $scope.$watch(function () {
                            return _this.searchService.getPager();
                        }, function () {
                            _this.pager = _this.searchService.getPager();
                            _this.initializeData();
                        });
                    }

                    _createClass(PaginationController, [{
                        key: 'initializeData',
                        value: function initializeData() {
                            this.showPagination = this.searchService.getResults().length > 0;
                            this.pageBlock = this.pager.pageData();
                            this.pageLast = this.pager.getLast();
                            this.pageFirst = this.pager.getFirst();
                        }
                    }, {
                        key: 'goto',
                        value: function goto(page) {
                            if (page) {
                                this.searchService.getFilter('page').setValue(page);
                                var routeBuilder = this.searchService.getRouteBuilder();
                                this.location.search(routeBuilder.getRoute());
                                this.searchService.query();

                                //Scroll to the top of the results when a new page is chosen
                                this.location.hash("searchTop");
                                this.anchorScroll();
                            }
                        }
                    }, {
                        key: 'next',
                        value: function next() {
                            this.goto(this.pager.nextPage());
                        }
                    }, {
                        key: 'prev',
                        value: function prev() {
                            this.goto(this.pager.prevPage());
                        }
                    }, {
                        key: 'isLastPageInBlock',
                        value: function isLastPageInBlock() {
                            return _lodash2['default'].find(this.pageBlock, { num: this.pageLast }) ? true : false;
                        }
                    }, {
                        key: 'isFirstPageInBlock',
                        value: function isFirstPageInBlock() {
                            return _lodash2['default'].find(this.pageBlock, { num: this.pageFirst }) ? true : false;
                        }
                    }]);

                    return PaginationController;
                })();

                exports.PaginationController = PaginationController;

                PaginationController.$inject = ['$scope', '$location', '$anchorScroll', 'searchService'];

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, {}], 6: [function (require, module, exports) {
            'use strict';

            Object.defineProperty(exports, '__esModule', {
                value: true
            });

            var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

            var QueryController = (function () {
                function QueryController($location, searchService, uiConfig) {
                    _classCallCheck(this, QueryController);

                    this.service = searchService;
                    this.location = $location;
                    this.keywords = searchService.getFilter('q').getValue();
                    this.typingTimeout = uiConfig.typingTimeout;
                }

                _createClass(QueryController, [{
                    key: 'update',
                    value: function update() {
                        this.service._isNewSearch = true;
                        this.service.getFilter('page').setValue("1");
                        this.service.getFilter('q').setValue(this.keywords);

                        var routerBuilder = this.service.getRouteBuilder();
                        this.location.search("q=" + this.keywords + "&page=1&sortBy=date&sortOrder=desc");

                        

                        this.service.query();
                        //this.service._isNewSearch = false;

                    }
                }]);

                return QueryController;
            })();

            exports.QueryController = QueryController;

            QueryController.$inject = ['$location', 'searchService', 'uiConfig'];

        }, {}], 7: [function (require, module, exports) {
            'use strict';

            Object.defineProperty(exports, '__esModule', {
                value: true
            });

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

            var ResultsController = function ResultsController($scope, searchService) {
                var _this = this;

                _classCallCheck(this, ResultsController);

                this.service = searchService;
                this.docs = [];

                $scope.$watchCollection(function () {
                    return searchService.getResults();
                }, function () {
                    _this.docs = searchService.getResults();
                });
            };

            exports.ResultsController = ResultsController;

            ResultsController.$inject = ['$scope', 'searchService'];

        }, {}], 8: [function (require, module, exports) {
            'use strict';

            Object.defineProperty(exports, '__esModule', {
                value: true
            });

            var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

            var _Core = require('../../Core');

            var SortController = (function () {
                function SortController($scope, $location, searchService) {
                    var _this = this;

                    _classCallCheck(this, SortController);

                    this.searchService = searchService;
                    this.location = $location;
                    this.sortBy = this.searchService.getSortByValue();
                    this.sortOrder = this.searchService.getCurrentSortOrder();

                    $scope.$watch(function () {
                        return _this.searchService.getSortByValue();
                    }, function () {
                        _this.sortBy = _this.searchService.getSortByValue();
                    });

                    $scope.$watch(function () {
                        return _this.searchService.getCurrentSortOrder();
                    }, function () {
                        _this.sortOrder = _this.searchService.getCurrentSortOrder();
                    });
                }

                _createClass(SortController, [{
                    key: 'update',
                    value: function update() {
                        var routeBuilder = this.searchService.getRouteBuilder();
                        this.location.search(routeBuilder.getRoute());
                        this.searchService.query();
                    }
                }, {
                    key: 'setBy',
                    value: function setBy(by) {
                        this.searchService.setSort(new _Core.SortBy({ id: 'sortBy', value: by }));
                        this.update();
                    }
                }, {
                    key: 'setByOrFlipOrder',
                    value: function setByOrFlipOrder(by) {
                        if (by === this.sortBy) {
                            this.searchService.flipSortOrder();
                        } else {
                            this.searchService.setSort(new _Core.SortBy({ id: 'sortBy', value: by }));
                        }

                        this.update();
                    }
                }, {
                    key: 'isSortedAsc',
                    value: function isSortedAsc(by) {
                        return by === this.sortBy && this.sortOrder === 'asc';
                    }
                }, {
                    key: 'isSortedDesc',
                    value: function isSortedDesc(by) {
                        return by === this.sortBy && this.sortOrder === 'desc';
                    }
                }]);

                return SortController;
            })();

            exports.SortController = SortController;

            SortController.$inject = ['$scope', '$location', 'searchService'];

        }, { "../../Core": 11 }], 9: [function (require, module, exports) {
            (function (global) {
                "use strict";

                Object.defineProperty(exports, "__esModule", {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var _Core = require("../Core");

                var SearchBootstrapper = (function () {
                    function SearchBootstrapper($location, searchService) {
                        _classCallCheck(this, SearchBootstrapper);

                        this.searchService = searchService;
                        this.location = $location;
                    }

                    _createClass(SearchBootstrapper, [{
                        key: "bootstrap",
                        value: function bootstrap() {
                            var _this = this;

                            var hashParams = this.location.search();
                            var filterParams = ["q", "page"];
                            var sortBy;
                            var sortOrder;
                            _lodash2["default"].each(hashParams, function (value, key) {
                                if (key === "sortBy") {
                                    sortBy = value;
                                    return;
                                }

                                if (key === "sortOrder") {
                                    sortOrder = value;
                                    return;
                                }

                                if (filterParams.indexOf(key) !== -1) {
                                    _this.createFilter(key, value);
                                    return;
                                }

                                _this.createFacet(key, value);
                            });

                            if (!sortBy) {
                                sortBy = "date";
                            }

                            var order;
                            if (sortOrder) {
                                order = new _Core.SingleSortOrder();
                                order.current = sortOrder;
                            }

                            var by = new _Core.SortBy({ id: "sortBy", value: sortBy });
                            this.searchService.setSort(by, order);

                            this.searchService.query();
                        }
                    }, {
                        key: "createFilter",
                        value: function createFilter(key, value) {
                            this.searchService.addFilter(new _Core.ValueFilter({ id: key, value: value }));
                        }
                    }, {
                        key: "createFacet",
                        value: function createFacet(key, value) {
                            var group = new _Core.FacetGroup({ id: key });
                            var items = [];
                            try {
                                items = value.split(";");
                                _lodash2["default"].each(items, function (facetValue) {
                                    var facet = new _Core.Facet({ id: facetValue, selected: true });
                                    group.addFacet(facet);
                                });
                            } catch (ex) {
                                console.log(ex);
                            }
                            
                            this.searchService.addFacetGroup(group);
                        }
                    }]);

                    return SearchBootstrapper;
                })();

                exports.SearchBootstrapper = SearchBootstrapper;

                SearchBootstrapper.$inject = ["$location", "searchService"];

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, { "../Core": 11 }], 10: [function (require, module, exports) {
            (function (global) {
                'use strict';

                Object.defineProperty(exports, '__esModule', {
                    value: true
                });

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

                var _angular = (typeof window !== "undefined" ? window['angular'] : typeof global !== "undefined" ? global['angular'] : null);

                var _angular2 = _interopRequireDefault(_angular);

                var _BaseUIConfig = require('./BaseUIConfig');

                var _Core = require('../Core');

                var _ControllersQueryController = require('./Controllers/QueryController');

                var _ControllersResultsController = require('./Controllers/ResultsController');

                var _ControllersFacetsController = require('./Controllers/FacetsController');

                var _ControllersMetaController = require('./Controllers/MetaController');

                var _ControllersPaginationController = require('./Controllers/PaginationController');

                var _ControllersSortController = require('./Controllers/SortController');

                var _SearchBootstrapper = require('./SearchBootstrapper');

                // Set up the default filters
                var defaultFilters = [new _Core.ValueFilter({ id: 'q' }), new _Core.ValueFilter({ id: 'page' })];

                // Register an angular module
                var SearchModule = _angular2['default'].module('velir.search', []);

                // Register UI configuration as constant
                SearchModule.value('uiConfig', _BaseUIConfig.BaseUiConfig);

                // Register the search service provider
                SearchModule.provider('searchService', _Core.SearchServiceProvider);

                // Register the search bootstrapper
                SearchModule.service('searchBootstrapper', _SearchBootstrapper.SearchBootstrapper);

                // Register controllers
                SearchModule.controller('searchQueryController', _ControllersQueryController.QueryController);
                SearchModule.controller('searchResultsController', _ControllersResultsController.ResultsController);
                SearchModule.controller('searchFacetsController', _ControllersFacetsController.FacetsController);
                SearchModule.controller('searchMetaController', _ControllersMetaController.MetaController);
                SearchModule.controller('searchPaginationController', _ControllersPaginationController.PaginationController);
                SearchModule.controller('searchSortController', _ControllersSortController.SortController);

                SearchModule.controller("InformaFacetsController", function ($scope, $controller, $location, $http, searchService, searchBootstrapper) {
                    $controller('searchFacetsController', { $scope: $scope, $location: $location, $http: $http, searchService: searchService, searchBootstrapper: searchBootstrapper }); //This works
                });

                // Configure the search service
                SearchModule.config(function (searchServiceProvider) {
                    searchServiceProvider.setFilters(defaultFilters);
                });

                SearchModule.run(function (searchBootstrapper) {
                    searchBootstrapper.bootstrap();
                });

                exports.SearchModule = SearchModule;

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, { "../Core": 11, "./BaseUIConfig": 2, "./Controllers/FacetsController": 3, "./Controllers/MetaController": 4, "./Controllers/PaginationController": 5, "./Controllers/QueryController": 6, "./Controllers/ResultsController": 7, "./Controllers/SortController": 8, "./SearchBootstrapper": 9 }], 11: [function (require, module, exports) {
            "use strict";

            Object.defineProperty(exports, "__esModule", {
                value: true
            });

            var _CoreSearchService = require("./Core/SearchService");

            var _CoreQuery = require("./Core/Query");

            var _CoreParam = require("./Core/Param");

            var _CoreCollectionsParamableCollection = require("./Core/Collections/ParamableCollection");

            var _CoreErrors = require("./Core/Errors");

            var _CoreFacetsFacet = require("./Core/Facets/Facet");

            var _CoreFacetsFacetGroup = require("./Core/Facets/FacetGroup");

            var _CoreHttpTransportJqueryTransport = require("./Core/HttpTransport/JqueryTransport");

            var _CoreFiltersBaseFilter = require("./Core/Filters/BaseFilter");

            var _CoreFiltersValueFilter = require("./Core/Filters/ValueFilter");

            var _CoreSortSortBy = require("./Core/Sort/SortBy");

            var _CoreSortSingleSortOrder = require("./Core/Sort/SingleSortOrder");

            exports.SearchServiceProvider = _CoreSearchService.SearchServiceProvider;
            exports.Query = _CoreQuery.Query;
            exports.Param = _CoreParam.Param;
            exports.NotImplementedError = _CoreErrors.NotImplementedError;
            exports.ParamableCollection = _CoreCollectionsParamableCollection.ParamableCollection;
            exports.Facet = _CoreFacetsFacet.Facet;
            exports.FacetGroup = _CoreFacetsFacetGroup.FacetGroup;
            exports.JqueryTransport = _CoreHttpTransportJqueryTransport.JqueryTransport;
            exports.BaseFilter = _CoreFiltersBaseFilter.BaseFilter;
            exports.ValueFilter = _CoreFiltersValueFilter.ValueFilter;
            exports.SortBy = _CoreSortSortBy.SortBy;
            exports.SingleSortOrder = _CoreSortSingleSortOrder.SingleSortOrder;

        }, { "./Core/Collections/ParamableCollection": 12, "./Core/Errors": 13, "./Core/Facets/Facet": 14, "./Core/Facets/FacetGroup": 15, "./Core/Filters/BaseFilter": 16, "./Core/Filters/ValueFilter": 17, "./Core/HttpTransport/JqueryTransport": 19, "./Core/Param": 21, "./Core/Query": 22, "./Core/SearchService": 24, "./Core/Sort/SingleSortOrder": 25, "./Core/Sort/SortBy": 26 }], 12: [function (require, module, exports) {
            (function (global) {
                'use strict';

                Object.defineProperty(exports, '__esModule', {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var ParamableCollection = (function () {
                    function ParamableCollection() {
                        var params = arguments.length <= 0 || arguments[0] === undefined ? [] : arguments[0];

                        _classCallCheck(this, ParamableCollection);

                        this._params = params;
                    }

                    _createClass(ParamableCollection, [{
                        key: 'count',
                        value: function count() {
                            return this._params.length;
                        }
                    }, {
                        key: 'find',
                        value: function find(id) {
                            return _lodash2['default'].find(this._params, { id: id });
                        }
                    }, {
                        key: 'filterByType',
                        value: function filterByType(type) {
                            return _lodash2['default'].where(this._params, { type: type });
                        }
                    }, {
                        key: 'add',
                        value: function add(paramable) {
                            var match = this.find(paramable.id);
                            if (match) {
                                var index = _lodash2['default'].indexOf(this._params, match);
                                this._params.splice(index, 1, paramable);
                            } else {
                                this._params.push(paramable);
                            }
                        }
                    }, {
                        key: 'addParamableCollection',
                        value: function addParamableCollection(pc) {
                            var _this = this;

                            _lodash2['default'].each(pc, function (p) {
                                return _this.add(p);
                            });
                        }
                    }, {
                        key: 'remove',
                        value: function remove(id) {
                            _lodash2['default'].remove(this._params, { id: id });
                        }
                    }, {
                        key: 'toHash',
                        value: function toHash() {
                            var hash = {};
                            _lodash2['default'].each(this._params, function (p) {
                                var param = p.toParam();
                                hash[param.id] = param.value;
                            });

                            return hash;
                        }
                    }, {
                        key: 'toArray',
                        value: function toArray() {
                            return this._params;
                        }
                    }, {
                        key: 'toParamArray',
                        value: function toParamArray() {
                            return _lodash2['default'].map(this._params, function (p) {
                                return p.toParam();
                            });
                        }
                    }]);

                    return ParamableCollection;
                })();

                exports.ParamableCollection = ParamableCollection;

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, {}], 13: [function (require, module, exports) {
            "use strict";

            Object.defineProperty(exports, "__esModule", {
                value: true
            });

            var _get = function get(_x, _x2, _x3) { var _again = true; _function: while (_again) { var object = _x, property = _x2, receiver = _x3; desc = parent = getter = undefined; _again = false; if (object === null) object = Function.prototype; var desc = Object.getOwnPropertyDescriptor(object, property); if (desc === undefined) { var parent = Object.getPrototypeOf(object); if (parent === null) { return undefined; } else { _x = parent; _x2 = property; _x3 = receiver; _again = true; continue _function; } } else if ("value" in desc) { return desc.value; } else { var getter = desc.get; if (getter === undefined) { return undefined; } return getter.call(receiver); } } };

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

            function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) subClass.__proto__ = superClass; }

            var NotImplementedError = (function (_Error) {
                _inherits(NotImplementedError, _Error);

                function NotImplementedError() {
                    _classCallCheck(this, NotImplementedError);

                    _get(Object.getPrototypeOf(NotImplementedError.prototype), "constructor", this).call(this);
                    this.name = "Not Implemented Error";
                    this.message = "This method has been has not been implemented. It may have been inherited from a base class which has no implementation.";
                }

                return NotImplementedError;
            })(Error);

            exports.NotImplementedError = NotImplementedError;

        }, {}], 14: [function (require, module, exports) {
            "use strict";

            Object.defineProperty(exports, "__esModule", {
                value: true
            });

            var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

            var _Param = require("../Param");

            var Facet = (function () {
                function Facet(_ref) {
                    var id = _ref.id;
                    var label = _ref.label;
                    var _ref$parentId = _ref.parentId;
                    var parentId = _ref$parentId === undefined ? null : _ref$parentId;
                    var _ref$count = _ref.count;
                    var count = _ref$count === undefined ? 0 : _ref$count;
                    var _ref$selected = _ref.selected;
                    var selected = _ref$selected === undefined ? false : _ref$selected;
                    var _ref$sublist = _ref.sublist;
                    var sublist = _ref$sublist === undefined ? [] : _ref$sublist;

                    _classCallCheck(this, Facet);

                    this.type = "Facet";
                    this.id = id;
                    this.parentId = parentId;
                    this.label = label;
                    this.selected = selected;
                    this.count = count;
                    this.sublist = sublist;
                }

                _createClass(Facet, null, [{
                    key: "buildFromJson",
                    value: function buildFromJson(rawFacet) {
                        var parentId = arguments.length <= 1 || arguments[1] === undefined ? null : arguments[1];

                        return new Facet({
                            id: rawFacet.id,
                            parentId: parentId,
                            label: rawFacet.name,
                            selected: rawFacet.selected,
                            count: rawFacet.count,
                            sublist: rawFacet.sublist
                        });
                    }
                }]);

                return Facet;
            })();

            exports.Facet = Facet;

        }, { "../Param": 21 }], 15: [function (require, module, exports) {
            (function (global) {
                "use strict";

                Object.defineProperty(exports, "__esModule", {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var _Param = require("../Param");

                var _Facet = require("./Facet");

                var FacetGroup = (function () {
                    function FacetGroup(_ref) {
                        var id = _ref.id;
                        var label = _ref.label;
                        var _ref$facets = _ref.facets;
                        var facets = _ref$facets === undefined ? [] : _ref$facets;
                        var _ref$subgroups = _ref.subgroups;
                        var subgroups = _ref$subgroups === undefined ? [] : _ref$subgroups;

                        _classCallCheck(this, FacetGroup);

                        this.type = "FacetGroup";
                        this.id = id;
                        this.label = label;
                        this.facets = facets;
                        this.subgroups = subgroups;
                    }

                    _createClass(FacetGroup, [{
                        key: "addGroup",
                        value: function addGroup(facetGroup) {
                            FacetGroup._addOrReplaceById(this.subgroups, facetGroup);
                        }
                    }, {
                        key: "getGroup",
                        value: function getGroup(id) {
                            var deep = arguments.length <= 1 || arguments[1] === undefined ? false : arguments[1];

                            var group = _lodash2["default"].find(this.subgroups, { id: id });

                            if (!group && deep) {
                                _lodash2["default"].each(this.subgroups, function (g) {
                                    group = g.getGroup(id, true);
                                });
                            }

                            return group;
                        }
                    }, {
                        key: "removeGroup",
                        value: function removeGroup(id) {
                            var deep = arguments.length <= 1 || arguments[1] === undefined ? false : arguments[1];

                            var group = this.getGroup(id);

                            if (!group && deep) {
                                _lodash2["default"].each(this.subgroups, function (g) {
                                    g.removeGroup(id, true);
                                });
                            } else if (group) {
                                _lodash2["default"].remove(this.subgroups, { id: id });
                            }
                        }
                    }, {
                        key: "addFacet",
                        value: function addFacet(facet) {
                            FacetGroup._addOrReplaceById(this.facets, facet);
                        }
                    }, {
                        key: "getFacet",
                        value: function getFacet(id) {
                            var deep = arguments.length <= 1 || arguments[1] === undefined ? false : arguments[1];

                            var upperId = id.toUpperCase();
                            var facet = _lodash2["default"].find(this.facets, function (o) { return o.id.toUpperCase() === upperId; });

                            if (!facet) {
                                _lodash2["default"].each(this.facets, function (f) {
                                    facet = _lodash2["default"].find(f.sublist, function (o) { return o.id.toUpperCase() === upperId; });
                                    return !facet;
                                });
                            }

                            if (!facet && deep) {
                                _lodash2["default"].some(this.subgroups, function (g) {
                                    facet = g.getFacet(id, true);
                                    return facet !== undefined;
                                });
                            }

                            return facet;
                        }
                    }, {
                        key: "getSelectedFacets",
                        value: function getSelectedFacets() {
                            var deep = arguments.length <= 0 || arguments[0] === undefined ? false : arguments[0];

                            var selectedFacets = _lodash2["default"].where(this.facets, { selected: true });
                            _lodash2["default"].each(this.facets, function (facet) {
                                selectedFacets = selectedFacets.concat(_lodash2["default"].where(facet.sublist, { selected: true }));
                            });
                            if (deep) {
                                _lodash2["default"].each(this.subgroups, function (group) {
                                    selectedFacets = selectedFacets.concat(group.getSelectedFacets(true));
                                });
                            }

                            return selectedFacets;
                        }
                    }, {
                        key: "removeFacet",
                        value: function removeFacet(id) {
                            _lodash2["default"].remove(this.facets, { id: id });
                        }
                    }, {
                        key: "selectFacet",
                        value: function selectFacet(id) {
                            var deep = arguments.length <= 1 || arguments[1] === undefined ? false : arguments[1];

                            var facet = this.getFacet(id, deep);
                            if (facet) {
                                facet.selected = true;
                            }
                        }
                    }, {
                        key: "deselectFacet",
                        value: function deselectFacet(id) {
                            var deep = arguments.length <= 1 || arguments[1] === undefined ? false : arguments[1];

                            var facet = this.getFacet(id, deep);
                            if (facet) {
                                facet.selected = false;
                            }
                        }
                    }, {
                        key: "toggleFacet",
                        value: function toggleFacet(id, deep) {
                            var facet = this.getFacet(id, deep);
                            if (facet) {
                                facet.selected = !facet.selected;
                            }
                        }
                    }, {
                        key: "toParam",
                        value: function toParam() {
                            return new _Param.Param({ id: this.id, value: this._facetValues() });
                        }
                    }, {
                        key: "_facetValues",
                        value: function _facetValues() {
                            var deep = arguments.length <= 0 || arguments[0] === undefined ? true : arguments[0];

                            return _lodash2["default"].map(this.getSelectedFacets(deep), function (f) {
                                return f.id;
                            }).join(";");
                        }
                    }], [{
                        key: "buildFromJson",
                        value: function buildFromJson(rawGroup) {
                            var id = rawGroup.id;
                            var label = rawGroup.label;
                            var facets = [];
                            var subgroups = [];
                            _lodash2["default"].each(rawGroup.values, function (value) {
                                // check if nested group or raw facet
                                // if 'values' is either not a property or a null value, it is a facet
                                if (value.hasOwnProperty("values") && value.values !== null) {
                                    subgroups.push(FacetGroup.buildFromJson(value));
                                } else {
                                    facets.push(_Facet.Facet.buildFromJson(value, id));
                                }
                            });

                            return new FacetGroup({
                                id: id,
                                label: label,
                                facets: facets,
                                subgroups: subgroups
                            });
                        }
                    }, {
                        key: "_addOrReplaceById",
                        value: function _addOrReplaceById(list, thing) {
                            var match = _lodash2["default"].find(list, { id: thing.id });
                            if (match) {
                                var index = _lodash2["default"].indexOf(list, match);
                                list.splice(index, 1, thing);
                            } else {
                                list.push(thing);
                            }
                        }
                    }]);

                    return FacetGroup;
                })();

                exports.FacetGroup = FacetGroup;

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, { "../Param": 21, "./Facet": 14 }], 16: [function (require, module, exports) {
            "use strict";

            Object.defineProperty(exports, "__esModule", {
                value: true
            });

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

            var BaseFilter = function BaseFilter() {
                _classCallCheck(this, BaseFilter);

                this.type = "Filter";
            };

            exports.BaseFilter = BaseFilter;

        }, {}], 17: [function (require, module, exports) {
            'use strict';

            Object.defineProperty(exports, '__esModule', {
                value: true
            });

            var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

            var _get = function get(_x, _x2, _x3) { var _again = true; _function: while (_again) { var object = _x, property = _x2, receiver = _x3; desc = parent = getter = undefined; _again = false; if (object === null) object = Function.prototype; var desc = Object.getOwnPropertyDescriptor(object, property); if (desc === undefined) { var parent = Object.getPrototypeOf(object); if (parent === null) { return undefined; } else { _x = parent; _x2 = property; _x3 = receiver; _again = true; continue _function; } } else if ('value' in desc) { return desc.value; } else { var getter = desc.get; if (getter === undefined) { return undefined; } return getter.call(receiver); } } };

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

            function _inherits(subClass, superClass) { if (typeof superClass !== 'function' && superClass !== null) { throw new TypeError('Super expression must either be null or a function, not ' + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) subClass.__proto__ = superClass; }

            var _BaseFilter2 = require('./BaseFilter');

            var _Param = require('../Param');

            var ValueFilter = (function (_BaseFilter) {
                _inherits(ValueFilter, _BaseFilter);

                function ValueFilter(_ref) {
                    var id = _ref.id;
                    var value = _ref.value;

                    _classCallCheck(this, ValueFilter);

                    _get(Object.getPrototypeOf(ValueFilter.prototype), 'constructor', this).call(this);
                    this.id = id;
                    this._value = value;
                }

                _createClass(ValueFilter, [{
                    key: 'getValue',
                    value: function getValue() {
                        return this._value;
                    }
                }, {
                    key: 'setValue',
                    value: function setValue(value) {
                        this._value = value;
                    }
                }, {
                    key: 'toParam',
                    value: function toParam() {
                        return new _Param.Param({ id: this.id, value: this.getValue() });
                    }
                }]);

                return ValueFilter;
            })(_BaseFilter2.BaseFilter);

            exports.ValueFilter = ValueFilter;

        }, { "../Param": 21, "./BaseFilter": 16 }], 18: [function (require, module, exports) {
            'use strict';

            Object.defineProperty(exports, '__esModule', {
                value: true
            });

            var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

            var AngularTransport = (function () {
                function AngularTransport(transport) {
                    _classCallCheck(this, AngularTransport);

                    this._transport = transport;
                    this._method = 'GET';
                }

                _createClass(AngularTransport, [{
                    key: 'fetch',

                    // TODO - output something Promises/A compatible
                    value: function fetch(url, data) {
                        return this._transport({
                            url: url,
                            method: this._method,
                            params: data
                        });
                    }
                }, {
                    key: 'setMethod',
                    value: function setMethod(method) {
                        this._method = method;
                    }
                }]);

                return AngularTransport;
            })();

            exports.AngularTransport = AngularTransport;

        }, {}], 19: [function (require, module, exports) {
            'use strict';

            Object.defineProperty(exports, '__esModule', {
                value: true
            });

            var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

            var JqueryTransport = (function () {
                function JqueryTransport(transport) {
                    _classCallCheck(this, JqueryTransport);

                    this._transport = transport;
                    this._method = 'GET';
                }

                _createClass(JqueryTransport, [{
                    key: 'fetch',

                    // TODO - output something Promises/A compatible
                    value: function fetch(url, data) {
                        return this._transport({
                            url: url,
                            type: this._method,
                            dataType: 'json',
                            data: data
                        });
                    }
                }, {
                    key: 'setMethod',
                    value: function setMethod(method) {
                        this._method = method;
                    }
                }]);

                return JqueryTransport;
            })();

            exports.JqueryTransport = JqueryTransport;

        }, {}], 20: [function (require, module, exports) {
            "use strict";

            Object.defineProperty(exports, "__esModule", {
                value: true
            });

            var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

            var Pager = (function () {
                function Pager(_ref) {
                    var _ref$pageBlockRange = _ref.pageBlockRange;
                    var pageBlockRange = _ref$pageBlockRange === undefined ? 2 : _ref$pageBlockRange;
                    var _ref$totalResults = _ref.totalResults;
                    var totalResults = _ref$totalResults === undefined ? 0 : _ref$totalResults;
                    var _ref$current = _ref.current;
                    var current = _ref$current === undefined ? 1 : _ref$current;
                    var _ref$perPage = _ref.perPage;
                    var perPage = _ref$perPage === undefined ? 10 : _ref$perPage;

                    _classCallCheck(this, Pager);

                    // number of page to show before and after current page
                    // if at first or last page, double the number will be shown
                    // either after or before
                    this.pageBlockRange = pageBlockRange;
                    this.totalResults = parseInt(totalResults, 10);
                    this.current = parseInt(current, 10);
                    this.perPage = parseInt(perPage, 10);
                }

                _createClass(Pager, [{
                    key: "currentPage",
                    value: function currentPage() {
                        return this.current;
                    }
                }, {
                    key: "nextPage",
                    value: function nextPage() {
                        return this.atLast() ? this.current : this.current + 1;
                    }
                }, {
                    key: "prevPage",
                    value: function prevPage() {
                        return this.atFirst() ? this.current : this.current - 1;
                    }
                }, {
                    key: "validPage",
                    value: function validPage(num) {
                        return num >= this.getFirst() && num <= this.getLast();
                    }
                }, {
                    key: "currentResults",
                    value: function currentResults() {
                        var start = this.current * this.perPage - this.perPage + 1;
                        var end = start + this.perPage - 1;

                        if (end > this.totalResults) {
                            end = this.totalResults;
                        }
						if (end == 0) {
                             start = 0;
                         }

                        if (end == 0) {
                            start = 0;
                        }

                        return "" + start + " - " + end;
                    }
                }, {
                    key: "totalPages",
                    value: function totalPages() {
                        return Math.ceil(this.totalResults / this.perPage);
                    }
                }, {
                    key: "getFirst",
                    value: function getFirst() {
                        return 1;
                    }
                }, {
                    key: "getLast",
                    value: function getLast() {
                        return this.totalPages();
                    }
                }, {
                    key: "atLast",
                    value: function atLast() {
                        return this.current === this.totalPages();
                    }
                }, {
                    key: "atFirst",
                    value: function atFirst() {
                        return this.current === 1;
                    }
                }, {
                    key: "pageData",
                    value: function pageData() {
                        var block = [{ num: this.current, label: this.current, current: true }];

                        // keep track of how many pages we're add
                        // to the left or right of the current page
                        var nextCount = this.current;
                        var prevCount = this.current;
                        for (var i = 0; i < this.pageBlockRange; i++) {
                            var num;

                            // add page to the left and increment prevCount
                            // if page is less than first page
                            // add it to the right, and increment that counter
                            num = prevCount - 1;
                            if (num < this.getFirst()) {
                                num = nextCount + 1;
                                nextCount++;
                            } else {
                                prevCount--;
                            }

                            if (this.validPage(num)) {
                                block.push({ num: num, label: num, current: false });
                            }

                            // add page to the right and increment nextCount
                            // if page is more than the last page
                            // add it to the left, and increment that counter
                            num = nextCount + 1;
                            if (num > this.getLast()) {
                                num = prevCount - 1;
                                prevCount--;
                            } else {
                                nextCount++;
                            }

                            if (this.validPage(num)) {
                                block.push({ num: num, label: num, current: false });
                            }
                        }

                        return block.sort(function (a, b) {
                            return a.num - b.num;
                        });
                    }
                }]);

                return Pager;
            })();

            exports.Pager = Pager;

        }, {}], 21: [function (require, module, exports) {
            "use strict";

            Object.defineProperty(exports, "__esModule", {
                value: true
            });

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

            var Param = function Param(_ref) {
                var id = _ref.id;
                var value = _ref.value;

                _classCallCheck(this, Param);

                this.id = id;
                this.value = value;
            };

            exports.Param = Param;

        }, {}], 22: [function (require, module, exports) {
            (function (global) {
                "use strict";

                Object.defineProperty(exports, "__esModule", {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var Query = (function () {
                    function Query(_ref) {
                        var url = _ref.url;
                        var transport = _ref.transport;
                        var _ref$data = _ref.data;
                        var data = _ref$data === undefined ? {} : _ref$data;

                        _classCallCheck(this, Query);

                        this._url = url;
                        this._transport = transport;
                        this._data = data;
                        this.beforeTransform = function (data) {
                            return data;
                        };
                        this.afterTransform = function (data) {
                            return data;
                        };
                    }

                    _createClass(Query, [{
                        key: "exec",
                        value: function exec() {
                            var _this = this;

                            var data = this.beforeTransform.call(this, this._data);
                            return this._transport.fetch(this._url, data).then(function (data, status, xhr) {
                                var response = _this.afterTransform.call(_this, data);
                                return data;
                            });
                        }
                    }]);

                    return Query;
                })();

                exports.Query = Query;

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, {}], 23: [function (require, module, exports) {
            (function (global) {
                "use strict";

                Object.defineProperty(exports, "__esModule", {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var RouteBuilder = (function () {
                    function RouteBuilder(collection) {
                        _classCallCheck(this, RouteBuilder);

                        this.collection = collection;
                        this.segmentDelimiter = "&";
                        this.assignmentDelimiter = "=";
                        this.segmentOrder = [];
                        this.paramMap = {};
                    }

                    _createClass(RouteBuilder, [{
                        key: "getRoute",
                        value: function getRoute() {
                            var _this = this;

                            var routeString = "";
                            var paramArr = this._order(this.collection.toParamArray());
                            _lodash2["default"].each(paramArr, function (p) {
                                // does p have a value that is not null, undefined or emptry string?
                                // if it is, don't include it in the route
                                if (p.value === null || p.value === undefined || p.value === "") {
                                    return;
                                }

                                // check if we've mapped the param id to a different key for the route
                                var segmentId = _this.paramMap[p.id] ? _this.paramMap[p.id] : p.id;
                                var segmentValue = p.value;

                                var maybeNewSegment = _lodash2["default"].indexOf(paramArr, p) === 0 ? "" : _this.segmentDelimiter;
                                routeString = routeString + maybeNewSegment + encodeURIComponent(segmentId) + _this.assignmentDelimiter + encodeURIComponent(segmentValue);
                            });

                            return routeString;
                        }
                    }, {
                        key: "_order",
                        value: function _order(paramArr) {
                            // if no segment order specified, just return original array
                            if (!this.segmentOrder.length) {
                                return paramArr;
                            }

                            // create an object where ids are keys
                            var groups = _lodash2["default"].groupBy(paramArr, function (p) {
                                return p.id;
                            });

                            var ordered = _lodash2["default"].map(this.segmentOrder, function (seg) {
                                // if the id in the segmentOrder matches the id in the group
                                // add it to the map
                                if (groups[seg]) {
                                    return groups[seg][0];
                                }
                            });

                            // compact the ordered array, and add the original param array
                            // to it to include params that were not part the segmentOrder array
                            var concated = _lodash2["default"].compact(ordered).concat(paramArr);

                            // return only unique objects in the array
                            return _lodash2["default"].uniq(concated, false, function (i) {
                                return i.id;
                            });
                        }
                    }]);

                    return RouteBuilder;
                })();

                exports.RouteBuilder = RouteBuilder;

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, {}], 24: [function (require, module, exports) {
            (function (global) {
                'use strict';

                Object.defineProperty(exports, '__esModule', {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var _Query = require('./Query');

                var _CollectionsParamableCollection = require('./Collections/ParamableCollection');

                var _FacetsFacet = require('./Facets/Facet');

                var _FacetsFacetGroup = require('./Facets/FacetGroup');

                var _SortSortBy = require('./Sort/SortBy');

                var _SortSingleSortOrder = require('./Sort/SingleSortOrder');

                var _RouteBuilder = require('./RouteBuilder');

                var _Pager = require('./Pager');

                var _HttpTransportAngularTransport = require('./HttpTransport/AngularTransport');

                var SearchServiceProvider = (function () {
                    function SearchServiceProvider() {
                        _classCallCheck(this, SearchServiceProvider);

                        this._config = {
                            id: null,
                            url: null,
                            responseTransform: null,
                            queryTransform: null,
                            filters: []
                        };

                        this.$get = ['$http', function (http) {
                            var transport = new _HttpTransportAngularTransport.AngularTransport(http);
                            return this.getService(transport);
                        }];
                    }

                    _createClass(SearchServiceProvider, [{
                        key: 'getService',
                        value: function getService(http) {
                            return new SearchService(http, this._config);
                        }
                    }, {
                        key: 'getConfig',
                        value: function getConfig() {
                            return this._config;
                        }
                    }, {
                        key: 'setSearchId',
                        value: function setSearchId(id) {
                            this._config.id = id;
                            return this;
                        }
                    }, {
                        key: 'setUrl',
                        value: function setUrl(url) {
                            this._config.url = url;
                            return this;
                        }
                    }, {
                        key: 'setFilters',
                        value: function setFilters(filters) {
                            this._config.filters = filters;
                        }
                    }, {
                        key: 'setResponseTransform',
                        value: function setResponseTransform(transformer) {
                            this._config.responseTransform = transformer;
                            return this;
                        }
                    }, {
                        key: 'setQueryTransform',
                        value: function setQueryTransform(transformer) {
                            this._config.queryTransform = transformer;
                            return this;
                        }
                    }, {
                        key: 'setHttpTransport',
                        value: function setHttpTransport(transport) {
                            this._transport = transport;
                        }
                    }]);

                    return SearchServiceProvider;
                })();

                exports.SearchServiceProvider = SearchServiceProvider;

                var SearchService = (function () {
                    function SearchService(httpTransport, config) {
                        var _this = this;

                        _classCallCheck(this, SearchService);

                        this._http = httpTransport;
                        this._searchId = config.id;
                        this._url = config.url;
                        this._responseTransform = config.responseTransform;
                        this._queryTransform = config.queryTransform;
                        this._params = new _CollectionsParamableCollection.ParamableCollection();
                        this._routeBuilder = new _RouteBuilder.RouteBuilder(this._params);
                        this._pager = new _Pager.Pager({});
                        this._results = [];

                        this._isNewSearch = false;

                        _lodash2['default'].each(config.filters, function (filter) {
                            _this.addParamable(filter);
                        });
                    }

                    _createClass(SearchService, [
                        {
                            key: 'getNewSearch',
                            value: function getNewSearch() {
                                return this._isNewSearch;
                            }
                        }, {
                        key: 'getParams',
                        value: function getParams() {
                            return this._params;
                        }
                    }, {
                        key: 'getSearchId',
                        value: function getSearchId() {
                            return this._searchId;
                        }
                    }, {
                        key: 'getUrl',
                        value: function getUrl() {
                            return this._url;
                        }
                    }, {
                        key: 'getFilters',
                        value: function getFilters() {
                            return this._params.filterByType('Filter');
                        }
                    }, {
                        key: 'getFacetGroups',
                        value: function getFacetGroups() {
                            return this._params.filterByType('FacetGroup');
                        }
                    }, {
                        key: 'getFacetGroup',
                        value: function getFacetGroup(id) {
                            return _lodash2['default'].find(this._params.filterByType('FacetGroup'), { id: id });
                        }
                    }, {
                        key: 'getFacet',
                        value: function getFacet(id) {
                            var facet;
                            _lodash2['default'].some(this.getFacetGroups(), function (group) {
                                facet = group.getFacet(id, true);
                                return facet !== undefined;
                            });

                            return facet;
                        }
                    }, {
                        key: 'getSortByValue',
                        value: function getSortByValue() {
                            var sorters = this._params.filterByType('SortBy');
                            return sorters[0].value;
                        }
                    }, {
                        key: 'getCurrentSortOrder',
                        value: function getCurrentSortOrder() {
                            var sorters = this._params.filterByType('SortOrder');
                            return sorters[0].current;
                        }
                    }, {
                        key: 'getResults',
                        value: function getResults() {
                            return this._results;
                        }
                    }, {
                        key: 'getRouteBuilder',
                        value: function getRouteBuilder() {
                            return this._routeBuilder;
                        }
                    }, {
                        key: 'getPager',
                        value: function getPager() {
                            return this._pager;
                        }
                    }, {
                        key: 'addFacetGroup',
                        value: function addFacetGroup(group) {
                            this.addParamable(group);
                        }
                    }, {
                        key: 'selectFacet',
                        value: function selectFacet(facetId) {
                            _lodash2['default'].each(this.getFacetGroups(), function (group) {
                                group.selectFacet(facetId, true);
                            });
                        }
                    }, {
                        key: 'deselectFacet',
                        value: function deselectFacet(facetId) {
                            _lodash2['default'].each(this.getFacetGroups(), function (group) {
                                group.deselectFacet(facetId, true);
                            });
                        }
                    }, {
                        key: 'toggleFacet',
                        value: function toggleFacet(facetId) {
                            _lodash2['default'].each(this.getFacetGroups(), function (group) {
                                group.toggleFacet(facetId, true);
                            });
                        }
                    }, {
                        key: 'addFilter',
                        value: function addFilter(filter) {
                            this.addParamable(filter);
                        }
                    }, {
                        key: 'getFilter',
                        value: function getFilter(filterId) {
                            return _lodash2['default'].find(this.getFilters(), { id: filterId });
                        }
                    }, {
                        key: 'setSort',
                        value: function setSort(sorter) {
                            var order = arguments.length <= 1 || arguments[1] === undefined ? new _SortSingleSortOrder.SingleSortOrder() : arguments[1];

                            this.addParamable(sorter);

                            // implicitly create a SortOrder if none currently exists
                            if (!this._params.filterByType('SortOrder').length) {
                                this.addParamable(order);
                            }
                        }
                    }, {
                        key: 'flipSortOrder',
                        value: function flipSortOrder() {
                            var sorter = this._params.filterByType('SortOrder')[0];
                            if (sorter) {
                                sorter.flip();
                            }
                        }
                    }, {
                        key: 'addParamable',
                        value: function addParamable(paramable) {
                            this._params.add(paramable);
                        }
                    }, {
                        key: 'query',
                        value: function query() {
                            var _this2 = this;
                            
                            var query = new _Query.Query({
                                url: this._url,
                                transport: this._http,
                                data: this._params.toHash()
                            });

                            if (typeof this._queryTransform === 'function') {
                                query.beforeTransform = this._queryTransform;
                            }

                            if (typeof this._responseTransform === 'function') {
                                query.afterTransform = this._responseTransform;
                            }

                            return query.exec().then(function (data) {
                                if (data.hasOwnProperty('data')) {
                                    data = data.data;
                                }
                                _this2._processResults(data);
                            });
                        }
                    }, {
                        key: 'queryTimePeriod',
                        value: function queryTimePeriod(passedParams) {
                            var _this2 = this;

                            var query = new _Query.Query({
                                url: this._url,
                                transport: this._http,
                                data: passedParams
                            });



                            if (typeof this._queryTransform === 'function') {
                                query.beforeTransform = this._queryTransform;
                            }

                            if (typeof this._responseTransform === 'function') {
                                query.afterTransform = this._responseTransform;
                            }

                            return query.exec().then(function (data) {
                                if (data.hasOwnProperty('data')) {
                                    data = data.data;
                                }
                                _this2._processResults(data);
                            });
                        }
                    }, {
                        key: '_processResults',
                        value: function _processResults(data) {
                            var _this3 = this;

                            this._results = data.results;
                            this._pager = new _Pager.Pager({
                                current: data.request.page,
                                perPage: data.request.perPage,
                                totalResults: data.totalResults
                            });

                            _lodash2['default'].each(data.facets, function (rawGroup) {
                                _this3.addParamable(_FacetsFacetGroup.FacetGroup.buildFromJson(rawGroup));
                            });
                        }
                    }]);

                    return SearchService;
                })();

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, { "./Collections/ParamableCollection": 12, "./Facets/Facet": 14, "./Facets/FacetGroup": 15, "./HttpTransport/AngularTransport": 18, "./Pager": 20, "./Query": 22, "./RouteBuilder": 23, "./Sort/SingleSortOrder": 25, "./Sort/SortBy": 26 }], 25: [function (require, module, exports) {
            (function (global) {
                'use strict';

                Object.defineProperty(exports, '__esModule', {
                    value: true
                });

                var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

                function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

                function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

                var _lodash = (typeof window !== "undefined" ? window['_'] : typeof global !== "undefined" ? global['_'] : null);

                var _lodash2 = _interopRequireDefault(_lodash);

                var _Param = require('../Param');

                var SingleSortOrder = (function () {
                    function SingleSortOrder() {
                        var _ref = arguments.length <= 0 || arguments[0] === undefined ? {} : arguments[0];

                        var _ref$id = _ref.id;
                        var id = _ref$id === undefined ? 'sortOrder' : _ref$id;
                        var _ref$orders = _ref.orders;
                        var orders = _ref$orders === undefined ? ['asc', 'desc'] : _ref$orders;
                        var _ref$current = _ref.current;
                        var current = _ref$current === undefined ? 'desc' : _ref$current;

                        _classCallCheck(this, SingleSortOrder);

                        this.id = id;
                        this.type = 'SortOrder';
                        this.orders = orders;
                        this.current = current;
                    }

                    _createClass(SingleSortOrder, [{
                        key: 'flip',
                        value: function flip() {
                            var currentIndex = _lodash2['default'].indexOf(this.orders, this.current);
                            this.current = currentIndex === 0 ? this.orders[1] : this.orders[0];
                        }
                    }, {
                        key: 'toParam',
                        value: function toParam() {
                            return new _Param.Param({ id: this.id, value: this.current });
                        }
                    }]);

                    return SingleSortOrder;
                })();

                exports.SingleSortOrder = SingleSortOrder;

            }).call(this, typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : typeof window !== "undefined" ? window : {})

        }, { "../Param": 21 }], 26: [function (require, module, exports) {
            "use strict";

            Object.defineProperty(exports, "__esModule", {
                value: true
            });

            var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

            function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

            var _Param = require("../Param");

            var SortBy = (function () {
                function SortBy() {
                    var _ref = arguments.length <= 0 || arguments[0] === undefined ? {} : arguments[0];

                    var value = _ref.value;
                    var _ref$id = _ref.id;
                    var id = _ref$id === undefined ? "sortBy" : _ref$id;

                    _classCallCheck(this, SortBy);

                    this.id = id;
                    this.value = value;
                    this.type = "SortBy";
                }

                _createClass(SortBy, [{
                    key: "toParam",
                    value: function toParam() {
                        return new _Param.Param({ id: this.id, value: this.value });
                    }
                }]);

                return SortBy;
            })();

            exports.SortBy = SortBy;

        }, { "../Param": 21 }]
    }, {}, [1])(1)
});
//# sourceMappingURL=velir.search.angular.js.map
