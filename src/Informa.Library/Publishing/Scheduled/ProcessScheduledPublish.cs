using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Publishing;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ProcessScheduledPublish : IProcessScheduledPublish
	{
		protected readonly IRetrieveItemToPublish RetrieveItemToPublish;
		protected readonly IScheduledPublishingDatabaseContext DatabaseContext;
		protected readonly IScheduledPublishingTargetsContext PublishingTargetsContext;

		public ProcessScheduledPublish(
			IRetrieveItemToPublish retrieveItemToPublish,
			IScheduledPublishingDatabaseContext databaseContext,
			IScheduledPublishingTargetsContext publishingTargetsContext)
		{
			RetrieveItemToPublish = retrieveItemToPublish;
			DatabaseContext = databaseContext;
			PublishingTargetsContext = publishingTargetsContext;
		}

		public IPublishProcessStatus Process(IScheduledPublish scheduledPublish)
		{
			using (new DatabaseSwitcher(DatabaseContext.Database))
			{
				var item = RetrieveItemToPublish.Get(scheduledPublish);

				if (item == null)
				{
					return new PublishProcessStatus
					{
						Status = PublishStatus.Failed
					};
				}

				var languages = string.IsNullOrEmpty(scheduledPublish.Language) ? item.Languages : new Language[] { item.Language };
				var publishingTargetDatabases = PublishingTargetsContext.Databases.ToArray();
				var handle = PublishManager.PublishItem(item, publishingTargetDatabases, languages, false, false, true);

				return new PublishProcessStatus
				{
					PublishHandle = handle,
					Status = PublishStatus.Processing
				};
			}
		}
	}
}
