namespace Informa.Library.Publishing.Scheduled.History
{
	public interface IScheduledPublishHistoryFactory
	{
		IScheduledPublishHistory Create(IScheduledPublish scheduledPublish);
	}
}
