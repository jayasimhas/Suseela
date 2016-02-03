using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	public class UpdateScheduledPublishes : IUpdateScheduledPublishes
	{
		protected readonly IUpdateScheduledPublish UpdateScheduledPublish;

		public UpdateScheduledPublishes(
			IUpdateScheduledPublish updateScheduledPublish)
		{
			UpdateScheduledPublish = updateScheduledPublish;
		}

		public void Update(IEnumerable<IScheduledPublish> scheduledPublishes)
		{
			scheduledPublishes.ToList().ForEach(sp => UpdateScheduledPublish.Update(sp));
		}
	}
}
