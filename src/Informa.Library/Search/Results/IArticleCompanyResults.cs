using System.Collections.Generic;
using Sitecore.ContentSearch;

namespace Informa.Library.Search.Results
{
	public interface IArticleCompanyResults
    {
		[IndexField("referenced_companies_t")]
		string CompanyRecordIDs { get; set; }
	}
}