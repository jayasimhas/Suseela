using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class SavedDocumentSaveRequest
    {
        public string DocumentID { get; set; }
        public string SalesforceId { get; set; }
    }
}