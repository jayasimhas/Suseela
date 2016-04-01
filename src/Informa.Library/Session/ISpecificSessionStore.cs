namespace Informa.Library.Session
{
	public interface ISpecificSessionStore : ISessionStore
	{
		string Id { get; }
		void Clear();
	}
}