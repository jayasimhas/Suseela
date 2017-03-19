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
					if(key !== 'Route'){
					HeadingStr += "<th class='headerPad rigalign'>"+
										"<div>&nbsp;</div>"+
										"<div>"+ key.split("|")[0] +"</div>"+
									"</th>";
					}
					else{
						HeadingStr += "<th class='headerPad'>"+
										"<div>&nbsp;</div>"+
										"<div>"+ key.split("|")[0] +"</div>"+
									"</th>";
					}
				} else {
					HeadingStr += "<th class='headerPad rigalign'>"+
										"<div>"+ key.split("|")[0] +"</div>"+
										"<div>"+ key.split("|")[1] +"</div>"+
									"</th>";
				}
			}
			var rowIdx = 0, cls = '';
			for(var i = 0; i < Data.length; i++) {
				rowIdx++;
				cls = (rowIdx % 2 === 0) ? 'oddCls' : '';
				var EachValue = Data[i],
					Td = "",
					align = "";
				for(var key in EachValue) { 
					if(key !== "Route") {
						align = "rigalign";
					}
					Td += '<td colspan="1" class="borderpad '+align+'">' + EachValue[key] + '</td>';
				}
				TbodyStr += '<tr class="'+cls+'">' +Td + '</tr>';
			}

			var Table = '<table class="table">'+
							'<thead class="table_head">'+
								'<tr class="blueBg">'+
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
				ArticleStr += '<div class="R16 borderpad rigalign">' + Data[i][key] + '</div>';
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
               },
               1024:{
                items:2
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
										'<div class="title headerPad rigalign">'+
											key +
										'</div>'+
										self.getData(key, Data)+
									'</div>';
				} else {
					FixedPart += '<div class="title">'+ key +'</div>'+
									self.getFixedData(key, Data);
				}
			}

			var Carousel = '<div class="table_head pad-10 clearfix"></div>'+
							'<div class="clearfix" style="margin-bottom: 1rem; border:1px solid #d1d3d4; border-top: none;">'+
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
			if($(window).width() > 1024){
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