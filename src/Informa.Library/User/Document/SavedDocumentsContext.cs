using Informa.Library.SalesforceConfiguration;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Informa.Library.Utilities.CMSHelpers;
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
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IGetUserProductPreferences GetUserProductPreferences;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;

        public SavedDocumentsContext(
            IAuthenticatedUserContext userContext,
            IAuthenticatedUserSession userSession,
            IFindSavedDocuments findSavedDocuments,
            IGetUserProductPreferences getUserProductPreferences,
            ISiteRootContext siteRootContext,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            IVerticalRootContext verticalRootContext)
        {
            UserContext = userContext;
            UserSession = userSession;
            FindSavedDocuments = findSavedDocuments;
            GetUserProductPreferences = getUserProductPreferences;
            SiteRootContext = siteRootContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            VerticalRootContext = verticalRootContext;
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

                var savedDocuments = SavedDocuments = SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                    GetUserProductPreferences.GetProductPreferences<IList<ISavedDocument>>(UserContext.User, 
                    VerticalRootContext?.Item?.Vertical_Name,
                    SiteRootContext?.Item?.Publication_Code,
                    ProductPreferenceType.SavedDocuments) :
                    FindSavedDocuments.Find(UserContext.User?.Username);

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
