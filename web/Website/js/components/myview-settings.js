function setClsforFlw(t) {
	for(var i=0; i<t.length; i++){
	  var tableFlwrow = $(t[i]).find('.followrow.disabled:eq(0)');
	  tableFlwrow.addClass('frow');
	}
}
 
function sort_table(tbody, col, asc, sortstatus) {
	var allrows = tbody[0].rows, rows = [];
		if(sortstatus === 'followingBtn'){
			for(var j = 0; j < allrows.length; j++){
				if(allrows[j].className == 'followrow disabled' || allrows[j].className == 'followrow disabled frow'){
					rows.push(allrows[j]);
				}
			}
		}
		else if(sortstatus === 'followingrow'){   
			for(var j = 0; j < allrows.length; j++){
				if(allrows[j].className == 'followingrow'){
					rows.push(allrows[j]);
				} 
			}
		}
		else if(sortstatus === 'followrow'){
			for(var j = 0; j < allrows.length; j++){
				if(allrows[j].className == 'followrow disabled' || allrows[j].className == 'followrow disabled frow'){
					rows.push(allrows[j]);
				}
			}
		}
		
		var rlen = rows.length,
		arr = new Array(),
		i, j, cells, clen;
	for (i = 0; i < rlen; i++) {
		cells = rows[i].cells;
		clen = cells.length;
		arr[i] = new Array();
		for (j = 0; j < clen; j++) {
			arr[i][j] = cells[j].innerHTML;
		}
	}
	// sort the array by the specified column number (col) and order (asc)
	arr.sort(function (a, b) {
		return (a[col] == b[col]) ? 0 : ((a[col] > b[col]) ? asc : -1 * asc);
	});
	// replace existing rows with new rows created from the sorted array
	for (i = 0; i < rlen; i++) {
		//rows[i].innerHTML = "<td class='wd-55'>" + arr[i].join("</td><td class='wd-25'>") + "</td>";
		rows[i].innerHTML = "<td class='wd-55'>" + arr[i][0] + "</td><td class='wd-25'>" + arr[i][1] + "</td><td class='wd-15'>" + arr[i][2] + "</td>";
	}
}

