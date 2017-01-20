using System;

namespace Informa.Library.Utilities.References
{
    public interface IItemReferences
    {
        Guid HomePage { get; }


        Guid NlmConfiguration { get; }

        Guid NlmErrorDistributionList { get; }

        Guid UserLockoutedEmails { get; }

		// Pharma Globals
		Guid NlmCopyrightStatement { get; }
        Guid InformaBar { get; }

        Guid GeneratedDictionary { get; }

        Guid DownloadTypes { get; }

        //Global Taxonomy folder
        Guid RegionsTaxonomyFolder { get; }

        // Pharma Taxonomy Folders
        Guid PharmaTaxonomyRootFolder { get; }
        Guid Folder { get; }
        Guid PharmaSubjectsTaxonomyFolder { get; }        
        Guid PharmaTherapyAreasTaxonomyFolder { get; }
        Guid PharmaDeviceAreasTaxonomyFolder { get; }
        Guid PharmaIndustriesTaxonomyFolder { get; }
       
        // Agri Taxonomy Folders
        Guid AgriTaxonomyRootFolder { get; }
        Guid AgriAgencyRegulatorTaxonomyFolder { get; }
        Guid AgriAnimalHealthTaxonomyFolder { get; }
        Guid AgriCommercialTaxonomyFolder { get; }
        Guid AgriCommodityTaxonomyFolder { get; }
        Guid AgriCompaniesTaxonomyFolder { get; }
        Guid AgriCropProtectionaxonomyFolder { get; }
        Guid AgriIndustriesTaxonomyFolder { get; }

        // Maritime Taxonomy Folders
        Guid MaritimeTaxonomyRootFolder { get; }       
        Guid MaritimeCompaniesTaxonomyFolder { get; }        
        Guid MaritimeHotTopicsTaxonomyFolder { get; }
        Guid MaritimeMarketsTaxonomyFolder { get; }
        Guid MaritimeRegularsTaxonomyFolder { get; }
        Guid MaritimeSectorsTaxonomyFolder { get; }
        Guid MaritimeTopicTaxonomyFolder { get; }
       

        //Account Drop Down
        Guid AccountCountries { get; }
        Guid AccountJobFunctions { get; }
        Guid AccountJobIndustries { get; }
        Guid AccountNameSuffixes { get; }
        Guid AccountPhoneTypes { get; }
        Guid AccountSalutations { get; }

    //Issues Items
    Guid IssuesRootCurrent { get; }
    Guid IssuesRootArchive { get; }

        
		// Templates
		Guid FolderTemplate { get; }

        Guid TaxonomyRoot { get; }

        Guid SubscriptionPage { get; }

        Guid EmailPreferences { get; }

        Guid PasswordRecoveryEmail { get; }
        // Restriction Access
        Guid FreeWithEntitlement { get; }
        Guid FreeWithRegistration { get; }

        Guid CustomPublishingConfig { get; }
	}
}
