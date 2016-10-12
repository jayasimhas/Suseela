using Glass.Mapper.Sc;
using Informa.Library.Search.Results;
using Velir.Search.Core.Managers;

namespace Informa.Library.Search.SearchManager
{
    public class SearchManagerFactory
    {
        public static ISearchManager<InformaSearchResultItem> CreateSearchManager(string indexName, ISitecoreContext context)
        {
            if(indexName.Contains("agri"))
            {
                return AgriSearchManager.CreateInstance(indexName, context);
            }
            else
            {
                return PharmaSearchManager.CreateInstance(indexName, context);
            }
        }
    }
}
