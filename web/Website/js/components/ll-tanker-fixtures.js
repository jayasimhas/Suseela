(function () {
	var tankerFixtures = {
		renderDateData: function(data){
			if(data[0]['SelectDate'] !== undefined){
				$('#tankerselectDay').html(this.loadDropdownData(data[0]['SelectDate']));
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
			var self = this, loadDateVal = $('#tankerselectDay option').val();
			for(var prop in tableObj[0]){
				self.sendHTTPRequest(tableObj[0][prop], prop);
			}		
			//self.callAjaxFn(loadDateVal);
			$(document).on('change', '#tankerselectDay', function(){
				var selectDateVal = $('#tankerselectDay option').val();
				self.callAjaxFn(selectDateVal);
			});
		},
		callAjaxFn: function(seldateVal){
			 var self = this;
			$.ajax({
				url: '/Download/JsonDataFromFeed/ReadJsonMarketFixture/',
				data: {'dateVal': seldateVal, 'feedUrl': $('#TankerFixHiddenVal').val()},
				dataType: 'json',
				type: 'GET',
				success: function (searchData) {
					$('#tankerFixtures').empty();
					for(var prop in searchData[0]){
						self.sendHTTPRequest(searchData[0][prop], prop);
					}
					
				},
				error: function (err) {
					console.log('Feed url is getting error: ' + JSON.stringify(err));
				}
			});
		},
		sendHTTPRequest: function(searchData, prop){
			var self = this, tableStr = '';

			tableStr += self.loadDesktopView(searchData, prop);
			tableStr += self.loadMobileView(searchData, prop);
			 
			$('#tankerFixtures').append(tableStr);
		},
		loadDesktopView: function(tableData, prop){
			var tableStr = '', tableHead = tableData[0], rowIdx = 0;
				tableStr += '<table class="table descView"><thead class="table_head">';
				tableStr += '<tr>';
				tableStr += '<th colspan="8" class="pad-full-10">'+prop+'</th>';
				tableStr += '</tr>';
				tableStr += '<tr class="visible-lg">';
				for(var prop in tableHead){
					tableStr += '<th class="pad-10">'+prop+'</th>';
				}
				tableStr += '</tr>';
				tableStr += '</thead>';
				
				tableStr += '<tbody class="visible-lg">';		
				for(var i=0; i<tableData.length; i++){
					rowIdx++; 
					var rowCls = (rowIdx % 2 === 0) ? 'oddCls' : '';
					tableStr += '<tr class="'+rowCls+'">';
					for(var prop in tableData[i]){
						tableStr += '<td class="R16 pad-10">'+tableData[i][prop]+'</td>';
					}
					tableStr += '</tr>';
				}
				tableStr += '</tbody>';
				tableStr += '</table>';
					 
			return tableStr;
		},
		loadMobileView: function(tableData, prop){
			var mobileStr = '';
				mobileStr += '<table class="table mobView">';
				mobileStr += '<thead class="table_head">';
				mobileStr += '<tr>';
				mobileStr += '<th colspan="2" class="pad-full-10">'+prop+'</th>';
				mobileStr += '</tr>';
				mobileStr += '</thead>';
				$.each(tableData, function(p, v){
					mobileStr += '<tbody class="visible-sm">';
					for(var prop in v){
						mobileStr += '<tr>';
						mobileStr += '<td class="pad-10 mobleftCol">'+prop+'</td>';
						mobileStr += '<td class="pad-10 mobrigCol">'+v[prop]+'</td>';
						mobileStr += '</tr>';
					}
					mobileStr += '<tr class="borTop"><td colspan="2"></td></tr>';
					mobileStr += '</tbody>';
				});
				mobileStr += '</table>'; 
			return mobileStr;
		},
		init: function(dateObj) {
			this.renderDateData(dateObj);
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#tanker-fixtures').length > 0) {
			tankerFixtures.init(TankerFixturesDateOptions);
		}
	});
})();