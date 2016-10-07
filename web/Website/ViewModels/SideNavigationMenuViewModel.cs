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

namespace Informa.Web.ViewModels
{
    public class SideNavigationMenuViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly ISiteMainNavigationContext SiteMainNavigationContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUserPreferenceContext UserPreferences;
        private readonly IEnumerable<ISubscription> _subcriptions;

        public SideNavigationMenuViewModel(
            ISiteMainNavigationContext siteMainNavigationContext,
            ITextTranslator textTranslator,
            IAuthenticatedUserContext authenticatedUserContext,
            IUserPreferenceContext userPreferences,
            IUserSubscriptionsContext userSubscriptionsContext)
        {
            SiteMainNavigationContext = siteMainNavigationContext;
            TextTranslator = textTranslator;
            AuthenticatedUserContext = authenticatedUserContext;
            UserPreferences = userPreferences;
            _subcriptions = userSubscriptionsContext.Subscriptions;
        }

        #region Side Navigation Menu Items  
        public IEnumerable<INavigation> Navigation => SiteMainNavigationContext.Navigation;
        public IEnumerable<ISubscription> NavigationWithSubs => GetNavigationWithSubScriptions();
        public string MenuText => TextTranslator.Translate("MainNavigation.Menu");
        public string MenuButtonText => TextTranslator.Translate("MainNavigation.ToggleMenu");
        public string MyViewHelpText => TextTranslator.Translate("MainNavigation.MyViewHelpText");
        public string MyViewEditBtnText => TextTranslator.Translate("MainNavigation.MyViewEditButtonText");
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public IEnumerable<ISubscription> Subscriptions => _subcriptions;
        public Navigation Preferencelst { get; set; }
        public Navigation MyViewPreferences => GetUserPreferences();
        #endregion

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
            if (IsAuthenticated)
            {
                if (UserPreferences.Preferences != null &&
                UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count > 0)
                {
                    foreach (var preference in UserPreferences.Preferences.PreferredChannels)
                    {
                        //preferredChannels.Add(new Navigation { Text = preference.Channel.ChannelName, Link = new Link { Url = preference.Channel.ChannelLink } });
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

            #region hardcoded data
            //var tempObj = new Navigation();
            //var children = new List<Navigation>();
            //tempObj.Children = new List<INavigation>();

            //tempObj.Text = "My View";
            //tempObj.Link = new Link { Url = @"http://facebook.com" };

            //var Prefdchannel = new ChannelPreference();
            //Prefdchannel.Channel = new Channel();
            //Prefdchannel.Channel.ChannelName = "Beverages";
            //Prefdchannel.Channel.ChannelOrder = 1;
            //Prefdchannel.Channel.ChannelLink = @"http://google.com";

            //children.Add(new Navigation { Text = Prefdchannel.Channel.ChannelName, Link = new Link { Url = Prefdchannel.Channel.ChannelLink } });

            //Prefdchannel.Channel = new Channel();
            //Prefdchannel.Channel.ChannelName = "Cocoa";
            //Prefdchannel.Channel.ChannelOrder = 2;
            //Prefdchannel.Channel.ChannelLink = @"http://yahoo.com";

            //children.Add(new Navigation { Text = Prefdchannel.Channel.ChannelName, Link = new Link { Url = Prefdchannel.Channel.ChannelLink } });

            //tempObj.Children = children;

            //return tempObj;
            #endregion
        }

        /// <summary>
        /// Method to get the logged in user subscriptions
        /// </summary>
        /// <returns>list of subscriptions</returns>
        public IEnumerable<ISubscription> GetNavigationWithSubScriptions()
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

            ISubscription subscription2 = new SalesforceSubscription();
            subscription2.Publication = "People";
            subscription2.ExpirationDate = new DateTime(2016, 10, 25);
            subscriptions.Add(subscription2);
            return subscriptions;
            #endregion
        }
    }
}