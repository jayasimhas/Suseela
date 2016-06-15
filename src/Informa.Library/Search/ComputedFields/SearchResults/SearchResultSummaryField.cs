using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchResultSummaryField : BaseGlassComputedField<I___BasePage>
	{
		public override object GetFieldValue(I___BasePage indexItem)
		{
			
			var generalContentPage = indexItem as IGeneral_Content_Page;
			if (generalContentPage != null)
			{
				return SearchSummaryUtil.GetTruncatedSearchSummary(generalContentPage.Summary);
			}

			IArticle articleItem = indexItem as IArticle;
			if (articleItem != null)
			{
				return SearchSummaryUtil.GetTruncatedSearchSummary(articleItem.Summary);
			}

			return SearchSummaryUtil.GetTruncatedSearchSummary(indexItem.Body);
		}
	}
}