(function () {
	// body...
	'use strict';

	var shipRoro = {
		RenderTable: function(data, Parent) {
			var self = this, TableStr = "";
			Parent.empty();
			
			for(var key in data[0]) {
				TableStr += self.RenderSingleTable(data[0][key]);
			}

			Parent.append(TableStr);

		},
		RenderSingleTable: function(Data) {
			console.log(Data);
			var HeadingStr = "",
				SubHeadingStr = "",
				TbodyStr = "",
				Heading = Data[0];


			for(var key in Heading) {
					HeadingStr += "<th colspan='1'>"+
										'<div class="pad-full-10">'+ key +'</div>'+
									"</th>";
			}

			for(var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
					Td = "",
					align = "right";
				for(var key in EachValue) {
					Td += '<td colspan="1" class="pad-full-10" align="left">' + EachValue[key] + '</td>';
				}
				TbodyStr += '<tr>' +Td + '</tr>';
			}

			var Table = '<table class="table">'+
							'<thead class="table_head">'+
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
	
	if($('#shipRoro').length > 0) {
		shipRoro.init(window.jsonshipRoro, $('#shipRoro'));	
	}

	
})();