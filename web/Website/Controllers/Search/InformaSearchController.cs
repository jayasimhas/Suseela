using Glass.Mapper.Sc;
using Informa.Library.Search.PredicateBuilders;
using Informa.Library.Search.Results;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Factory;
using Velir.Search.Core.Managers;
using Velir.Search.Core.Page;
using Velir.Search.Core.Queries;
using Velir.Search.Core.Results;
using Velir.Search.WebApi.Controllers;
using Velir.Search.WebApi.Models;

namespace Informa.Web.Controllers.Search
{
    public class InformaSearchController : VelirSearchController<InformaSearchResultItem>
    {
        private readonly ICacheProvider _cache;
        private readonly IGlassInterfaceFactory _interfaceFactory;
        private ISitecoreContext _context;
        private readonly ISearchPageParser _parser;
        private readonly ISearchManager<InformaSearchResultItem> _searchManager;

        public InformaSearchController(ISearchManager<InformaSearchResultItem> searchManager, ISearchPageParser parser,
            IGlassInterfaceFactory interfaceFactory, ISitecoreContext context, ICacheProvider cache)
            : base(searchManager, parser)
        {
            _searchManager = searchManager;
            _parser = parser;
            _context = context;
            _cache = cache;
            _interfaceFactory = interfaceFactory;
        }

        public override IQueryResults Get(ApiSearchRequest request)
        {
            //If an improper request is passed in return nothing
            if (string.IsNullOrEmpty(request?.PageId))
            {
                return null;
            }

            var predicateBuilder = new InformaPredicateBuilder<InformaSearchResultItem>(_parser, request);

            //predicateBuilder.ListableOnly = true;

            var q = new SearchQuery<InformaSearchResultItem>(request, _parser);
            q.PredicateBuilder = predicateBuilder;

            return _searchManager.GetItems(q);
        }
    }
}