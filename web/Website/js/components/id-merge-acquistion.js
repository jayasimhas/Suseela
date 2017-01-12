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
				Wrapper.find('tr:last-child').append('<td align="left" deal="deal_month" type="date" month="'  +data[key].deal_month+ '" class="R16 pad-10">' +this.MonthNames[data[key].deal_month - 1]+ '</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="acquirer_company_name" type="text" class="R16 pad-10">'+data[key].acquirer_company_name+'</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="target_company_name" type="text" class="R16 pad-10">'+data[key].target_company_name+'</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="target_sector" type="text" class="R16 pad-10">'+data[key].target_sector+'</td>');
				Wrapper.find('tr:last-child').append('<td align="left" deal="target_location" type="text" class="R16 pad-10">'+data[key].target_location+'</td>');
				Wrapper.find('tr:last-child').append('<td align="right" deal="details" type="number" class="R16 pad-10">'+data[key].details+'</td>');
				if(data[key].price) {
					var Price = data[key].price;
				} else {
					Price = '--';
				}
				Wrapper.find('tr:last-child').append('<td align="right" deal="price" type="number" class="R16 pad-10">'+Price+'</td>');
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
		init: function(data, Parent) {
			this.CurrentArray = data;
			this.RenderDesktopVersion(data, Parent);
			this.SortingEvent(data, Parent);
			
			//this.RenderMobileVersion(data, Parent);
		}
	}

	if($('.merge-acquistion').length > 0) {
		MergeAcquistion.init(window.jsonMergeAcquistion, $('.merge-acquistion'));
	}
})();