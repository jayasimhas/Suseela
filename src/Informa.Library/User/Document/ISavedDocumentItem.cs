using System;

namespace Informa.Library.User.Document
{
	public interface ISavedDocumentItem
	{
		string DocumentId { get; }
		string Publication { get; }
		DateTime Published { get; }
		string Title { get; }
		string Url { get; }
		bool OnCurrentSite { get; }
	}
}