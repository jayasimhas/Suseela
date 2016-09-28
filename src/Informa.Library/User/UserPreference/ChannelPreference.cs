namespace Informa.Library.User.UserPreference
{
    using System.Collections.Generic;

    public class ChannelPreference : IChannelPreference
    {
        public Channel Channel { get; set; }
        public List<Topic> Topics { get; set; }
    }
}
