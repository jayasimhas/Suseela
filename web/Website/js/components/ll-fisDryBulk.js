(function () {
	var fisDryBulk = {
		renderDateData: function(data){
			if(data[0]['SelectDate'] !== undefined){
				$('#fisDryBulkselectDay').html(this.loadDropdownData(data[0]['SelectDate']));
			}
		},
		loadDropdownData: function(options){
			var optionStr = '';
			$.each(options, function(idx, val){
				if(idx == 0){
					optionStr += '<option value="'+val.Value+'" selected="selected">'+val.Text+'</option>';
				}
				else{
					optionStr += '<option value="'+val.Value+'">'+val.Text+'</option>';
				}
			});
			return optionStr;
		},
		renderTable: function(renderEle){
			var self = this, loadDateVal = $('#fisDryBulkselectDay option').val();
			 
			self.callAjaxFn(loadDateVal, renderEle);
			$(document).on('change', '#fisDryBulkselectDay', function(){
				var selectDateVal = $('#fisDryBulkselectDay option').val();
				self.callAjaxFn(selectDateVal, renderEle);
			});
		},
		callAjaxFn: function(seldateVal, renderEle){
			 var self = this;
			$.ajax({
				url: '/Download/JsonDataFromFeed/ReadJsonMarketFixture/',
				data: {'dateVal': seldateVal, 'feedUrl': $('#fisDryBulkHiddenVal').val()},
				dataType: 'json',
				type: 'GET',
				success: function (searchData) {
					self.sendHTTPRequest(searchData, renderEle);
				},
				error: function (err) {
					console.log('Feed url is getting error: ' + JSON.stringify(err));
				}
			});
		},
		sendHTTPRequest: function(searchData, renderEle){
			var self = this;
			self.loadDesktopView(searchData[0], renderEle);
			self.loadMobileView(searchData[0], renderEle);
		},
		loadDesktopView: function(tableData, renderEle){
			var tableStr = '<table class="table">';
			$.each(tableData, function(key, val){
				tableStr += '<tr class="tableTitle"><td colspan="5" class="pad-full-10">'+key+'</td></tr>';
				$.each(val, function(idx, value){
					var objVal = val[idx];
					tableStr += '<tr class="tableheader">';
					for(var p in objVal){
						tableStr += '<td class="pad-full-10">'+p+'</td>';
					}
					tableStr += '</tr>';
					tableStr += '<tr>';
					for(var p in objVal){
						tableStr += '<td class="pad-full-10">'+objVal[p]+'</td>';
					}
					tableStr += '</tr>';
				});
			});
			
			$(renderEle+'.showinDesk').html(tableStr);
		},
		loadMobileView: function(tableData, renderEle){
			var tableStr = '<table class="table">';
				
				$.each(tableData, function(key, val){
					tableStr += '<tr class="tableTitle"><td colspan="2" class="pad-full-10">'+key+'</td></tr>';
					$.each(val, function(idx, value){
						var curObj = val[idx];
						for(var prop in curObj){
							tableStr += '<tr><td class="pad-10 mobleftCol">'+prop+'</td><td class="pad-10 mobrigCol">'+curObj[prop]+'</td></tr>';
						}
						tableStr += '<tr><td colspan="2" class="boedTop"></td></tr>';
					});
				});
				tableStr += '</table>';
				
			$(renderEle+'.showinMob').html(tableStr);
		},
		setColHeight: function(parentId){
			$(parentId).closest('#howeRobinsonContainer').find('.states_heading .R16').each(function(idx){
				var $this = $(this), colHeight = $this.height();
				$('.titleHead').each(function() {
					$($(this).find('.R16')[idx]).height(colHeight);
				});
			});
		},
		initiateCarousel: function(id){
			$(id).owlCarousel({
				loop:false,
				margin:0,
				merge:true,
				nav:false,
				slideBy: 4,
				responsive: {
				   0:{
				   items:4
				   },
				   678:{
				   items:2
				   },
				   320:{
					items:2
				   },
				   480:{
					items:2
				   },
				   1024:{
				   items:3
				   }
				}
			});
		},
		init: function(dateObj, renderEle) {
			var self = this;
			self.renderDateData(dateObj);
			self.renderTable(renderEle);
			$(window).resize(function() {
				self.setColHeight(renderEle);
			});
		}
	}
	
	$(document).ready(function() {
		if($('#fisDryBulkContainer').length > 0) {
			fisDryBulk.init(fisDryBulkDateOptions, '.fisDryBulkTable');
		}
	});
})();