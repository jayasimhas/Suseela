using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Subscription;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Linq;
using Informa.Library.User.Profile;
using Informa.Library.Subscription.User;

namespace Informa.Web.ViewModels
{
    [AutowireService(LifetimeScope.PerScope)]
    public class IndividualRenewalMessageViewModel : IIndividualRenewalMessageViewModel
    {
        private const string PRODUCT_CODE = "scrip";
        private const string PRODUCT_TYPE = "publication";
        private readonly string[] SUBSCRIPTIONTYPE = new string[] { "individual", "free-trial", "individual internal" };

        protected readonly ITextTranslator TextTranslator;
        protected readonly IIndividualSubscriptionRenewalMessageContext Context;
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IUserSubscriptionsContext UserSubscriptionsContext;

        public IndividualRenewalMessageViewModel(
            ITextTranslator textTranslator, 
            IIndividualSubscriptionRenewalMessageContext context, 
            IAuthenticatedUserContext userContext, 
            ISiteRootContext siteRootContext,
			IUserSubscriptionsContext userSubscriptionsContext)
        {
            Context = context;
            TextTranslator = textTranslator;
            UserContext = userContext;
            SiteRootContext = siteRootContext;
            UserSubscriptionsContext = userSubscriptionsContext;
        }

        public string DismissText
        {
            get
            {
                return TextTranslator.Translate("Subscriptions.Renewals.Dismiss");
            }
        }

        private ISubscription GetLatestRecord()
        {
            return UserSubscriptionsContext.Subscriptions?.OrderByDescending(o => o.ExpirationDate).FirstOrDefault() ?? null;
        }

        public bool Display
        {
            get
            {
                if (UserContext == null || !UserContext.IsAuthenticated)
                    return false;

                var latestRecord = GetLatestRecord();

                if(latestRecord == null
                    || latestRecord.ProductCode.ToLower() != PRODUCT_CODE
                    || (latestRecord.ExpirationDate - DateTime.Now).TotalDays > SiteRootContext.Item.Days_To_Expiration
                    || SUBSCRIPTIONTYPE.Contains(latestRecord.SubscriptionType.ToLower()) == false
                    || latestRecord.ProductType.ToLower() != PRODUCT_TYPE)
                    return false;

                return true;
            }
        }

        public string Id
        {
            get
            {
                return Context.ID;
            }
        }

        public string Message
        {
            get
            {
                var latestRecord = GetLatestRecord();
                if(latestRecord == null)
                    return Context.Message_FreeTrial
                        .Replace("#FIRST_NAME#", UserContext.User.Name)
                        .Replace("#SUB_EXPIRATION#", string.Empty);
                else if (latestRecord?.SubscriptionType.ToLower() == "free-trial")
                    return Context.Message_FreeTrial
                        .Replace("#FIRST_NAME#", UserContext.User.Name)
                        .Replace("#SUB_EXPIRATION#", latestRecord.ExpirationDate.ToShortDateString());
                else
                    return Context.Message_IndividualSubscriptiong
                        .Replace("#FIRST_NAME#", UserContext.User.Name)
                        .Replace("#SUB_EXPIRATION#", latestRecord.ExpirationDate.ToShortDateString());
            }
        }

        public string RenewURL
        {
            get
            {
                return Context.RenewalLinkURL;
            }
        }

        public string RenewURLText
        {
            get
            {
                return Context.RenewalLinkText;
            }
        }
    }
}