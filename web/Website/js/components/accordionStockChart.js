$(function() {
	var GWPGraph = false;
	$("#GWP").on('click', function() {
		if (GWPGraph == false) {
			GWPcreateStockChart();
			GWPGraph = true;
		}
	});

	var NWPGraph = false;
	$("#NWP").on('click', function () {
		if (NWPGraph == false) {
			NWPcreateStockChart();
			NWPGraph = true;
		}
	});

	var URGraph = false;
	$("#UR").on('click', function () {
		if (URGraph == false) {
			URcreateStockChart();
			URGraph = true;
		}
	});

	var NPGraph = false;
	$("#NP").on('click', function () {
		if (NPGraph == false) {
			NPcreateStockChart();
			NPGraph = true;
		}
	});

	var SFGraph = false;
	$("#SF").on('click', function () {
		if (SFGraph == false) {
			SFcreateStockChart();
			SFGraph = true;
		}
	});

	var NWPNRGraph = false;
	$("#NWPNR").on('click', function () {
		if (NWPNRGraph == false) {
			NWPNRcreateStockChart();
			NWPNRGraph = true;
		}
	});

	var SFNRGraph = false;
	$("#SFNR").on('click', function () {
		if (SFNRGraph == false) {
			SFNRcreateStockChart();
			SFNRGraph = true;
		}
	});

	var NPSFRGraph = false;
	$("#NPSFR").on('click', function () {
		if (NPSFRGraph == false) {
			NPSFRcreateStockChart();
			NPSFRGraph = true;
		}
	});

});

