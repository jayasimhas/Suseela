var defaults = {
                totalCategories: 30,
                categoryLimit: 10,
                currentPage: 1,
                paginationEle: 'table',
},  toVal = 1, fromVal = 1; 
 
$.fn.setPagination = function(source){
    $.extend(defaults, source);
};

function paginationCur(fv, tv){
		if(defaults.paginationEle === 'table'){
						$('tbody.hidden-xs tr', '.page-account__table').hide();
						$('tbody.hidden-lg tr', '.page-account__table').hide();
						for(var i = fv; i < tv; i++){
										$('tbody.hidden-xs tr', '.page-account__table').eq(i).show();
										$('tbody.hidden-lg tr', '.page-account__table').eq(i).show();
						}
		}
		else{
						$(defaults.paginationEle).hide();
						for(var i = fv; i < tv; i++){
										$(defaults.paginationEle).eq(i).show();
						}
		}
}

$(function(){
	var showPageLinks = Math.ceil(defaults.totalCategories / defaults.categoryLimit);
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
										var idx = +$('.pagination span a.active').html() - 1, toVal = idx * defaults.categoryLimit, fromVal = toVal - defaults.categoryLimit;
										
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
										var idx = +$('.pagination span a.active').html() + 1, toVal = idx * defaults.categoryLimit, fromVal = toVal - defaults.categoryLimit;
										
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
														toVal = defaults.categoryLimit * $val;
														fromVal = toVal - defaults.categoryLimit;
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

	$('.sortable-table__header').on('click', '.sortable-table__col', function(){
		var $this = $(this), table = $this.closest('.sortable-table'), tbodytrs = table.find('tbody tr');
		setTimeout(function(){
			tbodytrs.removeAttr('style');
			if(!$('.pagination span a:eq(0)').hasClass('active')){
				$('.pagination span a:eq(0)').click();
			}
			else{
				paginationCur(0, defaults.categoryLimit);
			}
		}, 1);
	});
});

