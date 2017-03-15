(function () {
	var marketImarex = {
		table: '', 
		renderDate: function(dateData){ 
			var options = '';
			$.each(dateData[0], function(key, val){ 
				$.each(val, function(idx, value){ 
					options += '<option value="'+value.Value+'">'+value.Text+'</option>'
				});
			});
			
			$('#imarexselect').html(options);
		},
		renderTable: function(){
			var self = this, loadDateVal = $('#imarexselect option').val();
			
			self.callAjaxFn(loadDateVal);
			$(document).on('change', '#imarexselect', function(){
				var selectDateVal = $('#imarexselect option').val();
				self.callAjaxFn(selectDateVal);
			});
		},
		callAjaxFn: function(seldateVal){
			 var self = this;
			$.ajax({
				url: '/Download/JsonDataFromFeed/ReadJsonMarketFixture/',
				data: {'dateVal': seldateVal, 'feedUrl': $('#imarexHiddenVal').val()},
				dataType: 'json',
				type: 'GET',
				success: function (searchData) {
					self.sendHTTPRequest(searchData);
				},
				error: function (err) {
					console.log('Feed url is getting error: ' + JSON.stringify(err));
				}
			});
		},
		sendHTTPRequest: function(searchData){
			$('#marketImarex').html(this.loadDataView(searchData));
		},
		loadDataView: function(tableObj){
			var self = this;
			$.each(tableObj[0], function(key, val){ 
				self.table += '<table class="table">'
				self.table += '<thead class="table_head">';
				self.table += '<tr><th colspan="6" class="pad-full-10">'+key+'</th></tr>'									
				self.table += '<tr class="blueBg">';
				var tableHead = val[0];
					for(var prop in tableHead){
						if(prop != "Header"){
						self.table += '<th class="pad-10">'+prop+'</th>';
						}else{
						self.table += '<th class="pad-10"></th>';
						}
					}
					self.table += '</tr>';
					self.table += '</thead>';
					
					self.table += '<tbody>';
					var bgIdx = 0;
					$.each(val, function(idx, value){
						bgIdx++;
						var cls = (bgIdx % 2 === 0) ? 'oddCls' : '';
						self.table += '<tr class="'+cls+'">';
						$.each(value, function(k, v){
							self.table += '<td class="R16 pad-10">'+v+'</td>';
						});
						self.table += '</tr>';
					});
					self.table += '</tbody>';
				self.table += '</table>';
				
			});
			$('#marketImarex').html(self.table); 
		}, 
		init: function(dateData, id) {
			this.renderDate(dateData);
			this.renderTable(id);
		}
	}
	
	$(document).ready(function() {
		if($('#marketImarex').length > 0) {
			marketImarex.init(imarexTabledate, $('#marketImarex'));
		}
	});
})();