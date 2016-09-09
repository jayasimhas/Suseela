using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.Article.Models.Article.EmailFriend {
    public class EmailDCDRequest {

        public string RecipientEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string PersonalMessage { get; set; }
        public string Subject { get; set; }
				public string RecaptchaResponse { get; set; }
    }
}