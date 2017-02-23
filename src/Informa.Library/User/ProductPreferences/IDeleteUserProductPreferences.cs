using Informa.Library.User.Authentication;
using Informa.Library.User.Content;

namespace Informa.Library.User.ProductPreferences
{
    public interface IDeleteUserProductPreferences
    {
        IContentResponse DeleteUserProductPreference(string accessToken, string itemId);
    }
}