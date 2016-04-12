using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Publishing;
using System.Linq;
using Informa.Library.Publishing.Switcher;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class PublishScheduledPublish : IPublishScheduledPublish
	{
		protected readonly IRetrieveItemToPublish RetrieveItemToPublish;
		protected readonly IScheduledPublishingDatabaseContext DatabaseContext;
		protected readonly IScheduledPublishingTargetsContext PublishingTargetsContext;

		public PublishScheduledPublish(
			IRetrieveItemToPublish retrieveItemToPublish,
			IScheduledPublishingDatabaseContext databaseContext,
			IScheduledPublishingTargetsContext publishingTargetsContext)
		{
			RetrieveItemToPublish = retrieveItemToPublish;
			DatabaseContext = databaseContext;
			PublishingTargetsContext = publishingTargetsContext;
		}

		public IPublishingStatus Publish(IScheduledPublish scheduledPublish)
		{
		    using (new ScheduledPublishContext())
		    {
		        using (new DatabaseSwitcher(DatabaseContext.Database))
		        {
		            var item = RetrieveItemToPublish.Get(scheduledPublish);

		            if (item == null)
		            {
		                return new PublishingStatus
		                {
		                    Status = PublishStatus.Failed
		                };
		            }

		            var languages = string.IsNullOrEmpty(scheduledPublish.Language)
		                ? item.Languages
		                : new Language[] {item.Language};
		            var publishingTargetDatabases = PublishingTargetsContext.Databases.ToArray();
		            var handle = PublishManager.PublishItem(item, publishingTargetDatabases, languages, false, false, true);

					if (handle == null)
					{
						return new PublishingStatus
						{
							Status = PublishStatus.Failed
						};
					}

		            return new PublishingStatus
		            {
		                PublishHandle = handle,
		                Status = PublishStatus.Processing
		            };
		        }
		    }
		}
	}
}
