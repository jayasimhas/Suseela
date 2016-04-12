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

        public bool ShowRenewButton(string productCode)
        {
            //The user's individual subscription expires in < 119 days and the user has not renewed 
            //(ie. there isn't another subscription with an expiration date in the future)

            //translation: if there isn't any subscription outside renew range
            return !Subscriptions.Any(a => a.ProductCode.Equals(productCode) && !WithinRenewRange(a.ExpirationDate));
        }

        public bool ShowSubscribeButton(string productCode)
        {
            //Show Subscribe Button:
            //If the user has a Free Trial and has not yet subscribed.
            //Continue showing the "Subscribe" button even after the Free Trial has expired.
            //Do Not Show Subscribe Button
            //If the user has an (active or expired Free Trial) AND(has subscribed)

            //doesn't mention case of only having expired regular subscription
            //assuming would still want them to subscribe if they were on regular expiration also
            
            //translation: if there aren't any valid subscriptions
            return !Subscriptions.Any(k => k.ProductCode.Equals(productCode) && IsValidSubscription(k));
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