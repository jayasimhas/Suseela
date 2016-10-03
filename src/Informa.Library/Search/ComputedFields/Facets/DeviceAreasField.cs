using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Sitecore.Data.Items;
using Sitecore.Data;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class DeviceAreasField : BaseGlassComputedField<I___BaseTaxonomy>
	{
		public override object GetFieldValue(I___BaseTaxonomy glassItem)
		{
			if (glassItem?.Taxonomies != null)
			{
                Item item = Sitecore.Context.ContentDatabase.GetItem(new ID(glassItem._Id));

                var rootItem = item.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == "{DE3615F6-1562-4CB4-80EA-7FA45F49B7B7}");
                var taxonomyItems = glassItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsDeviceAreaTaxonomy(x._Path, rootItem.Name));

				return taxonomyItems.Where(x => !string.IsNullOrEmpty(x.Item_Name)).Select(x => x.Item_Name.Trim()).ToList();
			}
			return new List<string>();
		}
	}
}