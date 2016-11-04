namespace Informa.Library.User.UserPreference
{
    using System;
    using System.Collections.Generic;

    public class UserPreferences : IUserPreferences
    {
        public IList<Channel> PreferredChannels { get; set; }
        public bool IsNewUser { get; set; }
        public bool IsChannelLevel { get; set; }
        public string LastUpdateOn { get; set; }
    }
}
