(function () {
	// body...
	'use strict';

	var ResponsiveTable = {
		GetAjaxData: function(data, id) {
			var self = this;
			if(data) {
				window.ResponsiveJSON = data;
				window.ResponsiveModalJSON = data;
			 	self.RenderCarousel(data, id);
			 	self.RenderModal(data, id);
				//} 
			}
		},
		RenderModal: function(data, id) {
			var ModalId = $(id).attr('data-modal'),
				Parent = $('#' + ModalId),
				HeaderData = data[0],
				Header = "",
				category = "";

			
			Parent.find('.table').empty();
			for(var key in HeaderData) {
				if(key !== "ID") {
					Header+="<div class='tableHead'><strong>" + key + "</strong><a href='#' class='sort' category='" +category +"' type='ascending'></a><a href='#' class='sort' category='" +category +"' type='descending'></a></div>";
				}
			}
			Parent.find('.table').append('<div class="tableRow">' + Header + '</div>');
			for(var key in data) {
				var Item = data[key],
					Template = "";
				for(var val in Item) {
					var content = "";
					if(val !== "ID") {
						if(Array.isArray(Item[val])) {
							content = Item[val][0].value;
						} else {
							content = Item[val];
						}
						Template += "<div class='tableCell'>" + content + "</div>";
					}
				}
				Parent.find('.table').append('<div class="tableRow">' + Template + '</div>');
			}

		},
		RenderCarousel: function(data, Parent) {
			
			Parent.find('.owl-carousel').remove();
			Parent.find('.states_heading').find('.RB16').remove();
			Parent.find('.states_heading').after('<div class="owl-carousel"></div>');
			var CreateList = window.jsonMappingData;

			for(var key in CreateList) {
				Parent.find('.owl-carousel').append('<div class="article" data-head="' +CreateList[key].Key+ '"><div class="year_heading"><span>' + CreateList[key].Value + '</span><a href="#" class="sort" type="ascending"><svg class="sorting-arrows__arrow sorting-arrows__arrow--up"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#sort-down-arrow"></use></svg></a><a href="#" class="sort" type="descending"><svg class="sorting-arrows__arrow sorting-arrows__arrow--down"><use xmlns:xlink="http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#sort-down-arrow"></use></svg></a></div></div>');
			}
			var Items = Parent.find('.owl-carousel').find('.article');

			for(var i = 0; i < data.length; i++) {
				var Item = data[i], index = i,
					CompanyLink = data[i].Company.toLowerCase().split(" ").join("-") + ".htm";
				Parent.find('.states_heading').append('<div class="RB16"><a href="' + CompanyLink + '">' +data[i].Company+ '</a></div>');
				for(var key in Item) {
					if(Array.isArray(Item[key])) {
						Parent.find('.article[data-head="' +key+ '"]').append('<div  class="R16 TableRow'+index+'">' +Item[key][0].value+ '</div>');
					}
				}
			}
			this.InitiateCarousel(Parent);
			this.HeightManagement(Parent);
			
		},
		HeightManagement: function(Parent) {
			var HeadingItems = Parent.find('.year_heading'),
				MaxHeadingHeight = 0;

			Parent.find('.states_heading .RB16').each(function(key){
				var Height = $(this).height(),
					Item = [];

					$('.article').each(function() {
						var CurrentItem = $(this).find('.R16')[key];
						Item.push(CurrentItem);
					});


				for(var i = 0; i < Item.length; i++) {
					$(Item[i]).height(Height);
				}
			});
			// HeadingItems.height(0);
			HeadingItems.each(function() {
				var thisHeight = $(this).height();
				if(thisHeight > MaxHeadingHeight) {
					MaxHeadingHeight = thisHeight;
				}
			});
			$(HeadingItems).find('span').height(MaxHeadingHeight);
		},
		InitiateCarousel: function(Parent) {

			
			Parent.find('.owl-carousel').owlCarousel({
               loop:true,
               margin:10,
               merge:true,
               nav:true,
               navText: [
               	  "<img src='/dist/img/lessthan.png'/>",
               	  "<img src='/dist/img/greaterthan.png'/>"
               	  ],
			   slideBy: 3,  
               responsive:{
               0:{
               items:3
               },
               678:{
               items:3
               },
               320:{
                items:2
               },
               480:{
                items:2
               },
               1000:{
               items:3
               }
               }
            });
        },
        ModalEvents: function() {
        	$(document).on('click', 'a[data-toggle="modal-financialresults"]', function(e) {
        		e.preventDefault();
        		$('#modal-financialresults').show();
        	});
        	$(document).on('click', '#modal-financialresults .table_close', function(e) {
        		e.preventDefault();
        		$(this).parents('.ID-responsive-table-modal').hide();
        	});
        	$(document).on('click', '.ID-responsive-table-modal', function(e) {
    			if($(e.target).parents('.container').length > 0 || $(e.target).hasClass('.container')) {
    				return false;
    			} else {
    				$(this).hide();
    			}
        	});
        },
        SortingModal: function(id) {
        	var self = this;

        	$(document).on('click','#modal-table .sort', function(e) {
        		e.preventDefault();
        		var MainData = window.ResponsiveJSON,
        			Index = $(this).parents('.tableHead').index(),
        			Items = [],
        			type = $(this).attr('type'),
        			Category = $(this).attr('category'),
        			ModalData = window.ResponsiveModalJSON,
        			UpdatedJson = [],
        			HeadingText = $(this).parents('.tableHead').find('strong').text();

        		$('#modal-table .tableRow').each(function() {
        			if($(this).find('.tableCell').length > 0) {
	        			var Text = $($(this).find('.tableCell')[Index]).text();
	        			if(HeadingText == 'Company') {
		        			Items.push(Text);
		        		} else {
		        			Items.push(parseFloat(Text));
		        		}
	        			
	        		}
        		});

        		if(HeadingText == 'Company') {
					if(type == "descending") {
						Items.sort().reverse();
					} else {
						Items.sort();
					}

					for(var key in Items) {
						for(var json in ModalData) {
							if(Items[key] == ModalData[json].Company) {
								UpdatedJson.push(ModalData[json]);
							}
						}
					}
				} else {
					if(type == "descending") {
						Items.sort(function(a, b){
						  return b - a;
						});
					} else {
						Items.sort(function(a, b){
						  return a - b;
						});
					}

					for(var key in Items) {
						for(var json in ModalData) {
							if(Items[key] == ModalData[json][HeadingText][0].value) {
								UpdatedJson.push(ModalData[json]);
							}
						}
					}
				}
				window.ResponsiveModalJSON = UpdatedJson;

				self.RenderModal(window.ResponsiveModalJSON, id);
        	});
        },
        SortingFunctionality: function(id) {
        	//Sorting Functionality
        	var self = this;
			$(document).on('click', '.year_heading .sort', function(e) {
				e.preventDefault();
				var Parent = $(this).parents('.article'),
					Values = Parent.find('.R16'),
					Content = Parent.attr('data-head'),
					type = $(this).attr('type'),
					category = $(this).attr('category'),
					Items = [],
					CarouselControl = $(this).parents('.ID-Responsive-Table').find('.owl-controls').find('.owl-dots'),
					ControlIndex = CarouselControl.find('.active').index(),
					CarouselStyles = $('#financialresults .owl-stage').attr('style'),
					OwlItems = $('#financialresults .owl-stage').find('.owl-item'),
					ClonedItems = [],
					ActiveItems = [];

				OwlItems.each(function() {
					if($(this).hasClass('cloned')) {
						ClonedItems.push($(this).index());
					}
					if($(this).hasClass('active')) {
						ActiveItems.push($(this).index());
					}
				});
				$('.year_heading .sort').removeClass('active');
				$(this).addClass('active');

				if(category == 'companies') {
					var CompanyNames = $(this).parents('.states_heading').find('.RB16');

					CompanyNames.each(function() {
						Items.push($(this).text());
					});
					if(type == "descending") {
						Items.sort().reverse();
					} else {
						Items.sort();
					}
				} else {
					Values.each(function() {
						Items.push(parseFloat($(this).text()));
					});
					if(type == "descending") {
						Items.sort(function(a, b){
						  return b - a;
						});
					} else {
						Items.sort(function(a, b){
						  return a - b;
						});
					}
				}
				
				self.RecreateObject(Content, Items, window.ResponsiveJSON, id, category);
				
				$('#financialresults .owl-stage').attr('style', CarouselStyles);
				$('#financialresults .owl-stage .owl-item').removeClass('cloned');
				$('#financialresults .owl-stage .owl-item').removeClass('active');
				for(var key in ClonedItems) {
					$($('#financialresults .owl-stage .owl-item')[ClonedItems[key]]).addClass('cloned');
				}

				for(var key in ActiveItems) {
					$($('#financialresults .owl-stage .owl-item')[ActiveItems[key]]).addClass('active');
				}

				$('#financialresults .owl-dot').removeClass('active');
				$($('#financialresults .owl-dot')[ControlIndex]).addClass('active');
				$('#financialresults .article[data-head="' + Content + '"] .sort[type="' + type + '"]').addClass('active');
			});
        },
        RecreateObject: function(Content, SortedItem, MainArray, id, category, modal) {
        	var self = this, RecreatedArray = [];
        	// id.find('.RB16').remove();
        	if(category === 'companies') {
        		for(var i = 0; i < SortedItem.length; i++) {
	        		for(var key in MainArray) {
	        			var Name = MainArray[key].Company;
	        			if(Name == SortedItem[i]) {
	        				RecreatedArray.push(MainArray[key]);
	        			}
	        		}
	        	}
        	} else {
	        	for(var i = 0; i < SortedItem.length; i++) {
	        		for(var key in MainArray) {
	        			var _Object = MainArray[key];
	        			if(_Object[Content][0].value == SortedItem[i]) {
	        				RecreatedArray.push(_Object);
	        			}
	        		}
	        	}
	        }
        	// $owl.trigger('destroy.owl.carousel');
        	
        	if(modal) {
        		window.ResponsiveModalJSON = RecreatedArray;
        		self.RenderModal(window.ResponsiveModalJSON, id);
        	} else {
        		window.ResponsiveJSON = RecreatedArray;
	        	self.RenderCarousel(window.ResponsiveJSON, id);
	        }
        },
		init: function(data, id) {
			var self = this;
			self.GetAjaxData(data, id);
			self.ModalEvents();
			self.SortingFunctionality(id);
			self.SortingModal(id);
		}
	}

	if($('#financialresults').length > 0) {
		ResponsiveTable.init(window.jsonResultFinancial, $('#financialresults'));	
	}
	

	
})();