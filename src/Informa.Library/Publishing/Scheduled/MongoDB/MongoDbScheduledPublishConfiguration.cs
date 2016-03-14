using Sitecore.Configuration;
using System.Configuration;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public class MongoDbScheduledPublishConfiguration : IMongoDbScheduledPublishConfiguration
	{
		private const string CollectionNameConfigKey = "MongoDbScheduledPublishConfiguration.CollectionName";
		private const string ConnectionStringConfigKey = "scheduledpublishing";

		public string ConnectionString => ConfigurationManager.ConnectionStrings[ConnectionStringConfigKey].ConnectionString;
		public string CollectionName => Settings.GetSetting(CollectionNameConfigKey);
	}
}
