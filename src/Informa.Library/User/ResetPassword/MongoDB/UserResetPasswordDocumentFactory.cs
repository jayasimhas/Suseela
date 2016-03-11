namespace Informa.Library.User.ResetPassword.MongoDB
{
	public class UserResetPasswordDocumentFactory : IUserResetPasswordDocumentFactory
	{
		public UserResetPasswordDocument Create(IUserResetPassword userResetPassword)
		{
			return new UserResetPasswordDocument
			{
				Expiration = userResetPassword.Expiration,
				Token = userResetPassword.Token,
				Username = userResetPassword.Username
			};
		}
	}
}
