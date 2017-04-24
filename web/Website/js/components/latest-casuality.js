(function (argument) {
	var LatestCasuality = {
		RenderLinks: function(data, Parent) {
			var latestcasualties = data[0].latestcasualties,
				Html = "";

			for(var key in latestcasualties) {
				Html += '<li class="article-topics__li"><a href="'+ $('#CasualtyDetailPageUrl').val() + '?incidentId=' + latestcasualties[key].IncidentId +'"><strong>' +latestcasualties[key].title+ '</strong> - '+ latestcasualties[key].date +' </a></li>';
			}
			Parent.find('ul').append(Html);
			//<li class="article-topics__li"><a href="#"><strong>MCC Shanghai</strong> - 01.01.2017</a></li>
		},
		init: function(data, parent) {
			this.RenderLinks(data, parent);
		}
	}

	if($('.lloyd-related-links').length > 0) {
		if(typeof window.jsonLatestCasualties !== 'undefined' && typeof window.jsonLatestCasualties !== 'string'){
			LatestCasuality.init(window.jsonLatestCasualties, $('.lloyd-related-links'));
		}
		else{
			$('.lloyd-related-links').append('<div class="alert-error" style="display: block;"><svg class="alert__icon"><use xmlns:xlink=http://www.w3.org/1999/xlink" xlink:href="/dist/img/svg-sprite.svg#alert"></use></svg><p class="page-account-contact__error">'+$('#hdnErrormessage').val()+'</p></div>');
		}
	}
})();