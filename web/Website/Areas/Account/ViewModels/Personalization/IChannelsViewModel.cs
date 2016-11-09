using Informa.Library.User.UserPreference;
using System.Collections.Generic;

namespace Informa.Web.Areas.Account.ViewModels.Personalization
{
    public interface IChannelsViewModel
    {
        IList<Channel> Channels { get; }
        string ChannelFollowingButtonText { get; }
        string ChannelFollowButtonText { get; }
        string TopicFollowingButtonText { get; }
        string TopicFollowButtonText { get; }
        string FollowAllButtonText { get; }
        string UnfollowAllButtonText { get; }
        string SubscribeButtonText { get; }
        string SubscribedButtonText { get; }
        string MoveLableText { get; }
        bool IsNewUser { get; }
        string PickAndChooseLableText { get; }
        string PickAndChooseLableMobileText { get; }
        string SubscribeMessageText { get; }
        string SubscribeUrl { get; }
        string SubscribedMessageText { get; }
        bool isChannelBasedRegistration { get; set; }
        bool isFromRegistration { get; set; }
        bool enableSavePreferencesCheck { get; set; }
        string publicationname { get; }
    }
}