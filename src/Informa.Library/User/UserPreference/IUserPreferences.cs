namespace Informa.Library.User.UserPreference
{
    using System.Collections.Generic;
    public interface IUserPreferences
    {
        IList<Channel> PreferredChannels { get; set; }
    }
}
