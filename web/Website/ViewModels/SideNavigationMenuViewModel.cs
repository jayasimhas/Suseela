using System.Collections.Generic;
using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.User.Authentication;
using Informa.Library.User.UserPreference;
using System.Linq;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;
using System;
using Informa.Library.Salesforce.Subscription;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
    public class SideNavigationMenuViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly ISiteMainNavigationContext SiteMainNavigationContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUserPreferenceContext UserPreferences;
        private readonly IEnumerable<ISubscription> _subcriptions;
        public readonly ICallToActionViewModel CallToActionViewModel;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IGlobalSitecoreService GlobalService;

        public SideNavigationMenuViewModel(
            ISiteMainNavigationContext siteMainNavigationContext,
            ITextTranslator textTranslator,
            IAuthenticatedUserContext authenticatedUserContext,
            IUserPreferenceContext userPreferences,
            IUserSubscriptionsContext userSubscriptionsContext,
            ICallToActionViewModel callToActionViewModel,
            ISiteRootContext siterootContext,
            IGlobalSitecoreService globalService)
        {
            SiteMainNavigationContext = siteMainNavigationContext;
            TextTranslator = textTranslator;
            AuthenticatedUserContext = authenticatedUserContext;
            UserPreferences = userPreferences;
            _subcriptions = userSubscriptionsContext.Subscriptions;
            CallToActionViewModel = callToActionViewModel;
            SiterootContext = siterootContext;
            GlobalService = globalService;
        }

        #region Side Navigation Menu Items  
        public IEnumerable<INavigation> Navigation => SiteMainNavigationContext.Navigation;
        public IEnumerable<ISubscription> ValidSubscriptions => GetValidSubscriptions();
        public string MyViewLinkURL => GetNavigationUrl();
        public string MenuText => TextTranslator.Translate("MainNavigation.Menu");
        public string MenuButtonText => TextTranslator.Translate("MainNavigation.ToggleMenu");
        public string MyViewHelpText => TextTranslator.Translate("MainNavigation.MyViewHelpText");
        public string MyViewEditBtnText => TextTranslator.Translate("MainNavigation.MyViewEditButtonText");
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public IEnumerable<ISubscription> Subscriptions => _subcriptions;
        public Navigation Preferencelst { get; set; }
        public Navigation MyViewPreferences => GetUserPreferences();
        #endregion
        private string GetNavigationUrl()
        {
            if (IsAuthenticated)
            {
                if (UserPreferences.Preferences != null &&
                    UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count > 0)
                {
                    //Take user to Personalized home page.
                    return "/personal-home";
                }
                else
                {
                    //Take to MyView settings page
                    return SiterootContext.Item?.MyView_Settings_Page?._Url;
                }
            }
            else
            {
                return "/";
            }
        }
        /// <summary>
        /// Method to get the logged in user preferences
        /// </summary>
        /// <returns>list of user preferences</returns>
        public Navigation GetUserPreferences()
        {
            #region reading actual preferences
            //var navigation = new Navigation();
            //navigation.Children = new List<INavigation>();
            //var preferredChannels = new List<Navigation>();
            //IUserPreferences prefChannels = new UserPreferences();
            //if (IsAuthenticated)
            //{
            //    var channelPages = GlobalService.GetItem<IChannels_Page>(GlassModel?._Id.ToString()).
            //    _ChildrenWithInferType.OfType<IChannel_Page>();

            //    if (UserPreferences != null && UserPreferences.Preferences != null &&
            //    UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count > 0)
            //    {
            //        foreach (var preference in UserPreferences.Preferences.PreferredChannels)
            //        {
            //            //channel based preferences
            //            //check channel is followed
            //            if (preference.IsFollowing && string.IsNullOrWhiteSpace(preference.ChannelId) && string.IsNullOrWhiteSpace(preference.ChannelName))
            //            {
            //                preferredChannels.Add(new Navigation { Text = preference.ChannelName, Link = new Link { Url = channelPages.Where(m=>m.LinkableText== preference.ChannelName).Select(n=>n.LinkableUrl).ToString() } });
            //            }
            //            //check if any topic is followed
            //            else if (preference.Topics != null&&preference.Topics.Any(n=>n.IsFollowing) && string.IsNullOrWhiteSpace(preference.ChannelId) && string.IsNullOrWhiteSpace(preference.ChannelName))
            //            {
            //                preferredChannels.Add(new Navigation { Text = preference.ChannelName, Link = new Link { Url = channelPages.Where(m => m.LinkableText == preference.ChannelName).Select(n => n.LinkableUrl).ToString() } });
            //            }
            //            //topic based navigation
            //            else if(preference.Topics != null && preference.Topics.Any(n => n.IsFollowing) && !string.IsNullOrWhiteSpace(preference.ChannelId) && !string.IsNullOrWhiteSpace(preference.ChannelName))
            //            {
            //                foreach (var topic in preference.Topics)
            //                {
            //                    if (topic.IsFollowing && !string.IsNullOrWhiteSpace(topic.TopicId) && !string.IsNullOrWhiteSpace(topic.TopicName))
            //                    {
            //                        preferredChannels.Add(new Navigation { Text = topic.TopicName, Link = new Link { Url = channelPages.Where(m => m.LinkableText == topic.TopicName).Select(n => n.LinkableUrl).ToString() } });
            //                    }
            //                }
            //            }
            //        }
            //        navigation.Children = preferredChannels;
            //    }
            //    return navigation;
            //}
            //else
            //{
            //    return null;
            //}
            #endregion

            #region hardcoded data
            var tempObj = new Navigation();
            var children = new List<Navigation>();
            tempObj.Children = new List<INavigation>();

            tempObj.Text = "My View";
            tempObj.Link = new Link { Url = @"http://facebook.com" };

            var Prefdchannel = new UserPreferences();
            Prefdchannel.PreferredChannels = new List<Channel>();
            var channel = new Channel();
            channel.ChannelName = "Beverages";
            channel.ChannelOrder = 1;
            channel.ChannelLink = @"http://google.com";

            children.Add(new Navigation { Text = channel.ChannelName, Link = new Link { Url = channel.ChannelLink } });

            var channel1 = new Channel();
            channel1.ChannelName = "Cocoa";
            channel1.ChannelOrder = 2;
            channel1.ChannelLink = @"http://yahoo.com";

            children.Add(new Navigation { Text = channel1.ChannelName, Link = new Link { Url = channel1.ChannelLink } });

            tempObj.Children = children;

            return tempObj;
            #endregion
        }

        /// <summary>
        /// Method to get the logged in user subscriptions
        /// </summary>
        /// <returns>list of subscriptions</returns>
        public IEnumerable<ISubscription> GetValidSubscriptions()
        {
            #region reading actual subscriptions
            //var subscriptions = new List<ISubscription>();
            //if (IsAuthenticated)
            //{
            //    if (Subscriptions != null && Subscriptions.Count() > 0)
            //    {
            //        foreach (var subscription in Subscriptions)
            //        {
            //            if (subscription != null && subscription.ExpirationDate > DateTime.Now)
            //            {
            //                subscriptions.Add(subscription);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //do topic level subscriptions
            //    }
            //    return subscriptions;
            //}
            //else
            //{
            //    return Enumerable.Empty<ISubscription>();
            //}
            #endregion

            #region hardcoded data
            var subscriptions = new List<ISubscription>();
            ISubscription subscription = new SalesforceSubscription();
            subscription.Publication = "Frozen Foods";
            subscription.ExpirationDate = new DateTime(2016, 10, 25);
            subscriptions.Add(subscription);

            ISubscription subscription1 = new SalesforceSubscription();
            subscription1.Publication = "Dairy";
            subscription1.ExpirationDate = new DateTime(2016, 10, 25);
            subscriptions.Add(subscription1);

            return subscriptions;
            #endregion

            #region new code
            //var subscriptions = new List<ISubscription>();
            //IUserPreferences prefChannels = new UserPreferences();

            //return subscriptions;
            #endregion
        }
    }
}