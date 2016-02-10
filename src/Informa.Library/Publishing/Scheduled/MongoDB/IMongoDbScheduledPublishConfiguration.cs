namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public interface IMongoDbScheduledPublishConfiguration
	{
		string ConnectionString { get; }
		string CollectionName { get; }
	}
}
