using Informa.Library.User.UserPreference;
using System.Collections.Generic;

namespace Informa.Web.Areas.Account.ViewModels.Personalization
{
    public interface IChannelsViewModel
    {
        IList<Channel> Channels { get; }
        string FollowingButtonText { get; }
        string FollowButtonText { get; }
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
    }
}