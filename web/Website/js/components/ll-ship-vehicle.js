(function () {
	// body...
	'use strict';

	var shipVehicle = {
		RenderTable: function(data, Parent) {
			var self = this, TableStr = "";
			Parent.empty();
			

			//Heading
			for(var key in data) {
				TableStr += self.RenderSingleTable(key, data[key]);
			}

			Parent.append(TableStr);

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

			for(var i = 0; i< Data.length; i++) {
				var Body = "",
					Values = Data[i];

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

				TbodyStr += '<tr>' + Body + '</tr>';
			}

			var Table = '<table class="table theme-table">'+
							'<thead>'+
								'<tr>'+
									'<th align="left" colspan="14" class="pad-10 main-heading">' + heading + '</th>'+
								'</tr>'+
								'<tr>'+
								SubHeadingStr+
								'</tr>'+
								'<tr>'+
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
								'<span class="RB16">' + Name + '</span>'+
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
		init: function(data, id) {
			var self = this;
			if($(window).width() > 667)
				self.RenderTable(data[0], id);
			else
				self.RenderCarousel(data[0], id);
		}
	}

	if($('#shipVehicle').length > 0) {
		shipVehicle.init(window.jsonShipVehicle, $('#shipVehicle'));	
	}
	

	
})();