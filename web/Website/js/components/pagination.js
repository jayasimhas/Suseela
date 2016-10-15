var toVal = 1, fromVal = 1;

function paginationCur(fv, tv){
	$('.eachCategory').hide();
	for(var i = fv; i < tv; i++){
		$('.eachCategory').eq(i).show();
	}
}
$(function(){
	var showPageLinks = Math.ceil(paginationObj.totalCategories / paginationObj.categoryLimit);
	var linkStr = '';
	for(var i=1; i<=showPageLinks; i++){
		linkStr += '<a href="javascript:void(0);">'+i+'</a>';
	} 
	if(showPageLinks > 1){
		$('.pagination span').html(linkStr);
	}
	else{
		$('.pagination').hide();
	}

	$('.pagination a').click(function(){
		var $this = $(this), $val = $this.html();
		if($val.toLowerCase().indexOf('prev') >= 0){
			var idx = $('.pagination span a.active').index();
			$('.pagination span a:eq('+idx+')').prev('a').click();
			var curidx = $('.pagination span a.active').index();
			$('.pagination a:last').attr('href','javascript:void(0);');
			if(curidx == 0){
				$('.pagination a:eq(0)').removeAttr('href');
			}
		}
		else if($val.toLowerCase().indexOf('next') >= 0){
			var idx = $('.pagination span a.active').index();
			$('.pagination span a:eq('+idx+')').next('a').click();
			var curidx = $('.pagination span a.active').index(), pagesLen = $('.pagination li > span a').length-1;
			$('.pagination a:first').attr('href','javascript:void(0);');
			if(curidx == pagesLen){
				$('.pagination a:last').removeAttr('href');
			} 
		}
		else{
			if(!$this.hasClass('active')){
				$('.pagination span a').removeClass('active').attr('href','javascript:void(0);');
				$this.addClass('active').removeAttr('href');
				toVal = paginationObj.categoryLimit * $val;
				fromVal = toVal - paginationObj.categoryLimit;
				paginationCur(fromVal, toVal);
				$('.pagination a:last').attr('href','javascript:void(0);');
				$('.pagination a:first').attr('href','javascript:void(0);');
				if($('.pagination span a.active').next('a').length == 0){
					$('.pagination a:last').removeAttr('href');
				} 
				if($('.pagination span a.active').prev('a').length == 0){
					$('.pagination a:first').removeAttr('href');
				}
			}
		}
	});
	$('.pagination span a:eq(0)').click();
	$('.pagination a:eq(0)').removeAttr('href');
});

