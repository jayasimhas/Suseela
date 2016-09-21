namespace Informa.Library.User.UserPreference
{
    using System.Collections.Generic;

    public interface IChannelPreference
    {
        IChannel Channel { get; set; }
        List<ITopic> Topics { get; set; }
    }
}
