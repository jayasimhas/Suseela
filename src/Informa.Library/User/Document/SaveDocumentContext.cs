using Informa.Library.SalesforceConfiguration;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Informa.Library.Utilities.CMSHelpers;
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
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IAddUserProductPreference AddUserProductPreference;
        protected readonly IVerticalRootContext VerticalRootContext;
        protected readonly ISiteRootContext SiteRootContext;

        public SaveDocumentContext(
			IAuthenticatedUserContext userContext,
			ISavedDocumentsContext savedDocumentsContext,
			ISaveDocument saveDocument,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            IAddUserProductPreference addUserProductPreference,
            IVerticalRootContext verticalRootContext,
            ISiteRootContext siteRootContext)
		{
			UserContext = userContext;
			SavedDocumentsContext = savedDocumentsContext;
			SaveDocument = saveDocument;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            AddUserProductPreference = addUserProductPreference;
            VerticalRootContext = verticalRootContext;
            SiteRootContext = siteRootContext;
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
			var result =SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                AddUserProductPreference.AddSavedDocument(VerticalRootContext?.Item?.Vertical_Name ?? string.Empty, SiteRootContext?.Item?.Publication_Code ?? string.Empty, UserContext?.User?.Username, documentName, documentDescription, documentId, UserContext.User?.AccessToken ?? string.Empty) : 
                SaveDocument.Save(UserContext.User?.Username, documentName, documentDescription, documentId);

			if (result.Success)
			{
				SavedDocumentsContext.Clear();
			}

			return result;
		}
	}
}
