using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class ManagementValidationReasons
    {
        public const string Required = "Required";
        public const string PasswordMismatch = "PasswordMismatch";
        public const string PasswordRequirements = "PasswordRequirements";
    }
}