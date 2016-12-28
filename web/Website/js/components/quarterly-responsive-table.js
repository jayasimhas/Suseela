(function () {
	// body...
	'use strict';

	var ResponsiveFinancialTable = {
		RenderTable:function(data, Parent) {
			 var Wrapper = $('#ID-Quaterly-Responsive-Table .table, #modal-table-quaterly .table'),
			 	QuaterlyDataHeader = data[0].QuaterlyDataHeader,
			 	QuaterlyData = data[0].QuaterlyData,
			 	QuaterlyResultHeader = data[0].QuaterlyResultHeader[0],
			 	QuaterlyResult= data[0].QuaterlyResult[0];

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
        	$(document).on('click', 'a[data-toggle="modal-table-quaterly"]', function(e) {
        		e.preventDefault();
        		$('#modal-table-quaterly').show();
        	});
        	$(document).on('click', '#modal-table-quaterly .table_close', function(e) {
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

	if($('#ID-Quaterly-Responsive-Table').length > 0) {
		ResponsiveFinancialTable.init(window.CompanyQuarterlyFinancials, $('#ID-Quaterly-Responsive-Table'));	
	}
})();