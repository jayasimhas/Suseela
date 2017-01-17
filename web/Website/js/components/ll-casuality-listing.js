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
				Header.find('tr').append('<th class="pad-10">'+HeaderItems[headItem]+'</th>');
			}
			//Body
			Parent.append('<tbody class="visible-lg"></tbody>');
			
			var Wrapper = Parent.find('tbody.visible-lg');
			for(var key in data) {
				//Appending Heading
				$('#jumpTo').append('<option value="'+data[key].casualtytitle+'">'+data[key].casualtytitle+'</option>');
				self.JumpToArray.push(data[key].casualtytitle);
				Wrapper.append('<tr data-jump="'+data[key].casualtytitle+'"><td colspan="2" class="graybg RB18 pad-10"> '+data[key].casualtytitle+'</td><td colspan="1" align="right" class="graybg RB18 pad-10"><a href="#">top</a></td></tr>');

				//Appending Body
                var CasualityData = data[key].casualtyData;
                for(var item in CasualityData) {
                	Wrapper.append('<tr><td class="RB16 pad-10"><a href="'+ $('#casualtyDetailUrl').val()+ '?incidentId='+ CasualityData[item].incidentId +'">'+CasualityData[item].Title+'</a></td><td class="R16 pad-10">'+CasualityData[item]["Date of Incident"]+'</td><td class="R16 pad-10">'+CasualityData[item]["Area"]+'</td></tr>');
                }
			}

			

			
		},
		MobileVersion:function(data, Parent) {
			Parent.append('<tbody class="visible-sm"></tbody>');

			var Wrapper = Parent.find('tbody.visible-sm');
			for(var key in data) {
				Wrapper.append('<tr><td class="graybg RB18 pad-full-10">'+data[key].casualtytitle+'</td><td align="right" class="graybg RB18 pad-10"><a href="#">top</a></td></tr>');

				var HeaderItems = this.HeaderLinks;
				var CasualData = data[key].casualtyData;
				
				for(var key in CasualData) {
					for(var i in HeaderItems) {
						Wrapper.append('<tr>'+
		                                 '<td class="pad-10 R21_GrayColor">'+HeaderItems[i]+'</td>'+
		                                 '<td class="pad-10 R21_RedColor">'+CasualData[key][HeaderItems[i]]+'</td>'+
	                              		'</tr>');
						}
				}
			}
		},
		RenderTable: function(data, Parent) {
			Parent.empty();
			this.DesktopVersion(data, Parent);
			this.MobileVersion(data, Parent);
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
				var Top = $('#casualty-listing-table tr[data-jump='+Value+']').offset().top;

				window.scrollTo(0, Top);
			});
		},
		init: function(data, Parent) {
			var FirstValue = $('#relDate').val(),
				CurrentObj = data[0][FirstValue];

			this.FindHeaderLinks(CurrentObj);
			this.RenderTable(CurrentObj, Parent);
			this.ChangeReport();
		}
	}
	
	$(document).ready(function() {
		if($('#casualty-listing-table').length > 0) {
			CasualityListing.init(window.jsonCasualtyListing, $('#casualty-listing-table'));
		}
	});
})();