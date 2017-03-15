(function () {
	var dryCargo = {
		renderDate: function(dateObj){ 
			var options = '';
			$.each(dateObj[0], function(key, val){ 
				$.each(val, function(idx, value){ 
					options += '<option value="'+value.Value+'">'+value.Text+'</option>'
				});
			});
			$('#dryCargodateselect').html(options);
		},
		renderTable: function(){
			var self = this, loadDateVal = $('#dryCargodateselect option').val();
			self.callAjaxFn(loadDateVal);
			$(document).on('change', '#dryCargodateselect', function(){
				var selectDateVal = $('#dryCargodateselect option').val();
				self.callAjaxFn(selectDateVal);
			});
		},
		callAjaxFn: function(seldateVal){
			 var self = this;
			$.ajax({
				url: '/Download/JsonDataFromFeed/ReadJsonMarketFixture/',
				data: {'dateVal': seldateVal, 'feedUrl': $('#drycargoHiddenVal').val()},
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
			var self = this, tableStr = '';
			tableStr += this.renderDesktop(searchData);
			tableStr += this.renderMobile(searchData);
			$('#dryCargo').html(tableStr);			 
		},
		renderDesktop: function(tableObj){
			var self = this, deskStr = ''; 
			 $.each(tableObj, function(datekey, date){ 
				$.each(date, function(key, value) { 
					deskStr += '<table class="table descView">'
					deskStr += '<thead class="table_head">';
					deskStr += '<tr><th colspan="6" class="pad-full-10">'+key+'</th></tr>'									
					deskStr += '<tr class="visible-lg">';
					var tableHead = value[0];
						for(var key in tableHead){
							deskStr += '<th class="pad-10">'+key+'</th>'
						}
						deskStr += '</tr>';
						deskStr += '</thead>';
						deskStr += '<tbody class="visible-lg">';
						deskStr += '<tr>';
						$.each(value, function(objData, objVal) {
							$.each(objVal, function(responseKey, responseVal) {
								deskStr += '<td class="R16 pad-10">'+responseVal+'</td>';                           
							});	
							deskStr += '</tr>';	
						});					
					deskStr += '</tbody>';
					deskStr += '</table>';	
				});
			 });
			 return deskStr;
		},
		renderMobile: function(tableObj) {
			var self = this, mobStr = '';
			 $.each(tableObj, function(datekey, date){ 
				
					$.each(date, function(key, value) { 
						mobStr += '<table class="table mobView" width="100%">'
						mobStr += '<thead class="table_head">';
						mobStr += '<tr><th colspan="2" class="pad-full-10">'+key+'</th></tr>';
						mobStr += '</thead>';
						mobStr += '<tbody class="visible-sm">';
						
						$.each(value, function(objData, objVal) {					
							$.each(objVal, function(responseKey, responseVal) { 
								mobStr += '<tr>';
								mobStr += '<td class="pad-10 mobleftCol">'+responseKey+'</td>';   
								mobStr += '<td class="pad-10 mobrigCol">'+responseVal+'</td>'; 
								mobStr += '</tr>';		
							});	
							mobStr += '<tr><td class="rowbordinMob" colspan="2"></td></tr>';
						});					
						mobStr += '</tbody>';
						mobStr += '</table>';
					}); 
			 });
			 return mobStr;
		},
		init: function(dateData) {
			this.renderDate(dateData);
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#dryCargo').length > 0) {
			dryCargo.init(marketdryCargodate);
		}
	});
})();