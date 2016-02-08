using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IUpsertScheduledPublishes
	{
		void Upsert(IEnumerable<IScheduledPublish> scheduledPublishes);
	}
}
