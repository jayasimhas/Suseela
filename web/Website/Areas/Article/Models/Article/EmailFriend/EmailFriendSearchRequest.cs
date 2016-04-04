using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.Article.Models.Article.EmailFriend
{
    public class EmailFriendSearchRequest
    {
        public string RecipientEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string PersonalMessage { get; set; }
        public string ResultIDs { get; set; }
        public string QueryTerm { get; set; }
        public string QueryUrl { get; set; }
    }
}