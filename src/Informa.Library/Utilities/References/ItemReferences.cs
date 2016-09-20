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

        #region Pharma Globals
        public Guid NlmCopyrightStatement => new Guid(ItemIdResolver.GetItemIdByKey("NlmCopyrightStatement"));     
        public Guid InformaBar => new Guid(ItemIdResolver.GetItemIdByKey("InformaBar"));

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
