using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;
using Informa.Library.Utilities.CMSHelpers;

namespace Informa.Library.Utilities.References
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemReferences : IItemReferences
	{
        
		public static IItemReferences Instance => AutofacConfig.ServiceLocator.Resolve<Owned<IItemReferences>>().Value;


		public Guid HomePage => new Guid(ItemIdResolver.GetItemIdByKey("ScripHomePage"));
        
		public Guid NlmConfiguration => new Guid(ItemIdResolver.GetItemIdByKey("NlmConfiguration"));
		public Guid NlmErrorDistributionList => new Guid(ItemIdResolver.GetItemIdByKey("NlmErrorDistributionList"));
        public Guid CustomPublishingConfig => new Guid("{E892C2E5-4091-43A7-AAF6-C3A2DFCE05CE}");
        #region Pharma Globals
        public Guid NlmCopyrightStatement => new Guid(ItemIdResolver.GetItemIdByKey("NlmCopyrightStatement"));     
        public Guid InformaBar => new Guid(ItemIdResolver.GetItemIdByKey("InformaBar"));

        public Guid UserLockoutedEmails => new Guid("{8A553CE0-6AD6-4CC8-964C-BEAA9714F74B}");

        public Guid GeneratedDictionary => new Guid(ItemIdResolver.GetItemIdByKey("GeneratedDictionary"));

        public Guid DownloadTypes => new Guid(ItemIdResolver.GetItemIdByKey("DownloadTypes"));
        #endregion

        #region Taxonomy Folders

        public Guid Folder => new Guid("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}"); //This Guid not found in CMS
        public Guid SubjectsTaxonomyFolder => new Guid(ItemIdResolver.GetItemIdByKey("SubjectsTaxonomyFolder"));
		public Guid RegionsTaxonomyFolder => new Guid(ItemIdResolver.GetItemIdByKey("RegionsTaxonomyFolder"));
		public Guid TherapyAreasTaxonomyFolder => new Guid(ItemIdResolver.GetItemIdByKey("TherapyAreasTaxonomyFolder"));
        public Guid DeviceAreasTaxonomyFolder => new Guid(ItemIdResolver.GetItemIdByKey("DeviceAreasTaxonomyFolder"));

		#endregion

		public Guid SearchPage => new Guid(ItemIdResolver.GetItemIdByKey("ScripSearchPage"));
		public Guid VwbSearchPage => new Guid(ItemIdResolver.GetItemIdByKey("VwbSearchPage"));

    public Guid IssuesRootCurrent => new Guid(ItemIdResolver.GetItemIdByKey("IssuesRootCurrent"));
    public Guid IssuesRootArchive => new Guid(ItemIdResolver.GetItemIdByKey("IssuesRootArchive"));
    public Guid IssueTemplate => new Guid(ItemIdResolver.GetItemIdByKey("IssueTemplate"));
    public Guid IssueArchivedTemplate => new Guid(ItemIdResolver.GetItemIdByKey("IssueArchivedTemplate"));
     

    public Guid SubscriptionPage => new Guid(ItemIdResolver.GetItemIdByKey("ScripSubscriptionPage"));

		public Guid EmailPreferences => new Guid(ItemIdResolver.GetItemIdByKey("ScripEmailPreferences"));

        public Guid PasswordRecoveryEmail => new Guid("{0645BE3C-B851-427D-B91D-FC566FB813FA}");

        #region Renderings

        public Guid SiteHeaderRendering => new Guid("{83398B37-08CB-43A2-BC0A-7EB47E764AF4}");
		public Guid SiteFooterRendering => new Guid("{2889497D-2921-4BE1-BBF4-F4B4D2131231}");
		public Guid SiteSideNavigationRendering => new Guid("{65EDEFC9-82C2-47EE-93CB-A4D9372A45C0}");
		public Guid SignInPopOutRendering => new Guid("{82E58C71-9C3A-4967-AA0D-85AEB64D2E72}");
		
		#endregion

		#region Account Contact Info Drop Downs

		public Guid AccountCountries => new Guid(ItemIdResolver.GetItemIdByKey("AccountCountries"));
		public Guid AccountJobFunctions => new Guid(ItemIdResolver.GetItemIdByKey("AccountJobFunctions"));
		public Guid AccountJobIndustries => new Guid(ItemIdResolver.GetItemIdByKey("AccountJobIndustries"));
		public Guid AccountNameSuffixes => new Guid(ItemIdResolver.GetItemIdByKey("AccountNameSuffixes"));
		public Guid AccountPhoneTypes => new Guid(ItemIdResolver.GetItemIdByKey("AccountPhoneTypes"));
		public Guid AccountSalutations => new Guid(ItemIdResolver.GetItemIdByKey("AccountSalutations"));

	  #endregion Account Contact Info Drop Downs

		#region Templates

		public Guid FolderTemplate => new Guid(ItemIdResolver.GetItemIdByKey("FolderTemplate"));
		public Guid TaxonomyRoot => new Guid(ItemIdResolver.GetItemIdByKey("TaxonomyRoot")); //Need confirmation as it points to taxonomy root



        #endregion

        #region Restriction Access

        public Guid FreeWithEntitlement => new Guid(ItemIdResolver.GetItemIdByKey("FreeWithEntitlement"));
        public Guid FreeWithRegistration => new Guid(ItemIdResolver.GetItemIdByKey("FreeWithRegistration"));

        #endregion Restriction Access
    }
}
