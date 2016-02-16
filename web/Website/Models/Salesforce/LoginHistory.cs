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
    public class LoginHistory
    {
        [Key]
        [Display(Name = "Login History Id")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [Display(Name = "User ID")]
        [Createable(false), Updateable(false)]
        public string UserId { get; set; }

        [Display(Name = "Login Time")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset LoginTime { get; set; }

        [Display(Name = "Login Type")]
        [Createable(false), Updateable(false)]
        public string LoginType { get; set; }

        [Display(Name = "Source IP")]
        [StringLength(39)]
        [Createable(false), Updateable(false)]
        public string SourceIp { get; set; }

        [Display(Name = "Login URL")]
        [StringLength(255)]
        [Createable(false), Updateable(false)]
        public string LoginUrl { get; set; }

        [StringLength(64)]
        [Createable(false), Updateable(false)]
        public string Browser { get; set; }

        [StringLength(64)]
        [Createable(false), Updateable(false)]
        public string Platform { get; set; }

        [StringLength(128)]
        [Createable(false), Updateable(false)]
        public string Status { get; set; }

        [StringLength(64)]
        [Createable(false), Updateable(false)]
        public string Application { get; set; }

        [Display(Name = "Client Version")]
        [StringLength(64)]
        [Createable(false), Updateable(false)]
        public string ClientVersion { get; set; }

        [Display(Name = "API Type")]
        [StringLength(64)]
        [Createable(false), Updateable(false)]
        public string ApiType { get; set; }

        [Display(Name = "API Version")]
        [StringLength(32)]
        [Createable(false), Updateable(false)]
        public string ApiVersion { get; set; }

    }
}
