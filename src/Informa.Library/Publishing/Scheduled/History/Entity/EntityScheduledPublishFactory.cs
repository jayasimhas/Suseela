using System;

namespace Informa.Library.Publishing.Scheduled.History.Entity
{
	public class EntityScheduledPublishFactory : IEntityScheduledPublishFactory
	{
		public ScheduledPublishHistory Create(IScheduledPublishHistory scheduledPublishHistory)
		{
			return new ScheduledPublishHistory
			{
				Id = Guid.NewGuid(),
				ItemId = scheduledPublishHistory.ItemId,
				Language = scheduledPublishHistory.Language,
				PublishedOn = scheduledPublishHistory.PublishedOn,
				Type = scheduledPublishHistory.Type,
				Version = scheduledPublishHistory.Version
			};
		}
	}
}
