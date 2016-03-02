using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbStoreUserResetPassword : IStoreUserResetPassword
	{
		protected readonly IUserResetPasswordDocumentFactory DocumentFatory;
		protected readonly IMongoDbUserResetPasswordContext MongoDbContext;

		public MongoDbStoreUserResetPassword(
			IUserResetPasswordDocumentFactory documentFatory,
			IMongoDbUserResetPasswordContext mongoDbContext)
		{
			DocumentFatory = documentFatory;
			MongoDbContext = mongoDbContext;
		}

		public void Store(IUserResetPassword userResetPassword)
		{
			var document = DocumentFatory.Create(userResetPassword);

			MongoDbContext.UserResetPasswords.Save(document);
		}
	}
}
