namespace Informa.Library.User.Search
{
	public class SavedSearchInput : ISavedSearchSaveable
	{
		public string Title { get; set; }
		public string Url { get; set; }
		public bool AlertEnabled { get; set; }
        public string Id { get; set; }
    }
}