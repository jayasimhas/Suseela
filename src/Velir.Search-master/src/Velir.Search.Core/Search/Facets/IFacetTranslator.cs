using System.Collections.Generic;
using Velir.Search.Core.Search.Results.Facets;

namespace Velir.Search.Core.Search.Facets
{
	public interface IFacetTranslator
	{
		string GetFacetKey(string fieldName);
		IEnumerable<FacetResultValue> SortValues(string fieldName, IEnumerable<FacetResultValue> values);
	}
}