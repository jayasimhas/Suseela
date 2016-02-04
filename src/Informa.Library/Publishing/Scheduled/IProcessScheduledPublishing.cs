using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IProcessScheduledPublishing
	{
		void Process(IEnumerable<IScheduledPublish> scheduledPublishes);
	}
}
