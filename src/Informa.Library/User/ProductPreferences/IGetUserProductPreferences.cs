using Informa.Library.User.Authentication;

namespace Informa.Library.User.ProductPreferences
{
    public interface IGetUserProductPreferences
    {
        T GetProductPreferences<T>(IAuthenticatedUser user, string publicationCode, ProductPreferenceType type);
    }
}
