using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchResultSummaryField : BaseGlassComputedField<IArticle>
	{
		public override object GetFieldValue(IArticle indexItem)
		{
			return SearchSummaryUtil.GetTruncatedSearchSummary(indexItem.Summary);
		}
	}
}