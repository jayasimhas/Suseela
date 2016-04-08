using System.Collections.Generic;

namespace Informa.Library.User.Document
{
	public interface ISavedDocumentsContext
	{
		IEnumerable<ISavedDocument> SavedDocuments { get; set; }
		void Clear();
	}
}