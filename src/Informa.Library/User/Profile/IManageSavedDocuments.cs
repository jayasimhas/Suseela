using Informa.Library.User.Authentication;

namespace Informa.Library.User.Profile
{
    public interface IManageSavedDocuments
    {
        ISavedDocumentWriteResult RemoveItem(IAuthenticatedUser user, string documentId);
        ISavedDocumentWriteResult SaveItem(IAuthenticatedUser user, string documentName, string documentDescription, string documentId);
    }
}
