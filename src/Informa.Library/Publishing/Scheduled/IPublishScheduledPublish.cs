namespace Informa.Library.Publishing.Scheduled
{
	public interface IPublishScheduledPublish
	{
		IPublishProcessStatus Process(IScheduledPublish scheduledPublish);
	}
}
