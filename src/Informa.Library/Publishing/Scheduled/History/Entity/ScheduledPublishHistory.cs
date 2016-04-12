using System;

namespace Informa.Library.Publishing.Scheduled.History.Entity
{
	public class ScheduledPublishHistory : IScheduledPublishHistory
	{
		public Guid Id { get; set; }
		public Guid ItemId { get; set; }
		public string Language { get; set; }
		public string Version { get; set; }
		public DateTime PublishedOn { get; set; }
		public ScheduledPublishType Type { get; set; }
	}
}
