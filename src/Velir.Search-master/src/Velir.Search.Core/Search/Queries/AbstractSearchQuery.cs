using System.Linq;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Models;
using Velir.Search.Core.Search.Facets;
using Velir.Search.Core.Search.PredicateBuilders;
using Velir.Search.Core.Search.Sorts;

namespace Velir.Search.Core.Search.Queries
{
	public abstract class AbstractSearchQuery<T> : ISearchQueryable<T> where T : SearchResultItem
	{
		protected AbstractSearchQuery(ISearchRequest request)
		{
			SearchRequest = request;
		}

		public ISearchRequest SearchRequest { get; set; }
		public ISortBuilder<T> SortBuilder { get; set; }
		public IPredicateBuilder<T> PredicateBuilder { get; set; }
		public IFacetBuilder<T> FacetBuilder { get; set; }

		public IQueryable<T> ApplySkip(IQueryable<T> query)
		{
			var skip = SearchRequest.Page * SearchRequest.PerPage - SearchRequest.PerPage;
			if (skip > 0)
			{
				query = query.Skip(skip);
			}

			return query;
		}

		public virtual IQueryable<T> ApplyTake(IQueryable<T> query)
		{
			if (SearchRequest.PerPage > 0)
			{
				query = query.Take(SearchRequest.PerPage);
			}

			return query;
		}

		public virtual IQueryable<T> ApplyFilters(IQueryable<T> query)
		{
			if (PredicateBuilder != null)
			{
				var @where = PredicateBuilder.Build();
				if (@where != null)
				{
					query = query.Where(@where);
				}
			}

			return query;
		}

		public virtual IQueryable<T> ApplySorts(IQueryable<T> query)
		{
			if (SortBuilder != null)
			{
				var orderBy = SortBuilder.Build();
				if (orderBy != null)
				{
					var sortedQuery = orderBy(query);
					if (sortedQuery != null)
					{
						query = sortedQuery;
					}
				}
			}

			return query;
		}

		public virtual IQueryable<T> ApplyFacets(IQueryable<T> query)
		{
			if (FacetBuilder != null)
			{
				var facets = FacetBuilder.Build();
				if (facets != null)
				{
					query = facets.Aggregate(query, (current, facetExp) => current.FacetOn(facetExp, 1));
				}
			}

			return query;
		}

		public virtual IQueryable<T> ApplyAll(IQueryable<T> query)
		{
			query = ApplyFilters(query);
			query = ApplySorts(query);
			query = ApplyFacets(query);
			query = ApplySkip(query);
			query = ApplyTake(query);
			
			return query;
		}
	}
}
