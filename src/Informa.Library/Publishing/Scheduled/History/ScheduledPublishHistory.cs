using System;

namespace Informa.Library.Publishing.Scheduled.History
{
	public class ScheduledPublishHistory : IScheduledPublishHistory
	{
		public Guid ItemId { get; set; }
		public string Language { get; set; }
		public string Version { get; set; }
		public DateTime PublishedOn { get; set; }
		public ScheduledPublishType Type { get; set; }
	}
}
