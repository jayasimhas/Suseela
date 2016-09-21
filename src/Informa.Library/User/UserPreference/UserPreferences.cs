namespace Informa.Library.User.UserPreference
{
    using System;
    using System.Collections.Generic;

    public class UserPreferences : IUserPreferences
    {
       public List<IChannelPreference> Preferences { get; set; }
    }
}
