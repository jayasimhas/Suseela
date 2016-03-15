namespace Informa.Library.Publishing.Scheduled.Entity
{
	public interface IEntityScheduledPublishFactory
	{
		ScheduledPublish Create(IScheduledPublish scheduledPublish);
	}
}