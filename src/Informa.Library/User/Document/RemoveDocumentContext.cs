using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Document
{
	[AutowireService(LifetimeScope.Default)]
	public class RemoveDocumentContext : IRemoveDocumentContext
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly ISavedDocumentsContext SavedDocumentsContext;
		protected readonly IRemoveDocument RemoveDocument;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IDeleteUserProductPreferences DeleteUserProductPreferences;


        public RemoveDocumentContext(
			IAuthenticatedUserContext userContext,
			ISavedDocumentsContext savedDocumentsContext,
			IRemoveDocument removeDocument,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            IDeleteUserProductPreferences deleteUserProductPreferences)
		{
			UserContext = userContext;
			SavedDocumentsContext = savedDocumentsContext;
			RemoveDocument = removeDocument;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            DeleteUserProductPreferences = deleteUserProductPreferences;
        }

		public ISavedDocumentWriteResult Remove(string documentId, string salesforceId)
		{
			var result =SalesforceConfigurationContext.IsNewSalesforceEnabled ? DeleteUserProductPreferences?.DeleteSavedocument(UserContext.User?.AccessToken, salesforceId) :  RemoveDocument.Remove(UserContext.User?.Username, documentId);

			if (result.Success)
			{
				SavedDocumentsContext.Clear();
			}

			return result;
		}
	}
}
