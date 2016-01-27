using System;
using Autofac;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Search.Managers;

namespace Velir.Search.Core.Search.Factory
{
	public class SearchManagerFactory : ISearchManagerFactory
	{
		private readonly IComponentContext _resolver;

		public SearchManagerFactory(IComponentContext resolver)
		{
			if (resolver == null) throw new ArgumentNullException("resolver");
			_resolver = resolver;
		}

		public ISearchManager<TResultItem> For<TResultItem>() where TResultItem : SearchResultItem
		{
			return _resolver.Resolve<ISearchManager<TResultItem>>();
		}
	}
}
