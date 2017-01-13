(function () {
	var marketDataTool = {
		
		renderShippingData: function(data){
			if(data[0]['Areas'] !== undefined){
				$('#areaCode').html(this.loadDropdownData(data[0]['Areas']));
			}
			if(data[0]['MovementTypes'] !== undefined){
				$('#movementType').html(this.loadDropdownData(data[0]['MovementTypes']));
			}
		},
		loadDropdownData: function(options){
			var optionStr = '';
			$.each(options, function(idx, val){
				if(idx == 0){
					optionStr += '<option value="'+val.Value+'" selected="selected">'+val.Text+'</option>';
				}
				else{
					optionStr += '<option value="'+val.Value+'">'+val.Text+'</option>';
				}
			});
			return optionStr;
		},
		renderTable: function(){
			$('.submit').click(function(){
				$.ajax({
					url: '/Download/JsonDataFromFeed/ReadJsonShippingMovements/ ',
					data: {'feed': $('#ResultTableFeedUrl').val(), 'areaCode': $('#areaCode option').val(), 'movementType': ('#movementType option').val()},
					dataType: 'json',
					type: 'GET',
					success: function (searchData) {
						//var searchData = tableObj;
						var tableStr = '<thead class="table_head">', dataIdx = 0;
							tableStr += '<tr><th colspan="7" class="pad-full-10">'+searchData[0].areaname+'</th></tr>';
						$.each(searchData[0], function(key, val){
							if(typeof val === 'object'){
								tableStr += '<tr class="visible-lg">';
								$.each(val[0], function(k, v){
									//tableStr += '<td class="pad-10">'+k+'</td>';
								});
								tableStr += '</tr>';
							}
						});
						tableStr += '</thead>';
						
						tableStr += '<tbody class="visible-lg">';
						$.each(searchData[0], function(key, val){
							if(typeof val === 'object'){
								dataIdx++;
								tableStr += '<tr data-name="focusData_'+dataIdx+'">';
								tableStr += '<td colspan="6" class="graybg RB18 pad-10">'+key+'</td>';
								tableStr += '<td colspan="1" align="right" class="graybg RB18 pad-10 moveTop"><a href="javascript: void(0);">top</a></td>';
								tableStr += '</tr>';
								
								$.each(val, function(i, v){
									tableStr += '<tr>';
									tableStr += '<td class="RB16 pad-10">'+v["Move Date"]+'</td>';
									tableStr += '<td class="R16 pad-10">'+v["Vessel Name"]+'</td>';
									tableStr += '<td class="R16 pad-10">'+v["Flag"]+'</td>';
									tableStr += '<td class="R16 pad-10">'+v["Gross"]+'</td>';
									tableStr += '<td class="R16 pad-10">'+v["Origin"]+'</td>';
									tableStr += '<td class="R16 pad-10">'+v["Destination"]+'</td>';
									tableStr += '</tr>';
								});
							}
						});
						tableStr += '</tbody>';
						
						tableStr += '<tbody class="visible-sm">';
						$.each(searchData[0], function(key, val){
							if(typeof val === 'object'){
								tableStr += '<tr>';
								tableStr += '<td colspan="2" class="graybg RB18 pad-full-10">'+key+'</td>';
								tableStr += '</tr>';
								
								$.each(val, function(i, v){
									$.each(v, function(idx, vl){
										tableStr += '<tr>';
										tableStr += '<td class="pad-10 R21_GrayColor">'+idx+'</td>';
										tableStr += '<td class="pad-10 R21_RedColor">'+vl+'</td>';
										tableStr += '</tr>';
									});
								});
							}
						});
						tableStr += '</tbody>';
						
						$('#marketDataTable').html(tableStr);
						
						var marketLinks = '<ul>', linkIdx = 0;
						$.each(searchData[0], function(key, val){
							if(typeof val === 'object'){
								linkIdx++;
								marketLinks += '<li class="article-topics__li"><a href="javascript: void(0);" data-link=focusData_'+linkIdx+'>'+key+'</a></li>';
							}
						});
						marketLinks += '</ul>';
						
						$('.gotolinks').html(marketLinks);
					},
					error: function (err) {
						console.log(err)  
					}
				});
				
				$('.shippingData').addClass('hide');
				$('.hideMarketData').removeClass('hide');
			});
			
			$(document).on('click', '.moveTop', function(){
				$(window).scrollTop(0);
			});
			
			$('.gotolinks').on('click', 'li a', function(){
				var $this = $(this), redirectLink = $this.attr('data-link');
				$(window).scrollTop($('#marketDataTable tr[data-name='+redirectLink+']').offset().top);
			});
		},
		init: function(data) {
			//this.renderShippingData(data);
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#market-data-tool').length > 0) {
			marketDataTool.init(window.shippingMovements);
		}
	});
})();