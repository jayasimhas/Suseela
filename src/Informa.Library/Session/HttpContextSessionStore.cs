using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Web;

namespace Informa.Library.Session
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HttpContextSessionStore : ISessionStore
	{
		public ISessionValue<T> Get<T>(string key)
		{
			var obj = HttpContext.Current.Session[key];

			if (!(obj is ISessionValue<T>))
			{
				return new SessionValue<T>
				{
					HasValue = false,
					Value = default(T)
				};
			}

			return (ISessionValue<T>)obj;
		}

		public void Set<T>(string key, T obj)
		{
			HttpContext.Current.Session[key] = new SessionValue<T>
			{
				HasValue = true,
				Value = obj
			};
		}

		public void ClearAll(string keyStartsWith)
		{
			var keys = new List<string>();

			foreach (string key in HttpContext.Current.Session)
			{
				if (key.StartsWith(keyStartsWith))
				{
					keys.Add(key);
				}
			}

			keys.ForEach(k => HttpContext.Current.Session.Remove(k));
		}
	}
}
