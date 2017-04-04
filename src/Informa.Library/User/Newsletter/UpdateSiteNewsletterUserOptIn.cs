using Informa.Library.SalesforceConfiguration;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Jabberwocky.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
    [AutowireService]
    public class UpdateSiteNewsletterUserOptIn : IUpdateSiteNewsletterUserOptIn
    {
        protected readonly IUpdateNewsletterUserOptIns UpdateOptIns;
        protected readonly ISiteNewsletterUserOptInsContext SiteNewsletterUserOptInsContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUpdateUserProductPreference UpdateUserProductPreference;
        protected readonly ISiteRootContext SiteRootContext;


        public UpdateSiteNewsletterUserOptIn(
            IUpdateNewsletterUserOptIns updateOptIns,
            ISiteNewsletterUserOptInsContext siteNewsletterUserOptInsContext,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            IAuthenticatedUserContext authenticatedUserContext,
            IUpdateUserProductPreference updateUserProductPreference,
            ISiteRootContext siteRootContext)
        {
            UpdateOptIns = updateOptIns;
            SiteNewsletterUserOptInsContext = siteNewsletterUserOptInsContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            AuthenticatedUserContext = authenticatedUserContext;
            UpdateUserProductPreference = updateUserProductPreference;
            SiteRootContext = siteRootContext;
        }

        public bool Update(string username, bool optIn)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            var optIns = SiteNewsletterUserOptInsContext.OptIns.ToList();

            optIns.ForEach(oi => oi.OptIn = optIn);

            return SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                UpdateUserProductPreference.UpdateNewsletterUserOptIns(AuthenticatedUserContext.User?.AccessToken, AuthenticatedUserContext.User?.Username, SiteRootContext?.Item?.Publication_Code, optIns) :
                UpdateOptIns.Update(optIns, username);
        }
    }
}
