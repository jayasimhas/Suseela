(function () {
	var shipContainerShipFixtures = {
		table: '',	
		renderTable: function(){
		var self = this;
			 $.each(tableObj, function(datekey, value){ 
				//console.log(tableObj);
				self.table += '<table class="table">';	
				self.table += '<tbody class="visible-lg">';
			
				$.each(value, function(objData, objVal) {
					//console.log(objVal);	
					self.table += '<tr>';
					self.table += '<td class="R16 pad-10">';
					$.each(objVal, function(responseKey, responseVal) {
						self.table += responseVal;                           
					});	
					self.table += '</td>';
					self.table += '</tr>';	
				});					
				self.table += '</tbody>';
				self.table += '</table>';	
				 $('#shipContainerShipFixtures').html(self.table);
					
			 });
		
		},
		init: function() {
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#shipContainerShipFixtures').length > 0) {
			shipContainerShipFixtures.init();
		}
	});
})();