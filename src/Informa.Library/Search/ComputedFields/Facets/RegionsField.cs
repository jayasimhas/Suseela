using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class RegionsField : BaseGlassComputedField<I___BaseTaxonomy>
	{
		public override object GetFieldValue(I___BaseTaxonomy indexItem)
		{
			if (indexItem?.Taxonomies != null)
			{
				var regionTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsRegionTaxonomy(x._Path));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(regionTaxonomyItems);
            }
			return new List<string>();
		}
	}
}
