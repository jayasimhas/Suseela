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
					HeadingStr += '<th align="left" colspan="2" class="pad-10 main-heading">' + key + '</th>';
					var SubHeading = Heading[key][0];
					for(var k in SubHeading) {
						SubHeadingStr += '<th  class="pad-10">' + k+ '</th>';
					}
				} else {
					HeadingStr += '<th align="left" class="pad-10 main-heading">' + key + '</th>';
					SubHeadingStr += '<th></th>';
				}
			}

			for(var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
					Td = "";
				for(var key in EachValue) {
					if(Array.isArray(EachValue[key])) {
						var Values = EachValue[key][0];
						for(var k in Values) {
							Td += '<td class="pad-10" align="right">' + Values[k] + '</td>';
						}
					} else {
						Td += '<td class="pad-10">' + EachValue[key] + '</td>';
					}
				}
				TbodyStr += '<tr>' +Td + '</tr>';
			}
			

			var Table = '<table class="table">'+
							'<thead class="table_head">'+
								'<tr>'+
									HeadingStr+
								'</tr>'+
								'<tr>'+
								SubHeadingStr+
								'</tr>'+
							'</thead>'+
							'<tbody>'+
								TbodyStr+
							'</tbody>'+
						'</table>';
			return Table;
		}, 
		init: function(data, renderid) {
			this.RenderTable(data, renderid);
		}
	}
	if($('#shipCoalExport').length > 0) {
		shipCoalExport.init(window.jsonShipCoalExport, $('#shipCoalExport'));	
	}
})();