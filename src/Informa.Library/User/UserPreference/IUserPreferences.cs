namespace Informa.Library.User.UserPreference
{
    using System.Collections.Generic;
    public interface IUserPreferences
    {
        IList<Channel> PreferredChannels { get; set; }
        bool IsNewUser { get; set; }
        bool IsChannelLevel { get; set; }
        string LastUpdateOn { get; set; }
    }
}
