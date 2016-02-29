using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserResetPasswordDocumentFactory : IUserResetPasswordDocumentFactory
	{
		public UserResetPasswordDocument Create(IUserResetPassword userResetPassword)
		{
			return new UserResetPasswordDocument
			{
				Expiration = userResetPassword.Expiration,
				Name = userResetPassword.Name,
				Token = userResetPassword.Token,
				Username = userResetPassword.Username
			};
		}
	}
}
