using System;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IScheduledPublishingDateTime
	{
		DateTime Now { get; }
	}
}