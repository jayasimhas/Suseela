using Informa.Library.User.Authentication;

namespace Informa.Library.User.ProductPreferences
{
    public interface IAddUserProductPreference
    {
        bool AddUserProductPreference<T>(IAuthenticatedUser user, T value, ProductPreferenceType type);
    }
}