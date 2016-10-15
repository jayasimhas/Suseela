function setClsforFlw(t) {
	for(var i=0; i<t.length; i++){
	  var tableFlwrow = $(t[i]).find('.followrow.disabled:eq(0)');
	  tableFlwrow.addClass('frow');
	}
}
$(function(){
	$('#allPublicationsPan').on('click', '.followAllBtn', function(){
		var $this = $(this), curpublicPan = $this.closest('.publicationPan'), div = $this.closest('div'), $lgfollow = curpublicPan.find('.followBtn'), table = $('.table');
		$this.addClass('hideBtn');
		div.find('.unfollowAllBtn').removeClass('hideBtn');
		$lgfollow.addClass('followingBtn').removeClass('followBtn').html('following');
		curpublicPan.find('.unfollowAllBtn').removeClass('hideBtn');
		for(var i=0; i<$lgfollow.length; i++){
			$($lgfollow[i], curpublicPan).closest('tr').removeAttr('class').addClass('followingrow');
		}
		setClsforFlw(table);
	});
	
	$('#allPublicationsPan').on('click', '.unfollowAllBtn', function(){
		var $this = $(this), curpublicPan = $this.closest('.publicationPan'), div = $this.closest('div'), $lgfollowing = curpublicPan.find('.followingBtn');
		$this.addClass('hideBtn');
		div.find('.followAllBtn').removeClass('hideBtn');
		$lgfollowing.addClass('followBtn').removeClass('followingBtn').html('follow');
		for(var i=0; i<$lgfollowing.length; i++){
			$($lgfollowing[i], curpublicPan).closest('tr').removeAttr('class').addClass('followrow disabled ufa');
		}
	});
	
	$('#allPublicationsPan .donesubscribe').on('click', '.followrow .followBtn', function(){
	  var $this = $(this), followrow = $this.closest('.followrow'), table = $this.closest('.table'), followAllBtn = table.find('.followAllBtn'), unfollowAllBtn = table.find('.unfollowAllBtn'), trs = $this.closest('tbody').find('tr'), trsfollowing = $this.closest('tbody').find('tr.followingrow');
	  followrow.attr('draggable', true);
	  followrow.addClass('followingrow').removeClass('followrow disabled frow');
	  $this.addClass('followingBtn').removeClass('followBtn').html('Following');
	  setClsforFlw(table);
	  if($('.followrow.disabled.frow', table).length){
		  followrow.appendTo(followrow.clone().insertBefore(table.find('.followrow.disabled.frow')));
	  }
	  else{
		followrow.clone().appendTo($this.closest('tbody'));
	  }
	  followrow.remove();
	  if(trs.length === trsfollowing.length+1){
		followAllBtn.addClass('hideBtn');
		unfollowAllBtn.removeClass('hideBtn');
	  }
	  else{
		followAllBtn.removeClass('hideBtn');
		unfollowAllBtn.removeClass('hideBtn');
	  }
	});
	
	$('#allPublicationsPan .donesubscribe').on('click', '.followingrow .followingBtn', function(){
	  var $this = $(this), followingrow = $this.closest('.followingrow'), followAllBtn = $this.closest('table').find('.followAllBtn'), unfollowAllBtn = $this.closest('table').find('.unfollowAllBtn'), trs = $this.closest('tbody').find('tr'), trsfollow = $this.closest('tbody').find('tr.followrow');
	  followingrow.addClass('followrow disabled').removeClass('followingrow');
	  $this.addClass('followBtn').removeClass('followingBtn').html('Follow');
	  followingrow.clone().appendTo($this.closest('tbody'));
	  followingrow.remove();
	  
	  if(trs.length === trsfollow.length+1){
		unfollowAllBtn.addClass('hideBtn');
		followAllBtn.removeClass('hideBtn');
	  }
	  else{
		followAllBtn.removeClass('hideBtn');
		unfollowAllBtn.removeClass('hideBtn');
	  }
	});
	
	$('.publicationPan').on('click', '.accordionImg a', function(){
		var $this = $(this), pPan = $this.closest('.publicationPan'), thead = pPan.find('thead'), tbody = pPan.find('tbody'), trs = tbody.find('tr'), disabledtrs = tbody.find('tr.disabled'), accCont = pPan.find('.accCont'), followlbl = thead.find('.followlbl'), followinglbl = thead.find('.followinglbl'); 
		if($this.hasClass('collapsed')){
			$this.removeClass('collapsed');
			tbody.addClass('tbodyhidden');
			accCont.addClass('tbodyhidden');
			pPan.find('.smfollowingBtn').hide();
			pPan.find('.graybg').hide(); 
			thead.find('.mtp').addClass('hideBtn'); 
			if(trs.length === disabledtrs.length){
				followlbl.removeClass('hideBtn');
			}
			else{
				followinglbl.removeClass('hideBtn');
			}
		}
		else{
			$this.addClass('collapsed');
			tbody.removeClass('tbodyhidden');
			accCont.removeClass('tbodyhidden');
			thead.find('.mtp').addClass('hideBtn');
			pPan.find('.smfollowingBtn').show();
			pPan.find('.graybg').show();
		}
	});
	
	var tables = $('.publicationPan table');
	setClsforFlw(tables);
	
	
	
	$('.saveview').click(function () {
		var alltables = $('.table'),
		    UserPreferences = {}, allpublications = $('.publicationPan', '#allPublicationsPan');
		UserPreferences.PreferredChannels = [];
		
		for(var k = 0; k < allpublications.length; k++){
			var tbody = $(allpublications[k]).find('tbody'), newtrs = tbody.find('tr'), cnt = 0;
			newtrs.removeAttr('data-row');
			for(var v = 0; v < newtrs.length; v++){
				$(newtrs[v]).attr('data-row', v+1);
			}
		}
		 
		for (var i = 0; i < alltables.length; i++) {
			var currenttabtrs = $(alltables[i]).find('tbody tr'),
			    pubPanPosition = $(alltables[i]).closest('.publicationPan').attr('data-row'),
			    tableId = $(alltables[i]).attr('id'),
			    publicationName = $(alltables[i]).find('h2').attr('data-publication'),
			    subscribeStatus = $(alltables[i]).find('.subscribed').html();
			var alltdata = [];
			for (var j = 0; j < currenttabtrs.length; j++) {
				var datarowNo = $(currenttabtrs[j]).attr('data-row'),
				    eachrowAttr = $(currenttabtrs[j]).find('input[type=hidden]').attr('data-row-topic'),
				    secondtd = $(currenttabtrs[j]).find('td.wd-25 span').html();

				var followStatus = (secondtd.toLowerCase() == 'following') ? true : false;
				var subscripStatus = (subscribeStatus.toUpperCase()) == 'SUBSCRIBED' ? true : false;
				alltdata.push({ 'TopicCode': eachrowAttr, 'TopicOrder': datarowNo, 'IsFollowing': followStatus });
			}
			UserPreferences.PreferredChannels.push({ "ChannelCode": publicationName, "ChannelOrder": pubPanPosition, Topics: alltdata });
		}
		$.post('/Account/api/PersonalizeUserPreferencesApi/Update/', { 'UserPreferences': JSON.stringify(UserPreferences) });
	}); 
	
	if (window.matchMedia('(max-width: 630px)').matches){
		$('.mobshowView').removeClass('desktophide');
	}
	else{
		$('.mobshowView').addClass('desktophide');
		
	}
	
	$('.publicationPan.donesubscribe').dragswap({
		element : '.table tbody tr',
		dropAnimation: true  
	}); 
	
	$('#allPublicationsPan').dragswap({
		element : '.publicationPan.donesubscribe',
		dropAnimation: true  
	}); 

});








