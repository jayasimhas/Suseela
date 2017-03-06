using Informa.Library.SalesforceConfiguration;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Newsletter;
using Informa.Library.User.ProductPreferences;
using Informa.Library.Utilities.CMSHelpers;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Offer
{
    [AutowireService(LifetimeScope.Default)]
    public class UpdateOfferUserOptInContext : IUpdateOfferUserOptInContext
    {
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IAddUserProductPreference AddUserProductPreference;
        protected readonly IOfferUserOptedInContext OfferOptedInContext;
        protected readonly IUpdateOfferUserOptIn UpdateOfferOptIn;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IUpdateUserProductPreference UpdateUserProductPreference;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;


        public UpdateOfferUserOptInContext(
    IAuthenticatedUserContext userContext,
    IOfferUserOptedInContext offerOptedInContext,
    IUpdateOfferUserOptIn updateOfferOptIn,
    ISalesforceConfigurationContext salesforceConfigurationContext,
    IAddUserProductPreference addUserProductPreference,
    IUpdateUserProductPreference updateUserProductPreference,
    ISiteRootContext siteRootContext,
    IVerticalRootContext verticalRootContext
    )
        {
            UserContext = userContext;
            OfferOptedInContext = offerOptedInContext;
            UpdateOfferOptIn = updateOfferOptIn;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            AddUserProductPreference = addUserProductPreference;
            UpdateUserProductPreference = updateUserProductPreference;
            SiteRootContext = siteRootContext;
            VerticalRootContext = verticalRootContext;
        }

        public bool Update(bool optIn, NewsletterPreference method)
        {
            if (!UserContext.IsAuthenticated)
            {
                return false;
            }
            bool success = false;
            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                if (method.Equals(NewsletterPreference.Add))
                {
                    success = AddUserProductPreference.AddOffersOptIns(VerticalRootContext?.Item?.Vertical_Name ?? string.Empty,
                SiteRootContext?.Item?.Publication_Code ?? string.Empty,
                UserContext.User?.Username ?? string.Empty, UserContext?.User.AccessToken, optIn);
                }
                else
                {
                    var Optins = OfferOptedInContext.OptedIn;
                    Optins.OptIn = !optIn;
                    success = UpdateUserProductPreference.UpdateOffersOptIns(UserContext?.User.AccessToken, UserContext.User?.Username ?? string.Empty,SiteRootContext?.Item?.Publication_Code ?? string.Empty, Optins);
                }
            }
            else
            {
                success = UpdateOfferOptIn.Update(UserContext.User?.Username, optIn);
            }

            if (success)
            {
                OfferOptedInContext.Clear();
            }

            return success;
        }
    }
}
