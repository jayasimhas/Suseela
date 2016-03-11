using System.Linq;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityDeleteScheduledPublish : IDeleteScheduledPublish
	{
		protected readonly IEntityScheduledPublishContextFactory ContextFactory;

		public EntityDeleteScheduledPublish(
			IEntityScheduledPublishContextFactory contextFactory)
		{
			ContextFactory = contextFactory;
		}

		public void Delete(IScheduledPublish scheduledPublish)
		{
			using (var context = ContextFactory.Create())
			{
				context.ScheduledPublishes.RemoveRange(context.ScheduledPublishes.Where(sp =>
					sp.ItemId == scheduledPublish.ItemId &&
					string.Equals(sp.Language, (scheduledPublish.Language ?? string.Empty)) &&
					string.Equals(sp.Version, (scheduledPublish.Version ?? string.Empty)) &&
					sp.Type == scheduledPublish.Type
				));
				context.SaveChanges();
			}
		}
	}
}
