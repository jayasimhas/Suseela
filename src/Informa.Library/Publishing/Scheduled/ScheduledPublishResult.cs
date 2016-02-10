namespace Informa.Library.Publishing.Scheduled
{
	public class ScheduledPublishResult : IScheduledPublishResult
	{
		public IScheduledPublish ScheduledPublish { get; set; }
		public IPublishingStatus PublishingStatus { get; set; }
	}
}
