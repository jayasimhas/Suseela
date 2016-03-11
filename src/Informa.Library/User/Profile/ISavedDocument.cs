using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public interface ISavedDocument
    {
        DateTime SaveDate { get; set; }
        string DocumentId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
