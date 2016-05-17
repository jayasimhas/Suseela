using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Document
{
	[AutowireService]
	public class SavedDocumentItemsContext : ISavedDocumentItemsContext
	{
		protected readonly ISavedDocumentsContext SavedDocumentsContext;
		protected readonly ISavedDocumentItemsFactory SavedDocumentItemsFactory;

		public SavedDocumentItemsContext(
			ISavedDocumentsContext savedDocumentsContext,
			ISavedDocumentItemsFactory savedDocumentItemsFactory)
		{
			SavedDocumentsContext = savedDocumentsContext;
			SavedDocumentItemsFactory = savedDocumentItemsFactory;
		}

		public IEnumerable<ISavedDocumentItem> SavedDocumentItems => SavedDocumentItemsFactory.Create(SavedDocumentsContext.SavedDocuments);
	}
}
