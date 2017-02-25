//import { analyticsEvent } from '../controllers/analytics-controller'; 

function setClsforFlw(t) {
    for (var i = 0; i < t.length; i++) {
        var tableFlwrow = $(t[i]).find('.followrow.disabled:eq(0)');
        tableFlwrow.addClass('frow');
    }
}

function createJSONData(alltables, UserPreferences, url) {
    for (var i = 0; i < alltables.length; i++) {
        var currenttabtrs = $(alltables[i]).find('tbody tr'),
			pubPanPosition = $(alltables[i]).closest('.publicationPan').attr('data-row'),
			tableId = $(alltables[i]).attr('id'),
			publicationName = $(alltables[i]).find('h2').attr('data-publication'),
			subscribeStatus = $(alltables[i]).find('.subscribed').html(),
			channelId = $(alltables[i]).find('h2').attr('data-item-id'),
		channelStatus = $(alltables[i]).find('h2').attr('data-item-status');
        var alltdata = [];
        for (var j = 0; j < currenttabtrs.length; j++) {
            var eachrowAttr = $(currenttabtrs[j]).find('input[type=hidden]').attr('data-row-topic'),
				topicId = $(currenttabtrs[j]).find('input[type=hidden]').attr('data-row-item-id'),
				secondtd = $(currenttabtrs[j]).find('td.wd-25 span').html(),
				datarowNo = $(currenttabtrs[j]).attr('data-row');

            var followStatus = (secondtd == $('#followingButtonText').val()) ? true : false;
            var subscripStatus = (subscribeStatus.toUpperCase()) == 'SUBSCRIBED' ? true : false;

            alltdata.push({ 'TopicCode': eachrowAttr, 'TopicOrder': datarowNo, 'IsFollowing': followStatus, 'TopicId': topicId });
        }
        UserPreferences.PreferredChannels.push({ "ChannelCode": publicationName, "ChannelOrder": pubPanPosition, "IsFollowing": channelStatus, "ChannelId": channelId, Topics: alltdata });
    }
    sendHttpRequest(UserPreferences, null, url);
}

function sendHttpRequest(UserPreferences, setFlag, redirectUrl) {
    $.ajax({
        url: '/Account/api/PersonalizeUserPreferencesApi/Update/',
        data: { 'UserPreferences': JSON.stringify(UserPreferences) },
        dataType: 'json',
        type: 'POST',
        success: function (data) {
            if (data && data.success) {
                $('.alert-success p').html(data.reason);
                $('.alert-success').show();
                if ($('.alert-success').length > 0) {
                    $(window).scrollTop($('.informa-ribbon').offset().top + $('.informa-ribbon').height());
                }
                if (setFlag == 'register' && redirectUrl == 'href') {
                    window.location.href = $('.registrationBtn').attr('href');
                }
                else if (setFlag == 'register' && redirectUrl == 'name') {
                    window.location.href = $('.registrationBtn').attr('name');
                }
				if(redirectUrl != 'href' && redirectUrl != 'name'){
					window.location.href = redirectUrl;
				}
            }
            else {
				if(redirectUrl != 'href' && redirectUrl != 'name'){
					window.location.href = redirectUrl;
				}
                if (setFlag == 'register') {
                    $('.alert-error.register-error p').html(data.reason);
                    $('.alert-error.register-error').show();
                }
                else {
                    $('.alert-error.myview-error p').html(data.reason);
                    $('.alert-error.myview-error').show();
                }
            }
        },
        error: function (err) {
            if (err && !err.success) {
                if (setFlag == 'register') {
                    $('.alert-error.register-error p').html(data.reason);
                    $('.alert-error.register-error').show();
                }
                else {
                    $('.alert-error.myview-error p').html(data.reason);
                    $('.alert-error.myview-error').show();
                }
            }
        }
    });
}

function setDataRow(allpublications) {
    for (var k = 0; k < allpublications.length; k++) {
        var tbody = $(allpublications[k]).find('tbody'), newtrs = tbody.find('tr');
        newtrs.removeAttr('data-row');
        for (var v = 0; v < newtrs.length; v++) {
            $(newtrs[v]).attr('data-row', v + 1);
        }
    }
}

function showModal() {
    $('.modal-overlay').addClass('in');
    $('.modal-view').show();
}

