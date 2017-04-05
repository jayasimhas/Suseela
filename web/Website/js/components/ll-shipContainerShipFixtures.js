(function () {
	var shipContainerFixtures = {
		renderDateData: function(data){
			if(data[0]['SelectDate'] !== undefined){
				$('#shipfixselectDay').html(this.loadDropdownData(data[0]['SelectDate']));
			}
		},
		loadDropdownData: function(options){
			var optionStr = '';
			try {
				$.each(options, function(idx, val){
					if(idx == 0){
						optionStr += '<option value="'+val.Value+'" selected="selected">'+val.Text+'</option>';
					}
					else{
						optionStr += '<option value="'+val.Value+'">'+val.Text+'</option>';
					}
				});
				return optionStr;
			}
			catch(err) {
				$('#shipFixtures').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
			}
		},
		renderTable: function(){
			var self = this, loadDateVal = $('#shipfixselectDay option').val();
			
			//self.callAjaxFn(loadDateVal);
			$(document).on('change', '#shipfixselectDay', function(){
				var selectDateVal = $('#shipfixselectDay option').val();
				self.callAjaxFn(selectDateVal);
			});
		},
		callAjaxFn: function(seldateVal){
			 var self = this;
			$.ajax({
				url: '/Download/JsonDataFromFeed/ReadJsonMarketFixture/',
				data: {'dateVal': seldateVal, 'feedUrl': $('#shipfixHiddenVal').val()},
				dataType: 'json',
				type: 'GET',
				success: function (searchData) {
					if(searchData && searchData.length){
						self.sendHTTPRequest(searchData);
					}
					else{
						if(searchData.length == 0){
							$('#shipFixtures').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnInfomessage').val()+'</p></div>');
						}
						else{
							$('#shipFixtures').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
						}
					}
				},
				error: function (err) { 
					$('#shipFixtures').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
				}
			});
		},
		sendHTTPRequest: function(searchData){
			$('#shipFixtures').html(this.loadDataView(searchData[0]));
		},
		loadDataView: function(tableData){
			var tableStr = '<table class="table">', idx = 0;
			for(var prop in tableData){
				idx++;
				var cls = (idx % 2 == 0) ? 'oddCls' : '';
				tableStr += '<tr class="'+cls+'"><td class="R16 pad-10">' + tableData[prop] + '</td></tr>';
			}
			tableStr += '</table>';
			return tableStr;
		},
		init: function(dateObj) {
			this.renderDateData(dateObj);
			this.renderTable();
		}
	}
	
	$(document).ready(function() {
		if($('#shipContainerFixtures').length > 0) { 
			if(typeof window.shipContShipFixDateOptions !== 'undefined'){
				shipContainerFixtures.init(window.shipContShipFixDateOptions);
			}
			else{
				$('#shipFixtures').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
			}
		}
	});
})();