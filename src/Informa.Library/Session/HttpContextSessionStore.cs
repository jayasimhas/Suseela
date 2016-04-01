using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Library.Session
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HttpContextSessionStore : ISessionStore
	{
		public T Get<T>(string key)
		{
			var obj = HttpContext.Current.Session[key];

			return obj is T ? (T)obj : default(T);
		}

		public void Set<T>(string key, T obj)
		{
			HttpContext.Current.Session[key] = obj;
		}

		public void ClearAll(string keyStartsWith)
		{
			foreach (string key in HttpContext.Current.Session)
			{
				if (key.StartsWith(keyStartsWith))
				{
					HttpContext.Current.Session.Remove(key);
				}
			}
		}
	}
}
