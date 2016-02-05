using System;

namespace Informa.Library.Publishing.Scheduled
{
	public class ScheduledPublish : IScheduledPublish
	{
		public Guid ItemId { get; set; }
		public string Language { get; set; }
		public string Version { get; set; }
		public DateTime PublishOn { get; set; }
		public bool Published { get; set; }
		public ScheduledPublishType Type { get; set; }
	}
}
