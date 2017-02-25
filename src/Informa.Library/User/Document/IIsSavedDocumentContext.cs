using System;

namespace Informa.Library.User.Document
{
	public interface IIsSavedDocumentContext
	{
		bool IsSaved(Guid documentId);
        string GetSalesforceId(Guid documentId);

    }
}