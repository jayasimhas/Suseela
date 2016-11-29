namespace Informa.Web.Areas.Account.ViewModels.Personalization
{
    using Informa.Library.Globalization;
    using Informa.Library.Services.Global;
    using Informa.Library.Site;
    using Informa.Library.Subscription;
    using Informa.Library.Subscription.User;
    using Informa.Library.User.UserPreference;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
    using Jabberwocky.Autofac.Attributes;
    using Sitecore.Data.Items;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [AutowireService]
    public class ChannelsViewModel : IChannelsViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly IUserPreferenceContext UserPreferences;
        private readonly IEnumerable<ISubscription> _subcriptions;
        protected readonly ISiteRootContext SiterootContext;

        public ChannelsViewModel(
        ITextTranslator translator,
        ISiteRootContext siterootContext,
        IGlobalSitecoreService globalService,
        IUserPreferenceContext userPreferences,
        IUserSubscriptionsContext userSubscriptionsContext)
        {
            TextTranslator = translator;
            SiterootContext = siterootContext;
            GlobalService = globalService;
            UserPreferences = userPreferences;
            _subcriptions = userSubscriptionsContext.Subscriptions;
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
        /// Gets a value indicating whether this instance is new user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is new user; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewUser => UserPreferences.Preferences == null || UserPreferences.Preferences.IsNewUser;

        /// <summary>
        /// Gets the current subscriptions.
        /// </summary>
        /// <value>
        /// The current subscriptions.
        /// </value>
        public ISubscription currentSubscriptions
        {
            get
            {
                return _subcriptions.Where(n => n.ProductCode == SiterootContext?.Item.Publication_Code).FirstOrDefault();
            }
        }

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
                return GetAllChannels();
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

            if (homeItem != null)
            {
                Channel channel = null;
                channel = new Channel();
                channel.ChannelId = homeItem._Id.ToString();
                channel.ChannelName = homeItem._Parent._Name;
                channel.ChannelCode = SiterootContext.Item.Publication_Code;
                channel.ChannelLink = homeItem._Url;
                channel.ChannelOrder = 1;
                channel.IsSubscribed = currentSubscriptions?.ProductCode == channel.ChannelCode && currentSubscriptions.ExpirationDate > DateTime.Now;
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
                                topic.IsSubscribed = currentSubscriptions?.SubscribedChannels?.Any(ch => ch.ChannelId == channelPage.Channel_Code && ch.ExpirationDate > DateTime.Now) ?? false;
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
                                    topic.IsSubscribed = currentSubscriptions?.SubscribedChannels?.Any(tp=>tp.SubscribedTopics.Any(sub=>sub.TopicId==channelPage.Channel_Code && sub.ExpirationDate>DateTime.Now))?? false;
                                    topic.IsFollowing = IsNewUser ? IsNewUser : topic.TopicOrder > 0;
                                    channel.Topics.Add(topic);
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Gets all channels.
        /// </summary>
        /// <returns>List of channels</returns>
        private IList<Channel> GetAllChannels()
        {
            var channels = new List<Channel>();

            var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
                _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();

            if (homeItem != null)
            {
                var channelsPageItem = homeItem._ChildrenWithInferType.OfType<IChannels_Page>().FirstOrDefault();

                if (channelsPageItem != null)
                {
                    var channelPages = channelsPageItem._ChildrenWithInferType.OfType<IChannel_Page>();
                    isChannelBasedRegistration = channelPages.Count() > 1 && !string.Equals(channelPages.FirstOrDefault().Channel_Code, SiterootContext.Item.Publication_Code, StringComparison.OrdinalIgnoreCase);
                    if (channelPages != null && channelPages.Any())
                    {
                        Channel channel = null;
                        foreach (IChannel_Page channelPage in channelPages)
                        {
                            channel = new Channel();
                            channel.ChannelId = channelPage._Id.ToString();
                            channel.ChannelName = string.IsNullOrWhiteSpace(channelPage.Display_Text) ? channelPage.Title : channelPage.Display_Text;
                            channel.ChannelCode = string.IsNullOrWhiteSpace(channelPage.Channel_Code) ? channelPage.Title : channelPage.Channel_Code;
                            channel.ChannelLink = channelPage.LinkableUrl;
                            SetChannelOrderAndStatus(channelPage, channel);
                            channel.IsSubscribed = currentSubscriptions?.SubscribedChannels?.Any(ch => ch.ChannelId == channelPage.Channel_Code && ch.ExpirationDate > DateTime.Now) ?? false;
                            GetTopics(channel, channelPage);
                            channels.Add(channel);
                        }
                    }
                }
            }

            return channels;
        }

        /// <summary>
        /// Sets the channel order and status.
        /// </summary>
        /// <param name="channelPage">The channel page.</param>
        /// <param name="channel">The channel.</param>
        private void SetChannelOrderAndStatus(IChannel_Page channelPage, Channel channel)
        {
            if (UserPreferences.Preferences != null && UserPreferences.Preferences.PreferredChannels != null
                && UserPreferences.Preferences.PreferredChannels.Any())
            {
                var preferredChannel = UserPreferences.Preferences.PreferredChannels.
                    Where(ch => ch.ChannelCode.Equals(channelPage.Channel_Code.ToString(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (preferredChannel != null)
                {
                    channel.ChannelOrder = preferredChannel.ChannelOrder;
                    channel.IsFollowing = preferredChannel.IsFollowing;
                }
            }
        }

        /// <summary>
        /// Gets the topics.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="channelPage">The channel page.</param>
        private void GetTopics(Channel channel, IChannel_Page channelPage)
        {
            channel.Topics = new List<Topic>();

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
                        GetTopicOrderAndStatus(channel, topicItem, topic);
                        channel.Topics.Add(topic);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the topic order and status.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="topicPage">The topic page.</param>
        /// <param name="topic">The topic.</param>
        private void GetTopicOrderAndStatus(Channel channel, Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topicPage, Topic topic)
        {
            if (UserPreferences.Preferences != null && UserPreferences.Preferences.PreferredChannels != null
                && UserPreferences.Preferences.PreferredChannels.Any())
            {
                var preferredChannel = UserPreferences.Preferences.PreferredChannels.
                    Where(ch => ch.ChannelCode.Equals(channel.ChannelCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (preferredChannel != null && preferredChannel.Topics != null && preferredChannel.Topics.Any())
                {
                    var preferredTopic = preferredChannel.Topics.Where(t => t.TopicCode.Equals(topicPage.Navigation_Code.ToString(),
                        StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (preferredTopic != null)
                    {
                        topic.TopicOrder = preferredTopic.TopicOrder;
                        topic.IsFollowing = IsNewUser ? channel.IsFollowing : preferredTopic.IsFollowing;
                    }
                }
                if (channel.IsFollowing && UserPreferences.Preferences.IsNewUser)
                {
                    topic.IsFollowing = true;
                }
            }
        }
    }
}