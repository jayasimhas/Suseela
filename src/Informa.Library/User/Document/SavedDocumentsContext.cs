using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Document
{
	[AutowireService(LifetimeScope.Default)]
	public class SavedDocumentsContext : ISavedDocumentsContext
	{
		private const string sessionKey = nameof(SavedDocumentsContext);

		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;
		protected readonly IFindSavedDocuments FindSavedDocuments;

		public SavedDocumentsContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IFindSavedDocuments findSavedDocuments)
		{
			UserContext = userContext;
			UserSession = userSession;
			FindSavedDocuments = findSavedDocuments;
		}

		public IEnumerable<ISavedDocument> SavedDocuments
		{
			get
			{
				if (!UserContext.IsAuthenticated)
				{
					return Enumerable.Empty<ISavedDocument>();
				}

				var savedDocumentsSession = UserSession.Get<IEnumerable<ISavedDocument>>(sessionKey);

				if (savedDocumentsSession.HasValue)
				{
					return savedDocumentsSession.Value;
				}

				var savedDocuments = SavedDocuments = FindSavedDocuments.Find(UserContext.User?.Username);

				return savedDocuments;
			}
			set
			{
				UserSession.Set(sessionKey, value);
			}
		}

		public void Clear()
		{
			UserSession.Clear(sessionKey);
		}
	}
}
