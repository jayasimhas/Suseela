using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
	public interface ISavedDocumentsContext
	{
		IEnumerable<ISavedDocument> SavedDocuments { get; set; }
		void Clear();
	}
}