using System;

namespace Informa.Library.Utilities.References
{
	public interface IItemReferences
	{
		Guid HomePage { get; }

		Guid DCDConfigurationItem { get; }

		Guid NlmConfiguration { get; }

		Guid SiteConfig { get; }

        // Taxonomy Folders
        Guid SubjectsTaxonomyFolder { get; }
        Guid RegionsTaxonomyFolder { get; }
        Guid TherapyAreasTaxonomyFolder { get; } 

		// Templates
		Guid FolderTemplate { get; }

		Guid TaxonomyRoot { get; }

		Guid SubscriptionPage { get; }
		
		Guid EmailPreferences { get; }
	}
}
