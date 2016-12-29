(function () {
	// body...
	'use strict';

	var ResponsiveFinancialTable = {
		RenderCarousel: function(data, Parent) {
			var self = this,
				Header = data[0].Header,
				Values = data[0].Values,
				StatesHeading = Parent.find('.states_heading'),
				Carousel = Parent.find('.owl-carousel');
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
				FinanceModal = $('#modal-table-finance'),
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
               margin:10,
               merge:true,
               nav:true,
               navText: [
               	  "<img src='/dist/img/lessthan.png'/>",
               	  "<img src='/dist/img/greaterthan.png'/>"
               	  ],
			   slideBy: 2,  
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
               items:4
               }
               }
            });
        },
        ModalEvents: function() {
        	$(document).on('click', 'a[data-toggle="modal-table-finance"]', function(e) {
        		e.preventDefault();
        		$('#modal-table-finance').show();
        	});
        	$(document).on('click', '#modal-table-finance .table_close', function(e) {
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
		init: function(data, Parent) {
			var self = this;
			self.RenderCarousel(data, Parent);
			self.RenderModal(data, Parent);
			self.ModalEvents();
		}

	}

	if($('#ID-Financial-Responsive-Table').length > 0) {
		ResponsiveFinancialTable.init(window.jsonResultAnnual, $('#ID-Financial-Responsive-Table'));	
	}
})();