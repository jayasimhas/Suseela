$(function(){
	$('.availableGraphs').off('click').on('click', 'li a', function () {
		var $this = $(this), id = $this.attr('id'), getFocusId = id.split('-')[1];
		$(window).scrollTop($('#' + getFocusId).offset().top);
		$('#'+getFocusId).trigger('click');
	});
	
	var chartAccordionIDs = ["GWP", "NWP", "UR", "NP", "SF", "NWPNR", "SFNR", "NPSFR"];
	$('.expandAll', '.compareChart').click(function(){
		var eachChartData  = $('.eachChartData');
		for(var i = 0; i < chartAccordionIDs.length; i++){
			$(eachChartData[i]).find('.chartexpand').click();
			$("#"+chartAccordionIDs[i]+"-DATA").removeClass('hide');
		}
		$('.chartexpand', '.compareChart').addClass('active');
	});
	
	$('.collapseAll', '.compareChart').click(function(){
		$('.chartData', '.compareChart').addClass('hide');
		$('.chartexpand', '.compareChart').removeClass('active');
	});
	
	var geturl = window.location.href;
	if(geturl.indexOf('graphid=') !== -1){
		var getId = geturl.split('graphid=')[1];
		$(window).scrollTop($('#' + getId).offset().top);
		$('#'+getId).trigger('click');
	}
});

