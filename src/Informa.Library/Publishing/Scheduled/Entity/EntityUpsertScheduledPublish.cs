using System;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityUpsertScheduledPublish : IUpsertScheduledPublish
	{
		protected readonly IEntityScheduledPublishFactory ScheduledPublishFactory;
		protected readonly IEntityScheduledPublishContextFactory ContextFactory;

		public EntityUpsertScheduledPublish(
			IEntityScheduledPublishFactory scheduledPublishFactory,
			IEntityScheduledPublishContextFactory contextFactory)
		{
			ScheduledPublishFactory = scheduledPublishFactory;
			ContextFactory = contextFactory;
		}

		public void Upsert(IScheduledPublish scheduledPublish)
		{
			using (var context = ContextFactory.Create())
			{
				var record = context.ScheduledPublishes.FirstOrDefault(sp =>
					sp.ItemId == scheduledPublish.ItemId &&
					sp.Language == (scheduledPublish.Language ?? string.Empty) &&
					sp.Version == (scheduledPublish.Version ?? string.Empty) &&
					sp.Type == scheduledPublish.Type
				);

				if (record == null)
				{
					record = ScheduledPublishFactory.Create(scheduledPublish);

					context.ScheduledPublishes.Add(record);
				}
				else
				{
					record.LastUpdated = DateTime.Now;
					record.Published = scheduledPublish.Published;
					record.PublishOn = scheduledPublish.PublishOn;
				}

				context.SaveChanges();
			}
		}
	}
}
