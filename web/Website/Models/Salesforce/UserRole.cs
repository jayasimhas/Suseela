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
    public class UserRole
    {
        [Key]
        [Display(Name = "Role ID")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [StringLength(80)]
        public string Name { get; set; }

        [Display(Name = "Parent Role ID")]
        public string ParentRoleId { get; set; }

        [Display(Name = "Description")]
        [StringLength(80)]
        public string RollupDescription { get; set; }

        [Display(Name = "Opportunity Access Level for Account Owner")]
        public string OpportunityAccessForAccountOwner { get; set; }

        [Display(Name = "Case Access Level for Account Owner")]
        public string CaseAccessForAccountOwner { get; set; }

        [Display(Name = "Contact Access Level for Account Owner")]
        [Createable(false), Updateable(false)]
        public string ContactAccessForAccountOwner { get; set; }

        [Display(Name = "User ID")]
        public string ForecastUserId { get; set; }

        [Display(Name = "May Forecast Manager Share")]
        [Createable(false), Updateable(false)]
        public bool MayForecastManagerShare { get; set; }

        [Display(Name = "Last Modified Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset LastModifiedDate { get; set; }

        [Display(Name = "Last Modified By ID")]
        [Createable(false), Updateable(false)]
        public string LastModifiedById { get; set; }

        [Display(Name = "System Modstamp")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset SystemModstamp { get; set; }

        [Display(Name = "Developer Name")]
        [StringLength(80)]
        public string DeveloperName { get; set; }

        [Display(Name = "Account ID")]
        [Updateable(false)]
        public string PortalAccountId { get; set; }

        [Display(Name = "Portal Type")]
        [Updateable(false)]
        public string PortalType { get; set; }

        [Display(Name = "User ID")]
        [Createable(false), Updateable(false)]
        public string PortalAccountOwnerId { get; set; }

    }
}
