namespace Informa.Library.User.ResetPassword.MongoDB
{
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

		public bool Store(IUserResetPassword userResetPassword)
		{
			var document = DocumentFatory.Create(userResetPassword);
			var result = MongoDbContext.UserResetPasswords.Save(document);

			return true;
		}
	}
}
