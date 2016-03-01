using MongoDB.Driver.Builders;

namespace Informa.Library.User.ResetPassword.MongoDB
{
	public class MongoDbFindUserResetPassword : IFindUserResetPassword
	{
		protected readonly IMongoDbUserResetPasswordContext MongoDbContext;

		public MongoDbFindUserResetPassword(
			IMongoDbUserResetPasswordContext mongoDbContext)
		{
			MongoDbContext = mongoDbContext;
		}

		public IUserResetPassword Find(string token)
		{
			var query = Query<UserResetPasswordDocument>.Where(urpd => urpd.Token == token);

			return MongoDbContext.UserResetPasswords.FindOne(query);
		}
	}
}
