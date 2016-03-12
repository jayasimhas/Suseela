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
            Subscriptions = false //(result.Success)
                            ? result.Subscriptions.Where(z => z.ProductType.Equals(ProductTypeKey))
                            : new List<ISubscription>()
                            {
                                //case 1 : show button
                                new Library.User.Profile.Subscription() //within renew range
                                {
                                    Publication = "Test Show Renew",
                                    ExpirationDate = DateTime.Now.AddDays(118),
                                    ProductCode = "AA"
                                },
                                //case 2 - one outside renew range : don't show button
                                new Library.User.Profile.Subscription() //within renew range
                                {
                                    Publication = "Test Don't Show Renew",
                                    ExpirationDate = DateTime.Now.AddDays(118),
                                    ProductCode = "BB"
                                },
                                new Library.User.Profile.Subscription() //outside renew range
                                {
                                    Publication = "Test Don't Show Renew",
                                    ExpirationDate = DateTime.Now.AddDays(119),
                                    ProductCode = "BB"
                                },
                                //case 3 - no valid subscriptions : show button
                                new Library.User.Profile.Subscription()
                                {
                                    Publication = "Test Show Subscribe",
                                    ExpirationDate = DateTime.Now.AddDays(-1),
                                    ProductCode = "CC"
                                },
                                //case 4 - valid subscription : don't show
                                new Library.User.Profile.Subscription()
                                {
                                    Publication = "Test Don't Show Subscribe",
                                    ExpirationDate = DateTime.Now.AddDays(1),
                                    ProductCode = "DD"
                                }
                            };
        }

        public IEnumerable<ISubscription> Subscriptions;

        public bool ShowRenewButton(string productCode)
        {
            //The user's individual subscription expires in < 119 days and the user has not renewed 
            //(ie. there isn't another subscription with an expiration date in the future)

            //translation: if there isn't any subscription outside renew range
            return !Subscriptions.Any(a => a.ProductCode.Equals(productCode) && !WithinRenewRange(a.ExpirationDate));
        }

        public bool ShowSubscribeButton(string productCode, DateTime dt)
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
        public string ProductTypeKey => TextTranslator.Translate("Subscriptions.ProductTypeKey");

    }
}