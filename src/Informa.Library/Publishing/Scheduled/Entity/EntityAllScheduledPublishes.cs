using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityAllScheduledPublishes : IAllScheduledPublishes
	{
		protected readonly IEntityScheduledPublishContextFactory ContextFactory;

		public EntityAllScheduledPublishes(
			IEntityScheduledPublishContextFactory contextFactory)
		{
			ContextFactory = contextFactory;
		}

		public IEnumerable<IScheduledPublish> ScheduledPublishes
		{
			get
			{
				using (var context = ContextFactory.Create())
				{
					return context.ScheduledPublishes.ToList();
				}
			}
		}
	}
}
