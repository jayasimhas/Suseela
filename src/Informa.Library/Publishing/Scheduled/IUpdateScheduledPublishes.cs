using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IUpdateScheduledPublishes
	{
		void Update(IEnumerable<IScheduledPublish> scheduledPublishes);
	}
}
