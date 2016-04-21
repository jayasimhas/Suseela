namespace Informa.Library.User.Search
{
	public interface ISavedSearchUserContext : IUserContentRepository<ISavedSearchEntity>
	{
		void Clear();
	}
}
