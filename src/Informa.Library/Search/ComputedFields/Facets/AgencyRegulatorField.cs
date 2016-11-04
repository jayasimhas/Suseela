using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class AgencyRegulatorField : BaseGlassComputedField<I___BaseTaxonomy>
    {
        public override object GetFieldValue(I___BaseTaxonomy indexItem)
        {
            if (indexItem?.Taxonomies != null)
            {
                var agencyRegulatorTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsAgencyRegulatorTaxonomy(x._Path));

                return agencyRegulatorTaxonomyItems.Where(x => !string.IsNullOrEmpty(x.Item_Name)).Select(x => x.Item_Name.Trim()).ToList();
            }
            return new List<string>();
        }
    }
}
