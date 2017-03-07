using Informa.Library.SalesforceConfiguration;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Informa.Library.Utilities.CMSHelpers;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
    [AutowireService(LifetimeScope.PerScope)]
    public class NewsletterUserOptInsContext : INewsletterUserOptInsContext
    {
        private const string sessionKey = nameof(NewsletterUserOptInsContext);

        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IAuthenticatedUserSession UserSession;
        protected readonly IFindNewsletterUserOptIns NewsletterOptIn;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IGetUserProductPreferences GetUserProductPreferences;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;

        public NewsletterUserOptInsContext(
            IAuthenticatedUserContext userContext,
            IAuthenticatedUserSession userSession,
            IFindNewsletterUserOptIns newsletterOptIn,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            IGetUserProductPreferences getUserProductPreferences,
            ISiteRootContext siteRootContext,
            IVerticalRootContext verticalRootContext)
        {
            NewsletterOptIn = newsletterOptIn;
            UserSession = userSession;
            UserContext = userContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            GetUserProductPreferences = getUserProductPreferences;
            SiteRootContext = siteRootContext;
            VerticalRootContext = verticalRootContext;
        }

        public IEnumerable<INewsletterUserOptIn> OptIns
        {
            get
            {
                if (!UserContext.IsAuthenticated)
                {
                    return Enumerable.Empty<INewsletterUserOptIn>();
                }

                var optInsSession = UserSession.Get<IEnumerable<INewsletterUserOptIn>>(sessionKey);

                if (optInsSession.HasValue)
                {
                    return optInsSession.Value;
                }
                var optIns = OptIns = UserContext.IsAuthenticated ? SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                     GetUserProductPreferences.GetProductPreferences<IList<INewsletterUserOptIn>>(UserContext.User,
                     VerticalRootContext?.Item?.Vertical_Name,
                     SiteRootContext?.Item?.Publication_Code,
                     ProductPreferenceType.EmailPreference) : NewsletterOptIn.Find(UserContext.User?.Username) : Enumerable.Empty<INewsletterUserOptIn>();

                return optIns;
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
