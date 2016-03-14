using System;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class ScheduledPublish : IScheduledPublish
	{
		public Guid Id { get; set; }
		public DateTime Added { get; set; }
		public DateTime LastUpdated { get; set; }
		public Guid ItemId { get; set; }
		public string Language { get; set; }
		public string Version { get; set; }
		public DateTime PublishOn { get; set; }
		public bool Published { get; set; }
		public ScheduledPublishType Type { get; set; }
	}
}
