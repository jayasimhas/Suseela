using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
	public interface IArticleCompanyFilter
	{
		IList<string> CompanyRecordNumbers { get; set; } 
	}
}