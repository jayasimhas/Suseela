using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class SubscriptionsViewModel : GlassViewModel<ISubscriptions_Page>
    {
        public readonly ITextTranslator TextTranslator;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly IManageSubscriptions ManageSubscriptions;
        public readonly ISignInViewModel SignInViewModel;

        public SubscriptionsViewModel(
            ITextTranslator translator,
            IAuthenticatedUserContext userContext,
            IManageSubscriptions manageSubscriptions,
            ISignInViewModel signInViewModel)
        {
            TextTranslator = translator;
            UserContext = userContext;
            ManageSubscriptions = manageSubscriptions;
            SignInViewModel = signInViewModel;

            var result = ManageSubscriptions.QueryItems(UserContext.User);
            Subscriptions = (result.Success)
                            ? result.Subscriptions
                            : Enumerable.Empty<ISubscription>();
        }

        public IEnumerable<ISubscription> Subscriptions;

        public bool ShowRenewButton(string productCode)
        {
            //subscription expires < 119 days && there isn't another subscription with an expiration date in the future
            IEnumerable<ISubscription> matches = Subscriptions.Where(a => a.ProductCode.Equals(productCode));
            int renewCount = matches.Count(b => WithinRenewRange(b.ExpirationDate));
            return renewCount > 0 && matches.Count() == renewCount;
        }

        public bool ShowSubscribeButton(string productCode, DateTime dt)
        {
            //find all of same product type
            IEnumerable<ISubscription> matches = Subscriptions.Where(a => a.ProductCode.Equals(productCode));
            if (!matches.Any())
                return false;

            //Do Not Show Subscribe Button if user has an has subscription that's not a free trial
            bool hasSubscription = matches.Any(b => IsValidSubscription(b.ExpirationDate) && !IsFreeTrial(b));
            if (hasSubscription)
                return false;

            //find current product
            IEnumerable<ISubscription> curMatch = matches.Where(c => c.ExpirationDate.Equals(dt));
            if (!curMatch.Any())
                return false;
            
            //Show If the user has a Free Trial and has not yet subscribed.
            ISubscription s = curMatch.First();
            if (IsFreeTrial(s) && !hasSubscription)
                return true;

            //Continue showing the "Subscribe" button even after the Free Trial has expired.
            return !IsValidSubscription(s.ExpirationDate);
        }


        public bool WithinRenewRange(DateTime dt)
        {
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((dt - DateTime.Now).TotalDays);
            return days < 119;
        }

        public bool IsValidSubscription(DateTime dt)
        {
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((dt - DateTime.Now).TotalDays);
            return days > 0;
        }

        public bool IsFreeTrial(ISubscription s)
        {
            return s.SubscriptionType.ToLower().Equals("free trial");
        }

        public string OffSiteSubscriptionLink => GlassModel.Off_Site_Subscription_Link?.Url ?? "#";
        public bool IsAuthenticated => UserContext.IsAuthenticated;
        public string Title => GlassModel?.Title;
        public string PublicationText => TextTranslator.Translate("Subscriptions.Publication");
        public string SubscriptionTypeText => TextTranslator.Translate("Subscriptions.SubscriptionType");
        public string ExpirationDateText => TextTranslator.Translate("Subscriptions.ExpirationDate");
    }
}