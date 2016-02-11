using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchResultPublicationTitleField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            return "Scrip Intelligence";
        }
    }
}
