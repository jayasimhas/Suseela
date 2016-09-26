function RenderAMChartData() {

    var amChartType = $('#amChartType').val();
    var dataProvider = $('#amChartDataProvider').val();
    var categoryField = $('#CategoryField').val();
    var valueField = $('#ValueField').val();

    AmCharts.makeChart("chartdiv", {
        "type": amChartType,
        "dataProvider": dataProvider,
        "categoryField": categoryField,
        "graphs": [{
            "valueField": valueField,
            "type": "column"
        }]
    });
};