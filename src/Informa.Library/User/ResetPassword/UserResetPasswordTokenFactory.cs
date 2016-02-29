using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.User.ResetPassword
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserResetPasswordTokenFactory : IUserResetPasswordTokenFactory
	{
		public string Create()
		{
			return Guid.NewGuid().ToString("N");
		}
	}
}
