using System.Collections;
using System.Collections.Generic;
using Velir.Search.Core.Models;
using Velir.Search.Core.Results;
using Velir.Search.Core.Results.Facets;

namespace Informa.Library.Search.Results
{
	public class QueryResults : IQueryResults
	{
		public IList<T> GetModels<T>() where T : class
		{
			throw new System.NotImplementedException();
		}

		public ISearchRequest Request { get; set; }
		public long TotalResults { get; set; }
		public IList Results { get; set; }
		public IEnumerable<FacetGroup> Facets { get; set; }
	}
}