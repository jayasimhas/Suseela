using System;
using System.Collections.Generic;
using Informa.Library.Search.Results;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Article.Search
{
	public class ArticleSearchResultItem : SearchResultItem, ITaxonomySearchResults
	{
		public List<Guid> Taxonomies { get; set; }
		[IndexField(IArticleConstants.Actual_Publish_DateFieldName)]
		public DateTime ActualPublishDate { get; set; }
	}
}
