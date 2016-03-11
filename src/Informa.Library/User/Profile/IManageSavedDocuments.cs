using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Authentication;

namespace Informa.Library.User.Profile
{
    public interface IManageSavedDocuments
    {
        ISavedDocumentReadResult QueryItems(IAuthenticatedUser user);
        bool IsBookmarked(IAuthenticatedUser user, Guid g);
        ISavedDocumentWriteResult RemoveItem(IAuthenticatedUser user, string documentId);
        ISavedDocumentWriteResult SaveItem(IAuthenticatedUser user, string documentName, string documentDescription, string documentId);
    }
}
