using System;

namespace Informa.Library.User.Search
{
	public interface ISavedSearchEntity : ISavedSearchItemId
	{
		string SearchString { get; set; }
		bool HasAlert { get; set; }
		DateTime DateCreated { get; set; }
        string UnsubscribeToken { get; set; }
        string Publication { get; set; }
    }
}
