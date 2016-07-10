using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Taxonomy;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Services.Global {
    public interface ITaxonomyService
    {
        IEnumerable<HierarchyLinks> GetHeirarchyChildLinks(I___BaseTaxonomy taxItem);
    }
}
