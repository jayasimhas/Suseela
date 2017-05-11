namespace Informa.Web.Areas.Account.ViewModels.Personalization
{
    using Library.Globalization;
    using Library.Services.Global;
    using Library.Site;
    using Library.Subscription;
    using Library.Subscription.User;
    using Library.User.UserPreference;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
    using Jabberwocky.Autofac.Attributes;
    using Sitecore.Data.Items;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Library.Utilities.References;
    using Web.Models;

    [AutowireService]
    public class ChannelsViewModel : IChannelsViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly IUserPreferenceContext UserPreferences;
        private readonly IEnumerable<ISubscription> _subcriptions;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IItemReferences ItemReferences;
        protected readonly PreferencesUtil PreferencesUtil;

        public ChannelsViewModel(
        ITextTranslator translator,
        ISiteRootContext siterootContext,
        IGlobalSitecoreService globalService,
        IUserPreferenceContext userPreferences,
        IUserSubscriptionsContext userSubscriptionsContext,
                IItemReferences itemReferences,
                PreferencesUtil preferencesUtil)
        {
            TextTranslator = translator;
            SiterootContext = siterootContext;
            GlobalService = globalService;
            UserPreferences = userPreferences;
            _subcriptions = userSubscriptionsContext.Subscriptions;
            ItemReferences = itemReferences;
            PreferencesUtil = preferencesUtil;
        }

        public string ChannelFollowingButtonText => TextTranslator.Translate("MyViewSettings.ChannelFollowingButtonText");

        public string ChannelFollowButtonText => TextTranslator.Translate("MyViewSettings.ChannelFollowButtonText");

        /// <summary>
        /// Gets the topic following button text.
        /// </summary>
        /// <value>
        /// The topic following button text.
        /// </value>
        public string TopicFollowingButtonText => TextTranslator.Translate("MyViewSettings.TopicFollowingButtonText");

        /// <summary>
        /// Gets the topic follow button text.
        /// </summary>
        /// <value>
        /// The topic follow button text.
        /// </value>
        public string TopicFollowButtonText => TextTranslator.Translate("MyViewSettings.TopicFollowButtonText");

        /// <summary>
        /// Gets the follow all button text.
        /// </summary>
        /// <value>
        /// The follow all button text.
        /// </value>
        public string FollowAllButtonText => TextTranslator.Translate("MyViewSettings.FollowAllButtonText");

        /// <summary>
        /// Gets the unfollow all button text.
        /// </summary>
        /// <value>
        /// The unfollow all button text.
        /// </value>
        public string UnfollowAllButtonText => TextTranslator.Translate("MyViewSettings.UnfollowAllButtonText");

        /// <summary>
        /// Gets the subscribe button text.
        /// </summary>
        /// <value>
        /// The subscribe button text.
        /// </value>
        public string SubscribeButtonText => TextTranslator.Translate("MyViewSettings.SubscribeButtonText");

        /// <summary>
        /// Gets the subscribed button text.
        /// </summary>
        /// <value>
        /// The subscribed button text.
        /// </value>
        public string SubscribedButtonText => TextTranslator.Translate("MyViewSettings.SubscribedButtonText");

        /// <summary>
        /// Gets the move lable text.
        /// </summary>
        /// <value>
        /// The move lable text.
        /// </value>
        public string MoveLableText => TextTranslator.Translate("MyViewSettings.MoveLableText");

        /// <summary>
        /// Gets the pick and choose lable text.
        /// </summary>
        /// <value>
        /// The pick and choose lable text.
        /// </value>
        public string PickAndChooseLableText => TextTranslator.Translate("MyViewSettings.PickAndChooseLableText");

        /// <summary>
        /// Gets the subscribe message text.
        /// </summary>
        /// <value>
        /// The subscribe message text.
        /// </value>
        public string SubscribeMessageText => TextTranslator.Translate("MyViewSettings.SubscribeMessageText");

        /// <summary>
        /// Gets the pick and choose lable mobile text.
        /// </summary>
        /// <value>
        /// The pick and choose lable mobile text.
        /// </value>
        public string PickAndChooseLableMobileText => TextTranslator.Translate("MyViewSettings.PickAndChooseLableMobileText");

        /// <summary>
        /// Gets the subscribe URL.
        /// </summary>
        /// <value>
        /// The subscribe URL.
        /// </value>
        public string SubscribeUrl => SiterootContext.Item?.Subscribe_Link?.Url;

        /// <summary>
        /// Gets the publicationname.
        /// </summary>
        /// <value>
        /// The publicationname.
        /// </value>
        public string publicationname => SiterootContext.Item.Publication_Name;

        /// <summary>
        /// Gets the subscribed message text.
        /// </summary>
        /// <value>
        /// The subscribed message text.
        /// </value>
        public string SubscribedMessageText => TextTranslator.Translate("Registration.SubscribedMessageText");

        /// <summary>
        /// Gets or sets a value indicating whether this instance is channel based registration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is channel based registration; otherwise, <c>false</c>.
        /// </value>
        public bool isChannelBasedRegistration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is from registration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is from registration; otherwise, <c>false</c>.
        /// </value>
        public bool isFromRegistration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable save preferences check].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable save preferences check]; otherwise, <c>false</c>.
        /// </value>
        public bool enableSavePreferencesCheck { get; set; }

        /// <summary>
        /// Gets the channels.
        /// </summary>
        /// <value>
        /// The channels.
        /// </value>
        public IList<Channel> Channels => GetChannels();


        /// <summary>
        /// Gets a value indicating whether [show subscription status].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show subscription status]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSubscriptionStatus => SiterootContext.Item.Entitlement_Type._Id.Equals(ItemReferences.ChannelLevelEntitlementType);

        /// <summary>
        /// Gets a value indicating whether this instance is new user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is new user; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewUser => UserPreferences.Preferences == null || UserPreferences.Preferences.IsNewUser;

        /// <summary>
        /// Gets the channels.
        /// </summary>
        /// <returns>List of channels</returns>
        private IList<Channel> GetChannels()
        {

            Item CurrentItem = Sitecore.Context.Item;
            isFromRegistration = string.IsNullOrEmpty(CurrentItem["IsFromRegistration"]) ? false : true;
            enableSavePreferencesCheck = string.IsNullOrEmpty(CurrentItem["EnableSavePreferencesCheck"]) ? false : true;
            if (isFromRegistration)
            {
                return GetPublicationDetailsForRegistration();
            }
            else
            {
                var channels = PreferencesUtil.GetAllChannels();
                isChannelBasedRegistration = PreferencesUtil.IsChannelBasedRegistration;
                return channels;
            }
        }
        /// <summary>
        /// IPMP-752 :Content Customization during registration: Get Publication details
        /// </summary>
        /// <returns>Publication and Channels details</returns>
        private IList<Channel> GetPublicationDetailsForRegistration()
        {
            var channels = new List<Channel>();

            var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
                _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();
            var channelsPageItem = homeItem._ChildrenWithInferType.OfType<IChannels_Page>().FirstOrDefault();
            var channelPage = channelsPageItem?._ChildrenWithInferType.OfType<IChannel_Page>()?.FirstOrDefault();
            if (homeItem != null)
            {
                Channel channel = null;
                channel = new Channel();
                channel.ChannelName = homeItem._Parent._Name;
                channel.ChannelCode = channelPage != null ? channelPage.Channel_Code : SiterootContext.Item.Publication_Code;
                channel.ChannelLink = homeItem._Url;
                channel.ChannelOrder = 1;
                channel.IsSubscribed = _subcriptions != null && _subcriptions.Any(subcription => subcription
                 .ProductCode.Equals(PreferencesUtil.GetProductCode(channel.ChannelCode), StringComparison.OrdinalIgnoreCase) &&
                 subcription.ExpirationDate > DateTime.Now);
                GetTopicsForRegistration(channel);
                channels.Add(channel);
            }
            return channels;
        }
        /// <summary>
        /// IPMP-752 :Content Customization during registration: Get Topic details
        /// </summary>
        /// <param name="channel">Channel object</param>
        private void GetTopicsForRegistration(Channel channel)
        {
            channel.Topics = new List<Topic>();

            var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
                 _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();

            if (homeItem != null)
            {
                var channelsPageItem = homeItem._ChildrenWithInferType.OfType<IChannels_Page>().FirstOrDefault();

                if (channelsPageItem != null)
                {
                    var channelPages = channelsPageItem._ChildrenWithInferType.OfType<IChannel_Page>();

                    //channel based content customization registration
                    if (channelPages.Count() > 1 && !string.Equals(channelPages.FirstOrDefault().Channel_Code, SiterootContext.Item.Publication_Code, StringComparison.OrdinalIgnoreCase))
                    {
                        isChannelBasedRegistration = true;
                        if (channelPages != null && channelPages.Any())
                        {
                            Topic topic = null;
                            foreach (IChannel_Page channelPage in channelPages)
                            {
                                topic = new Topic();
                                topic.TopicId = channelPage._Id.ToString();
                                topic.TopicName = string.IsNullOrWhiteSpace(channelPage.Display_Text) ? channelPage.Title : channelPage.Display_Text;
                                topic.TopicCode = string.IsNullOrWhiteSpace(channelPage.Channel_Code) ? channelPage.Title : channelPage.Channel_Code;
                                topic.IsSubscribed = _subcriptions != null && _subcriptions.Any(subcription => subcription
                                                    .ProductCode.Equals(PreferencesUtil.GetProductCode(topic.TopicCode), StringComparison.OrdinalIgnoreCase) &&
                                                     subcription.ExpirationDate > DateTime.Now);
                                channel.Topics.Add(topic);
                            }
                        }
                    }
                    else
                    {
                        //topic based content customization registration
                        isChannelBasedRegistration = false;
                        var channelPage = channelsPageItem._ChildrenWithInferType.OfType<IChannel_Page>().FirstOrDefault();
                        var pageAssetsItem = channelPage._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();
                        if (pageAssetsItem != null)
                        {
                            var topics = pageAssetsItem._ChildrenWithInferType.
                                OfType<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>();
                            if (topics != null && topics.Any())
                            {
                                Topic topic = null;
                                foreach (Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic
                                    topicItem in topics)
                                {
                                    topic = new Topic();
                                    topic.TopicId = topicItem._Id.ToString();
                                    topic.TopicName = string.IsNullOrWhiteSpace(topicItem.Navigation_Text) ? topicItem.Title : topicItem.Navigation_Text;
                                    topic.TopicCode = topicItem.Navigation_Code;
                                    topic.IsSubscribed = _subcriptions != null && _subcriptions.Any(subcription => subcription
                                                   .ProductCode.Equals(PreferencesUtil.GetProductCode(topic.TopicCode), StringComparison.OrdinalIgnoreCase) &&
                                                    subcription.ExpirationDate > DateTime.Now);
                                    channel.Topics.Add(topic);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}