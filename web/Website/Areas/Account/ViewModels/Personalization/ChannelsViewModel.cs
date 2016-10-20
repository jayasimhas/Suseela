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

        public IList<Channel> Channels => GetChannels();

        public bool IsNewUser => UserPreferences.Preferences == null || UserPreferences.Preferences.PreferredChannels == null
                || !UserPreferences.Preferences.PreferredChannels.Any();

        private IList<Channel> GetChannels()
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
                            channel.ChannelCode = channelPage.Channel_Code;
                            channel.ChannelLink = channelPage.LinkableUrl;
                            channel.ChannelOrder = GetChannelOrder(channelPage);
                            channel.IsSubscribed = _subcriptions.Where(sub => sub.ProductCode.Equals(channel.ChannelCode, StringComparison.InvariantCultureIgnoreCase)).Any();

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
                    Where(ch => ch.ChannelCode != null && ch.ChannelCode.Equals(channelPage.Channel_Code.ToString(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
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
                        topic.TopicName = string.IsNullOrWhiteSpace(topicItem.Navigation_Text) ? topicItem.Title : topicItem.Navigation_Text;
                        topic.TopicCode = topicItem.Navigation_Code;
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
                    var preferredTopic = preferredChannel.Topics.Where(t => t.TopicCode !=null && t.TopicCode.Equals(topicPage.Navigation_Code.ToString(),
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