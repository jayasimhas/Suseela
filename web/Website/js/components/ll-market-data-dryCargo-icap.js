(function () {
	var dryCargoIcap = {
		renderTable: function(tableData, id){
			var self = this, tableStr = '';
			tableStr += self.renderDesktop(tableData);
			tableStr += self.renderMobile(tableData);
			id.html(tableStr);
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
						mobStr += '<table class="table mobView">'
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
							
						});					
						mobStr += '</tbody>';
						mobStr += '</table>';
						 $('#dryCargo').html(mobStr);
					}); 
			 });
			 return mobStr;
		},		
		init: function(tableData, id) {
			this.renderTable(tableData, id);
		}
	}
	
	$(document).ready(function() {
		if($('#dryCargoIcap').length > 0) {
			dryCargoIcap.init(drycragoicapTable, $('#dryCargoIcap'));
		}
	});
})();