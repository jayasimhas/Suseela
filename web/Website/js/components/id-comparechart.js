$(function(){
	$('.chartexpand', '.eachChartData').on('click', function(){
		var $this = $(this), chartData = $this.closest('.eachChartData').find('.chartData');
		if(chartData.is(':visible')){
			chartData.addClass('hide');
			$this.removeClass('active');
		}
		else{
			chartData.removeClass('hide');
			$this.addClass('active');
		}
	});
	
	$('.availableGraphs').on('click', 'li a', function(){
		var $this = $(this), id = $this.attr('id'), getFocusId = id.split('-')[1];
		$(window).scrollTop($('#' + getFocusId).position().top);
		$('#' + getFocusId +'-DATA').removeClass('hide'); 
		$('#' + getFocusId).find('.chartexpand').addClass('active');
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
});

