using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class SavedContentSaveRequest
    {
        public string DocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentID { get; set; }
    }
}