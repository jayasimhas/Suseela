(function () {
	var dryCargoIcap = {
		table: '',
		renderTable: function(tableData){
		var self = this;
			 $.each(tableData, function(datekey, date){ 
					$.each(date, function(key, value) { 
						//console.log(key);
						self.table += '<table class="table descView">'
						self.table += '<thead class="table_head">';
						self.table += '<tr><th colspan="6" class="pad-full-10">'+key+'</th></tr>'									
						self.table += '<tr class="visible-lg">';
						//console.log(value[0]);
						var tableHead = value[0];
							for(var key in tableHead){
								self.table += '<th class="pad-10">'+key+'</th>'
							}
							self.table += '</tr>';
							self.table += '</thead>';
							self.table += '<tbody class="visible-lg">'; 
							self.table += '<tr>';
							$.each(value, function(objData, objVal) {
								//console.log(objVal);						
								$.each(objVal, function(responseKey, responseVal) {
									self.table += '<td class="R16 pad-10">'+responseVal+'</td>';                           
								});	
								self.table += '</tr>';	
							});					
						self.table += '</tbody>';
						self.table += '</table>';	
						 $('#dryCargoIcap').html(self.table);
					});
					
			 });
		
		},		
		init: function(tableData) {
			this.renderTable(tableData);
		}
	}
	
	$(document).ready(function() {
		if($('#dryCargoIcap').length > 0) {
			dryCargoIcap.init(dracragoicapTable);
		}
	});
})();