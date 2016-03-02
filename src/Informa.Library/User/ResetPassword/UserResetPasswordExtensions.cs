using System;

namespace Informa.Library.User.ResetPassword
{
	public static class UserResetPasswordExtensions
	{
		public static bool IsValid(this IUserResetPassword source)
		{
			if (source == null)
			{
				return false;
			}

			return source.Expiration > DateTime.Now;
		}
	}
}
