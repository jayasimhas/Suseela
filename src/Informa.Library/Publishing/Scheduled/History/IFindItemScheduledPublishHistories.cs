using System;
using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled.History
{
	public interface IFindItemScheduledPublishHistories
	{
		IEnumerable<IScheduledPublishHistory> Find(Guid itemId);
	}
}
