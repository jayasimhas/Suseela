using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Informa.Library.Search.Results;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Article.Search
{
	public class ArticleSearchResultItem : SearchResultItem, ITaxonomySearchResults, IArticleNumber, IArticleEScenicID, IReferencedArticles
	{
		public List<Guid> Taxonomies { get; set; }
		[IndexField(IArticleConstants.Actual_Publish_DateFieldName)]
		public DateTime ActualPublishDate { get; set; }

	    public string ArticleNumber { get; set; }
	    public long ArticleIntegerNumber { get; set; }
	    public string EScenicID { get; set; }

	    #region Implementation of IReferencedArticles

	    public List<Guid> ReferencedArticles { get; set; }

	    #endregion
	}
}


