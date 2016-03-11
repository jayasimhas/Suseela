using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.Entity
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class EntityUserResetPasswordFactory : IEntityUserResetPasswordFactory
	{
		public UserResetPassword Create(IUserResetPassword userResetPassword)
		{
			return new UserResetPassword
			{
				Expiration = userResetPassword.Expiration,
				Token = userResetPassword.Token,
				Username = userResetPassword.Username
			};
		}
	}
}
