﻿using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Linq;

namespace Informa.Library.User.Profile
{
	[AutowireService(LifetimeScope.Default)]
	public class IsSavedDocumentContext : IIsSavedDocumentContext
	{
		protected readonly ISavedDocumentsContext SavedDocumentsContext;

		public IsSavedDocumentContext(
			ISavedDocumentsContext savedDocumentsContext)
		{
			SavedDocumentsContext = savedDocumentsContext;
		}

		public bool IsSaved(Guid documentId)
		{
			var rawDocuementId = documentId.ToString("D").ToUpper();

			return SavedDocumentsContext.SavedDocuments.Any(sd => sd.DocumentId.ToUpper().Equals(rawDocuementId));
		}
	}
}
