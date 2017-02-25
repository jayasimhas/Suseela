using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.User.Document;

namespace Informa.Library.User.ProductPreferences
{
    public interface IDeleteUserProductPreferences
    {
        IContentResponse DeleteUserProductPreference(string accessToken, string itemId);

        ISavedDocumentWriteResult DeleteSavedocument(string accessToken, string itemId);

        IContentResponse DeleteUserProductPreferences(string userName, string accessToken, string publicationCode, ProductPreferenceType type);
    }
}