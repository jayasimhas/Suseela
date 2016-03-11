using System;

namespace Informa.Library.User.ResetPassword.Entity
{
	public class EntityStoreUserResetPassword : IStoreUserResetPassword
	{
		protected readonly IEntityUserResetPasswordContextFactory UserResetPasswordContext;
		protected readonly IEntityUserResetPasswordFactory UserResetPasswordFactory;

		public EntityStoreUserResetPassword(
			IEntityUserResetPasswordContextFactory userResetPasswordContext,
			IEntityUserResetPasswordFactory userResetPasswordFactory)
		{
			UserResetPasswordContext = userResetPasswordContext;
			UserResetPasswordFactory = userResetPasswordFactory;
		}

		public bool Store(IUserResetPassword userResetPassword)
		{
			using (var context = UserResetPasswordContext.Create())
			{
				var record = UserResetPasswordFactory.Create(userResetPassword);

				context.UserResetPasswords.Add(record);

				try
				{
					context.SaveChanges();

					return true;
				}
				catch (Exception ex)
				{
					return false;
				}
			}
		}
	}
}
