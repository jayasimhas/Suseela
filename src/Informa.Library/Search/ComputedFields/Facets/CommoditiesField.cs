﻿using System.Collections.Generic;
using System.Linq;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class CommoditiesField : BaseGlassComputedField<I___BaseTaxonomy>
    {
        public override object GetFieldValue(I___BaseTaxonomy indexItem)
        {
            if (indexItem?.Taxonomies != null)
            {
                var commoditiesTaxonomyItems = indexItem.Taxonomies.Where(x => SearchTaxonomyUtil.IsCommodityTaxonomy(x._Path));

                return SearchTaxonomyUtil.GetHierarchicalFacetFieldValue(commoditiesTaxonomyItems);
            }
            return new List<string>();
        }
    }
}