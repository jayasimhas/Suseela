using MongoDB.Driver;

namespace Informa.Library.User.ResetPassword.MongoDB
{
	public interface IMongoDbUserResetPasswordContext
	{
		MongoCollection<UserResetPasswordDocument> UserResetPasswords { get; }
	}
}
