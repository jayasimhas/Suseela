using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IRetrieveScheduledPublishes
	{
		IEnumerable<IScheduledPublish> All { get; }
	}
}
