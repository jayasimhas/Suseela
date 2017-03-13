(function () {
	var tankerFixtures = {
		renderDateData: function(data){
			if(data[0]['SelectDate'] !== undefined){
				$('#shipfixselectDay').html(this.loadDropdownData(data[0]['SelectDate']));
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
			var self = this, loadDateVal = $('#shipfixselectDay option').val();
			self.callAjaxFn(loadDateVal);
			$(document).on('change', '#shipfixselectDay', function(){
				var selectDateVal = $('#shipfixselectDay option').val();
				self.callAjaxFn(selectDateVal);
			});
		},
		callAjaxFn: function(seldateVal){
			 var self = this;
			$.ajax({
				url: '/Download/JsonDataFromFeed/ReadJsonMarketFixture/',
				data: {'dateVal': seldateVal, 'feedUrl': $('#shipfixHiddenVal').val()},
				dataType: 'json',
				type: 'GET',
				success: function (searchData) {
					self.sendHTTPRequest(searchData);
				},
				error: function (err) {
					console.log('Data is not there: ' + JSON.stringify(err));
				}
			});
		},
		sendHTTPRequest: function(searchData){
			$('#shipFixtures').html(this.loadDataView(searchData[0]));
		},
		loadDataView: function(tableData){
			var tableStr = '<table class="table">', idx = 0;
			for(var prop in tableData){
				idx++;
				var cls = (idx % 2 == 0) ? 'oddCls' : '';
				tableStr += '<tr class="'+cls+'"><td class="R16 pad-10">' + tableData[prop] + '</td></tr>';
			}
			tableStr += '</table>';
			return tableStr;
		},
		init: function(dateObj) {
			this.renderDateData(dateObj);
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#shipContainerFixtures').length > 0) {
			tankerFixtures.init(shipContShipFixDateOptions);
		}
	});
})();