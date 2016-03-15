using System;

namespace Informa.Library.Publishing.Scheduled.History
{
	public interface IScheduledPublishHistory
	{
		Guid ItemId { get; }
		string Language { get; }
		string Version { get; }
		DateTime PublishedOn { get; }
		ScheduledPublishType Type { get; }
	}
}
