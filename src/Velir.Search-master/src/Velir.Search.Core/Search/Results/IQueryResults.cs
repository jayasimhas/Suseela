using System.Collections;
using System.Collections.Generic;
using Velir.Search.Core.Models;
using Velir.Search.Core.Search.Results.Facets;

namespace Velir.Search.Core.Search.Results
{
	public interface IQueryResults
	{
		ISearchRequest Request { get; }
		long TotalResults { get; }
		IList Results { get; set; }
		IEnumerable<FacetGroup> Facets { get; }

		IList<T> GetModels<T>() where T : class;
	}
}
