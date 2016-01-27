using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Search.Queries;
using Velir.Search.Core.Search.Results;

namespace Velir.Search.Core.Search.Managers
{
	/// <summary>
	/// Interface designed to define how to search content in Sitecore
	/// </summary>
	/// <typeparam name="T">The type of the result.</typeparam>
	public interface ISearchManager<T> where T : SearchResultItem
	{
		IQueryResults GetItems(ISearchQueryable<T> queryArguments);

		int GetCount(ISearchQueryable<T> queryArguments);
	}
}
