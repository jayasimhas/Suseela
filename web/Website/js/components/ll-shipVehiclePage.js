(function () {
	// body...
	'use strict';

	var shipVehicle = {
		renderDate: function(dateObj){ 
			var options = '';
			$.each(dateObj[0], function(key, val){ 
				$.each(val, function(idx, value){ 
					options += '<option value="'+value.Value+'">'+value.Text+'</option>'
				});
			});
			$('#shipVehicleselDay').html(options);
		},
		RenderTable: function(data, Parent) {
			var self = this, TableStr = "";
			Parent.empty();
			

			//Heading
			for(var key in data) {
				TableStr += self.RenderSingleTable(key, data[key]);
			}

			Parent.append(TableStr);
			Parent.find('.R16').mouseover(function(){
				alert('Enter..!');
			});
		},
		RenderSingleTable: function(heading, Data) {
			console.log(Data);
			var SubHeadingStr = "",
				SubSubHeadingStr = "",
				TbodyStr = "",
				SubHeading = Data[0];

			for(var key in SubHeading) {
				SubHeadingStr += '<th class="pad-10" colspan="2">'+ key +'</th>';
				if(Array.isArray(SubHeading[key])) {
					var SubHeadingArray = SubHeading[key][0];
					for(var sub in SubHeadingArray) {
						SubSubHeadingStr += '<th class="pad-10" colspan="1">' +sub+ '</th>';
					}
				} else {
					SubSubHeadingStr += '<th class="pad-10" colspan="2"></th>';
				}
			}
			var rowIdx = 0;
			for(var i = 0; i< Data.length; i++) {
				rowIdx++;
				var Body = "",
					Values = Data[i],
					rowcls = (rowIdx % 2 === 0) ? 'oddCls' : '';

				for(var key in Values) {
					if(Array.isArray(Values[key])) {
						var items = Values[key][0];
						for(var k in items) {
							Body += '<td class="pad-10" align="right">' +items[k]+ '</td>';
						}
					} else {
						Body += '<td class="pad-10" align="right" colspan="2">' +Values[key]+ '</td>';
					}
				}

				TbodyStr += '<tr class="'+rowcls+'">' + Body + '</tr>';
			}

			var Table = '<table class="table">'+
							'<thead class="table_head">'+
								'<tr>'+
									'<th align="left" colspan="14" class="pad-10 main-heading">' + heading + '</th>'+
								'</tr>'+
								'<tr class="visible-lg">'+
								SubHeadingStr+
								'</tr>'+
								'<tr class="visible-lg">'+
								SubSubHeadingStr+
								'</tr>'+
							'</thead>'+
							'<tbody>'+
								TbodyStr+
							'</tbody>'+
						'</table>';


			return Table;
		},
		RenderCarousel: function(data, Parent) {
			var self = this, CarouselStr = "";
			Parent.empty();
			

			//Heading
			for(var key in data) {
				CarouselStr += self.RenderSingleCarousel(key, data[key]);
			}

			Parent.append(CarouselStr);
			self.InitCarousel(Parent);
		},
		InitCarousel: function(Parent) {
			Parent.find('.owl-carousel').owlCarousel({
               loop:false,
               margin:0,
               merge:true,
               nav:false,
			   slideBy: 1,  
               responsive:{
               0:{
               items:1
               },
               678:{
               items:1
               },
               320:{
                items:1
               },
               480:{
                items:1
               },
			   768:{
				items:2
			   },
			   1025:{
				items:3
			   }
               }
            });
		},
		getData: function(key, Data) {
			var ArticleStr = "";
			for(var i = 0; i < Data.length; i++) {
				// ArticleStr += Data[i][]
				var ArticleContent = Data[i][key][0];
				var Str = "";
				for(var k in ArticleContent) {
					Str += '<span class="sub-item">'+ArticleContent[k]+'</span>';
				}
				ArticleStr += '<div class="R16">' + Str + '</div>';
			}
			return ArticleStr;
		},
		getKeyChild: function(key, Object) {
			var ChildStr = "";
			for(var i in Object[0]) {
				ChildStr += "<span class='sub-item'>" + i + "</span>";
			}
			return ChildStr;
		},
		getFixedData: function(key, Data) {
			var FixedStr = "";
			for(var i=0; i < Data.length; i++) {
				FixedStr += '<div class="R16">' + Data[i][key] + '</div>';
			}
			return FixedStr;
		},
		RenderSingleCarousel: function(Name, Data) {
			var FixedPart = "",
				CarouselPart = "",
				Heading = Data[0],
				self = this;
			for(var key in Heading) {
				if(Array.isArray(Heading[key])) {
					CarouselPart += '<div class="article">'+
										'<div class="year_heading">'+
											'<div>'+ key +'</div>'+
											self.getKeyChild(key, Heading[key])+
										'</div>'+
										self.getData(key, Data)+
									'</div>';
				} else {
					FixedPart += '<div class="year_heading">'+ key +'</div>'+
									self.getFixedData(key, Data);
				}
			}

			var Carousel = '<div class="table_head pad-10 clearfix">'+
								'<span>' + Name + '</span>'+
							'</div>'+
							'<div class="clearfix" style="margin-bottom: 1rem; border:1px solid #d1d3d4">'+
								'<div class="states_heading">'+
									FixedPart+
								'</div>'+
								'<div class="owl-wrapper">'+
									'<div class="owl-carousel">'+
										CarouselPart+
									'</div>'+
								'</div>'+
							'</div>';

			return Carousel;
		},
		init: function(dateData, tabledata, id) {
			var self = this, dataObj = tabledata[0];
			this.renderDate(dateData);
			if($(window).width() < 668)
				self.RenderCarousel(dataObj, id);
			else
				self.RenderTable(dataObj, id);
		}
	}

	if($('#shipVehiclePage').length > 0) {
		shipVehicle.init(window.shipVehicleDateOptions, window.shipVehicleTableData, $('#shipVehiclePage'));	
	}	
})();