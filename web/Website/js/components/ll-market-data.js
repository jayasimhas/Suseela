(function () {
	var marketData = {
		renderTable: function(data, renderId){
			this.loadMobileView(data, renderId[0]);
			this.loadDescView(data, renderId[1]);
			this.initiateCarousel();
			this.setColHeight(renderId);
		},
		loadMobileView: function(tableData, id){
			var headObj = tableData[0], indx = 0, titflag = true;
				$.each(headObj, function(key, val){
					if(titflag){
						titflag = false;
						$('.states_heading').append('<div class="title" data-title="'+key+'"><span>'+ key + '</span></div>');
					}
					else{
						$(id).append('<div class="item titleHead" data-title="'+key+'"><div class="title">'+key+'</div></div>');
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
		loadDescView: function(tableData, id){
			var thead = tableData[0], descStr = '<thead class="table_head">', index = 0, indx = 0;
			descStr += "<tr>";
			for(var prop in thead){
				descStr += "<th class='title'>" + prop + "</th>";
			}
			descStr += "</tr>";
			descStr += "</thead>";
			
						  
			$.each(tableData, function(idx, val){
				index++;
				var oddCls = index % 2 == 0 ? 'oddCls' : '';
				descStr += "<tr class='"+oddCls+"'>";
				for(var prop in val){
					var cls = (val[prop].split(' ')[1].indexOf('-') !== -1) ? 'fall' : 'rise';
					if(indx >= 1){
						descStr += "<td class='R16 pad-10'><span class='numData'>"+val[prop].split(' ')[0]+'</span><span class="'+cls+'">'+val[prop].split(' ')[1]+"</span></td>";
					}
					else{
						descStr += "<td class='R16 pad-10'>"+val[prop]+"</td>";
					}
					indx++;
				}
				indx = 0;
				descStr += "</tr>";
			});
			$(id).html(descStr);
		},
		initiateCarousel: function(){
			$('.owl-carousel').owlCarousel({
				loop:true,
				autoPlay: false,
				nav: true,
				navContainer: '#customNav',
				dotsContainer: '#customDots',
				slideBy: 1,
				responsive:{
					0:{
					items:4
					},
					678:{
					items:3
					},
					320:{
					 items:1
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
			marketData.init(window.jsonBalticIndices, $('.marketDataTable'));
		}
	});
})();