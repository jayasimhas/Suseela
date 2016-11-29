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
			linkStr += '<a href="javascript:void(0);">'+i+'</a>';
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
		$('.pagination').on('click', 'a', function(){
			var $this = $(this), $val = $this.html();
			if($val.toLowerCase().indexOf('prev') >= 0){
				var idx = +$('.pagination span a.active').html() - 1, toVal = idx * window.paginationdefaults.categoryLimit, fromVal = toVal - window.paginationdefaults.categoryLimit;
				
				if($('.pagination span a:first').hasClass('active')) return;
				paginationCur(fromVal, toVal);
				
				var curidx = $('.pagination span a.active').index(), pagesLen = $('.pagination li > span a').length-1;
				$('.pagination span a').eq(curidx).removeClass('active');
				$('.pagination span a').eq(curidx).prev('a').addClass('active');
				$('.pagination a:last').attr('href','javascript:void(0);');
				if(curidx == 0){
					$('.pagination a:eq(0)').removeAttr('href');
				}
			}
			else if($val.toLowerCase().indexOf('next') >= 0){
				var idx = +$('.pagination span a.active').html() + 1, toVal = idx * window.paginationdefaults.categoryLimit, fromVal = toVal - window.paginationdefaults.categoryLimit;
				
				if($('.pagination span a:last').hasClass('active')) return;
				paginationCur(fromVal, toVal);
				
				var curidx = $('.pagination span a.active').index(), pagesLen = $('.pagination li > span a').length-1;
				$('.pagination span a').eq(curidx).removeClass('active');
				$('.pagination span a').eq(curidx).next('a').addClass('active');
				$('.pagination a:first').attr('href','javascript:void(0);');
				if(curidx == pagesLen){
					$('.pagination a:last').removeAttr('href');
				} 
			}
			else{
				if(!$this.hasClass('active')){
				$('.pagination span a').removeClass('active').attr('href','javascript:void(0);');
				$this.addClass('active').removeAttr('href');
				toVal = window.paginationdefaults.categoryLimit * $val;
				fromVal = toVal - window.paginationdefaults.categoryLimit;
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

