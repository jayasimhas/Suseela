(function () {
	var dryCargoBulkFixtures = {
		table: '',
		recreateObj: {},
		renderDate: function(dateObj){ 
			var options = '';
			$.each(dateObj[0], function(key, val){ 
				$.each(val, function(idx, value){ 
					options += '<option value="'+value+'">'+value+'</option>'
				});
			});
			
			$('#drybulkselectDay').html(options);
		},
		renderTable: function(){
			var self = this, loadDateVal = $('#drybulkselectDay option').val();

			self.callAjaxFn(loadDateVal);
			$(document).on('change', '#drybulkselectDay', function(){
				var selectDateVal = $('#drybulkselectDay option').val();
				self.callAjaxFn(selectDateVal);
			});
			
			$('table').on('click', 'a.top', function(){
				var $this = $(this), table = $this.closest('table'), tablePos = table.offset().top;
				if(window.matchMedia("(max-width: 400px)").matches){
					$(window).scrollTop(tablePos - 40);
				}
				else{
					$(window).scrollTop(tablePos);
				}
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
			var self = this, loadHead = true, tableStr = '';
			
			tableStr += self.loadDescView(searchData);
			tableStr += self.loadMobileView(searchData);
			 
			$('#dryCargoBulkFixtures table').html(tableStr);
		},
		loadDescView: function(tableObj){
			var self = this, getArr;
			$.each(tableObj[0], function(key, val){ 
				self.table += '<table class="table">'
				self.table += '<thead class="table_head">';
				self.table += '<tr><th colspan="3" class="pad-full-10">'+key+'</th></tr>';
				self.table += '</thead>';
				self.table += '</table>';	
			 });
			 
			$.each(tableObj[0], function(idx, val){
				getArr = val;
			});
			for(var i=0; i<getArr.length; i++){
			  if(self.recreateObj[getArr[i]['fixtureType']] == undefined){ 
				self.recreateObj[getArr[i]['fixtureType']] = [];
			  }
			}
			for(var prop in self.recreateObj){
				for(var i=0; i<getArr.length; i++){
					if(prop == getArr[i]['fixtureType']){
						self.recreateObj[prop].push(getArr[i]);
					}
				}
			}
			var tbody = '<tbody class="visible-lg">';
			$.each(self.recreateObj, function(key, val){
				tbody += '<tr><td colspan="2" class="graybg RB18 p-10">'+key+'</td><td align="right" class="graybg RB18 p-10"><a href="javascript: void(0);" class="top"><span class="arrow"></span>Top</a></td></tr>';
				$.each(val, function(idx, value){
					tbody += '<tr>';
					$.each(value, function(k, v){
						tbody += '<td class="R16 pad-10">'+v+'</td>';
					});
					tbody += '</tr>';
				});
			});
			tbody += '</tbody>';
			
			return tbody;
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
			
			return tbody;
		},
		init: function(dateOptions) {
			this.renderDate(dateOptions);
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#dryCargoBulkFixtures').length > 0) {
			dryCargoBulkFixtures.init(DryBulkFixturesDateOptions);
		}
	});
})();