using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.SolrProvider;

namespace Informa.Library.Search.SearchIndex
{
    public class CustomSolrSearchIndex : Sitecore.Support.ContentSearch.SolrProvider.SolrSearchIndex
	{
        public CustomSolrSearchIndex(string name, string core, IIndexPropertyStore propertyStore, string @group) : base(name, core, propertyStore, @group)
        {
        }

        public CustomSolrSearchIndex(string name, string core, IIndexPropertyStore propertyStore)
            : base(name, core, propertyStore)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            FieldNameTranslator = new CustomSolrFieldNameTranslator(this);
        }
    }
}
