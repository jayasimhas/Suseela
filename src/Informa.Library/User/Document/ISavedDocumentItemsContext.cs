using System.Collections.Generic;

namespace Informa.Library.User.Document
{
	public interface ISavedDocumentItemsContext
	{
		IEnumerable<ISavedDocumentItem> SavedDocumentItems { get; }
	}
}