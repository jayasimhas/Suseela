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

        public string TopicFollowingButtonText => TextTranslator.Translate("MyViewSettings.TopicFollowingButtonText");

        public string TopicFollowButtonText => TextTranslator.Translate("MyViewSettings.TopicFollowButtonText");

        public string FollowAllButtonText => TextTranslator.Translate("MyViewSettings.FollowAllButtonText");

        public string UnfollowAllButtonText => TextTranslator.Translate("MyViewSettings.UnfollowAllButtonText");

        public string SubscribeButtonText => TextTranslator.Translate("MyViewSettings.SubscribeButtonText");

        public string SubscribedButtonText => TextTranslator.Translate("MyViewSettings.SubscribedButtonText");

        public string MoveLableText => TextTranslator.Translate("MyViewSettings.MoveLableText");

        public string PickAndChooseLableText => TextTranslator.Translate("MyViewSettings.PickAndChooseLableText");

        public string SubscribeMessageText => TextTranslator.Translate("MyViewSettings.SubscribeMessageText");

        public string PickAndChooseLableMobileText => TextTranslator.Translate("MyViewSettings.PickAndChooseLableMobileText");

        public string SubscribeUrl => SiterootContext.Item?.Subscribe_Link?.Url;

        public string publicationname => SiterootContext.Item.Publication_Name;

        public string SubscribedMessageText => TextTranslator.Translate("Registration.SubscribedMessageText");
        public bool isChannelBasedRegistration { get; set; }
        public bool isFromRegistration { get; set; }
        public bool enableSavePreferencesCheck { get; set; }
        public IList<Channel> Channels => GetChannels();

        public bool IsNewUser => UserPreferences.Preferences == null || UserPreferences.Preferences.IsNewUser;
        public ISubscription currentSubscriptions
        {
            get
            {
                return _subcriptions.Where(n => n.ProductCode == SiterootContext?.Item.Publication_Code).FirstOrDefault();
            }
        }

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
                            SetChannelOrderAndStus(channelPage, channel);
                            channel.IsSubscribed = currentSubscriptions?.SubscribedChannels?.Any(ch => ch.ChannelId == channelPage.Channel_Code && ch.ExpirationDate > DateTime.Now) ?? false;
                            GetTopics(channel, channelPage);
                            channels.Add(channel);
                        }
                    }
                }
            }

            return channels;
        }

        private void SetChannelOrderAndStus(IChannel_Page channelPage, Channel channel)
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