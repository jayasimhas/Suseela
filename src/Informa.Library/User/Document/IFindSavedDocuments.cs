using System.Collections.Generic;

namespace Informa.Library.User.Document
{
	public interface IFindSavedDocuments
	{
		IEnumerable<ISavedDocument> Find(string username);
	}
}
