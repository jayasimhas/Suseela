using Sitecore.ContentSearch;

namespace Informa.Library.Search.Results
{
	public interface IArticlePublicationResults
	{
		[IndexField("searchpublicationtitle_s")]
		string PublicationTitle { get; set; }
	}
}