namespace Informa.Library.User.UserPreference
{
    using System.Collections.Generic;

    public interface IChannelPreference
    {
        Channel Channel { get; set; }
        List<Topic> Topics { get; set; }
    }
}
