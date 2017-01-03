using Glass.Mapper.Sc;
using Informa.Library.Search.Results;
using Velir.Search.Core.Managers;

namespace Informa.Library.Search.SearchManager
{
    public class SearchManagerFactory
    {
        public static ISearchManager<InformaSearchResultItem> CreateSearchManager(string indexName, ISitecoreContext context)
        {
            if(indexName.Contains("agri"))//TDDO hard coded agri will be replaced with publication name
            {
                return AgriSearchManager.CreateInstance(indexName, context);
            }
            else if(indexName.Contains("maritime"))//TDDO hard coded agri will be replaced with publication name
                {
                    return MaritimeSearchManager.CreateInstance(indexName, context);
                }
                else
            {
                return PharmaSearchManager.CreateInstance(indexName, context);
            }
        }
    }
}
