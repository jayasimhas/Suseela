using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IPublishScheduledPublishes
	{
		IScheduledPublishesResult Publish(IEnumerable<IScheduledPublish> scheduledPublishes);
	}
}
