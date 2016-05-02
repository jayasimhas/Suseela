﻿using System;
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

        private readonly Dictionary<string, bool> RenewBtnSettings;
        private readonly Dictionary<string, bool> SubscriptionBtnSettings;

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

            RenewBtnSettings = new Dictionary<string, bool>();
            SubscriptionBtnSettings = new Dictionary<string, bool>();
            foreach (var sub in Subscriptions) {
                //renew btns
                if (!RenewBtnSettings.ContainsKey(sub.ProductCode))
                    RenewBtnSettings.Add(sub.ProductCode, WithinRenewRange(sub.ExpirationDate));
                else 
                    RenewBtnSettings[sub.ProductCode] &= WithinRenewRange(sub.ExpirationDate);

                //subscribe btns
                if (!SubscriptionBtnSettings.ContainsKey(sub.ProductCode))
                    SubscriptionBtnSettings.Add(sub.ProductCode, IsValidSubscription(sub));
                else 
                    SubscriptionBtnSettings[sub.ProductCode] |= IsValidSubscription(sub);
            }
        }

        public IList<ISubscription> Subscriptions => UserSubscriptionsContext.Subscriptions.ToList();
            
        public bool ShowRenewButton(ISubscription subscription)
        {
            //this subscription is not multi-user if all subscriptions of this type are within renew range
            if (IsMultiUser(subscription.SubscriptionType))
                return false;

            return (RenewBtnSettings.ContainsKey(subscription.ProductCode)) && RenewBtnSettings[subscription.ProductCode];
        }

        public bool ShowSubscribeButton(string productCode)
        {
            //if there aren't any valid subscriptions
            return (SubscriptionBtnSettings.ContainsKey(productCode)) && !SubscriptionBtnSettings[productCode];
        }

        public bool IsMultiUser(string subscriptionType)
        {
            return subscriptionType.ToLower().Equals("multi-user");
        }

        public bool WithinRenewRange(DateTime dt)
        {
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((dt - DateTime.Now).TotalDays);
            return days < 119 && days >= 0;
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