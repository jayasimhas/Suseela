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
    public class Profile
    {
        [Key]
        [Display(Name = "Profile ID")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [StringLength(255)]
        [Createable(false)]
        public string Name { get; set; }

        [Display(Name = "Send Email")]
        [Createable(false)]
        public bool PermissionsEmailSingle { get; set; }

        [Display(Name = "Mass Email")]
        [Createable(false)]
        public bool PermissionsEmailMass { get; set; }

        [Display(Name = "Edit Tasks")]
        [Createable(false)]
        public bool PermissionsEditTask { get; set; }

        [Display(Name = "Edit Events")]
        [Createable(false)]
        public bool PermissionsEditEvent { get; set; }

        [Display(Name = "Export Reports")]
        [Createable(false)]
        public bool PermissionsExportReport { get; set; }

        [Display(Name = "Import Personal Contacts")]
        [Createable(false)]
        public bool PermissionsImportPersonal { get; set; }

        [Display(Name = "Weekly Data Export")]
        [Createable(false)]
        public bool PermissionsDataExport { get; set; }

        [Display(Name = "Manage Users")]
        [Createable(false)]
        public bool PermissionsManageUsers { get; set; }

        [Display(Name = "Manage Public Templates")]
        [Createable(false)]
        public bool PermissionsEditPublicTemplates { get; set; }

        [Display(Name = "Modify All Data")]
        [Createable(false)]
        public bool PermissionsModifyAllData { get; set; }

        [Display(Name = "Manage Cases")]
        [Createable(false)]
        public bool PermissionsManageCases { get; set; }

        [Display(Name = "Mass Edits from Lists")]
        [Createable(false)]
        public bool PermissionsMassInlineEdit { get; set; }

        [Display(Name = "Tag Manager")]
        [Createable(false)]
        public bool PermissionsTagManager { get; set; }

        [Display(Name = "Manage Published Solutions")]
        [Createable(false)]
        public bool PermissionsManageSolutions { get; set; }

        [Display(Name = "Customize Application")]
        [Createable(false)]
        public bool PermissionsCustomizeApplication { get; set; }

        [Display(Name = "Edit Read Only Fields")]
        [Createable(false)]
        public bool PermissionsEditReadonlyFields { get; set; }

        [Display(Name = "Run Reports")]
        [Createable(false)]
        public bool PermissionsRunReports { get; set; }

        [Display(Name = "View Setup and Configuration")]
        [Createable(false)]
        public bool PermissionsViewSetup { get; set; }

        [Display(Name = "Transfer Record")]
        [Createable(false)]
        public bool PermissionsTransferAnyEntity { get; set; }

        [Display(Name = "Report Builder")]
        [Createable(false)]
        public bool PermissionsNewReportBuilder { get; set; }

        [Display(Name = "Manage Self-Service Portal")]
        [Createable(false)]
        public bool PermissionsManageSelfService { get; set; }

        [Display(Name = "Edit Self-Service Users")]
        [Createable(false)]
        public bool PermissionsManageCssUsers { get; set; }

        [Display(Name = "Activate Contracts")]
        [Createable(false)]
        public bool PermissionsActivateContract { get; set; }

        [Display(Name = "Import Leads")]
        [Createable(false)]
        public bool PermissionsImportLeads { get; set; }

        [Display(Name = "Manage Leads")]
        [Createable(false)]
        public bool PermissionsManageLeads { get; set; }

        [Display(Name = "Transfer Leads")]
        [Createable(false)]
        public bool PermissionsTransferAnyLead { get; set; }

        [Display(Name = "View All Data")]
        [Createable(false)]
        public bool PermissionsViewAllData { get; set; }

        [Display(Name = "Manage Public Documents")]
        [Createable(false)]
        public bool PermissionsEditPublicDocuments { get; set; }

        [Display(Name = "View Encrypted Data")]
        [Createable(false)]
        public bool PermissionsViewEncryptedData { get; set; }

        [Display(Name = "Manage Letterheads")]
        [Createable(false)]
        public bool PermissionsEditBrandTemplates { get; set; }

        [Display(Name = "Edit HTML Templates")]
        [Createable(false)]
        public bool PermissionsEditHtmlTemplates { get; set; }

        [Display(Name = "Chatter Internal User")]
        [Createable(false)]
        public bool PermissionsChatterInternalUser { get; set; }

        [Display(Name = "Manage Dashboards")]
        [Createable(false)]
        public bool PermissionsManageDashboards { get; set; }

        [Display(Name = "Delete Activated Contracts")]
        [Createable(false)]
        public bool PermissionsDeleteActivatedContract { get; set; }

        [Display(Name = "Invite Customers To Chatter")]
        [Createable(false)]
        public bool PermissionsChatterInviteExternalUsers { get; set; }

        [Display(Name = "Send Stay-in-Touch Requests")]
        [Createable(false)]
        public bool PermissionsSendSitRequests { get; set; }

        [Display(Name = "Api Only User")]
        [Createable(false)]
        public bool PermissionsApiUserOnly { get; set; }

        [Display(Name = "Manage Connected Apps")]
        [Createable(false)]
        public bool PermissionsManageRemoteAccess { get; set; }

        [Display(Name = "Drag-and-Drop Dashboard Builder")]
        [Createable(false)]
        public bool PermissionsCanUseNewDashboardBuilder { get; set; }

        [Display(Name = "Manage Categories")]
        [Createable(false)]
        public bool PermissionsManageCategories { get; set; }

        [Display(Name = "Convert Leads")]
        [Createable(false)]
        public bool PermissionsConvertLeads { get; set; }

        [Display(Name = "Password Never Expires")]
        [Createable(false)]
        public bool PermissionsPasswordNeverExpires { get; set; }

        [Display(Name = "Use Team Reassignment Wizards")]
        [Createable(false)]
        public bool PermissionsUseTeamReassignWizards { get; set; }

        [Display(Name = "Download AppExchange Packages")]
        [Createable(false)]
        public bool PermissionsInstallMultiforce { get; set; }

        [Display(Name = "Upload AppExchange Packages")]
        [Createable(false)]
        public bool PermissionsPublishMultiforce { get; set; }

        [Display(Name = "Create and Own New Chatter Groups")]
        [Createable(false)]
        public bool PermissionsChatterOwnGroups { get; set; }

        [Display(Name = "Edit Opportunity Product Sales Price")]
        [Createable(false)]
        public bool PermissionsEditOppLineItemUnitPrice { get; set; }

        [Display(Name = "Create AppExchange Packages")]
        [Createable(false)]
        public bool PermissionsCreateMultiforce { get; set; }

        [Display(Name = "Bulk API Hard Delete")]
        [Createable(false)]
        public bool PermissionsBulkApiHardDelete { get; set; }

        [Display(Name = "Deploy Change Sets")]
        [Createable(false)]
        public bool PermissionsInboundMigrationToolsUser { get; set; }

        [Display(Name = "Import Solutions")]
        [Createable(false)]
        public bool PermissionsSolutionImport { get; set; }

        [Display(Name = "Manage Call Centers")]
        [Createable(false)]
        public bool PermissionsManageCallCenters { get; set; }

        [Display(Name = "Create and Customize Reports")]
        [Createable(false)]
        public bool PermissionsEditReports { get; set; }

        [Display(Name = "Create and Upload Change Sets")]
        [Createable(false)]
        public bool PermissionsOutboundMigrationToolsUser { get; set; }

        [Display(Name = "View Content in Portals")]
        [Createable(false)]
        public bool PermissionsViewContent { get; set; }

        [Display(Name = "Manage Email Client Configurations")]
        [Createable(false)]
        public bool PermissionsManageEmailClientConfig { get; set; }

        [Display(Name = "Send Outbound Messages")]
        [Createable(false)]
        public bool PermissionsEnableNotifications { get; set; }

        [Display(Name = "Manage Data Integrations")]
        [Createable(false)]
        public bool PermissionsManageDataIntegrations { get; set; }

        [Display(Name = "View Data Categories")]
        [Createable(false)]
        public bool PermissionsViewDataCategories { get; set; }

        [Display(Name = "Manage Data Categories")]
        [Createable(false)]
        public bool PermissionsManageDataCategories { get; set; }

        [Display(Name = "Author Apex")]
        [Createable(false)]
        public bool PermissionsAuthorApex { get; set; }

        [Display(Name = "Manage Mobile Configurations")]
        [Createable(false)]
        public bool PermissionsManageMobile { get; set; }

        [Display(Name = "API Enabled")]
        [Createable(false)]
        public bool PermissionsApiEnabled { get; set; }

        [Display(Name = "Manage Custom Report Types")]
        [Createable(false)]
        public bool PermissionsManageCustomReportTypes { get; set; }

        [Display(Name = "Edit Case Comments")]
        [Createable(false)]
        public bool PermissionsEditCaseComments { get; set; }

        [Display(Name = "Transfer Cases")]
        [Createable(false)]
        public bool PermissionsTransferAnyCase { get; set; }

        [Display(Name = "Manage Salesforce CRM Content")]
        [Createable(false)]
        public bool PermissionsContentAdministrator { get; set; }

        [Display(Name = "Create Libraries")]
        [Createable(false)]
        public bool PermissionsCreateWorkspaces { get; set; }

        [Display(Name = "Manage Content Permissions")]
        [Createable(false)]
        public bool PermissionsManageContentPermissions { get; set; }

        [Display(Name = "Manage Content Properties")]
        [Createable(false)]
        public bool PermissionsManageContentProperties { get; set; }

        [Display(Name = "Manage record types and layouts for Files")]
        [Createable(false)]
        public bool PermissionsManageContentTypes { get; set; }

        [Display(Name = "Schedule Dashboards")]
        [Createable(false)]
        public bool PermissionsScheduleJob { get; set; }

        [Display(Name = "Manage Exchange Configurations")]
        [Createable(false)]
        public bool PermissionsManageExchangeConfig { get; set; }

        [Display(Name = "Manage Reporting Snapshots")]
        [Createable(false)]
        public bool PermissionsManageAnalyticSnapshots { get; set; }

        [Display(Name = "Schedule Reports")]
        [Createable(false)]
        public bool PermissionsScheduleReports { get; set; }

        [Display(Name = "Manage Business Hours Holidays")]
        [Createable(false)]
        public bool PermissionsManageBusinessHourHolidays { get; set; }

        [Display(Name = "Manage Dynamic Dashboards")]
        [Createable(false)]
        public bool PermissionsManageDynamicDashboards { get; set; }

        [Display(Name = "Manage Force.com Flow")]
        [Createable(false)]
        public bool PermissionsManageInteraction { get; set; }

        [Display(Name = "View My Team's Dashboards")]
        [Createable(false)]
        public bool PermissionsViewMyTeamsDashboards { get; set; }

        [Display(Name = "Moderate Chatter")]
        [Createable(false)]
        public bool PermissionsModerateChatter { get; set; }

        [Display(Name = "Reset User Passwords and Unlock Users")]
        [Createable(false)]
        public bool PermissionsResetPasswords { get; set; }

        [Display(Name = "Require Force.com Flow User Feature License")]
        [Createable(false)]
        public bool PermissionsFlowUFLRequired { get; set; }

        [Display(Name = "Insert System Field Values for Chatter Feeds")]
        [Createable(false)]
        public bool PermissionsCanInsertFeedSystemFields { get; set; }

        [Display(Name = "Manage Email Templates")]
        [Createable(false)]
        public bool PermissionsEmailTemplateManagement { get; set; }

        [Display(Name = "Email Administration")]
        [Createable(false)]
        public bool PermissionsEmailAdministration { get; set; }

        [Display(Name = "Manage Chatter Messages")]
        [Createable(false)]
        public bool PermissionsManageChatterMessages { get; set; }

        [Display(Name = "Email-Based Identity Confirmation Option")]
        [Createable(false)]
        public bool PermissionsAllowEmailIC { get; set; }

        [Display(Name = "Two-Factor Authentication for User Interface Logins")]
        [Createable(false)]
        public bool PermissionsForceTwoFactor { get; set; }

        [Display(Name = "View Event Log Files")]
        [Createable(false)]
        public bool PermissionsViewEventLogFiles { get; set; }

        [Display(Name = "Manage Auth. Providers")]
        [Createable(false)]
        public bool PermissionsManageAuthProviders { get; set; }

        [Display(Name = "Run Flows")]
        [Createable(false)]
        public bool PermissionsRunFlow { get; set; }

        [Display(Name = "View All Users")]
        [Createable(false)]
        public bool PermissionsViewAllUsers { get; set; }

        [Display(Name = "Connect Organization to Environment Hub")]
        [Createable(false)]
        public bool PermissionsConnectOrgToEnvironmentHub { get; set; }

        [Display(Name = "Create and Customize List Views")]
        [Createable(false)]
        public bool PermissionsCreateCustomizeFilters { get; set; }

        [Display(Name = "Two-Factor Authentication for API Logins")]
        [Createable(false)]
        public bool PermissionsTwoFactorApi { get; set; }

        [Display(Name = "Delete Topics")]
        [Createable(false)]
        public bool PermissionsDeleteTopics { get; set; }

        [Display(Name = "Edit Topics")]
        [Createable(false)]
        public bool PermissionsEditTopics { get; set; }

        [Display(Name = "Create Topics")]
        [Createable(false)]
        public bool PermissionsCreateTopics { get; set; }

        [Display(Name = "Assign Topics")]
        [Createable(false)]
        public bool PermissionsAssignTopics { get; set; }

        [Display(Name = "Use Identity Features")]
        [Createable(false)]
        public bool PermissionsIdentityEnabled { get; set; }

        [Display(Name = "Use Identity Connect")]
        [Createable(false)]
        public bool PermissionsIdentityConnect { get; set; }

        [Display(Name = "Access Custom Mobile Apps")]
        [Createable(false)]
        public bool PermissionsCustomMobileAppsAccess { get; set; }

        [Display(Name = "View Help Link")]
        [Createable(false)]
        public bool PermissionsViewHelpLink { get; set; }

        [Display(Name = "Manage Profiles and Permission Sets")]
        [Createable(false)]
        public bool PermissionsManageProfilesPermissionsets { get; set; }

        [Display(Name = "Assign Permission Sets")]
        [Createable(false)]
        public bool PermissionsAssignPermissionSets { get; set; }

        [Display(Name = "Manage Roles")]
        [Createable(false)]
        public bool PermissionsManageRoles { get; set; }

        [Display(Name = "Manage IP Addresses")]
        [Createable(false)]
        public bool PermissionsManageIpAddresses { get; set; }

        [Display(Name = "Manage Sharing")]
        [Createable(false)]
        public bool PermissionsManageSharing { get; set; }

        [Display(Name = "Manage Internal Users")]
        [Createable(false)]
        public bool PermissionsManageInternalUsers { get; set; }

        [Display(Name = "Manage Password Policies")]
        [Createable(false)]
        public bool PermissionsManagePasswordPolicies { get; set; }

        [Display(Name = "Manage Login Access Policies")]
        [Createable(false)]
        public bool PermissionsManageLoginAccessPolicies { get; set; }

        [Display(Name = "Manage Custom Permissions")]
        [Createable(false)]
        public bool PermissionsManageCustomPermissions { get; set; }

        [Display(Name = "Manage Unlisted Groups")]
        [Createable(false)]
        public bool PermissionsManageUnlistedGroups { get; set; }

        [Display(Name = "Manage Two-Factor Authentication")]
        [Createable(false)]
        public bool PermissionsManageTwoFactor { get; set; }

        [Display(Name = "Access Chatter For SharePoint")]
        [Createable(false)]
        public bool PermissionsChatterForSharePoint { get; set; }

        [Display(Name = "User License ID")]
        [Createable(false), Updateable(false)]
        public string UserLicenseId { get; set; }

        [Display(Name = "User Type")]
        [Createable(false), Updateable(false)]
        public string UserType { get; set; }

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

        [Display(Name = "System Modstamp")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset SystemModstamp { get; set; }

        [StringLength(255)]
        [Createable(false)]
        public string Description { get; set; }

        [Display(Name = "Last Viewed Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset? LastViewedDate { get; set; }

        [Display(Name = "Last Referenced Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset? LastReferencedDate { get; set; }

    }
}
