using System;

namespace Informa.Library.Utilities.References
{
	public interface IItemReferences
	{
		Guid HomePage { get; }

		Guid DCDConfigurationItem { get; }

		Guid NlmConfiguration { get; }

		Guid NlmErrorDistributionList { get; }

		// Pharma Globals
		Guid NlmCopyrightStatement { get; }
		Guid InformaBar { get; }

		Guid GeneratedDictionary { get; }

		// Taxonomy Folders
		Guid SubjectsTaxonomyFolder { get; }
		Guid RegionsTaxonomyFolder { get; }
		Guid TherapyAreasTaxonomyFolder { get; }

		//Account Drop Down
		Guid AccountCountries { get; }
		Guid AccountJobFunctions { get; }
		Guid AccountJobIndustries { get; }
		Guid AccountNameSuffixes { get; }
		Guid AccountPhoneTypes { get; }
		Guid AccountSalutations { get; }

		// Renderings
		Guid SiteHeaderRendering { get; }
		Guid SiteFooterRendering { get; }
		Guid SiteSideNavigationRendering { get; }
		Guid EmailSearchPopOutRendering { get; }
		Guid SaveSearchPopOutRendering { get; }

		// Templates
		Guid FolderTemplate { get; }

		Guid TaxonomyRoot { get; }

		Guid SubscriptionPage { get; }

		Guid EmailPreferences { get; }
	}
}
