using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Document
{
	[AutowireService(LifetimeScope.Default)]
	public class SaveDocumentContext : ISaveDocumentContext
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly ISavedDocumentsContext SavedDocumentsContext;
		protected readonly ISaveDocument SaveDocument;

		public SaveDocumentContext(
			IAuthenticatedUserContext userContext,
			ISavedDocumentsContext savedDocumentsContext,
			ISaveDocument saveDocument)
		{
			UserContext = userContext;
			SavedDocumentsContext = savedDocumentsContext;
			SaveDocument = saveDocument;
		}

		public ISavedDocumentWriteResult Save(string documentName, string documentDescription, string documentId)
		{
			if (SavedDocumentsContext.SavedDocuments.Any(sd => string.Equals(sd.DocumentId, documentId)))
			{
				return new SavedDocumentWriteResult
				{
					Success = true
				};
			}

			var result = SaveDocument.Save(UserContext.User?.Username, documentName, documentDescription, documentId);

			if (result.Success)
			{
				SavedDocumentsContext.Clear();
			}

			return result;
		}
	}
}
