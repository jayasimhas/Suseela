function setClsforFlw(t) {
	for(var i=0; i<t.length; i++){
	  var tableFlwrow = $(t[i]).find('.followrow.disabled:eq(0)');
	  tableFlwrow.addClass('frow');
	}
}

function createJSONData(alltables, UserPreferences){
	for (var i = 0; i < alltables.length; i++) {
		var currenttabtrs = $(alltables[i]).find('tbody tr'),
			pubPanPosition = $(alltables[i]).closest('.publicationPan').attr('data-row'),
			tableId = $(alltables[i]).attr('id'),
			publicationName = $(alltables[i]).find('h2').attr('data-publication'),
			subscribeStatus = $(alltables[i]).find('.subscribed').html(),
			channelId = $(alltables[i]).find('h2').attr('data-item-id');
		var alltdata = [];
		for (var j = 0; j < currenttabtrs.length; j++) {
			var eachrowAttr = $(currenttabtrs[j]).find('input[type=hidden]').attr('data-row-topic'),
				topicId = $(currenttabtrs[j]).find('input[type=hidden]').attr('data-row-item-id'),
				secondtd = $(currenttabtrs[j]).find('td.wd-25 span').html(),
				datarowNo = secondtd.toLowerCase() == 'following' ? $(currenttabtrs[j]).attr('data-row') : '0'; 
			
			var followStatus = (secondtd.toLowerCase() == 'following') ? true : false;
			var subscripStatus = (subscribeStatus.toUpperCase()) == 'SUBSCRIBED' ? true : false;
			
			alltdata.push({ 'TopicCode': eachrowAttr, 'TopicOrder': datarowNo, 'IsFollowing': followStatus, 'TopicId': topicId });
		}
		UserPreferences.PreferredChannels.push({ "ChannelCode": publicationName, "ChannelOrder": pubPanPosition, "ChannelId" : channelId, Topics: alltdata });
	}
	sendHttpRequest(UserPreferences);
}

function sendHttpRequest(UserPreferences, setFlag){
	$.ajax({
		url: '/Account/api/PersonalizeUserPreferencesApi/Update/', 
		data: {'UserPreferences': JSON.stringify(UserPreferences)}, 
		dataType: 'json',
		type: 'POST',
		success: function(data){
			if(data && data.success){
				$('.alert-success p').html(data.reason);
				$('.alert-success').show();
				if(setFlag == 'register'){
					window.location.href = '/';
				}
			}
			else{
				if(setFlag == 'register'){
					$('.alert-error.register-error p').html(data.reason);
					$('.alert-error.register-error').show();
					setRegisterFlag = false;
				}
				else{
					$('.alert-error.myview-error p').html(data.reason);
					$('.alert-error.myview-error').show();
				}
			}
		},
		error: function(err){
			if(err && !err.success){
				if(setFlag == 'register'){
					$('.alert-error.register-error p').html(data.reason);
					$('.alert-error.register-error').show();
					setRegisterFlag = false;
				}
				else{
					$('.alert-error.myview-error p').html(data.reason);
					$('.alert-error.myview-error').show();
				}
			}
		}
	});
}

function setDataRow(allpublications){
	for(var k = 0; k < allpublications.length; k++){
		var tbody = $(allpublications[k]).find('tbody'), newtrs = tbody.find('tr');
		newtrs.removeAttr('data-row');
		for(var v = 0; v < newtrs.length; v++){
			$(newtrs[v]).attr('data-row', v+1);
		}
	}
}

