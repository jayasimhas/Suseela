(function (argument) {
	var MergeAcquistion = {
		CurrentArray: [],
		MonthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
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
				Wrapper.find('tr:last-child').append('<td align="right" deal="Detail" type="number" class="R16 pad-10">'+data[key].Detail+'</td>');
				if(data[key].Price) {
					var Price = data[key].Price;
				} else {
					Price = '--';
				}
				Wrapper.find('tr:last-child').append('<td align="right" deal="Price" type="number" class="R16 pad-10">'+Price+'</td>');
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
					if(Category === 'month') {
						SortingArray.push(parseInt($($(this).find('td')[Index]).attr('month')));
					} else {
						SortingArray.push($($(this).find('td')[Index]).text());
					}
				});
				console.log(SortingArray);
				if(Category === 'month') {
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
				
				var CurrentItem = self.CurrentArray;
				for(var i in SortingArray) {
					for(var j in CurrentItem) {
						if(SortingArray[i] == CurrentItem[j][SortingType]) {
							SortedElements.push(CurrentItem[j]);
							j++;
						}
					}
				}
				if(AppendToEndElements.length > 0) {
					SortedElements.push(AppendToEndElements);
				}
				// var UniqueArray = [];
				// for(var k = 0; k < SortedElements.length; k++) {
				// 	if(!SortedElements.contains(SortedElements[k])) {
				// 		UniqueArray.push(SortedElements[k]);
				// 	}
				// }

				self.CurrentArray = SortedElements;
				self.RenderDesktopVersion(self.CurrentArray, $('.merge-acquistion'));
			});
		},
		FilterEvent: function(data, Parent) {
			var InputValues = Parent.find('th').find('input'),
				self = this;

			InputValues.on('keyup', function() {
				var textFieldValue = $(this).val(),
					DealType = $(this).parents('th').find('.sorting-buttons').attr('deal'),
					Index = $(this).parents('th').index(),
					Body = Parent.find('tbody.visible-lg'),
					regExp = new RegExp(textFieldValue, "i"),
					ItemArray = [],
					FilteredArray = [],
					Obj = {},
					StartField = Parent.find('.range-field')[0].value,
					EndField = Parent.find('.range-field')[1].value;

				Parent.find('th').each(function(key) {
					var DealType = $(this).find('.sorting-buttons').attr('deal');
					if($(this).find('input').val().length > 0) {
						Obj[DealType] = $(this).find('input').val();
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
						for(var j in Obj) {
							var text = ""
							if(Obj[j].length > 0) {
								if(j == 'Price') {
									var Price = parseFloat(window.jsonMergeAcquistion[i][j]);
									if(StartField.length > 0 && EndField.length > 0) {
										if((Price > parseFloat(StartField)) && (Price < parseFloat(EndField))) {
											FilteredArray.push(window.jsonMergeAcquistion[i]);
										}
									} else if(StartField.length > 0 && EndField.length == 0) {
										if((Price > parseFloat(StartField))) {
											FilteredArray.push(window.jsonMergeAcquistion[i]);
										}
									} else {
										if((Price < parseFloat(EndField))) {
											FilteredArray.push(window.jsonMergeAcquistion[i]);
										}
									}
								} else if (j == 'Month') {
									var MonthValue = window.jsonMergeAcquistion[i][j] - 1;
									if(self.MonthNames[MonthValue].toLowerCase().includes(Obj[j].toLowerCase())) {
										FilteredArray.push(window.jsonMergeAcquistion[i]);
									}
								} else {
									if(window.jsonMergeAcquistion[i][j].includes('<a href=')) {
										text = $(window.jsonMergeAcquistion[i][j]).text();
									} else {
										text = window.jsonMergeAcquistion[i][j];
									}
									if(text.toLowerCase().includes(Obj[j].toLowerCase())) {
										FilteredArray.push(window.jsonMergeAcquistion[i]);
									}
								}
							}
						}
					}
				} else {
					FilteredArray = window.jsonMergeAcquistion;
				}
				self.RenderDesktopVersion(FilteredArray, $('.merge-acquistion'));

			});
		},
		YearChange: function() {
			$(document).on('change', '#idYearSelect', function() {
				var Href = $(this).attr('data-href'),
					value= $(this).find('.selectivity-single-selected-item').attr('data-item-id'),
					newUrl = window.location.href.split('?')[0].concat("?year="+value);


				window.location =newUrl;			

			});
		},
		init: function(data, Parent) {
			this.CurrentArray = data;
			this.RenderDesktopVersion(data, Parent);
			this.SortingEvent(data, Parent);
			this.FilterEvent(data, Parent);
			this.YearChange();
			//this.RenderMobileVersion(data, Parent);
		}
	}

	if($('.merge-acquistion').length > 0) {
		MergeAcquistion.init(window.jsonMergeAcquistion, $('.merge-acquistion'));
	}
})();