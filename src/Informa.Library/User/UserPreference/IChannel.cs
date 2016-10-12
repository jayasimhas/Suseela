using System.Collections.Generic;

namespace Informa.Library.User.UserPreference
{
    public interface IChannel
    {
        string ChannelId { get; set; }
        string ChannelCode { get; set; }
        string ChannelName { get; set; }
        int ChannelOrder { get; set; }
        string ChannelLink { get; set; }
        bool IsSubscribed { get; set; }
        IList<Topic> Topics { get; set; }
    }
}
