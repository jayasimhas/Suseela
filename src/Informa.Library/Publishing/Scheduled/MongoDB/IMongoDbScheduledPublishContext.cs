using MongoDB.Driver;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public interface IMongoDbScheduledPublishContext
	{
		MongoCollection<ScheduledPublishDocument> ScheduledPublishes { get; }
	}
}
