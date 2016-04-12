using System.Collections;
using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Informa.Library.Rss.Interfaces
{
    public interface IRssSitecoreItemRetrieval
    {
        IEnumerable<Item> GetSitecoreItems(Item feedItem);
    }
}
