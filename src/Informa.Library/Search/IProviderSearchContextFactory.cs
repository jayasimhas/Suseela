using Sitecore.ContentSearch;

namespace Informa.Library.Search
{
	public interface IProviderSearchContextFactory
	{
		IProviderSearchContext Create();
		IProviderSearchContext Create(string database);
	}
}
