using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IAllScheduledPublishes
	{
		IEnumerable<IScheduledPublish> ScheduledPublishes { get; }
	}
}
