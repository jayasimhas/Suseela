using System;
using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled.History
{
	public interface IFindItemLanguageScheduledPublishHistories
	{
		IEnumerable<IScheduledPublishHistory> Find(Guid itemId, string language);
	}
}
