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
        private readonly IUserSubscriptionsContext UserSubcriptions;
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
            UserSubcriptions = userSubscriptionsContext;
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
        public Navigation Preferencelst { get; set; }
        public bool IsGlobalToggleEnabled => SiterootContext.Item.Enable_MyView_Toggle;
        public Navigation MyViewPreferences => GetUserPreferences();
        #endregion

        /// <summary>
        /// IPMP:283 Get NAvigation URL for My View Items
        /// </summary>
        /// <returns>friendly URL</returns>
        private string GetNavigationUrl()
        {
            if (IsAuthenticated && IsGlobalToggleEnabled)
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
        /// IPMP:283 Method to get the logged in user preferences
        /// </summary>
        /// <returns>list of user preferences</returns>        
        public Navigation GetUserPreferences()
        {
            #region reading actual preferences
            var navigation = new Navigation();
            navigation.Children = new List<INavigation>();
            var preferredChannels = new List<Navigation>();
            IUserPreferences prefChannels = new UserPreferences();
            if (IsAuthenticated && IsGlobalToggleEnabled)
            {

                var channelPages = GlobalService.GetItem<IChannels_Page>(GlassModel?._Id.ToString()).
                _ChildrenWithInferType.OfType<IChannel_Page>();

                //channel based navigation
                if (UserPreferences != null && UserPreferences.Preferences != null &&
                UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count() > 0)
                {
                    if (UserPreferences.Preferences.PreferredChannels.FirstOrDefault().ChannelCode != SiterootContext.Item.Publication_Code)
                    {
                        foreach (var preference in UserPreferences.Preferences.PreferredChannels)
                        {
                            bool isTopicsFollowing = preference.Topics != null ? preference.Topics.Any(tp => tp.IsFollowing) : false;
                            string linkId = string.Empty;
                            string navigationLink = string.Empty;
                            if (!string.IsNullOrWhiteSpace(preference.ChannelCode) && (preference.IsFollowing || isTopicsFollowing))
                            {
                                var channelName = Navigation.SelectMany(p => p.Children.Where(n => n.Code == preference.ChannelCode).Select(q => q.Text)).FirstOrDefault();
                                if (!string.IsNullOrEmpty(channelName))
                                {
                                    linkId = isTopicsFollowing ? preference.Topics.FirstOrDefault(tp => tp.IsFollowing).TopicId : preference.ChannelId;
                                    navigationLink = string.Format("{0}/name={1}", SiterootContext.Item?.MyView_Page?._Url, linkId);
                                    preferredChannels.Add(new Navigation { Code = preference.ChannelCode, Text = channelName, Link = new Link { Url = navigationLink } });
                                }
                            }
                        }
                        navigation.Children = preferredChannels;
                    }
                    else
                    {
                        //topic based navigation
                        if (UserPreferences.Preferences.PreferredChannels != null
                            && UserPreferences.Preferences.PreferredChannels.SelectMany(n => n.Topics) != null
                            && UserPreferences.Preferences.PreferredChannels.SelectMany(n => n.Topics).Count() > 0)
                        {
                            string navigationLink = string.Empty;
                            foreach (var topic in UserPreferences.Preferences.PreferredChannels.SelectMany(n => n.Topics))
                            {
                                if (!string.IsNullOrWhiteSpace(topic.TopicCode) && topic.IsFollowing)
                                {
                                    var topicName = Navigation.SelectMany(p => p.Children.Where(n => n.Code == topic.TopicCode).Select(q => q.Text)).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(topicName))
                                    {
                                        navigationLink = string.Format("{0}/name={1}", SiterootContext.Item?.MyView_Page?._Url, topic.TopicId);
                                        preferredChannels.Add(new Navigation { Code = topic.TopicCode, Text = topicName, Link = new Link { Url = navigationLink } });
                                    }
                                }
                            }
                            navigation.Children = preferredChannels;
                        }
                    }
                    return navigation;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            #endregion
        }

        /// <summary>
        /// IPMP:283 Method to get the logged in user subscriptions
        /// </summary>
        /// <returns>list of subscriptions</returns>
        public IEnumerable<ISubscription> GetValidSubscriptions()
        {
            #region reading actual subscriptions
            var subscriptions = new List<ISubscription>();
            var channelSubscriptions = new List<ChannelSubscription>();
            var topicSubscriptions = new List<TopicSubscription>();
            if (IsAuthenticated && IsGlobalToggleEnabled)
            {

                //channel based subscriptions
                var currentPublication = SiterootContext.Item.Publication_Code;
                var userSubscriptions = UserSubcriptions?.Subscriptions?.Where(n => n.ProductCode == currentPublication);
                if (userSubscriptions != null)
                {
                    if (userSubscriptions.SelectMany(n => n.SubscribedChannels) != null && userSubscriptions.SelectMany(n => n.SubscribedChannels).Count() > 0)
                    {
                        foreach (var subscription in userSubscriptions.SelectMany(n => n.SubscribedChannels))
                        {

                            if (subscription != null && subscription.ExpirationDate > DateTime.Now)
                            {
                                channelSubscriptions.Add(subscription);
                            }
                        }
                        subscriptions.Add(new SalesforceSubscription { SubscribedChannels = channelSubscriptions });
                    }

                    //Topic based subscriptions
                    else if (userSubscriptions.SelectMany(n => n.SubscribedTopics) != null && userSubscriptions.SelectMany(n => n.SubscribedTopics).Count() > 0)
                    {
                        foreach (var subscription in userSubscriptions.SelectMany(n => n.SubscribedTopics))
                        {
                            if (subscription != null && subscription.ExpirationDate > DateTime.Now)
                            {
                                topicSubscriptions.Add(subscription);
                            }
                        }
                        subscriptions.Add(new SalesforceSubscription { SubscribedTopics = topicSubscriptions });
                    }
                    return subscriptions;
                }
                else
                {
                    return Enumerable.Empty<ISubscription>();
                }
            }
            else
            {
                return Enumerable.Empty<ISubscription>();
            }
            #endregion
        }
    }
}