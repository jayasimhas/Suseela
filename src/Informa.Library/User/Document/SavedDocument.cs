using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Document
{
    public class SavedDocument : ISavedDocument
    {
        public DateTime SaveDate { get; set; }
        public string DocumentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SalesforceId { get; set; }
    }
}
