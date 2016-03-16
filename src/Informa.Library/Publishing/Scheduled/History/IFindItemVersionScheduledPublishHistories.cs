using System;
using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled.History
{
	public interface IFindItemVersionScheduledPublishHistories
	{
		IEnumerable<IScheduledPublishHistory> Find(Guid itemId, string language, string version);
	}
}
