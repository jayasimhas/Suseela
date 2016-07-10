using System;
using System.Collections.Generic;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Utilities.Extensions
{
	public static class GlassBaseExtensions
	{
		public static IEnumerable<T> GetAncestors<T>(this IGlassBase item) where T : class, IGlassBase
		{
			if (item == null) yield break;

			for (IGlassBase parent = item._Parent; parent != null; parent = parent._Parent)
			{
				var navItem = parent as T;
				if (navItem != null)
				{
					yield return navItem;
				}
			}
		}

		public static T Crawl<T>(this IGlassBase item) where T : class
		{
			return item.Crawl<T>(null);
		}

		public static T Crawl<T>(this IGlassBase item, Func<IGlassBase, bool> func) where T : class
		{
			if (item == null)
			{
				return default(T);
			}

			if (item is T && (func == null || func(item)))
			{
				return item as T;
			}

			return item._Parent.Crawl<T>(func);
		}
	}
}
