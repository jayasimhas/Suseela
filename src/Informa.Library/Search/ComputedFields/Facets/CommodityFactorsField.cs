using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class CommodityFactorsField : BaseGlassComputedField<I___BaseTaxonomy>
    {
        public override object GetFieldValue(I___BaseTaxonomy indexItem)
        {
            if (indexItem?.Taxonomies != null)
            {
                var commodityFactorsTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsCommodityFactorsTaxonomy(x._Path));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(commodityFactorsTaxonomyItems);
            }
            return new List<string>();
        }
    }
}