// GWP STOCK CHART
function GWPcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array(), matches, d;
	var chart = new AmCharts.AmStockChart();
    chart.startDuration = 0;
    chart.startRadius = "100%";
    chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";


	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr=0; dataSetCtr<chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (var i = 0; i<dataVars['chartData' + (dataSetCtr+1) + '_GWP'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_GWP'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr+1) + '_GWP'][i]['year'],dataVars['chartData' + (dataSetCtr+1) + '_GWP'][i]['year'],'4'];
			d = new Date(matches[1],11,31,0,0,0,0);
			dataVars['chartData' + (dataSetCtr+1) + '_GWP'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [ {
			fromField : "value",
			toField : "value"
		}, {
			fromField : "year",
			toField : "year"
		} ];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr+1) + '_GWP'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
    chart.chartCursorSettings.zoomable = false;


	// PANELS ///////////////////////////////////////////


	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";


	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
    stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";
	// second stock panel

    // set panels to the chart
    chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
    sbsettings.selectedBackgroundColor = "#888";
    sbsettings.selectedGraphFillColor = "#666";
    sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;
	chart.write('GWP-CHART');
}

// NWP STOCK CHART
function NWPcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart();
    chart.startDuration = 0;
    chart.startRadius = "100%";

    chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (var dataSetCtr=0; dataSetCtr<chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (var i = 0; i<dataVars['chartData' + (dataSetCtr+1) + '_NWP'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_NWP'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr+1) + '_NWP'][i]['year'],dataVars['chartData' + (dataSetCtr+1) + '_NWP'][i]['year'],'4'];
			d = new Date(matches[1],11,31,0,0,0,0);
			dataVars['chartData' + (dataSetCtr+1) + '_NWP'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [ {
			fromField : "value",
			toField : "value"
		}, {
			fromField : "year",
			toField : "year"
		} ];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_NWP'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
    chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
    stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
    sbsettings.selectedBackgroundColor = "#888";
    sbsettings.selectedGraphFillColor = "#666";
    sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('NWP-CHART');

}

// UR STOCK CHART
function URcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart();
    chart.startDuration = 0;
    chart.startRadius = "100%";

    chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (dataSetCtr=0; dataSetCtr<chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
	    for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_UR'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_UR'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
	        matches = [dataVars['chartData' + (dataSetCtr + 1) + '_UR'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_UR'][i]['year'], '4'];
			d = new Date(matches[1],11,31,0,0,0,0);
			dataVars['chartData' + (dataSetCtr + 1) + '_UR'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [ {
			fromField : "value",
			toField : "value"
		}, {
			fromField : "year",
			toField : "year"
		} ];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_UR'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
    chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
    stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
    sbsettings.selectedBackgroundColor = "#888";
    sbsettings.selectedGraphFillColor = "#666";
    sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('UR-CHART');

}

// NP STOCK CHART
function NPcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart();
    chart.startDuration = 0;
    chart.startRadius = "100%";

    chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (dataSetCtr=0; dataSetCtr<chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
	    for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_NP'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_NP'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
	        matches = [dataVars['chartData' + (dataSetCtr + 1) + '_NP'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_NP'][i]['year'], '4'];
			d = new Date(matches[1],11,31,0,0,0,0);
			dataVars['chartData' + (dataSetCtr + 1) + '_NP'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [ {
			fromField : "value",
			toField : "value"
		}, {
			fromField : "year",
			toField : "year"
		} ];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_NP'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
    chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
    stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
    sbsettings.selectedBackgroundColor = "#888";
    sbsettings.selectedGraphFillColor = "#666";
    sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('NP-CHART');

}

// SF STOCK CHART
function SFcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart();
    chart.startDuration = 0;
    chart.startRadius = "100%";

    chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (dataSetCtr=0; dataSetCtr<chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
	    for (i = 0; i < dataVars['chartData' + (dataSetCtr + 1) + '_SF'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_SF'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
	        matches = [dataVars['chartData' + (dataSetCtr + 1) + '_SF'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_SF'][i]['year'], '4'];
			d = new Date(matches[1],11,31,0,0,0,0);
			dataVars['chartData' + (dataSetCtr + 1) + '_SF'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [ {
			fromField : "value",
			toField : "value"
		}, {
			fromField : "year",
			toField : "year"
		} ];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr + 1) + '_SF'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
    chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
    stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
    sbsettings.selectedBackgroundColor = "#888";
    sbsettings.selectedGraphFillColor = "#666";
    sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('SF-CHART');

}

// NWPNR STOCK CHART
function NWPNRcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart();
    chart.startDuration = 0;
    chart.startRadius = "100%";

    chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (dataSetCtr=0; dataSetCtr<chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i<dataVars['chartData' + (dataSetCtr+1) + '_NWPNR'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_NWPNR'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
		    matches = [dataVars['chartData' + (dataSetCtr + 1) + '_NWPNR'][i]['year'], dataVars['chartData' + (dataSetCtr + 1) + '_NWPNR'][i]['year'], '4'];
			d = new Date(matches[1],11,31,0,0,0,0);
			dataVars['chartData' + (dataSetCtr+1) + '_NWPNR'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [ {
			fromField : "value",
			toField : "value"
		}, {
			fromField : "year",
			toField : "year"
		} ];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr+1) + '_NWPNR'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
    chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
    stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
    sbsettings.selectedBackgroundColor = "#888";
    sbsettings.selectedGraphFillColor = "#666";
    sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('NWPNR-CHART');

}

// SFNR STOCK CHART
function SFNRcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart();
    chart.startDuration = 0;
    chart.startRadius = "100%";

    chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (dataSetCtr=0; dataSetCtr<chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i<dataVars['chartData' + (dataSetCtr+1) + '_SFNR'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_SFNR'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr+1) + '_SFNR'][i]['year'],dataVars['chartData' + (dataSetCtr+1) + '_SFNR'][i]['year'],'4'];
			d = new Date(matches[1],11,31,0,0,0,0);
			dataVars['chartData' + (dataSetCtr+1) + '_SFNR'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [ {
			fromField : "value",
			toField : "value"
		}, {
			fromField : "year",
			toField : "year"
		} ];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr+1) + '_SFNR'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
    chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
    stockPanel1.addStockGraph(graph1);
    stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
    sbsettings.selectedBackgroundColor = "#888";
    sbsettings.selectedGraphFillColor = "#666";
    sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('SFNR-CHART');

}

// NPSFR STOCK CHART
function NPSFRcreateStockChart() {
	var dataSet = new Array();
	var dataSetList = new Array();
	var chart = new AmCharts.AmStockChart();
    chart.startDuration = 0;
    chart.startRadius = "100%";

    chart.pathToImages = "/skins/insurance-day/amcharts_2.7.6/images/";

	// DATASETS //////////////////////////////////////////
	// create data sets first

	for (dataSetCtr=0; dataSetCtr<chartDataSet.length; dataSetCtr++) {

		// KLUGE: translate given string e.g. 2006 - Q1 to date object
		for (i = 0; i<dataVars['chartData' + (dataSetCtr+1) + '_NPSFR'].length; i++) {
			//matches = window['chartData' + (dataSetCtr+1) + '_NPSFR'][i]['year'].match(/(\d\d\d\d).*Q(\d)/);
			//d = new Date();
			//d.setFullYear(matches[1], 0, 0);
			//d.setDate(d.getDate() + 90 * parseInt(matches[2], 10));
			matches = [dataVars['chartData' + (dataSetCtr+1) + '_NPSFR'][i]['year'],dataVars['chartData' + (dataSetCtr+1) + '_NPSFR'][i]['year'],'4'];
			d = new Date(matches[1],11,31,0,0,0,0);
			dataVars['chartData' + (dataSetCtr+1) + '_NPSFR'][i]['year'] = d;
		}
		// KLUGE ends here

		dataSet[dataSetCtr] = new AmCharts.DataSet();
		dataSet[dataSetCtr].title = chartDataSet[dataSetCtr];

		dataSet[dataSetCtr].fieldMappings = [ {
			fromField : "value",
			toField : "value"
		}, {
			fromField : "year",
			toField : "year"
		} ];

		dataSet[dataSetCtr].dataProvider = dataVars['chartData' + (dataSetCtr+1) + '_NPSFR'];
		dataSet[dataSetCtr].categoryField = "year";

		dataSetList.push(dataSet[dataSetCtr]);
	}
	chart.dataSets = dataSetList;
    chart.chartCursorSettings.zoomable = false;

	// PANELS ///////////////////////////////////////////

	// first stock panel
	var stockPanel1 = new AmCharts.StockPanel();
	stockPanel1.showCategoryAxis = true;
	stockPanel1.title = "Company";
	stockPanel1.percentHeight = 70;
    stockPanel1.startDuration = 0;
    stockPanel1.startRadius = "100%";

	// graph of first stock panel
	var graph1 = new AmCharts.StockGraph();
	graph1.valueField = "value";
	graph1.comparable = true;
	graph1.compareField = "value";
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
	stockPanel1.addStockGraph(graph1);
    stockPanel1.recalculateToPercents = "never";

	// create stock legend
	stockPanel1.stockLegend = new AmCharts.StockLegend();
	// second stock panel

	chart.panels = [stockPanel1];

	// OTHER SETTINGS ////////////////////////////////////
	var sbsettings = new AmCharts.ChartScrollbarSettings();
	sbsettings.graph = graph1;
    sbsettings.selectedBackgroundColor = "#888";
    sbsettings.selectedGraphFillColor = "#666";
    sbsettings.enabled = false;
	chart.chartScrollbarSettings = sbsettings;

	// DATA SET SELECTOR
	var dataSetSelector = new AmCharts.DataSetSelector();
	dataSetSelector.position = "right";
	chart.dataSetSelector = dataSetSelector;

	chart.write('NPSFR-CHART');

}