using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Search
{
	public class SavedSearchEntity : SavedSearchItemId, ISavedSearchEntity
	{
		public virtual string SearchString { get; set; }
		public bool HasAlert { get; set; }
		public DateTime DateCreated { get; set; }
	    public string UnsubscribeToken { get; set; }
        public string Publication { get; set; }
	}

	public class SavedSearchItemId : ISavedSearchItemId, IEquatable<ISavedSearchItemId>
	{
		public virtual string Username { get; set; }
		public virtual string Name { get; set; }
		public bool Equals(ISavedSearchItemId other)
		{
			return Username == other.Username && Name == other.Name;
		}
	}

	public class SearchStringEqualityComparer: IEqualityComparer<ISavedSearchEntity>
	{
		public bool Equals(ISavedSearchEntity x, ISavedSearchEntity y)
		{
			var xParams = x.SearchString.Split('&');
			var yParams = y.SearchString.Split('&');

			if (xParams.Length != yParams.Length) return false;

			return xParams.All(xp => yParams.Contains(xp));
		}

		public int GetHashCode(ISavedSearchEntity obj)
		{
			return obj.SearchString.Split('&').Select(p => p.GetHashCode()).Sum();
		}
	}
}