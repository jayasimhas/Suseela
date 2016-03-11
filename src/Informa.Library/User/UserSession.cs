using Informa.Library.Session;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserSession : IUserSession
	{
		private const string UserSessionKey = "User.";

		protected readonly ISessionStore SessionStore;

		public UserSession(
			ISessionStore sessionStore)
		{
			SessionStore = sessionStore;
		}

		public T Get<T>(string key)
		{
			return SessionStore.Get<T>(CreateSessionKey(key));
		}

		public void Set<T>(string key, T obj)
		{
			SessionStore.Set(CreateSessionKey(key), obj);
		}

		public string CreateSessionKey(string key)
		{
			return string.Concat(UserSessionKey, key);
		}
	}
}
