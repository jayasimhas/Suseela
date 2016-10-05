function setClsforFlw(t) {
	for(var i=0; i<t.length; i++){
	  var tableFlwrow = $(t[i]).find('.followrow.disabled:eq(0)');
	  tableFlwrow.addClass('frow');
	}
}
$(function(){
	
	$('#allPublicationsPan').on('click', '.followAllBtn', function(){
		var curpublicPan = $(this).closest('.publicationPan'), $lgfollow = curpublicPan.find('.followBtn'), table = $('.table');
		$lgfollow.addClass('followingBtn').removeClass('followBtn').html('following');
		
		curpublicPan.find('.unfollowAllBtn').removeClass('hideBtn');
		
		for(var i=0; i<$lgfollow.length; i++){
			$($lgfollow[i], curpublicPan).closest('tr').removeAttr('class').addClass('followingrow');
		}
		setClsforFlw(table);
	});
	
	$('#allPublicationsPan').on('click', '.unfollowAllBtn', function(){
		var curpublicPan = $(this).closest('.publicationPan'), $lgfollowing = curpublicPan.find('.followingBtn');
		$(this).addClass('hideBtn');
		$lgfollowing.addClass('followBtn').removeClass('followingBtn').html('follow');
		
		for(var i=0; i<$lgfollowing.length; i++){
			$($lgfollowing[i], curpublicPan).closest('tr').removeAttr('class').addClass('followrow disabled ufa');
		}
	});
	
	$('#allPublicationsPan').on('click', '.mcheckedAll', function(){
		var $this = $(this), mcall = $this.closest('.mca'), muall = $this.closest('.smfollowingBtn').find('.mua'), curpublicPan = $(this).closest('.publicationPan'), mchecked = curpublicPan.find('.mchecked');
		mcall.addClass('hideBtn');
		muall.removeClass('hideBtn');
		
		for(var i=0; i<mchecked.length; i++){
			$(mchecked[i], curpublicPan).addClass('munchecked').removeClass('mchecked');
		}
	});
	
	$('#allPublicationsPan').on('click', '.muncheckedAll', function(){
		var $this = $(this), mcall = $this.closest('.smfollowingBtn').find('.mca'), muall = $this.closest('.mua'), curpublicPan = $(this).closest('.publicationPan'), munchecked = curpublicPan.find('.munchecked');
		muall.addClass('hideBtn');
		mcall.removeClass('hideBtn');
		
		for(var i=0; i<munchecked.length; i++){
			$(munchecked[i], curpublicPan).addClass('mchecked').removeClass('munchecked');
		}
	});
	
	$('#allPublicationsPan .donesubscribe').on('click', '.smfollowingBtn .mchecked', function(){
		var $this = $(this), smfollowingBtn = $this.closest('.smfollowingBtn');
		smfollowingBtn.find('a').addClass('munchecked').removeClass('mchecked');
	});
	
	$('#allPublicationsPan .donesubscribe').on('click', '.smfollowingBtn .munchecked', function(){
		var $this = $(this), smfollowingBtn = $this.closest('.smfollowingBtn');
		smfollowingBtn.find('a').addClass('mchecked').removeClass('munchecked');
	});
	
	$('#allPublicationsPan .donesubscribe').on('click', '.followrow .followBtn', function(){
	  var $this = $(this), followrow = $this.closest('.followrow'), table = $this.closest('.table');
	  followrow.addClass('followingrow').removeClass('followrow disabled frow');
	  $this.addClass('followingBtn').removeClass('followBtn').html('Following');
	  setClsforFlw(table);
	  if($('.followrow.disabled.frow', table).length){
		  followrow.appendTo(followrow.clone().insertBefore('.followrow.disabled.frow'));
	  }
	  else{
		followrow.clone().appendTo($this.closest('tbody'));
	  }
	  followrow.remove(); 
	});
	
	$('#allPublicationsPan .donesubscribe').on('click', '.followingrow .followingBtn', function(){
	  var $this = $(this), followingrow = $this.closest('.followingrow');
	  followingrow.addClass('followrow disabled').removeClass('followingrow');
	  $this.addClass('followBtn').removeClass('followingBtn').html('Follow');
	  followingrow.clone().appendTo($this.closest('tbody'));
	  followingrow.remove();
	});
	
	$('.publicationPan').on('click', '.accordionImg a', function(){
		var $this = $(this), pPan = $this.closest('.publicationPan'), tbody = pPan.find('tbody');
		if($this.hasClass('collapsed')){
			$this.removeClass('collapsed');
			tbody.addClass('tbodyhidden');
		}
		else{
			$this.addClass('collapsed');
			tbody.removeClass('tbodyhidden');
		}
	});
	
	var tables = $('.publicationPan table');
	setClsforFlw(tables);
	
	$('.saveview').click(function(){
		var alltables = $('.table'), createtableData = {};
		createtableData.allpublications = {};
		for(var i=0; i<alltables.length; i++){
			var currenttabtrs = $(alltables[i]).find('tbody tr'), pubPanPosition = $(alltables[i]).closest('.publicationPan').attr('data-row'), tableId = $(alltables[i]).attr('id'), publicationName = $(alltables[i]).find('h2').html(), subscribeStatus = $(alltables[i]).find('.subscribed').html();
			var alltdata = [];
			for(var j=0; j<currenttabtrs.length; j++){
				var datarowNo = $(currenttabtrs[j]).attr('data-row'), firsttd = $(currenttabtrs[j]).find('td.wd-55').html(), secondtd = $(currenttabtrs[j]).find('td.wd-25 span:first').html();
				alltdata.push({'tableRowNo': datarowNo, 'topic': firsttd, 'subStatus': secondtd});
			}
			createtableData.allpublications[tableId] = {"publicationName": publicationName, "subscribeStatus": subscribeStatus, "position": pubPanPosition, "tableData": alltdata}
		}
		
		console.log(JSON.stringify(createtableData));
	});

});








