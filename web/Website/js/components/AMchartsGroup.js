var PrintCompanyGraph = function (chartData, divId, graphType, color) {

    if(Array.isArray(chartData)) {
        if(chartData.length === 0) {
            var ErrorMessage = document.getElementById('hdnInfomessage').value;
            document.getElementById(divId).innerHTML = 
               '<div class="alert-error js-form-error js-form-error-PasswordRequhdnErrormessageirements" style="display: block;">'+
               '<svg class="alert__icon">'+
                        '<use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use>'+
                  '</svg>'+
               '<p class="page-account-contact__error">'+
                        ErrorMessage+
                  '</p>'+
               '</div>';
        } else {
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
    } else {
        var ErrorMessage = document.getElementById('hdnErrormessage').value;
            document.getElementById(divId).innerHTML = 
                  '<div class="alert-error js-form-error js-form-error-PasswordRequhdnErrormessageirements" style="display: block;">'+
                  '<svg class="alert__icon">'+
                           '<use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use>'+
                     '</svg>'+
                  '<p class="page-account-contact__error">'+
                           ErrorMessage+
                     '</p>'+
                  '</div>';
    }


}