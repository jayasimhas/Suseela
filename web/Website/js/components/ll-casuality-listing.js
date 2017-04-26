(function () {
	var CasualityListing = {
		HeaderLinks: [],
		JumpToArray: [],
		DesktopVersion: function(data, Parent) {
			//Header
			Parent.append('<thead class="table_head"></thead>');

			var Header = Parent.find('thead.table_head'),
				HeaderItems = this.HeaderLinks,
				self = this;
			self.JumpToArray = [];
			Header.append('<tr class="visible-lg"></tr>');

			for(var headItem in HeaderItems) {
				Header.find('tr').append('<th class="p-10">'+HeaderItems[headItem]+'</th>');
			}
			//Body
			Parent.append('<tbody class="visible-lg"></tbody>');
			
			var Wrapper = Parent.find('tbody.visible-lg');
			for(var key in data) {
				//Appending Heading
				$('#jumpTo').append('<option value="'+data[key].casualtytitle+'">'+data[key].casualtytitle+'</option>');
				self.JumpToArray.push(data[key].casualtytitle);
				Wrapper.append('<tr data-jump="'+data[key].casualtytitle+'"><td colspan="2" class="graybg RB18 p-10"> '+data[key].casualtytitle+'</td><td colspan="1" align="right" class="graybg RB18 p-10"><a href="javascript: void(0);" class="top"><span class="arrow"></span>Top</a></td></tr>');

				//Appending Body
                var CasualityData = data[key].casualtyData;
                for(var item in CasualityData) {
                	Wrapper.append('<tr><td class="RB16 pad-10"><a href="'+ $('#casualtyDetailUrl').val()+ '?incidentId='+ CasualityData[item].incidentId +'">'+CasualityData[item].Title+'</a></td><td class="R16 pad-10">'+CasualityData[item]["Date Of Incident"].split(' ')[0].split('-').reverse().join('.')+'</td><td class="R16 pad-10">'+CasualityData[item]["Area"]+'</td></tr>');
                }
			}

			

			
		},
		MobileVersion:function(data, Parent) {
			Parent.append('<tbody class="visible-sm"></tbody>');

			var Wrapper = Parent.find('tbody.visible-sm');
			for(var key in data) {
				Wrapper.append('<tr data-jump="'+data[key].casualtytitle+'"><td colspan="2" class="graybg RB18 pad-full-0"><div style="float: left; margin-top: 18px;">'+data[key].casualtytitle+'</div><div style="text-align: right; float: right;" class="graybg RB18 p-10"><a class="top" href="javascript: void(0);"><span class="arrow"></span>Top</a></div></td></tr>');

				var HeaderItems = this.HeaderLinks;
				var CasualData = data[key].casualtyData;
				
				for(var key in CasualData) {
					for(var i in HeaderItems) {
						if(HeaderItems[i] == 'Title') {
							Wrapper.append('<tr>'+
			                                 '<td class="pad-10 R21_GrayColor border-right">'+HeaderItems[i]+'</td>'+
			                                 '<td class="pad-10 R21_GrayColor"><a href="'+ $('#casualtyDetailUrl').val()+ '?incidentId='+ CasualData[key].incidentId +'">'+CasualData[key][HeaderItems[i]]+'</a></td>'+
		                              		'</tr>');
						} else {
							Wrapper.append('<tr>'+
			                                 '<td class="pad-10 R21_GrayColor border-right">'+HeaderItems[i]+'</td>'+
			                                 '<td class="pad-10 R21_GrayColor">'+CasualData[key][HeaderItems[i]]+'</td>'+
		                              		'</tr>');
						}
					}
					Wrapper.append('<tr>'+
		                                 '<td><hr /></td>'+
		                                 '<td><hr /></td>'+
	                              		'</tr>');
				}
			}
		},
		RenderTable: function(data, Parent) { 
			Parent.empty();
			this.DesktopVersion(data, Parent);
			this.MobileVersion(data, Parent);
			$('table').on('click', 'a.top', function(){
				var $this = $(this), table = $this.closest('table'), tablePos = table.offset().top;
				if(window.matchMedia("(max-width: 400px)").matches){
					$(window).scrollTop(tablePos - 40);
				}
				else{
					$(window).scrollTop(tablePos);
				}
			}); 
		},
		FindHeaderLinks: function(data) {
			for(var key in data) {
				var CasualityData = data[key].casualtyData;
                for(var item in CasualityData) {
                	var List = CasualityData[item];
                	for(var list in List) {
                		if(list != "incidentId") {
                			this.HeaderLinks.push(list);
                		}
                	}
                	break;
                }
                break;
			}
		},
		ChangeReport: function() {
			var self = this;
			$(document).on('change','#relDate', function(){
				var Value = $(this).find('.selectivity-single-selected-item').attr('data-item-id');
				if(window.jsonCasualtyListing[0][Value] != undefined) {
					self.RenderTable(window.jsonCasualtyListing[0][Value], $('#casualty-listing-table'));
					$('#casualty-listing-table').show();
				} else {
					$('#casualty-listing-table').hide();
				}

				if($('.jumpToSection #jumpTo')) {
					$('.jumpToSection #jumpTo').remove();
				}
				$('.jumpToSection').append('<select name="jumpTo" id="jumpTo" class="common-field inline"></select>');

				for(var i = 0; i < self.JumpToArray.length; i++) {
					$('#jumpTo').append('<option value="'+self.JumpToArray[i]+'">'+self.JumpToArray[i]+'</option>');
				}
				$('#jumpTo').selectivity({
					showSearchInputInDropdown: false
				});

				$(".selectivity-input .selectivity-single-select").each(function() {
	 			   $(this).append('<span class="selectivity-arrow"><svg class="alert__icon"><use xlink:href="/dist/img/svg-sprite.svg#sort-down-arrow"></use></svg></span>');
	 			 });
			});
			$(document).on('change','#jumpTo', function(){
				var Value = $(this).find('.selectivity-single-selected-item').attr('data-item-id');
				var Top = $('#casualty-listing-table tr[data-jump="'+Value+'"]').offset().top;
				if($(window).width() < 667) {
					var Top = $($('#casualty-listing-table tr[data-jump="'+Value+'"]')[1]).offset().top - 40;
				}
				$('html, body').scrollTop(Top);
			});
		},
		init: function(data, Parent) { 
			if(data.length !== 0){
				var FirstValue = $('#relDate').val(),
					CurrentObj = data[0][FirstValue];

				this.FindHeaderLinks(CurrentObj);
				this.RenderTable(CurrentObj, Parent);
				this.ChangeReport();
			}
			else{
				$('#casualty-listing-table').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnInfomessage').val()+'</p></div>');
			}
		}
	}
	
	$(document).ready(function() {
		if($('#casualty-listing-table').length > 0) {
			if(typeof window.jsonCasualtyListing !== 'undefined' && typeof window.jsonCasualtyListing !== 'string'){
				CasualityListing.init(window.jsonCasualtyListing, $('#casualty-listing-table'));
			}
			else{
				$('#casualty-listing-table').html('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
			}
		}
	});
})();