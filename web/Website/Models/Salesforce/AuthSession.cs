using Newtonsoft.Json;
using Salesforce.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Web.Models.Salesforce
{
    public class AuthSession
    {
        [Key]
        [Display(Name = "Auth Session ID")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [Display(Name = "User ID")]
        [Createable(false), Updateable(false)]
        public string UsersId { get; set; }

        [Display(Name = "Created")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Updated")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset LastModifiedDate { get; set; }

        [Display(Name = "Valid For")]
        [Createable(false), Updateable(false)]
        public int NumSecondsValid { get; set; }

        [Display(Name = "User Type")]
        [Createable(false), Updateable(false)]
        public string UserType { get; set; }

        [Display(Name = "Source IP")]
        [StringLength(39)]
        [Createable(false), Updateable(false)]
        public string SourceIp { get; set; }

        [Display(Name = "Login")]
        [Createable(false), Updateable(false)]
        public string LoginType { get; set; }

        [Display(Name = "Session Type")]
        [Createable(false), Updateable(false)]
        public string SessionType { get; set; }

        [Display(Name = "Session Security Level")]
        [Createable(false), Updateable(false)]
        public string SessionSecurityLevel { get; set; }

        [Display(Name = "Logout URL")]
        [StringLength(1500)]
        [Createable(false), Updateable(false)]
        public string LogoutUrl { get; set; }

        [Display(Name = "Auth Session ID")]
        [Createable(false), Updateable(false)]
        public string ParentId { get; set; }

    }
}
