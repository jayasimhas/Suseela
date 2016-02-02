using System.Collections.Generic;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Search.Sorts;

namespace Velir.Search.Core.Models
{
	public interface ISearchRequest
	{
		string PageId { get; set; }
		int Page { get; set; }
		int PerPage { get; set; }
		string SortBy { get; set; }
		string SortOrder { get; set; }
		IDictionary<string, string> QueryParameters { get; set; }

		IEnumerable<SortOption> GetSorts();
		IEnumerable<ISearchRefinement> GetRefinements();
	}
}