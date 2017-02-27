(function () {
	var marketImarex = {
		table: '', 
		renderDate: function(){ 
			var options = '';
			$.each(dateObj[0], function(key, val){ 
				$.each(val, function(idx, value){ 
					options += '<option value="'+value.Value+'">'+value.Text+'</option>'
				});
			});
			
			$('#selectWeek').html(options);
		},
		renderTable: function(){
			this.loadDescView();
			//this.loadMobileView();
		},
		loadDescView: function(){
			var self = this;
			$.each(tableObj[0], function(key, val){ 
				self.table += '<table class="table">'
				self.table += '<thead class="table_head">';
				self.table += '<tr><th colspan="6" class="pad-full-10">'+key+'</th></tr>'									
				self.table += '<tr class="blueBg">';
				var tableHead = val[0];
					for(var prop in tableHead){
						self.table += '<th class="pad-10">'+prop+'</th>';
					}
					self.table += '</tr>';
					self.table += '</thead>';
					
					self.table += '<tbody>';
					
					$.each(val, function(idx, value){ 
						self.table += '<tr>';
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
		loadMobileView: function(){
			var tbody = '<tbody class="visible-sm">';
			$.each(this.recreateObj, function(key, val){
				tbody += '<tr><td class="graybg RB18 p-10">'+key+'</td><td colspan="1" align="right" class="graybg RB18 p-10"><a href="javascript: void(0);" class="top"><span class="arrow"></span>Top</a></td></tr>';
				$.each(val, function(idx, value){
					$.each(value, function(k, v){
						tbody += '<tr><td class="pad-10 R21_GrayColor border-right">'+k+'</td><td class="pad-10 R21_GrayColor">'+v+'</td></tr>';
					});
					tbody += '<tr><td><hr /></td><td><hr /></td></tr>';
				});
			});
			tbody += '</tbody>';
			
			$('#marketImarex table').append(tbody);
		},
		init: function() {
			this.renderDate();
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#marketImarex').length > 0) {
			marketImarex.init();
		}
	});
})();