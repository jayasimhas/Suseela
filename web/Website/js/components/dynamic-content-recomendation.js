(function() {
	var RecomendedContent = {
		AjaxData: function(url, type, data, SuccessCallback) {
			$.ajax({ 
				url: url,
				data: JSON.stringify(data),
				dataType: 'json',
				contentType: "application/json",
				type: type,
				cache: false,
				success: function (data) { 
					//if (data.articles && typeof data.articles === "object" && data.articles.length >= 3) {
					 	SuccessCallback(data);
					//} 
				}
			});
		},
		RecomendedTemplate: function(data) {
			var Template = '';
				if(data.articles.length > 0) {
					for(var i = 0; i < 2; i++) {
						Template += '<div class="section-article">'+
										'<img class="article-related-content__img" src="' +data.articles[i].listableImage+ '">'+
										'<span class="article-related-content__category"> ' +data.articles[i].linkableText+ ' </span>'+
										'<h5><a class="click-utag" href="' + data.articles[i].linkableUrl + '">' +data.articles[i].linkableText+ '</a></h5>'+
										'<time class="article-related-content__date">' + data.articles[i].listableDate +'</time>'+
									'</div>';
					}
				}

				Template += '</div>';

			$('.ContentRecomm-ReadNext').append(Template);
								
		},
		SuggestedTemplate: function(data) {
			var Template = '';
				if(data.articles.length > 0) {
					for(var i = 0; i < 2; i++) {
						Template += '<div class="contentRecomm-article">'+
										'<img class="article-related-content__img" src="https://beta.agra-net.com/-/media/legacy-images/2016/july/orange_juice_167615144.jpg">'+
										'<span class="article-related-content__category">scrip Intelligence</span>'+
										'<h5>'+
											'<a class="click-utag" href="/articles/2016/07/04/probe-into-alleged-orange-juice-cartel-will-continue">'+
											'</a>'+
										'</h5>'+
										'<time class="article-related-content__date">22 Aug 2016</time>'+
									'</div>';
					}
				}

				Template += '</div>';

			$('.suggested-article').append(Template);
		},
		init: function() {
			var self = this;
			if($('.ContentRecomm-ReadNext').length > 0) {
				self.AjaxData('/api/articlesearch', 'POST', { 'TaxonomyIds': $('#hdnTaxonomyIds').val() , 'PageNo': 1, 'PageSize': 4 } , self.RecomendedTemplate);
			}
			if($('#hdnPreferanceIds').val()) {
				self.AjaxData('/api/articlesearch', 'POST', { 'TaxonomyIds': $('#hdnPreferanceIds').val() , 'PageNo': 1, 'PageSize': 4 } , self.SuggestedTemplate);
			}
		}
	}

	RecomendedContent.init();
})();
