function sortableTableController() {

	/*
	Based on SortTable version 2
	7th April 2007
	Stuart Langridge, http://www.kryogenix.org/code/browser/sorttable/
	Licenced as X11: http://www.kryogenix.org/code/browser/licence.html
	*/


	var stIsIE = /*@cc_on!@*/false;

	var isSortedTable = false;
	var tfo, mtch, sortfn, hasInputs;
	var sorttable = {
		init: function initing() {
			// quit if this function has already been called
			if (isSortedTable) return;
			// flag this function so we don't do the same thing twice
			isSortedTable = true;

			if (!document.createElement || !document.getElementsByTagName) return;

			sorttable.DATE_RE = /^(\d\d?)[\/\.-](\d\d?)[\/\.-]((\d\d)?\d\d)$/;

			$('.js-sortable-table').each(function (indx, item) {
				sorttable.makeSortable(item);
			});

		},

		makeSortable: function(table) {

			// Sorttable v1 put rows with a class of "sortbottom" at the bottom (as
			// "total" rows, for example). This is B&R, since what you're supposed
			// to do is put them in a tfoot. So, if there are sortbottom rows,
			// for backwards compatibility, move them to tfoot (creating it if needed).
			var sortbottomrows = [];
			for (var i = 0; i < table.rows.length; i++) {
				if ($(table.rows[i]).hasClass('.sortbottom')) {
					sortbottomrows[sortbottomrows.length] = table.rows[i];
				}
			}

			if (sortbottomrows) {
				if (table.tFoot == null) {
					// table doesn't have a tfoot. Create one.
					tfo = document.createElement('tfoot');
					table.appendChild(tfo);
				}
				for (var j = 0; j < sortbottomrows.length; j++) {
					tfo.appendChild(sortbottomrows[j]);
				}
				sortbottomrows = undefined;
			}

			// work through each column and calculate its type
			var headrow = table.tHead.rows[0].cells;
			for (var i = 0; i < headrow.length; i++) {

				// manually override the type with a sorttable_type attribute
				if (!headrow[i].className.match(/\bsorttable_nosort\b/)) { // skip this col
					headrow[i].sorttable_sortfunction = sorttable.guessType(table,i);
				}

			};

			$('.js-sortable-table-sorter').on('click', function(e) {

				// If child element is clicked, redirect the click to the
				// proper element: the parent itself.
				if(e.target !== this) {
					this.click();
					return;
				}


				var colNum = $(e.target).closest('.js-sortable-table-sorter').data('sortable-table-col') - 1;
				console.log(colNum);
				var guesstype = sorttable.guessType(table, colNum);

				if ($(e.target).hasClass('sorttable_sorted')) {
					// if we're already sorted by this column, just
					// reverse the table, which is quicker
					sorttable.reverse(table.tBodies[0]);
					$(e.target).removeClass('sorttable_sorted').addClass('sorttable_sorted_reverse');
					return;
				}

				if ($(e.target).hasClass('sorttable_sorted_reverse')) {
					// if we're already sorted by this column in reverse, just
					// re-reverse the table, which is quicker
					sorttable.reverse(table.tBodies[0]);
					$(e.target).removeClass('sorttable_sorted_reverse').addClass('sorttable_sorted');
					return;
				}

				// remove sorttable_sorted classes
				var theadrow = e.target.parentNode;
				forEach(theadrow.childNodes, function(cell) {
					if (cell.nodeType == 1) { // an element
						$(cell).removeClass('sorttable_sorted_reverse sorttable_sorted');
					}
				});

				var sortfwdind = document.getElementById('sorttable_sortfwdind');
				if (sortfwdind) { sortfwdind.parentNode.removeChild(sortfwdind); }
				var sortrevind = document.getElementById('sorttable_sortrevind');
				if (sortrevind) { sortrevind.parentNode.removeChild(sortrevind); }

				$(e.target).addClass('sorttable_sorted');

				// build an array to sort. This is a Schwartzian transform thing,
				// i.e., we "decorate" each row with the actual sort key,
				// sort based on the sort keys, and then put the rows back in order
				// which is a lot faster because you only do getInnerText once per row
				var row_array = [];
				var col = colNum;

				var rows = table.tBodies[0].rows;

				for (var j = 0; j < rows.length; j++) {
					row_array[row_array.length] = [$(rows[j].cells[col]).text().trim(), rows[j]];
				}
				/* If you want a stable sort, uncomment the following line */
				//sorttable.shaker_sort(row_array, this.sorttable_sortfunction);
				/* and comment out this one */
				row_array.sort(guesstype);

				var tb = table.tBodies[0];
				for (var j=0; j<row_array.length; j++) {
					tb.appendChild(row_array[j][1]);
				}

				row_array = undefined;
			});

		},

	guessType: function(table, column) {

		// guess the type of a column based on its first non-blank row
		sortfn = sorttable.sort_alpha;

		for (var i=0; i<table.tBodies[0].rows.length; i++) {

			var text = $(table.tBodies[0].rows[i].cells[column]).text().trim();
			if (text != '') {
				// If column is numeric or appears to be money, sort numeric
				if (text.match(/^-?[£$¤]?[\d,.]+%?$/)) {
					return sorttable.sort_numeric;
				}

				// Check for valid date
				// If found, assume column is full of dates, sort by date!
				if(Date.parse(text) > 0) {
					return sorttable.sort_by_date;
				}

			}
		}
		return sortfn;
	},

	reverse: function(tbody) {
		// reverse the rows in a tbody
		var newrows = [];
		for (var i=0; i<tbody.rows.length; i++) {
			newrows[newrows.length] = tbody.rows[i];
		}
		for (var i=newrows.length-1; i>=0; i--) {
			tbody.appendChild(newrows[i]);
		}
		newrows = undefined;
	},

	  /* sort functions
	     each sort function takes two parameters, a and b
	     you are comparing a[0] and b[0] */
	  sort_numeric: function(a,b) {
	    aa = parseFloat(a[0].replace(/[^0-9.-]/g,''));
	    if (isNaN(aa)) aa = 0;
	    bb = parseFloat(b[0].replace(/[^0-9.-]/g,''));
	    if (isNaN(bb)) bb = 0;
	    return aa-bb;
	  },
	  sort_alpha: function(a,b) {
	    if (a[0]==b[0]) return 0;
	    if (a[0]<b[0]) return -1;
	    return 1;
	  },

	  sort_by_date: function(a, b) {
		  // http://stackoverflow.com/questions/10123953/sort-javascript-object-array-by-date
		  // Turn your strings into dates, and then subtract them
		  // to get a value that is either negative, positive, or zero.
		  return new Date(b[0]) - new Date(a[0]);
	  },

	  shaker_sort: function(list, comp_func) {
	    // A stable sort function to allow multi-level sorting of data
	    // see: http://en.wikipedia.org/wiki/Cocktail_sort
	    // thanks to Joseph Nahmias
	    var b = 0;
	    var t = list.length - 1;
	    var swap = true;

	    while(swap) {
	        swap = false;
	        for(var i = b; i < t; ++i) {
	            if ( comp_func(list[i], list[i+1]) > 0 ) {
	                var q = list[i]; list[i] = list[i+1]; list[i+1] = q;
	                swap = true;
	            }
	        } // for
	        t--;

	        if (!swap) break;

	        for(var i = t; i > b; --i) {
	            if ( comp_func(list[i], list[i-1]) < 0 ) {
	                var q = list[i]; list[i] = list[i-1]; list[i-1] = q;
	                swap = true;
	            }
	        } // for
	        b++;

	    } // while(swap)
	  }
	}

	/// HELPER FUNCTIONS

	// Dean's forEach: http://dean.edwards.name/base/forEach.js
	/*
		forEach, version 1.0
		Copyright 2006, Dean Edwards
		License: http://www.opensource.org/licenses/mit-license.php
	*/

	// array-like enumeration
	if (!Array.forEach) { // mozilla already supports this
		Array.forEach = function(array, block, context) {
			for (var i = 0; i < array.length; i++) {
				block.call(context, array[i], i, array);
			}
		};
	}

	// generic enumeration
	Function.prototype.forEach = function(object, block, context) {
		for (var key in object) {
			if (typeof this.prototype[key] == "undefined") {
				block.call(context, object[key], key, object);
			}
		}
	};

	// character enumeration
	String.forEach = function(string, block, context) {
		Array.forEach(string.split(""), function(chr, index) {
			block.call(context, chr, index, string);
		});
	};

	// globally resolve forEach enumeration
	var forEach = function(object, block, context) {
		if (object) {
			var resolve = Object; // default
			if (object instanceof Function) {
				// functions have a "length" property
				resolve = Function;
			} else if (object.forEach instanceof Function) {
				// the object implements a custom forEach method so use that
				object.forEach(block, context);
				return;
			} else if (typeof object == "string") {
				// the object is a string
				resolve = String;
			} else if (typeof object.length == "number") {
				// the object is array-like
				resolve = Array;
			}
			resolve.forEach(object, block, context);
		}
	};

	sorttable.init();
};

export default sortableTableController;
