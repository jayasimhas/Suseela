(function (argument) {
	var MergeAcquistion = {
		CurrentArray: [],
		LargeValue: [],
		MonthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
		HeadingNames: ['Month', 'Acquirer', 'Target', 'TargetSector', 'TargetLocation', 'Detail', 'Price'],
		RenderDesktopVersion: function(data, Parent) {
			Parent.find('tbody.visible-lg').remove();
			Parent.append('<tbody class="visible-lg"></tbody>');

			var Wrapper = Parent.find('tbody.visible-lg');

			for(var key in data) {
				Wrapper.append('<tr></tr>');				
				Wrapper.find('tr:last-child').append('<td align="left" deal="Month" type="date" month="'  +data[key].Month+ '" class="R16 pad-10">' +this.MonthNames[data[key].Month - 1]+ '</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="Acquirer" type="text" class="R16 pad-10">'+data[key].Acquirer+'</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="Target" type="text" class="R16 pad-10">'+data[key].Target+'</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="TargetSector" type="text" class="R16 pad-10">'+data[key].TargetSector+'</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="TargetLocation" type="text" class="R16 pad-10">'+data[key].TargetLocation+'</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="Detail" type="text" class="R16 pad-10">'+data[key].Detail+'</td>');
				if(data[key].Price) {
					var Price = data[key].Price;
				} else {
					Price = '-';
				}
				Wrapper.find('tr:last-child').append('<td align="right" deal="Price" type="number" class="R16 pad-10">'+Price+'</td>');
			}

		},
		RenderMobileVersion: function(data, Parent) {
			Parent.find('tbody.visible-xs').remove();
			Parent.append('<tbody class="visible-xs"></tbody>');

			var Wrapper = Parent.find('tbody.visible-xs'),
				Headings = this.HeadingNames;


			for(var key in data) {
				Wrapper.append('<tr>'+
										'<td width="50%" class="boder-none"><hr class="m-0"/></td>'+
										'<td width="50%" class="boder-none"><hr class="m-0"/></td>'+
									'</tr>');
				for(var j in Headings) {
					if(Headings[j] == 'Month') {
						Wrapper.append('<tr>'+
										'<td class="pad-10" width="50%">' + Headings[j] + '</td>'+
										'<td class="pad-10" width="50%">' + this.MonthNames[data[key][Headings[j]] - 1]+ '</td>'+
									'</tr>');
					} else if (Headings[j] == 'Price') {
						var Price = "";
						if(data[key][Headings[j]].length > 0) {
							Price = data[key][Headings[j]];
						} else {
							Price = '-';
						}
						Wrapper.append('<tr>'+
										'<td class="pad-10" width="50%">' + Headings[j] + '</td>'+
										'<td class="pad-10" width="50%">' + Price + '</td>'+
									'</tr>');
					} else {
						Wrapper.append('<tr>'+
										'<td class="pad-10" width="50%">' + Headings[j] + '</td>'+
										'<td class="pad-10" width="50%">' + data[key][Headings[j]] + '</td>'+
									'</tr>');
					}
				}
				Wrapper.append('<tr>'+
										'<td width="50%" class="boder-none"><hr/></td>'+
										'<td width="50%" class="boder-none"><hr/></td>'+
									'</tr>');
			}
		},
		SortingEvent: function(data, Parent) {
			var self = this;
			$(document).on('click','.sorting-buttons a', function(e){
				e.preventDefault();
				var Type = $(this).attr('type'),
					Category = $(this).attr('category'),
					Index = $(this).parents('th').index(),
					SortingArray = [],
					MyArray = self.CurrentArray,
					Body = Parent.find('tbody.visible-lg'),
					AppendToEndElements = [],
					SortedElements = [],
					SortingType = $(this).parents('.sorting-buttons').attr('deal');
				
				//Fetching Elements
				Body.find('tr').each(function(key) {
					var Text = "";
					if(Category === 'month') {
						SortingArray.push(parseInt($($(this).find('td')[Index]).attr('month')));
					} else if (Category === 'number') {
						var Num = $($(this).find('td')[Index]).text();
						if(Num != '-') {
							SortingArray.push(parseFloat(Num));
						} else {
							AppendToEndElements.push(self.CurrentArray[key]);
						}
					} 
					else {
						if($($(this).find('td')[Index]).text().includes('<a href=')) {
							Text = $($(this).find('td')[Index]).find('a').text();
						} else {
							Text = $($(this).find('td')[Index]).text();
						}
						SortingArray.push(Text);
					}
				});
				console.log(SortingArray);
				if(Category === 'month' || Category === 'number') {
					if(Type === 'ascending') {
						SortingArray.sort(function(a, b){
						  return a - b;
						});
					} else {
						SortingArray.sort(function(a, b){
						  return b - a;
						});
					}
				} else {
					if(Type === 'ascending') {
						SortingArray.sort();
					} else {
						SortingArray.sort().reverse();
					}
				}
				console.log(SortingArray);
				SortedElements = [];
				var CurrentItem = self.CurrentArray;
				for(var i in SortingArray) {
					for(var j in CurrentItem) {
						if(CurrentItem[j] != undefined) {
							if(SortingArray[i] == CurrentItem[j][SortingType]) {
								SortedElements.push(CurrentItem[j]);
								CurrentItem = CurrentItem.filter(function(item, index) { 
								    return CurrentItem[index] !== CurrentItem[j];
								});
							}
						}
					}
				}
				//if(AppendToEndElements.length > 0) {
					var NewArray = SortedElements.concat(AppendToEndElements);
				//}
				// var UniqueArray = [];
				// for(var k = 0; k < SortedElements.length; k++) {
				// 	if(!SortedElements.contains(SortedElements[k])) {
				// 		UniqueArray.push(SortedElements[k]);
				// 	}
				// }
				self.CurrentArray = [];
				self.CurrentArray = NewArray;
				self.RenderDesktopVersion(self.CurrentArray, $('.merge-acquistion'));
				self.RenderMobileVersion(self.CurrentArray, $('.merge-acquistion'));
			});
		},
		FilterEvent: function(data, Parent) {
			var InputValues = Parent.find('th').find('input'),
				self = this;
			if($(window).width() < 668) {
				InputValues = $('.merge-form-items input');
			}
			InputValues.on('keyup', function() {
				var textFieldValue = $(this).val(),
					DealType = $(this).attr('deal'),
					Index = $(this).parents('th').index(),
					Body = Parent.find('tbody.visible-lg'),
					regExp = new RegExp(textFieldValue, "i"),
					ItemArray = [],
					FilteredArray = [],
					Obj = {},
					StartField = Parent.find('.range-field')[0].value,
					EndField = Parent.find('.range-field')[1].value;

				if($(window).width() < 668) {
					Index = $(this).parents('.forms').index();
					StartField = $('.merge-form-items .range-field.start').val();
					EndField = $('.merge-form-items .range-field.end').val();
				}
				InputValues.each(function(key) {
					var DealType = $(this).attr('deal');
					if($(this).val()) {
						if(DealType != 'Price') {
							Obj[DealType] = $(this).val();
						} else {
							var Start, End;
							if(StartField) {
								Start = parseFloat(StartField);
							} else {
								Start = 0;
							}
							if(EndField) {
								End = parseFloat(EndField);
							} else {
								End = null;
							}
							Obj[DealType] = {
								Start : Start,
								End : End
							}
							// Obj[DealType]['End'] = 
						}
					}
				});

				// Body.find('tr').each(function(key) {
				// 	ItemArray.push($($(this).find('td')[Index]).text());
				// });

				//for(var i in ItemArray) {
					// if(regExp.test(ItemArray[i])) {
					// 	for(var j = 0; j < window.jsonMergeAcquistion.length; j++) {
					// 		if(ItemArray[i] == window.jsonMergeAcquistion[j][DealType]) {
					// 			FilteredArray.push(window.jsonMergeAcquistion[j]);
					// 		}
					// 	}
					// }
				//}
				if(Object.keys(Obj).length > 0) {
					for(var i in window.jsonMergeAcquistion) {
						var count = 0;
						for(var j in Obj) {
							var text = "";
							if(j == 'Price') {
								var Price = parseFloat(window.jsonMergeAcquistion[i][j]);
								
								if(Obj[j]['End'] != null) {
									if((Price >= Obj[j]['Start']) && (Price <= Obj[j]['End'])) {
										count++;
									}
								} else {
									if(Price >= Obj[j]['Start']) {
										count++;
									}
								}
							} else if (j == 'Month') {
								var MonthValue = window.jsonMergeAcquistion[i][j] - 1;
								if(self.MonthNames[MonthValue].toLowerCase().includes(Obj[j].toLowerCase())) {
									count++;
								}
							} else {
								if(window.jsonMergeAcquistion[i][j].includes('<a href=')) {
									text = $(window.jsonMergeAcquistion[i][j]).text();
								} else {
									text = window.jsonMergeAcquistion[i][j];
								}
								if(text.toLowerCase().includes(Obj[j].toLowerCase())) {
									count++;
								}
							}
						}

						if(count === Object.keys(Obj).length) {
							FilteredArray.push(window.jsonMergeAcquistion[i]);
						}
					}
				} else {
					FilteredArray = window.jsonMergeAcquistion;
				}
				self.CurrentArray = FilteredArray;
				self.RenderDesktopVersion(self.CurrentArray, $('.merge-acquistion'));
				self.RenderMobileVersion(self.CurrentArray, $('.merge-acquistion'));
			});
		},
		YearChange: function() {
			$(document).on('change', '.idYearSelect', function() {
				var Href = $(this).attr('data-href'),
					value= $(this).find('.selectivity-single-selected-item').attr('data-item-id'),
					newUrl = window.location.href.split('?')[0].concat("?year="+value);


				window.location =newUrl;			

			});
		},
		MobileEvent: function() {
			$(document).on('click', '.merge-acordian', function(e) {
				e.preventDefault();
				$(this).parents('.merges-form').toggleClass('open');
			});
		},
		showLargestEvent: function() {
			var self = this;
			$(document).on('click', '.show-largest-btn', function(e) {
				e.preventDefault();
				var ThresholdValue = $(this).attr('data-large'),
					Items = [],
					PriceItem = [];

				if($(this).hasClass('active')) {
					self.RenderDesktopVersion(self.CurrentArray, $('.merge-acquistion'));
					self.RenderMobileVersion(self.CurrentArray, $('.merge-acquistion'));
				} else {
					for(var i= 0; i < self.CurrentArray.length; i++) {
						if(self.CurrentArray[i].Price != '-') {
							if(parseFloat(self.CurrentArray[i].Price) > parseFloat(ThresholdValue)) {
								PriceItem.push(parseFloat(self.CurrentArray[i].Price));
							}
						}
					}
					PriceItem.sort(function(a, b){
					  return b - a;
					});
					for(var i = 0; i < PriceItem.length; i++) {
						for(var j= 0; j < self.CurrentArray.length; j++) {
							if(self.CurrentArray[j].Price == PriceItem[i]) {
								Items.push(self.CurrentArray[j]);
							}
						}
					}
					self.RenderDesktopVersion(Items, $('.merge-acquistion'));
					self.RenderMobileVersion(Items, $('.merge-acquistion'));
				}

				$(this).toggleClass('active');
			})
		},
		autoSuggest: function(data) {
			var haystackMonth = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

				$('input[deal="Month"]').suggest(this.MonthNames);

				var haystackAcquirer = [], haystackTarget = [], haystackTargetSector = [], haystackTargetLocation = [];
				for(var i = 0; i < data.length; i++) {
					var item = data[i];
					
					for(var key in item) {
						var Text = "";
						if(item[key].indexOf('<a href') != -1) {

							Text = $(item[key]).text();
						} else {
							Text = item[key];
						}

						if(key == 'Acquirer') {
							haystackAcquirer.push(Text);
						}
						if(key == 'Target') {
							haystackTarget.push(Text);
						}
						if(key == 'TargetSector') {
							haystackTargetSector.push(Text);
						}
						if(key == 'TargetLocation') {
							haystackTargetLocation.push(Text);
						}
					}
				}
				
				$('input[deal="Acquirer"]').suggest(haystackAcquirer);
				$('input[deal="Target"]').suggest(haystackTarget);
				$('input[deal="TargetSector"]').suggest(haystackTargetSector);
				$('input[deal="TargetLocation"]').suggest(haystackTargetLocation);
		},
		scrollbarSticky: function() {},
		init: function(data, Parent) {
			this.CurrentArray = data;
			this.RenderDesktopVersion(data, Parent);
			this.RenderMobileVersion(data, Parent);
			this.SortingEvent(data, Parent);
			this.FilterEvent(data, Parent);
			this.YearChange();
			this.MobileEvent();
			this.showLargestEvent();
			
			if($(window).width() > 667) {
				this.autoSuggest(data);
			}
			this.scrollbarSticky();
		}
	}

	if($('.merge-acquistion').length > 0) {
		MergeAcquistion.init(window.jsonMergeAcquistion, $('.merge-acquistion'));
	}
})();