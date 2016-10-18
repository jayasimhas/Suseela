namespace Informa.Library.User.UserPreference
{
    public interface ISetUserPreferences
    {
        bool Set(string username, string channelPreferences);
    }
}