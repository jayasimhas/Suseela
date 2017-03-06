using Informa.Library.SalesforceConfiguration;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Informa.Library.Utilities.CMSHelpers;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
    [AutowireService]
    public class UpdateNewsletterUserOptInsContext : IUpdateNewsletterUserOptInsContext
    {
        protected readonly IUpdateNewsletterUserOptIns UpdateUserOptIns;
        protected readonly INewsletterUserOptInsContext OptInsContext;
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;
        protected readonly IUpdateUserProductPreference UpdateUserProductPreference;

        public UpdateNewsletterUserOptInsContext(
            IUpdateNewsletterUserOptIns updateUserOptIns,
            INewsletterUserOptInsContext optInsContext,
            IAuthenticatedUserContext userContext,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            ISiteRootContext siteRootContext,
            IVerticalRootContext verticalRootContext,
            IUpdateUserProductPreference updateUserProductPreference
            )
        {
            UpdateUserOptIns = updateUserOptIns;
            OptInsContext = optInsContext;
            UserContext = userContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            SiteRootContext = siteRootContext;
            VerticalRootContext = verticalRootContext;
            UpdateUserProductPreference = updateUserProductPreference;
        }

        public bool Update(IEnumerable<INewsletterUserOptIn> optIns)
        {
            if (!UserContext.IsAuthenticated)
            {
                return false;
            }
            var success = SalesforceConfigurationContext.IsNewSalesforceEnabled ? UpdateUserProductPreference.UpdateNewsletterUserOptIns(UserContext.User?.AccessToken,UserContext.User?.Username, UserContext?.User.Name,optIns) : UpdateUserOptIns.Update(optIns, UserContext.User?.Username);

            if (success)
            {
                OptInsContext.Clear();
            }

            return success;
        }
    }
}
