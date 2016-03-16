using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Publishing.Scheduled.History
{
	[AutowireService(LifetimeScope.Default)]
	public class ScheduledPublishHistoryFactory : IScheduledPublishHistoryFactory
	{
		public IScheduledPublishHistory Create(IScheduledPublish scheduledPublish)
		{
			return new ScheduledPublishHistory
			{
				ItemId = scheduledPublish.ItemId,
				Language = scheduledPublish.Language,
				PublishedOn = scheduledPublish.PublishOn,
				Type = scheduledPublish.Type,
				Version = scheduledPublish.Version
			};
		}
	}
}
