namespace Informa.Library.Session
{
	public abstract class SpecificSessionStore : ISessionStore, ISpecificSessionStore
	{
		private const string basePrefix = "Informa.";

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

		public abstract string SessionStoreId { get; }

		public string Prefix => string.Concat(basePrefix, SessionStoreId);

		public string CreatePrefixedSessionKey(string key)
		{
			return string.Concat(Prefix, ".", key);
		}

		public void ClearAll(string keyStartsWith)
		{
			SessionStore.ClearAll(string.Concat(Prefix, keyStartsWith));
		}

		public void Clear()
		{
			SessionStore.ClearAll(Prefix);
		}
	}
}
