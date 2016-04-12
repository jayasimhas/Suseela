using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class MediaTypeField : BaseGlassComputedField<IArticle>
	{
		public override object GetFieldValue(IArticle indexItem)
		{
			if (string.IsNullOrEmpty(indexItem?.Media_Type?.Item_Name))
			{
				return string.Empty;
			}

			return indexItem.Media_Type.Item_Name.Trim();
		}
	}
}
