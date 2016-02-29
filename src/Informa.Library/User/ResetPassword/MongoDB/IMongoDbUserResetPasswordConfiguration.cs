namespace Informa.Library.User.ResetPassword.MongoDB
{
	public interface IMongoDbUserResetPasswordConfiguration
	{
		string ConnectionString { get; }
		string CollectionName { get; }
	}
}
