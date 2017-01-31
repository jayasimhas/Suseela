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
			var self = this;
			$('.submit').click(function(){
				var searchData = tableObj;
				/*$.ajax({
					url: '/Download/JsonDataFromFeed/ReadJsonShippingMovements/ ',
					data: {'feed': $('#ResultTableFeedUrl').val(), 'areaCode': $('#areaCode option').val(), 'movementType': $('#movementType option').val()},
					dataType: 'json',
					type: 'GET',
					success: function (searchData) {*/
						self.sendHTTPRequest(searchData);
					/*},
					error: function (err) {
						console.log(err)  
					}
				});*/
				
				$('.shippingData').addClass('hide');
				$('.hideMarketData').removeClass('hide');
			});
			
			$(document).on('click', '.moveTop', function(){
				$(window).scrollTop(0);
			});
			
			$('.gotolinks').on('click', 'li a', function(){
				var $this = $(this), redirectLink = $this.attr('data-link');
				if(window.matchMedia("(max-width: 640px)").matches) {
					$(window).scrollTop($('#marketDataTable tr[data-mname='+redirectLink+']').offset().top - 40);
				}
				else{
					$(window).scrollTop($('#marketDataTable tr[data-name='+redirectLink+']').offset().top);
				}
			});
		},
		sendHTTPRequest: function(searchData){
			var self = this, loadHead = true,
				tableStr = '<thead class="table_head">';
				tableStr += '<tr><th colspan="7" class="pad-full-10">'+searchData[0].areaname+'</th></tr>';
			$.each(searchData[0], function(key, val){
				if(typeof val === 'object' && loadHead){
					loadHead = false;
					tableStr += '<tr class="visible-lg">';
					$.each(val[0], function(k, v){
						tableStr += '<td class="pad-10">'+k+'</td>';
					});
					tableStr += '</tr>';
				}
			});
			tableStr += '</thead>';
			
			tableStr += self.loadDesktopView(searchData[0]);
			tableStr += self.loadMobileView(searchData[0]);
			 
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
		
		loadDesktopView: function(tableData){
			var desktopStr = '<tbody class="visible-lg">', dataIdx = 0;
			$.each(tableData, function(key, val){
				if(typeof val === 'object'){
					dataIdx++;
					desktopStr += '<tr data-name="focusData_'+dataIdx+'">';
					desktopStr += '<td colspan="6" class="graybg RB18 pad-10">'+key+'</td>';
					desktopStr += '<td colspan="1" align="right" class="graybg RB18 pad-10 moveTop"><a href="javascript: void(0);" class="top"><span class="arrow"></span>Top</a></td>';
					desktopStr += '</tr>';
					
					$.each(val, function(i, v){
						desktopStr += '<tr>';
						desktopStr += '<td class="R16 pad-10">'+v["Move Date"]+'</td>';
						desktopStr += '<td class="R16 pad-10">'+v["Vessel Name"]+'</td>';
						desktopStr += '<td class="R16 pad-10">'+v["Flag"]+'</td>';
						desktopStr += '<td class="R16 pad-10">'+v["Gross"]+'</td>';
						desktopStr += '<td class="R16 pad-10">'+v["Origin"]+'</td>';
						desktopStr += '<td class="R16 pad-10">'+v["Destination"]+'</td>';
						desktopStr += '<td class="R16 pad-10">'+v["Vessel Type"]+'</td>';
						desktopStr += '</tr>';
					});
				}
			});
			desktopStr += '</tbody>';
			return desktopStr;
		},
		loadMobileView: function(tableData){
			var mobileStr = '<tbody class="visible-sm">', dataIdx = 0;
			$.each(tableData, function(key, val){
				if(typeof val === 'object'){
					dataIdx++;
					mobileStr += '<tr data-mname="focusData_'+dataIdx+'">';
					
					mobileStr += '<td colspan="2" class="graybg RB18 pad-full-10">'+key+'<a href="javascript: void(0);" class="moveTop top"><span class="arrow"></span>Top</a></td>';
					mobileStr += '</tr>';
					
					$.each(val, function(i, v){
						var indx = 0;
						$.each(v, function(idx, vl){
							indx++;
							var borTop = i !== 0 && indx == 1 ? 'borTop' : '';
							mobileStr += '<tr>';
							mobileStr += '<td class="pad-10 mobleftCol '+borTop+'">'+ idx +'</td>';
							mobileStr += '<td class="pad-10 mobrigCol '+borTop+'">'+vl+'</td>';
							mobileStr += '</tr>';
						});
					});
				}
			});
			mobileStr += '</tbody>';
			return mobileStr;
		},
		init: function(data) {
			this.renderShippingData(data);
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#market-data-tool').length > 0) {
			marketDataTool.init(window.shippingMovements);
		}
	});
})();