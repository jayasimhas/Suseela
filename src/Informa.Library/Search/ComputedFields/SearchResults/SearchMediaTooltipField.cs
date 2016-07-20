using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchMediaTooltipField : BaseGlassComputedField<IArticle>
	{
		public override object GetFieldValue(IArticle indexItem)
		{
			//TODO refactor this to use the shared functionality in ArticleListItemModelFactory
			return indexItem.Media_Type?.Tooltip ?? string.Empty;
		}
	}
}