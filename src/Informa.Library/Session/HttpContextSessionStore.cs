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

			return (T)(obj is T ? obj : null);
		}

		public void Set<T>(string key, T obj)
		{
			HttpContext.Current.Session[key] = obj;
		}
	}
}
