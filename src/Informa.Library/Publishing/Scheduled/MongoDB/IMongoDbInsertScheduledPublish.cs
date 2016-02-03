namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public interface IMongoDbInsertScheduledPublish
	{
		void Insert(IScheduledPublish scheduledPublish);
	}
}
