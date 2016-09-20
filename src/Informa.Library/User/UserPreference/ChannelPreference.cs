namespace Informa.Library.User.UserPreference
{
    using System.Collections.Generic;

    public class ChannelPreference : IChannelPreference
    {
        public IChannel Channel { get; set; }
        public List<ITopic> Topics { get; set; }
    }
}
