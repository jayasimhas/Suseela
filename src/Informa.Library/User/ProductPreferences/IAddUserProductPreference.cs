using Informa.Library.User.Content;
using Informa.Library.User.Search;

namespace Informa.Library.User.ProductPreferences
{
    public interface IAddUserProductPreference
    {
        IContentResponse AddUserSavedSearch(string accessToken, ISavedSearchEntity entity);
    }
}