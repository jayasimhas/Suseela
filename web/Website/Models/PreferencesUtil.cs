using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;
using Informa.Library.User.UserPreference;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Informa.Web.Models
{
    public class PreferencesUtil
    {
        private string channelCodeFormat = "{0}.{1}";
        protected readonly ITextTranslator TextTranslator;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly IUserPreferenceContext UserPreferences;
        private readonly IEnumerable<ISubscription> _subcriptions;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IItemReferences ItemReferences;
        private bool _isChannelBasedRegistration;

        public PreferencesUtil(
        ITextTranslator translator,
        ISiteRootContext siterootContext,
        IGlobalSitecoreService globalService,
        IUserPreferenceContext userPreferences,
        IUserSubscriptionsContext userSubscriptionsContext,
                IItemReferences itemReferences)
        {
            TextTranslator = translator;
            SiterootContext = siterootContext;
            GlobalService = globalService;
            UserPreferences = userPreferences;
            _subcriptions = userSubscriptionsContext.Subscriptions;
            ItemReferences = itemReferences;
        }
        /// <summary>
        /// Gets a value indicating whether this instance is new user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is new user; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewUser => UserPreferences.Preferences == null || UserPreferences.Preferences.IsNewUser;
        public bool IsChannelBasedRegistration
        {
            get
            {
                return _isChannelBasedRegistration;
            }

            set
            {
                _isChannelBasedRegistration = value;
            }
        }

        /// <summary>
        /// Gets the preference identifier.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <returns></returns>
        public string GetPreferenceId(string itemKey)
        {
            if (string.IsNullOrWhiteSpace(itemKey))
                return string.Empty;
            return Sitecore.Configuration.Settings.GetSetting(itemKey); 
        }

        /// <summary>
        /// Gets the product code.
        /// </summary>
        /// <param name="subCode">The sub code</param>
        /// <returns> The product code</returns>
        public string GetProductCode(string subCode)
        {
            return string.Format(channelCodeFormat, SiterootContext?.Item.Publication_Code, subCode);
        }

        /// <summary>
        /// Gets all channels.
        /// </summary>
        /// <returns>List of channels</returns>
        public IList<Channel> GetAllChannels(bool checkStatus = true)
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
                    _isChannelBasedRegistration = channelPages.Count() > 1 && !string.Equals(channelPages.FirstOrDefault().Channel_Code, SiterootContext.Item.Publication_Code, StringComparison.OrdinalIgnoreCase);
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
                            if (checkStatus)
                            {
                                SetChannelOrderAndStatus(channelPage, channel);
                                channel.IsSubscribed = _subcriptions != null && _subcriptions.Any(subcription => subcription
                                                       .ProductCode.Equals(GetProductCode(channel.ChannelCode), StringComparison.OrdinalIgnoreCase) &&
                                                        subcription.ExpirationDate > DateTime.Now);
                            }
                            else
                                channel.IsFollowing = true;
                            GetTopics(channel, channelPage, checkStatus);
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
        private void GetTopics(Channel channel, IChannel_Page channelPage, bool checkStatus = true)
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
                        if (checkStatus)
                            GetTopicOrderAndStatus(channel, topicItem, topic);
                        else
                            topic.IsFollowing = true;
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
                        topic.IsFollowing = IsNewUser && IsChannelBasedRegistration ? channel.IsFollowing : preferredTopic.IsFollowing;
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