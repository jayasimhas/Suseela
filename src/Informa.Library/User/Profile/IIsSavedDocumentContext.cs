using System;

namespace Informa.Library.User.Profile
{
	public interface IIsSavedDocumentContext
	{
		bool IsSaved(Guid documentId);
	}
}