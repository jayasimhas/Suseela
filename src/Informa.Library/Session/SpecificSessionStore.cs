namespace Informa.Library.Session
{
	public abstract class SpecificSessionStore : ISessionStore
	{
		protected readonly ISessionStore SessionStore;

		public SpecificSessionStore(
			ISessionStore sessionStore)
		{
			SessionStore = sessionStore;
		}

		public T Get<T>(string key)
		{
			return SessionStore.Get<T>(CreatePrefixedSessionKey(key));
		}

		public void Set<T>(string key, T obj)
		{
			SessionStore.Set(CreatePrefixedSessionKey(key), obj);
		}

		public abstract string Prefix { get; }

		public string CreatePrefixedSessionKey(string key)
		{
			return string.Concat(Prefix, ".", key);
		}
	}
}
