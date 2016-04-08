using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Profile
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
			var result = SaveDocument.Save(UserContext.User.Username, documentName, documentDescription, documentId);

			if (result.Success)
			{
				SavedDocumentsContext.Clear();
			}

			return result;
		}
	}
}
