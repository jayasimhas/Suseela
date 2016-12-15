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
	
	$('.expandAll', '.compareChart').click(function(){
		$('.chartData', '.compareChart').removeClass('hide');
		$('.chartexpand', '.compareChart').addClass('active');
	});
	
	$('.collapseAll', '.compareChart').click(function(){
		$('.chartData', '.compareChart').addClass('hide');
		$('.chartexpand', '.compareChart').removeClass('active');
	});
});