function sort_table(tbody, col, asc, sortstatus) {
	var rows = [];
	if(tbody[0] && tbody[0].rows){
		var allrows = tbody[0].rows
	}
	else{
		return;
	}
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
		curpublicPan.find('.firstrow .lableStatus').val('followinglbl');
		curpublicPan.find('.accordionStatus .lableStatus').val('followinglbl');
		$lgfollow.addClass('followingBtn').removeClass('followBtn').html('following');
		
		for(var i=0; i<tbody.find('.followingBtn').length; i++){
						$(tbody.find('.followrow')[i]).attr('draggable', true);
		}
		
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
		$this.closest('.smfollowingBtn').find('.followAllBtn').addClass('fr');
		$('#validatePreference').val(1); 
		div.find('.followAllBtn').removeClass('hideBtn');
		curpublicPan.find('.firstrow .lableStatus').val('followlbl');
		curpublicPan.find('.accordionStatus .lableStatus').val('followlbl');
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
	  table.find('.firstrow .lableStatus').val('followinglbl');
	  table.find('.accordionStatus .lableStatus').val('followinglbl');
	  table.find('.followAllBtn').removeClass('fr');
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
	  var $this = $(this), table = $this.closest('table'), followingrow = $this.closest('.followingrow'), followAllBtn = $this.closest('table').find('.followAllBtn'), unfollowAllBtn = $this.closest('table').find('.unfollowAllBtn'), tbody = $this.closest('tbody'), trs = $this.closest('tbody').find('tr'), disabledtrs = $this.closest('tbody').find('.followrow.disabled'), trsfollow = $this.closest('tbody').find('tr.followrow');
	  followingrow.addClass('followrow disabled').removeClass('followingrow');
	  $this.addClass('followBtn').removeClass('followingBtn').html('Follow');
	  followingrow.clone().appendTo($this.closest('tbody'));
	  followingrow.remove();
	  $('#validatePreference').val(1);
	  table.find('.followAllBtn').removeClass('fr');
	  sort_table(tbody, 0, 1, 'followingBtn');
	  
	  if(trs.length === disabledtrs.length+1){
		table.find('.firstrow .lableStatus').val('followlbl');
		table.find('.accordionStatus .lableStatus').val('followlbl');
	  }
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
		var $this = $(this), allPublications = $('#allPublicationsPan'), pPan = $this.closest('.publicationPan'), thead = pPan.find('thead'), tbody = pPan.find('tbody'), trs = tbody.find('tr'), disabledtrs = tbody.find('tr.disabled'), followlbl = thead.find('.followlbl'), followinglbl = thead.find('.followinglbl'), accStatusflwLbl = thead.find('.accordionStatus.flwLbl'), accStatusflwBtn = thead.find('.accordionStatus.flwBtn'), allpubpans = allPublications.find('.publicationPan'), pickTxt = thead.find('.pickTxt'), setFlag = true;
		 
		if($this.hasClass('expanded')){
			setFlag = false;
			tbody.addClass('tbodyhidden');
			//pPan.find('.smfollowingBtn').hide();  
			accStatusflwLbl.removeClass('hideRow');
			accStatusflwBtn.addClass('hideRow');
			thead.find('.mtp').addClass('hideBtn');
			
			for(var i=0; i<pickTxt.length; i++){
				$(pickTxt[i]).closest('.accordionStatus').addClass('hideRow');
			}
			if(trs.length === disabledtrs.length){
				followlbl.removeClass('hideBtn');
			}
			else{
				followinglbl.removeClass('hideBtn');
			}
			var position = $this.closest('.publicationPan').position();
			$(window).scrollTop(position.top - 20);
		}
		else{
			allPublications.find('tbody').addClass('tbodyhidden');
			for(var i=0; i<allpubpans.length; i++){
				var eachPickTxt = $(allpubpans[i]).find('thead .pickTxt');
				for(var j = 0; j < eachPickTxt.length; j++){
					$(eachPickTxt[j]).closest('.accordionStatus').addClass('hideRow');;
				}
			}
			thead.find('tr').removeClass('hidden');
			tbody.removeClass('tbodyhidden');
			pPan.find('.smfollowingBtn').show();
			for(var i=0; i<pickTxt.length; i++){
				$(pickTxt[i]).closest('.accordionStatus').removeClass('hideRow');
			}
			if(setFlag){
				for(var i = 0; i < allpubpans.length; i++){
					$(allpubpans[i]).find('.accordionStatus.flwLbl').removeClass('hideRow');
					$(allpubpans[i]).find('.accordionStatus.flwBtn').addClass('hideRow');
				}
			}
			accStatusflwLbl.addClass('hideRow');
			accStatusflwBtn.removeClass('hideRow');
			
			var position = $this.closest('.publicationPan').position();
			$(window).scrollTop(position.top - 20);
			
			for(var i = 0; i < allpubpans.length; i++){
				var labelVal = $(allpubpans[i]).find('.firstrow .lableStatus').val();
				$('.' + labelVal, allpubpans[i]).removeClass('hideBtn');
			}
			thead.find('.mtp').addClass('hideBtn');
		}
	});
	
	$('.publicationPan').on('click', '.accordionImg a.desktopMode', function(){
		var $this = $(this), allPublications = $('#allPublicationsPan'), pPan = $this.closest('.publicationPan'), accCont = pPan.find('.accCont'), thead = pPan.find('thead'), tbody = pPan.find('tbody'), trs = tbody.find('tr'), disabledtrs = tbody.find('tr.disabled'), flwlbl = thead.find('.flwLbl'), flwBtn = thead.find('.flwBtn'), followlbl = thead.find('.followlbl'), followinglbl = thead.find('.followinglbl'), allpubpans = allPublications.find('.publicationPan');
		 
		if($this.hasClass('expanded')){
			$this.removeClass('expanded');
			tbody.addClass('tbodyhidden');
			thead.find('.mtp').addClass('hideBtn'); 
			accCont.addClass('tbodyhidden'); 
			if(trs.length === disabledtrs.length){
				followlbl.removeClass('hideBtn');
				thead.find('.firstrow .lableStatus').val('followlbl');
			}
			else{
				followinglbl.removeClass('hideBtn');
				thead.find('.firstrow .lableStatus').val('followinglbl');
			}
			var position = $this.closest('.publicationPan').position();
			$(window).scrollTop(position.top);
		}
		else{
			allPublications.find('tbody').addClass('tbodyhidden');
			allPublications.find('.publicationPan .accordionImg a').removeClass('expanded');
			allPublications.find('.publicationPan thead tr').not(':nth-child(1)').addClass('hidden');
			allPublications.find('.publicationPan thead tr.showinview').removeClass('hidden');
			thead.find('tr').removeClass('hidden');
			$this.addClass('expanded'); 
			accCont.removeClass('tbodyhidden');
			tbody.removeClass('tbodyhidden'); 
			flwBtn.addClass('hideRow');
			flwlbl.removeClass('hideRow');
			
			for(var i = 0; i < allpubpans.length; i++){
				var labelVal = $(allpubpans[i]).find('.firstrow .lableStatus').val();
				$('.' + labelVal, allpubpans[i]).removeClass('hideBtn');
			}
			thead.find('.mtp').addClass('hideBtn');
			
			var position = $this.closest('.publicationPan').position();
			$(window).scrollTop(position.top);
		}
	});
	
	var tables = $('.publicationPan table');
	setClsforFlw(tables);
	
	$('.saveview').click(function () {
		var alltables = $('.table'),
		    UserPreferences = {}, allpublications = $('.publicationPan', '#allPublicationsPan');
		UserPreferences.PreferredChannels = [];
		
		setDataRow(allpublications);
		createJSONData(alltables, UserPreferences);		
		
		$('#validatePreference').val(0);
	});
	
	$('.registrationBtn').click(function (e) {
		var table = $('.table', '.publicationPan'), alltrs = table.find('tbody tr'),
		    UserPreferences = {}, allpublications = $('.publicationPan', '#allPublicationsPan');
			UserPreferences.PreferredChannels = [];
		
		e.preventDefault();
		if(!+$('#validatePreference').val()){
			$('.alert-error.register-not-selected').show();
			return false;
		}
		setDataRow(allpublications);
		
		if(!!$('#isChannelBasedRegistration').val()){
			for (var i = 0; i < alltrs.length; i++) {
				var eachrowAttr = $(alltrs[i]).find('input[type=hidden]').attr('data-row-topic'),
					channelId = $(alltrs[i]).find('input[type=hidden]').attr('data-row-item-id'),
					secondtd = $(alltrs[i]).find('td.wd-25 span').html(),
					channelOrder = (secondtd.toLowerCase() == 'following') ? $(alltrs[i]).attr('data-row') : '0',
					followStatus = (secondtd.toLowerCase() == 'following') ? true : false;
				
				UserPreferences.PreferredChannels.push({ "ChannelCode": eachrowAttr, "ChannelOrder": channelOrder, "IsFollowing": followStatus, "ChannelId": channelId, "Topics": [] });
			}
			sendHttpRequest(UserPreferences, 'register');
		}
		else{
			createJSONData(table, UserPreferences);
		}
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

	$('.publicationPan.donesubscribe').dragswap({
		element : '.table tbody tr',
		dropAnimation: true  
	}); 
	
	$('#allPublicationsPan').dragswap({
		element : '.publicationPan.donesubscribe',
		dropAnimation: true  
	}); 

});








