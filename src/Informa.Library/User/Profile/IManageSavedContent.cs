using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Authentication;

namespace Informa.Library.User.Profile
{
    public interface IManageSavedContent
    {
        ISavedContentReadResult QueryItems(IAuthenticatedUser user);
        ISavedContentWriteResult RemoveItem(IAuthenticatedUser user, string documentId);
        ISavedContentWriteResult SaveItem(IAuthenticatedUser user, string documentName, string documentDescription, string documentId, DateTime saveDate);
    }
}
