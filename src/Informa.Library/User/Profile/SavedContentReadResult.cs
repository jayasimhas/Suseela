using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public class SavedContentReadResult : ISavedContentReadResult
    {
        public bool Success { get; set; }
        public IEnumerable<ISavedContent> SavedContentItems { get; set; }
    }
}
