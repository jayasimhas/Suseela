using System.Collections.Generic;

namespace Informa.Library.User.Document
{
	public interface ISavedDocumentItemsFactory
	{
		IEnumerable<ISavedDocumentItem> Create(IEnumerable<ISavedDocument> savedDocuments);
	}
}