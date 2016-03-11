using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public interface ISavedDocumentReadResult
    {
        bool Success { get; set; }
        IEnumerable<ISavedDocument> SavedDocuments { get; set; }
    }
}
