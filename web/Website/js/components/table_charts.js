
function PrintCompanyGraph(chartData, divId, graphType){
	var chart1 = new AmCharts.AmSerialChart();

	chart1.dataProvider = chartData;
	chart1.categoryField = "year";

	chart1.marginTop = 20;
	chart1.marginBottom = 25;
	chart1.marginLeft = 55;
	//chart1.marginLeft = 45;
	chart1.marginRight = 25;
	chart1.height = '100%';
	chart1.width = '100%';
	//chart1.fontSize = 8;

    chart1.startDuration = 0;
    chart1.startRadius = "100%";

	var graph1 = new AmCharts.AmGraph();
	graph1.valueField = "value";

	graph1.lineColor = "#" + chartData[0].colour;
	graph1.fillColor = "#" + chartData[0].colour;
	graph1.balloonText = "[[category]]: [[value]]";
	graph1.type = graphType;
	graph1.lineAlpha = 0;
	graph1.fillAlphas = 0.8;
    graph1.startDuration = 0;
    graph1.startRadius = "100%";
	//graph1.lineThickness = 2;
	chart1.addGraph(graph1);

	var catAxis = chart1.categoryAxis;

	catAxis.gridPosition = "start";
	catAxis.autoGridCount = true;

	//catAxis.autoGridCount = false;
	//catAxis.gridCount = 5;
	//catAxis.labelFrequency = 1;
	
	// chart1.addTitle("Millions $", 8);

	chart1.write(divId);
}

$(function() {
	if($('#graph-carousel') && $('#graph-carousel').length){
		if (window.matchMedia("(min-width: 670px)").matches){
			$('.graphsPan').removeClass('owl-carousel');
			$('.loadChart').removeClass('item');
		}
	
		PrintCompanyGraph(chartData_GWP, "chartdiv1", "line");
		PrintCompanyGraph(chartData_NWP, "chartdiv2", "column");
		PrintCompanyGraph(chartData_UR, "chartdiv3", "line");
		PrintCompanyGraph(chartData_NP, "chartdiv4", "column");
		PrintCompanyGraph(chartData_SF, "chartdiv5", "line");
		PrintCompanyGraph(chartData_NWPNR, "chartdiv6", "column");
		PrintCompanyGraph(chartData_SFNR, "chartdiv7", "line");
		PrintCompanyGraph(chartData_NPSFR, "chartdiv8", "column");
	}
});