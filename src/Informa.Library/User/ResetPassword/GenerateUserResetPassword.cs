using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class GenerateUserResetPassword : IGenerateUserResetPassword
	{
		protected readonly IUserResetPasswordFactory UserResetPasswordFactory;
		protected readonly IStoreUserResetPassword StoreUserResetPassword;

		public GenerateUserResetPassword(
			IUserResetPasswordFactory userResetPasswordFactory,
			IStoreUserResetPassword storeUserResetPassword)
		{
			UserResetPasswordFactory = userResetPasswordFactory;
			StoreUserResetPassword = storeUserResetPassword;
		}

		public IUserResetPassword Generate(IUser user)
		{
			var userResetPassword = UserResetPasswordFactory.Create(user);

			if (userResetPassword != null)
			{
				StoreUserResetPassword.Store(userResetPassword);
			}

			return userResetPassword;
		}
	}
}
