using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Configuration;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class IndustriesField : BaseGlassComputedField<I___BaseTaxonomy>
	{
		public override object GetFieldValue(I___BaseTaxonomy glassItem)
		{
			if (glassItem?.Taxonomies != null)
			{
                Item rootItem = null;
                var dbContext = Factory.GetDatabase("master");
                Item item = dbContext.GetItem(new ID(glassItem._Id));

                if(item != null)
                    rootItem = item.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == Settings.GetSetting("VerticalTemplate.global"));

                var taxonomyItems = glassItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsIndustryTaxonomy(x._Path, (rootItem != null) ? rootItem.Name : string.Empty));

				return taxonomyItems.Where(x => !string.IsNullOrEmpty(x.Item_Name)).Select(x => x.Item_Name.Trim()).ToList();
			}
			return new List<string>();
		}
	}
}