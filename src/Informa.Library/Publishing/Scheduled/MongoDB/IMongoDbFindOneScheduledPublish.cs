namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public interface IMongoDbFindOneScheduledPublish
	{
		ScheduledPublishDocument Find(IScheduledPublish scheduledPublish);
	}
}
