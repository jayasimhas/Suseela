(function () {
	var howeRobinson = {
		renderTable: function(tableObj, renderEle){
			var tableStr = '';
			tableStr += this.loadDesktopView(tableObj, renderEle);
			this.initiateCarousel(renderEle);
			this.setColHeight(renderEle);
		},
		loadDesktopView: function(tableData, renderEle){
			var getTitles = [], self = this, indx = 0, titflag = true, clsIdx = 0;
			$.each(tableData, function(idx, val){
				$.each(val, function(prop, value){
					getTitles.push(prop); 
				});
			});
			 
			for(var i=0; i<getTitles.length; i++){ 
				if(titflag){
					titflag = false;
					$('.states_heading').append('<div class="title">&nbsp;</div>');
				}
				else{
					$(renderEle).append('<div class="item titleHead" data-title="'+getTitles[i]+'"><div class="title headerPad rigalign">'+getTitles[i]+'</div></div>');
				} 
			}
			
			$.each(tableData, function(idx, val){
				$.each(val, function(key, arr){
					for(var i=0; i<arr.length; i++){
						clsIdx++;
						var oddCls = clsIdx % 2 === 0 ? 'oddCls' : '';
						if(indx >= 1){
							$('div[data-title="'+key+'"]').append('<div class="R16 borderpad rigalign '+oddCls+'">'+((arr[i]) ? arr[i] : '&nbsp;')+'</div>');
						}
						else{
							$('.states_heading').append('<div class="R16 leftbord '+oddCls+'">'+((arr[i]) ? arr[i] : '&nbsp;')+'</div>');
						}
					}
					indx++;
				});
			});
		},
		setColHeight: function(parentId){
			$(parentId).closest('#howeRobinsonContainer').find('.states_heading .R16').each(function(idx){
				var $this = $(this), colHeight = $this.height();
				$('.titleHead').each(function() {
					$($(this).find('.R16')[idx]).height(colHeight);
				});
			});
		},
		initiateCarousel: function(id){
			$(id).owlCarousel({
				loop:false,
				margin:0,
				merge:true,
				nav:false,
				slideBy: 4,
				responsive: {
				   0:{
				   items:4
				   },
				   678:{
				   items:2
				   },
				   320:{
					items:2
				   },
				   480:{
					items:2
				   },
				   1024:{
				   items:3
				   }
				}
			});
		},
		init: function(tableObj, renderEle) {
			var self = this;
			self.renderTable(tableObj, renderEle);
			$(window).resize(function() {
				self.setColHeight(renderEle);
			});
		}
	}
	
	$(document).ready(function() {
		if($('#howeRobinsonContainer').length > 0) {
			howeRobinson.init(jsonHoweRobinson, '.howeRobinsonTable');
		}
	});
})();