(function () {
	var marketImarex = {
		renderDate: function(dateData){ 
			var options = '';
			$.each(dateData[0], function(key, val){ 
				$.each(val, function(idx, value){ 
					options += '<option value="'+value.Value+'">'+value.Text+'</option>'
				});
			});
			
			$('#imarexselect').html(options);
		},
		renderTable: function(Parent){
			var self = this, loadDateVal = $('#imarexselect option').val();
			//self.sendHTTPRequest(imarexTable, Parent);
			self.callAjaxFn(loadDateVal, Parent);
			$(document).on('change', '#imarexselect', function(){
				var selectDateVal = $('#imarexselect option').val();
				self.callAjaxFn(selectDateVal, Parent);
			});
		},
		callAjaxFn: function(seldateVal, Parent){
			 var self = this;
			$.ajax({
				url: '/Download/JsonDataFromFeed/ReadJsonMarketFixture/',
				data: {'dateVal': seldateVal, 'feedUrl': $('#imarexHiddenVal').val()},
				dataType: 'json',
				type: 'GET',
				success: function (searchData) {
					self.sendHTTPRequest(searchData, Parent);
				},
				error: function (err) {
					console.log('Feed url is getting error: ' + JSON.stringify(err));
				}
			});
		},
		sendHTTPRequest: function(searchData, Parent){
			this.loadDataView(searchData, Parent);
		},
		loadDataView: function(tableObj, Parent){
			var tableStr = '';
			$.each(tableObj[0], function(key, val){ 
				tableStr += '<table class="table">'
				tableStr += '<thead class="table_head">';
				tableStr += '<tr><th colspan="6" class="pad-full-10">'+key+'</th></tr>'									
				tableStr += '<tr class="blueBg">';
				var tableHead = val[0];
					for(var prop in tableHead){
						if(prop != "Header"){
						tableStr += '<th class="headerPad rigalign">'+prop+'</th>';
						}else{
						tableStr += '<th class="headerPad"></th>';
						}
					}
					tableStr += '</tr>';
					tableStr += '</thead>';
					
					tableStr += '<tbody>';
					var bgIdx = 0;
					$.each(val, function(idx, value){
						bgIdx++;
						var cls = (bgIdx % 2 === 0) ? 'oddCls' : '', tdIdx = 0;
						tableStr += '<tr class="'+cls+'">';
						$.each(value, function(k, v){
							if(tdIdx){
								tableStr += '<td class="borderpad rigalign">'+v+'</td>';
							}
							else{
								tableStr += '<td class="borderpad">'+v+'</td>';
							} 
							tdIdx++;
						});
						tableStr += '</tr>';
					});
					tableStr += '</tbody>';
				tableStr += '</table>';
				
			});
			Parent.html(tableStr); 
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