$(function(){
	$('#allPublicationsPan').on('click', '.followAllBtn', function(){
		var $this = $(this), curpublicPan = $this.closest('.publicationPan'), tbody = curpublicPan.find('tbody'), div = $this.closest('div'), $lgfollow = curpublicPan.find('.followBtn'), table = $('.table');
		$this.addClass('hideBtn');
		$('#validatePreference').val(1);
		div.find('.unfollowAllBtn').removeClass('hideBtn');
		
		$lgfollow.addClass('followingBtn').removeClass('followBtn').html('following');
		curpublicPan.find('.unfollowAllBtn').removeClass('hideBtn');
		for(var i=0; i<$lgfollow.length; i++){
			$($lgfollow[i], curpublicPan).closest('tr').removeAttr('class').addClass('followingrow');
		}
		setClsforFlw(table);
		sort_table(tbody, 0, 1, 'followingrow');
	});
	
	$('#allPublicationsPan').on('click', '.unfollowAllBtn', function(){
		var $this = $(this), curpublicPan = $this.closest('.publicationPan'), tbody = curpublicPan.find('tbody'), div = $this.closest('div'), $lgfollowing = curpublicPan.find('.followingBtn');
		$this.addClass('hideBtn');
		$('#validatePreference').val(1);
		div.find('.followAllBtn').removeClass('hideBtn');
		
		$lgfollowing.addClass('followBtn').removeClass('followingBtn').html('follow');
		for(var i=0; i<$lgfollowing.length; i++){
			$($lgfollowing[i], curpublicPan).closest('tr').removeAttr('class').addClass('followrow disabled');
		}
		sort_table(tbody, 0, 1, 'followrow');
	});
	
	$('#allPublicationsPan .donesubscribe').on('click', '.followrow .followBtn', function(){
	  var $this = $(this), followrow = $this.closest('.followrow'), table = $this.closest('.table'), followAllBtn = table.find('.followAllBtn'), unfollowAllBtn = table.find('.unfollowAllBtn'), trs = $this.closest('tbody').find('tr'), trsfollowing = $this.closest('tbody').find('tr.followingrow');
	  followrow.attr('draggable', true);
	  $('#validatePreference').val(1);
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
	  var $this = $(this), followingrow = $this.closest('.followingrow'), followAllBtn = $this.closest('table').find('.followAllBtn'), unfollowAllBtn = $this.closest('table').find('.unfollowAllBtn'), tbody = $this.closest('tbody'), trs = $this.closest('tbody').find('tr'), trsfollow = $this.closest('tbody').find('tr.followrow');
	  followingrow.addClass('followrow disabled').removeClass('followingrow');
	  $this.addClass('followBtn').removeClass('followingBtn').html('Follow');
	  followingrow.clone().appendTo($this.closest('tbody'));
	  console.log(followingrow.clone())
	  followingrow.remove();
	  $('#validatePreference').val(1);
	  sort_table(tbody, 0, 1, 'followingBtn');
	  if(trs.length === trsfollow.length+1){
		unfollowAllBtn.addClass('hideBtn');
		followAllBtn.removeClass('hideBtn');
	  }
	  else{
		followAllBtn.removeClass('hideBtn');
		unfollowAllBtn.removeClass('hideBtn');
	  }
	});
	
	$('.publicationPan').on('click', '.accordionImg a.mobileMode', function(){
		var $this = $(this), allPublications = $('#allPublicationsPan'), pPan = $this.closest('.publicationPan'), thead = pPan.find('thead'), tbody = pPan.find('tbody'), trs = tbody.find('tr'), disabledtrs = tbody.find('tr.disabled'), accCont = pPan.find('.accCont'), followlbl = thead.find('.followlbl'), followinglbl = thead.find('.followinglbl'), accStatusflwLbl = thead.find('.accordionStatus.flwLbl'), accStatusflwBtn = thead.find('.accordionStatus.flwBtn');
		 
		if($this.hasClass('expanded')){
			$this.removeClass('expanded');
			tbody.addClass('tbodyhidden');
			allPublications.find('.publicationPan thead tr.accordionStatus:first').addClass('hidden');
			accCont.addClass('tbodyhidden');
			thead.find('.expandHide').addClass('collapseshow');
			pPan.find('.smfollowingBtn').hide(); 
			//pPan.find('.graybg').hide();
			accStatusflwLbl.removeClass('hideRow');
			accStatusflwBtn.addClass('hideRow');
			thead.find('.mtp').addClass('hideBtn');
			if(trs.length === disabledtrs.length){
				followlbl.removeClass('hideBtn');
			}
			else{
				followinglbl.removeClass('hideBtn');
			}
			$(window).scrollTop(450);
		}
		else{
			allPublications.find('tbody').addClass('tbodyhidden');
			allPublications.find('.publicationPan .accordionImg a').removeClass('expanded');
			thead.find('tr').removeClass('hidden');
			$this.addClass('expanded');
			accStatusflwLbl.addClass('hideRow');
			accStatusflwBtn.removeClass('hideRow');
			tbody.removeClass('tbodyhidden');
			accCont.removeClass('tbodyhidden');
			thead.find('.expandHide').removeClass('collapseshow');
			thead.find('.mtp').addClass('hideBtn');
			pPan.find('.smfollowingBtn').show();
			$(window).scrollTop(450);
			
			//pPan.find('.graybg').show();
			
			/*var alltheads = allPublications.find('thead'), alltbodys = allPublications.find('tbody');
			for(var i = 0; i < alltbodys.length; i++){
				var eachTableTrs = $(alltbodys[i]).find('tr'), eachTabledisTrs = $(alltbodys[i]).find('tr.disabled'),
					eachTablefollowlbl = $(alltheads[i]).find('.followlbl'), eachTablefollowinglbl = $(alltheads[i]).find('.followinglbl');
				if(eachTableTrs.length === eachTabledisTrs.length){
					eachTablefollowlbl.removeClass('hideBtn');
				}
				else{
					eachTablefollowinglbl.removeClass('hideBtn');
				}
			}*/
		}
	});
	
	$('.publicationPan').on('click', '.accordionImg a.desktopMode', function(){
		var $this = $(this), allPublications = $('#allPublicationsPan'), pPan = $this.closest('.publicationPan'), accCont = pPan.find('.accCont'), thead = pPan.find('thead'), tbody = pPan.find('tbody'), trs = tbody.find('tr'), disabledtrs = tbody.find('tr.disabled'), flwlbl = thead.find('.flwLbl'), flwBtn = thead.find('.flwBtn'), followlbl = thead.find('.followlbl'), followinglbl = thead.find('.followinglbl');
		 
		if($this.hasClass('expanded')){
			$this.removeClass('expanded');
			tbody.addClass('tbodyhidden');
			thead.find('.mtp').addClass('hideBtn'); 
			accCont.addClass('tbodyhidden');
			pPan.find('.smfollowingBtn').hide();
			if(trs.length === disabledtrs.length){
				followlbl.removeClass('hideBtn');
			}
			else{
				followinglbl.removeClass('hideBtn');
			}
			$(window).scrollTop(500);
		}
		else{
			allPublications.find('tbody').addClass('tbodyhidden');
			allPublications.find('.publicationPan .accordionImg a').removeClass('expanded');
			allPublications.find('.publicationPan thead tr').not(':nth-child(1)').addClass('hidden');
			thead.find('tr').removeClass('hidden');
			$this.addClass('expanded');
			thead.find('.mtp').addClass('hideBtn');
			accCont.removeClass('tbodyhidden');
			tbody.removeClass('tbodyhidden'); 
			flwBtn.addClass('hideRow');
			flwlbl.removeClass('hideRow');
			pPan.find('.smfollowingBtn').show();
			$(window).scrollTop(500);
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
				var eachrowAttr = $(currenttabtrs[j]).find('input[type=hidden]').attr('data-row-topic'),
				    secondtd = $(currenttabtrs[j]).find('td.wd-25 span').html(),
					datarowNo = secondtd.toLowerCase() == 'following' ? $(currenttabtrs[j]).attr('data-row') : '0'; 
				
				var followStatus = (secondtd.toLowerCase() == 'following') ? true : false;
				var subscripStatus = (subscribeStatus.toUpperCase()) == 'SUBSCRIBED' ? true : false;
				alltdata.push({ 'TopicCode': eachrowAttr, 'TopicOrder': datarowNo, 'IsFollowing': followStatus });
			}
			UserPreferences.PreferredChannels.push({ "ChannelCode": publicationName, "ChannelOrder": pubPanPosition, Topics: alltdata });
		}
		$.post('/Account/api/PersonalizeUserPreferencesApi/Update/', { 'UserPreferences': JSON.stringify(UserPreferences) });
		$('#validatePreference').val(0);
	});
	
	$('.gotoview').click(function(e){
		if(+$('#validatePreference').val()){
			e.preventDefault();
			$('.modal-overlay').addClass('in');
			$('.modal-view').show();
		}
	});
	$('.close-modal').click(function(){
		$('.modal-overlay').removeClass('in');
		$('.modal-view').hide();
	});
	 
	/*if (window.matchMedia('(max-width: 630px)').matches){
		$('.mobshowView').removeClass('desktophide');
	}
	else{
		$('.mobshowView').addClass('desktophide');
		
	}*/
	
	$('.publicationPan.donesubscribe').dragswap({
		element : '.table tbody tr',
		dropAnimation: true  
	}); 
	
	$('#allPublicationsPan').dragswap({
		element : '.publicationPan.donesubscribe',
		dropAnimation: true  
	}); 

});








