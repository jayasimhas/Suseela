(function () {
	var marketData = {
		renderTable: function(data, renderId){
			this.loadTableView(data, renderId);
			this.initiateCarousel();
			this.setColHeight(renderId);
		},
		loadTableView: function(tableData, id){
			var headObj = tableData[0], indx = 0, titflag = true;
				$.each(headObj, function(key, val){
					if(titflag){
						titflag = false;
						$('.states_heading').append('<div class="title" data-title="'+key+'"><span>'+ key + '</span></div>');
					}
					else{
						id.append('<div class="item titleHead" data-title="'+key+'"><div class="title">'+key+'</div></div>');
					}
				});
				
				$.each(tableData, function(idx, val){
					var oddCls = idx % 2 !== 0 ? 'oddCls' : '';
					$.each(val, function(k, v){
						var cls = (v.split(' ')[1].indexOf('-') !== -1) ? 'fall' : 'rise';
						
						if(indx >= 1){
							$('div[data-title="'+k+'"]').append('<div class="R16 leftbord '+oddCls+'"><span class="numData">'+v.split(' ')[0]+'</span><span class="'+cls+'">'+v.split(' ')[1]+'</span></div>');
						}
						else{
							$('.states_heading').append('<div class="R16 leftbord '+oddCls+'">'+v+'</div>');
						}
						indx++;
					});
					indx = 0; 
				}); 
		},
		initiateCarousel: function(){
			$('.owl-carousel').owlCarousel({
				loop:true,
				autoPlay: false,
				nav: true,
				navContainer: '#customNav',
				dotsContainer: '#customDots',
				slideBy: 2,
				responsive:{
					0:{
					items:4
					},
					678:{
					items:6
					},
					320:{
					 items:2
					},
					480:{
					 items:2
					},
					1000:{
					items: 6
					}
				}
			});
		},
		setColHeight: function(parentId){
			parentId.closest('#main-carousel').find('.states_heading .R16').each(function(idx){
				var $this = $(this), colHeight = $this.height();
				$('.titleHead').each(function() {
					$($(this).find('.R16')[idx]).height(colHeight);
				});
			});
		},
		init: function(data, renderId) {
			var self = this;
			self.renderTable(data, renderId);
			$(window).resize(function() {
				self.setColHeight(renderId);
			})
		}
	}
	
	$(document).ready(function() {
		if($('#market-data').length > 0) {
			marketData.init(window.jsonBalticIndices, $('#marketDataTable'));
		}
	});
})();