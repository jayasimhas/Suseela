using Informa.Library.Threading;
using MongoDB.Driver;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public class MongoDbScheduledPublishContext : ThreadSafe<MongoDatabase>, IMongoDbScheduledPublishContext
	{
		public readonly IMongoDbScheduledPublishConfiguration Configuration;

		public MongoDbScheduledPublishContext(
			IMongoDbScheduledPublishConfiguration configuration)
		{
			Configuration = configuration;
		}

		public MongoDatabase Database => SafeObject;

		public MongoCollection<ScheduledPublishDocument> ScheduledPublishes => Database.GetCollection<ScheduledPublishDocument>(Configuration.CollectionName);

		protected override MongoDatabase UnsafeObject
		{
			get
			{
				var url = new MongoUrl(Configuration.ConnectionString);
				var clientSettings = MongoClientSettings.FromUrl(url);
				var client = new MongoClient(clientSettings);
				var server = client.GetServer();

				return server.GetDatabase(url.DatabaseName);
			}
		}
	}
}
