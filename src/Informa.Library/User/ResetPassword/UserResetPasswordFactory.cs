using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.User.ResetPassword
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserResetPasswordFactory : IUserResetPasswordFactory
	{
		protected readonly IUserResetPasswordTokenFactory TokenFactory;

		public UserResetPasswordFactory(
			IUserResetPasswordTokenFactory tokenFactory)
		{
			TokenFactory = tokenFactory;
		}

		public IUserResetPassword Create(IUser user)
		{
			return new UserResetPassword
			{
				Expiration = DateTime.Now.AddDays(1),
				Token = TokenFactory.Create(),
				Username = user.Username
			};
		}
	}
}
