using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Models;

namespace Velir.Search.Core.Search.Sorts
{
	public class SearchSortBuilder<T> : AbstractSortBuilder<T> where T : SearchResultItem
	{
		public SearchSortBuilder(IEnumerable<SortOption> sortOptions) : base(sortOptions)
		{
			
		} 

		public SearchSortBuilder(IEnumerable<ISort> sorts)
			: base(sorts.Select(s => new SortOption(s)))
		{

		}
	}
}
