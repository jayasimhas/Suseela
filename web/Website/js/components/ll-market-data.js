(function () {
	var marketData = {
		recreateArr: [],
		renderTable: function(dataObj, renderId){
			console.log('Length of object is: ' + dataObj.length);
			for(var i=0; i<dataObj.length; i++){
				var filterObj = Object.keys(dataObj[i]), filterKeys = filterObj.sort().reverse(), createObj = {};
				for(var j=0; j<filterKeys.length; j++){
					for(var prop in dataObj[i]){
						if(prop == filterKeys[j]){
							createObj[prop] = dataObj[i][prop]; 
						}
					}
				}
				this.recreateArr.push(createObj);
			}
			this.loadMobileView(this.recreateArr, renderId[0]);
			this.loadDescView(this.recreateArr, renderId[1]);
			this.initiateCarousel(renderId[0]);
			this.setColHeight(renderId);
		},
		loadMobileView: function(tableData, id){
			var self = this, headObj = tableData[0], indx = 0, titflag = true, colInd = 0;
				$.each(headObj, function(key, val){
					colInd++;
					if(colInd <= 6){
						if(titflag){
							titflag = false;
							$('.states_heading').append('<div class="title" data-title="'+key+'"><span>'+ key + '</span></div>');
						}
						else{
							$(id).append('<div class="item titleHead" data-title="'+key+'"><div class="title">'+self.genarateDate(key)+'</div></div>');
						}
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
			var self = this, thead = tableData[0], descStr = '<thead class="table_head">', index = 0, indx = 0, colHeadInd = 0, colInd = 0, titflag = true;
			descStr += "<tr>";
			for(var prop in thead){
				colHeadInd++;
				if(colHeadInd <= 6){
					if(titflag){
						titflag = false;
						descStr += "<th class='title'><div class='pad'>" + prop + "</div></th>";
					}
					else{
						descStr += "<th class='title'><div class='pad'>" + self.genarateDate(prop) + "</div></th>";
					}
				}
				else break;
			}
			descStr += "</tr>";
			descStr += "</thead>";
			
						  
			$.each(tableData, function(idx, val){
				index++;
				var oddCls = index % 2 == 0 ? 'oddCls' : ''; 
				descStr += "<tr class='"+oddCls+"'>";
				for(var prop in val){
					var cls = (val[prop].split(' ')[1].indexOf('-') !== -1) ? 'fall' : 'rise';
					colInd++;
					if(colInd <= 6){
						if(indx >= 1){
							descStr += "<td class='R16 pad-10'><div class='pad'><span class='numData'>"+val[prop].split(' ')[0]+'</span><span class="'+cls+'">'+val[prop].split(' ')[1]+"</span></div></td>";
						}
						else{
							descStr += "<td class='R16 pad-10'><div class='pad'>"+val[prop]+"</div></td>";
						}
						indx++;
					}
				}
				colInd = 0;
				indx = 0;
				descStr += "</tr>";
			});
			$(id).html(descStr);
		},
		genarateDate(date){
			if(date.split(' ').length === 2){
				var getDate = date.split(' ')[0], 
					splDate = getDate.split('-'),
					getObjDate = new Date(splDate[0], splDate[1]-1, splDate[2]),
					getObjDateSpl = getObjDate.toString().split(' ');
					return getObjDateSpl[2] + ' ' + getObjDateSpl[1] + ' ' + getObjDateSpl[3];
			}
			else{
				return date.split('-').reverse().join(' ');
			}
		},
		initiateCarousel: function(id){
			$(id).owlCarousel({
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
					items:2
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
		init: function(dataObj, renderId) {
			var self = this;
			self.renderTable(dataObj, renderId);
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