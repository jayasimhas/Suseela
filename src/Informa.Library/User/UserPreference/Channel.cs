namespace Informa.Library.User.UserPreference
{
    public class Channel : IChannel
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int ChannelOrder { get; set; }
        public string ChannelLink { get; set; }
    }
}
