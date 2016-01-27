using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Informa.Models.FactoryInterface;
using Informa.Models.GlassModels;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates
{
    partial interface I___BaseTaxonomy : IListable, ITaxonomyHierarchy
    {
        [SitecoreField(I___BaseTaxonomyConstants.TaxonomiesFieldName)]
        IEnumerable<ITaxonomyHierarchy> TaxonomyLongIds { get; set; }
    }
}