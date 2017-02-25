(function () {
	var dryCargoBulkFixtures = {
		table: '',
		renderTable: function(){
		var self = this, getArr, newObj = {};
		
			$.each(tableObj[0], function(key, val){ 
					self.table += '<table class="table descView">'
					self.table += '<thead class="table_head">';
					self.table += '<tr><th colspan="6" class="pad-full-10">'+key+'</th></tr>'									
					self.table += '<tr class="visible-lg">';
					var tableHead = val[0];
						for(var prop in tableHead){
							self.table += '<th class="pad-10">'+prop+'</th>';
						}
						self.table += '</tr>';
						self.table += '</thead>';
					self.table += '</table>';	
					$('#dryCargoBulkFixtures').html(self.table);
				 
			 });
			 
			$.each(tableObj[0], function(idx, val){
				getArr = val; 
			});
			for(var i=0; i<getArr.length; i++){
			  if(newObj[getArr[i]['fixtureType']] == undefined){ 
				newObj[getArr[i]['fixtureType']] = [];
			  }
			}
			for(var prop in newObj){
				for(var i=0; i<getArr.length; i++){
					if(prop == getArr[i]['fixtureType']){
						newObj[prop].push(getArr[i]);
					}
				}
			}
			var tbody = '<tbody>';
			$.each(newObj, function(key, val){
				tbody += '<tr><td colspan="3" class="graybg RB18 p-10">'+key+'</td></tr>';
				$.each(val, function(idx, value){
					tbody += '<tr>';
					$.each(value, function(k, v){
						tbody += '<td class="R16 pad-10">'+v+'</td>';
					});
					tbody += '</tr>';
				});
			});
			tbody += '</tbody>';
			
			$('#dryCargoBulkFixtures table').append(tbody);
		},		
		init: function() {
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#dryCargoBulkFixtures').length > 0) {
			dryCargoBulkFixtures.init();
		}
	});
})();