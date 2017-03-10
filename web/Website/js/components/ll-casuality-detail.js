(function () {
	var CasualityDetail = {
		RenderTable: function(data, Parent) {
			var Data;
			$.each(data[0], function(key, val){
				Data = val;
			});
			Parent.append('<thead class="table_head">'+
									'<tr>'+
									  '<th colspan="2" class="p-10">'+((Data.Heading) ? Data.Heading : '&nbsp;')+'</th>'+
									'</tr>'+
								'</thead>');
			Parent.append('<tbody></tbody>');
			var Body = Parent.find('tbody');
			for(var key in Data[0]) {
				if(key != 'Heading') {
						Body.append('<tr>'+
									  '<td class="R16">'+key+'</td>'+
									  '<td class="R16">'+Data[0][key]+'</td>'+
									'</tr>');
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