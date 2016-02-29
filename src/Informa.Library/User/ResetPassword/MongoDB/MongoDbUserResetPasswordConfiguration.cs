using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Configuration;
using System.Configuration;

namespace Informa.Library.User.ResetPassword.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbUserResetPasswordConfiguration : IMongoDbUserResetPasswordConfiguration
	{
		private const string CollectionNameConfigKey = "MongoDbUserResetPasswordConfiguration.CollectionName";
		private const string ConnectionStringConfigKey = "scheduledpublishing";

		public string ConnectionString => ConfigurationManager.ConnectionStrings[ConnectionStringConfigKey].ConnectionString;
		public string CollectionName => Settings.GetSetting(CollectionNameConfigKey);
	}
}
