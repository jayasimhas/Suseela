var PrintCompanyGraph = function (chartData, divId, graphType, color) {

    AmCharts.makeChart(divId,
                    {
                        "type": "serial",
                        "categoryField": "year",
                        "dataDateFormat": "YYYY",
                        "handDrawScatter": 4,
                        "theme": "default",
                        "categoryAxis": {
                            "minPeriod": "YYYY",
                            "parseDates": true,
                            "gridPosition": "start",
                            "autoGridCount": true
                        },
                        "chartCursor": {
                            "enabled": true,
                            "animationDuration": 0,
                            "categoryBalloonDateFormat": "YYYY"
                        },
                        "trendLines": [],
                        "graphs": [
                            {
                                "fillAlphas": 0.7,
                                "id": "AmGraph-1",
                                "lineAlpha": 0,
                                "title": "graph 1",
                                "valueField": "value",
                                "type": graphType,
                                "lineColor": "#" + color,
                                "fillColor": "#" + color,
                                "startDuration": 0,
                                "startRadius": "100%",
                                "balloonText": "[[category]]: [[value]]"
                            }
                        ],
                        "guides": [],
                        "valueAxes": [
                            {
                                "id": "ValueAxis-1",
                                "title": ""
                            }
                        ],
                        "allLabels": [],
                        "balloon": {},
                        "dataProvider": chartData
                    }
                );
}