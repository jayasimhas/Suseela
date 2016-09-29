namespace Informa.Library.Salesforce.User.UserPreferences
{
    using Informa.Library.User.UserPreference;

    public interface ISalesforceFindUserPreferences
    {
        IUserPreferences Find(string username);
    }
}
