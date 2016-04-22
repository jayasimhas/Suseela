using System;

namespace Informa.Library.User.Search
{
	public class SavedSearchEntity : SavedSearchItemId, ISavedSearchEntity
	{
		public virtual string SearchString { get; set; }
		public bool HasAlert { get; set; }
		public DateTime DateCreated { get; set; }
	}

	public class SavedSearchItemId : ISavedSearchItemId
	{
		public virtual string Username { get; set; }
		public virtual string Name { get; set; }
	}
}