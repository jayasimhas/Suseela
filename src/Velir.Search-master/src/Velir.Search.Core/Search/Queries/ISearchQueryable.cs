using System.Linq;
using Velir.Search.Core.Models;

namespace Velir.Search.Core.Search.Queries
{
	public interface ISearchQueryable<T>
	{
		ISearchRequest SearchRequest { get; set; }

		IQueryable<T> ApplyFilters(IQueryable<T> query);
		IQueryable<T> ApplySorts(IQueryable<T> query);
		IQueryable<T> ApplyFacets(IQueryable<T> query);
		IQueryable<T> ApplyTake(IQueryable<T> query);
		IQueryable<T> ApplySkip(IQueryable<T> query);
		IQueryable<T> ApplyAll(IQueryable<T> query);
	}
}
