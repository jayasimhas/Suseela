using Glass.Mapper.Sc;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Search.Queries;
using Velir.Search.Core.Search.Results;

namespace Velir.Search.Core.Search.Managers
{
	public class SearchManager<T> : AbstractSearchManager<T> where T : SearchResultItem
	{
		public SearchManager(string databaseName):base(databaseName)
		{
		}

		public SearchManager(string indexName = null, ISitecoreService service = null)
			: base(indexName, service)
		{
		}

		public SearchManager(ISearchIndex index)
			: base(index)
		{
		}
		
		public override IQueryResults GetItems(ISearchQueryable<T> queryArguments)
		{
			using (var context = SearchIndex.CreateSearchContext())
			{
				var query = context.GetQueryable<T>();

				if (queryArguments != null)
				{
					query = queryArguments.ApplyAll(query);
				}

				return new QueryResults<T>(queryArguments.SearchRequest, query.GetResults(), query.GetFacets());
			}
		}

		public override int GetCount(ISearchQueryable<T> queryArguments)
		{
			using (var context = SearchIndex.CreateSearchContext())
			{
				var query = context.GetQueryable<T>();

				if (queryArguments != null)
				{
					query = queryArguments.ApplyFilters(query);
				}

				return query.GetResults().TotalSearchResults;
			}
		}
	}
}
