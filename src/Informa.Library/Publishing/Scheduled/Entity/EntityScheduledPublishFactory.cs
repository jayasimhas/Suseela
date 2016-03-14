using System;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityScheduledPublishFactory : IEntityScheduledPublishFactory
	{
		public ScheduledPublish Create(IScheduledPublish scheduledPublish)
		{
			var now = DateTime.Now;

			return new ScheduledPublish
			{
				Id = Guid.NewGuid(),
				Added = now,
				ItemId = scheduledPublish.ItemId,
				Language = scheduledPublish.Language ?? string.Empty,
				Published = scheduledPublish.Published,
				PublishOn = scheduledPublish.PublishOn,
				Version = scheduledPublish.Version ?? string.Empty,
				LastUpdated = now,
				Type = scheduledPublish.Type
			};
		}
	}
}
