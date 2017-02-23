namespace Informa.Library.User.Search
{
	public interface ISavedSearchSaveable
	{
		string Title { get; set; }
		string Url { get; set; }
		bool AlertEnabled { get; set; }
        string Id { get; set; }
    }
}