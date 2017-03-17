(function () {
	// body...
	'use strict';

	var marketDataDryCargoSsyAtl = {
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
				if(key.split("|").length === 1) {
					HeadingStr += "<th class='pad-full-10' colspan='1'>"+
										"<div class='text-center'>&nbsp;</div>"+
										"<div class='text-center'>"+ key.split("|")[0] +"</div>"+
									"</th>";
				} else {
					HeadingStr += "<th class='pad-full-10' colspan='1'>"+
										"<div class='text-center'>"+ key.split("|")[0] +"</div>"+
										"<div class='text-center'>"+ key.split("|")[1] +"</div>"+
									"</th>";
				}
			}

			for(var i = 0; i < Data.length; i++) {
				var EachValue = Data[i],
					Td = "",
					align = "right";
				for(var key in EachValue) {
					if(key == "Route") {
						align = "left";
					}
					Td += '<td colspan="1" class="pad-full-10" align="' + align + '">' + EachValue[key] + '</td>';
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
		RenderCarousel: function(data, Parent) {
			var self = this, CarouselStr = "";
			Parent.empty();
			

			//Heading
			for(var key in data[0]) {
				CarouselStr += self.RenderSingleCarousel(key, data[0][key]);
			}

			Parent.append(CarouselStr);
			self.InitCarousel(Parent);
			
			self.setColHeight(Parent);
		},
		getFixedData: function(key, Data) {
			var FixedStr = "";
			for(var i = 0; i < Data.length; i++) {
				FixedStr += '<div class="R16">' + Data[i][key] + '</div>';
			}
			return FixedStr;
		},
		getData: function(key, Data) {
			var ArticleStr = "";
			for(var i = 0; i < Data.length; i++) {
				// ArticleStr += Data[i][]
				ArticleStr += '<div class="R16">' + Data[i][key] + '</div>';
			}
			return ArticleStr;
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
		RenderSingleCarousel: function(Name, Data) {
			var FixedPart = "",
				CarouselPart = "",
				Heading = Data[0],
				self = this,
				i = 0;
			for(var key in Heading) {
				if(key != 'Route') {
					CarouselPart += '<div class="article">'+
										'<div class="year_heading">'+
											key +
										'</div>'+
										self.getData(key, Data)+
									'</div>';
				} else {
					FixedPart += '<div class="year_heading">'+ key +'</div>'+
									self.getFixedData(key, Data);
				}
			}

			var Carousel = '<div class="table_head pad-10 clearfix">'+
								'<span>&nbsp;</span>'+
							'</div>'+
							'<div class="clearfix" style="margin-bottom: 1rem; border:1px solid #d1d3d4;">'+
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
		setColHeight: function(parentId){
			parentId.find('.states_heading .R16').each(function(idx){
				var $this = $(this), colHeight = $this.height();
				$('.article').each(function() {
					$($(this).find('.R16')[idx]).css('height', colHeight);
				});
			});
		},
		init: function(data, id) {
			var self = this;
			if($(window).width() > 668){
				self.RenderTable(data, id);
			}
			else{
				self.RenderCarousel(data, id);
			}
			
			$(window).resize(function() {
				self.setColHeight(id);
			});
		}
	}
	
	if($('#marketDataDryCargo').length > 0) {
		marketDataDryCargoSsyAtl.init(window.jsonMarketdataDryCargo, $('#marketDataDryCargo'));	
	}

	
})();