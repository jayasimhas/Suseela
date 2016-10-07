using System.Collections.Generic;

namespace Informa.Library.User.UserPreference
{
    public interface IChannel
    {
        int ChannelId { get; set; }
        string ChannelName { get; set; }
        int ChannelOrder { get; set; }

        IList<Topic> Topics { get; set; }
    }
}
