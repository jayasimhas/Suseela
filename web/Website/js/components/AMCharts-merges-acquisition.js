$(document).ready(function() {
	var FirstChartType = $('#chartdiv-deals').attr('data-type'),
		FirstChartColor = $('#chartdiv-deals').attr('data-color'),
		SecondChartType = $('#chartdiv-region').attr('data-type'),
		SecondChartColor = $('#chartdiv-region').attr('data-color'),
		ThirdChartType = $('#chartdiv-region2').attr('data-type'),
		ThirdChartColor = $('#chartdiv-region2').attr('data-color'),
		FourthChartType = $('#chartdiv-type').attr('data-type'),
		FourthChartColor = $('#chartdiv-type').attr('data-color');
    AmCharts.ready(function() {
    var chart = new AmCharts.AmSerialChart();
    chart.dataProvider = generateChartDataSizeRange();
    chart.categoryField = "range";
    var valueAxis1 = new AmCharts.ValueAxis();
    valueAxis1.integersOnly = true;
    chart.addValueAxis(valueAxis1);
    var graph = new AmCharts.AmGraph();
    graph.valueField = "count";
    graph.type = FirstChartType;
    graph.fillAlphas = 0.7;FourthChartType
    graph.lineAlpha = 0.5;
    graph.balloonText = "[[title]]: [[count]]";
    graph.balloonColor = '#'+FirstChartColor;
    graph.fillColors = '#'+FirstChartColor;
    graph.lineColor = '#'+FirstChartColor;
    chart.addGraph(graph);
    chart.write('chartdiv-deals');
    var chart2 = new AmCharts.AmSerialChart();
    chart2.dataProvider = generateChartDataDealsByRegion();
    chart2.categoryField = "region";
    var valueAxis2 = new AmCharts.ValueAxis();
    valueAxis2.integersOnly = true;
    chart2.addValueAxis(valueAxis2);
    var graph2 = new AmCharts.AmGraph();
    graph2.valueField = "tally";
    graph2.type = SecondChartType;
    graph2.fillAlphas = 0.7;
    graph2.lineAlpha = 0.5;
    graph2.balloonText = "[[title]]: [[value]]";
    graph2.balloonColor = '#'+SecondChartColor;
    graph2.fillColors = '#'+SecondChartColor;
    graph2.lineColor = '#'+SecondChartColor;
    chart2.categoryAxis.labelRotation = 40;
    chart2.categoryAxis.autoGridCount = false;
    chart2.addGraph(graph2);
    chart2.write('chartdiv-region');
    var chart3 = new AmCharts.AmSerialChart();
    chart3.dataProvider = generateChartDataByRegion();
    chart3.categoryField = "region";
    var valueAxis3 = new AmCharts.ValueAxis();
    valueAxis3.integersOnly = true;
    chart3.addValueAxis(valueAxis3);
    var graph3 = new AmCharts.AmGraph();
    graph3.valueField = "count";
    graph3.type = ThirdChartType;
    graph3.fillAlphas = 0.7;
    graph3.lineAlpha = 0.5;
    graph3.balloonText = "[[title]]: [[value]]";
    graph3.balloonColor = '#'+ThirdChartColor;
    graph3.fillColors = '#'+ThirdChartColor;
    graph3.lineColor = '#'+ThirdChartColor;
    chart3.categoryAxis.labelRotation = 40;
    chart3.categoryAxis.autoGridCount = false;
    chart3.addGraph(graph3);
    chart3.write('chartdiv-region2');
    var chart4 = new AmCharts.AmSerialChart();
    chart4.dataProvider = generateChartData();
    chart4.validateData();
    chart4.categoryField = "type";
    var valueAxis4 = new AmCharts.ValueAxis();
    valueAxis4.integersOnly = true;
    chart4.addValueAxis(valueAxis4);
    var graph4 = new AmCharts.AmGraph();
    graph4.valueField = "count";
    graph4.type = FourthChartType;
    graph4.fillAlphas = 0.7;
    graph4.lineAlpha = 0.5;
    graph4.balloonText = "[[title]]: [[value]]";
    graph4.balloonColor = '#'+FourthChartColor;
    graph4.fillColors = '#'+FourthChartColor;
    graph4.lineColor = '#'+FourthChartColor;
    chart4.addGraph(graph4);
    chart4.write('chartdiv-type');
    var inputSet = $(".merge-acquistion th input");
    inputSet.each(function() {
    	if(!$(this).hasClass('range-field')) {
	        $(this).keyup(function() {
	            chart.dataProvider = generateChartDataSizeRange();
	            chart.validateData();
	            chart2.dataProvider = generateChartDataDealsByRegion();
	            chart2.validateData();
	            chart3.dataProvider = generateChartDataByRegion();
	            chart3.validateData();
	            chart4.dataProvider = generateChartData();
	            chart4.validateData();
	        });
	    }
    });
    // $("#filterDropDown").change(function() {
    //     chart.dataProvider = generateChartDataSizeRange();
    //     chart.validateData();
    //     chart2.dataProvider = generateChartDataDealsByRegion();
    //     chart2.validateData();
    //     chart3.dataProvider = generateChartDataByRegion();
    //     chart3.validateData();
    //     chart4.dataProvider = generateChartData();
    //     chart4.validateData();
    // });
    $($(".range-field")[0]).keyup(function() {
        chart.dataProvider = generateChartDataSizeRange();
        chart.validateData();
        chart2.dataProvider = generateChartDataDealsByRegion();
        chart2.validateData();
        chart3.dataProvider = generateChartDataByRegion();
        chart3.validateData();
        chart4.dataProvider = generateChartData();
        chart4.validateData();
    });
    $($(".range-field")[1]).keyup(function() {
        chart.dataProvider = generateChartDataSizeRange();
        chart.validateData();
        chart2.dataProvider = generateChartDataDealsByRegion();
        chart2.validateData();
        chart3.dataProvider = generateChartDataByRegion();
        chart3.validateData();
        chart4.dataProvider = generateChartData();
        chart4.validateData();
    });
    // $("#largest-deals").click(function() {
    //     chart.dataProvider = generateChartDataSizeRange();
    //     chart.validateData();
    //     chart2.dataProvider = generateChartDataDealsByRegion();
    //     chart2.validateData();
    //     chart3.dataProvider = generateChartDataByRegion();
    //     chart3.validateData();
    //     chart4.dataProvider = generateChartData();
    //     chart4.validateData();
    // });
    var defaultRegions = ["Africa", "Asia-Pacific", "Bermuda", "Europe", "Global", "Latin America", "London/UK", "North America"];
    var defaultTypes = ["non-life", "life", "international/reinsurance", "composite"];

    function generateChartDataSizeRange() {
        var chartDataType = [];
        var price = [];
        var i;
        $(".merge-acquistion td[deal='Price']").each(function() {
            var tempPrice = $(this).text();
            price.push(tempPrice);
        });
        var range = {
            100: 0,
            250: 0,
            1000: 0
        };
        for (i = 0; i < price.length; i++) {
            if (price[i] >= 100 && price[i] < 250) {
                range[100] = range[100] + 1;
            } else if (price[i] >= 250 && price[i] < 1000) {
                range[250] = range[250] + 1;
            } else if (price[i] >= 1000) {
                range[1000] = range[1000] + 1;
            }
        }
        var rangeAsArray = [];
        var rangeCountAsArray = [];
        for (var key in range) {
            rangeAsArray.push(key);
            rangeCountAsArray.push(range[key]);
        }
        for (i = 0; i < rangeAsArray.length; i++) {
            rangeAsArray[i] = rangeAsArray[i] + "+";
            var obj = {
                range: rangeAsArray[i],
                count: rangeCountAsArray[i],
                title: rangeAsArray[i]
            };
            chartDataType.push(obj);
        }
        return chartDataType;
    }

    function generateChartDataDealsByRegion() {
        var chartDataType = [];
        var regions = [];
        $(".merge-acquistion td[deal='TargetLocation']").each(function() {
            var tempRegion = $(this).text();
            regions.push(tempRegion);
        });
        var price = [];
        $(".merge-acquistion td[deal='Price']").each(function() {
            var tempPrice = $(this).text();
            price.push(tempPrice);
        });
        var regionWithTotalPrice = {};
        var regionName = {};
        for (i = 0; i < regions.length; ++i) {
            regionWithTotalPrice[regions[i]] = 0;
            regionName[regions[i]] = regions[i];
        }
        for (i = 0; i < regions.length; ++i) {
            if (price[i] != "-" && price[i] >= 100) {
                regionWithTotalPrice[regions[i]] = regionWithTotalPrice[regions[i]] + parseFloat(price[i]);
            }
        }
        var regionNameAsArray = [];
        for (var key in regionName) {
            if (regionName.hasOwnProperty(key)) {
                regionNameAsArray.push(regionName[key]);
            }
        }
        regionNameAsArray.sort();
        var isNull = false;
        isNull = jQuery.isEmptyObject(price);
        if (!isNull) {
            for (var i = 0; i < regionNameAsArray.length; i++) {
                if (regionWithTotalPrice[regionNameAsArray[i]] != 0) {
                    var abbrRegionNameAsArray = abbreviate(regionNameAsArray[i]);
                    var obj = {
                        region: abbrRegionNameAsArray,
                        tally: regionWithTotalPrice[regionNameAsArray[i]],
                        title: regionNameAsArray[i]
                    };
                    chartDataType.push(obj);
                }
            }
        } else {
            for (i = 0; i < defaultRegions.length; i++) {
                var abbrRegionNameAsArray = abbreviate(defaultRegions[i]);
                var obj = {
                    region: abbrRegionNameAsArray,
                    tally: 0,
                    title: defaultRegions[i]
                };
                chartDataType.push(obj);
            }
        }
        if (chartDataType.length === 0) {
            for (i = 0; i < defaultRegions.length; i++) {
                var abbrRegionNameAsArray = abbreviate(defaultRegions[i]);
                var obj = {
                    region: abbrRegionNameAsArray,
                    tally: 0,
                    title: defaultRegions[i]
                };
                chartDataType.push(obj);
            }
        }
        return chartDataType;
    }

    function generateChartDataByRegion() {
        var chartDataType = [];
        var regions = [];
        $(".merge-acquistion td[deal='TargetLocation']").each(function() {
            var tempRegion = $(this).text();
            regions.push(tempRegion);
        });
        var price = [];
        $(".merge-acquistion td[deal='Price']").each(function() {
            var tempPrice = $(this).text();
            price.push(tempPrice);
        });
        var regionOver100m = [];
        for (var i = 0; i < regions.length; i++) {
            if (price[i] != "-" && price[i] >= 100) {
                regionOver100m.push(regions[i]);
            }
        }
        var uniqueRegions = new stringSet();
        if (regionOver100m.length != 0) {
            for (i = 0; i < regionOver100m.length; i++) {
                uniqueRegions.add(regionOver100m[i]);
            }
        }
        var orderedValueRegions = uniqueRegions.values();
        orderedValueRegions.sort();
        var cR = uniqueRegions.count();
        var isNull = false;
        isNull = jQuery.isEmptyObject(uniqueRegions.count());
        if (!isNull) {
            for (i = 0; i < orderedValueRegions.length; i++) {
                var abbrUniqueValueType = abbreviate(orderedValueRegions[i]);
                var obj = {
                    region: abbrUniqueValueType,
                    count: cR[orderedValueRegions[i]],
                    title: orderedValueRegions[i]
                };
                chartDataType.push(obj);
            }
        } else {
            for (i = 0; i < defaultRegions.length; i++) {
                var abbrRegionNameAsArray = abbreviate(defaultRegions[i]);
                var obj = {
                    region: abbrRegionNameAsArray,
                    count: 0,
                    title: defaultRegions[i]
                };
                chartDataType.push(obj);
            }
        }
        return chartDataType;
    }

    function generateChartData() {
        var chartDataType = [];
        var types = [];
        $(".merge-acquistion td[deal='TargetSector']").each(function() {
            var tempType = $(this).text();
            types.push(tempType);
        });
        var price = [];
        $(".merge-acquistion td[deal='Price']").each(function() {
            var tempPrice = $(this).text();
            price.push(tempPrice);
        });
        var typesOver100m = [];
        for (var i = 0; i < types.length; i++) {
            if (price[i] != "-" && price[i] >= 100) {
                typesOver100m.push(types[i]);
            }
        }
        var uniqueTypes = new stringSet();
        if (typesOver100m.length != 0) {
            for (i = 0; i < typesOver100m.length; i++) {
                uniqueTypes.add(typesOver100m[i]);
            }
        }
        var uniqueValueTypes = uniqueTypes.values();
        uniqueValueTypes.sort();
        uniqueValueTypes.reverse();
        var cT = uniqueTypes.count();
        var isNull = false;
        isNull = jQuery.isEmptyObject(uniqueTypes.count());
        if (!isNull) {
            for (i = 0; i < uniqueValueTypes.length; i++) {
                var abbrUniqueValueType = abbreviate(uniqueValueTypes[i]);
                var obj = {
                    type: abbrUniqueValueType,
                    count: cT[uniqueValueTypes[i]],
                    title: uniqueValueTypes[i]
                };
                chartDataType.push(obj);
            }
        } else {
            for (i = 0; i < defaultTypes.length; i++) {
                var abbrTypeNameAsArray = abbreviate(defaultTypes[i]);
                var obj = {
                    type: abbrTypeNameAsArray,
                    count: 0,
                    title: defaultTypes[i]
                };
                chartDataType.push(obj);
            }
        }
        return chartDataType;
    }

    function abbreviate(str) {
        if (str == "international/reinsurance") {
            str = "int/re"
        }
        if (str == "composite") {
            str = "com"
        }
        if (str == "Africa") {
            str = "AF"
        }
        if (str == "Asia-Pacific") {
            str = "APAC"
        }
        if (str == "Bermuda") {
            str = "BM"
        }
        if (str == "Europe") {
            str = "EU"
        }
        if (str == "Global") {
            str = "G"
        }
        if (str == "Latin America") {
            str = "LA"
        }
        if (str == "London/UK") {
            str = "UK"
        }
        if (str == "North America") {
            str = "NA"
        }
        return str;
    }

    function stringSet() {
        var setObj = {},
            val = {};
        var objectCount = {};
        this.add = function(str) {
            setObj[str] = val;
            if (objectCount[str] == null || objectCount[str] == {}) {
                objectCount[str] = 1;
            } else {
                var count = objectCount[str];
                objectCount[str] = count + 1;
            }
        };
        this.count = function() {
            return objectCount;
        }
        this.contains = function(str) {
            return setObj[str] === val;
        };
        this.remove = function(str) {
            delete setObj[str];
        };
        this.values = function() {
            var values = [];
            for (var i in setObj) {
                if (setObj[i] === val) {
                    values.push(i);
                }
            }
            return values;
        };
    }
});
});