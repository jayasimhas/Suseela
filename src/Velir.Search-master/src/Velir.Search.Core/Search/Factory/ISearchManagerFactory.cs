using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Search.Managers;

namespace Velir.Search.Core.Search.Factory
{
	public interface ISearchManagerFactory
	{
		ISearchManager<TResultItem> For<TResultItem>() where TResultItem : SearchResultItem;
	}
}
