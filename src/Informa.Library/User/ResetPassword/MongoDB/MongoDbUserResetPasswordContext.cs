using MongoDB.Driver;
using Informa.Library.Threading;

namespace Informa.Library.User.ResetPassword.MongoDB
{
	public class MongoDbUserResetPasswordContext : ThreadSafe<MongoDatabase>, IMongoDbUserResetPasswordContext
	{
		protected readonly IMongoDbUserResetPasswordConfiguration Configuration;

		public MongoDbUserResetPasswordContext(
			IMongoDbUserResetPasswordConfiguration configuration)
		{
			Configuration = configuration;
		}

		public MongoCollection<UserResetPasswordDocument> UserResetPasswords => Database.GetCollection<UserResetPasswordDocument>(Configuration.CollectionName);

		public MongoDatabase Database => SafeObject;

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
