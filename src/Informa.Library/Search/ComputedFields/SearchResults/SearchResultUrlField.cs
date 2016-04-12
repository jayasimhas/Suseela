using Sitecore.Data.Items;
using Sitecore.Links;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchResultUrlField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            return LinkManager.GetItemUrl(indexItem);
        }
    }
}
