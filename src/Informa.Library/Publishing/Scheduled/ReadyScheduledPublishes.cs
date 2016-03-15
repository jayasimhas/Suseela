using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ReadyScheduledPublishes : IReadyScheduledPublishes
	{
		protected readonly IAllScheduledPublishes AllScheduledPublishes;

		public ReadyScheduledPublishes(
			IAllScheduledPublishes allScheduledPublishes)
		{
			AllScheduledPublishes = allScheduledPublishes;
		}

		public IEnumerable<IScheduledPublish> ScheduledPublishes
		{
			get
			{
				var scheduledPublishes = AllScheduledPublishes.ScheduledPublishes;
				var now = DateTime.Now;

				return scheduledPublishes.Where(sc => !sc.Published && sc.PublishOn <= now).ToList();
			}
		}
	}
}
