using System.ComponentModel.DataAnnotations;
using Informa.Library.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class PasswordUpdateRequest
    {
        public string CurrentPassword { get; set; }
        [Password(ErrorMessage = ManagementValidationReasons.PasswordRequirements)]
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword), ErrorMessage = ManagementValidationReasons.PasswordMismatch)]
        public string NewPasswordConfirm { get; set; }
    }
}