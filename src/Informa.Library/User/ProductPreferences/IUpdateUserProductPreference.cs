using Informa.Library.User.Content;
using Informa.Library.User.Search;

namespace Informa.Library.User.ProductPreferences
{
    public interface IUpdateUserProductPreference
    {
        IContentResponse UpdateUserSavedSearch(string accessToken, ISavedSearchEntity entity);
    }
}