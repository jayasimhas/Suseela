using System;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityDeleteVersionScheduledPublishes : IDeleteVersionScheduledPublishes
	{
		protected readonly IEntityScheduledPublishContextFactory ContextFactory;

		public EntityDeleteVersionScheduledPublishes(
			IEntityScheduledPublishContextFactory contextFactory)
		{
			ContextFactory = contextFactory;
		}

		public void Delete(Guid itemId, string language, string version)
		{
			using (var context = ContextFactory.Create())
			{
				context.ScheduledPublishes.RemoveRange(context.ScheduledPublishes.Where(sp =>
					sp.ItemId == itemId &&
					string.Equals(sp.Language, language) &&
					string.Equals(sp.Version, version)
				));
				context.SaveChanges();
			}
		}
	}
}
