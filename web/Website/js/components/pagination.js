window.paginationdefaults = {
	totalCategories: 30,
	categoryLimit: 10,
	currentPage: 1,
	paginationEle: 'table',
}; 
 
window.setPagination = function(source){
    $.extend(window.paginationdefaults, source);
};

function paginationCur(fv, tv){
		if(window.paginationdefaults.paginationEle === 'table'){
			$('tbody.hidden-xs tr', '.page-account__table').hide();
			$('tbody.hidden-lg tr', '.page-account__table').hide();
			for(var i = fv; i < tv; i++){
				$('tbody.hidden-xs tr', '.page-account__table').eq(i).show();
				$('tbody.hidden-lg tr', '.page-account__table').eq(i).show();
			}
		}
		else{
			$(window.paginationdefaults.paginationEle).hide();
			for(var i = fv; i < tv; i++){
				$(window.paginationdefaults.paginationEle).eq(i).show();
			}
		}
}

window.loadPaginationNums = function(){
	var showPageLinks = Math.ceil(window.paginationdefaults.totalCategories / window.paginationdefaults.categoryLimit),
		linkStr = '';
		for(var i=1; i<=showPageLinks; i++){
			linkStr += '<a href="#">'+i+'</a>';
		} 
		if(showPageLinks > 1){
			$('.pagination span').html(linkStr);
		}
		else{
			$('.pagination').hide();
		}
		
		$('.pagination span a:eq(0)').click();
		$('.pagination a:eq(0)').removeAttr('href');
}

$(function(){
	$('.pagination').on('click', 'a', function(e){
		var $this = $(this), $val = $this.html();
		if($val.toLowerCase().indexOf('prev') >= 0){
			var idx = +$('.pagination span a.active').html() - 1, toVal = idx * window.paginationdefaults.categoryLimit, fromVal = toVal - window.paginationdefaults.categoryLimit;
			
			if($('.pagination span a:first').hasClass('active')) return false;
			paginationCur(fromVal, toVal);
			
			var curidx = $('.pagination span a.active').index(), pagesLen = $('.pagination li > span a').length-1;
			$('.pagination span a').eq(curidx).removeClass('active');
			$('.pagination span a').eq(curidx).prev('a').addClass('active');
			$('.pagination a:last').attr('href','#');
			
			$('.pagination .next').removeClass('active');
			
			if($('.pagination span a:first').hasClass('active')){
				$('.pagination .prev').addClass('active');
			}
		}
		else if($val.toLowerCase().indexOf('next') >= 0){
			var idx = +$('.pagination span a.active').html() + 1, toVal = idx * window.paginationdefaults.categoryLimit, fromVal = toVal - window.paginationdefaults.categoryLimit;
			
			if($('.pagination span a:last').hasClass('active')) return false;
			paginationCur(fromVal, toVal);
			
			var curidx = $('.pagination span a.active').index(), pagesLen = $('.pagination li > span a').length-1;
			$('.pagination span a').eq(curidx).removeClass('active');
			$('.pagination span a').eq(curidx).next('a').addClass('active');
			$('.pagination a:first').attr('href','#');  
			
			$('.pagination .prev').removeClass('active');
			
			if($('.pagination span a:last').hasClass('active')){
				$('.pagination .next').addClass('active');
			}
		}
		else{
			if(!$this.hasClass('active')){
				$('.pagination span a').removeClass('active').attr('href','#');
				$this.addClass('active');
				toVal = window.paginationdefaults.categoryLimit * $val;
				fromVal = toVal - window.paginationdefaults.categoryLimit;
				paginationCur(fromVal, toVal);
				$('.pagination a:last').attr('href','#');
				$('.pagination a:first').attr('href','#');
				
				if ($('.pagination span a:first').hasClass('active')) {
					$('.pagination .prev').addClass('active');
					$('.pagination .next').removeClass('active');
				} else if ($('.pagination span a:last').hasClass('active')) {
					$('.pagination .prev').removeClass('active');
					$('.pagination .next').addClass('active');
				} else {
					$('.pagination .prev').removeClass('active');
					$('.pagination .next').removeClass('active');
				}
			}
			else{
				e.preventDefault();
			}
		}
	}); 
		
	$('.sortable-table__header').on('click', '.sortable-table__col', function(){
		var $this = $(this), table = $this.closest('.sortable-table'), tbodytrs = table.find('tbody tr');
		setTimeout(function(){
			tbodytrs.removeAttr('style');
			if(!$('.pagination span a:eq(0)').hasClass('active')){
				$('.pagination span a:eq(0)').click();
			}
			else{
				paginationCur(0, window.paginationdefaults.categoryLimit);
			}
		}, 1);
	});
});