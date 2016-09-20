using Glass.Mapper.Sc;
using Informa.Library.Search.Results;
using Informa.Library.Search.SearchIndex;
using Informa.Library.Search.SearchManager;
using Informa.Library.Site;
using System.Web.Http;
using Velir.Search.Core.Managers;

namespace Informa.Web.Controllers.Search
{
    public class InformaBaseSearchController : ApiController
    {
        public ISearchManager<InformaSearchResultItem> _searchManager;
        public InformaBaseSearchController(ISiteRootContext siterootContext, ISitecoreContext context, ISearchIndexNameService indexNameService)
        {
            string indexName = indexNameService.GetIndexName();
            _searchManager = SearchManagerFactory.CreateSearchManager(indexName, context);
        }
    }
}