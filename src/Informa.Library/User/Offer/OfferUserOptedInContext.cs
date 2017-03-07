using Informa.Library.SalesforceConfiguration;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Newsletter;
using Informa.Library.User.ProductPreferences;
using Informa.Library.Utilities.CMSHelpers;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Common;
using System.Linq;

namespace Informa.Library.User.Offer
{
	[AutowireService(LifetimeScope.Default)]
	public class OfferUserOptedInContext : IOfferUserOptedInContext
	{
		private const string sessionKey = nameof(OfferUserOptedInContext);

		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;
		protected readonly IOfferUserOptedIn OfferOptedIn;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IGetUserProductPreferences GetUserProductPreferences;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;


        public OfferUserOptedInContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IOfferUserOptedIn offerOptedIn,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            IGetUserProductPreferences getUserProductPreferences,
            ISiteRootContext siteRootContext,
            IVerticalRootContext verticalRootContext)
		{
			UserContext = userContext;
			UserSession = userSession;
			OfferOptedIn = offerOptedIn;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            GetUserProductPreferences = getUserProductPreferences;
            SiteRootContext = siteRootContext;
            VerticalRootContext = verticalRootContext;
        }

		public OffersOptIn OptedIn
		{
			get
			{
				if (!UserContext.IsAuthenticated)
				{
                    return new OffersOptIn();
                }

				var optedInSession = UserSession.Get<OffersOptIn>(sessionKey);

				if (optedInSession.HasValue)
				{
					return optedInSession.Value;
				}

				var optedIn = OptedIn = SalesforceConfigurationContext.IsNewSalesforceEnabled ? 
                    GetUserProductPreferences.GetProductPreferences<OffersOptIn>(UserContext.User,
                    VerticalRootContext?.Item?.Vertical_Name,
                    SiteRootContext?.Item?.Publication_Code,
                    ProductPreferenceType.EmailSignUp) :  
                    OfferOptedIn.OptedIn(UserContext.User?.Username);

				return optedIn;
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
