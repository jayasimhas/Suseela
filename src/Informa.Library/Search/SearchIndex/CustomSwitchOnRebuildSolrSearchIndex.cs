using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.SolrProvider;

namespace Informa.Library.Search.SearchIndex
{
    public class CustomSwitchOnRebuildSolrSearchIndex : Sitecore.Support.ContentSearch.SolrProvider.SwitchOnRebuildSolrSearchIndex
	{
        public CustomSwitchOnRebuildSolrSearchIndex(string name, string core, string rebuildcore, IIndexPropertyStore propertyStore)
            : base(name, core, rebuildcore, propertyStore)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            FieldNameTranslator = new CustomSolrFieldNameTranslator(this);
        }
    }
}
