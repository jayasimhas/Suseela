using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IProcessScheduledPublishes
	{
		void Process(IEnumerable<IScheduledPublish> scheduledPublishes);
	}
}
