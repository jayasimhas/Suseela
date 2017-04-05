(function () {
	var CasualityDetail = {
		RenderTable: function(data, Parent) {
			var Data;
			$.each(data[0], function(key, val){
				Data = val;
			});
			Parent.append('<thead class="table_head">'+
									'<tr>'+
									  '<th colspan="2" class="p-10">'+((Data.Heading) ? Data.Heading : Data.Title)+'</th>'+
									'</tr>'+
								'</thead>');
			Parent.append('<tbody></tbody>');
			var Body = Parent.find('tbody');
			for(var key in Data) {
				if(key != 'Heading') {
					if(Array.isArray(Data[key])) {
						if(key == 'Messages') {
							var StrMsg = "",
								Messages = Data[key];
							for(var i in Messages) {
								StrMsg += "<p><strong>" +Messages[i].Date+ "</strong>" +Messages[i].Message+ "</p>";
							}
							Body.append('<tr>'+
									  '<td class="R16">'+key+'</td>'+
									  '<td class="R16">'+StrMsg+'</td>'+
									'</tr>');
						}
					} else {
						Body.append('<tr>'+
									  '<td class="R16">'+key+'</td>'+
									  '<td class="R16">'+Data[key]+'</td>'+
									'</tr>');
					}
				}
			}
		},
		init: function(data, Parent) {
			this.RenderTable(data, Parent);
		}
	}
	
	$(document).ready(function() {
		if($('#casualty-detail-table').length > 0) {
			if(typeof window.jsonCasualtyDetailData !== 'undefined' && window.jsonCasualtyDetailData.length !== 0){
				CasualityDetail.init(window.jsonCasualtyDetailData, $('#casualty-detail-table'));
			}
			else{
				$('#casualty-detail-table').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">Table data is not loading</p></div>');
			}
		}
	});
})();