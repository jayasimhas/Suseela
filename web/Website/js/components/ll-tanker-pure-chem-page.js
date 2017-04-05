(function () {
	// body...
	'use strict';

	var tankerPureChem = {
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
				if(key !== 'Age'){
					SubHeadingStr += '<th class="pad-10" colspan="2" style="text-align: center;">'+ key +'</th>';
				}
				else{
					SubHeadingStr += '<th class="pad-10" colspan="2" style="text-align: left;">'+ key +'</th>';
				}
				if(Array.isArray(SubHeading[key])) {
					var SubHeadingArray = SubHeading[key][0];
					for(var sub in SubHeadingArray) {
						SubSubHeadingStr += '<th class="pad-10" style="text-align: right;">' +sub+ '</th>';
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
						Body += '<td class="pad-10" align="left" colspan="2">' +Values[key]+ '</td>';
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
			self.setColHeight(Parent);
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
											'<div class="topTitle">'+ key +'</div>'+
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
							'<div class="clearfix" style="border:1px solid #d1d3d4; border-top: none; margin-bottom: 1rem;">'+
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
			for (var i = 0; i < parentId.find('.states_heading').length; i++) {
				var stateObj = parentId.find('.states_heading')[i];
				$('.R16', stateObj).each(function (idx) {
					var $this = $(this),
						colHeight = $this.height();
					$('.owl-stage-outer:eq(' + i + ')', parentId).find('.article').each(function () {
						$($(this).find('.R16')[idx]).css('height', colHeight);
					});
				});
			}
		},
		init: function(data, id) {
			var self = this, dataObj = data[0], idx = 0;
			
			for(var prop in dataObj){
				for(var key in dataObj[prop][0]){
					idx++;
				}
				break;
			}
			if($(window).width() < 780 || idx > 5)
				self.RenderCarousel(dataObj, id);
			else
				self.RenderTable(dataObj, id);
			
			$(window).resize(function() {
				self.setColHeight(id);
			});
		}
	}

	if($('#tankerPureChemPage').length > 0) {
		if(typeof window.currentFleetTableData !== 'undefined' && window.currentFleetTableData.length !== 0){
			tankerPureChem.init(window.currentFleetTableData, $('#tankerPureChemPage'));
		}
		else{
			$('#tankerPureChemPage').css('margin-bottom', '1rem');
			$('#tankerPureChemPage').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
		}
		
	}
	if($('#onOrderTable').length > 0) {
		if(typeof window.onOrderTableData !== 'undefined' && window.onOrderTableData.length !== 0){
			tankerPureChem.init(window.onOrderTableData, $('#onOrderTable'));
		}
		else{
			$('#onOrderTable').css('margin-bottom', '1rem');
			$('#onOrderTable').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
		}
	}

	
})();