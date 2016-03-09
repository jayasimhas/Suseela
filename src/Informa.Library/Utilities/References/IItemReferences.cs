using System;

namespace Informa.Library.Utilities.References
{
	public interface IItemReferences
	{
		Guid HomePage { get; }

		Guid DCDConfigurationItem { get; }

		Guid SiteConfig { get; }

		// Templates
		Guid FolderTemplate { get; }

		Guid TaxonomyRoot { get; }

		Guid SubscriptionPage { get; }
		
		Guid EmailPreferences { get; }
	}
}
