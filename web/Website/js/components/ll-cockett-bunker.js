(function () {
	// body...
	'use strict';

	var cockettBunker = {
		RenderTable: function(data, Parent) {
			var self = this, TableStr = "";
			Parent.empty();
			
			for(var key in data[0]) {
				TableStr += self.RenderSingleTable(key, data[0][key]);
			}

			Parent.append(TableStr);

		},
		RenderSingleTable: function(item, Data) {
			console.log(Data);
			var HeadingStr = "",
				SubHeadingStr = "",
				TbodyStr = "",
				Heading = Data[0];


			for(var key in Heading) {
				if(key.split("|").length === 1) {
					HeadingStr += "<th class='headerPad'>"+
										"<div>&nbsp;</div>"+
									"</th>";
				} else {
					HeadingStr += "<th class='headerPad rigalign'>"+
				"<div class=''>"+ key.split("|")[0] + " (" + key.split("|")[1] + ")</div>"+
									"</th>"; 
				}
			}
			var rowIdx = 0;
			for(var i = 0; i < Data.length; i++) {
				rowIdx++;
				var cls = (rowIdx % 2 === 0) ? 'oddCls' : '';
				var EachValue = Data[i],
					Td = "", align = "";
				for(var key in EachValue) {
					if(key !== "Header") {
						align = "rigalign"; 
					}
					Td += '<td class="borderpad '+align+'">' + EachValue[key] + '</td>';
				}
				TbodyStr += '<tr class="'+cls+'">' +Td + '</tr>';
			}

			var Table = '<table class="table">'+
							'<thead class="table_head">'+
								'<tr>'+
									'<th class="pad-full-10" colspan="3">'+
										item+
									'</th>'+
								'</tr>'+
								'<tr>'+
									HeadingStr+
								'</tr>'+
							'</thead>'+
							'<tbody>'+
								TbodyStr+
							'</tbody>'+
						'</table>';


			return Table;
		},
		init: function(data, id) {
			if(data.length !== 0){ 
				var self = this;
				self.RenderTable(data, id);
				id.closest('.small_table').css('width', '100%');
			}
			else{
				$('#cockettBunker').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnInfomessage').val()+'</p></div>');
			}
		}
	}
	
	if($('#cockettBunker').length > 0) { 
		if(typeof window.jsonCockettBunker !== 'undefined'){
			cockettBunker.init(window.jsonCockettBunker, $('#cockettBunker'));
		}
		else{
			$('#cockettBunker').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
		} 
	}

	
})();