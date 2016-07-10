using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Document
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SavedDocumentItemsContext : ISavedDocumentItemsContext
	{
		public SavedDocumentItemsContext(
			ISavedDocumentsContext savedDocumentsContext,
			ISavedDocumentItemsFactory savedDocumentItemsFactory)
		{
			SavedDocumentItems = savedDocumentItemsFactory.Create(savedDocumentsContext.SavedDocuments);
		}

		public IEnumerable<ISavedDocumentItem> SavedDocumentItems { get; set; }
	}
}
