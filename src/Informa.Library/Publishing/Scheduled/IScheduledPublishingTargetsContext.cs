using Sitecore.Data;
using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IScheduledPublishingTargetsContext
	{
		IEnumerable<Database> Databases { get; }
	}
}
