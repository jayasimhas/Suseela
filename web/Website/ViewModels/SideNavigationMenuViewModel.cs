﻿using System.Collections.Generic;
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
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;

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

                //Take to MyView settings page
                return SiterootContext.Item?.MyView_Settings_Page?._Url;
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
                var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
                 _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();

                if (homeItem != null)
                {
                    var channelsPageItem = homeItem._ChildrenWithInferType.OfType<IChannels_Page>().FirstOrDefault();

                    if (channelsPageItem != null)
                    {
                        var channelPages = channelsPageItem._ChildrenWithInferType.OfType<IChannel_Page>();
                        if (channelPages != null && channelPages.Any())
                        {
                            //channel based navigation
                            if (UserPreferences != null && UserPreferences.Preferences != null &&
                            UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count() > 0)
                            {
                                if (UserPreferences.Preferences.PreferredChannels.FirstOrDefault().ChannelCode != SiterootContext.Item.Publication_Code)
                                {
                                    foreach (var preference in UserPreferences.Preferences.PreferredChannels.OrderBy(channel => channel.ChannelOrder).ToList())
                                    {
                                        bool isTopicsFollowing = preference.Topics != null ? preference.Topics.Any(tp => tp.IsFollowing) : false;

                                        if (!string.IsNullOrWhiteSpace(preference.ChannelCode) && ((preference.IsFollowing && UserPreferences.Preferences.IsNewUser) || isTopicsFollowing))
                                        {
                                            var channel = channelPages.Where(p => p.Channel_Code == preference.ChannelCode).FirstOrDefault();
                                            if (channel != null && !string.IsNullOrEmpty(channel.Display_Text))
                                            {
                                                preferredChannels.Add(new Navigation { Code = preference.ChannelCode, Text = channel.Display_Text, Link = new Link { Url = SiterootContext.Item?.MyView_Page?._Url, TargetId = new Guid(preference.ChannelId) } });
                                            }
                                        }
                                    }
                                    navigation.Children = preferredChannels;
                                }
                                else
                                {
                                    //topic based navigation
                                    var channelPage = channelsPageItem._ChildrenWithInferType.OfType<IChannel_Page>().FirstOrDefault();
                                    var pageAssetsItem = channelPage._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();
                                    if (pageAssetsItem != null)
                                    {
                                        var topics = pageAssetsItem._ChildrenWithInferType.
                                            OfType<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>();
                                        if (topics != null && topics.Any())
                                        {
                                            if (UserPreferences.Preferences.PreferredChannels != null
                                        && UserPreferences.Preferences.PreferredChannels.SelectMany(n => n.Topics) != null
                                        && UserPreferences.Preferences.PreferredChannels.SelectMany(n => n.Topics).Count() > 0)
                                            {
                                                string navigationLink = string.Empty;
                                                foreach (var topic in UserPreferences.Preferences.PreferredChannels.SelectMany(n => n.Topics).OrderBy(topic => topic.TopicOrder).ToList())
                                                {
                                                    if (!string.IsNullOrWhiteSpace(topic.TopicCode) && topic.IsFollowing)
                                                    {
                                                        var topicObj = topics.Where(p => p.Navigation_Code == topic.TopicCode).FirstOrDefault();
                                                        if (topicObj != null && !string.IsNullOrEmpty(topicObj.Title))
                                                        {
                                                            preferredChannels.Add(new Navigation { Code = topic.TopicCode, Text = topicObj.Title, Link = new Link { Url = SiterootContext.Item?.MyView_Page?._Url, TargetId = new Guid(topic.TopicId) } });
                                                        }
                                                    }
                                                }
                                                navigation.Children = preferredChannels;
                                            }
                                        }
                                    }
                                }
                                return navigation;
                            }
                        }
                    }
                }
            }
            return null;
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
            if (IsAuthenticated && IsGlobalToggleEnabled)
            {

                //channel based subscriptions
                var currentPublication = SiterootContext.Item.Publication_Code;
                var userSubscriptions = UserSubcriptions?.Subscriptions?.Where(n => n.ProductCode == currentPublication).FirstOrDefault();
                if (userSubscriptions != null && userSubscriptions.SubscribedChannels != null && userSubscriptions.SubscribedChannels.Count() > 0)
                {

                    if (userSubscriptions.SubscribedChannels.FirstOrDefault().ChannelId != SiterootContext.Item.Publication_Code)
                    {
                        foreach (var subscription in userSubscriptions.SubscribedChannels)
                        {
                            bool isTopicsSubscribed = subscription.SubscribedTopics != null ? subscription.SubscribedTopics.Any(tp => tp.ExpirationDate > DateTime.Now) : false;
                            if (subscription != null && (subscription.ExpirationDate > DateTime.Now || isTopicsSubscribed))
                            {
                                channelSubscriptions.Add(subscription);
                            }
                        }
                        subscriptions.Add(new SalesforceSubscription { SubscribedChannels = channelSubscriptions, IsTopicSubscription = false });
                    }

                    //Topic based subscriptions
                    else
                    {
                        if (userSubscriptions.SubscribedChannels != null
                            && userSubscriptions.SubscribedChannels.SelectMany(n => n.SubscribedTopics) != null
                            && userSubscriptions.SubscribedChannels.SelectMany(n => n.SubscribedTopics).Count() > 0)
                            foreach (var subscription in userSubscriptions.SubscribedChannels.SelectMany(n => n.SubscribedTopics))
                            {
                                if (!string.IsNullOrWhiteSpace(subscription.TopicId) && subscription.ExpirationDate > DateTime.Now)
                                {
                                    channelSubscriptions.Add(new ChannelSubscription { ChannelId = subscription.TopicId, ChannelName = subscription.TopicName, ExpirationDate = subscription.ExpirationDate, _ChannelId = subscription.TopicId });
                                }
                            }
                        subscriptions.Add(new SalesforceSubscription { SubscribedChannels = channelSubscriptions, IsTopicSubscription = true });
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
    
        public string CurrentItemId => GlassModel?._Id.ToString();
	}

}
