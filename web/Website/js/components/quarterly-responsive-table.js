(function () {
	// body...
	'use strict';

	var ResponsiveFinancialTable = {
		RenderTable:function(data, Parent) {
			 var Tables = $('#quarterlyresults, #modal-quarterlyresults'),
			 	QuaterlyDataHeader = data[0].QuaterlyDataHeader,
			 	QuaterlyData = data[0].QuaterlyData,
			 	QuaterlyResultHeader = data[0].QuaterlyResultHeader[0],
			 	QuaterlyResult= data[0].QuaterlyResult[0];

			 	$('#quarterlyresults').find('.states_heading').parent().remove();
			 	$('#quarterlyresults').append('<div class="table-wrapper"><div class="table"></div></div>');
			 	var Wrapper = $('#quarterlyresults .table, #modal-table-quaterly .table');

			 	Wrapper.append('<div class="tableRow"></div>');
				for(var key in QuaterlyDataHeader) {
				 	Wrapper.find('.tableRow:last-child').append('<div class="tableHead">' +QuaterlyDataHeader[key]+ '</div>');
			 	}
		 		
		 		
				for(var key in QuaterlyData) {
					Wrapper.append('<div class="tableRow"></div>');
					var Content = QuaterlyData[key];
					for(var item in Content) {
					 	Wrapper.find('.tableRow:last-child').append('<div class="tableCell">' +Content[item]+ '</div>');
					}
			 	}

			 	Wrapper.append('<div class="tableRow"></div>');
				for(var key in QuaterlyResultHeader) {
				 	Wrapper.find('.tableRow:last-child').append('<div class="tableHead">' +QuaterlyResultHeader[key]+ '</div>');
			 	}

			 	Wrapper.append('<div class="tableRow"></div>');
				for(var key in QuaterlyResult) {
				 	Wrapper.find('.tableRow:last-child').append('<div class="tableCell">' +QuaterlyResult[key]+ '</div>');
			 	}
			 
		},
        ModalEvents: function() {
        	$(document).on('click', 'a[data-toggle="modal-quarterlyresults"]', function(e) {
        		e.preventDefault();
        		$('#modal-table-quaterly').show();
        	});
        	$(document).on('click', '#modal-quarterlyresults .table_close', function(e) {
        		e.preventDefault();
        		$(this).parents('.ID-responsive-table-modal').hide();
        	});
        	$(document).on('click', '.ID-responsive-table-modal', function(e) {
    			if($(e.target).parents('.container').length > 0 || $(e.target).hasClass('.container')) {
    				return false;
    			} else {
    				$(this).hide();
    			}
        	});
        },
		init: function(data, Parent) {
			var self = this;
			self.RenderTable(data, Parent);
			self.ModalEvents();
		}

	}

	if($('#quarterlyresults').length > 0) {
		ResponsiveFinancialTable.init(window.jsonResultQuarterly, $('#quarterlyresults'));	
	}
})();