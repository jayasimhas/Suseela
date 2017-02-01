(function () {
	var tankerFixtures = {
		renderDateData: function(data){
			if(data[0]['SelectDate'] !== undefined){
				$('#selectDay').html(this.loadDropdownData(data[0]['SelectDate']));
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
		renderTable: function(tableData){
			var self = this;
			self.sendHTTPRequest(tableData);
			
			$(document).on('change', '#selectDay', function(){
				var searchData = tableData;
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
			});
		},
		sendHTTPRequest: function(searchData){
			var self = this, loadHead = true, tableStr = '';
			
			tableStr += self.loadDesktopView(searchData);
			tableStr += self.loadMobileView(searchData);
			 
			$('#tankerFixtures').html(tableStr);
		},
		loadDesktopView: function(tableData){
			var tableStr = '';
			for(var i = 0; i < tableData.length; i++){
				var theadFlag = true, tbodyFlag = true, tbodyFlagend = false, idx = 0;
					tableStr += '<table class="table descView"><thead class="table_head">';
				for(var prop in tableData[i]){
					tableStr += '<tr>';
					tableStr += '<th colspan="8" class="pad-full-10">'+prop+'</th>';
					tableStr += '</tr>';
					for(var j = 0; j < tableData[i][prop].length; j++){
						idx++;
						if(tableData[i][prop].length == idx - 1) tbodyFlagend = true;
						var eachObj = tableData[i][prop];
						
						if(theadFlag){
							theadFlag = false;
							tableStr += '<tr class="visible-lg">';
							for(var p in eachObj[j]){
								tableStr += '<th class="pad-10">'+p+'</th>';
							}
							tableStr += '</thead>';
						}
						if(tbodyFlag){
							tbodyFlag = false;
							tableStr += '<tbody class="visible-lg">';
						}
						tableStr += '<tr>';
						for(var p in eachObj[j]){
							tableStr += '<td class="R16 pad-10">'+eachObj[j][p]+'</td>';
						}
						tableStr += '</tr>';
						if(tbodyFlagend){
							tbodyFlagend = false;
							tableStr += '</tbody>';
						}
					}
				}
				tableStr += '</table>';
			}
			return tableStr;
		},
		loadMobileView: function(tableData){
			var mobileStr = '', dataIdx = 0;
			$.each(tableData, function(index, value){
				mobileStr += '<table class="table mobView">';
				$.each(value, function(key, val){
					mobileStr += '<thead class="table_head">';
					mobileStr += '<tr>';
					mobileStr += '<th colspan="8" class="pad-full-10">'+key+'</th>';
					mobileStr += '</tr>';
					mobileStr += '</thead>';
					
					mobileStr += '<tbody class="visible-sm">';
					$.each(val, function(i, v){
						for(var prop in v){
							mobileStr += '<tr>';
							mobileStr += '<td class="pad-10 R21_GrayColor">'+prop+'</td>';
							mobileStr += '<td class="pad-10 R21_GrayColorVal">'+v[prop]+'</td>';
							mobileStr += '</tr>';
						}
					});
					mobileStr += '</tbody>';
				});
				mobileStr += '</table>';
			});
			return mobileStr;
		},
		init: function(dateObj, tableDataObj) {
			this.renderDateData(dateObj);
			this.renderTable(tableDataObj);
		}
	}
	
	$(document).ready(function() {
		if($('#tanker-fixtures').length > 0) {
			tankerFixtures.init(window.dateOptions, window.tableObj);
		}
	});
})();