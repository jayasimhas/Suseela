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
    public class Organization
    {
        [Key]
        [Display(Name = "Organization ID")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [StringLength(80)]
        [Createable(false)]
        public string Name { get; set; }

        [StringLength(80)]
        [Createable(false)]
        public string Division { get; set; }

        [Createable(false)]
        public string Street { get; set; }

        [StringLength(40)]
        [Createable(false)]
        public string City { get; set; }

        [Display(Name = "State/Province")]
        [StringLength(80)]
        [Createable(false)]
        public string State { get; set; }

        [Display(Name = "Zip/Postal Code")]
        [StringLength(20)]
        [Createable(false)]
        public string PostalCode { get; set; }

        [StringLength(80)]
        [Createable(false), Updateable(false)]
        public string Country { get; set; }

        [Createable(false)]
        public double? Latitude { get; set; }

        [Createable(false)]
        public double? Longitude { get; set; }

        [Phone]
        [Createable(false)]
        public string Phone { get; set; }

        [Phone]
        [Createable(false)]
        public string Fax { get; set; }

        [Display(Name = "Primary Contact")]
        [StringLength(80)]
        [Createable(false)]
        public string PrimaryContact { get; set; }

        [Display(Name = "Locale")]
        [Createable(false)]
        public string DefaultLocaleSidKey { get; set; }

        [Display(Name = "Language")]
        [Createable(false)]
        public string LanguageLocaleKey { get; set; }

        [Display(Name = "Info Emails")]
        [Createable(false)]
        public bool ReceivesInfoEmails { get; set; }

        [Display(Name = "Info Emails Admin")]
        [Createable(false)]
        public bool ReceivesAdminInfoEmails { get; set; }

        [Display(Name = "RequireOpportunityProducts")]
        [Createable(false)]
        public bool PreferencesRequireOpportunityProducts { get; set; }

        [Display(Name = "Fiscal Year Starts In")]
        [Createable(false), Updateable(false)]
        public int? FiscalYearStartMonth { get; set; }

        [Display(Name = "Fiscal Year Name by Start")]
        [Createable(false), Updateable(false)]
        public bool UsesStartDateAsFiscalYearName { get; set; }

        [Display(Name = "Default Account Access")]
        [Createable(false), Updateable(false)]
        public string DefaultAccountAccess { get; set; }

        [Display(Name = "Default Contact Access")]
        [Createable(false), Updateable(false)]
        public string DefaultContactAccess { get; set; }

        [Display(Name = "Default Opportunity Access")]
        [Createable(false), Updateable(false)]
        public string DefaultOpportunityAccess { get; set; }

        [Display(Name = "Default Lead Access")]
        [Createable(false), Updateable(false)]
        public string DefaultLeadAccess { get; set; }

        [Display(Name = "Default Case Access")]
        [Createable(false), Updateable(false)]
        public string DefaultCaseAccess { get; set; }

        [Display(Name = "Default Calendar Access")]
        [Createable(false), Updateable(false)]
        public string DefaultCalendarAccess { get; set; }

        [Display(Name = "Default Price Book Access")]
        [Createable(false), Updateable(false)]
        public string DefaultPricebookAccess { get; set; }

        [Display(Name = "Default Campaign Access")]
        [Createable(false), Updateable(false)]
        public string DefaultCampaignAccess { get; set; }

        [Display(Name = "System Modstamp")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset SystemModstamp { get; set; }

        [Display(Name = "Compliance BCC Email")]
        [EmailAddress]
        [Createable(false), Updateable(false)]
        public string ComplianceBccEmail { get; set; }

        [Display(Name = "UI Skin")]
        [Createable(false)]
        public string UiSkin { get; set; }

        [Display(Name = "Trial Expiration Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset? TrialExpirationDate { get; set; }

        [Display(Name = "Edition")]
        [Createable(false), Updateable(false)]
        public string OrganizationType { get; set; }

        [Display(Name = "Instance Name")]
        [StringLength(5)]
        [Createable(false), Updateable(false)]
        public string InstanceName { get; set; }

        [Display(Name = "Is Sandbox")]
        [Createable(false), Updateable(false)]
        public bool IsSandbox { get; set; }

        [Display(Name = "Web to Cases Default Origin")]
        [StringLength(40)]
        [Createable(false)]
        public string WebToCaseDefaultOrigin { get; set; }

        [Display(Name = "Monthly Page Views Used")]
        [Createable(false), Updateable(false)]
        public int? MonthlyPageViewsUsed { get; set; }

        [Display(Name = "Monthly Page Views Allowed")]
        [Createable(false), Updateable(false)]
        public int? MonthlyPageViewsEntitlement { get; set; }

        [Display(Name = "Total Trusted Application Requests Limit")]
        [Createable(false), Updateable(false)]
        public int? TotalTrustedRequestsLimit { get; set; }

        [Display(Name = "Total Trusted Application Requests Usage")]
        [Createable(false), Updateable(false)]
        public int? TotalTrustedRequestsUsage { get; set; }

        [Display(Name = "Created Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Created By ID")]
        [Createable(false), Updateable(false)]
        public string CreatedById { get; set; }

        [Display(Name = "Last Modified Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset LastModifiedDate { get; set; }

        [Display(Name = "Last Modified By ID")]
        [Createable(false), Updateable(false)]
        public string LastModifiedById { get; set; }

    }
}
