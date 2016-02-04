namespace Informa.Library.Publishing.Scheduled
{
	public interface IPublishScheduledPublish
	{
		IPublishingStatus Publish(IScheduledPublish scheduledPublish);
	}
}
