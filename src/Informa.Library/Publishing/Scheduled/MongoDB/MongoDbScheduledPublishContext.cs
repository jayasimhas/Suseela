using Jabberwocky.Glass.Autofac.Attributes;
using MongoDB.Driver;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbScheduledPublishContext : IMongoDbScheduledPublishContext
	{
		private MongoDatabase database;
		private object databaseLock = new object();

		public MongoDatabase Database
		{
			get
			{
				if (database != null)
				{
					return database;
				}

				lock(databaseLock)
				{
					if (database != null)
					{
						return database;
					}

					// TODO: Read connection details from connection strings config
					var client = new MongoClient(new MongoClientSettings
					{
						Server = new MongoServerAddress("localhost", 27017)
					});

					return database = client.GetServer().GetDatabase("Informa_scheduled_publishing");
				}
			}
		}

		public MongoCollection<ScheduledPublishDocument> ScheduledPublishes => Database.GetCollection<ScheduledPublishDocument>("scheduled_publishes");
	}
}
