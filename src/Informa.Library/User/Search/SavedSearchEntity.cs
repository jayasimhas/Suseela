using System;

namespace Informa.Library.User.Search
{
	public class SavedSearchEntity : ISavedSearchEntity, ISavedSearchItemId
	{
		public virtual string Username { get; set; }
		public virtual string Title { get; set; }
		public virtual string Url { get; set; }
		public bool HasAlert { get; set; }
		public DateTime DateSaved { get; set; }
	}
}