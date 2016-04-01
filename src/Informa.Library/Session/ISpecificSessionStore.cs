namespace Informa.Library.Session
{
	public interface ISpecificSessionStore : ISessionStore
	{
		void Clear();
	}
}