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
					HeadingStr += "<th class='pad-full-10' colspan='1'>"+
										"<div class='text-center'>&nbsp;</div>"+
										"<div class='text-center'></div>"+
									"</th>";
				} else {
					HeadingStr += "<th class='pad-full-10' colspan='1'>"+
				"<div class='text-center'>"+ key.split("|")[0] + " (" + key.split("|")[1] + ")</div>"+
									"</th>";
				}
			}

			for(var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
					Td = "",
					align = "right";
				for(var key in EachValue) {
					if(key == "Header") {
						align = "left";
					}
					Td += '<td colspan="1" class="pad-full-10" align="' + align + '">' + EachValue[key] + '</td>';
				}
				TbodyStr += '<tr>' +Td + '</tr>';
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
			var self = this;
			self.RenderTable(data, id);
		}
	}
	
	if($('#cockettBunker').length > 0) {
		cockettBunker.init(window.jsonCockettBunker, $('#cockettBunker'));	
	}

	
})();