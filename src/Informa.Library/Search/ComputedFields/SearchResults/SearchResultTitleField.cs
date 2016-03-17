using System.Web;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
	public class SearchResultTitleField : BaseContentComputedField
	{
		public override object GetFieldValue(Item indexItem)
		{
			var glassItem = indexItem.GlassCast<IGlassBase>(inferType: true);
			var page = glassItem as I___BasePage;
			var title = indexItem.Name;

			if (page == null) return HttpUtility.HtmlDecode(title);
			if (!string.IsNullOrEmpty(page.Title))
			{
				title = page.Title;
			}

			if (!string.IsNullOrEmpty(page.Navigation_Title))
			{
				title = page.Navigation_Title;
			}

			if (!string.IsNullOrEmpty(indexItem.DisplayName))
			{
				title = indexItem.DisplayName;
			}

			return HttpUtility.HtmlDecode(title);
		}
	}
}
