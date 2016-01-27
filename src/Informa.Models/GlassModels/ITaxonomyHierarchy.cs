using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;

namespace Informa.Models.GlassModels
{
    public interface ITaxonomyHierarchy
    {
        string Category { get; }
        IEnumerable<ITaxonomy_Item> LongIds { get; }  
    }
}
