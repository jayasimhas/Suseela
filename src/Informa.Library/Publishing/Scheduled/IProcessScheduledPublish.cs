namespace Informa.Library.Publishing.Scheduled
{
	public interface IProcessScheduledPublish
	{
		IPublishProcessStatus Process(IScheduledPublish scheduledPublish);
	}
}
