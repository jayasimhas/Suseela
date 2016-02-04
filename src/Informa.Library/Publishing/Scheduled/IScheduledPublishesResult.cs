using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IScheduledPublishesResult
	{
		IEnumerable<IScheduledPublishResult> ScheduledPublishes { get; }
	}
}
