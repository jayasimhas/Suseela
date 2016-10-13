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

        // Taxonomy Folders
		Guid GlobalTaxonomyFolder { get; }
        Guid Folder { get; }
        Guid SubjectsTaxonomyFolder { get; }
		Guid RegionsTaxonomyFolder { get; }
		Guid TherapyAreasTaxonomyFolder { get; }
        Guid DeviceAreasTaxonomyFolder { get; }

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
	}
}
