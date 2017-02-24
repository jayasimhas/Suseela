using System;

namespace Informa.Library.User.Search
{
	public interface ISavedSearchEntity : ISavedSearchItemId
	{
        string Id { get; set; }
        string SearchString { get; set; }
		bool HasAlert { get; set; }
		DateTime DateCreated { get; set; }
        string UnsubscribeToken { get; set; }
        string Publication { get; set; }
        string PublicationCode { get; set; }
        string PublicationName { get; set; }
        string VerticalName { get; set; }

    }
}