function sendRegisterData(alltrs, UserPreferences, redirectUrl) {
    for (var i = 0; i < alltrs.length; i++) {
        var eachrowAttr = $(alltrs[i]).find('input[type=hidden]').attr('data-row-topic'),
			channelId = $(alltrs[i]).find('input[type=hidden]').attr('data-row-item-id'),
			secondtd = $(alltrs[i]).find('td.wd-25 span').html(),
			channelOrder = $(alltrs[i]).attr('data-row'),
			followStatus = (secondtd == $('#followingButtonText').val()) ? true : false;

        UserPreferences.PreferredChannels.push({ "ChannelCode": eachrowAttr, "ChannelOrder": channelOrder, "IsFollowing": followStatus, "ChannelId": channelId, "Topics": [] });
    }
    sendHttpRequest(UserPreferences, 'register', redirectUrl);
}

function sort_table(tbody, col, asc, sortstatus) {
    var rows = [];
    if (tbody[0] && tbody[0].rows) {
        var allrows = tbody[0].rows
    }
    else {
        return;
    }
    if (sortstatus === 'followingBtn') {
        for (var j = 0; j < allrows.length; j++) {
            if (allrows[j].className == 'followrow disabled' || allrows[j].className == 'followrow disabled frow') {
                rows.push(allrows[j]);
            }
        }
    }
    else if (sortstatus === 'followingrow') {
        for (var j = 0; j < allrows.length; j++) {
            if (allrows[j].className == 'followingrow') {
                rows.push(allrows[j]);
            }
        }
    }
    else if (sortstatus === 'followrow') {
        for (var j = 0; j < allrows.length; j++) {
            if (allrows[j].className == 'followrow disabled' || allrows[j].className == 'followrow disabled frow') {
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

$(function () {
    var clickedUrl = '';
    $('a').click(function (e) {
        if ($('#validatePriority') && $('#validatePriority').val() == "true") {
            if (!$(this).hasClass("validationChk")) {
                e.preventDefault();
                showModal();
            }
        }
        if ($('#validateMyViewPriority') && $('#validateMyViewPriority').val() == "true") {
            if (!$(this).hasClass("validationChk")) {
                e.preventDefault();
                showModal();
                if(!$(this).hasClass('myviewLink')){
					clickedUrl = $(this).attr('href');
				}
				else{
					clickedUrl = $(this).attr('href') + '#' + $(this).attr('name');
				}
            }
        }
    });

    $('form').submit(function () {
        if ($('#validatePriority') && $('#validatePriority').val() == "true") {
            showModal();
            return false;
        }
        if ($('#validateMyViewPriority') && $('#validateMyViewPriority').val() == "true") {
            showModal();
            return false;
        }
    });

    $('#allPublicationsPan').on('click', '.followAllBtn', function () {
        var $this = $(this), curpublicPan = $this.closest('.publicationPan'), thead = curpublicPan.find('thead.hidden-xs'), firstRow = thead.find('tr:first-child'), tbody = curpublicPan.find('tbody'), div = $this.closest('div'), $lgfollow = curpublicPan.find('.followBtn'), table = $('.table'), lableStatus = '';
        $this.addClass('hideBtn');
        $('#validatePreference').val(1);
        div.find('.unfollowAllBtn').removeClass('hideBtn');
        curpublicPan.find('.firstrow .lableStatus').val('followinglbl');
        curpublicPan.find('.accordionStatus .lableStatus').val('followinglbl');
        $lgfollow.addClass('followingBtn').removeClass('followBtn button--filled').html($('#followingButtonText').val());
        $('#validatePriority').val(true);
        $('#validateMyViewPriority').val(true);
		
		lableStatus = thead.find('.lableStatus').val();
		thead.find('.mtp').addClass('hideBtn');
		thead.find('.mtp.'+ lableStatus).removeClass('hideBtn');
		thead.removeClass('followbg').addClass('followingbg');
		
        for (var i = 0; i < tbody.find('.followingBtn').length; i++) {
            $(tbody.find('.followrow')[i]).attr('draggable', true);
        }
		curpublicPan.addClass('active');
        curpublicPan.find('.unfollowAllBtn').removeClass('hideBtn');
        for (var i = 0; i < $lgfollow.length; i++) {
            $($lgfollow[i], curpublicPan).closest('tr').removeAttr('class').addClass('followingrow');
        }
        setClsforFlw(table);
        sort_table(tbody, 0, 1, 'followingrow');
    });


    $('#allPublicationsPan').on('click', '.unfollowAllBtn', function () {
        var $this = $(this), curpublicPan = $this.closest('.publicationPan'), thead = curpublicPan.find('thead.hidden-xs'), firstRow = thead.find('tr:first-child'), tbody = curpublicPan.find('tbody'), div = $this.closest('div'), $lgfollowing = curpublicPan.find('.followingBtn'), lableStatus = '';
        $this.addClass('hideBtn');
        $this.closest('.smfollowingBtn').find('.followAllBtn').addClass('fr');
        $('#validatePreference').val(1);
        div.find('.followAllBtn').removeClass('hideBtn');
        curpublicPan.find('.firstrow .lableStatus').val('followlbl');
        curpublicPan.find('.accordionStatus .lableStatus').val('followlbl');
        $lgfollowing.addClass('followBtn button--outline').removeClass('followingBtn button--filled').html($('#followButtonText').val());
        $('#validatePriority').val(false);
        $('#validateMyViewPriority').val(true);
		
		lableStatus = thead.find('.lableStatus').val();
		thead.find('.mtp').addClass('hideBtn');
		thead.find('.mtp.' + lableStatus).removeClass('hideBtn'); 
		thead.removeClass('followingbg').addClass('followbg');
		
		curpublicPan.removeClass('active');
        curpublicPan.find('tbody .frow').removeClass('frow');
        for (var i = 0; i < $lgfollowing.length; i++) {
            $($lgfollowing[i], curpublicPan).closest('tr').removeAttr('class').addClass('followrow disabled');
        }
        sort_table(tbody, 0, 1, 'followrow');
    });
	
	$('#allPublicationsPan .donesubscribe').on('click', '.followrow .followBtn', function (e) {
		var $this = $(this), currenttr = $this.closest('tr'), currentTopic = $.trim(currenttr.find('.wd-55').html().split('<input')[0]), currentChannel = currenttr.closest('.table').find('thead h2').html(), eventDetails;
		if($('.registrationBtn') && $('.registrationBtn').length){
			//eventDetails = { "event_name":"channel_follow","page_name":"Registration","ga_eventCategory":"Channel Follow","ga_eventAction": analytics_data["publication"], "ga_eventLabel": currentTopic, "follow_publication":analytics_data["publication"], "follow_channel": currentTopic };
		}
		else{
			//eventDetails = { "event_name":"topic_follow", "page_name":"My view settings", "ga_eventCategory":"Topic Follow","ga_eventAction": analytics_data["publication"] +':'+ currentChannel, "ga_eventLabel": currentTopic, "follow_publication": analytics_data["publication"], "follow_topic": currentTopic, "follow_channel": currentChannel };
		}
		//analyticsEvent( eventDetails );
		
		eventDetails = {};
	});
	
	$('#allPublicationsPan .donesubscribe').on('click', '.followingrow .followingBtn', function (e) {
		var $this = $(this), currenttr = $this.closest('tr'), currentTopic = $.trim(currenttr.find('.wd-55').html().split('<input')[0]), currentChannel = currenttr.closest('.table').find('thead h2').html(), eventDetails;
		if($('.registrationBtn') && $('.registrationBtn').length){
			//eventDetails = { "event_name": "channel_unfollow", "page_name": "Registration", "ga_eventCategory":"Channel Unfollow", "ga_eventAction": analytics_data["publication"], "ga_eventLabel": currentTopic, "follow_publication": analytics_data["publication"], "follow_channel": currentTopic };
		}
		else{
			//eventDetails = {"event_name": "topic_unfollow", "page_name": "My view settings", "ga_eventCategory": "Topic Unfollow","ga_eventAction": analytics_data["publication"] +':'+ currentChannel, "ga_eventLabel": currentTopic,"follow_publication": analytics_data["publication"], "follow_topic": currentTopic, "follow_channel":currentChannel };
		}
		//analyticsEvent( eventDetails );
		
		eventDetails = {};
	});
	
    $('#allPublicationsPan .donesubscribe').on('click', '.followrow .followBtn', function (e) {
		var $this = $(this), followrow = $this.closest('.followrow'), table = $this.closest('.table'), curpublicPan = $this.closest('.publicationPan'), thead = table.find('thead.hidden-xs'), followAllBtn = table.find('.followAllBtn'), unfollowAllBtn = table.find('.unfollowAllBtn'), trs = $this.closest('tbody').find('tr'), trsfollowing = $this.closest('tbody').find('tr.followingrow'), lableStatus = '';
		followrow.attr('draggable', true);
		$('#validatePreference').val(1);
		followrow.addClass('followingrow').removeClass('followrow disabled frow');
		$this.addClass('followingBtn').removeClass('followBtn button--filled').html($('#followingButtonText').val());
		setClsforFlw(table);
		table.find('.firstrow .lableStatus').val('followinglbl');
		table.find('.accordionStatus .lableStatus').val('followinglbl');
		$('#validateMyViewPriority').val(true);
		
		lableStatus = thead.find('.lableStatus').val();
		thead.find('.mtp').addClass('hideBtn');
		thead.find('.mtp.' + lableStatus).removeClass('hideBtn');
		thead.removeClass('followbg').addClass('followingbg');
		curpublicPan.addClass('active');
		
		if (trs.hasClass('followingrow')) {
			$('#validatePriority').val(true);
			//unfollowAllBtn.addClass('hideBtn');
		}

		if ($('.followrow.disabled.frow', table).length) {
			followrow.appendTo(followrow.clone().insertBefore(table.find('.followrow.disabled.frow')));
		}
		else {
			followrow.clone().appendTo($this.closest('tbody'));
		}
		followrow.remove();
		if (trs.length === trsfollowing.length + 1) {
			followAllBtn.addClass('hideBtn');
			unfollowAllBtn.removeClass('hideBtn');
		}
		else {
			followAllBtn.removeClass('hideBtn');
			unfollowAllBtn.removeClass('hideBtn');
		}
    });
	
	$('#allPublicationsPan .donesubscribe').on('mouseenter', '.followBtn', function (e) { 
		$(this).html($('#followText').val());
	}).on('mouseleave', '.followBtn', function() {
		$(this).html($('#followButtonText').val());
	}); 

    $('#allPublicationsPan .donesubscribe').on('click', '.followingrow .followingBtn', function (e) {
        var $this = $(this), table = $this.closest('table'), curpublicPan = $this.closest('.publicationPan'), followAllBtn = $this.closest('table').find('.followAllBtn'), thead = table.find('thead.hidden-xs'), unfollowAllBtn = $this.closest('table').find('.unfollowAllBtn'), followingrow = $this.closest('.followingrow'), tbody = $this.closest('tbody'), trs = $this.closest('tbody').find('tr'), disabledtrs = $this.closest('tbody').find('.followrow.disabled'), trsfollow = $this.closest('tbody').find('tr.followrow'), lableStatus = '';
        followingrow.addClass('followrow disabled').removeClass('followingrow');
        $this.addClass('followBtn').removeClass('followingBtn').html($('#followButtonText').val());
        followingrow.clone().appendTo($this.closest('tbody'));
        followingrow.remove();
        $('#validatePreference').val(1);
        sort_table(tbody, 0, 1, 'followingBtn');
        $('#validateMyViewPriority').val(true); 
		
        if (trs.length === disabledtrs.length + 1) {
            table.find('.firstrow .lableStatus').val('followlbl');
            table.find('.accordionStatus .lableStatus').val('followlbl');
			curpublicPan.removeClass('active');
			thead.removeClass('followingbg').addClass('followbg');
        }
        if (trs.length === trsfollow.length + 1) {
            unfollowAllBtn.addClass('hideBtn');
            followAllBtn.removeClass('hideBtn');

            $('#validatePriority').val(false);
        }
        else {
            followAllBtn.removeClass('hideBtn');
            unfollowAllBtn.removeClass('hideBtn');
        }
		lableStatus = thead.find('.lableStatus').val();
		thead.find('.mtp').addClass('hideBtn');
		thead.find('.mtp.' + lableStatus).removeClass('hideBtn');
    });
	
	$('#allPublicationsPan .donesubscribe').on('mouseenter', '.followingBtn', function (e) {
		$(this).html($('#unfollowText').val());
	}).on('mouseleave', '.followingBtn', function() {
		$(this).html($('#followingButtonText').val());
	});
	
    $('.publicationPan').on('click', '.accordionImg .mobileMode', function () {
        var $this = $(this), allPublications = $('#allPublicationsPan'), pPan = $this.closest('.publicationPan'), thead = pPan.find('thead'), tbody = pPan.find('tbody'), trs = tbody.find('tr'), disabledtrs = tbody.find('tr.disabled'), followlbl = thead.find('.followlbl'), followinglbl = thead.find('.followinglbl'), accStatusflwLbl = thead.find('.accordionStatus.flwLbl'), accStatusflwBtn = thead.find('.accordionStatus.flwBtn'), allpubpans = allPublications.find('.publicationPan'), pickTxt = thead.find('.pickTxt'), setFlag = true;

        if ($this.hasClass('expanded')) {
            setFlag = false;
            tbody.addClass('tbodyhidden');
            //pPan.find('.smfollowingBtn').hide();  
            accStatusflwLbl.removeClass('hideRow');
            accStatusflwBtn.addClass('hideRow');
            thead.find('.mtp').addClass('hideBtn');

            for (var i = 0; i < pickTxt.length; i++) {
                $(pickTxt[i]).closest('.accordionStatus').addClass('hideRow');
            }
            if (trs.length === disabledtrs.length) {
                followlbl.removeClass('hideBtn');
            }
            else {
                followinglbl.removeClass('hideBtn');
            }
            var position = $this.closest('.publicationPan').position();
            $(window).scrollTop(position.top - 40);
        }
        else {
            allPublications.find('tbody').addClass('tbodyhidden');
            for (var i = 0; i < allpubpans.length; i++) {
                var eachPickTxt = $(allpubpans[i]).find('thead .pickTxt');
                for (var j = 0; j < eachPickTxt.length; j++) {
                    $(eachPickTxt[j]).closest('.accordionStatus').addClass('hideRow');;
                }
            }
            thead.find('tr').removeClass('hidden');
            tbody.removeClass('tbodyhidden');
            pPan.find('.smfollowingBtn').show();
            for (var i = 0; i < pickTxt.length; i++) {
                $(pickTxt[i]).closest('.accordionStatus').removeClass('hideRow');
            }
            if (setFlag) {
                for (var i = 0; i < allpubpans.length; i++) {
                    $(allpubpans[i]).find('.accordionStatus.flwLbl').removeClass('hideRow');
                    $(allpubpans[i]).find('.accordionStatus.flwBtn').addClass('hideRow');
                }
            }
            accStatusflwLbl.addClass('hideRow');
            accStatusflwBtn.removeClass('hideRow');

            var position = $this.closest('.publicationPan').position();
            $(window).scrollTop(position.top - 40);

            for (var i = 0; i < allpubpans.length; i++) {
                var labelVal = $(allpubpans[i]).find('.firstrow .lableStatus').val();
                $('.' + labelVal, allpubpans[i]).removeClass('hideBtn');
            }
            thead.find('.mtp').addClass('hideBtn');
        }
    });

    $('#allPublicationsPan .publicationPan').on('click', 'thead.hidden-xs tr:first-child', function (e) {
        var $this = $(this), allPublications = $('#allPublicationsPan'), pPan = $this.closest('.publicationPan'), accCont = pPan.find('.accCont'), thead = pPan.find('thead'), tbody = pPan.find('tbody'), trs = tbody.find('tr'), disabledtrs = tbody.find('tr.disabled'), flwlbl = thead.find('.flwLbl'), flwBtn = thead.find('.flwBtn'), followlbl = thead.find('.followlbl'), followinglbl = thead.find('.followinglbl'), allpubpans = allPublications.find('.publicationPan'), allthead = $this.closest('#allPublicationsPan').find('.publicationPan thead.hidden-xs'), lableStatus = allthead.find('.lableStatus').val();
		
		allPublications.find('.publicationPan thead.hidden-xs tr:first-child').not($(this)).removeClass('expanded').addClass('collapsed');
		
		if(e.target.className !== 'subscribed'){
			if ($this.hasClass('expanded')) {
				$this.removeClass('expanded').addClass('collapsed');
				tbody.addClass('tbodyhidden');
				thead.find('.mtp').addClass('hideBtn');
				accCont.addClass('tbodyhidden');
				if (trs.length === disabledtrs.length) {
					followlbl.removeClass('hideBtn');
					thead.find('.firstrow .lableStatus').val('followlbl');
				}
				else {
					followinglbl.removeClass('hideBtn');
					thead.find('.firstrow .lableStatus').val('followinglbl');
				}
				for(var i = 0; i < allthead.length; i++){
					var curthead = $(allthead[i]), getlableStatus = curthead.find('.lableStatus').val();
					curthead.removeClass('followingbg followbg').addClass(getlableStatus == 'followinglbl' ? 'followingbg' : 'followbg');
				}
				allPublications.find('.sorting_arrow--up').removeClass('act').addClass('hide');
				allPublications.find('.sorting_arrow--down').removeClass('hide');
				pPan.removeClass('active');
				var position = $this.closest('.publicationPan').position();
				$(window).scrollTop(position.top); 
			}
			else {
				allPublications.find('tbody').addClass('tbodyhidden');
				allPublications.find('.publicationPan .accordionImg span.accImg .sorting_arrow--up').removeClass('act').addClass('hide');
				allPublications.find('.publicationPan .accordionImg span.accImg .sorting_arrow--down').removeClass('hide');
				allPublications.find('.publicationPan thead tr').not(':nth-child(1)').addClass('hidden');
				allPublications.find('.publicationPan thead tr.showinview').removeClass('hidden');
				thead.find('tr').removeClass('hidden');
				$this.addClass('expanded').removeClass('collapsed');
				accCont.removeClass('tbodyhidden');
				tbody.removeClass('tbodyhidden');
				flwBtn.addClass('hideRow');
				flwlbl.removeClass('hideRow');
				
				pPan.find('.sorting_arrow--up').addClass('act').removeClass('hide');
				pPan.find('.sorting_arrow--down').addClass('hide');
				pPan.find('.expandTxt').removeAttr('style');
				pPan.find('.mvTxt').removeAttr('style');
				
				allPublications.find('.publicationPan').removeClass('active');
				if(trs.length == disabledtrs.length){
					pPan.removeClass('active');
				}
				else{
					pPan.addClass('active');
				}
				for (var i = 0; i < allpubpans.length; i++) {
					var labelVal = $(allpubpans[i]).find('.firstrow .lableStatus').val(),
					getLabelStatus = (labelVal == 'followingbg') ? 'followingbg' : 'followbg';
					$(allpubpans[i]).find('thead.hidden-xs').removeClass('followingbg followbg').addClass(getLabelStatus);
				}
				//thead.find('.mtp').addClass('hideBtn');

				var position = $this.closest('.publicationPan').position();
				$(window).scrollTop(position.top);
			}
		}
    }).on('mouseenter', 'thead.hidden-xs tr:first-child', function () {
		var $this = $(this), firstTrtds = $this.find('th'), thead = $this.closest('thead.hidden-xs'), lableStatus = $this.find('.lableStatus').val();
		firstTrtds.addClass('active');
		if($this.hasClass('collapsed') && lableStatus == 'followinglbl'){
			thead.removeClass('followinglbl-txt followlbl-txt').addClass(lableStatus + '-txt');
			thead.find('.mvTxt').css('visibility', 'visible');
			thead.find('.expandTxt').css('visibility', 'visible');
			thead.find('.accImg .sorting_arrow--down').addClass('act'); 
		}
		else if($this.hasClass('collapsed') && lableStatus == 'followlbl'){
			//thead.removeClass('followinglbl-txt followlbl-txt').addClass(lableStatus + '-txt');
			thead.find('.expandTxt').css('visibility', 'visible');
		}
	}).on('mouseleave', 'thead.hidden-xs tr:first-child', function() {
		var $this = $(this), firstTrtds = $this.find('th'), thead = $this.closest('thead.hidden-xs');
		firstTrtds.removeClass('active');
		if($this.hasClass('collapsed')){
			thead.removeClass('followinglbl-txt followlbl-txt');
			thead.find('.mvTxt').removeAttr('style');
			thead.find('.expandTxt').removeAttr('style');
			thead.find('.accImg .sorting_arrow--down').removeClass('act'); 
		}
	}); 

    var tables = $('.publicationPan table');
    setClsforFlw(tables);

    $('.saveview').click(function (e) {
        /*if ($('.modal-overlay').hasClass('in')) {
            window.location.href = clickedUrl;
        }*/
        //else {
		 var alltables = $('.table'), allpublicationsEles = $('.publicationPan'),
			isChannelLevel = $('#isChannelBasedRegistration').val(),
		UserPreferences = { "IsNewUser": false, "IsChannelLevel": isChannelLevel }, allpublications = $('.publicationPan', '#allPublicationsPan');
		UserPreferences.PreferredChannels = [];
		$('#validateMyViewPriority').val(false);
		e.preventDefault();
		setDataRow(allpublications);
		allpublicationsEles.removeAttr('data-row');
		for (var i = 0; i < allpublicationsEles.length; i++) {
			var j = i + 1;
			$(allpublicationsEles[i]).attr('data-row', j);
		}
		
		if($(this).hasClass('validationChk')){
			createJSONData(alltables, UserPreferences, clickedUrl);
		}
		else{
			createJSONData(alltables, UserPreferences);
           $('#validatePreference').val(0);
		}
        //}
    });

    $('.registrationBtn').click(function (e) {
        var table = $('.table', '.publicationPan'), alltrs = table.find('tbody tr'),
            isChannelLevel = $('#isChannelBasedRegistration').val(),
		    UserPreferences = { "IsNewUser": true, "IsChannelLevel": isChannelLevel }, allpublications = $('.publicationPan', '#allPublicationsPan');
        UserPreferences.PreferredChannels = [];

        e.preventDefault();
        if ($('#validatePriority').val() == "true" && $('#enableSavePreferencesCheck').val() === "false") {
            setDataRow(allpublications);
            sendRegisterData(alltrs, UserPreferences, 'name');
            return false;
        }
        if ($('#validatePriority').val() == "false") {
            if ($('#enableSavePreferencesCheck').val() === "true" && table.find('.followingrow').length == 0) {
                $('.alert-error.register-not-selected').show();
                return false;
            }
            setDataRow(allpublications);

            if ($('#isChannelBasedRegistration').val() == "true") {
                sendRegisterData(alltrs, UserPreferences, 'href');
            }
            else {
                createJSONData(table, UserPreferences);
            }
        }
        else {
            setDataRow(allpublications);
            sendRegisterData(alltrs, UserPreferences, 'href');
        }
    });

    $('.gotoview').click(function (e) {
        if ($('#validatePriority') && $('#validatePriority').val() == "true") {
            showModal();
        }
        else {
            if (+$('#validatePreference').val()) {
                e.preventDefault();
                showModal();
            }
        }
    });

    $('.close-modal').click(function () {
        $('.modal-overlay').removeClass('in');
        $('.modal-view').hide();
    });
	
	$('.cancel-modal').click(function () {
        window.location.href = clickedUrl;
    });

    if ($('.publicationPan') && $('.publicationPan').length) {
        $('.publicationPan.donesubscribe').dragswap({
            element: '.table tbody tr',
            dropAnimation: true
        });

        $('#allPublicationsPan').dragswap({
            element: '.publicationPan.donesubscribe',
            dropAnimation: true
        });
    }
	
	$(document).on('click', '.editView', function(){
		var eventDetails = {"event_name":"myview_edit_my_view","page_name": analytics_data["page_name"], "ga_eventCategory":"My View Page Link", "ga_eventAction":"Link Click", "ga_eventLabel":"EDIT MY VIEW"};
		//analyticsEvent( $.extend(analytics_data, eventDetails) );
		eventDetails = {};
	});
	
	$('.personalisationPan').on('click', '.loadmore', function(){
		var id = $(this).closest('.eachstoryMpan').find('.eachstory').attr('id'), getIdx = 0;
		for(var i=0; i<loadPreferanceId["Sections"].length; i++){
			if(loadPreferanceId["Sections"][i]["ChannelId"] == id){
				getIdx = i;
				break;
			}
		}
		var eventDetails = { "event_name": "myview_load_more", "page_name": analytics_data["page_name"], "ga_eventCategory": "My View Page Publications", "ga_eventAction": analytics_data["publication"], "ga_eventLabel": loadPreferanceId["Sections"][getIdx]["ChannelName"], "publication_click": analytics_data["publication"] };
		
		//analyticsEvent( $.extend(analytics_data, eventDetails) );
		eventDetails = {};
	});
});
