using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using System.Threading.Tasks;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class CropProtectionField : BaseGlassComputedField<I___BaseTaxonomy>
    {
        public override object GetFieldValue(I___BaseTaxonomy indexItem)
        {
            if (indexItem?.Taxonomies != null)
            {
                var cropProtectionTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsCropProtectionTaxonomy(x._Path));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(cropProtectionTaxonomyItems);
            }
            return new List<string>();
        }
    }
}
