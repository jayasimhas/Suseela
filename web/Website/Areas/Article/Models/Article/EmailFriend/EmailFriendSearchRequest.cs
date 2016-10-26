using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Informa.Web.Areas.Account.Models.User.Registration;

namespace Informa.Web.Areas.Article.Models.Article.EmailFriend
{
    public class EmailFriendSearchRequest
    {
        [Required(ErrorMessage = EmailFriendValidationReason.Required)]
        [EmailAddress(ErrorMessage = EmailFriendValidationReason.EmailRequirements)]
        public string RecipientEmail { get; set; }
        [Required(ErrorMessage = EmailFriendValidationReason.Required)]
        public string SenderName { get; set; }
        [Required(ErrorMessage = EmailFriendValidationReason.Required)]
        [EmailAddress(ErrorMessage = EmailFriendValidationReason.EmailRequirements)]
        public string SenderEmail { get; set; }
        [Required(ErrorMessage = EmailFriendValidationReason.Required)]
        public string Subject { get; set; }
        [Required(ErrorMessage = EmailFriendValidationReason.Required)]
        public string PersonalMessage { get; set; }
        [Required(ErrorMessage = EmailFriendValidationReason.Required)]
        public string ResultIDs { get; set; }
        [Required(ErrorMessage = EmailFriendValidationReason.Required)]
        public string QueryTerm { get; set; }
        [Required(ErrorMessage = EmailFriendValidationReason.Required)]
        public string QueryUrl { get; set; }
				public string RecaptchaResponse { get; set; }
    }
}