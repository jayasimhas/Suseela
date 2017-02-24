(function () {
	var dryCargo = {
		table: '',
		renderTable: function(){
		var self = this;
		 //console.log(tableObj);
		 $.each(tableObj, function(datekey, date){ 
			
				$.each(date, function(key, value) {
					//console.log(key);
					self.table += '<thead class="table_head">';
					self.table += '<tr><th colspan="8" class="pad-full-10">'+key+'</th></tr>'									
					self.table += '<tr class="visible-lg">';
					$.each(value, function(objData, objVal) {
						//console.log(objVal);
						var tableHead = objVal; 
						for(var key in tableHead){
							self.table += '<th class="pad-10">'+key+'</th>'
						}
						self.table += '</tr>';
						self.table += '</thead>';
						self.table += '<tbody class="visible-lg">';
						self.table += '<tr>';
						$.each(objVal, function(responseKey, responseVal) {
                            self.table += '<td class="R16 pad-10">'+responseVal+'</td>';                           
						});	
						self.table += '</tr>';
						self.table += '</tbody>';	
					});
					 $('.table').html(self.table);
				});
				
		 });
		
		},
		renderMobile: function() {
			var self = this;
			 //console.log(tableObj);
			 $.each(tableObj, function(datekey, date){ 
				
					$.each(date, function(key, value) {
						//console.log(key);
						self.table += '<tbody class="visible-sm">';
						self.table += '<tr>';
						$.each(value, function(objData, objVal) {
							//console.log(objVal);
							var tableHead = objVal; 
							for(var key in tableHead){
								self.table += '<td class="pad-10 mobleftCol">'+key+'</th>'
							}
							$.each(objVal, function(responseKey, responseVal) {
								self.table += '<td class="pad-10 mobrigCol">'+responseVal+'</td>';                           
							});	
							self.table += '</tr>';
							self.table += '</tbody>';	
						});
						 $('.table').html(self.table);
					});
					
			 });
		},
		init: function() {
			this.renderTable();
			//this.renderMobile();
		}
	}
	
	$(document).ready(function() {
		if($('#dryCargo').length > 0) {
			dryCargo.init();
		}
	});
})();