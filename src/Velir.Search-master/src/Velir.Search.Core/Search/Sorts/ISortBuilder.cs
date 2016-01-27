using System;
using System.Linq;
using Sitecore.ContentSearch.SearchTypes;

namespace Velir.Search.Core.Search.Sorts
{
	public interface ISortBuilder<T> where T : SearchResultItem
	{
		Func<IQueryable<T>, IOrderedQueryable<T>> Build();
	}
}
