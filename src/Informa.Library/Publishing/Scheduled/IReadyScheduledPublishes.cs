using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IReadyScheduledPublishes
	{
		IEnumerable<IScheduledPublish> ScheduledPublishes { get; }
	}
}
