using System.Collections.Generic;
using System;

namespace Informa.Library.User.UserPreference
{
    public class Channel : IChannel
    {
        public string ChannelId { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public int ChannelOrder { get; set; }
        public string ChannelLink { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsFollowing { get; set; }
        public IList<Topic> Topics { get; set; }
        public string Taxonomy { get; set; }
    }
}
