using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ReadyScheduledPublishes : IReadyScheduledPublishes
	{
		protected readonly IAllScheduledPublishes AllScheduledPublishes;
		protected readonly IScheduledPublishingDateTime ScheduledPublishingDateTime;

		public ReadyScheduledPublishes(
			IAllScheduledPublishes allScheduledPublishes,
			IScheduledPublishingDateTime scheduledPublishingDateTime)
		{
			AllScheduledPublishes = allScheduledPublishes;
			ScheduledPublishingDateTime = scheduledPublishingDateTime;
		}

		public IEnumerable<IScheduledPublish> ScheduledPublishes
		{
			get
			{
				var scheduledPublishes = AllScheduledPublishes.ScheduledPublishes;

				return scheduledPublishes.Where(sc => !sc.Published && sc.PublishOn <= ScheduledPublishingDateTime.Now).ToList();
			}
		}
	}
}
