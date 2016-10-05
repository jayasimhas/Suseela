using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Sitecore.Data.Items;
using Sitecore.Data;

namespace Informa.Library.Search.ComputedFields.Facets
{
	public class AreasField : BaseGlassComputedField<I___BaseTaxonomy>
	{
		public override object GetFieldValue(I___BaseTaxonomy indexItem)
		{
			if (indexItem?.Taxonomies != null)
			{
                var areaTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsAreaTaxonomy(x._Path));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(areaTaxonomyItems);
            }
			return new List<string>();
		}
	}
}
