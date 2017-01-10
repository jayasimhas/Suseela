(function (argument) {
	var LatestCasuality = {
		RenderLinks: function(data, Parent) {
			var latestcasualties = data[0].latestcasualties,
				Html = "";

			for(var key in latestcasualties) {
				Html += '<li class="article-topics__li"><a href="'+ latestcasualties[key].url +'"><strong>' +latestcasualties[key].title+ '</strong> - '+ latestcasualties[key].date +' </a></li>';
			}
			Parent.find('ul').append(Html);
			//<li class="article-topics__li"><a href="#"><strong>MCC Shanghai</strong> - 01.01.2017</a></li>
		},
		init: function(data, parent) {
			this.RenderLinks(data, parent);
		}
	}

	if($('.lloyd-related-links').length > 0) {
		LatestCasuality.init(window.jsonLatestCasualties, $('.lloyd-related-links'));
	}
})();