using Informa.Library.User.Authentication;

namespace Informa.Library.User.ProductPreferences
{
    public interface IUpdateUserProductPreference
    {
       bool UpdateUserProductPreference<T>(IAuthenticatedUser user, T value, ProductPreferenceType type);
    }
}