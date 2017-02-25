(function () {
	var dryCargoBulkFixtures = {
		table: '',
		renderTable: function(){
		var self = this;
		 //console.log(tableObj);
			 $.each(tableObj, function(datekey, date){ 
				
					$.each(date, function(key, value) { 
						self.table += '<table class="table descView">'
						self.table += '<thead class="table_head">';
						self.table += '<tr><th colspan="6" class="pad-full-10">'+key+'</th></tr>'									
						self.table += '<tr class="visible-lg">';
						//console.log(value[0]);
						var tableHead = value[0];
							for(var key in tableHead){
								self.table += '<th class="pad-10">'+key+'</th>';
							}
							self.table += '</tr>';
							self.table += '</thead>';
							self.table += '<tbody class="visible-lg">'; 
							self.table += '<tr>';
							$.each(value, function(objData, objVal) {
								//console.log(objVal);									
								$.each(objVal, function(responseKey, responseVal) {
								
								/*var fixtureType = responseVal[0];
								for(var key in fixtureType){
									self.table += '<td class="pad-10">'+key+'</td>'
								}*/
									self.table += '<td class="R16 pad-10">'+responseVal+'</td>';                           
								});	
								self.table += '</tr>';	
							});					
						self.table += '</tbody>';
						self.table += '</table>';	
						 $('#dryCargoBulkFixtures').html(self.table);
					});
					
			 });
		
		},
		renderMobile: function() {
			var self = this;
			 $.each(tableObj, function(datekey, date){ 
				
					$.each(date, function(key, value) {
						//console.log(key);
						self.table += '<table class="table mobView">'
						self.table += '<thead class="table_head">';
						self.table += '<tr><th colspan="8" class="pad-full-10">'+key+'</th></tr>';
						self.table += '</thead>';
						self.table += '<tbody class="visible-sm">';
						
						$.each(value, function(objData, objVal) {
							//console.log(objVal);						
							$.each(objVal, function(responseKey, responseVal) { 
								self.table += '<tr>';
								self.table += '<td class="pad-10 mobleftCol">'+responseKey+'</td>';   
								self.table += '<td class="pad-10 mobrigCol">'+responseVal+'</td>'; 
								self.table += '</tr>';		
							});	
							
						});					
						self.table += '</tbody>';
						self.table += '</table>';
						 $('#dryCargoBulkFixtures').html(self.table); 
					});
					
			 });
		},
		init: function() {
			this.renderTable();
			this.renderMobile();
		}
	}
	
	$(document).ready(function() {
		if($('#dryCargoBulkFixtures').length > 0) {
			dryCargoBulkFixtures.init();
		}
	});
})();