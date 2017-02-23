using Informa.Library.User.Authentication;

namespace Informa.Library.User.ProductPreferences
{
    public interface IDeleteUserProductPreferences
    {
        bool DeleteUserProductPreference<T>(IAuthenticatedUser user, T value, ProductPreferenceType type);
    }
}