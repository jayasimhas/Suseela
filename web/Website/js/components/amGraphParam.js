function ConvertToCSV(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';

    for (var i = 0; i < array.length; i++) {
        var line = '';
        var value= '';
        var year='';
        for (var index in array[i]) {
            if (index == 'year') year = array[i][index];
            if (index == 'value') value = array[i][index];
        }
        line = year+';'+value;
        str += line + '\n';
    }

    return str;
}

window.PrintGraph = function(chartData, divId, skinUrl){

    if (chartData.length>0){

        /*if ($.browser.msie){

            var jsonObject = JSON.stringify(chartData);

            var csvData = ConvertToCSV(jsonObject);

            var chartColor = chartData[0].colour;

            var params =
            {
                bgcolor:"#E7E7E7",
                wmode:"opaque"
            };

            var chartSettings ="<settings><data_type>csv</data_type><background><color>E7E7E7</color></background><text_size>9</text_size><colors>"+chartColor+","+chartColor+"</colors><plot_area><margins><left>55</left><right>25</right><top>20</top><bottom>25</bottom></margins></plot_area><grid><category><alpha>13</alpha><dash_length>1</dash_length></category><value><alpha>13</alpha><dash_length>1</dash_length><approx_count>3</approx_count></value></grid><axes><category><width>1</width></category><value><width>1</width></value></axes><values><category><frequency>2</frequency></category><value><skip_first>0</skip_first></value></values><legend><enabled>0</enabled></legend><angle>0</angle><column><alpha>83</alpha><border_alpha>23</border_alpha><balloon_text>{category}:{value}</balloon_text></column><graphs><graphgid='0'><title>Stock</title><alpha>85</alpha><visible_in_legend>0</visible_in_legend></graph></graphs><labels><labellid='0'><y>18</y><align>center</align></label></labels></settings>";

            var flashVars =
            {
                path: skinUrl + "amcharts_2.7.6/flash/",

                chart_data: csvData,

                chart_settings: chartSettings
            };

            swfobject.embedSWF(skinUrl + "amcharts_2.7.6/flash/amcolumn.swf", divId, "220", "120", "8.0.0", skinUrl + "amcharts_2.7.6/flash/expressInstall.swf", flashVars, params);*/
        
        //} else{
          var chart1 = new AmCharts.AmSerialChart();
          /*
          // Remove " - Q1, - Q2" from the year:
          $.each(chartData, function(i, item) {
              if(item.year){
                  var posQ = item.year.indexOf(" - ");
                  if(posQ > 0){
                      item.year = item.year.substring(0,posQ);
                  }
              } // Year is defined.
          });
          */

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
          graph1.type = "column";
          graph1.lineAlpha = 0;
          graph1.fillAlphas = 0.8;
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
        //}
    }
}

$(function(){
	if($('#mycarousel') && $('#mycarousel').length){
		$('#mycarousel.owl-carousel').owlCarousel({
			loop:true,
			autoPlay: false,
			nav: true,
			navContainer: '#customNav',
			dotsContainer: '#customDots',
			slideBy: 4,
			 navText: [
				  "<img src='/dist/img/prev-horizontal.png'/>",
				  "<img src='/dist/img/next-horizontal.png'/>"
				  ],
			responsive:{
				0:{
				items:4
				},
				678:{
				items:4
				},
				320:{
				 items:1
				},
				480:{
				 items:1
				},
				1000:{
				items:4
				}
			}
		});
	}
});