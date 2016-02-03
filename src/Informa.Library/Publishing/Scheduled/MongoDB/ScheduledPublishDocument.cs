using System;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public class ScheduledPublishDocument : ScheduledPublish, IScheduledPublish
	{
		public DateTime Added { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}
