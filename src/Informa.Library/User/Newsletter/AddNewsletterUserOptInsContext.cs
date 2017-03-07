using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Informa.Library.Utilities.CMSHelpers;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Newsletter
{
    [AutowireService]
    public class AddNewsletterUserOptInsContext : IAddNewsletterUserOptInsContext
    {
        private const string sessionKey = "Email Preference";
        protected readonly IAddUserProductPreference AddUserOptIns;
        protected readonly INewsletterUserOptInsContext OptInsContext;
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;
        protected readonly IAuthenticatedUserSession UserSession;
        protected readonly INewsletterUserOptInsContext NewsletterUserOptInsContext;

        public AddNewsletterUserOptInsContext(
            INewsletterUserOptInsContext optInsContext,
            IAuthenticatedUserContext userContext,
            IAddUserProductPreference addUserOptIns,
            ISiteRootContext siteRootContext,
            IVerticalRootContext verticalRootContext,
            IAuthenticatedUserSession userSession,
            INewsletterUserOptInsContext newsletterUserOptInsContext
            )
        {
            OptInsContext = optInsContext;
            UserContext = userContext;
            AddUserOptIns = addUserOptIns;
            SiteRootContext = siteRootContext;
            VerticalRootContext = verticalRootContext;
            UserSession = userSession;
            NewsletterUserOptInsContext = newsletterUserOptInsContext;
        }

        public bool Add(IEnumerable<INewsletterUserOptIn> optIns)
        {
            if (!UserContext.IsAuthenticated)
            {
                return false;
            }
            var success = AddUserOptIns.AddNewsletterUserOptIns(VerticalRootContext?.Item?.Vertical_Name ?? string.Empty,
                SiteRootContext?.Item?.Publication_Code ?? string.Empty,
                UserContext.User?.Username ?? string.Empty, UserContext?.User.AccessToken, optIns);

            if (success)
            {
                clear();
            }

            return success;
        }

        public void clear()
        {
            UserSession.Clear(sessionKey);
            NewsletterUserOptInsContext.Clear();
        }
    }
}
