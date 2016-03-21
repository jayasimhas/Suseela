namespace Informa.Library.Publishing.Scheduled.History.Entity
{
	public interface IEntityScheduledPublishFactory
	{
		ScheduledPublishHistory Create(IScheduledPublishHistory scheduledPublishHistory);
	}
}