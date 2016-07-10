using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.Base
{
	public class IsSearchableField : BaseContentComputedField
	{
		public override object GetFieldValue(Item indexItem)
		{
			if (!indexItem.Paths.Path.StartsWith("/sitecore/content")) return false;

			if (indexItem.TemplateID != IArticleConstants.TemplateId && indexItem.TemplateID != IGeneral_Content_PageConstants.TemplateId)
			{
				return false;
			}

			I___BasePage article = indexItem.GlassCast<I___BasePage>();

			return article.Include_In_Search;
		}
	}
}
