using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public class SavedDocumentReadResult : ISavedDocumentReadResult
    {
        public bool Success { get; set; }
        public IEnumerable<ISavedDocument> SavedDocuments { get; set; }
    }
}
