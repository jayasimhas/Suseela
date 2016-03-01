using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.User.ResetPassword
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class FindValidUserResetPassword : IFindValidUserResetPassword
	{
		protected readonly IFindUserResetPassword FindUserResetPassword;

		public FindValidUserResetPassword(
			IFindUserResetPassword findUserResetPassword)
		{
			FindUserResetPassword = findUserResetPassword;
		}

		public IUserResetPassword Find(string token)
		{
			var userResetPassword = FindUserResetPassword.Find(token);

			if (userResetPassword == null || userResetPassword.Expiration < DateTime.Now)
			{
				return null;
			}

			return userResetPassword;
		}
	}
}
