using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Search.Utilities;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class RegionsField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            I___BaseTaxonomy taxonomyItem = indexItem.GlassCast<I___BaseTaxonomy>(inferType: true);

            if (taxonomyItem?.Taxonomies != null)
            {
                var regionTaxonomyItems =
                    taxonomyItem.Taxonomies.Where(
                        x => SearchTaxonomyUtil.IsRegionTaxonomy(x._Path));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(regionTaxonomyItems);
            }
            return new List<string>();
        }
    }
}
