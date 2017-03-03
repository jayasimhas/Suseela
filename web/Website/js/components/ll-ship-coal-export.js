(function () {
	// body...
	'use strict';

	var shipCoalExport = {
		RenderTable: function(data, Parent) {
			var self = this, TableStr = "";
			Parent.empty();
			
			TableStr = self.RenderSingleTable(data);

			Parent.append(TableStr);

		},
		RenderSingleTable: function(Data) {
			console.log(Data);
			var HeadingStr = "",
				SubHeadingStr = "",
				TbodyStr = "",
				Heading = Data[0];


			for(var key in Heading) {
				if(Array.isArray(Heading[key])) {
					HeadingStr += '<th  colspan="2" class="pad-full-10">' + key + '</th>';
					var SubHeading = Heading[key][0];
					for(var k in SubHeading) {
						SubHeadingStr += '<th colspan="1" class="pad-full-10">' + k+ '</th>';
					}
				} else {
					HeadingStr += '<th  colspan="1" class="pad-full-10">' + key + '</th>';
					SubHeadingStr += '<th colspan="1"></th>';
				}
			}

			for(var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
					Td = "";
				for(var key in EachValue) {
					if(Array.isArray(EachValue[key])) {
						var Values = EachValue[key][0];
						for(var k in Values) {
							Td += '<td colspan="1" class="pad-full-10" align="right">' + Values[k] + '</td>';
						}
					} else {
						Td += '<td colspan="1" class="pad-full-10">' + EachValue[key] + '</td>';
					}
				}
				TbodyStr += '<tr>' +Td + '</tr>';
			}
			

			var Table = '<table class="table">'+
							'<thead class="table_head">'+
								'<tr>'+
									HeadingStr+
								'</tr>'+
								'<tr class="visible-lg">'+
								SubHeadingStr+
								'</tr>'+
							'</thead>'+
							'<tbody class="visible-lg">'+
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

	if($('#shipCoalExport').length > 0) {
		shipCoalExport.init(window.jsonShipCoalExport, $('#shipCoalExport'));	
	}
	

	
})();