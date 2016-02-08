namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public interface IScheduledPublishDocumentFactory
	{
		ScheduledPublishDocument Create(IScheduledPublish scheduledPublish);
	}
}
