using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Data.Managers;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class RetrieveItemToPublish : IRetrieveItemToPublish
	{
		protected readonly IScheduledPublishingDatabaseContext DatabaseContext;

		public RetrieveItemToPublish(
			IScheduledPublishingDatabaseContext databaseContext)
		{
			DatabaseContext = databaseContext;
		}

		public Item Get(IScheduledPublish scheduledPublish)
		{
			Item item = null;
			Language language = null;
			Version version = null;
			var id = ID.Parse(scheduledPublish.ItemId);

			if (!string.IsNullOrWhiteSpace(scheduledPublish.Language))
			{
				language = LanguageManager.GetLanguage(scheduledPublish.Language);
			}

			if (!string.IsNullOrWhiteSpace(scheduledPublish.Version))
			{
				version = new Version(scheduledPublish.Version);
			}

			if (language != null && version != null)
			{
				item = DatabaseContext.Database.GetItem(id, language, version);
			}
			else if (language != null)
			{
				item = DatabaseContext.Database.GetItem(id, language);
			}
			else
			{
				item = DatabaseContext.Database.GetItem(id);
			}

			return item;
		}
	}
}
