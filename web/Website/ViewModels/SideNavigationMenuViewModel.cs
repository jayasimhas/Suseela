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
        public bool IsGlobalToggleEnabled => SiterootContext.Item.Enable_MyView_Toggle;
        public Navigation Preferencelst { get; set; }
        public Navigation MyViewPreferences => GetUserPreferences();
        #endregion

        /// <summary>
        /// Get NAvigation URL for My View Items
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
        /// Method to get the logged in user preferences
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
                UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count > 0)
                {
                    
                    foreach (var preference in UserPreferences.Preferences.PreferredChannels)
                    {
                        
                        if (!string.IsNullOrWhiteSpace(preference.ChannelCode))
                        {
                            var channelName = Navigation.SelectMany(p => p.Children.Where(n => n.Code == preference.ChannelCode).Select(q => q.Text)).FirstOrDefault();
                            if(!string.IsNullOrEmpty(channelName))
                            preferredChannels.Add(new Navigation { Code = preference.ChannelCode, Text = channelName, Link = new Link { Url = channelPages.Where(m => m.LinkableText == preference.ChannelName).Select(n => n.LinkableUrl).ToString() } });
                        }
                    }
                    navigation.Children = preferredChannels;
                }
                //Topic based navigation
                else if (UserPreferences != null && UserPreferences.Preferences != null &&
                UserPreferences.Preferences.PreferredTopics != null && UserPreferences.Preferences.PreferredTopics.Count > 0)
                {
                    foreach (var preference in UserPreferences.Preferences.PreferredTopics)
                    {
                        if (!string.IsNullOrWhiteSpace(preference.TopicCode) && !string.IsNullOrWhiteSpace(preference.TopicName))
                        {
                            var topicName = Navigation.SelectMany(p => p.Children.Where(n => n.Code == preference.TopicCode).Select(q => q.Text)).FirstOrDefault();
                            if (!string.IsNullOrEmpty(topicName))
                                preferredChannels.Add(new Navigation { Code = preference.TopicCode, Text = topicName, Link = new Link { Url = channelPages.Where(m => m.LinkableText == preference.TopicName).Select(n => n.LinkableUrl).ToString() } });
                        }
                    }
                    navigation.Children = preferredChannels;
                }
                return navigation;
            }
            else
            {
                return null;
            }

            #endregion
        }

        /// <summary>
        /// Method to get the logged in user subscriptions
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
                if (UserSubcriptions != null && UserSubcriptions.Subscriptions != null && UserSubcriptions.Subscriptions.SelectMany(n => n.SubscribedChannels).ToList() != null && UserSubcriptions.Subscriptions.SelectMany(n => n.SubscribedChannels).Count() > 0)
                {
                    foreach (var subscription in UserSubcriptions.Subscriptions.SelectMany(n => n.SubscribedChannels))
                    {
                        if (subscription != null && subscription.ExpirationDate > DateTime.Now)
                        {
                            channelSubscriptions.Add(subscription);
                        }
                    }
                    subscriptions.Add(new SalesforceSubscription { SubscribedChannels = channelSubscriptions });
                }

                //Topic based subscriptions
                else if (UserSubcriptions != null && UserSubcriptions.Subscriptions != null && UserSubcriptions.Subscriptions.SelectMany(n => n.SubscribedTopics).ToList() != null && UserSubcriptions.Subscriptions.SelectMany(n => n.SubscribedTopics).Count() > 0)
                {
                    foreach (var subscription in UserSubcriptions.Subscriptions.SelectMany(n => n.SubscribedTopics))
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
            #endregion
        }
    }
}