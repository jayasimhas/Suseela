using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
	public interface IFindSavedDocuments
	{
		IEnumerable<ISavedDocument> Find(string username);
	}
}
