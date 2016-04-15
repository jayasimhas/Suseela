using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.Subscription.User;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class SubscriptionsViewModel : GlassViewModel<ISubscriptions_Page>
    {
        public readonly ITextTranslator TextTranslator;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly IUserSubscriptionsContext UserSubscriptionsContext;
        public readonly ISignInViewModel SignInViewModel;

        public SubscriptionsViewModel(
            ITextTranslator translator,
            IAuthenticatedUserContext userContext,
			IUserSubscriptionsContext userSubscriptionsContext,
            ISignInViewModel signInViewModel)
        {
            TextTranslator = translator;
            UserContext = userContext;
            UserSubscriptionsContext = userSubscriptionsContext;
            SignInViewModel = signInViewModel;
        }

        public IEnumerable<ISubscription> Subscriptions => UserSubscriptionsContext.Subscriptions;

        public bool ShowRenewButton(ISubscription subscription)
        {
            //if all subscriptions of this type are within renew range and this subscription is not multi-user 
            return Subscriptions
                    .Where(a => a.ProductCode.Equals(subscription.ProductCode))
                    .All(b => WithinRenewRange(b.ExpirationDate)) 
                && !IsMultiUser(subscription.SubscriptionType);
        }

        public bool ShowSubscribeButton(string productCode)
        {
            //if there aren't any valid subscriptions
            return !Subscriptions.Any(k => k.ProductCode.Equals(productCode) && IsValidSubscription(k));
        }

        public bool IsMultiUser(string subscriptionType)
        {
            return subscriptionType.ToLower().Equals("multi-user");
        }

        public bool WithinRenewRange(DateTime dt)
        {
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((dt - DateTime.Now).TotalDays);
            return days < 119;
        }

        public bool IsValidSubscription(ISubscription s)
        {
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((s.ExpirationDate - DateTime.Now).TotalDays);
            return days > 0;
        }
        
        public string OffSiteRenewLink => GlassModel.Off_Site_Renew_Link?.Url ?? "#";
        public string OffSiteSubscriptionLink => GlassModel.Off_Site_Subscription_Link?.Url ?? "#";
        public bool IsAuthenticated => UserContext.IsAuthenticated;
        public string Title => GlassModel?.Title;
        public string PublicationText => TextTranslator.Translate("Subscriptions.Publication");
        public string SubscriptionTypeText => TextTranslator.Translate("Subscriptions.SubscriptionType");
        public string ExpirationDateText => TextTranslator.Translate("Subscriptions.ExpirationDate");
        public string ActionText => TextTranslator.Translate("Subscriptions.Action");
    }
}