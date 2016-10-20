﻿namespace Informa.Web.Areas.Account.ViewModels.Personalization
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


        public string FollowingButtonText => TextTranslator.Translate("MyViewSettings.FollowingButtonText");

        public string FollowButtonText => TextTranslator.Translate("MyViewSettings.FollowButtonText");

        public string FollowAllButtonText => TextTranslator.Translate("MyViewSettings.FollowAllButtonText");

        public string UnfollowAllButtonText => TextTranslator.Translate("MyViewSettings.UnfollowAllButtonText");

        public string SubscribeButtonText => TextTranslator.Translate("MyViewSettings.SubscribeButtonText");

        public string SubscribedButtonText => TextTranslator.Translate("MyViewSettings.SubscribedButtonText");

        public string MoveLableText => TextTranslator.Translate("MyViewSettings.MoveLableText");

        public string PickAndChooseLableText => TextTranslator.Translate("MyViewSettings.PickAndChooseLableText");

        public string SubscribeMessageText => TextTranslator.Translate("MyViewSettings.SubscribeMessageText");

        public string PickAndChooseLableMobileText => TextTranslator.Translate("MyViewSettings.PickAndChooseLableMobileText");

        public string SubscribeUrl => SiterootContext.Item?.Subscribe_Link?.Url;

        public string SubscribedMessageText => TextTranslator.Translate("Registration.SubscribedMessageText");
        public bool isChannelBasedRegistration { get; set; }
        public IList<Channel> Channels => GetChannels();

        public bool IsNewUser => UserPreferences.Preferences == null || UserPreferences.Preferences.PreferredChannels == null
                || !UserPreferences.Preferences.PreferredChannels.Any();

        private IList<Channel> GetChannels()
        {

            Item CurrentItem = Sitecore.Context.Item;
            string isFromRegistration = CurrentItem["IsFromRegistration"];
            if (!string.IsNullOrEmpty(isFromRegistration) && isFromRegistration == "1")
            {
                return GetPublicationAsChannel();
            }
            else
            {
                return GetAllChannels();
            }
        }

        private IList<Channel> GetPublicationAsChannel()
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

                // For beta user will be subscribed for all the channels. Bellow line will be replaced by commented line after Beta.
                channel.IsSubscribed = true;
                //channel.IsSubscribed = _subcriptions.Where(sub => sub.ProductCode.Equals(channel.ChannelCode, StringComparison.InvariantCultureIgnoreCase)).Any();

                GetChannelsAsTopics(channel);
                channels.Add(channel);
            }
            return channels;
        }
        private void GetChannelsAsTopics(Channel channel)
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
                    //channel based registration
                    if (channelPages.Count() > 1)
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
                                // topic.TopicOrder = GetChannelOrder(channelPage);

                                // For beta user will be subscribed for all the channels. Bellow line will be replaced by commented line after Beta.
                                topic.IsSubscribed = true;
                                //channel.IsSubscribed = _subcriptions.Where(sub => sub.ProductCode.Equals(channel.ChannelCode, StringComparison.InvariantCultureIgnoreCase)).Any();
                                channel.Topics.Add(topic);
                            }
                        }
                    }
                    else
                    {
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
                                    topic.TopicName = string.IsNullOrWhiteSpace(topicItem.Display_Text) ? topicItem.Title : topicItem.Display_Text;
                                    topic.TopicCode = string.IsNullOrWhiteSpace(topicItem.Topic_Code) ? topicItem.Title : topicItem.Topic_Code;
                                    //topic.TopicOrder = GetTopicOrder(channel, topicItem);
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
                            channel.ChannelOrder = GetChannelOrder(channelPage);

                            // For beta user will be subscribed for all the channels. Bellow line will be replaced by commented line after Beta.
                            channel.IsSubscribed = true;
                            //channel.IsSubscribed = _subcriptions.Where(sub => sub.ProductCode.Equals(channel.ChannelCode, StringComparison.InvariantCultureIgnoreCase)).Any();

                            GetTopics(channel, channelPage);

                            channels.Add(channel);
                        }
                    }
                }
            }

            return channels;
        }

        private int GetChannelOrder(IChannel_Page channelPage)
        {
            if (UserPreferences.Preferences != null && UserPreferences.Preferences.PreferredChannels != null
                && UserPreferences.Preferences.PreferredChannels.Any())
            {
                var preferredChannel = UserPreferences.Preferences.PreferredChannels.
                    Where(ch => ch.ChannelCode.Equals(channelPage.Title.ToString(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (preferredChannel != null)
                {
                    return preferredChannel.ChannelOrder;
                }
            }
            return 0;
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
                        topic.TopicName = string.IsNullOrWhiteSpace(topicItem.Display_Text) ? topicItem.Title : topicItem.Display_Text;
                        topic.TopicCode = string.IsNullOrWhiteSpace(topicItem.Topic_Code) ? topicItem.Title : topicItem.Topic_Code;
                        topic.TopicOrder = GetTopicOrder(channel, topicItem);
                        topic.IsFollowing = IsNewUser ? IsNewUser : topic.TopicOrder > 0;
                        channel.Topics.Add(topic);
                    }
                }
            }
        }

        private int GetTopicOrder(Channel channel, Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topicPage)
        {
            if (UserPreferences.Preferences != null && UserPreferences.Preferences.PreferredChannels != null
                && UserPreferences.Preferences.PreferredChannels.Any())
            {
                var preferredChannel = UserPreferences.Preferences.PreferredChannels.
                    Where(ch => ch.ChannelCode.Equals(channel.ChannelCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (preferredChannel != null)
                {
                    var preferredTopic = preferredChannel.Topics.Where(t => t.TopicCode.Equals(topicPage.Title.ToString(),
                        StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (preferredTopic != null)
                    {
                        return preferredTopic.TopicOrder;
                    }
                }
            }

            return 0;
        }
    }
}