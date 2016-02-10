using System;
using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IFindScheduledPublishes
	{
		IEnumerable<IScheduledPublish> Find(Guid itemId, string language, string version);
	}
}
