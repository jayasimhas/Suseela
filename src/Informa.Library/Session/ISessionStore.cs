namespace Informa.Library.Session
{
	public interface ISessionStore
	{
		void Set<T>(string key, T obj);
		ISessionValue<T> Get<T>(string key);
		void Clear(string key);
		void ClearAll(string keyStartsWith);
	}
}
