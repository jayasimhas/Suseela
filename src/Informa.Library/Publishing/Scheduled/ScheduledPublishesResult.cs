using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public class ScheduledPublishesResult : IScheduledPublishesResult
	{
		public IEnumerable<IScheduledPublishResult> ScheduledPublishes { get; set; }
	}
}
