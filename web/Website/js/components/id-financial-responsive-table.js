(function () {
	// body...
	'use strict';

	var ResponsiveFinancialTable = {
		LastItem: null,
		FirstItem: null,
		RenderCarousel: function(data, Parent) {
			Parent.find('.owl-carousel').remove();
			Parent.find('.states_heading').parent().append('<div class="owl-wrapper"><div class="owl-carousel"></div></div>');
			var self = this,
				Header = data[0].Header,
				Values = data[0].Values,
				StatesHeading = Parent.find('.states_heading'),
				Carousel = Parent.find('.owl-carousel');

			self.FirstItem = data[0].Header[1];
			self.LastItem = data[0].Header[data[0].Header.length - 1];
			Parent.find('.states_heading').empty();
			for(var key in Header) {
				if(key == 0) {
					StatesHeading.append('<div class="year_heading">' + Header[key] + '</div>');
				} else {
					Carousel.append('<div class="article"><div class="year_heading" data-head="' +Header[key]+ '">' + Header[key] + '</div></div>');
				}
			}

			for(var key in Values) {
				var CurrentValue = Values[key];
				for(var item in CurrentValue) {
					if(item == 0) {
						StatesHeading.append('<div class="RB16">' +CurrentValue[item]+ '</div>');
					} else {
						$(Parent.find('.article')[item-1]).append('<div class="R16">' +CurrentValue[item]+ '</div>');
					}
				}
			}
			self.InitateCarousel(Parent);
			self.HeightManagement(Parent);
			self.ChangeStateEvents(Parent);
		},
		ChangeStateEvents: function(Parent) {
			var OwlNext = Parent.find('.owl-next'),
				OwlPrevious = Parent.find('.owl-previous'),
				self = this;

			$('.owl-prev').addClass('disabled');
			$(document).on('click','.owl-prev, .owl-next', function() {
				setTimeout(function() {
					var ActiveElements = Parent.find('.owl-item.active .year_heading'),
						ActiveElementsTexts = [];

					ActiveElements.each(function() {
						ActiveElementsTexts.push($(this).text().trim());
					});
					$('.owl-prev, .owl-next').removeClass('disabled');
					if(self.FirstItem.trim() == ActiveElementsTexts[0]) {
						$('.owl-prev').addClass('disabled');
					} else {
						$('.owl-prev').removeClass('disabled');
					}
					if(self.LastItem.trim() == ActiveElementsTexts[ActiveElementsTexts.length - 1]) {
						$('.owl-next').addClass('disabled');
					} else {
						$('.owl-next').removeClass('disabled');
					}
				}, 400);
			});
 		},
		HeightManagement: function(Parent) {
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
			})
		},
		RenderModal: function(data, Parent) {
			var Header = data[0].Header,
				Values = data[0].Values,
				FinanceModal = $('#modal-financialresults'),
				ModalTable = FinanceModal.find('.table');

			ModalTable.append('<div class="tableRow"></div>');
			for(var key in Header) {
				ModalTable.find('.tableRow').append('<div class="tableHead"><strong>' +Header[key]+ '</strong></div>');
			}

			for(var key in Values) {
				ModalTable.append('<div class="tableRow"></div>');
				var CurrentValue = Values[key];
				for(var item in CurrentValue) {
					ModalTable.find('.tableRow:last-child').append('<div class="tableCell">' +CurrentValue[item]+ '</div>');
				}
			}

		},
		InitateCarousel: function(Parent) {
			Parent.find('.owl-carousel').owlCarousel({
               loop:true,
               merge:true,
               margin:0,
               nav:false,
               onDragged: function() {
               		var ActiveElements = Parent.find('.owl-item.active .year_heading'),
						ActiveElementsTexts = [];

					ActiveElements.each(function() {
						ActiveElementsTexts.push($(this).text().trim());
					});
					$('.owl-prev, .owl-next').removeClass('disabled');
					if(self.FirstItem.trim() == ActiveElementsTexts[0]) {
						$('.owl-prev').addClass('disabled');
					} else {
						$('.owl-prev').removeClass('disabled');
					}
					if(self.LastItem.trim() == ActiveElementsTexts[ActiveElementsTexts.length - 1]) {
						$('.owl-next').addClass('disabled');
					} else {
						$('.owl-next').removeClass('disabled');
					}
               },
			   slideBy: 1,  
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
               items:5
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
        RenderTable: function(data, Parent) {
    	 	Parent.find('.states_heading').parent().remove();
		 	Parent.append('<div class="table-wrapper"><div class="table"></div></div>');
		 	var Wrapper = $('#financialresults .table'),
		 		Header = data[0].Header,
		 		Values = data[0].Values;

		 	Wrapper.append('<div class="tableRow"></div>');
			
			for(var key in Header) {
				Wrapper.find('.tableRow:last-child').append('<div class="tableHead">' +Header[key]+ '</div>');
			}

			for(var keyItem in Values) {
				var Items = Values[keyItem];
				Wrapper.append('<div class="tableRow"></div>');
				for(var item in Items) {
					Wrapper.find('.tableRow:last-child').append('<div class="tableCell">' +Items[item]+ '</div>');
				}
			}
        },
		init: function(data, Parent) {
			var self = this,
				windowSize = $( document ).width();

			if(windowSize > 736) {
				self.RenderTable(data, Parent);
			} else {
				self.RenderCarousel(data, Parent);
			}
			self.RenderModal(data, Parent);
			self.ModalEvents();
		}

	}

	if($('#financialresults').length > 0) {
		ResponsiveFinancialTable.init(window.jsonResultFinancial, $('#financialresults'));	
	}
})();