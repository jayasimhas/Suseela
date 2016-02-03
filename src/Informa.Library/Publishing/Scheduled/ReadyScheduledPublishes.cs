using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	public class ReadyScheduledPublishes : IReadyScheduledPublishes
	{
		protected readonly IRetrieveScheduledPublishes RetrieveScheduledPublishes;

		public ReadyScheduledPublishes(
			IRetrieveScheduledPublishes retrieveScheduledPublishes)
		{
			RetrieveScheduledPublishes = retrieveScheduledPublishes;
		}

		public IEnumerable<IScheduledPublish> ScheduledPublishes
		{
			get
			{
				var scheduledPublishes = RetrieveScheduledPublishes.All;
				var now = DateTime.Now;

				return scheduledPublishes.Where(sc => !sc.Published && sc.PublishOn <= now);
			}
		}
	}
}
