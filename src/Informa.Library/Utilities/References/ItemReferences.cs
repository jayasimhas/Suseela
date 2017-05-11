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

        //Global Taxonomy Folder
        string taxonomyFolderName = string.Empty;
        public Guid RegionsTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("RegionsTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid ContentTypesFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("ContentTypesFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid MediaTypeIconsFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("MediaTypeIconsFolder")) ? taxonomyFolderName : Guid.Empty.ToString());

        // Pharma Taxonomy Folders
        public Guid Folder => new Guid("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}"); //This Guid not found in CMS
        public Guid PharmaTaxonomyRootFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("TaxonomyRootFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid PharmaSubjectsTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("SubjectsTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid PharmaTherapyAreasTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("TherapyAreasTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid PharmaDeviceAreasTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("DeviceAreasTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid PharmaIndustriesTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("IndustryTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());

        // Agri Taxonomy Folders
        public Guid AgriTaxonomyRootFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("TaxonomyRootFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AgriAgencyRegulatorTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("AgencyRegulatorTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AgriAnimalHealthTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("AnimalHealthTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AgriCommercialTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("CommercialTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AgriCommodityTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("CommodityTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AgriCompaniesTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("CompaniesTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AgriCropProtectionaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("CropProtectionaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AgriCommoditesFactorsTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("CommodityFactorsTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AgriIndustriesTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("IndustriesTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());

        // Maritime Taxonomy Folders
        public Guid MaritimeTaxonomyRootFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("TaxonomyRootFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid MaritimeCompaniesTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("MaritimeCompaniesTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid MaritimeHotTopicsTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("HotTopicsTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid MaritimeMarketsTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("MarketsTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid MaritimeRegularsTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("RegularsTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid MaritimeSectorsTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("SectorsTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid MaritimeTopicTaxonomyFolder => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("TopicTaxonomyFolder")) ? taxonomyFolderName : Guid.Empty.ToString());

        #endregion


        public Guid SearchPage => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("ScripSearchPage")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid VwbSearchPage => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("VwbSearchPage")) ? taxonomyFolderName : Guid.Empty.ToString());


        public Guid IssuesRootCurrent => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("IssuesRootCurrent")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid IssuesRootArchive => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("IssuesRootArchive")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid IssueTemplate => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("IssueTemplate")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid IssueArchivedTemplate => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("IssueArchivedTemplate")) ? taxonomyFolderName : Guid.Empty.ToString());



        public Guid SubscriptionPage => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("ScripSubscriptionPage")) ? taxonomyFolderName : Guid.Empty.ToString());


        public Guid EmailPreferences => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("ScripEmailPreferences")) ? taxonomyFolderName : Guid.Empty.ToString());

        public Guid PasswordRecoveryEmail => new Guid("{0645BE3C-B851-427D-B91D-FC566FB813FA}");

        #region Renderings

        public Guid SiteHeaderRendering => new Guid("{83398B37-08CB-43A2-BC0A-7EB47E764AF4}");
        public Guid SiteFooterRendering => new Guid("{2889497D-2921-4BE1-BBF4-F4B4D2131231}");
        public Guid SiteSideNavigationRendering => new Guid("{65EDEFC9-82C2-47EE-93CB-A4D9372A45C0}");
        public Guid SignInPopOutRendering => new Guid("{82E58C71-9C3A-4967-AA0D-85AEB64D2E72}");

        #endregion

        #region Account Contact Info Drop Downs

        public Guid AccountCountries => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("AccountCountries")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AccountJobFunctions => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("AccountJobFunctions")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AccountJobIndustries => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("AccountJobIndustries")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AccountNameSuffixes => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("AccountNameSuffixes")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AccountPhoneTypes => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("AccountPhoneTypes")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid AccountSalutations => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("AccountSalutations")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid SiteLevelEntitlementType => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("EntitlementLevel.Content.Site")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid ChannelLevelEntitlementType => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("EntitlementLevel.Content.Channel")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid ItemLevelEntitlementType => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("EntitlementLevel.Content.Item")) ? taxonomyFolderName : Guid.Empty.ToString());

        #endregion Account Contact Info Drop Downs

        #region Templates

        public Guid FolderTemplate => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("FolderTemplate")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid TaxonomyRoot => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("TaxonomyRoot")) ? taxonomyFolderName : Guid.Empty.ToString()); //Need confirmation as it points to taxonomy root



        #endregion

        #region Restriction Access

        public Guid FreeWithEntitlement => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("FreeWithEntitlement")) ? taxonomyFolderName : Guid.Empty.ToString());
        public Guid FreeWithRegistration => new Guid(!string.IsNullOrEmpty(taxonomyFolderName = ItemIdResolver.GetItemIdByKey("FreeWithRegistration")) ? taxonomyFolderName : Guid.Empty.ToString());

        #endregion Restriction Access
    }
}
