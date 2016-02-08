using Sitecore.Data;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IScheduledPublishingDatabaseContext
	{
		Database Database { get; }
	}
}
