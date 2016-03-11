using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityFindScheduledPublishes : IFindScheduledPublishes
	{
		protected readonly IEntityScheduledPublishContextFactory ContextFactory;

		public EntityFindScheduledPublishes(
			IEntityScheduledPublishContextFactory contextFactory)
		{
			ContextFactory = contextFactory;
		}

		public IEnumerable<IScheduledPublish> Find(Guid itemId, string language, string version)
		{
			using (var context = ContextFactory.Create())
			{
				return context.ScheduledPublishes.Where(sp =>
					sp.ItemId == itemId &&
					string.Equals(sp.Language, (language ?? string.Empty)) &&
					string.Equals(sp.Version, (version ?? string.Empty))
				).ToList();
			}
		}
	}
}
