using Sitecore.ContentSearch;
using Sitecore;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Search
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ProviderSearchContextFactory : IProviderSearchContextFactory
	{
		public IProviderSearchContext Create()
		{
			var indexName = string.Format("sitecore_{0}_index", Context.Database.Name.ToLower());

			return ContentSearchManager.GetIndex(indexName).CreateSearchContext();
		}

		public IProviderSearchContext Create(string database)
		{
			var indexName = string.Format("sitecore_{0}_index", database.ToLower());

			return ContentSearchManager.GetIndex(indexName).CreateSearchContext();
		}
	}
}
