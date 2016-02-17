using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch.SolrProvider;

namespace Informa.Library.Search.SearchIndex
{
    public class CustomSolrFieldNameTranslator : SolrFieldNameTranslator
    {
        private readonly SolrFieldMap fieldMap;

        public CustomSolrFieldNameTranslator(SolrSearchIndex solrSearchIndex)
            : base(solrSearchIndex)
        {
            this.fieldMap = solrSearchIndex.Configuration.FieldMap as SolrFieldMap;
        }

        public override string GetIndexFieldName(string fieldName)
        {
            string suffix = string.Empty;

            foreach (SolrSearchFieldConfiguration fieldConfiguration in this.fieldMap.AvailableTypes)
            {
                string str = fieldConfiguration.FieldNameFormat.Replace("{0}", string.Empty);
                if (fieldName.EndsWith(str, StringComparison.Ordinal))
                {
                    suffix = str;
                    break;
                }
            }

            string processedFieldName = base.GetIndexFieldName(fieldName);

            return string.Format("{0}{1}", processedFieldName, suffix);
        }
    }
}
