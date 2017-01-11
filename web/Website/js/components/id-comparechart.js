$(function(){
	$('.availableGraphs').off('click').on('click', 'li a', function () {
		var $this = $(this), id = $this.attr('id'), getFocusId = id.split('-')[1];
		$(window).scrollTop($('#' + getFocusId).offset().top);
		$('#'+getFocusId).find('.chartexpand').trigger('click');
	});
	
	var chartAccordionIDs = ["GWP", "NWP", "UR", "NP", "SF", "NWPNR", "SFNR", "NPSFR"];
	$('.expandAll', '.compareChart').click(function(){
		var eachChartData  = $('.eachChartData');
		for(var i = 0; i < chartAccordionIDs.length; i++){
			if(!$(eachChartData[i]).find('.chartexpand').hasClass('active')){
				$(eachChartData[i]).find('.chartexpand').click();
				//$("#"+chartAccordionIDs[i]+"-DATA").removeClass('hide');
			}
		}
		$('.chartexpand', '.compareChart').addClass('active');
	});
	
	$('.collapseAll', '.compareChart').click(function(){
		if($('.graph-container') && $('.graph-container').length){
			$('.graph-container', '.compareChart').addClass('hide');
		}
		$('.chartData', '.compareChart').addClass('hide');
		$('.chartexpand', '.compareChart').removeClass('active');
	});
	
	$('.chartexpand', '.eachChartData').click(function () {
		var $this = $(this), eachChartData = $this.closest('.eachChartData'), chartexpand = eachChartData.find('.chartexpand'), graphCont = eachChartData.find('.graph-container');
		if(chartexpand.hasClass('active')){
			graphCont.addClass('hide');
			$this.removeClass('active');
		}
		else{
			graphCont.removeClass('hide');
			$this.addClass('active');
		}
	});
	
	var geturl = window.location.href;
	if(geturl.indexOf('graphid=') !== -1){
		var getId = geturl.split('graphid=')[1];
		$(window).scrollTop($('#' + getId).offset().top);
		$('#'+getId).find('.chartexpand').trigger('click'); 
	}
});

