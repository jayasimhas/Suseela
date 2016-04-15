using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class ContentTypeField : BaseGlassComputedField<IArticle>
	{
		public override object GetFieldValue(IArticle indexItem)
		{
			if (string.IsNullOrEmpty(indexItem?.Content_Type?.Item_Name))
			{
				return string.Empty;
			}

			return indexItem.Content_Type.Item_Name.Trim();

		}
	}
}
