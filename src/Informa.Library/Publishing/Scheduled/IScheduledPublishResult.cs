namespace Informa.Library.Publishing.Scheduled
{
	public interface IScheduledPublishResult
	{
		IScheduledPublish ScheduledPublish { get; }
		IPublishingStatus PublishingStatus { get; }
	}
}
