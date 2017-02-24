using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class AnimalHealthField: BaseGlassComputedField<I___BaseTaxonomy>
    {
        public override object GetFieldValue(I___BaseTaxonomy indexItem)
        {
            if (indexItem?.Taxonomies != null)
            {
                var AnimalHealthTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsAnimalHealthTaxonomy(x._Path));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(AnimalHealthTaxonomyItems);
            }
            return new List<string>();
        }
    }
}
