(function () {
	// body...
	'use strict';

	var ResponsiveTable = {
		GetAjaxData: function(url, type, id) {
			var self = this;
			$.ajax({ 
				url: url,
				type: type,
				success: function (data) { 
					//if (data.articles && typeof data.articles === "object" && data.articles.length >= 3) {
						window.ResponsiveJSON = data;
					 	self.RenderCarousel(data, id);
					 	self.RenderModal(data, id);
					//} 
				}
			});
		},
		RenderModal: function(data, id) {
			var ModalId = $(id).attr('data-modal'),
				Parent = $('#' + ModalId),
				HeaderData = data[0],
				Header = "";

			for(var key in HeaderData) {
				if(key !== "ID") {
					Header+="<div class='tableHead'><strong>" + key + "</strong></div>";
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
			var CreateList = data[0];

			for(var key in CreateList) {
				if(Array.isArray(CreateList[key])) {
					Parent.find('.owl-carousel').append('<div class="article" data-head="' +key+ '"><div class="year_heading">' + key + '<a href="#" class="sort" type="ascending"></a><a href="#" class="sort" type="descending"></a></div></div>');
				}
			}
			var Items = Parent.find('.owl-carousel').find('.article');

			for(var i = 0; i < data.length; i++) {
				var Item = data[i], index = i;
				Parent.find('.states_heading').append('<div class="RB16">' +data[i].Company+ '</div>');
				for(var key in Item) {
					if(Array.isArray(Item[key])) {
						Parent.find('.article[data-head="' +key+ '"]').append('<div  class="R16 TableRow'+index+'">' +Item[key][0].value+ '</div>');
					}
				}
			}
			this.InitiateCarousel(Parent);
			this.HeightManagement(Parent);
			this.SortingFunctionality(Parent);
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
                items:3
               },
               480:{
                items:3
               },
               1000:{
               items:3
               }
               }
            });
        },
        ModalEvents: function() {
        	$(document).on('click', 'a[data-toggle="modal-table"]', function(e) {
        		e.preventDefault();
        		$('#modal-table').show();
        	});
        	$(document).on('click', '#modal-table .table_close', function(e) {
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
        SortingFunctionality: function(id) {
        	//Sorting Functionality
        	var self = this;
			$(document).on('click', '.year_heading .sort', function(e) {
				e.preventDefault();
				var Parent = $(this).parents('.article'),
					Values = Parent.find('.R16'),
					Content = Parent.attr('data-head'),
					type = $(this).attr('type'),
					Items = [];

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
				
				self.RecreateObject(Content, Items, window.ResponsiveJSON, id);
			});
        },
        RecreateObject: function(Content, SortedItem, MainArray, id) {
        	var self = this, RecreatedArray = [];
        	// id.find('.RB16').remove();
        	
        	for(var i = 0; i < SortedItem.length; i++) {
        		for(var key in MainArray) {
        			var _Object = MainArray[key];
        			if(_Object[Content][0].value == SortedItem[i]) {
        				RecreatedArray.push(_Object);
        			}
        		}
        	}
        	// $owl.trigger('destroy.owl.carousel');
        	window.ResponsiveJSON = RecreatedArray;
        	self.RenderCarousel(window.ResponsiveJSON, id);
        },
		init: function(url, id) {
			var self = this;
			self.GetAjaxData(url, 'GET', id);
			self.ModalEvents();
		}
	}

	$('.ID-Responsive-Table').each(function() {
		var Url = $(this).data('url');
		ResponsiveTable.init(Url, $(this));	
	})
	

	
})();