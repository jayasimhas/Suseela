using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public class SavedDocumentWriteResult : ISavedDocumentWriteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
