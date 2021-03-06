﻿using Sitecore.ContentSearch;
using Sitecore;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Search
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ProviderSearchContextFactory : IProviderSearchContextFactory
	{
		public IProviderSearchContext Create()
		{
			return Create(Context.Database.Name);
		}

		public IProviderSearchContext Create(string database)
		{
			var indexName = string.Format("informa_content_{0}_index", database.ToLower());

			return ContentSearchManager.GetIndex(indexName).CreateSearchContext();
		}
	}
}
