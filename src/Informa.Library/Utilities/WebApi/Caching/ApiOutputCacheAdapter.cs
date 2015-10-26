using System;
using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Attributes;
using WebApi.OutputCache.Core.Cache;

namespace Informa.Library.Utilities.WebApi.Caching
{
	/// <summary>
	/// An adapter for the IApiOutputCache that uses the SiteCache implementation internally, including the eviction policies for said cache
	/// </summary>
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ApiOutputCacheAdapter : IApiOutputCache
	{
		private readonly ICacheProvider _cacheProvider;

		public ApiOutputCacheAdapter(ICacheProvider cacheProvider)
		{
			if (cacheProvider == null) throw new ArgumentNullException("cacheProvider");
			_cacheProvider = cacheProvider;
		}

		public void RemoveStartsWith(string key)
		{
			// Do nothing.
		}

		public T Get<T>(string key) where T : class
		{
			return _cacheProvider.GetFromCache<T>(key);
		}

		public object Get(string key)
		{
			return _cacheProvider.GetFromCache<object>(key);
		}

		public void Remove(string key)
		{
			// Do nothing.
		}

		public bool Contains(string key)
		{
			return _cacheProvider.GetFromCache<object>(key) != null;
		}

		public void Add(string key, object o, DateTimeOffset expiration, string dependsOnKey = null)
		{
			// Ignore expiration
			_cacheProvider.AddToCache(key, o);
		}

		public IEnumerable<string> AllKeys
		{
			// Not implemented
			get { return Enumerable.Empty<string>(); }
		}
	}
}
