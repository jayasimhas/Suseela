namespace Informa.Library.User.UserPreference
{
    public interface IUserPreferenceContext
    {
        IUserPreferences Preferences { get; set; }
        void clear();
    }
}
