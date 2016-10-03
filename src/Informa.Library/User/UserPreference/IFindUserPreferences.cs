namespace Informa.Library.User.UserPreference
{
    public interface IFindUserPreferences
    {
        IUserPreferences Find(string username);
    }
}
