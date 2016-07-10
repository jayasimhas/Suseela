using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Document
{
	[AutowireService(LifetimeScope.Default)]
	public class RemoveDocumentContext : IRemoveDocumentContext
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly ISavedDocumentsContext SavedDocumentsContext;
		protected readonly IRemoveDocument RemoveDocument;

		public RemoveDocumentContext(
			IAuthenticatedUserContext userContext,
			ISavedDocumentsContext savedDocumentsContext,
			IRemoveDocument removeDocument)
		{
			UserContext = userContext;
			SavedDocumentsContext = savedDocumentsContext;
			RemoveDocument = removeDocument;
		}

		public ISavedDocumentWriteResult Remove(string documentId)
		{
			var result = RemoveDocument.Remove(UserContext.User?.Username, documentId);

			if (result.Success)
			{
				SavedDocumentsContext.Clear();
			}

			return result;
		}
	}
}
