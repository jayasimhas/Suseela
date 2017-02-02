(function () {
	var marketData = {
		renderTable: function(data){
			var tableStr = '';
			tableStr += this.loadDesktopView(data);
			tableStr += this.loadMobileView(data);
			
			$('#marketDataTable').html(tableStr);
		},
		loadDesktopView: function(tableData){
			var headObj = tableData[0], indx = 0,
				desktopStr = '<thead class="table_head">';
				desktopStr += '<tr class="visible-lg">';
				$.each(headObj, function(key, val){
					desktopStr += '<td class="R16 pad-20">'+key+'</td>';
				});
				desktopStr += '</thead>';
						   
				desktopStr += '<tbody class="visible-lg">';
				$.each(tableData, function(idx, val){
					var oddCls = idx % 2 !== 0 ? 'oddCls' : '';
					desktopStr += '<tr class="'+oddCls+'">';
					$.each(val, function(k, v){
						var cls = (v.split(' ')[1].indexOf('-') !== -1) ? 'fall' : 'rise';
						if(indx >= 1){
							desktopStr += '<td class="R16 pad-20"><span class="numData">'+v.split(' ')[0]+'</span><span class="'+cls+'">'+v.split(' ')[1]+'</span></td>';
						}
						else{
							desktopStr += '<td class="R16 pad-20">'+v+'</td>';
						}
						indx++;
					});
					indx = 0;
					desktopStr += '</tr>';
				});
			 
			desktopStr += '</tbody>';
			return desktopStr;
		},
		loadMobileView: function(tableData){
			var mobileStr = '<tbody class="visible-sm">', indx = 0;
			$.each(tableData, function(idx, val){
				$.each(val, function(k, v){
					indx++;
					var cls = (v.split(' ')[1].indexOf('-') !== -1) ? 'fall' : 'rise';
					if(indx === 1){
						mobileStr += '<tr>';
						mobileStr += '<td class="pad-10 mobleftCol borTop">'+k+'</td>';
						mobileStr += '<td class="pad-10 mobrigCol borTop">'+v+'</td>';
						mobileStr += '</tr>';
					}
					else{
						mobileStr += '<tr>';
						mobileStr += '<td class="pad-10 mobleftCol">'+k+'</td>';
						mobileStr += '<td class="pad-10 mobrigCol">'+v.split(' ')[0]+'<span class="'+cls+'">'+v.split(' ')[1]+'</td>';
						mobileStr += '</tr>';
					}
				});
				indx = 0;
			});
			mobileStr += '</tbody>';
			return mobileStr;
		},
		init: function(data) {
			this.renderTable(data);
		}
	}
	
	$(document).ready(function() {
		if($('#market-data').length > 0) {
			marketData.init(window.jsonBalticIndices);
		}
	});
})();