using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.DCD.Models {
    public class EmailDealRequest {
        public string RecipientEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string PersonalMessage { get; set; }
        public string Subject { get; set; }
        public string RecaptchaResponse { get; set; }
        public string DealId { get; set; }
    }
}