using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.User.UserPreference;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class MyViewSettingsViewModel : GlassViewModel<IMy_View_Settings_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly IUserPreferenceContext UserPreferences;

        public MyViewSettingsViewModel(
                ITextTranslator translator,
                IGlobalSitecoreService globalService,
                IUserPreferenceContext userPreferences)
        {
            TextTranslator = translator;
            GlobalService = globalService;
            UserPreferences = userPreferences;
        }

        public string Title => GlassModel?.Title;

        public string SaveButtonText => TextTranslator.Translate("MyViewSettings.SaveButtonText");

        public string GoToMyViewButtonText => TextTranslator.Translate("MyViewSettings.GoToMyViewButtonText");

        public string FollowingButtonText => TextTranslator.Translate("MyViewSettings.FollowingButtonText");

        public string FollowButtonText => TextTranslator.Translate("MyViewSettings.FollowButtonText");

        public string FollowAllButtonText => TextTranslator.Translate("MyViewSettings.FollowAllButtonText");

        public string UnfollowAllButtonText => TextTranslator.Translate("MyViewSettings.UnfollowAllButtonText");

        public string SubscribeButtonText => TextTranslator.Translate("MyViewSettings.SubscribeButtonText");

        public string SubscribedButtonText => TextTranslator.Translate("MyViewSettings.SubscribedButtonText");

        public string SectionDescription => GlassModel?.Body;

        public IList<Channel> Channels => GetChannels();

        private IList<Channel> GetChannels()
        {
            var channels = new List<Channel>();

            var channelPages = GlobalService.GetItem<IChannels_Page>(GlassModel?._Id.ToString()).
                _ChildrenWithInferType.OfType<IChannel_Page>();

            if (channelPages != null && channelPages.Any())
            {
                Channel channel = null;
                foreach (IChannel_Page channelPage in channelPages)
                {
                    channel = new Channel();
                    channel.ChannelId = channelPage._Id.ToString();
                    channel.ChannelName = channelPage.Display_Text;
                    channel.ChannelCode = channelPage.Channel_Code;
                    channel.ChannelLink = channelPage.LinkableUrl;
                    channel.ChannelOrder = GetChannelOrder(channelPage);
                    GetTopics(channel, channelPage);

                    channels.Add(channel);
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
                    Where(ch => ch.ChannelId.Equals(channelPage._Id.ToString(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
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
            var topics = channelPage._ChildrenWithInferType.
                OfType<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>();
            if (topics != null && topics.Any())
            {
                Topic topic = null;
                foreach (Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic
                    topicItem in topics)
                {
                    topic = new Topic();
                    topic.TopicId = topicItem._Id.ToString();
                    topic.TopicName = topicItem.Title;
                    topic.TopicOrder = GetTopicOrder(channel, topicItem);
                    channel.Topics.Add(topic);
                }
            }
        }

        private int GetTopicOrder(Channel channel, Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topicPage)
        {
            if (UserPreferences.Preferences != null && UserPreferences.Preferences.PreferredChannels != null 
                && UserPreferences.Preferences.PreferredChannels.Any())
            {
                var preferredChannel = UserPreferences.Preferences.PreferredChannels.
                    Where(ch => ch.ChannelId.Equals(channel.ChannelId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (preferredChannel != null)
                {
                    var preferredTopic = preferredChannel.Topics.Where(t => t.TopicId.Equals(topicPage._Id.ToString(),
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

