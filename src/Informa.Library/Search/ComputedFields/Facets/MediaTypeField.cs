using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class MediaTypeField : BaseContentComputedField
	{
		public override object GetFieldValue(Item indexItem)
		{
			if (indexItem.TemplateID != IArticleConstants.TemplateId)
			{
				return string.Empty;
			}

			IArticle article = indexItem.GlassCast<IArticle>(inferType: true);

			if (string.IsNullOrEmpty(article?.Media_Type?.Item_Name))
			{
				return string.Empty;
			}

			return article.Media_Type.Item_Name.Trim();
		}
	}
}
