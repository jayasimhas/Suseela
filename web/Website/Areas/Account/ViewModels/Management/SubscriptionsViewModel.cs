using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.ViewModels.Account;
using Informa.Library.Publication;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using log4net;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class SubscriptionsViewModel : GlassViewModel<ISubscriptions_Page>
    {
        public readonly ITextTranslator TextTranslator;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly ISignInViewModel SignInViewModel;
        private readonly ILog _logger;
        protected readonly IFindSitePublicationByCode FindSitePublication;

        private readonly Dictionary<string, bool> RenewBtnSettings;
        private readonly Dictionary<string, bool> SubscriptionBtnSettings;
        private readonly IEnumerable<ISubscription> _subcriptions;

        public SubscriptionsViewModel(
            ITextTranslator translator,
            IAuthenticatedUserContext userContext,
            IUserSubscriptionsContext userSubscriptionsContext,
            ISignInViewModel signInViewModel,
            IFindSitePublicationByCode findSitePublication,
            ILog logger)
        {
            TextTranslator = translator;
            UserContext = userContext;
            SignInViewModel = signInViewModel;
            FindSitePublication = findSitePublication;
            _logger = logger;
            RenewBtnSettings = new Dictionary<string, bool>();
            SubscriptionBtnSettings = new Dictionary<string, bool>();

            _subcriptions = userSubscriptionsContext.Subscriptions.Where(w => string.IsNullOrEmpty(w.Publication) == false && w.ExpirationDate >= DateTime.Now.AddMonths(-6)).OrderByDescending(o => o.ExpirationDate);

        }

        public IEnumerable<ISite_Root> Sitecorepublications
        {
            get
            {
                var verticalRootItem = GlassModel.GetAncestors<IVertical_Root>().FirstOrDefault();
                return verticalRootItem._ChildrenWithInferType.OfType<ISite_Root>();
            }
        }

        public IEnumerable<SubscriptionViewModel> SubscriptionViewModels
        {
            get
            {
                try
                {
                    var subscriptionsList =  Sitecorepublications.Select(s => new SubscriptionViewModel
                    {
                        Expiration = _subcriptions.Any(p => p.ProductCode.Equals(s.Publication_Code)) ? _subcriptions.FirstOrDefault(p => p.ProductCode.Equals(s.Publication_Code)).ExpirationDate : DateTime.MinValue,
                        Publication = s.Publication_Name,
                        Renewable = IsRenewable(s.Publication_Code),
                        Subscribable = IsSubScribed(s.Publication_Code),
                        ChannelItems = GetChannelItemsByProductCode(s.Publication_Code),
                        IsCurrentPublication = GlassModel.GetAncestors<ISite_Root>().FirstOrDefault().Publication_Code.Equals(s.Publication_Code)
                    });
                    var filteredsubscriptionsList = subscriptionsList.Where(p => p.Subscribable == true && p.Renewable == false).Concat(subscriptionsList.Where(q => q.Renewable == true)).Concat(subscriptionsList.Where(r => r.Subscribable == false));
                    return filteredsubscriptionsList;

                }
                catch(Exception ex)
                {
                    _logger.Error("Error in Subscription page", ex);
                     return new List<SubscriptionViewModel>();
                }

            }
        }

        private bool IsRenewable(string pub_code)
        {
            if(string.IsNullOrEmpty(pub_code))
            {
                _logger.Warn($"Publication code is empty on " + GlassModel._Path);
                return false;
            }
            DateTime expirationDate = _subcriptions.Any(p => p.ProductCode.Equals(pub_code)) ? _subcriptions.FirstOrDefault(p => p.ProductCode.Equals(pub_code)).ExpirationDate : DateTime.MinValue;
            return WithinRenewRange(expirationDate);
        }

        private bool IsSubScribed(string pub_code)
        {
            DateTime expirationDate = _subcriptions.Any(p => p.ProductCode.Equals(pub_code)) ? _subcriptions.FirstOrDefault(p => p.ProductCode.Equals(pub_code)).ExpirationDate : DateTime.MinValue;
            return IsValidSubscription(expirationDate);
        }
        /// <summary>
        /// Gets Channel items for the give product code
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public IEnumerable<SubscriptionChannelViewModel> GetChannelItemsByProductCode(string productCode)
        {
            IEnumerable<SubscriptionChannelViewModel> mappedChannelItems = new List<SubscriptionChannelViewModel>();
            var verticalRootItem = GlassModel.GetAncestors<IVertical_Root>().FirstOrDefault();
            if (verticalRootItem != null)
            {
                try
                {
                    var siteRoot =
                        verticalRootItem._ChildrenWithInferType.OfType<ISite_Root>()
                            .FirstOrDefault(eachChild => eachChild.Publication_Code.Equals(productCode));
                    if (siteRoot != null)
                    {
                        var ChannelPage = siteRoot._ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault()
                            ._ChildrenWithInferType.OfType<IChannels_Page>().FirstOrDefault();


                        if (ChannelPage != null)
                        {
                            var channelItems = ChannelPage._ChildrenWithInferType.OfType<IChannel_Page>();
                            if (channelItems.Any())
                            {
                                mappedChannelItems =
                                (from ch in channelItems
                                 let expirationdate = GetChannelExpirationDate(productCode, ch.Channel_Code, ch._Path)
                                 select new SubscriptionChannelViewModel
                                 {
                                     ChannelName = ch.Display_Text,
                                     ChannelExpirationdate = expirationdate,
                                     Renewable = WithinRenewRange(expirationdate),
                                     Subscribable = IsValidSubscription(expirationdate)
                                 }
                                );
                                var filteredChannelItems = mappedChannelItems.Where(p => p.Subscribable == true && p.Renewable == false).Concat(mappedChannelItems.Where(q => q.Renewable == true)).Concat(mappedChannelItems.Where(r => r.Subscribable == false));
                                return filteredChannelItems;

                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    _logger.Error("Error in Subscription page", ex);
                }
            }
            return mappedChannelItems;
        }
        
        public bool ShowRenewButton(ISubscription subscription)
        {
            //if all subscriptions of this type are within renew range and this subscription is not multi-user 
            return _subcriptions
                            .Where(a => a.ProductCode.Equals(subscription.ProductCode))
                            .All(b => WithinRenewRange(b.ExpirationDate))
                    && !IsMultiUser(subscription.SubscriptionType);
        }

        public bool ShowSubscribeButton(string productCode)
        {
            //if there aren't any valid subscriptions
            return (SubscriptionBtnSettings.ContainsKey(productCode)) && !RenewBtnSettings[productCode];
        }

       
        public bool IsMultiUser(string subscriptionType)
        {
            return subscriptionType.ToLower().Equals("multi-user");
        }

        public bool WithinRenewRange(DateTime dt)
        {
            if (dt == DateTime.MinValue) return false;
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((dt - DateTime.Now).TotalDays);
            return days < 119 && days >= 0;
        }

        public bool IsValidSubscription(DateTime dt)
        {
            if (dt == DateTime.MinValue) return false;
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((dt - DateTime.Now).TotalDays);
            return days > 0;
        }

        public bool IsValidSubscription(ISubscription s)
        {
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((s.ExpirationDate - DateTime.Now).TotalDays);
            return days > 0;
        }
        /// <summary>
        /// Returns the channel expirydate for the given productcode, channelcode, and path
        /// </summary>
        /// <param name="productcode"></param>
        /// <param name="channelCode"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public DateTime GetChannelExpirationDate(string productcode, string channelCode, string path)
        {
            if (string.IsNullOrEmpty(channelCode))
            {
                _logger.Warn($"Channel code is empty for" + path);
                return DateTime.MinValue;
            }
            if (!_subcriptions.Any(p => p.ProductCode.Equals(productcode)))
                return DateTime.MinValue;
            ISubscription currentPublication = _subcriptions.FirstOrDefault(p => p.ProductCode.Equals(productcode));
            if (!currentPublication.SubscribedChannels.Any(p => p.ChannelId.Equals(channelCode)))
                return DateTime.MinValue;
            return currentPublication.SubscribedChannels.FirstOrDefault(p => p.ChannelId.Equals(channelCode)).ExpirationDate;

        }

        public string OffSiteRenewLink => GlassModel.Off_Site_Renew_Link?.Url ?? "#";
        public string SubscriptionHeaderSubTitle => GlassModel.Body;
	    public string SubjectType => TextTranslator.Translate("Subscriptions.SubjectType");
        public string Subscribe => TextTranslator.Translate("Subscriptions.Subscribe");
        public string Subscribed => TextTranslator.Translate("Subscriptions.Subscribed");
        public string Renew => TextTranslator.Translate("Subscriptions.Renew");
        public string OffSiteSubscriptionLink => GlassModel.Off_Site_Subscription_Link?.Url ?? "#";
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public string Title => TextTranslator.Translate("Subscriptions.Title");
		public string PublicationText => TextTranslator.Translate("Subscriptions.Publication");
		public string SubscriptionTypeText => TextTranslator.Translate("Subscriptions.SubscriptionType");
		public string ExpirationDateText => TextTranslator.Translate("Subscriptions.ExpirationDate");
		public string ActionText => TextTranslator.Translate("Subscriptions.Action");
        //public string BottomNotation => GlassModel.Bottom_Notation;
    }
}

