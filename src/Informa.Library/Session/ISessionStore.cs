namespace Informa.Library.Session
{
	public interface ISessionStore
	{
		void Set<T>(string key, T obj);
		T Get<T>(string key);
	}
}
