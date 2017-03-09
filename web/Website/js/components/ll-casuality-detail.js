(function () {
	var CasualityDetail = {
		RenderTable: function(data, Parent) {
			var Data = data[0].casualtyDetail;

			Parent.append('<thead class="table_head">'+
									'<tr>'+
									  '<th colspan="2" class="p-10">'+((Data.Heading) ? Data.Heading : '&nbsp;')+'</th>'+
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
								StrMsg += "<p><strong>" +Messages[i].date+ "</strong>" +Messages[i].message+ "</p>";
								
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
			CasualityDetail.init(window.jsonCasualtyDetailData, $('#casualty-detail-table'));
		}
	});
})();