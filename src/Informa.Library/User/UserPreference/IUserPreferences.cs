namespace Informa.Library.User.UserPreference
{
    using System.Collections.Generic;
    public interface IUserPreferences
    {
        List<ChannelPreference> PreferredChannels { get; set; }
    }
}
