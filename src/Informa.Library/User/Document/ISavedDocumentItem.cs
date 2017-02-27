using Informa.Library.Publication;
using System;

namespace Informa.Library.User.Document
{
	public interface ISavedDocumentItem
	{
		string DocumentId { get; }
		ISitePublication Publication { get; }
		DateTime PublishedOn { get; }
		DateTime SavedOn { get; }
		string Title { get; }
		string Url { get; }
		bool IsExternalUrl { get; }
        string SalesforceId { get; set; }
    }
}