namespace Informa.Library.User.UserPreference
{
    using System.Collections.Generic;
    public interface IUserPreferences
    {
        List<IChannelPreference> Preferences { get; set; }
    }
}
