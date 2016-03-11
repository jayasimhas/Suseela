using System;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityDeleteItemScheduledPublishes : IDeleteItemScheduledPublishes
	{
		protected readonly IEntityScheduledPublishContextFactory ContextFactory;

		public EntityDeleteItemScheduledPublishes(
			IEntityScheduledPublishContextFactory contextFactory)
		{
			ContextFactory = contextFactory;
		}

		public void Delete(Guid itemId)
		{
			using (var context = ContextFactory.Create())
			{
				context.ScheduledPublishes.RemoveRange(context.ScheduledPublishes.Where(sp => sp.ItemId == itemId));
				context.SaveChanges();
			}
		}
	}
}
