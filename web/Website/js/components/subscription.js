$(function(){
	$('.accordian', '.subjectsAct').click(function(){
		var $this = $(this), tablerow = $this.closest('.tablerow'), subjectsPan = $this.closest('.subjectsAct').find('.subjects');
		if(!$this.hasClass('active')){
			tablerow.find('.accordian').addClass('active');
			subjectsPan.removeClass('hide');
		}
		else{
			tablerow.find('.accordian').removeClass('active');
			subjectsPan.addClass('hide');
		}
	});
	
	$('.maintitle').click(function(){
		var $this = $(this), subjectsAct = $this.closest('.subjectsAct'), subjectsPan = subjectsAct.find('.subjects');
		if(!$this.hasClass('active')){
			subjectsPan.removeClass('hide');
			$this.addClass('active');
		}
		else{
			subjectsPan.addClass('hide');
			$this.removeClass('active');
		}
	});
});